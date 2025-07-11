using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;

namespace ScreenShotLib
{
    public class ScreenShot_Core
    {
        public static readonly string ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //ScreenShot Variables
        [DllImport("user32.dll")]
        private static extern IntPtr GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        public IntPtr Select_process { get; set; } = IntPtr.Zero;

        //Output properties
        public string Save_Suffix { get; set; } = "";
        public string Save_Prefix { get; set; } = "";
        public string Save_Path { get; set; } = @"./";
        public int Index { get; set; } = 0;
        public string Format { get; set; } = "000";

        private void TakeScreenShot(Bitmap bm, Point start, Point end, Size size, bool sfx = false)
        {
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.CopyFromScreen(start, end, size);
                bm.Save(GetSCPath(), ImageFormat.Png);
            }
            ScreenShot_Events.RaiseShortcutTrigger(this);

            //Notify User (if enable)
            if (sfx)
            {
                string soundFile = Path.Combine(ProgramDirectory, "sfx.wav");
                if (File.Exists(soundFile))
                    using (SoundPlayer sp = new SoundPlayer(soundFile))
                        sp.Play();
                else
                    ScreenShot_Events.RaiseWarning(this, "sfx.wav not found");
            }
        }
        public bool CaptureApplication(bool sfx = false)
        {
            try
            {
                GetClientRect(Select_process, out Rectangle rect);
                Point topleft = new Point(rect.Left, rect.Top);
                ClientToScreen(Select_process, ref topleft);
                using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                    TakeScreenShot(bm, topleft, Point.Empty, rect.Size, sfx);
                return true;
            }
            catch { ScreenShot_Events.RaiseWarning(this, "Invalid selection"); return false; }
        }
        public bool CaptureFullScreen(bool sfx = false)
        {
            try
            {
                Rectangle bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
                using (Bitmap bm = new Bitmap(bounds.Width, bounds.Height))
                    TakeScreenShot(bm, Point.Empty, Point.Empty, bounds.Size, sfx);
                return true;
            }
            catch
            {
                ScreenShot_Events.RaiseWarning(this, "Error capturing full screen");
                return false;
            }
        }
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
        public string GetSCPath()
        {
            return $"{Save_Path}/{Save_Prefix}{Index.ToString(Format)}{Save_Suffix}.png";
        }
    }
}
