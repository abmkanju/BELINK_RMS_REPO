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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmReportIssue : Form
    {
        public frmReportIssue()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public bool SendEmail(string subject, string body, string attachmentFilePath)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("pms.reportedissues@gmail.com", "Client Reported Issues");
                    mail.To.Add("abdulbasit.muner@healthwire.pk");
                    mail.To.Add("hassan.rasool@healthwire.pk");
                    
                    mail.Subject = subject;
                    mail.Body = "<h5>" + body + "</h5>";
                    mail.BodyEncoding = Encoding.GetEncoding("utf-8");
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(attachmentFilePath))
                    {
                        Attachment attachment = new Attachment(attachmentFilePath);
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("pms.reportedissues@gmail.com", "jjtdbhukpttkyepu");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckInternetAvailability()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidEmail(string email)
        {
            // Email format regular expression
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a Regex object with the specified pattern
            Regex regex = new Regex(pattern);

            // Check if the email matches the regular expression pattern
            Match match = regex.Match(email);

            // Return true if the email is a valid format, otherwise false
            return match.Success;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RETRY:
            if (string.IsNullOrEmpty(txtpharmacyname.Text))
            {
                MessageBox.Show("Please Enter Pharmacy Name");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtcontactnumber.Text))
            {
                MessageBox.Show("Please Enter Contact Number");
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtemail.Text) && !IsValidEmail(txtemail.Text))
            {
                MessageBox.Show("Invalid Email Address....");
            }
            if (!CheckInternetAvailability())
            {
                this.Cursor = Cursors.Default;
                DialogResult dr = MessageBox.Show("Internet Connection Not  Available, Please Try Again .....", "Error Connecting To the network", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (dr == DialogResult.Ignore)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                else if (dr == DialogResult.Abort)
                {
                    this.Hide();
                }
                else if (dr == DialogResult.Retry)
                {
                    goto RETRY;
                }
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            string subject = "Client Reported PMS Issues";
            StringBuilder body = new StringBuilder();
            body.Append("<!DOCTYPE html>");
            body.Append("<html>");
            body.Append("<head>");
            body.Append("<meta charset=\"UTF-8\">");
            body.Append("<title>Pharmacy Reported Issues</title>");
            body.Append("</head>");
            body.Append("<body>");
            body.Append("<h2>Pharmacy Information</h2>");
            body.Append("<p><b>Pharmacy Name:</b> " + txtpharmacyname.Text + "</p>");
            body.Append("<p><b>Contact Number:</b> " + txtcontactnumber.Text + "</p>");
            body.Append("<p><b>Email:</b> " + txtemail.Text + "</p>");
            body.Append("<hr />");
            body.Append("<h2>Message</h2>");
            body.Append("<p>" + txtmessage.Text + "</p>");
            body.Append("</body>");
            body.Append("</html>");
            string attachement = txtattachement.Text;
            bool status  = SendEmail(subject, body.ToString(), attachement);
            if (status)
            {
                MessageBox.Show("Your Reported Issue Submitted Successfully, Healthwire Customer Representative will contact You soon.", "Submited Request Successfully", MessageBoxButtons.OK , MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                this.Hide();
            }
            else
            {
                this.Cursor = Cursors.Default;
                DialogResult dr = MessageBox.Show("Something Went Wrong, Please Try Again .....", "Error While Submitted Your Request", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); 
                if(dr == DialogResult.Ignore)
                {
                    this.Cursor = Cursors.Default;
                }
                else if(dr == DialogResult.Abort)
                {
                    this.Hide();
                }
                else if(dr == DialogResult.Retry)
                {
                    goto RETRY;
                }
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Select an Image";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                txtattachement.Text = imagePath;
            }
        }
    }
}
