using System;

namespace ScreenShotLib
{
    public static class ScreenShot_Events
    {
        public static event EventHandler<string> Warning;
        public static event EventHandler<string> Error;
        public static event EventHandler ScreenshotShortcutTriggered;
        public static void RaiseWarning(object sender, string message)
        {
            Warning?.Invoke(sender, message);
        }
        public static void RaiseError(object sender, string message)
        {
            Error?.Invoke(sender, message);
        }
        public static void RaiseShortcutTrigger(object sender)
        {
            ScreenshotShortcutTriggered?.Invoke(sender, EventArgs.Empty);
        }
    }
}
