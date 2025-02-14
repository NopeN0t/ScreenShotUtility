using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Policy;
using System.Windows.Forms;

namespace SSU
{
    public class ScreenShot_Core
    {
        //Default settings win + shift +  a
        public int fsModifier = 12;
        public int vk = Keys.A.GetHashCode();
        public string vk_str = "A";
        public string f_path = "./shortcut.ini";
        public string res_name = "";
        public string res_prefix = "";
        public string res_path = "./";
        public bool sfx = true;
        public int index = 0;
        public string format = "000";
        public Process Select_process = null;

        //Constructor
        public ScreenShot_Core()
        {
            try
            {
                if (File.Exists(f_path))
                    load();
                else
                    save();
            }
            catch { save(); }
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
            res_path = dict["res_path"];
            sfx = Convert.ToBoolean(dict["sfx"]);
            format = dict["format"];
            res_name = dict["file_name"];
            res_prefix = dict["res_prefix"];
        }
    }
}
