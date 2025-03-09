using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmActivation : Form
    {
        frmPackages _frm = new frmPackages();
        public frmActivation(frmPackages frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        private UnitOfWork unitOfWork;
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private bool Activate(string key, int years, int months, int days, bool Isnetwork ,  string networkIp)
        {
            bool activated = false;
            DateTime expday = ProductActivation.GetCurrentDateTimeInPakistanTimeZone();
            DateTime expexact = new DateTime();
            if (expday != null)
            {
                expexact = Convert.ToDateTime(expday).AddYears(years).AddMonths(months).AddDays(days);
            }
            else
            {
                //MessageBox.Show("Unable To Connect to  The Internet, Please Try Again");
                return false;
            }
            if (expexact != null)
            {
                activated = true;
                Properties.Settings.Default.ActivationKey = "";
                Properties.Settings.Default.Save();
                ProductActivation.ActivateSystem(key, expday, expday.AddYears(years).AddMonths(months).AddDays(days), Isnetwork , networkIp , expday , false);
            }
            else
            {
                activated = false;
            }
            return activated;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.ActivationKey == "")
            {
                MessageBox.Show("Activation  Key Not Generated for this system, Please Request Activation Key or Contact Healthwire Customer Care.");
                return;
            }
            string Key = Properties.Settings.Default.ActivationKey;
            int months = Properties.Settings.Default.months;
            int days = Properties.Settings.Default.days;
            int years = Properties.Settings.Default.years;
            bool IsNetwork = Properties.Settings.Default.IsNetwork;
            string networkIp = Properties.Settings.Default.networkIp;
            if (Key != null)
            {
                if (Key == txtKey.Text)
                {
                    bool act = Activate(Key, years, months, days, IsNetwork, networkIp);
                    if (act)
                    {
                        
                        MessageBox.Show("Your Product Activated Successfully, Enjoy Using HCloud PMS.");
                        this.Hide();
                        _frm.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Something Went Wrong. Product Activation Aborted,Please Try Again .... ");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Activation Key, Please Try Again..... ");
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsKeySent =false;
            
            Properties.Settings.Default.Save();
            this.Hide();
            new frmPackages().ShowDialog();
        }
        

    }
}
