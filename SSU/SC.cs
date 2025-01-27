using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SSU
{
    internal class SC
    {
        //Default settings win + shift +  a
        public static int fsModifier = 12;
        public static int vk = Keys.A.GetHashCode();
        public static string vk_str = "A";
        public static string f_path = "./shortcut.ini";
        public static string res_name = "sc";
        public static string res_prefix = "";
        public static string res_path = "./";
        public static bool sfx = true;
        public static int index = 0;
        public static string format = "0";
        public static Process Select_process = null;
        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WinKey = 8
        }
        public static void SetFormat(int count)
        {
            format = "";
            for (int i = 0; i < count - 1; i++)
                format += "0";
        }
        public static void SetIndex()
        {
            index = 0;
            while (File.Exists(GetSCPath()))
                index++;
        }
        public static string GetKeyString()
        {
            int n = fsModifier;
            string s = "";
            if (n - 8 >= 0)
            {
                s += "Win+";
                n -= 8;
            }
            if (n - 4 >= 0)
            {
                s += "Shift+";
                n -= 4;
            }
            if (n - 2 >= 0)
            {
                s += "Ctrl+";
                n -= 2;
            }
            if (n - 1 >= 0)
            {
                s += "Alt+";
                n -= 1;
            }
            s += vk_str;
            return s;
        }
        public static string GetSCPath()
        {
            string s = "";
            s += $"{res_path}/{res_prefix}{index.ToString(format)}{res_name}.png";
            return s;
        }
        public static void save()
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
        public static void load()
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
