using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddCategory : Form
    {
        UnitOfWork unitOfWork;
        public frmAddCategory()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtCategory.Text.Trim().Equals(""))
                {
                    Category c = new Category();
                    c.Name = txtCategory.Text.Trim();

                    c.CreateDate = DateTime.Now;
                    c.UpdateDate = DateTime.Now;
                    c.IsActive = true;
                    c.IsNew = true;
                    c.IsActive = true;
                    using (unitOfWork = new UnitOfWork())
                    {
                        c.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (unitOfWork.CategoryRepository.CheckDuplicateName(c.Name))
                        {
                            SharedFunctions.MessageBoxShow("Name already exist", MessageType.Info);
                            return;
                        }
                        unitOfWork.CategoryRepository.Insert(c);
                        AuditLogTask.CreateAuditLog(unitOfWork, c.Name, AuditModule.Category, AuditAction.created);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, c, ModuleName.Category, ModuleAction.Create, c.CategoryId);
                    }
                    this.Close();
                    MessageBox.Show("Category saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Category with Given Name Already Exists, Please Specify Another Name", "Category Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("Error occurred while saving category, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
