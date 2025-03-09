using Pharmacy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class ActivationForm : Form
    {
        System.Threading.Thread MainThread;
        public ActivationForm()
        {
            InitializeComponent();
        }

        private void ActivationForm_Load(object sender, EventArgs e)
        {
            this.lblCode.Text += Environment.NewLine + Security.GetSerailNo().ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtKey.Text.Trim()))
            {

                long KeyCode = -1;
                long.TryParse(txtKey.Text.Trim(), out KeyCode);
                if (KeyCode != -1)
                {
                    if (Security.CheckKey(KeyCode))
                    {
                        lblStatus.BackColor = Color.White;
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Application Activated.";
                        Invoke(new Action(() => { this.Close(); }));
                        //frmMain f = new frmMain();
                        //f.Show();             
                        Properties.Settings.Default.Activated = CryptopEngine.Encrypt("True", GlobalSharing.Salt);
                        Properties.Settings.Default.Save();
                        MainThread = new System.Threading.Thread(OpenMainFrom);                    
                        MainThread.SetApartmentState(System.Threading.ApartmentState.STA);
                        MainThread.Start();
                    }
                    else
                    {
                        lblStatus.BackColor = Color.White;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Incorrect Activation Key, Please Try Again.";
                    }
                }
            }
        }

        private void OpenMainFrom()
        {
            Console.WriteLine("Activation Form");
            frmMain f = new frmMain();
            f.WindowState = FormWindowState.Maximized;
            Application.Run(f);
        }
    }
}