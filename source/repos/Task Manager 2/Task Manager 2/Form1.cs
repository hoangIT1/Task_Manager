using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_Manager_2
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            GetAllProcess();
        }
        /// <summary>
        /// Lưu lại danh sách process
        /// </summary>
        Process[] proc;
        /// <summary>
        /// Lấy lên danh sách process và lưu lại đồng thời show lên List View
        /// </summary>
        void GetAllProcess()
        {
            proc = System.Diagnostics.Process.GetProcesses();
            listView.Items.Clear();
            foreach(Process p in proc)
            {
                ListViewItem newproc = new ListViewItem() { Text = p.ProcessName };
                newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = p.PagedMemorySize64.ToString()});
                listView.Items.Add(newproc);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].Kill();
                    GetAllProcess();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer_Click(object sender, EventArgs e)
        {
            float fcpu = pCPU.NextValue();
            float fram = pRAM.NextValue();
            metroProgressBarCPU.Value = (int)fcpu;
            metroProgressBarRAM.Value = (int)fram;
            lblCPU.Text = string.Format("{0:0.00}%", fcpu);
            lblRAM.Text = string.Format("{0:0.00}%", fram);
            chart1.Series["CPU"].Points.AddY(fcpu);
            chart1.Series["RAM"].Points.AddY(fram);
            if (proc.Length != Process.GetProcesses().Length)
            {
                GetAllProcess();
            }
        }

        private void runNewTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Formruntask form = new Formruntask())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    GetAllProcess();
            }
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllProcess();
            timer.Start();
        }

        private void lblRAM_Click(object sender, EventArgs e)
        {

        }
    }
}
