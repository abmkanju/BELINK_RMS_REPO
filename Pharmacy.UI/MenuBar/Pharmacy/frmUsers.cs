using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmUsers : Form
    {
        UnitOfWork unitOfWork;
        public frmUsers()
        {
            InitializeComponent();
        }
        private void LoadForm()
        {
            try
            {
                bool IsAdmin = false;
                foreach (UserRole r in SharedVariables.LoggedInUser.UserRoles)
                {
                    IsAdmin = r.IsAdmin;
                }
                List<UserRole> Roles = new List<UserRole>();
                using (unitOfWork = new UnitOfWork())
                {
                    loadUserRoles(unitOfWork);// this is being used to populate contextMenue dropdownitems on button click.
                    var UserRoles = unitOfWork.UserRoleRepository.GetAllRolesWithUsersAndRights(IsAdmin);
                    if (UserRoles != null && UserRoles.Count() > 0)
                    {
                        foreach (var item in UserRoles)
                        {
                            item.Users = item.Users.Where(x => x.IsActive == true).ToList();
                            Roles.Add(item);
                        }
                    }
                }
                foreach (UserRole r in Roles)
                {
                    if (r.UserRoleId == 1)
                    {
                        foreach (User u in r.Users)
                        {
                            grdAdmins.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }

                    if (r.UserRoleId == 2)
                    {
                        foreach (User u in r.Users)
                        {
                            grdPharmacists.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }
                    if (r.UserRoleId == 3)
                    {
                        foreach (User u in r.Users)
                        {
                            grdAccountants.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }
                    if (r.UserRoleId == 4)// doctoes
                    {
                        foreach (User u in r.Users)
                        {
                            grdDoctors.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }

                    if (r.UserRoleId == 5)// doctoes
                    {
                        foreach (User u in r.Users)
                        {
                            grdPharmacyCashier.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }

                    if (r.UserRoleId == 6)// doctoes
                    {
                        foreach (User u in r.Users)
                        {
                            grdPharmacyManager.Rows.Add(u.UserId, u.UserName, u.Email, u.Phone, "Edit", "Delete");
                        }
                        continue;
                    }
                }
                tabUsers.TabPages.Remove(tpAccountant);
                tabUsers.TabPages.Remove(tpDoctor);
                tabUsers.TabPages.Remove(tpPharmacist);
                tabUsers.TabPages.Remove(tabPage1);
                tabUsers.TabPages.Remove(tabPage2);
            }
            catch (Exception ex)
            {

            }
        }
        private void frmUsers_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddUser });
                SharedFunctions.SetGridStyle(grdAccountants);
                SharedFunctions.SetGridStyle(grdAdmins);
                SharedFunctions.SetGridStyle(grdDoctors);
                SharedFunctions.SetGridStyle(grdPharmacists);
                SharedFunctions.SetGridStyle(grdPharmacyCashier);
                SharedFunctions.SetGridStyle(grdPharmacyManager);
                LoadForm();

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void loadUserRoles(UnitOfWork _unitOfWork)
        {
            if (cxtMenuUser.Items.Count > 0)
            {
                return;
            }
            List<UserRole> roles = new List<UserRole>();
            roles = _unitOfWork.UserRoleRepository.GetActiveUserRoles();
            foreach (var r in roles)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem();
                if (r.UserRoleId == 1)
                {
                    mi.Tag = 1;
                }
                else if (r.UserRoleId == 2)
                {
                    mi.Tag = 2;
                }
                else if (r.UserRoleId == 3)
                {
                    mi.Tag = 3;
                }
                else if (r.UserRoleId == 4)
                {
                    mi.Tag = 4;
                }
                else if (r.UserRoleId == 5)
                {
                    mi.Tag = 5;
                }
                else if (r.UserRoleId == 6)
                {
                    mi.Tag = 6;
                }
                mi.Text = r.Description;
                mi.Click += new EventHandler(cxtMenuUser_click);
                cxtMenuUser.Items.Add(mi);
            }
        }

        private void cxtMenuUser_click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = (ToolStripMenuItem)sender;
            Form f = SharedFunctions.OpenForm(new frmAddUser(0, (int)mi.Tag), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            RefrehForm();
        }

        private void RefrehForm()
        {
            grdAdmins.Rows.Clear();
            grdPharmacists.Rows.Clear();
            grdAccountants.Rows.Clear();
            grdDoctors.Rows.Clear();
            grdPharmacyCashier.Rows.Clear();
            grdPharmacyManager.Rows.Clear();
            unitOfWork = new UnitOfWork();
            LoadForm();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Create Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            SharedFunctions.OpenForm(new frmAddUser(0, 1), this.MdiParent);
            //cxtMenuUser.Show(btnAddUser, new Point(0, btnAddUser.Height));
        }


        private void grdAccountants_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                int UserId = Convert.ToInt32(grdAccountants.Rows[e.RowIndex].Cells["colUserIdAc"].Value);
                if (e.ColumnIndex == grdAccountants.Columns["colEditAc"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdAccountants.Columns["colDeleteAc"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }

                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User Deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private void grdPharmacists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                int UserId = Convert.ToInt32(grdPharmacists.Rows[e.RowIndex].Cells["colUserIdPh"].Value);
                if (e.ColumnIndex == grdPharmacists.Columns["colEditPh"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdPharmacists.Columns["colDeletePh"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User Deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private void grdAdmins_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                int UserId = Convert.ToInt32(grdAdmins.Rows[e.RowIndex].Cells["colUserIdAd"].Value);
                if (e.ColumnIndex == grdAdmins.Columns["colEditAd"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdAdmins.Columns["colDeleteAd"].Index)
                {

                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }


                    if (UserId == 1) // first default admin users
                    {
                        MessageBox.Show("This is super admin, this user can't be deleted.", "Action Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User Deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private void grdDoctors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                int UserId = Convert.ToInt32(grdDoctors.Rows[e.RowIndex].Cells["colUserIdDoc"].Value);
                if (e.ColumnIndex == grdDoctors.Columns["colEditDoc"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdDoctors.Columns["colDeleteDoc"].Index)
                {

                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private void grdPharmacyCashier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                int UserId = Convert.ToInt32(grdPharmacyCashier.Rows[e.RowIndex].Cells["colIdPhrCashier"].Value);
                if (e.ColumnIndex == grdPharmacyCashier.Columns["colEditPhrCashier"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdPharmacyCashier.Columns["colDeletePhrCashier"].Index)
                {

                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private void grdPharmacyManager_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                int UserId = Convert.ToInt32(grdPharmacyManager.Rows[e.RowIndex].Cells["colIdPhrMng"].Value);
                if (e.ColumnIndex == grdPharmacyManager.Columns["colEditPhrMng"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Edit Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    Form f = SharedFunctions.OpenForm(new frmAddUser(UserId, 0), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    return;
                }
                if (e.ColumnIndex == grdPharmacyManager.Columns["colDeletePhrMng"].Index)
                {

                    if (!SharedFunctions.IsActionallowed("Delete Users") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    DialogResult rs = MessageBox.Show("Are You Sure, You Want To Delete This User", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        User u = unitOfWork.UserRepository.GetById(UserId);
                        u.IsActive = false;
                        u.UpdateDate = DateTime.Now;
                        u.IsUpdate = true;
                        unitOfWork.UserRepository.Update(u);
                        UserDeleteAuditLog(u, unitOfWork);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, u, ModuleName.User, ModuleAction.Delete, u.UserId);
                        MessageBox.Show("User deleted Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefrehForm();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }
        private void UserDeleteAuditLog(User u, UnitOfWork unitOfWork)
        {
            AuditLogTask.CreateAuditLog(unitOfWork, u.UserName, AuditModule.User, AuditAction.deleted);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadForm();
        }
    }
}