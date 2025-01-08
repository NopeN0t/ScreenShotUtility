using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SSU
{
    internal class Global
    {
        public static DataTable pr = new DataTable();
        public static Process[] processes;
        public static IntPtr handle = IntPtr.Zero;
        public static void setup_pr()
        {
            pr.Columns.Add("Process Name");
            pr.Columns.Add("Process ID");
        }
        public static void load_pr()
        {
            pr.Rows.Clear();
            processes = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)).ToArray();
            for (int i = 0; i < processes.Length; i++)
                pr.Rows.Add(processes[i].ProcessName, processes[i].MainWindowTitle);
        }
        public static void RefreshGrid(DataGridView input, int header_height = 10, int font_size = 10, string font = "Thaoma", int row_height = 20)
        {
            input.DefaultCellStyle.Font = new Font(font, font_size);
            input.ColumnHeadersHeight = header_height;
            input.ColumnHeadersDefaultCellStyle.Font = new Font(font, font_size);
            for (int i = 0; i < input.Columns.Count; i++)
                input.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i < input.Rows.Count; i++)
                input.Rows[i].Height = row_height;
        }
    }
}