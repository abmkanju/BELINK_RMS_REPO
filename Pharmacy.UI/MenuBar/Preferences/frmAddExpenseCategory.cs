using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
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

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmAddExpenseCategory : Form
    {
        UnitOfWork unitOfWork;
        private int ExpenseCategoryId = 0;

        #region LoadExpenseCategory
        public frmAddExpenseCategory()
        {
            InitializeComponent();
        }
        public frmAddExpenseCategory(int ExpenseCategoryId)
        {
            InitializeComponent();
            this.ExpenseCategoryId = ExpenseCategoryId;
        }
        private void frmAddExpenseCategory_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetGridStyle(this.grdExpenseCategories);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnAdd });
            LoadDetails();
        }
        private void LoadDetails()
        {
            try
            {
                if (this.ExpenseCategoryId > 0)
                {
                    LoadEditExpenseCategory();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void LoadEditExpenseCategory()
        {
            AccountExpenseCategory r = new AccountExpenseCategory();
            grdExpenseCategories.Enabled = false;
            btnAdd.Enabled = false;
            using (unitOfWork = new UnitOfWork())
            {
                r = unitOfWork.AccountExpenseCategoryRepository.GetById(this.ExpenseCategoryId);
            }
            if (r != null)
            {
                txtName.Text = r.Name;
            }
        }
        #endregion

        #region DataGridEvents
        private void grdExpenseCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == grdExpenseCategories.Columns["colDelete"].Index)
                {
                    grdExpenseCategories.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region SaveDetails
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateExpenseCategory())
                    return;

                if (this.ExpenseCategoryId > 0)
                {
                    UpdateData();
                }
                else
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void Save()
        {
            try
            {
                List<AccountExpenseCategory> objList = new List<AccountExpenseCategory>();
                using (unitOfWork = new UnitOfWork())
                {
                    foreach (DataGridViewRow row in grdExpenseCategories.Rows)
                    {
                        AccountExpenseCategory model = new AccountExpenseCategory();
                        model.Name = row.Cells["colName"].Value.ToString().Trim();
                        model.IsActive = true;
                        model.CreatedAt = DateTime.Now;
                        model.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objList.Add(model);
                    }
                    unitOfWork.AccountExpenseCategoryRepository.InsertRange(objList);
                    unitOfWork.Save();
                    MessageBox.Show("Added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateData()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AccountExpenseCategory model = unitOfWork.AccountExpenseCategoryRepository.GetById(this.ExpenseCategoryId);
                    model.Name = txtName.Text.Trim();
                    model.UpdatedAt = DateTime.Now;
                    model.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    unitOfWork.AccountExpenseCategoryRepository.Update(model);
                    unitOfWork.Save();
                    MessageBox.Show("Updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ValidateExpenseCategory
        private bool ValidateExpenseCategory()
        {
            bool IsValidate = false;
            if (this.ExpenseCategoryId > 0)
            {
                if (ExpenseCategoryExists_Update(this.txtName.Text.Trim().ToLower(), this.ExpenseCategoryId))
                {
                    MessageBox.Show("Expense Category with Given Name Already Exists, Please Specify Another Name", "Expense Category Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return IsValidate;
                }
            }
            else
            {
                if (grdExpenseCategories.Rows.Count <= 0)
                {
                    return IsValidate;
                }
                foreach (DataGridViewRow r in grdExpenseCategories.Rows)
                {
                    if (ExpenseCategoryExists_new(r.Cells["colName"].Value.ToString().Trim().ToLower()))
                    {
                        MessageBox.Show("Expense Category with Given Name Already Exists, Please Specify Another Name", "Expense Category Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return IsValidate;
                    }
                }
            }
            return IsValidate = true;
        }
        private bool ExpenseCategoryExists_new(string name)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.AccountExpenseCategoryRepository.ExpenseCategoryExistsNew(name);
            }
        }
        private bool ExpenseCategoryExists_Update(string name, int TemplateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.AccountExpenseCategoryRepository.ExpenseCategoryExistsUpdate(name, this.ExpenseCategoryId);
            }
        }
        #endregion

        #region AddExpenseCategoryInGrid
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
                return;
            if (grdExpenseCategories.Rows.Cast<DataGridViewRow>().Any(r => r.Cells["colName"].Value.ToString().Equals(txtName.Text.Trim())))
            {
                SharedFunctions.MessageBoxShow("Expense Category already exists.", MessageType.Info);
                return;
            }
            grdExpenseCategories.Rows.Add(txtName.Text.Trim());
            Clear();
        }
        private void Clear()
        {
            txtName.Text = "";
        }
        #endregion

    }

}
