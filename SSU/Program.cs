using System;
using System.Threading;
using System.Windows.Forms;

namespace SSU
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check if another instance is already running
            bool isNewInstance;
            using (Mutex mutex = new Mutex(true, "ScreenShotUtility", out isNewInstance))
            {
                if (!isNewInstance)
                {
                    MessageBox.Show("Another instance of the program is already running.", "Instance Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
        }
    }
}
