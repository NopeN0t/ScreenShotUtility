using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SSU
{
    internal class Global
    {
        public static Process Selected_Process = null;
        public static readonly string Program_name = "SSU";
        public static readonly string Program_directory = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly FileVersionInfo program_version = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
    }
}