using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;

namespace ScreenShotLib
{
    public class ScreenShot_Core
    {
        //public string F_path { get; set; } = Path.Combine(ProgramDirectory, "shortcut.ini");

        private static readonly string ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //Output properties
        public string Save_Suffix { get; set; } = "";
        public string Save_Prefix { get; set; } = "";
        public string Save_Path { get; set; } = @"./";
        public int Index { get; set; } = 0;
        public string Format { get; set; } = "000";

        public void TakeScreenShot(Bitmap bm, Point start, Point end, Size size, bool sfx = false, bool save = true)
        {
            using (Graphics g = Graphics.FromImage(bm))
                g.CopyFromScreen(start, end, size);
            if (save)
                bm.Save(GetSCPath(), ImageFormat.Png);
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

        //Get the final path of the screenshot
        public string GetSCPath()
        {
            return $"{Save_Path}/{Save_Prefix}{Index.ToString(Format)}{Save_Suffix}.png";
        }
    }
}
