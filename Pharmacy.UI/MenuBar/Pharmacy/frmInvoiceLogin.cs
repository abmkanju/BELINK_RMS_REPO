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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmInvoiceLogin : Form
    {
        UnitOfWork unitOfWork;
        public User VerfiedUser;
        public frmInvoiceLogin()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidInput()) return;
                string UserName = txtUserName.Text.Trim();
                string Password = txtPassword.Text;
                VerfiedUser = unitOfWork.UserRepository.IsLoggedIn(UserName, Password);
                if (VerfiedUser == null)
                {
                    ErrMessage.Show();
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Couldn't login you, please try run the application again", ex.Message, "Login Failed");
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
    }
}