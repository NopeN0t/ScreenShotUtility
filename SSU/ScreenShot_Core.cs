using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SSU
{
    public class ScreenShot_Core
    {
        //Default settings win + shift +  a
        public int fsModifier { get; set; } = 12;
        public int vk { get; set; } = Keys.A.GetHashCode();
        public string vk_str { get; set; } = "A";
        public string f_path { get; set; } = Path.Combine(Global.program_directory, "shortcut.ini");
        public string res_name { get; set; } = "";
        public string res_prefix { get; set; } = "";
        public string res_path { get; set; } = @"./";
        public bool sfx { get; set; } = true;
        public int index { get; set; } = 0;
        public string format { get; set; } = "000";
        public Process Select_process { get; set; } = null;
        private IntPtr Shortcut_Handle { get; set; }
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
        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WinKey = 8
        }
        public void TakeScreenShot(Bitmap bm, Point start, Point end, Size size, bool save = true)
        {
            using (Graphics g = Graphics.FromImage(bm))
                g.CopyFromScreen(start, end, size);
            if (save)
                bm.Save(GetSCPath(), ImageFormat.Png);
        }
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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
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
