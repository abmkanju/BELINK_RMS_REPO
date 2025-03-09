using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
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
    public partial class frmPurchaseOrders : Form
    {
        private UnitOfWork unitOfWork;
        enum FilterType { Default = 1, Date = 2, PoNo = 3, Supplier = 4 };
        FilterType filter = FilterType.Default;
        IPagedList<PurchaseOrder> PurchaseOrders;
        int PageNo = 1;
        public frmPurchaseOrders()
        {
            InitializeComponent();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.R))
            {
                btnAddPurchaseOrder.PerformClick();
                return true;
            }
            #region GridView Events
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdPurchaseOrders.Focus();
                if (grdPurchaseOrders.Rows.Count >= 1)
                {
                    grdPurchaseOrders.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.V))
            {
                if (grdPurchaseOrders.SelectedRows.Count > 0)
                {
                    int colIndex = grdPurchaseOrders.Columns["colViewDetail"].Index;
                    int rowIndex = grdPurchaseOrders.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdPurchaseOrders_CellContentClick(grdPurchaseOrders, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.A))
            {
                if (grdPurchaseOrders.SelectedRows.Count > 0)
                {
                    int colIndex = grdPurchaseOrders.Columns["colAddStock"].Index;
                    int rowIndex = grdPurchaseOrders.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdPurchaseOrders_CellContentClick(grdPurchaseOrders, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdPurchaseOrders.SelectedRows.Count > 0)
                {
                    int colIndex = grdPurchaseOrders.Columns["colEdit"].Index;
                    int rowIndex = grdPurchaseOrders.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdPurchaseOrders_CellContentClick(grdPurchaseOrders, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                if (grdPurchaseOrders.SelectedRows.Count > 0)
                {
                    int colIndex = grdPurchaseOrders.Columns["colDelete"].Index;
                    int rowIndex = grdPurchaseOrders.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdPurchaseOrders_CellContentClick(grdPurchaseOrders, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            } if (keyData == (Keys.Alt | Keys.P))
            {
                if (grdPurchaseOrders.SelectedRows.Count > 0)
                {
                    int colIndex = grdPurchaseOrders.Columns["colPrint"].Index;
                    int rowIndex = grdPurchaseOrders.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdPurchaseOrders_CellContentClick(grdPurchaseOrders, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion GridView Events
            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.R))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadOrders()
        {
            try
            {
                grdPurchaseOrders.Rows.Clear();
                string Munufecturers = "";
                List<Manufacturer> manufectu = new List<Manufacturer>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Default)
                    {
                        PurchaseOrders = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderSummaryByDateRange(this.PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Date || filter == FilterType.Supplier)
                    {
                        long spId = Convert.ToInt64(cmbSuppliers.SelectedValue);
                        PurchaseOrders = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderSummaryByDateRange(dtpFrom.Value, dtpTo.Value, spId, this.PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.PoNo)
                    {
                        int PoNo = 0;
                        int.TryParse(txtPoNo.Text.Trim(), out PoNo);
                        PurchaseOrders = unitOfWork.PurchaseOrderRepository.GetPoByPoNo(PoNo);
                    }
                }
                
               
                foreach (PurchaseOrder order in PurchaseOrders)
                {
                    if (order.Manufecturers != null)
                    {
                        foreach (Manufacturer manufec in order.Manufecturers)
                        {
                            Munufecturers += manufec.Name + " , ";
                        }
                    }
                    if (order.Supplier != null)
                    {
                        order.SupplierName = order.Supplier.Name;
                        order.SupplierId = order.Supplier.SupplierID;
                    }
                    grdPurchaseOrders.Rows.Add(order.PurchaseOrderId, order.PurchaseOrderNo, order.OrderDate.ToString("dddd, dd MMMM yyyy"), order.SupplierId, order.SupplierName, Munufecturers, order.TotalAmount, order.User.UserName, null, null, null, null, null, order.StockId);
                    Munufecturers = "";
                }
               
                btnFirstPage.Enabled = btnPrevious.Enabled = PurchaseOrders.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = PurchaseOrders.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, PurchaseOrders.PageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading purchase orders data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbRangeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "today")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "yesterday")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-1);
            //    dtpTo.Value = DateTime.Today.AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 7 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-6);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 30 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-29);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "this month")
            //{
            //    dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last month")
            //{
            //    DateTime LastMonth = DateTime.Today.AddMonths(-1);
            //    dtpFrom.Value = new DateTime(LastMonth.Year, LastMonth.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "custom range")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
        }

        private void grdPurchaseOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                long StockId = 0;
                StockId = Convert.ToInt64(grdPurchaseOrders.Rows[e.RowIndex].Cells["colStockId"].Value);
                long PurchaseOrderId = Convert.ToInt64(grdPurchaseOrders.Rows[e.RowIndex].Cells["colPurchaseOrderId"].Value);
                string DocumentNo = SharedFunctions.ObjectToString(grdPurchaseOrders.Rows[e.RowIndex].Cells["colPurchaseOrderNo"].Value);
                if (e.RowIndex < 0)
                {
                    return;
                }
                if (e.ColumnIndex == grdPurchaseOrders.Columns["colViewDetail"].Index)
                {
                    frmPurchaseOrderDetail f = new frmPurchaseOrderDetail(PurchaseOrderId);
                    f.FormClosed += new FormClosedEventHandler(childForm_Closed);
                    f.Show();
                    return;
                }
                if (e.ColumnIndex == grdPurchaseOrders.Columns["colDelete"].Index)
                {
                    if (StockId > 0)
                    {
                        MessageBox.Show("Can't Edit/Delete/Add Stock for this Order, Stock Has been Added For This Order", "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    decimal OrderNo = Convert.ToDecimal(grdPurchaseOrders.Rows[e.RowIndex].Cells["colPurchaseOrderNo"].Value);
                    DialogResult rs = MessageBox.Show("Are You Sure You Want to Delete Order (" + OrderNo + ")", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == DialogResult.OK)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.PurchaseOrderRepository.SetInActive(PurchaseOrderId);
                            AuditLogTask.CreateAuditLog(unitOfWork, "PO# " + DocumentNo, AuditModule.Purchase_Order, AuditAction.deleted);
                            PurchaseOrder objPurchaseOrder = new PurchaseOrder() { PurchaseOrderId = PurchaseOrderId };
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, objPurchaseOrder, ModuleName.PurchaseOrder, ModuleAction.Delete, PurchaseOrderId);
                        }
                        grdPurchaseOrders.Rows.RemoveAt(e.RowIndex);
                        RefreshForm();
                        SharedFunctions.ShowSuccessMessage("Order Deleted Successfully", "Success");
                    }
                    return;
                }
                if (e.ColumnIndex == grdPurchaseOrders.Columns["colAddStock"].Index)
                {
                    if (!SharedFunctions.IsActionallowed("Add Stock"))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }

                    if (StockId > 0)
                    {
                        MessageBox.Show("Can't Edit/Delete/Add Stock for this Order, Stock Has been Added For This Order", "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //frmAddStock2 f = new frmAddStock2(PurchaseOrderId);
                    Form f = SharedFunctions.OpenForm(new frmAddStock(0, PurchaseOrderId), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(childForm_Closed);
                    f.Show();
                    return;
                }
                if (e.ColumnIndex == grdPurchaseOrders.Columns["colEdit"].Index)
                {
                    if (StockId > 0)
                    {
                        MessageBox.Show("Can't Edit/Delete/Add Stock for this Order, Stock Has been Added For This Order", "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    frmPurchaseOrder f = new frmPurchaseOrder(PurchaseOrderId);
                    f.FormClosed += new FormClosedEventHandler(childForm_Closed);
                    f.Show();
                    return;
                }

                if (e.ColumnIndex == grdPurchaseOrders.Columns["colPrint"].Index)
                {
                    Reports.PurchaseOrderViewer v = new Reports.PurchaseOrderViewer(PurchaseOrderId);
                    v.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void frmPurchaseOrders_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddPurchaseOrder, btnClearAll, btnClose });
            SharedFunctions.SetGridStyle(grdPurchaseOrders);
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            List<SuppliersComboVM> sl = new List<SuppliersComboVM>();
            using (unitOfWork = new UnitOfWork())
            {
                sl = unitOfWork.SupplierRepository.getSuppliersForCombo();
            }
            SuppliersComboVM all = new SuppliersComboVM();
            all.SupplierId = 0;
            all.Name = "Select All";
            sl.Insert(0, all);
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            cmbSuppliers.DataSource = sl;
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Clear()
        {
            //cmbRangeFilter.SelectedIndex = 0;
            dtpTo.Value = dtpFrom.Value = DateTime.Now;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            Clear();
            grdPurchaseOrders.Rows.Clear();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (PurchaseOrders.HasNextPage)
            {
                ++PageNo;
                LoadOrders();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            --PageNo;
            LoadOrders();
        }

        private void btnAddPurchaseOrder_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmPurchaseOrder(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(childForm_Closed);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            txtPoNo.Text = "";
            filter = FilterType.Date;
            PageNo = 1;
            LoadOrders();
        }

        private void frmPurchaseOrders_Shown(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            RefreshForm();
        }

        private void RefreshForm()
        {
            LoadOrders();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = PurchaseOrders.PageCount;
            LoadOrders();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadOrders();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            txtPoNo.Text = "";
            filter = FilterType.Date;
            PageNo = 1;
            LoadOrders();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            filter = FilterType.Default;
            txtPoNo.Text = "";
            LoadOrders();
            LoadSuppliers();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmPurchaseOrdersShortCuts f = new ShortCutDialogs.frmPurchaseOrdersShortCuts();
            f.ShowDialog();
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
                    if (_pageNo <= 0 || _pageNo > PurchaseOrders.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadOrders();
                }
            }
        }

        private void txtPoNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtPoNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            this.PageNo = 1;
            filter = FilterType.PoNo;
            LoadOrders();
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPoNo.Text = "";
            filter = FilterType.Supplier;
            this.PageNo = 1;
            LoadOrders();
        }
    }
}