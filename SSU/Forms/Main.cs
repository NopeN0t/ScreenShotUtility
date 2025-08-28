using Microsoft.Win32;
using ScreenShotLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSU
{
    public partial class Main : Form
    {
        //Actual Code Start's here
        readonly ScreenShot_Engine SC_Engine;
        readonly bool isInitilized = false;

        public Main()
        {
            InitializeComponent();

            //Initialize Hotkey
            SC_Engine = new ScreenShot_Engine();
            try { SC_Engine.RegisterRawInput(); }
            catch { MessageBox.Show("Error", "Failed To Register RawInput", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
            ScreenShot_Events.ScreenshotShortcutTriggered += (sender, e) => Update_preview();
            ScreenShot_Events.Warning += (sender, e) => MessageBox.Show(e, "Warning");

            //Initialize UI
            Update_preview();
            notifyIcon.ContextMenuStrip = Icon_Menu;
            Key_box.Text = SC_Engine.GetKeyString();
            string s = "1";
            for (int i = 0; i < SC_Engine.SC_Core.Format.Length; i++)
                s += "0";
            SC_cap.Text = s;
            Browse_box.Text = SC_Engine.SC_Core.Save_Path;
            Play_Sound.Checked = SC_Engine.Sfx;
            //Auto start
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Startup.Checked = rk?.GetValue(Global.Program_name) != null;
                //Check if auto start is valid (program location, version check)
                if (Startup.Checked)
                {
                    //Path check
                    if (rk.GetValue(Global.Program_name).ToString() != Application.ExecutablePath)
                    {
                        rk.DeleteValue(Global.Program_name, false);
                        rk.SetValue(Global.Program_name, Application.ExecutablePath);
                    }
                    //Version check
                    FileVersionInfo version = FileVersionInfo.GetVersionInfo(rk.GetValue(Global.Program_name).ToString());
                    if (Global.program_version.FileVersion.CompareTo(version.FileVersion) > 0)
                    {
                        rk.DeleteValue(Global.Program_name, false);
                        rk.SetValue(Global.Program_name, Application.ExecutablePath);
                    }
                }
            }
            catch (Exception e)
            { MessageBox.Show("Error loading startup settings\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            //Minimize to tray
            Start_minimized.Checked = Properties.Settings.Default.Start_Minimized;
            Task.Run(() => {
                Task.Delay(1).Wait(); //Allow UI to fully load before minimizing
                if (Properties.Settings.Default.Start_Minimized)
                {
                    this.Invoke((Action)(() => {
                        this.WindowState = FormWindowState.Minimized;
                    }));
                }
            });
            isInitilized = true;
        }

        //Update UI
        void Update_preview()
        {
            SC_Engine.SC_Core.SetIndex(); //Force refresh index
            SC_Sample.Text = Path.GetFileName(SC_Engine.SC_Core.GetSCPath());
            SC_suffix.Text = SC_Engine.SC_Core.Save_Suffix;
            SC_prefix.Text = SC_Engine.SC_Core.Save_Prefix;
            Index.Text = $"Image Saved\n{SC_Engine.SC_Core.Index}";
            Icon_Cap.Text = $"{SC_Engine.SC_Core.Index}/{SC_cap.Text} Images";
        }

        //Unload Hotkey and Save settings
        private void Form_close(object sender, FormClosingEventArgs e)
        {
            try { SC_Engine.UnregisterRawInput(); }
            catch { }
            SC_Engine.SaveSettings();
        }

        private void Key_set_Click(object sender, EventArgs e)
        {
            Change_Key ck = new Change_Key { SC_Lib = SC_Engine };
            if (ck.ShowDialog() == DialogResult.OK)
            {
                Key_box.Text = SC_Engine.GetKeyString();
                SC_Engine.SaveSettings();
                try
                {
                    SC_Engine.UnregisterRawInput();
                    SC_Engine.RegisterRawInput();
                }
                catch { MessageBox.Show("Failed To Change Shortcut", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        //Browse save path
        private void Browse_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                SC_Engine.SC_Core.Save_Path = fbd.SelectedPath;
                Browse_box.Text = SC_Engine.SC_Core.Save_Path;
            }
            SC_Engine.SC_Core.SetIndex();
            Update_preview();
        }

        //Update save path
        private void Browse_box_TextChanged(object sender, EventArgs e)
        {
            SC_Engine.SC_Core.Save_Path = Browse_box.Text;
            SC_Engine.SC_Core.SetIndex();
            Update_preview();
        }

        private void Play_Sound_CheckedChanged(object sender, EventArgs e)
        {
            SC_Engine.Sfx = Play_Sound.Checked;
        }

        //Update save name
        private void SC_name_TextChanged(object sender, EventArgs e)
        {
            SC_Engine.SC_Core.Save_Suffix = SC_suffix.Text;
            SC_Engine.SC_Core.SetIndex();
            Update_preview();
        }

        private void SC_cap_SelectedIndexChanged(object sender, EventArgs e)
        {
            SC_Engine.SC_Core.SetFormat(SC_cap.SelectedItem.ToString().Length);
            SC_Engine.SC_Core.SetIndex();
            Update_preview();
        }

        private void Process_mode_CheckedChanged(object sender, EventArgs e)
        {
            Process_box.Enabled = process_mode.Checked;
            process_select.Enabled = process_mode.Checked;
            Process_label.Enabled = process_mode.Checked;
            if (process_mode.Checked)
                SC_Engine.Mode = 1; // Process Mode
            else
                SC_Engine.Mode = 0; // Normal Mode
        }

        private void Process_select_Click(object sender, EventArgs e)
        {
            Select_Process sp = new Select_Process { SC_Lib = SC_Engine };
            if (sp.ShowDialog() == DialogResult.OK)
                Process_box.Text = Global.Selected_Process.ProcessName;
        }

        private void SC_prefix_TextChanged(object sender, EventArgs e)
        {
            SC_Engine.SC_Core.Save_Prefix = SC_prefix.Text;
            SC_Engine.SC_Core.SetIndex();
            Update_preview();
        }

        private void From_resize(object sender, EventArgs e)
        {
            //Minimize to tray
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
        }

        //Show from tray
        private void Notifyicon_doubleclick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Startup_CheckedChanged(object sender, EventArgs e)
        {
            if (isInitilized)
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (Startup.Checked)
                    rk.SetValue(Global.Program_name, Application.ExecutablePath);
                else
                    rk.DeleteValue(Global.Program_name, false);
            }
        }

        private void Start_minimized_CheckedChanged(object sender, EventArgs e)
        {
            if (isInitilized)
            {
                Properties.Settings.Default["Start_Minimized"] = Start_minimized.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void Icon_Show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Icon_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
