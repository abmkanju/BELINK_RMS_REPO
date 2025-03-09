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
    public partial class PendingPayemntUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> PendingPayments;
        int PageNo = 1;
        public PendingPayemntUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            LoadPendingPayments();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnPrint, btnExcel, btnRefresh });
            SharedFunctions.SetGridStyle(grdData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                LoadPendingPayments();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;

            } 
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private List<InvoiceTransactionsVM> LoadPedingPayments_Reports()
        {
            List<InvoiceTransactionsVM> PendPmts;
            using (unitOfWork = new UnitOfWork())
            {
                PendPmts = unitOfWork.InvoiceRepository.GetPendingPayments_Financial_Reports(dtpFrom.Value, dtpTo.Value);
            }
            foreach (InvoiceTransactionsVM t in PendPmts)
            {
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
                t.TotalPaid = t.GrandTotal - t.Due;
            }
            return PendPmts;
        }

        private void LoadPendingPayments()
        {
            try
            {
                SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    Sums = unitOfWork.InvoiceRepository.GetSums_PendingPayments_Financial(dtpFrom.Value, dtpTo.Value);
                    PendingPayments = unitOfWork.InvoiceRepository.GetPendingPayments_Financial(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }

                grdData.Rows.Clear();
                foreach (InvoiceTransactionsVM p in PendingPayments)
                {
                    if (p.Patient != null)
                    {
                        p.PatientName = p.Patient.Name;
                        p.MrNo = p.Patient.MRNo.ToString();
                    }
                    else
                    {                        
                        p.PatientName = "";
                        p.MrNo = "";
                    }                    
                    grdData.Rows.Add(p.InvoiceId, p.MrNo, p.PatientName, p.InvoiceRefNo, Math.Round(p.GrandTotal,2), Math.Round(p.GrandTotal - p.Due,2), Math.Round( p.Due,2), p.CreateDate, p.IsProcedureInvoice);
                }
                if (Sums != null)
                {
                    lblTotalDues.Text = Sums.TotalDues == null ? "" : Math.Round((double)Sums.TotalDues,2).ToString();
                }
                else
                {
                    lblTotalDues.Text = "0";
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = PendingPayments.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = PendingPayments.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, PendingPayments.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPendingPayments();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPendingPayments();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                if(IsProcedureInvoice)
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
            List<InvoiceTransactionsVM> PaymentsList = LoadPedingPayments_Reports();
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    Reports.Financial.PendingPaymentsViewer v = new PendingPaymentsViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, PaymentsList);
                    v.Show();
                }
                else if (d.IsSaveAsPdf)
                {
                    SavePdf(d.Orientation, PaymentsList);
                }
            }
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> PaymentsList)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.PendingPaymentsRpt_Portrait rpt = new PendingPaymentsRpt_Portrait();
                    rpt.SetDataSource(PaymentsList);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Pending Payments Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Reports.Financial.PendingPaymentsRpt_Landscape rpt = new PendingPaymentsRpt_Landscape();
                    rpt.SetDataSource(PaymentsList);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Pending Payments Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<InvoiceTransactionsVM> PendPmts = LoadPedingPayments_Reports();
                Reports.Financial.PendingPaymentsRpt_Landscape rpt = new PendingPaymentsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(PendPmts);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                //rpt.SetDataSource(PendPmts);
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Pending Payments Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadPendingPayments();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadPendingPayments();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPendingPayments();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = PendingPayments.PageCount;
            LoadPendingPayments();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadPendingPayments();
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
            LoadPendingPayments();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > PendingPayments.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadPendingPayments();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            LoadPendingPayments();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }
    }
}