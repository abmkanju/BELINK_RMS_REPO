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
namespace Pharmacy.UI.Reports.Financial
{
    public partial class RefundUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        public RefundUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            LoadTransactions();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnExcel, btnPrint});
            SharedFunctions.SetGridStyle(grdData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.Value = DateTime.Now;
                dtpFrom.Value = DateTime.Now;
                LoadTransactions();
                dtpTo.ValueChanged += dtpTo_ValueChanged;
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private List<InvoiceTransactionsVM> LoadTransactions_Report()
        {
            List<InvoiceTransactionsVM> Transactions = new List<InvoiceTransactionsVM>();
            using (unitOfWork = new UnitOfWork())
            {
                Transactions = unitOfWork.InvoiceRepository.GetRefundInvoices_Financial_Report(dtpFrom.Value, dtpTo.Value);
            }
            string Description = "";
            foreach (InvoiceTransactionsVM t in Transactions)
            {
                Description = ""; ;
                if (t.IsProcedureInvoice)
                {
                    foreach (InvoiceItemVM i in t.InvoiceItems)
                    {
                        Description += i.ProcedureName + ", ";
                    }
                    if (!string.IsNullOrEmpty(Description))
                    {
                        Description = Description.Trim().TrimEnd(',');
                    }
                }
                else
                {
                    foreach (InvoiceItemVM i in t.InvoiceItems)
                    {
                        Description += i.ItemName + ", ";
                    }
                    if (!string.IsNullOrEmpty(Description))
                    {
                        Description = Description.Trim().TrimEnd(',');
                    }
                }
                t.InvoiceId = t.InvoiceRefNo;
                t.Description = Description;
                if (t.Patient != null)
                {
                    t.PatientName = t.Patient.Name;
                    t.MrNo = t.Patient.MRNo.ToString();
                }
                else
                {
                    t.PatientName = "";
                    t.MrNo = "";
                }
                if (t.User != null)
                {
                    t.UserName = t.User.UserName;
                }
            }
            return Transactions;
        }

        private void LoadTransactions()
        {
            try
            {
                //SummationsVM Sums = new SummationsVM();
                double TotalRefund = 0;
                long mrNum = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    if (txtMRNoSearch.Text.Trim() != "")
                    {
                        mrNum = Convert.ToInt64(txtMRNoSearch.Text);
                        TotalRefund = unitOfWork.InvoiceRepository.GetSumRefundInvoicesByMrNo_Financial(mrNum);
                        Transactions = unitOfWork.InvoiceRepository.GetRefundInvoicesMrNum_Financial(mrNum, PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        TotalRefund = unitOfWork.InvoiceRepository.GetSumRefundInvoices_Financial(dtpFrom.Value, dtpTo.Value);
                        Transactions = unitOfWork.InvoiceRepository.GetRefundInvoices_Financial(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                }
                lblTotalRefund.Text = TotalRefund.ToString();
                grdData.Rows.Clear();
                string Description = "";
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    Description = "";
                    if (t.IsProcedureInvoice)
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ProcedureName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }
                    else
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ItemName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }
                    grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(t.TotalRefund, 2), t.ReasonForRefund, t.User == null ? "" : t.User.UserName, t.RefundDate, t.IsProcedureInvoice);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
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
            long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);

            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                if (IsProcedureInvoice)
                {
                    Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                else
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                //v.Show();
                //return;
            }

            //if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
            //{
            //    frmInvoice f = new frmInvoice(true, false, InvoiceId);
            //    f.Show();
            //    return;
            //}

            //if (e.ColumnIndex == grdData.Columns["colReturn"].Index)
            //{
            //    frmInvoice f = new frmInvoice(false, true, InvoiceId);
            //    f.Show();
            //    return;
            //}
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<InvoiceTransactionsVM> Transactions = LoadTransactions_Report();
            Reports.Financial.RefundViewer v = new RefundViewer(0, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
            v.Show();
            //Commented Code
            //List<InvoiceTransactionsVM> Transactions = LoadTransactions_Report(); dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            //        Reports.Financial.RefundViewer v = new RefundViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
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
                    Reports.Financial.RefundRpt_Portrait rpt = new RefundRpt_Portrait();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Refund Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Reports.Financial.RefundRpt_Landscape rpt = new RefundRpt_Landscape();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Refund Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            try
            {
                List<InvoiceTransactionsVM> Transactions = LoadTransactions_Report();
                RefundRpt_Landscape rpt = new RefundRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                //rpt.SetDataSource(Transactions);
                //rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Refund Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
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
            PageNo = Transactions.PageCount;
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

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadTransactions();
                }
            }
        }

        private void txtMRNoSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void txtMRNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMRNoSearch.Text = "";
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = DateTime.Now;
            PageNo = 1;
            LoadTransactions();
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        }
    }
}