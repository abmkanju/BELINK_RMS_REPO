using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.MenuBar.OrderVoucherPayment;
using Pharmacy.UI.MenuBar.Pharmacy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.OrderVouchers
{
    public partial class frmOrderVoucher : Form
    {
        enum FilterType { Default = 1, OrderNumber = 2, CustomerName = 3, Phone = 4 }
        FilterType filter = FilterType.Default;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        InvoiceRepository repInvoice = new InvoiceRepository(cxt);
        UnitOfWork unitOfWork = new UnitOfWork();
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        PatientRepository repPatients = new PatientRepository(cxt);
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int PageNo = 1;
        Service obj = new Service();
        Invoice objInvoice = new Invoice();
        DataTable dtStockConsumption = new DataTable();
        public static ManualResetEvent allCompleted = new ManualResetEvent(false);
        Response chkRes = new Response();
        public frmOrderVoucher()
        {
            InitializeComponent();
        }
        public frmOrderVoucher(long InvoiceId)
        {
            InitializeComponent();
        }

        private void OrderChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (OrderChange.Text.Trim() != "")
                {
                    filter = FilterType.OrderNumber;
                }
                else
                {
                    filter = FilterType.Default;
                }
                PageNo = 1;
                LoadVoucher();
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadVoucher();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadVoucher();
        }
        public void GetOnlineOrder()
        {
            List<OrderOnlineAws> Order = new List<OrderOnlineAws>();
            List<OrderOnlineAws> checkItem = new List<OrderOnlineAws>();
        }
        private void LoadVoucher()
        {
            string CustName = "", CustAddress = "", CustPhone = "";
            string itemId = "";
            string itemName = "";
            string Qty = "";

            IPagedList<VoucherVM> VoucherIntegrate;
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.OrderNumber)
                {
                    VoucherIntegrate = unitOfWork.VoucherRepository.GetVoucherIntegrateByNumber(OrderChange.Text, dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
                else
                {
                    VoucherIntegrate = unitOfWork.VoucherRepository.GetVoucherIntegrate(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
            }

            grdVoucher.Rows.Clear();
            foreach (VoucherVM i in VoucherIntegrate)
            {
                string voucher = "";
                if (i.IsVoucherPaid == true)
                {
                    voucher = "Paid";
                }
                else
                {
                    voucher = "UnPaid";
                }
                grdVoucher.Rows.Add
                (
                 i.OrderVoucherId,
                 i.VoucherNumber,
                 i.VoucherAmount,
                 i.SubTotal,
                 i.WalletCredit,
                 i.GrandTotal,
                 voucher,
                 i.CreateDate
                );
            }

            SharedFunctions.ShowPageNo(lblPageNo, PageNo, VoucherIntegrate.PageCount);
            btnFirstPage.Enabled = btnPrevious.Enabled = VoucherIntegrate.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = VoucherIntegrate.HasNextPage;
        }
        private void Clear()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void grdItems_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            grdVoucher.CellValueChanged -= grdItems_CellValueChanged_1;
            List<StockAddFilterVM> checkItem = new List<StockAddFilterVM>();
            if (e.RowIndex < 0) return;
            int batchId = SharedFunctions.StringToInt(grdVoucher.Rows[e.RowIndex].Cells["colBatchId"].Value);
            int ItemId = SharedFunctions.StringToInt(grdVoucher.Rows[e.RowIndex].Cells["colItemId"].Value);

            if (e.ColumnIndex == grdVoucher.Columns["colBatch"].Index)
            {
                string unitSet = grdVoucher.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                int ConvUnit = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                batchId = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colBatch"].Value);
                grdVoucher.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId;
                getAvailableStock(grdVoucher.Rows[e.RowIndex], batchId, ItemId);

                using (unitOfWork = new UnitOfWork())
                {
                    checkItem = unitOfWork.InvoiceRepository.LoadSaleReturn(ItemId, Convert.ToInt32(batchId));
                }

                foreach (StockAddFilterVM j in checkItem)
                {
                    j.BatchId = j.BatchId.Distinct().ToList();
                    foreach (var m in j.BatchId)
                    {
                        if (Convert.ToInt32(m) == batchId)
                        {
                            int getQty = j.ConsumedStock;
                            getQty = unitSet.ToLower() == "pack" ? getQty / ConvUnit : getQty;
                            grdVoucher.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = getQty;
                        }
                    }
                }

                //for Rate
                //int retQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value);
                int rate = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colRate"].Value);
                //grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                grdVoucher.Rows[e.RowIndex].Cells["colRefundAmount"].Value = 0;
                //int refAmt = retQty * rate;
            }
            grdVoucher.CellValueChanged += grdItems_CellValueChanged_1;
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdVoucher.Columns["colRemove"].Index)
            {
                int itemId = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colItemId"].Value);
                try
                {
                    grdVoucher.Rows.RemoveAt(e.RowIndex);
                }
                catch (Exception ex)
                { }
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            //if (keyData == (Keys.Alt | Keys.I))
            //{
            //    this.uC_SearchItems1.Focus();
            //    return true;
            //}
            //if (keyData == (Keys.Alt | Keys.A))
            //{
            //    btnAddItem.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Alt | Keys.D))
            //{
            //    btnClear.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (grdVoucher.SelectedRows != null && grdVoucher.SelectedRows.Count > 0)
                {
                    int colIndex = grdVoucher.Columns["colRemove"].Index;
                    int rowIndex = grdVoucher.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdVoucher, e);
                }
                return true;
            }
            //if (keyData == (Keys.Control | Keys.S))
            //{
            //    btnSave.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Control | Keys.D))
            //{
            //    btnClearAll.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.Control | Keys.F))
            {
                frmInvoice f = new frmInvoice();
                f.ShowDialog();
            }
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdVoucher.Focus();
                if (grdVoucher.Rows.Count >= 1)
                {
                    grdVoucher.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (grdVoucher.Rows.Count > 0)
                {
                    if (this.ActiveControl.Name == grdVoucher.Name)
                    {
                        DataGridViewRow r = grdVoucher.SelectedRows[0];
                        grdVoucher.CurrentCell = grdVoucher.SelectedRows[0].Cells["colReturnQuantity"];
                    }
                    else
                    {
                        if (grdVoucher.Rows.Count > 0)
                        {
                            this.ActiveControl = grdVoucher;
                            grdVoucher.Rows[0].Selected = true;
                            grdVoucher.CurrentCell = grdVoucher.SelectedRows[0].Cells["colReturnQuantity"];
                            grdVoucher.CurrentCell.Selected = true;
                        }
                    }
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.T))
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClearAll(bool isAfterSave)
        {
            if (grdVoucher.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.Yes;
                }
                else
                {
                    rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK || rs == DialogResult.Yes)
                {
                    grdVoucher.Rows.Clear();
                    Clear();
                }
            }
        }

        private void getAvailableStock(DataGridViewRow r, int batchId, int itemId)
        {
            BatchStockVM vm = new BatchStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.ItemRspository.GetBatchStockSaleRet(batchId,
                    itemId,
                    0,
                    SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
            }
            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConversionUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                double rate = vm.RetailPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                }
                r.Cells["colRate"].Value = rate;
            }
        }

        private void grdVoucher_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
                //int InvoiceId = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                int VoucherId = Convert.ToInt32(grdVoucher.Rows[e.RowIndex].Cells["colVoucherId"].Value);
                string voucherName = grdVoucher.Rows[e.RowIndex].Cells["colVoucherValue"].Value.ToString();

                if (e.ColumnIndex == grdVoucher.Columns["colPayment"].Index)
                {
                    frmVoucherPayments f = new frmVoucherPayments(VoucherId, voucherName);
                    f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            PageNo = 1;
        }

        private void frmOrderVoucher_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdVoucher);
            this.WindowState = FormWindowState.Maximized;
            LoadVoucher();
        }

        private void frmOrderVoucher_VisibleChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            filter = FilterType.Default;
            OrderChange.Text = "";
            LoadVoucher();
        }
    }
}
