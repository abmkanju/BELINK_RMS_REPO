using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.MenuBar.Pharmacy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmAddAccountExpenses : Form
    {
        private UnitOfWork unitOfWork;
        int EditingRowIndex = -1;
        int EditExpenseId = 0;

        #region LoadDetials
        public frmAddAccountExpenses()
        {
            InitializeComponent();
        }
        public frmAddAccountExpenses(int EditExpenseId)
        {
            InitializeComponent();
            this.EditExpenseId = EditExpenseId;
        }
        private void frmAddAccountExpenses_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddItem, btnClearForm, btnClearAll, btnSave });
            SharedFunctions.SetGridStyle(grdItems);
            cmbPaymentType.SelectedIndex = 0;
            LoadDetials();
        }
        private void LoadDetials()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    LoadExpenseCategories(unitOfWork);
                }
                if (this.EditExpenseId > 0)
                    LoadEditingDetails();
                else
                { picBox.Visible = false; lblImagePath.Visible = true; }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void LoadEditingDetails()
        {
            using (unitOfWork = new UnitOfWork())
            {
                AccountExpenseVM expense = unitOfWork.AccountExpenseRepository.GetExpenseById(this.EditExpenseId);
                txtDescription.Text = expense.Description;
                cmbCategory.SelectedValue = expense.AccountExpenseCategoryId;
                txtAmount.Text = expense.Amount.ToString();
                txtdebit.Text = expense.debit.ToString();
                dtDepositDate.Value = expense.DepositDate;
                cmbPaymentType.SelectedIndex = expense.PaymentMode;
                btnAddItem.Enabled = false;
                btnClearForm.Enabled = false;
                btnClearAll.Enabled = false;
                grdItems.Enabled = false;
                lblImagePath.Visible = false;
                picBox.Visible = true;
                if (expense.Image != null && expense.Image.Length > 0)
                    picBox.Image = SharedFunctions.ByteArrayToImage(expense.Image);
                else
                    picBox.Image = null;
            }
        }
        private void LoadExpenseCategories(UnitOfWork p_unitOfWork)
        {
            List<AccountExpenseCategory> accountExpenseCategories = new List<AccountExpenseCategory>();
            accountExpenseCategories = p_unitOfWork.AccountExpenseCategoryRepository.GetAllActiveDetails();
            AccountExpenseCategory model = new AccountExpenseCategory();
            model.AccountExpenseCategoryId = 0;
            model.Name = "Select Category";
            accountExpenseCategories.Insert(0, model);
            cmbCategory.DataSource = accountExpenseCategories;
            cmbCategory.ValueMember = "AccountExpenseCategoryId";
            cmbCategory.DisplayMember = "Name";
        }
        #endregion

        #region ClearData
        private void Clear()
        {
            this.btnAddItem.Text = "Add";
            txtDescription.Text = "";
            txtAmount.Text = "";
            txtdebit.Text = "";
            lblImagePath.Text = "";
            dtDepositDate.Value = DateTime.Now;
            cmbCategory.SelectedIndex = 0;
            cmbPaymentType.SelectedIndex = 0;
            ErrMessage.Visible = false;
            ErrDescription.Visible = false;
            ErrCategory.Visible = false;
            if (EditingRowIndex >= 0)
                EditingRowIndex = -1;
        }
        #endregion

        #region AddExpense
        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidFieldInput())
                {
                    AccountExpenseVM objExpense = new AccountExpenseVM();
                    FillFieldObjects(objExpense);
                    if (EditingRowIndex >= 0)
                        UpdateExpenseToGrid(objExpense);
                    else
                        AddExpenseToGrid(objExpense);
                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private bool IsValidFieldInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                ErrDescription.Visible = true;
                ErrFound = false;
            }
            else
            {
                ErrFound = true;
                ErrDescription.Visible = false;
            }
            if (cmbCategory.SelectedIndex <= 0)
            {
                ErrCategory.Visible = true;
                ErrFound = false;
            }
            else
            {
                ErrCategory.Visible = false;
                ErrFound = true;
            }
            if (!ErrFound)
            {
                ErrMessage.Visible = true;
            }
            else
            {
                ErrMessage.Visible = false;
            }
            return ErrFound;

        }
        private void FillFieldObjects(AccountExpenseVM objExpense)
        {
            objExpense.Description = txtDescription.Text.Trim();
            objExpense.AccountExpenseCategoryId = SharedFunctions.StringToInt(cmbCategory.SelectedValue);
            objExpense.AccountExpenseCategoryName = cmbCategory.Text;
            objExpense.Amount = SharedFunctions.StringToDouble(txtAmount.Text);
            objExpense.debit = SharedFunctions.StringToDouble(txtdebit.Text);
            objExpense.DepositDate = dtDepositDate.Value;
            objExpense.PaymentMode = SharedFunctions.StringToInt(cmbPaymentType.SelectedIndex);
            objExpense.PaymentName = cmbPaymentType.Text;
            objExpense.FilePath = string.IsNullOrEmpty(lblImagePath.Text) || lblImagePath.Text == "No File Choosen" ? "" : lblImagePath.Text;

        }
        private void AddExpenseToGrid(AccountExpenseVM objExpense)
        {
            grdItems.Rows.Add
            (
                objExpense.Description,
                objExpense.AccountExpenseCategoryId,
                objExpense.AccountExpenseCategoryName,
                objExpense.PaymentMode,
                objExpense.PaymentName,
                string.Format(objExpense.DepositDate.Year + "/" + objExpense.DepositDate.Month + "/" + objExpense.DepositDate.Day),
                objExpense.Amount,
               objExpense.debit,
                objExpense.FilePath
            );
        }
        private void UpdateExpenseToGrid(AccountExpenseVM objExpense)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colDescription"].Value = objExpense.Description;
            r.Cells["colCategoryId"].Value = objExpense.AccountExpenseCategoryId;
            r.Cells["colCategoryName"].Value = objExpense.AccountExpenseCategoryName;
            r.Cells["colPaymentId"].Value = objExpense.PaymentMode;
            r.Cells["colPaymentMode"].Value = objExpense.PaymentName;
            r.Cells["colDepositDate"].Value = string.Format(objExpense.DepositDate.Year + "/" + objExpense.DepositDate.Month + "/" + objExpense.DepositDate.Day);
            r.Cells["colAmount"].Value = objExpense.Amount.ToString();
            r.Cells["coldebit"].Value = objExpense.debit.ToString();
            r.Cells["colFilePath"].Value = objExpense.FilePath.ToString();
        }
        #endregion

        #region DataGridEvents
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                {
                    if (e.RowIndex == EditingRowIndex)
                    {
                        Clear();
                        EditingRowIndex = -1;
                    }
                    else if (e.RowIndex < EditingRowIndex)
                    {
                        EditingRowIndex--;
                    }
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                {
                    Clear();
                    EditExpenseGrdToField(grdItems.Rows[e.RowIndex]);
                    btnAddItem.Text = "Update";
                    EditingRowIndex = e.RowIndex;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void EditExpenseGrdToField(DataGridViewRow r)
        {
            txtDescription.Text = SharedFunctions.ObjectToString(r.Cells["colDescription"].Value);
            cmbCategory.SelectedValue = SharedFunctions.StringToInt(r.Cells["colCategoryId"].Value);
            cmbPaymentType.SelectedIndex = SharedFunctions.StringToInt(r.Cells["colPaymentId"].Value);
            string depositDate = SharedFunctions.ObjectToString(r.Cells["colDepositDate"].Value);
            string[] depositDateList = depositDate.Split('/');
            dtDepositDate.Value = new DateTime
            (
                SharedFunctions.StringToInt(depositDateList[0]),
                SharedFunctions.StringToInt(depositDateList[1]),
                SharedFunctions.StringToInt(depositDateList[2])
            );
            txtAmount.Text = SharedFunctions.ObjectToString(r.Cells["colAmount"].Value);
            txtdebit.Text = SharedFunctions.ObjectToString(r.Cells["coldebit"].Value);

        }
        #endregion

        #region SaveExpense
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.EditExpenseId > 0)
                {
                    if (IsValidFieldInput())
                    {
                        UpdateExpense();
                    }
                }
                else
                {
                    if (grdItems.Rows.Count <= 0)
                        return;
                    SaveExpense();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void SaveExpense()
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                using (unitOfWork = new UnitOfWork())
                {
                    int voucherNo = unitOfWork.AccountExpenseRepository.GetVoucherNo();
                    List<AccountExpense> objList = new List<AccountExpense>();

                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        AccountExpense expense = new AccountExpense();
                        voucherNo++;
                        expense.VoucherNo = voucherNo;
                        expense.PaymentMode = SharedFunctions.StringToInt(r.Cells["colPaymentId"].Value);
                        expense.Description = r.Cells["colDescription"].Value.ToString();
                        expense.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        expense.AccountExpenseCategory = unitOfWork.AccountExpenseCategoryRepository.GetById(SharedFunctions.StringToInt(r.Cells["colCategoryId"].Value));
                        expense.Amount = SharedFunctions.StringToDouble(r.Cells["colAmount"].Value);
                        expense.debit = SharedFunctions.StringToDouble(r.Cells["coldebit"].Value);
                        string date = r.Cells["colDepositDate"].Value.ToString();
                        string[] dateSplit = date.Split('/');
                        expense.DepositDate = new DateTime
                        (
                            SharedFunctions.StringToInt(dateSplit[0]),
                            SharedFunctions.StringToInt(dateSplit[1]),
                            SharedFunctions.StringToInt(dateSplit[2])
                        );
                        expense.CreatedAt = DateTime.Now;
                        expense.UpdatedAt = DateTime.Now;
                        expense.IsActive = true;
                        string filePath = SharedFunctions.ObjectToString(r.Cells["colFilePath"].Value);
                        if (!string.IsNullOrEmpty(filePath))
                            expense.Image = SharedFunctions.GetBytesFromImage(filePath);
                        objList.Add(expense);



                    }
                    
                    unitOfWork.AccountExpenseRepository.InsertRange(objList);
                    unitOfWork.Save();
                    List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                        Transaction.CreatedAt = DateTime.Now;
                        Transaction.TransactionDate = DateTime.Now;
                        Transaction.TransactionExpense = SharedFunctions.StringToDouble(r.Cells["colAmount"].Value);
                        Transaction.User = SharedVariables.LoggedInUser.UserId;
                        Transaction.InvoiceId = voucherNo;
                        objTransaction.Add(Transaction);
                    }

                    unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                    unitOfWork.Save(); 
                    scope.Complete();


                }
            }
            
            MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void UpdateExpense()
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                
                double Expense = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    AccountExpense expense = unitOfWork.AccountExpenseRepository.GetById(this.EditExpenseId);
                    
                    expense.Description = txtDescription.Text;
                    expense.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    expense.AccountExpenseCategory = unitOfWork.AccountExpenseCategoryRepository.GetById(SharedFunctions.StringToInt(cmbCategory.SelectedValue));

                    Expense = (expense.debit - expense.Amount) - (SharedFunctions.StringToDouble(txtdebit.Text) - SharedFunctions.StringToDouble(txtAmount.Text));

                    expense.Amount = SharedFunctions.StringToDouble(txtAmount.Text);

                    expense.debit = SharedFunctions.StringToDouble(txtdebit.Text);
                    expense.PaymentMode = SharedFunctions.StringToInt(cmbPaymentType.SelectedIndex);
                    expense.DepositDate = dtDepositDate.Value;
                    expense.UpdatedAt = DateTime.Now;
                    expense.IsActive = true;
                    if (picBox.Image != null)
                        expense.Image = SharedFunctions.ImageToByteArrays(picBox.Image);
                    else
                        expense.Image = null;

                    unitOfWork.AccountExpenseRepository.Update(expense);
                    unitOfWork.Save();
                    if(Expense != 0)
                    {
                        List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                        
                        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                        Transaction.CreatedAt = DateTime.Now;
                        Transaction.TransactionDate = DateTime.Now;
                        Transaction.TransactionExpense = Expense;
                        Transaction.User = SharedVariables.LoggedInUser.UserId;
                        Transaction.InvoiceId = expense.VoucherNo;
                        objTransaction.Add(Transaction);
                        unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                        unitOfWork.Save();
                    }
                    scope.Complete();
                }
            }
            MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There are Some Un-Saved Items In Grid, are you Sure you Want to clear", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Cancel)
                {
                    return;
                }
            }
            Clear();
            grdItems.Rows.Clear();
        }
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }
        #endregion

        #region FilePath
        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                string DestPath = SharedFunctions.CopyFileToLocal(selectedFile);
                if (this.EditExpenseId > 0)
                    picBox.Image = Image.FromFile(DestPath);
                else
                    lblImagePath.Text = DestPath;
            }
        }

        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.EditExpenseId > 0)
                picBox.Image = null;
            else
                lblImagePath.Text = "No File Choosen";
        }
        #endregion

        private void txtdebit_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtdebit_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

    }
}
