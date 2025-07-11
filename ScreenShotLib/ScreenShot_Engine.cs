using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenShotLib
{
    public class ScreenShot_Engine : Form
    {
        public ScreenShot_Core SC_Core = new ScreenShot_Core();
        public ScreenShot_Engine()
        {
            this.Text = "Raw Input Listener";
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Load += (s, e) => RegisterRawInput();
            this.FormClosing += (s, e) => UnregisterRawInput();
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
        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WinKey = 8
        }
        public IntPtr Select_process { get; set; } = IntPtr.Zero;
        public int FsModifier { get; set; } = 3;
        public int Vk { get; set; } = Keys.A.GetHashCode();
        public int mode { get; set; } = 0; //0 = Capture entire screen, 1 = Capture selected window, 2 = Capture selected area
        public int limit { get; set; } = 100; //Limit for screenshots, default is 100
        public string Vk_str { get; set; } = "A";


        [DllImport("user32.dll")]
        private static extern IntPtr GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint uiNumDevices, uint cbSize);
        [DllImport("user32.dll")]
        private static extern int GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);


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
            // RAWINPUT structure
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
                    // Only process key down events
                    if (raw.keyboard.Message == 0x0100 || raw.keyboard.Message == 0x0104) // WM_KEYDOWN or WM_SYSKEYDOWN
                    {
                        Keys key = (Keys)raw.keyboard.VKey;
                        int modifiers = 0;
                        if ((Control.ModifierKeys & Keys.LWin) == Keys.LWin) modifiers += 8;
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) modifiers += 4;
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control) modifiers += 2;
                        if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt) modifiers += 1;

                        if (key.GetHashCode() == Vk && modifiers == FsModifier)
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
                    if (SC_Core.Index == limit)
                    { ScreenShot_Events.RaiseWarning(this, "Limit Reached"); return; }
                    if (mode == 0)
                    {
                        Rectangle bounds = Screen.GetBounds(Point.Empty);
                        using (Bitmap bm = new Bitmap(bounds.Width, bounds.Height))
                            SC_Core.TakeScreenShot(bm, Point.Empty, Point.Empty, bounds.Size);
                    }
                    else if (mode == 1)
                    {
                        try
                        {
                            GetClientRect(Select_process, out Rectangle rect);
                            Point topleft = new Point(rect.Left, rect.Top);
                            ClientToScreen(Select_process, ref topleft);
                            using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                                SC_Core.TakeScreenShot(bm, topleft, Point.Empty, rect.Size);
                        }
                        catch { ScreenShot_Events.RaiseWarning(this, "Invalid selection"); return; }
                    }
                    else if (mode == 2)
                    {
                        //Todo
                    }
                    else
                        throw new Exception("Invalid mode");
                    SC_Core.Index++;
                    ScreenShot_Events.RaiseShortcutTrigger(this);
                }
            }
            base.WndProc(ref m);
        }
    }
}
