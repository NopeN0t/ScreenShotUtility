using System;
using System.Windows.Forms;

namespace SSU
{
    public partial class Change_Key : Form
    {
        private bool setmode = false;
        public Change_Key()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Change_Key_Load(object sender, EventArgs e)
        {
            string[] s = SC.GetKeyString().Split('+');
            foreach (string s2 in s)
            {
                if (s2 == "Win") win_key.Checked = true;
                else if (s2 == "Shift") shift_key.Checked = true;
                else if (s2 == "Alt") alt_key.Checked = true;
                else if (s2 == "Ctrl") ctrl_key.Checked = true;
                else key_box.Text = s2;
            }
        }

        private void Set_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
            setmode = true;
        }

        private void Key_in(object sender, KeyEventArgs e)
        {
            if (setmode)
            {
                if (e.KeyCode != Keys.LWin && !e.Alt && !e.Shift && !e.Control)
                    key_box.Text = e.KeyCode.ToString();
                setmode = false;
                foreach (Control c in this.Controls)
                    c.Enabled = true;
            }
        }

        private void Done_Click(object sender, EventArgs e)
        {
            SC.vk = (int)Enum.Parse(typeof(Keys), key_box.Text);
            SC.vk_str = key_box.Text.ToString();
            int n = 0;
            if (win_key.Checked) n += 8;
            if (shift_key.Checked) n += 4;
            if (ctrl_key.Checked) n += 2;
            if (alt_key.Checked) n += 1;
            SC.fsModifier = n;
            DialogResult = DialogResult.OK;
        }
    }
}
