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
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.UI.MenuBar.Pharmacy;
using Pharmacy.UI.Reports.Pharmacy;
using System.Globalization;
namespace Pharmacy.UI.Reports.Accounts
{
    public partial class BillPaymentsUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<StockVM> BillPayments;
        int PageNo = 1;
        bool isPageChanged = false;
        public BillPaymentsUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefreshForm });
            SharedFunctions.SetGridStyle(grdBills);
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    BillPayments = unitOfWork.SupplierRepository.GetBillPayments_Report(PageNo, SharedVariables.PageSize);
                    //BillPayments = BillPayments.Distinct().ToPagedList(PageNo, SharedVariables.PageSize);                
                }

                double GrandTotal = 0;
                double TotalPaid = 0;
                double Due = 0;
                double varTotalPaid = 0;

                grdBills.Rows.Clear();
                foreach (StockVM b in BillPayments)
                {
                    //grdBills.Rows.Add(b.StockId, b.DocumentNo, b.Supplier.SupplierID, b.Supplier.Name, b.CreateDate.ToShortDateString(), b.SupplierInvoiceNo, b.SupplierIvoiceDate, Math.Round(b.TotalAmount - (double)b.TotalPaid - b.TotalReturnStockAmount, 2), b.CreateDate);
                    foreach (StockPaymentVM sp in b.StockPayments)
                    {
                        varTotalPaid = (double)(sp.TotalPaid == null ? 0 : sp.TotalPaid);
                        TotalPaid += varTotalPaid;
                    }

                    GrandTotal = Convert.ToDouble(b.TotalAmount);
                    Due = GrandTotal - TotalPaid;
                    grdBills.Rows.Add(b.StockId, b.DocumentNo, b.Supplier.SupplierID, b.Supplier.Name, b.CreateDate.ToShortDateString(), b.SupplierInvoiceNo, b.SupplierIvoiceDate, Math.Round(b.TotalAmount, 2), Math.Round(Due, 2), Math.Round(TotalPaid, 2), b.CreateDate);
                    TotalPaid = 0;
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = BillPayments.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = BillPayments.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, BillPayments.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdBills.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            if (e.ColumnIndex == grdBills.Columns["colPrint"].Index)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                //v.Show();
                //return;
            }

            if (e.ColumnIndex == grdBills.Columns["colEdit"].Index)
            {
                frmInvoice f = new frmInvoice(true, false, InvoiceId);
                f.Show();
                return;
            }

            if (e.ColumnIndex == grdBills.Columns["colReturn"].Index)
            {
                frmInvoice f = new frmInvoice(false, true, InvoiceId);
                f.Show();
                return;
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //List<InvoiceTransactionsVM> Transactions = LoadTransactions_Report();            
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            //        Reports.acc.RefundViewer v = new RefundViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
            //        v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation, Transactions);
            //    }
            //}
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.TransactionsRpt_Portrait rpt = new Reports.Financial.TransactionsRpt_Portrait();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    //rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    //rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Reports.Financial.TransactionsRpt_Landscape rpt = new Reports.Financial.TransactionsRpt_Landscape();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    //rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    //rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    List<InvoiceTransactionsVM> Transactions = GetReportData();
            //    Reports.Financial.TransactionsRpt_Landscape rpt = new TransactionsRpt_Landscape();
            //    rpt.SetDataSource(Transactions);
            //    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
            //    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
            //    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
            //    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
            //    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
            //    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
            //    if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
            //        MessageBox.Show("Financial Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            //}
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadTransactions();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTransactions();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = BillPayments.PageCount;
            LoadTransactions();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void txtInvoiceNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void grdBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdBills.Columns["colPayment"].Index)
            {
                long SupId = Convert.ToInt64(grdBills.Rows[e.RowIndex].Cells["colSupplierId"].Value);
                frmStockPayments f = new frmStockPayments(SupId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
            }
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                LoadTransactions();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > BillPayments.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadTransactions();
                }
            }
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }
    }
}