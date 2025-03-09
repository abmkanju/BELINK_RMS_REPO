using Newtonsoft.Json;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddUser : Form
    {
        UnitOfWork unitOfwork;
        int UserId = 0;
        int RoleId = 0;
        int UserPIN = 0;
        //private void Password = "";
        public frmAddUser()
        {
            InitializeComponent();
        }

        public frmAddUser(int UserId, int RoleId)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.RoleId = RoleId;
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            try
            {
                if (SharedVariables.AdminShiftMasterSetting.ShiftsEnabled)
                {
                    lblShift.Visible = true;
                    cmbShifts.Visible = true;
                    LoadShifts();
                }
                LoadUserRolesAndRight();
                ShowSpecialPermissions();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadShifts()
        {
            try
            {
                List<AdminShiftSetting> shifts = new List<AdminShiftSetting>();
                using (unitOfwork = new UnitOfWork())
                {
                    shifts = unitOfwork.AdminShiftsSettingRepository.GetActiveShifts();
                }
                AdminShiftSetting select = new AdminShiftSetting
                {
                    AdminShiftSettingId = 0,
                    Name = "Select Shift"
                };
                //if (this.RoleId == 1)
                {
                    shifts.Insert(0, select);
                }
                cmbShifts.DataSource = shifts;
                cmbShifts.ValueMember = "AdminShiftSettingId";
                cmbShifts.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load shifts data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadUser(User LoadedUser)
        {

            this.btnAdd.Text = "Update";
            //cmbUserType.SelectedValue = LoadedUser.UserRole.UserRoleId;
            txtName.Text = LoadedUser.UserName;
            txtEmail.Text = LoadedUser.Email;
            txtPhone.Text = LoadedUser.Phone;
            UserPIN = LoadedUser.Pin;
            txtpin.Text = LoadedUser.Pin.ToString();
            txtPassword.Text = LoadedUser.Password;
            if (SharedVariables.AdminShiftMasterSetting.ShiftsEnabled)
            {
                cmbShifts.SelectedValue = LoadedUser.ShiftId ?? 0;
            }
            foreach (UserRole r in LoadedUser.UserRoles)
            {
                foreach (CheckBox cb in pnlRoles.Controls)
                {
                    int roleId = Convert.ToInt32(cb.Tag);
                    if (r.UserRoleId == roleId)
                    {
                        cb.Checked = true;
                        break;
                    }
                }
            }
            // For Checked Select All Check Box
            var CountcheckedRights = 0;
            foreach (Right r in LoadedUser.Rights)
            {
                foreach (CheckBox cb in pnlRights.Controls)
                {
                    int RightId = Convert.ToInt32(cb.Tag);
                    if (r.RightId == RightId)
                    {
                        cb.Checked = true;
                        CountcheckedRights++;
                        break;
                    }
                }
            }
            if(pnlRights.Controls.Count == CountcheckedRights)
            {
                SelectcAllheckBox.Checked = true;
            }
            //Check Adjust Stock
            //if (!LoadedUser.Rights.Any(x=>x.Description == "Adjust Stock"))
            //{
            //    pnlRights.Controls.OfType<CheckBox>().Where(x => x.Text == "Adjust Stock").FirstOrDefault().Checked = false;
            //}

            chkCanGiveDiscount.Checked = LoadedUser.CanGiveDiscount;
            numDiscLimit.Value = (decimal)LoadedUser.DiscLimit;
            ShowSpecialPermissions();

        }
        private int LoadSpecificUserRole(int UserId)
        {
            int RoleId = 0;
            try
            {
                List<UserRole> roles = new List<UserRole>();
                using (UnitOfWork ctx = new UnitOfWork())
                {
                    UserRole role = ctx.UserRoleRepository.GetUserRoleId(UserId);
                    if (role != null && role.UserRoleId > 0)
                    {
                        RoleId = role.UserRoleId;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RoleId;
        }
        private void LoadUserRolesAndRight()
        {
            User LoadedUser = new User();
            List<UserRole> roles = new List<UserRole>();
            List<Right> Rights = new List<Right>();
            using (unitOfwork = new UnitOfWork())
            {
                if (this.UserId > 0) // it mean its editing and load all roles so user may select any
                {
                    LoadedUser = unitOfwork.UserRepository.GetUserDetailsWithRolesAndRights(this.UserId);
                    if (LoadedUser != null && LoadedUser.UserRoles.Any(x => x.Description == "Admin"))//(LoadSpecificUserRole(this.UserId) == 1)
                    {
                        roles = unitOfwork.UserRoleRepository.GetAdminOnlyUserRole();
                    }
                    else
                    {
                        roles = unitOfwork.UserRoleRepository.GetNonAdminUserRoles();
                    }
                    //roles = unitOfwork.UserRoleRepository.GetActiveUserRoles();
                }
                else
                {
                    if (RoleId == 1)
                    {
                        roles = unitOfwork.UserRoleRepository.GetAdminOnlyUserRole();
                    }
                    else
                    {
                        roles = unitOfwork.UserRoleRepository.GetNonAdminUserRoles();
                    }
                }
                Rights = unitOfwork.RightRepository.GetAll().ToList();
            }
            foreach (var r in roles)
            {
                CheckBox cb = new CheckBox();
                if (r.UserRoleId == this.RoleId)
                {
                    cb.Checked = true;
                }
                cb.Tag = r.UserRoleId;
                cb.Text = r.Description;
                cb.AutoSize = true;
                cb.CheckedChanged += (sender, EventArgs) => { Role_CheckChanged(sender, EventArgs, r.UserRoleId); };
                pnlRoles.Controls.Add(cb);
            }

            foreach (Right r in Rights)
            {
                CheckBox cb = new CheckBox();
                cb.Tag = r.RightId;
                cb.Text = r.Description;
                cb.Checked = this.UserId <= 0 && RoleId == 1 ? true : false;
                if (this.UserId == 0 && cb.Text == "Adjust Stock")
                    cb.Checked = true;
                cb.AutoSize = true;
                pnlRights.Controls.Add(cb);
            }
            if (this.UserId > 0)
            {
                LoadUser(LoadedUser);
            }
        }

        private void Role_CheckChanged(object sender, EventArgs e, int RoleId)
        {
            ShowSpecialPermissions();
        }

        private void ShowSpecialPermissions()
        {
            bool isPharmacistChecked = false;
            foreach (Control ctr in pnlRoles.Controls)
            {
                CheckBox cb = (CheckBox)ctr;
                if (cb.Tag.ToString() == "2" && cb.Checked)
                {
                    isPharmacistChecked = true;
                    break;
                }
            }
            if (isPharmacistChecked)
            {
                pnlDiscPermission.Visible = true;
            }
            else
            {
                pnlDiscPermission.Visible = false;
            }
        }

        private void LoadRights()
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!correctpin)
                {
                    MessageBox.Show("Entered PIN Is already in Used, Please Choose Another PIN.");
                    return;
                }
                
                if (IsValidInput())
                {
                    if (this.UserId > 0)
                    {
                        UpdateUser();
                        UserPIN = 0;
                        return;
                    }
                    InsertUser();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void InsertUser()
        {
            User objUser = new User();
            bool isPharmacist = false;

            objUser.UserName = txtName.Text.Trim();
            objUser.Phone = txtPhone.Text.Trim();
            objUser.Email = txtEmail.Text.Trim();
            objUser.Password = txtPassword.Text;
            objUser.IsNew = true;
            objUser.IsActive = true;
            objUser.CreateDate = DateTime.Now;
            objUser.UpdateDate = DateTime.Now;
            objUser.Pin = Convert.ToInt32(txtpin.Text);
            objUser.UserRoles = new List<UserRole>();//unitOfwork.UserRoleRepository.GetById(cmbUserType.SelectedValue);
            objUser.Rights = new List<Right>();
            objUser.CreatedById = SharedVariables.LoggedInUser.UserId;
            if (cmbShifts.SelectedIndex > 0)
            {
                objUser.ShiftId = (Convert.ToInt32(cmbShifts.SelectedValue));
            }
            else
            {
                objUser.ShiftId = null;
            }

            using (unitOfwork = new UnitOfWork())
            {
                foreach (CheckBox cb in pnlRights.Controls)
                {
                    if (cb.Checked)
                    {
                        Right r = unitOfwork.RightRepository.GetById((int)cb.Tag);
                        objUser.Rights.Add(r);
                    }
                }
                foreach (CheckBox cb in pnlRoles.Controls)
                {
                    if (cb.Checked)
                    {
                        UserRole r = unitOfwork.UserRoleRepository.GetById((int)cb.Tag);
                        objUser.UserRoles.Add(r);
                        if ((int)cb.Tag == 2) // pharmacist
                        {
                            isPharmacist = true;
                        }
                    }
                }

                if (isPharmacist) // check special permissions
                {
                    objUser.DiscLimit = (double)numDiscLimit.Value;
                    objUser.CanGiveDiscount = chkCanGiveDiscount.Checked;
                }
                AuditLogTask.CreateAuditLog(unitOfwork, objUser.UserName, AuditModule.User, AuditAction.created);
                unitOfwork.UserRepository.Insert(objUser);
                unitOfwork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfwork, objUser, ModuleName.User, ModuleAction.Create, objUser.UserId);

            }
            MessageBox.Show("User Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void UpdateUser()
        {
            unitOfwork = new UnitOfWork();
            try
            {
                User user = unitOfwork.UserRepository.GetUserDetailsWithRolesAndRights(this.UserId);
                user.UserName = txtName.Text.Trim();
                user.Phone = txtPhone.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.Password = txtPassword.Text;
                user.Pin = Convert.ToInt32(txtpin.Text);
                if (user.IsSynced)
                {
                    user.IsNew = false;
                    user.IsUpdate = true;
                    user.IsSynced = false;
                }
                if (cmbShifts.SelectedIndex > 0)
                {
                    user.ShiftId = (Convert.ToInt32(cmbShifts.SelectedValue));
                }
                else
                {
                    user.ShiftId = null;
                }
                user.UpdateDate = DateTime.Now;
                user.CreatedById = SharedVariables.LoggedInUser.UserId;
                List<Right> NewRightsList = new List<Right>();
                List<Right> DeletedRights = new List<Right>();

                List<UserRole> NewRolesList = new List<UserRole>();
                List<UserRole> DeletedRoles = new List<UserRole>();

                //user rights check and upate logic
                // get new rights all
                foreach (CheckBox cb in pnlRights.Controls)
                {
                    if (cb.Checked)
                    {
                        Right r = new Core.Entities.Right();
                        r.RightId = (int)cb.Tag;
                        NewRightsList.Add(r);
                    }
                }
                // get deleted rights
                bool IsDeleted = true;
                foreach (Right ER in user.Rights)
                {
                    IsDeleted = true;
                    foreach (Right NR in NewRightsList)
                    {
                        if (ER.RightId == NR.RightId)
                        {
                            IsDeleted = false; // its found, not deleted
                            break;
                        }
                    }
                    if (IsDeleted)
                    {
                        DeletedRights.Add(ER);
                    }
                }

                foreach (Right r in DeletedRights)
                {
                    Right ER = user.Rights.Where(rt => rt.RightId == r.RightId).FirstOrDefault();
                    user.Rights.Remove(ER);
                }
                bool IsNew = true;
                foreach (Right NR in NewRightsList)
                {
                    IsNew = true;
                    foreach (Right ER in user.Rights)
                    {
                        if (NR.RightId == ER.RightId)
                        {
                            IsNew = false;
                            break;
                        }
                    }
                    if (IsNew)
                    {
                        user.Rights.Add(unitOfwork.RightRepository.GetById(NR.RightId));
                    }
                }



                //user roles check and upate logic
                // get new rights all
                foreach (CheckBox cb in pnlRoles.Controls)
                {
                    if (cb.Checked)
                    {
                        UserRole r = new Core.Entities.UserRole();
                        r.UserRoleId = (int)cb.Tag;
                        NewRolesList.Add(r);
                    }
                }
                // get deleted rights
                IsDeleted = false;
                foreach (UserRole ER in user.UserRoles)
                {
                    IsDeleted = true;
                    foreach (UserRole NR in NewRolesList)
                    {
                        if (ER.UserRoleId == NR.UserRoleId)
                        {
                            IsDeleted = false; // its found, not deleted
                            break;
                        }
                    }
                    if (IsDeleted)
                    {
                        DeletedRoles.Add(ER);
                    }
                }

                foreach (UserRole r in DeletedRoles)
                {
                    UserRole ER = user.UserRoles.Where(rl => rl.UserRoleId == r.UserRoleId).FirstOrDefault();
                    user.UserRoles.Remove(ER);
                }
                IsNew = false;
                foreach (UserRole NR in NewRolesList)
                {
                    IsNew = true;
                    foreach (UserRole ER in user.UserRoles)
                    {
                        if (NR.UserRoleId == ER.UserRoleId)
                        {
                            IsNew = false;
                            break;
                        }
                    }
                    if (IsNew)
                    {
                        user.UserRoles.Add(unitOfwork.UserRoleRepository.GetById(NR.UserRoleId));
                    }
                }

                bool isPharmacist = user.UserRoles.Any(ur => ur.UserRoleId == 2);
                if (isPharmacist)
                {
                    user.DiscLimit = (double)numDiscLimit.Value;
                    user.CanGiveDiscount = chkCanGiveDiscount.Checked;
                }

                unitOfwork.UserRepository.Update(user);
                AuditLogTask.CreateAuditLog(unitOfwork, user.UserName, AuditModule.User, AuditAction.updated);
                unitOfwork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfwork, user, ModuleName.User, ModuleAction.Update, user.UserId);
                if (user.UserId == SharedVariables.LoggedInUser.UserId)
                {
                    SharedVariables.LoggedInUser.Rights = user.Rights;
                }
                this.Close();
                MessageBox.Show("User Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while updating user data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                unitOfwork.Dispose();
            }
        }

        private bool IsValidInput()
        {

            bool ErrFound = false;
            bool rolesFound = false;
            StringBuilder sb = new StringBuilder();
            if (txtName.Text.Contains("@"))
                sb.Append("Can not use @ in name.").AppendLine();
            /*if (!SharedFunctions.ValidationEmail(txtEmail.Text))
                sb.Append("Invalid email address").AppendLine();*/
            /*if (!SharedFunctions.ValidatePassword(txtPassword.Text))
                sb.Append("At least one uppercase letter, one lowercase letter and one number in password.").AppendLine();*/
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()) && txtPhone.Text.Length < 10)
                sb.Append("Phone must be 10 characters long.").AppendLine();

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                SharedFunctions.MessageBoxShow(sb.ToString(), MessageType.Warning);
                return ErrFound;
            }
            foreach (CheckBox c in pnlRoles.Controls)
            {
                if (c.Checked)
                {
                    rolesFound = true;
                    break;
                }
            }
            if (!rolesFound)
            {
                ErrUserType.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrUserType.Visible = false;
            }
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrName.Visible = false;
            }
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                errEmail.Visible = true;
                ErrFound = true;
            }
            else
            {
                errEmail.Visible = false;
            }
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()) || txtPassword.Text.Length < 8)
            {
                ErrPassword.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrPassword.Visible = false;
            }
            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreview_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.PasswordChar = '\0';
        }

        private void btnPreview_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = '*';
            txtPassword.UseSystemPasswordChar = true;
        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkCanGiveDiscount_CheckedChanged(object sender, EventArgs e)
        {
            pnlDiscLimit.Visible = chkCanGiveDiscount.Checked;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtpin_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                lblNumericValues.Text = "";
            }
            else
            {
                lblNumericValues.Text = "Only Numeric Values are Allowed";
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            txtpin.UseSystemPasswordChar = false;
            txtpin.PasswordChar = '\0';
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            txtpin.PasswordChar = '*';
            txtpin.UseSystemPasswordChar = true;
        }
        private DataTable get(string qry)
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
        bool correctpin = true;
        private void txtpin_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtpin.Text))
            {
                DataTable user = new DataTable();
                int PIN = Convert.ToInt32(txtpin.Text);
                user = get("Select * from  Users Where Pin=" + PIN);
                if (user.Rows.Count > 0 && !(UserPIN ==PIN))
                {
                    lblNumericValues.Text = "PIN Already Used for Another User , Please Choose Another PIN.";
                    correctpin = false;
                }
                else
                {
                    lblNumericValues.Text = "";
                    correctpin = true;
                }
            }
        }
        private void SelectAllCheckedChangedAllRights(string checkboxstatus)
        {
            if (pnlRights.Controls != null && pnlRights.Controls.Count > 0)
            {


                foreach (CheckBox cb in pnlRights.Controls)
                {
                    if (checkboxstatus == "Checked")
                    {
                        cb.Checked = true;
                    }
                    else if (checkboxstatus == "Unchecked")
                    {
                        cb.Checked = false;
                    }


                }
            }
        }
        private void SelectcAllheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllCheckedChangedAllRights(SelectcAllheckBox.CheckState.ToString());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ErrPassword_Click(object sender, EventArgs e)
        {

        }

        private void ErrName_Click(object sender, EventArgs e)
        {

        }
    }
}