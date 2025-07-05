using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SSU.ScreenShotLib
{
    public partial class ScreenShot_Core
    {
        //Constructor
        public ScreenShot_Core(IntPtr Forms_Handle)
        {
            Shortcut_Handle = Forms_Handle;
            try
            {
                if (File.Exists(F_path))
                    load();
                else
                    save();
            }
            catch
            {
                ScreenShot_Events.RaiseWarning(this, "Error Loading Settings");
                save();
            }
            SetIndex();
        }
        //=============================================================
        //Screenshot
        //=============================================================
        public string F_path { get; set; } = Path.Combine(Global.program_directory, "shortcut.ini");
        public string Res_name { get; set; } = "";
        public string Res_prefix { get; set; } = "";
        public string Res_path { get; set; } = @"./";
        public bool Sfx { get; set; } = false;
        public int Index { get; set; } = 0;
        public string Format { get; set; } = "000";
        public Process Select_process { get; set; } = null;

        public void TakeScreenShot(Bitmap bm, Point start, Point end, Size size, bool save = true)
        {
            using (Graphics g = Graphics.FromImage(bm))
                g.CopyFromScreen(start, end, size);
            if (save)
                bm.Save(GetSCPath(), ImageFormat.Png);
            //Notify User (if enable)
            if (Sfx)
            {
                if (File.Exists(Path.Combine(Global.program_directory, "sfx.wav")))
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.SoundLocation = Path.Combine(Global.program_directory, "./sfx.wav");
                    soundPlayer.Play();
                    soundPlayer.Dispose();
                }
                else
                    ScreenShot_Events.RaiseWarning(this, "sfx.wav not found");
            }
        }

        //=============================================================
        //Hotkey
        //=============================================================
        //Default settings win + shift +  a
        public int FsModifier { get; set; } = 12;
        public int Vk { get; set; } = Keys.A.GetHashCode();
        public string Vk_str { get; set; } = "A";
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
        private static extern IntPtr GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        //New key registeration method
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint uiNumDevices, uint cbSize);
        [DllImport("user32.dll")]
        private static extern IntPtr GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);
        private struct RawInputDevice
        {
            public ushort UsagePage;
            public ushort Usage;
            public uint Flags;
            public IntPtr Target;
        }

        public void RegisterRawInput()
        {
            RawInputDevice[] rid = new RawInputDevice[1];
            rid[0].UsagePage = 0x01; // Generic input device
            rid[0].Usage = 0x06; // Keyboard
            rid[0].Flags = 0x00000100; // Always capture input
            rid[0].Target = Shortcut_Handle;

            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(typeof(RawInputDevice))))
            {
                //Fatal error
                int errorCode = Marshal.GetLastWin32Error();
                throw new Exception($"Failed to register raw input device. Error code: {errorCode}");
            }
        }
        public void UnregisterRawInput()
        {
            RawInputDevice[] rid = new RawInputDevice[1];
            rid[0].UsagePage = 0x01; //Generic input device
            rid[0].Usage = 0x06; //it's a keyboard
            rid[0].Flags = 0x00000001; //Unregister
            rid[0].Target = IntPtr.Zero;
            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(typeof(RawInputDevice))))
            {
                //Fatal error
                int errorCode = Marshal.GetLastWin32Error();
                throw new Exception($"Failed to unregister raw input device. Error code: {errorCode}");
            }
        }
        public void HandleWndProc(int limit, int mode)
        {
            /// <summary>
            /// mode 0 = Capture entire screen
            /// mode 1 = Capture selected window
            /// mode 2 = Capture selected area
            /// </summary>
            //Check if limit reached
            if (Index == limit)
            { ScreenShot_Events.RaiseWarning(this, "Limit Reached"); return; }
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
                    GetClientRect(Global.handle, out Rectangle rect);
                    Point topleft = new Point(rect.Left, rect.Top);
                    ClientToScreen(Global.handle, ref topleft);
                    //If you want to include titlebar (approximate)
                    //topleft.Y -= 35;
                    //rect.Height += 35;
                    using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                        TakeScreenShot(bm, topleft, Point.Empty, rect.Size);
                }
                catch { ScreenShot_Events.RaiseWarning(this, "Invalid selection"); return; }
            }
            else if (mode == 2)
            {
                //Todo
            }
            else
                throw new Exception("Invalid mode");
            Index++;
            ScreenshotShortcutTriggered?.Invoke(this, EventArgs.Empty);
        }
        //=============================================================
        //Universal function
        //=============================================================
        public void SetFormat(int count)
        {
            Format = "";
            for (int i = 0; i < count - 1; i++)
                Format += "0";
        }

        public void SetIndex()
        {
            Index = 0;
            while (File.Exists(GetSCPath()))
                Index++;
        }

        //Convert KeyModifier to keyboard keys
        public string GetKeyString()
        {
            int n = FsModifier;
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
            return s += Vk_str;
        }

        //Get the final path of the screenshot
        public string GetSCPath()
        {
            string s = "";
            s += $"{Res_path}/{Res_prefix}{Index.ToString(Format)}{Res_name}.png";
            return s;
        }

        public void save()
        {
            StreamWriter sw = new StreamWriter(F_path, false);
            sw.WriteLine("[infos]");
            sw.WriteLine($"fsModifier = {FsModifier}");
            sw.WriteLine($"vk = {Vk}");
            sw.WriteLine($"vk_str = {Vk_str}");
            sw.WriteLine($"res_path = {Res_path}");
            sw.WriteLine($"res_prefix = {Res_prefix}");
            sw.WriteLine($"sfx = {Sfx}");
            sw.WriteLine($"format = {Format}");
            sw.WriteLine($"file_name = {Res_name}");
            sw.Close();
        }

        public void load()
        {
            var dict = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(F_path);
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                if (string.IsNullOrEmpty(s) || s.StartsWith("["))
                    continue;
                string[] s2 = s.Split('=');
                dict[s2[0].Trim()] = s2[1].Trim();
            }
            sr.Close();
            FsModifier = Convert.ToInt32(dict["fsModifier"]);
            Vk = Convert.ToInt32(dict["vk"]);
            Vk_str = dict["vk_str"];
            Res_path = dict["res_path"];
            Sfx = Convert.ToBoolean(dict["sfx"]);
            Format = dict["format"];
            Res_name = dict["file_name"];
            Res_prefix = dict["res_prefix"];
        }
    }
}
