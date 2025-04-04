using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SSU
{
    internal class Global
    {
        public static IntPtr handle = IntPtr.Zero;
        public static string program_name = "SSU";
        public static string program_directory = AppDomain.CurrentDomain.BaseDirectory;
        public static FileVersionInfo program_version = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
    }
}