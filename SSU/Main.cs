using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        void Register_key(bool reload = false)
        {
            if (reload)
                UnregisterHotKey(this.Handle, 0);
            RegisterHotKey(this.Handle, 0, SC.fsModifier, SC.vk);
        }
        //Shortcut Detection
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312) //0x0312 = 786
            {
                //Check if limit reached
                if ((SC.index).ToString() != SC_cap.Text)
                {
                    //Notify User (if enable)
                    if (SC.sfx)
                    {
                        if (File.Exists("./sfx.wav"))
                        {
                            SoundPlayer soundPlayer = new SoundPlayer();
                            soundPlayer.SoundLocation = "./sfx.wav";
                            soundPlayer.Play();
                            soundPlayer.Dispose();
                        }
                        else
                            MessageBox.Show("sfx.wav not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (!process_mode.Checked)
                    {
                        Rectangle bounds = Screen.GetBounds(Point.Empty);
                        using (Bitmap bm = new Bitmap(bounds.Width, bounds.Height))
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                            bm.Save(SC.GetSCPath(), ImageFormat.Png);
                        }
                    }
                    else
                    {
                        try
                        {
                            Rectangle rect = new Rectangle();
                            GetWindowRect(Global.handle, ref rect);
                            using (Bitmap bm = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y))
                            {
                                using (Graphics g = Graphics.FromImage(bm))
                                    g.CopyFromScreen(rect.Location, Point.Empty, bm.Size);
                                bm.Save(SC.GetSCPath(), ImageFormat.Png);
                            }
                        }
                        catch { MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    SC.index++;
                    Update_preview();
                }
                else
                    MessageBox.Show("Limit Reached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Actual Code Start's here
        public Main()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(SC.f_path))
                    SC.load();
                else
                    SC.save();
            }
            catch { SC.save(); }
            SC.SetIndex();
            Update_preview();
            Key_box.Text = SC.GetKeyString();
            string s = "1";
            for (int i = 0; i < SC.format.Length; i++)
                s += "0";
            SC_cap.Text = s;
            Browse_box.Text = SC.res_path;
            Play_Sound.Checked = SC.sfx;
            Global.setup_pr();
            Register_key();
        }
        void Update_preview()
        {
            SC_Sample.Text = Path.GetFileName(SC.GetSCPath());
            SC_name.Text = SC.res_name;
            //if (SC.index - 1 >= 0)
            //    Index.Text = $"Image Saved\n{SC.index - 1}";
            //else
            Index.Text = $"Image Saved\n{SC.index}";
        }
        private void Form_close(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
            SC.save();
        }

        private void Key_set_Click(object sender, EventArgs e)
        {
            Change_Key ck = new Change_Key();
            if (ck.ShowDialog() == DialogResult.OK)
            {
                Key_box.Text = SC.GetKeyString();
                SC.save();
                Register_key(true);
            }
        }

        private void Browse_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            Browse_box.Text = fbd.SelectedPath;
            SC.SetIndex();
            Update_preview();
        }

        private void Browse_box_TextChanged(object sender, EventArgs e)
        {
            SC.res_path = Browse_box.Text;
            SC.SetIndex();
            Update_preview();
        }

        private void Play_Sound_CheckedChanged(object sender, EventArgs e)
        {
            SC.sfx = Play_Sound.Checked;
        }

        private void SC_name_TextChanged(object sender, EventArgs e)
        {
            SC.res_name = SC_name.Text;
            SC.SetIndex();
            Update_preview();
        }

        private void SC_cap_SelectedIndexChanged(object sender, EventArgs e)
        {
            SC.SetFormat(SC_cap.SelectedItem.ToString().Length);
            SC.SetIndex();
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
            if (sp.ShowDialog() == DialogResult.OK)
                Process_box.Text = SC.Select_process.ProcessName;
        }

        private void SC_prefix_TextChanged(object sender, EventArgs e)
        {
            SC.res_prefix = SC_prefix.Text;
            SC.SetIndex();
            Update_preview();
        }

        private void From_resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyicon_doubleclick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
    }
}
