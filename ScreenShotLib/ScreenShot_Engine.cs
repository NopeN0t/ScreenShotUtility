using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenShotLib
{
    public class ScreenShot_Engine : Form
    {
        public ScreenShot_Core SC_Core = new ScreenShot_Core();
        public ScreenShot_Engine()
        {
            try
            {
                if (File.Exists(F_path))
                    LoadSettings();
                else
                    SaveSettings();
            }
            catch
            {
                ScreenShot_Events.RaiseWarning(this, "Error Loading Settings");
                SaveSettings();
            }
            SC_Core.SetIndex();
            this.Text = "Raw Input Listener";
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        struct RawInputDevice
        {
            public ushort UsagePage;
            public ushort Usage;
            public uint Flags;
            public IntPtr Target;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUTHEADER
        {
            public uint dwType;
            public uint dwSize;
            public IntPtr hDevice;
            public IntPtr wParam;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public ushort Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUT
        {
            public RAWINPUTHEADER header;
            public RAWKEYBOARD keyboard;
        }

        //Default settings ctrl + alt +  a
        //Old Keymodifer is there in case of fallback
        public string F_path { get; set; } = Path.Combine(ScreenShot_Core.ProgramDirectory, "shortcut.ini");
        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WinKey = 8
        }
        public int FsModifier { get; set; } = 3;
        public int Vk { get; set; } = Keys.A.GetHashCode();
        public int Mode { get; set; } = 0; //0 = Capture entire screen, 1 = Capture selected window, 2 = Capture selected area
        public int Limit { get; set; } = 100; //Limit for screenshots, default is 100
        public bool Sfx { get; set; } = true; //Enable sound effect on screenshot
        public string Vk_str { get; set; } = "A";


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint uiNumDevices, uint cbSize);
        [DllImport("user32.dll")]
        private static extern int GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

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
        public void RegisterRawInput()
        {
            RawInputDevice[] rid = new RawInputDevice[1];
            rid[0].UsagePage = 0x01; // Generic input device
            rid[0].Usage = 0x06; // Keyboard
            rid[0].Flags = 0x00000100; // Always capture input
            rid[0].Target = this.Handle;

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
        private bool ProcessRawInput(IntPtr lParam)
        {
            const int RID_INPUT = 0x10000003;
            uint dwSize = 0;
            GetRawInputData(lParam, RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));
            if (dwSize == 0) return false;

            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
            try
            {
                if (GetRawInputData(lParam, RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
                    return false;

                RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                if (raw.header.dwType == 1) // Keyboard
                {
                    if (raw.keyboard.Message == 0x0100 || raw.keyboard.Message == 0x0104) // WM_KEYDOWN or WM_SYSKEYDOWN
                    {
                        Keys key = (Keys)raw.keyboard.VKey;

                        int modifiers = 0;
                        if ((GetAsyncKeyState((int)Keys.LWin) & 0x8000) != 0 || (GetAsyncKeyState((int)Keys.RWin) & 0x8000) != 0)
                            modifiers += 8;
                        if ((GetAsyncKeyState((int)Keys.ShiftKey) & 0x8000) != 0)
                            modifiers += 4;
                        if ((GetAsyncKeyState((int)Keys.ControlKey) & 0x8000) != 0)
                            modifiers += 2;
                        if ((GetAsyncKeyState((int)Keys.Menu) & 0x8000) != 0) // Alt key
                            modifiers += 1;

                        if ((int)key == Vk && modifiers == FsModifier)
                        {
                            return true;
                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return false;
        }
        protected override void WndProc(ref Message m)
        {
            //0x0312 = 786 (RegisterHotKey Method)
            if (m.Msg == 0x00FF) //RawInput Method
            {
                if (ProcessRawInput(m.LParam))
                {
                    /// <summary>
                    /// mode 0 = Capture entire screen
                    /// mode 1 = Capture selected window
                    /// mode 2 = Capture selected area
                    /// </summary>
                    //Check if limit reached
                    if (SC_Core.Index == Limit)
                    { ScreenShot_Events.RaiseWarning(this, "Limit Reached"); return; }
                    if (Mode == 0)
                    {
                        if(!SC_Core.CaptureFullScreen(Sfx))
                            return;
                    }
                    else if (Mode == 1)
                    {
                        if (!SC_Core.CaptureApplication(Sfx))
                            return;
                    }
                    else if (Mode == 2)
                    { }
                    else
                        throw new Exception("Invalid mode");
                    SC_Core.Index++;
                }
            }
            base.WndProc(ref m);
        }
        public void SaveSettings()
        {
            StreamWriter sw = new StreamWriter(F_path, false);
            sw.WriteLine("[infos]");
            sw.WriteLine($"fsModifier = {FsModifier}");
            sw.WriteLine($"vk = {Vk}");
            sw.WriteLine($"vk_str = {Vk_str}");
            sw.WriteLine($"res_path = {SC_Core.Save_Path}");
            sw.WriteLine($"res_prefix = {SC_Core.Save_Prefix}");
            sw.WriteLine($"sfx = {Sfx}");
            sw.WriteLine($"format = {SC_Core.Format}");
            sw.WriteLine($"file_name = {SC_Core.Save_Suffix}");
            sw.Close();
        }
        public void LoadSettings()
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
            SC_Core.Save_Path = dict["res_path"];
            Sfx = Convert.ToBoolean(dict["sfx"]);
            SC_Core.Format = dict["format"];
            SC_Core.Save_Suffix = dict["file_name"];
            SC_Core.Save_Prefix = dict["res_prefix"];
        }
    }
}
