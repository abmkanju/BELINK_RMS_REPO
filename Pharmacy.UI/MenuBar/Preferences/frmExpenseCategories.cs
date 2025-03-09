using PagedList;
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
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmExpenseCategories : Form
    {
        int PageNo = 1;
        UnitOfWork unitOfWork;
        IPagedList<AccountExpenseCategory> accountExpenseList;

        #region LoadDetails
        public frmExpenseCategories()
        {
            InitializeComponent();
        }
        private void frmExpenseCategories_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdExpenseCategories);
            this.WindowState = FormWindowState.Maximized;
            LoadExpenseCategories();
        }
        private void LoadExpenseCategories()
        {
            try
            {
                grdExpenseCategories.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    DateTime? dtFrom = null;
                    if (dtpFromDate.Checked)
                        dtFrom = dtpFromDate.Value;
                    DateTime? dtTo = null;
                    if (dtpToDate.Checked)
                        dtTo = dtpToDate.Value;

                    accountExpenseList = unitOfWork.AccountExpenseCategoryRepository.GenericSearchForExpenseCategory(
                        dtpFromDate: dtFrom,
                        dtpToDate: dtTo,
                        PageNo: PageNo,
                        PageSize: SharedVariables.PageSize
                        );
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, accountExpenseList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = accountExpenseList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = accountExpenseList.HasNextPage;
                foreach (AccountExpenseCategory item in accountExpenseList)
                {
                    grdExpenseCategories.Rows.Add(item.AccountExpenseCategoryId, item.Name);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        #region HeaderButtons
        private void btnAddExpenseCategory_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddExpenseCategory(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadExpenseCategories();
        }
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged;
            dtpToDate.ValueChanged -= dtpToDate_ValueChanged;
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Checked = false;
            dtpToDate.Checked = false;
            dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
            dtpToDate.ValueChanged += dtpToDate_ValueChanged;
            PageNo = 1;
            LoadExpenseCategories();
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
        }
        #endregion

        #region Filters
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenseCategories();
        }
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenseCategories();
        }
        #endregion

        #region PaginationButtons
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadExpenseCategories();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadExpenseCategories();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = accountExpenseList.PageCount;
            LoadExpenseCategories();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenseCategories();
        }
        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }
        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > accountExpenseList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadExpenseCategories();
                }
            }
        }
        #endregion

        #region DataGridEvents
        private void grdExpenseCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int expenseCategoryId = SharedFunctions.StringToInt(grdExpenseCategories.Rows[e.RowIndex].Cells["colExpenseCategoryId"].Value);
                    if (expenseCategoryId <= 0)
                        return;
                    if (e.ColumnIndex == grdExpenseCategories.Columns["colEdit"].Index)
                    {
                        frmAddExpenseCategory f = new frmAddExpenseCategory(expenseCategoryId);
                        f.MdiParent = this.MdiParent;
                        f.FormClosed += new FormClosedEventHandler(ChildFormUpdate_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdExpenseCategories.Columns["colDelete"].Index)
                    {
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Expense Category?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.AccountExpenseCategoryRepository.SetExpenseCategoryInActive(expenseCategoryId);
                                unitOfWork.Save();
                            }
                            grdExpenseCategories.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Expense Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        #region ShortCutKeys
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddStores.PerformClick();
                return true;
            }

            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdExpenseCategories.SelectedRows.Count > 0)
                {
                    int colIndex = grdExpenseCategories.Columns["colEdit"].Index;
                    int rowIndex = grdExpenseCategories.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdExpenseCategories_CellContentClick(grdExpenseCategories, e);
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                {
                    int colIndex = grdExpenseCategories.Columns["colDelete"].Index;
                    int rowIndex = grdExpenseCategories.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdExpenseCategories_CellContentClick(grdExpenseCategories, e);
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

    }
}
