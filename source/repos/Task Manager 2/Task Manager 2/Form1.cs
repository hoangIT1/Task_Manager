using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Dynamic;

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
        Process[] proc = System.Diagnostics.Process.GetProcesses();
        ServiceController[] services = ServiceController.GetServices();
        Process p = System.Diagnostics.Process.GetCurrentProcess();
        /// <summary>
        /// Lấy lên danh sách process và lưu lại đồng thời show lên List View
        /// </summary>
        public string BytesToReadableValue(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }
        
        void GetAllProcess()
        {

            listView.Items.Clear();
            foreach (Process p in proc)
            {
                ///dynamic extraProcessInfo = GetProcessExtraInformation(p.Id);
                ListViewItem newproc = new ListViewItem() { Text = p.ProcessName };
                newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = p.Id.ToString() });
                newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = BytesToReadableValue(p.PrivateMemorySize64) });
                newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = p.BasePriority.ToString() });
                newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = p.Threads.ToString() });
                ///newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = extraProcessInfo.Username });
                ///newproc.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = extraProcessInfo.Description });
                listView.Items.Add(newproc);
            }
            listView1.Items.Clear();
            foreach (ServiceController service in services)
            {
                
                ListViewItem newser = new ListViewItem() { Text = service.ServiceName };
                newser.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = service.Status.ToString() });
                newser.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = service.StartType.ToString() });
                newser.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = service.CanPauseAndContinue.ToString() });
                
                listView1.Items.Add(newser);
                
            }
        }
/*            
        public ExpandoObject GetProcessExtraInformation(int processId)
        {
            // Query the Win32_Process
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Retrieve username 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                    response.Username = argList[0];

                    // You can return the domain too like (PCDesktop-123123\Username using instead
                    //response.Username = argList[1] + "\\" + argList[0];
                }

                // Retrieve process description if exists
                if (obj["ExecutablePath"] != null)
                {
                    try
                    {
                        FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                        response.Description = info.FileDescription;
                    }
                    catch { }
                }
            }

            return response;
        }
*/
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

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string comname = System.Windows.Forms.SystemInformation.ComputerName;
            string username = System.Windows.Forms.SystemInformation.UserName.ToString();

            textBox1.Text = comname;
            textBox2.Text = username;
        }
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try 
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    services[listView1.SelectedIndices[0]].Start();
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    services[listView1.SelectedIndices[0]].Stop();
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    services[listView1.SelectedIndices[0]].Pause();
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    services[listView1.SelectedIndices[0]].Continue() ;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Delete one function below
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.High;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float fcpu = pCPU.NextValue();
            float fram = pRAM.NextValue();
            metroProgressBarCPU.Value = (int)fcpu;
            metroProgressBarRAM.Value = (int)fram;
            lblCPU.Text = string.Format("{0:0.00}%", fcpu);
            lblRAM.Text = string.Format("{0:0.00}%", fram);
            chart1.Series["CPU"].Points.AddY(fcpu);
            chart1.Series["RAM"].Points.AddY(fram);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void endTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].Kill();
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.High;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void realTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.RealTime;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboveNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.AboveNormal;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.Normal;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count > 0)
                {
                    proc[listView.SelectedIndices[0]].PriorityClass = ProcessPriorityClass.BelowNormal;
                    GetAllProcess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
