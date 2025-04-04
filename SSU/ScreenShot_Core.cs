using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SSU
{
    public class ScreenShot_Core
    {
        //Constructor
        public ScreenShot_Core(IntPtr Forms_Handle)
        {
            Shortcut_Handle = Forms_Handle;
            try
            {
                if (File.Exists(f_path))
                    load();
                else
                    save();
            }
            catch
            {
                MessageBox.Show("Error Loading Settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                save();
            }
            SetIndex();
        }
        //=============================================================
        //Screenshot
        //=============================================================
        public string f_path { get; set; } = Path.Combine(Global.program_directory, "shortcut.ini");
        public string res_name { get; set; } = "";
        public string res_prefix { get; set; } = "";
        public string res_path { get; set; } = @"./";
        public bool sfx { get; set; } = false;
        public int index { get; set; } = 0;
        public string format { get; set; } = "000";
        public Process Select_process { get; set; } = null;

        public void TakeScreenShot(Bitmap bm, Point start, Point end, Size size, bool save = true)
        {
            using (Graphics g = Graphics.FromImage(bm))
                g.CopyFromScreen(start, end, size);
            if (save)
                bm.Save(GetSCPath(), ImageFormat.Png);
            //Notify User (if enable)
            if (sfx)
            {
                if (File.Exists(Path.Combine(Global.program_directory, "sfx.wav")))
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.SoundLocation = Path.Combine(Global.program_directory, "./sfx.wav");
                    soundPlayer.Play();
                    soundPlayer.Dispose();
                }
                else
                    MessageBox.Show("sfx.wav not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //=============================================================
        //Hotkey
        //=============================================================
        //Default settings win + shift +  a
        public int fsModifier { get; set; } = 12;
        public int vk { get; set; } = Keys.A.GetHashCode();
        public string vk_str { get; set; } = "A";
        private IntPtr Shortcut_Handle { get; set; }
        public event EventHandler ScreenshotShortcutTriggered;
        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WinKey = 8
        }
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        private static extern IntPtr GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        public void Register_key(bool reload = false)
        {
            if (reload)
                UnregisterHotKey(Shortcut_Handle, 0);
            RegisterHotKey(Shortcut_Handle, 0, fsModifier, vk);
        }

        public void Unregister_key()
        {
            UnregisterHotKey(Shortcut_Handle, 0);
        }

        public void HandleWndProc(int limit, int mode)
        {
            /// <summary>
            /// mode 0 = Capture entire screen
            /// mode 1 = Capture selected window
            /// mode 2 = Capture selected area
            /// </summary>
            //Check if limit reached
            if (index == limit)
            { MessageBox.Show("Limit Reached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (mode == 0)
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bm = new Bitmap(bounds.Width, bounds.Height))
                    TakeScreenShot(bm, Point.Empty, Point.Empty, bounds.Size);
            }
            else if (mode == 1)
            {
                try
                {
                    Rectangle rect;
                    GetClientRect(Global.handle, out rect);
                    Point topleft = new Point(rect.Left, rect.Top);
                    ClientToScreen(Global.handle, ref topleft);
                    //If you want to include titlebar (approximate)
                    //topleft.Y -= 35;
                    //rect.Height += 35;
                    using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                        TakeScreenShot(bm, topleft, Point.Empty, rect.Size);
                }
                catch { MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else if (mode == 2)
            {
                //Todo
            }
            else
                throw new Exception("Invalid mode");
            index++;
            ScreenshotShortcutTriggered?.Invoke(this, EventArgs.Empty);
        }
        //=============================================================
        //Universal function
        //=============================================================
        public void SetFormat(int count)
        {
            format = "";
            for (int i = 0; i < count - 1; i++)
                format += "0";
        }

        public void SetIndex()
        {
            index = 0;
            while (File.Exists(GetSCPath()))
                index++;
        }

        //Convert KeyModifier to keyboard keys
        public string GetKeyString()
        {
            int n = fsModifier;
            var modifiers = new[] { ("Win", 8), ("Shift", 4), ("Ctrl", 2), ("Alt", 1) };
            string s = "";
            foreach (var modifier in modifiers)
            {
                if (n >= modifier.Item2)
                {
                    s += modifier.Item1 + "+";
                    n -= modifier.Item2;
                }
            }
            return s += vk_str;
        }

        //Get the final path of the screenshot
        public string GetSCPath()
        {
            string s = "";
            s += $"{res_path}/{res_prefix}{index.ToString(format)}{res_name}.png";
            return s;
        }

        public void save()
        {
            StreamWriter sw = new StreamWriter(f_path, false);
            sw.WriteLine("[infos]");
            sw.WriteLine($"fsModifier = {fsModifier}");
            sw.WriteLine($"vk = {vk}");
            sw.WriteLine($"vk_str = {vk_str}");
            sw.WriteLine($"res_path = {res_path}");
            sw.WriteLine($"res_prefix = {res_prefix}");
            sw.WriteLine($"sfx = {sfx}");
            sw.WriteLine($"format = {format}");
            sw.WriteLine($"file_name = {res_name}");
            sw.Close();
        }

        public void load()
        {
            var dict = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(f_path);
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                if (string.IsNullOrEmpty(s) || s.StartsWith("["))
                    continue;
                string[] s2 = s.Split('=');
                dict[s2[0].Trim()] = s2[1].Trim();
            }
            sr.Close();
            fsModifier = Convert.ToInt32(dict["fsModifier"]);
            vk = Convert.ToInt32(dict["vk"]);
            vk_str = dict["vk_str"];
            res_path = dict["res_path"];
            sfx = Convert.ToBoolean(dict["sfx"]);
            format = dict["format"];
            res_name = dict["file_name"];
            res_prefix = dict["res_prefix"];
        }
    }
}
