using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Reports;
using PagedList;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmManageStores : Form
    {
        int PageNo = 1;
        UnitOfWork unitOfWork;
        IPagedList<StoreVM> storesList;

        #region LoadDetails
        public frmManageStores()
        {
            InitializeComponent();
        }
        private void frmManageStores_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            LoadStores();
        }
        private void LoadStores()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    DateTime? dtFrom = null;
                    if (dtpFromDate.Checked)
                        dtFrom = dtpFromDate.Value;
                    DateTime? dtTo = null;
                    if (dtpToDate.Checked)
                        dtTo = dtpToDate.Value;

                    storesList = unitOfWork.StoreRepository.GenericSearchForStores(
                        dtpFromDate: dtFrom,
                        dtpToDate: dtTo,
                        FilterByName: txtNameFilter.Text.Trim(),
                        PageNo: PageNo,
                        PageSize: SharedVariables.PageSize
                        );
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, storesList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = storesList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = storesList.HasNextPage;
                foreach (StoreVM item in storesList)
                {
                    grdItems.Rows.Add(item.StoreId, item.Name, item.UserName, item.CreateDate);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        #region HeaderButtons
        private void btnAddStores_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddStores(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadStores();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {

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
            txtNameFilter.Text = "";
            PageNo = 1;
            LoadStores();
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
            LoadStores();
        }
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadStores();
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                PageNo = 1;
                LoadStores();
            }
        }
        #endregion

        #region PaginationButtons
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadStores();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadStores();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = storesList.PageCount;
            LoadStores();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadStores();
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
                    if (_pageNo <= 0 || _pageNo > storesList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadStores();
                }
            }
        }
        #endregion

        #region DataGridEvents
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int storeId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colStoreId"].Value);
                    string storeName = SharedFunctions.ObjectToString(grdItems.Rows[e.RowIndex].Cells["colName"].Value);
                    if (storeId <= 0)
                        return;
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        frmAddStores f = new frmAddStores(storeId);
                        f.MdiParent = this.MdiParent;
                        f.FormClosed += new FormClosedEventHandler(ChildFormUpdate_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Store?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.StoreRepository.SetStoreInactive(storeId);
                                AuditLogTask.CreateAuditLog(unitOfWork, storeName, AuditModule.Store, AuditAction.deleted);
                                unitOfWork.Save();
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, new Store() { StoreId = storeId }, ModuleName.Store, ModuleAction.Delete, storeId);
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Store Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (keyData == (Keys.Control | Keys.P))
            {
                btnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.E))
            {
                btnExcel.PerformClick();
                return true;
            }
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
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                {
                    int colIndex = grdItems.Columns["colDelete"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

    }
}