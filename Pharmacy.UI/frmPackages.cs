using MongoDB.Bson;
using MongoDB.Driver;
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
    public partial class frmPackages : Form
    {
        public frmPackages()
        {
            InitializeComponent();
            /*
            Properties.Settings.Default.IsTrialUsed = false;
            Properties.Settings.Default.Save();*/
            DataTable dt = ProductActivation.get("select xijuekhaumi from HwFileDatas");
            if(dt.Rows.Count > 0)
            {
                if(CryptopEngine.Decrypt(dt.Rows[0][0].ToString() , GlobalSharing.Salt) == "true")
                {
                    chktrail.Visible = false;
                }
                else
                {
                    chktrail.Visible = true;
                }
            }
            else
            {
                chktrail.Visible = true;
            }
            
            if(Properties.Settings.Default.counterdate.ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                Properties.Settings.Default.counterdate = DateTime.Now;
                Properties.Settings.Default.counter++;
            }
            IsKeySentAlready();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radiocustom.Checked)
            {
                numcustomvalue.Visible = true;
                cmbcustomtype.Visible = true;
                cmbcustomtype.SelectedIndex = 2;
            }
            else
            {
                numcustomvalue.Visible = false;
                cmbcustomtype.Visible = false;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chktrail.Checked)
            {
                packagesgroup.Enabled = false;
                networkgroup.Enabled = false;
            }
            else
            {
                packagesgroup.Enabled = true;
                networkgroup.Enabled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.counter < 30)
            {
                if (button1.Text == "Hide This Pop up!" && false)
                {
                    this.Hide();
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
                
            }
        }
            private bool IsKeySentAlready()
        {
            if (Properties.Settings.Default.IsKeySent)
            {
                button2.Text = "Request New Activation Key";
                return true;
            }
            else
            {
                return false;
                button2.Text = "Request Activation Key";
            }
        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chknetworking.Checked)
            {
                numnoofsystems.Visible = true;
                txtipaddress.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
            }
            else
            {
                numnoofsystems.Visible = false;
                txtipaddress.Visible = false; 
                label3.Visible = false;
                label4.Visible = false;
            }
        }
        public string show(string key)
        {
            string k = "";
            for(int i = 5; i<key.Length; i++)
            {
                k += key.Substring(i , 1);
                if (key.Substring(i + 1) == "-")
                {
                    return  CryptopEngine.Decrypt(k , GlobalSharing.Salt);
                }
                
            }
            return CryptopEngine.Decrypt(k, GlobalSharing.Salt);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!chktermsandconditions.Checked)
            {
                MessageBox.Show("You Must Agree to the Terms and Conditions to continue .... ");
                return;
            }
            if(chknetworking.Checked && string.IsNullOrEmpty(txtipaddress.Text))
            {
                MessageBox.Show("Please Enter IP Address of Network System");
                return;
            }
            if(chknetworking.Checked && !string.IsNullOrEmpty(txtipaddress.Text))
            {
                if (!ProductActivation.IsIPAddress(txtipaddress.Text))
                {
                    MessageBox.Show("Invalid IP Address, Please  Enter The Correct IP Address");
                    return;
                }
            }
            this.Cursor = Cursors.WaitCursor;
            int years = 0;
            int months = 0;
            int days = 0;
            bool privat = false;
            int numofsystems = (int)numnoofsystems.Value;
            bool IsNetwork = false;
            if (chktrail.Checked)
            {
                privat = true;
                numofsystems = 0;
                days = 15;
                goto Trial;
            }
            if (chknetworking.Checked)
            {
                IsNetwork = true;
            }
            if (radio1year.Checked)
            {
                years = 1;
                months = 0;
                days = 0;
            }
            if (radio6month.Checked)
            {
                months = 6;
                years = 0;
                days = 0;
            }
            if (radiocustom.Checked)
            {
                if (cmbcustomtype.SelectedIndex == 0)
                {
                    months = 0;
                    years = 0;
                    days = (int)numcustomvalue.Value;
                }
                if (cmbcustomtype.SelectedIndex == 1)
                {
                    months = (int)numcustomvalue.Value;
                    years = 0;
                    days = 0;
                }
                if (cmbcustomtype.SelectedIndex == 2)
                {
                    months = 0;
                    years = (int)numcustomvalue.Value;
                    days = 0;
                }
            }
            string key = ProductActivation.GenerateActivationKey(years.ToString(), months.ToString(), days.ToString());
            /*            string body1 = "<!DOCTYPE html>"+
            "< html >"+
            "< head >"+
            "< style >"+
              "body {"+
                            "font - family: Arial, sans - serif;"+
                            "background - color: #f4f4f4;"+
                "margin: 0;"+
                 "           padding: 0;"+
                  "          line - height: 1.6;"+
                   "     }" +
              ".container {" +
               "             max - width: 600px;" +
                "            margin: 20px auto;" +
                 "           padding: 20px;" +
                  "          background - color: #ffffff;" +
                "border - radius: 5px;" +
                 "           box - shadow: 0px 2px 5px rgba(0, 0, 0, 0.1);" +
                  "      }" +
                   "     h1 {" +
                     "       font - size: 24px;" +
                      "      color: #333;" +
                "text - align: center;" +
                 "           margin - bottom: 20px;" +
                  "      }" +
                   "     h3 {" +
                    "        font - size: 18px;" +
                     "       color: #555;" +
               " margin: 10px 0;" +
                       "}" +
                        "p {"+
                            "font - size: 16px;"+
                            "color: #666;"+
              "}"+
              ".center {"+
                            "text - align: center;"+
             "           }"+
            "</ style >"+
            "</ head >"+
            "< body >"+
            "< div class='container'>"+
              "<h1>Product Activation Request</h1>"+
              "<h3>Profile Information</h3>"+
              "<p><strong>Profile Name:</strong> "+ SharedVariables.AdminPractiseSetting.Name + "</p>"+
              "<p><strong>Profile ID:</strong>" + SharedVariables.AdminPractiseSetting.PracticeId + "</p>" +
              "<p><strong>Store Address:</strong> " + SharedVariables.AdminPractiseSetting.Address + "</p>" +
              "<p><strong>Store Contact #:</strong>" + SharedVariables.AdminPractiseSetting.Phone + "</p>"+

              "<h3>Network Configuration Details</h3>"+
              "<p><strong>Using Network:</strong>"+ IsNetwork + "</p>"+
              "<p><strong>Network System Limits:</strong>"+ numnoofsystems.Value.ToString() + "</p>" +
              "<p><strong>Network IP Address:</strong>" + txtipaddress.Text + "</p>"+

              "<h3>Activation Period</h3>"+
              "<p><strong>Activation Period:</strong>"+ ((years != 0 ? years + "Years," : "") + (months != 0 ? months + "Months and " : "") + (days != 0 ? days + "Days" : "")) + "</p>"+

              "<div class='center'>"+
                "<p><strong>Your network activation key:</strong>"+ key +"</p>"+
              "</div>"+
            "</div>"+
            "</body>"+
            "</html>";*/
            //MessageBox.Show(show(Properties.Settings.Default.ActivationKey));
            //InsertKeyAsync();
            string subject = "Product Activation Request by " + SharedVariables.AdminPractiseSetting.Name;
            string Body = "<h3>Practise Name: " + SharedVariables.AdminPractiseSetting.Name + "<br>" + "Practise ID: " + SharedVariables.AdminPractiseSetting.PracticeId + "<br>" + " Pharmacy Address: " + SharedVariables.AdminPractiseSetting.Address + "<br>" + "Pharmacy Contact# : " + SharedVariables.AdminPractiseSetting.Phone + "</h3>"+ "<br>" + "Using Network : " + IsNetwork + "</h3>" + "<br>" + "Network System Limits : " + numnoofsystems.Value.ToString() + "</h3>" + "<br>" + "Network IP : " + txtipaddress.Text + "</h3>"+ "<br>" + "Activation Period : " + ((years != 0 ? years+ "Years," : "") + (months !=0 ? months+ "Months and " :"") + (days != 0 ? days + "Days" : "")) + "</h3>";
            Body = Body + "<Center><h1>" + key + "</h1></Center>";
            bool mail = ProductActivation.SendEmail(subject, Body);
            if (mail)
            {
                DateTime? actidate = ProductActivation.GetCurrentDateTimeInPakistanTimeZone();
                DateTime? expdate = actidate.Value.AddYears(years).AddMonths(months).AddDays(days);
                Properties.Settings.Default.IsKeySent = true;
                Properties.Settings.Default.ActivationKey = key;
                Properties.Settings.Default.months = months;
                Properties.Settings.Default.years = years;
                Properties.Settings.Default.days = days;
                Properties.Settings.Default.IsNetwork = IsNetwork;
                Properties.Settings.Default.numberofsystems = numofsystems;
                Properties.Settings.Default.Save();
                ProductActivation.WriteStringsToSheet(SharedVariables.AdminPractiseSetting.Name , ProductActivation.getCPUId() , actidate  , expdate , key , years , months , days, IsNetwork ,1 );
                MessageBox.Show("Activation Request Send to BELINK SUPPORT , Please Contact BELINKWIRE TEAM for Activation Key....");
            }
            else
            {
                MessageBox.Show("Connection to The Internet Connection Failed , Please Try Again .... ");
            }
            Trial:
            if (privat)
            {
                Properties.Settings.Default.ActivationKey = "";
                Properties.Settings.Default.IsTrialUsed = true;
                Properties.Settings.Default.Save();
                DateTime dt = ProductActivation.GetCurrentDateTimeInPakistanTimeZone();
                string keyy = ProductActivation.GenerateActivationKey("0", "0", "15");
                ProductActivation.WriteStringsToSheet(SharedVariables.AdminPractiseSetting.Name, ProductActivation.getCPUId(), dt, dt.AddDays(days), keyy, 0, 0, 15, false, 2);
                ProductActivation.ActivateSystem(keyy, dt, dt.AddDays(days), false, "", dt , true);
                MessageBox.Show("15 days Free Trial Started Successfully...");
                privat = false;
                this.Hide();
                
            }
            this.Cursor = Cursors.Default;
        }
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmActivation(this).ShowDialog();
        }

        private void frmPackages_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (ProductActivation.CheckActivation())
            {
                Application.Exit();
            }*/
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
