using ScreenShotLib;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SSU
{
    public partial class Select_Process : Form
    {   //SC_Core passed from main form
        public ScreenShot_Engine SC_Lib;
        public Select_Process()
        {
            InitializeComponent();
        }
        public DataTable pr = new DataTable();
        public Process[] processes;
        public void Setup_pr()
        {
            pr.Columns.Clear();
            pr.Columns.Add("Process Name");
            pr.Columns.Add("Process ID");
        }
        public void Load_pr()
        {
            pr.Rows.Clear();
            processes = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)).ToArray();
            for (int i = 0; i < processes.Length; i++)
                pr.Rows.Add(processes[i].ProcessName, processes[i].MainWindowTitle);
        }
        public void RefreshGrid(DataGridView input, int header_height = 10, int font_size = 10, string font = "Thaoma", int row_height = 20)
        {
            input.DefaultCellStyle.Font = new Font(font, font_size);
            input.ColumnHeadersHeight = header_height;
            input.ColumnHeadersDefaultCellStyle.Font = new Font(font, font_size);
            for (int i = 0; i < input.Columns.Count; i++)
                input.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i < input.Rows.Count; i++)
                input.Rows[i].Height = row_height;
        }
        private void Select_Process_Load(object sender, EventArgs e)
        {
            Setup_pr();
            Load_pr();
            dataGridView1.DataSource = pr;
            RefreshGrid(dataGridView1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int index = -1;
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
        }

        private void Done_Click(object sender, EventArgs e)
        {
            try
            {
                SC_Lib.SC_Core.Select_process = processes[index].MainWindowHandle;
                Global.Selected_Process = processes[index];
                if (SC_Lib.SC_Core.Select_process == IntPtr.Zero)
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
