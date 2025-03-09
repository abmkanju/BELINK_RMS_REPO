using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.MenuBar.Pharmacy;
using Pharmacy.UI.Reports.Pharmacy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class StaffDetailsUI : Form
    {

        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        private int UserId = 0;
        private DateTime FromDate;
        private DateTime ToDate;

        #region LoadDetails
        public StaffDetailsUI(int UserId, DateTime FromDate, DateTime ToDate)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
        }
        private void StaffDetailsUI_Load(object sender, EventArgs e)
        {
            LoadDetails();
        }
        public void LoadDetails()
        {
            try
            {
                SharedFunctions.SetGridStyle(grdData);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadInvoiceDetails();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void LoadInvoiceDetails()
        {
            try
            {
                grdData.Rows.Clear();
                SummationsVM Sums = new SummationsVM();
                using (unitOfWork = new UnitOfWork())
                {
                    Transactions = unitOfWork.InvoiceRepository.GenericSearchForStaffDetailsUI
                    (
                        FromDate,
                        ToDate,
                        this.UserId,
                        PageNo,
                        SharedVariables.PageSize
                    );
                }
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    grdData.Rows.Add
                    (
                        t.InvoiceId,
                        t.InvoiceRefNo,
                        t.PatientName,
                        t.GrandTotal,
                        t.TotalPaid,
                        t.PaymentDate
                    );
                }
                lblTotalCollection.Text = Math.Round((double)Transactions.Sum(x=>x.GrandTotal), 2).ToString();
                lblTotalCash.Text = Math.Round((double)Transactions.Sum(x=>x.TotalPaid), 2).ToString();
                //lblTotalExpense.Text = Math.Round((double)Sums.CostOfAvailableStock, 2).ToString();

                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region DataGridEvents
        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region ReportPrint
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<InvoiceTransactionsVM> Transactions = GetDataForReport();
                StaffDetailsUIViewer v = new StaffDetailsUIViewer(0, this.FromDate, this.ToDate, Transactions);
                v.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<InvoiceTransactionsVM> Transactions = this.GetDataForReport();
                InvoiceTransactionsRpt_Landscape rpt = new InvoiceTransactionsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.FromDate);
                rpt.SetParameterValue("DateTo", this.ToDate);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private List<InvoiceTransactionsVM> GetDataForReport()
        {
            List<InvoiceTransactionsVM> Finaltransactions = new List<InvoiceTransactionsVM>();
            List<InvoiceTransactionsVM> Transactions = new List<InvoiceTransactionsVM>();
            using (unitOfWork = new UnitOfWork())
            {
                Finaltransactions = unitOfWork.InvoiceRepository.GenericSearchForStaffDetailsUIRpt(FromDate, ToDate, this.UserId);
            }
            foreach (var item in Transactions)
            {

            }
            return Finaltransactions;
        }
        #endregion

        #region PaginationButtons
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadInvoiceDetails();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadInvoiceDetails();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadInvoiceDetails();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadInvoiceDetails();
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
                    LoadInvoiceDetails();
                }
            }
        }
        #endregion
    }
}
