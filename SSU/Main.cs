using Microsoft.Win32;
using SSU.ScreenShotLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SSU
{
    public partial class Main : Form
    {
        //Shortcut Detection
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312) //0x0312 = 786
            {
                int mode;
                if (!process_mode.Checked)
                    mode = 0; //0 capture entire screen
                else
                    mode = 1; //1 capture selected window
                //to add 2 capture selected area
                SC_Lib.HandleWndProc(Convert.ToInt32(SC_cap.Text), mode);

            }
        }
        //Actual Code Start's here
        ScreenShot_Core SC_Lib;
        bool isInitilized = false;

        public Main()
        {
            InitializeComponent();
            //Initialize Hotkey
            SC_Lib = new ScreenShot_Core(this.Handle);
            SC_Lib.RegisterRawInput();
            ScreenShot_Events.ScreenshotShortcutTriggered += (sender, e) => Update_preview();
            ScreenShot_Events.Warning += (sender, e) => MessageBox.Show(e, "Warning");

            //Initialize UI
            Update_preview();
            notifyIcon.ContextMenuStrip = Icon_Menu;
            Key_box.Text = SC_Lib.GetKeyString();
            string s = "1";
            for (int i = 0; i < SC_Lib.Format.Length; i++)
                s += "0";
            SC_cap.Text = s;
            Browse_box.Text = SC_Lib.Res_path;
            Play_Sound.Checked = SC_Lib.Sfx;
            //Auto start
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Startup.Checked = rk?.GetValue(Global.program_name) != null;
                //Check if auto start is valid (program location, version check)
                if (Startup.Checked)
                {
                    //Path check
                    if (rk.GetValue(Global.program_name).ToString() != Application.ExecutablePath)
                    {
                        rk.DeleteValue(Global.program_name, false);
                        rk.SetValue(Global.program_name, Application.ExecutablePath);
                    }
                    //Version check
                    FileVersionInfo version = FileVersionInfo.GetVersionInfo(rk.GetValue(Global.program_name).ToString());
                    if (Global.program_version.FileVersion.CompareTo(version.FileVersion) > 0)
                    {
                        rk.DeleteValue(Global.program_name, false);
                        rk.SetValue(Global.program_name, Application.ExecutablePath);
                    }
                }
            }
            catch (Exception e)
            { MessageBox.Show("Error loading startup settings\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            //Minimize to tray
            Start_minimized.Checked = Properties.Settings.Default.Start_Minimized;
            if (Properties.Settings.Default.Start_Minimized)
                this.WindowState = FormWindowState.Minimized;
            From_resize(this, null); //Trigger minimize event
            isInitilized = true;
        }

        //Update UI
        void Update_preview()
        {
            SC_Sample.Text = Path.GetFileName(SC_Lib.GetSCPath());
            SC_name.Text = SC_Lib.Res_name;
            Index.Text = $"Image Saved\n{SC_Lib.Index}";
            Icon_Cap.Text = $"{SC_Lib.Index}/{SC_cap.Text} Images";
        }

        //Unload Hotkey and Save settings
        private void Form_close(object sender, FormClosingEventArgs e)
        {
            SC_Lib.UnregisterRawInput();
            SC_Lib.save();
        }

        private void Key_set_Click(object sender, EventArgs e)
        {
            Change_Key ck = new Change_Key();
            ck.SC_Lib = SC_Lib;
            if (ck.ShowDialog() == DialogResult.OK)
            {
                Key_box.Text = SC_Lib.GetKeyString();
                SC_Lib.save();
                SC_Lib.UnregisterRawInput();
                SC_Lib.RegisterRawInput();
            }
        }

        //Browse save path
        private void Browse_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                SC_Lib.Res_path = fbd.SelectedPath;
                Browse_box.Text = SC_Lib.Res_path;
            }
            SC_Lib.SetIndex();
            Update_preview();
        }

        //Update save path
        private void Browse_box_TextChanged(object sender, EventArgs e)
        {
            SC_Lib.Res_path = Browse_box.Text;
            SC_Lib.SetIndex();
            Update_preview();
        }

        private void Play_Sound_CheckedChanged(object sender, EventArgs e)
        {
            SC_Lib.Sfx = Play_Sound.Checked;
        }

        //Update save name
        private void SC_name_TextChanged(object sender, EventArgs e)
        {
            SC_Lib.Res_name = SC_name.Text;
            SC_Lib.SetIndex();
            Update_preview();
        }

        private void SC_cap_SelectedIndexChanged(object sender, EventArgs e)
        {
            SC_Lib.SetFormat(SC_cap.SelectedItem.ToString().Length);
            SC_Lib.SetIndex();
            Update_preview();
        }

        private void process_mode_CheckedChanged(object sender, EventArgs e)
        {
            Process_box.Enabled = process_mode.Checked;
            process_select.Enabled = process_mode.Checked;
            Process_label.Enabled = process_mode.Checked;
        }

        private void process_select_Click(object sender, EventArgs e)
        {
            Select_Process sp = new Select_Process();
            sp.SC_Lib = SC_Lib;
            if (sp.ShowDialog() == DialogResult.OK)
                Process_box.Text = SC_Lib.Select_process.ProcessName;
        }

        private void SC_prefix_TextChanged(object sender, EventArgs e)
        {
            SC_Lib.Res_prefix = SC_prefix.Text;
            SC_Lib.SetIndex();
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
        private void notifyicon_doubleclick(object sender, MouseEventArgs e)
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
                    rk.SetValue(Global.program_name, Application.ExecutablePath);
                else
                    rk.DeleteValue(Global.program_name, false);
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
