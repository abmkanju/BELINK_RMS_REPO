using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmLogin : Form
    {
        UnitOfWork unitOfWork;
        public frmLogin()
        {
            InitializeComponent();
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
        private void radButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(radTextBox1.Text))
            {
                MessageBox.Show("Username/Email Is Required");
                return;
            }
            if (string.IsNullOrEmpty(radTextBox2.Text))
            {
                MessageBox.Show("Password Is Required");
                return;
            }
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
                    {
                        //if (!IsValidInput()) return;
                        string UserName = radTextBox1.Text.Trim();
                        int Password = Convert.ToInt32(radTextBox2.Text);

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
                            //ErrMessage.Show();
                            return;
                        }

                        //insertLoginHistory();
                        this.Close();
                    }
                    else
                    {
                        //if (!IsValidInput()) return;
                        string UserName = radTextBox1.Text.Trim();
                        string Password = radTextBox2.Text;

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
                            //ErrMessage.Show();
                            return;
                        }

                        //insertLoginHistory();
                        frmMain frmMain = new frmMain();
                        frmMain.Show();
                        this.Close();
                    }
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
            /*DataTable dt = new DataTable();
            if (!SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword) 
            {
                dt = get("select * from Users where (UserName='" + radTextBox1.Text + "' or Email='" + radTextBox1.Text + "') and Password='" + radTextBox2.Text + "';");
            }
            else
            {
                dt = get("select * from Users where (UserName='" + radTextBox1.Text + "' or Email='" + radTextBox1.Text + "') and Password='" + radTextBox2.Text + "' or Pin=" + radTextBox2.Text +";");
            }
            if (dt.Rows.Count > 0)
            {
                frmMain frmMain = new frmMain();
                frmMain.Show();
                this.Hide();
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
            }
            else
            {
                MessageBox.Show("Incorrect Username/Password");
                return;
            }*/
        }
        private Tuple<bool, string, string, int> IsPasswordRemembered()
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
                    return new Tuple<bool, string, string, int>(true, UserName, Password, pin);
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string, string, int>(false, "", "", pin);
                }
            }
            else
            {
                return new Tuple<bool, string, string, int>(false, "", "", pin);
            }
        }
        private void LoadDefaultItems()
        {
            try
            {
                //bool proceedToLoadData = false;
                //using (unitOfWork = new UnitOfWork())
                //{
                //    if (unitOfWork.SupplierRepository.SuppliersExist() && unitOfWork.ItemRspository.ItemsCount() > 1)
                //    {
                //        MessageBox.Show("Data already loaded", "Data exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //    else if (!unitOfWork.SupplierRepository.SuppliersExist() || unitOfWork.ItemRspository.ItemsCount() == 1)
                //    {
                //        proceedToLoadData = true;
                //    }
                //}
                //if (proceedToLoadData)
                //{
                //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Inventory-list.xls");
                //    DataTable dt = ReadExcel(path, ".xls");
                //    loadDefaultItemsData_FromExcelSheet(dt);
                //    //loadInventoryData_FromExcelSheet(dt);
                //}
                using (unitOfWork = new UnitOfWork())
                {
                    bool exists = unitOfWork.ItemRspository.ExistsAny();
                    bool removed = unitOfWork.AdminPractiseSettingRepository.IsUserDeletedDefaultData();

                    if (exists || removed)
                    {
                        return;
                    }
                }
                frmWaiting f = new frmWaiting();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataTable get(string qry)
        {
            try
            {
                SqlConnection con = new SqlConnection(Shared.GetConnectionString());
                con.Open();
                SqlCommand com = new SqlCommand(qry, con);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = com;
                sda.Fill(dt);
                con.Close();
                return dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something went Wrong, Please Try Again");
                
                return null;
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            LoadDefaultItems();
            var Result = IsPasswordRemembered();
            if (Result.Item1)
            {
                radTextBox1.Text = Result.Item2;
                if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
                {
                    radTextBox2.Text = Result.Item4.ToString();
                }
                else
                {
                    radTextBox2.Text = Result.Item3;
                }

                chkRememberMe.Checked = true;
            }
            if (SharedVariables.AdminPharmacySetting.LoginbyPINInsteadoffPassword)
            {
                radTextBox2.NullText = "Enter PIN";
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
