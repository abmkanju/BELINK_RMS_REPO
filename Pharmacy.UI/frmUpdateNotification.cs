using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmUpdateNotification : Form
    {
        public frmUpdateNotification()
        {
            InitializeComponent();
            
            linkLabel1.Text = "Latest Update Available to Download, Please Click on Check for Update Button to Update the Application.";
        }
        private void checkforupdate()
        {
            string ex1 = "C:\\";
            string ex2 = Application.StartupPath + "\\";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex2 + "PMSUpdater.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void radRepeatButton1_Click(object sender, EventArgs e)
        {
            checkforupdate();
        }

        private void radRepeatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
