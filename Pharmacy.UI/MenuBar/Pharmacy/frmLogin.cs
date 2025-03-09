using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using System.Data.Entity;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.MenuBar.UserMenue;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmLogin : Form
    {
        UnitOfWork unitOfWork;
        public frmLogin()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private Tuple<bool, string, string , int> IsPasswordRemembered()
        {
            bool Rem = Properties.Settings.Default.IsSavePassword;
            string userName = Properties.Settings.Default.UserName;
            string pswd = Properties.Settings.Default.Password;
            int pin = Properties.Settings.Default.PIN;
            if (Rem && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pswd))
            {
                string UserName = "";
                string Password = "";
                try
                {
                    UserName = CryptopEngine.Decrypt(Properties.Settings.Default.UserName, GlobalSharing.Salt);
                    Password = CryptopEngine.Decrypt(Properties.Settings.Default.Password, GlobalSharing.Salt);
                    return new Tuple<bool, string, string , int>(true, UserName, Password ,  pin);
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string, string , int>(false, "", "" , pin);
                }
            }
            else
            {
                return new Tuple<bool, string, string,int>(false, "", "", pin);
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
                {
                    if (!IsValidInput()) return;
                    string UserName = txtUserName.Text.Trim();
                    int Password = Convert.ToInt32(txtPassword.Text);

                    SharedVariables.LoggedInUser = unitOfWork.UserRepository.IsLoggedInbyPIN(UserName, Password);
                    if (SharedVariables.LoggedInUser != null && SharedVariables.LoggedInUser.Shift != null && SharedVariables.LoggedInUser.Shift.IsActive)
                    {
                        if (!TimeExist(SharedVariables.LoggedInUser))
                        {
                            SharedFunctions.MessageBoxShow("Current user login shift time out", MessageType.Info);
                            return;
                        }
                    }
                    if (chkRememberMe.Checked && SharedVariables.LoggedInUser != null)
                    {
                        Properties.Settings.Default.IsSavePassword = true;
                        Properties.Settings.Default.UserName = CryptopEngine.Encrypt(SharedVariables.LoggedInUser.Email, GlobalSharing.Salt);
                        Properties.Settings.Default.PIN = SharedVariables.LoggedInUser.Pin;
                    }
                    else
                    {
                        Properties.Settings.Default.IsSavePassword = false;
                    }
                    Properties.Settings.Default.Save();
                    if (SharedVariables.LoggedInUser == null)
                    {
                        ErrMessage.Show();
                        return;
                    }

                    insertLoginHistory();
                    this.Close();
                }
                else
                {
                    if (!IsValidInput()) return;
                    string UserName = txtUserName.Text.Trim();
                    string Password = txtPassword.Text;

                    SharedVariables.LoggedInUser = unitOfWork.UserRepository.IsLoggedIn(UserName, Password);
                    if (SharedVariables.LoggedInUser != null && SharedVariables.LoggedInUser.Shift != null && SharedVariables.LoggedInUser.Shift.IsActive)
                    {
                        if (!TimeExist(SharedVariables.LoggedInUser))
                        {
                            SharedFunctions.MessageBoxShow("Current user login shift time out", MessageType.Info);
                            return;
                        }
                    }
                    if (chkRememberMe.Checked && SharedVariables.LoggedInUser != null)
                    {
                        Properties.Settings.Default.IsSavePassword = true;
                        Properties.Settings.Default.UserName = CryptopEngine.Encrypt(SharedVariables.LoggedInUser.Email, GlobalSharing.Salt);
                        Properties.Settings.Default.Password = CryptopEngine.Encrypt(SharedVariables.LoggedInUser.Password, GlobalSharing.Salt);
                    }
                    else
                    {
                        Properties.Settings.Default.IsSavePassword = false;
                    }
                    Properties.Settings.Default.Save();
                    if (SharedVariables.LoggedInUser == null)
                    {
                        ErrMessage.Show();
                        return;
                    }

                    insertLoginHistory();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Input string was not in a correct format.")
                {

                    SharedFunctions.ShowErrorMessage("Enter 4 digit Numeric PIN Instead of Password", "", "Login Failed");
                    return;
                }
                SharedFunctions.ShowErrorMessage("Couldn't login you, please try run the application again", ex.Message, "Login Failed");
            }
        }

        private bool TimeExist(User user)
        {
            bool IsTimeExit = false;
            try
            {
                // convert datetime to a TimeSpan
                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan startTime = user.Shift.StartTime.TimeOfDay;
                TimeSpan endTime = user.Shift.EndTime.TimeOfDay;
                if ((now > startTime) && (now < endTime))
                {
                    //match found
                    return IsTimeExit = true;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return IsTimeExit;
        }
        private void insertLoginHistory()
        {
            try
            {
                //AuditLogTask.CreateAuditLog(unitOfWork, txtUserName.Text.Trim(), AuditModule.User, AuditAction.logged_in, unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId));
                LoginHistory h = new LoginHistory();
                h.UserId = SharedVariables.LoggedInUser.UserId;
                h.CreatedAt = DateTime.Now;
                unitOfWork.LoginHistoryRepository.Insert(h);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                ErrUserName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrUserName.Visible = false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errPswd.Visible = true;
                ErrFound = true;
            }
            else
            {
                errPswd.Visible = false;
            }
            return !ErrFound;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SharedVariables.LoggedInUser = null;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            var Result = IsPasswordRemembered();
            if (Result.Item1)
            {
                txtUserName.Text = Result.Item2;
                if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
                {
                    txtPassword.Text = Result.Item4.ToString();
                }
                else
                {
                    txtPassword.Text = Result.Item3;
                }
                
                chkRememberMe.Checked = true;
            }
            if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
            {
                label2.Text = "PIN";
            }
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRestoreData frm = new frmRestoreData();
            frm.ShowDialog();
        }

        private void ErrMessage_Click(object sender, EventArgs e)
        {

        }
    }
}