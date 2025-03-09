using Newtonsoft.Json;
using Pharmacy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmApiLogin : Form
    {
        public frmApiLogin()
        {
            InitializeComponent();
        }

        private void frmApiLogin_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnLogin });
                string email = Properties.Settings.Default.Email;
                string password = Properties.Settings.Default.Password;
                if (email != "" && password != "")
                {
                    email = CryptopEngine.Decrypt(email, SharedVariables.Salt);
                    password = CryptopEngine.Decrypt(password, SharedVariables.Salt);
                    txtEmail.Text = email;
                    txtPassword.Text = password;
                }
            }
            catch (Exception ex)
            {
                txtEmail.Text = "";
                txtPassword.Text = "";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string email = txtEmail.Text.Trim();
            //    string password = txtPassword.Text.Trim();

            //    Service<string> service = new Service<string>(0);
            //    string jsonLogin = service.Login(email, password);
            //    if (SharedFunctions.IsJson(jsonLogin))
            //    {
            //        Login login = new Login();
            //        login = JsonConvert.DeserializeObject<Login>(jsonLogin);
            //        if (login.status.ToLower().Equals("success"))
            //        {

            //            SharedVariables.API_Email = email;
            //            SharedVariables.API_Token = login.auth_token;
            //            SharedVariables.API_serverPath = ConfigurationManager.ConnectionStrings["Server"].ConnectionString;
            //            SharedVariables.PracticeId = Convert.ToInt32(CryptopEngine.Decrypt(ConfigurationManager.ConnectionStrings["PracticeId"].ConnectionString, SharedVariables.Salt));
            //            if (chkRemember.Checked)
            //            {
            //                Properties.Settings.Default.Email = CryptopEngine.Encrypt(email, SharedVariables.Salt);
            //                Properties.Settings.Default.Password = CryptopEngine.Encrypt(password, SharedVariables.Salt);
            //                Properties.Settings.Default.Save();
            //            }
            //            else
            //            {
            //                Properties.Settings.Default.Email = "";
            //                Properties.Settings.Default.Password = "";
            //                Properties.Settings.Default.Save();
            //            }
            //            MessageBox.Show("User logged in successfully", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information); ;

            //        }
            //        else
            //        {
            //            MessageBox.Show("User login failed, please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
    }
}