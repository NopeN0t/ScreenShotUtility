using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SSU
{
    public partial class Main : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        private static extern IntPtr GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        void Register_key(bool reload = false)
        {
            if (reload)
                UnregisterHotKey(this.Handle, 0);
            RegisterHotKey(this.Handle, 0, SC_Lib.fsModifier, SC_Lib.vk);
        }
        //Shortcut Detection
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312) //0x0312 = 786
            {
                //Check if limit reached
                if ((SC_Lib.index).ToString() == SC_cap.Text)
                { MessageBox.Show("Limit Reached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (!process_mode.Checked)
                {
                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                    using (Bitmap bm = new Bitmap(bounds.Width, bounds.Height))
                        SC_Lib.TakeScreenShot(bm, Point.Empty, Point.Empty, bounds.Size);
                }
                else
                {
                    try
                    {
                        Rectangle rect;
                        GetClientRect(Global.handle, out rect);
                        Point topleft = new Point(rect.Left, rect.Top);
                        ClientToScreen(Global.handle, ref topleft);
                        //If you want to include titlebar (approximate)
                        //topleft.Y -= 35;
                        //rect.Height += 35;
                        using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                            SC_Lib.TakeScreenShot(bm, topleft, Point.Empty, rect.Size);
                    }
                    catch { MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                }
                //Notify User (if enable)
                if (SC_Lib.sfx)
                {
                    if (File.Exists(Path.Combine(Global.program_directory, "sfx.wav")))
                    {
                        SoundPlayer soundPlayer = new SoundPlayer();
                        soundPlayer.SoundLocation = Path.Combine(Global.program_directory, "./sfx.wav");
                        soundPlayer.Play();
                        soundPlayer.Dispose();
                    }
                    else
                        MessageBox.Show("sfx.wav not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                SC_Lib.index++;
                Update_preview();
            }
        }
        //Actual Code Start's here
        ScreenShot_Core SC_Lib = new ScreenShot_Core();
        bool isInitilized = false;
        public Main()
        {
            InitializeComponent();
            //Initialize Hotkey
            Register_key();

            //Initialize UI
            Update_preview();
            notifyIcon.ContextMenuStrip = Icon_Menu;
            Key_box.Text = SC_Lib.GetKeyString();
            string s = "1";
            for (int i = 0; i < SC_Lib.format.Length; i++)
                s += "0";
            SC_cap.Text = s;
            Browse_box.Text = SC_Lib.res_path;
            Play_Sound.Checked = SC_Lib.sfx;
            //Auto start
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            if (rk != null)
                Startup.Checked =  rk.GetValue(Global.program_name) != null;
            else
                Startup.Checked = false;
            Start_minimized.Checked = Properties.Settings.Default.Start_Minimized;
            if (Properties.Settings.Default.Start_Minimized)
                this.WindowState = FormWindowState.Minimized;
            //Minimize to tray based on lunch option
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
            isInitilized = true;
        }

        //Update UI
        void Update_preview()
        {
            SC_Sample.Text = Path.GetFileName(SC_Lib.GetSCPath());
            SC_name.Text = SC_Lib.res_name;
            Index.Text = $"Image Saved\n{SC_Lib.index}";
        }

        //Unload Hotkey and Save settings
        private void Form_close(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
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
                Register_key(true);
            }
        }

        //Browse save path
        private void Browse_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            Browse_box.Text = fbd.SelectedPath;
            SC_Lib.SetIndex();
            Update_preview();
        }

        //Update save path
        private void Browse_box_TextChanged(object sender, EventArgs e)
        {
            SC_Lib.res_path = Browse_box.Text;
            SC_Lib.SetIndex();
            Update_preview();
        }

        private void Play_Sound_CheckedChanged(object sender, EventArgs e)
        {
            SC_Lib.sfx = Play_Sound.Checked;
        }

        //Update save name
        private void SC_name_TextChanged(object sender, EventArgs e)
        {
            SC_Lib.res_name = SC_name.Text;
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
            SC_Lib.res_prefix = SC_prefix.Text;
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
