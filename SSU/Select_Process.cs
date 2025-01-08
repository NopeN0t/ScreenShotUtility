using System;
using System.Windows.Forms;

namespace SSU
{
    public partial class Select_Process : Form
    {
        public Select_Process()
        {
            InitializeComponent();
        }

        private void Select_Process_Load(object sender, EventArgs e)
        {
            Global.load_pr();
            dataGridView1.DataSource = Global.pr;
            Global.RefreshGrid(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int index = -1;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
        }

        private void Done_Click(object sender, EventArgs e)
        {
            try
            {
                SC.Select_process = Global.processes[index];
                Global.handle = SC.Select_process.MainWindowHandle;
                if (Global.handle == IntPtr.Zero)
                    MessageBox.Show("Window Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("Invaid Selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
