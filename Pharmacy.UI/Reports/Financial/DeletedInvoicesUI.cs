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
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Data.SqlClient;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class DeletedInvoicesUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        bool isPageChanged = false;
        public DeletedInvoicesUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnPrint, btnExcel});
            SharedFunctions.SetGridStyle(grdData);
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            try
            {
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    Transactions = unitOfWork.InvoiceRepository.GenericSearchForDeleteInvoices(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
                grdData.Rows.Clear();
                string Description = "";
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    Description = string.Join(",", t.InvoiceItems.Select(x => x.ItemName).ToList());
                    grdData.Rows.Add
                    (
                        t.InvoiceRefNo,
                        t.Patient == null ? "" : t.Patient.Name,
                        Description,
                        t.Due > 0 ? t.Due : 0,
                        t.Due < 0 ? -t.Due : 0,
                        Math.Round(t.GrandTotal, 2),
                        Math.Round(t.TotalPaid, 2),
                        t.CreateDate,
                        t.User == null ? "" : t.User.UserName,
                        t.DeletedAt
                    );
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<DeleteInvoiceVM> invoices = LoadDeleteInvoicesRpt();
                DeleteInvoiceViewer v = new DeleteInvoiceViewer(invoices, dtpFrom.Value, dtpTo.Value);
                v.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private List<DeleteInvoiceVM> LoadDeleteInvoicesRpt()
        {
            List<DeleteInvoiceVM> result = new List<DeleteInvoiceVM>();
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    result = unitOfWork.InvoiceRepository.GenericSearchForDeleteInvoicesRpt(dtpFrom.Value, dtpTo.Value);
                }
                foreach (DeleteInvoiceVM t in result)
                {
                    t.Description = string.Join(",", t.ItemName.ToList());
                    t.Due = t.Due > 0 ? t.Due : 0;
                    t.Advance = t.Due < 0 ? -t.Due : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<DeleteInvoiceVM> invoices = LoadDeleteInvoicesRpt();
                DeleteInvoiceRpt_Landscape rpt = new DeleteInvoiceRpt_Landscape();
                rpt.SetDataSource(Transactions);
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
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

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
            catch (Exception ex)
            {

            }
        }

        #region CommentedCode
        private List<InvoiceTransactionsVM> LoadTransactions_Report()
        {
            try
            {
                List<InvoiceTransactionsVM> Transactions;
                using (unitOfWork = new UnitOfWork())
                {
                    Transactions = null; // unitOfWork.InvoiceRepository.GenericSearchForDeleteInvoices(dtpFrom.Value, dtpTo.Value);
                }
                string Description = "";
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    foreach (InvoiceItemVM i in t.InvoiceItems)
                    {
                        Description += i.ItemName + ", ";
                    }
                    if (!string.IsNullOrEmpty(Description))
                    {
                        t.Description = Description.Trim().TrimEnd(',');
                    }

                    if (t.Patient != null)
                    {
                        t.MrNo = t.Patient.MRNo.ToString();
                        t.PatientName = t.Patient.Name;
                    }
                    else
                    {
                        t.MrNo = "";
                        t.PatientName = "";
                    }
                }
                return Transactions;
            }
            catch (Exception ex)
            {
                throw ex;
                //SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            LoadTransactions();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }
        private void UpdateDB(String Query)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            SqlCommand com = new SqlCommand(Query, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            com.ExecuteNonQuery();
            con.Close();

        }
        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdData.Columns["colRestore"].Index)
            {

                int InvoicerefNo = Convert.ToInt32(grdData.Rows[e.RowIndex].Cells[0].Value.ToString());
                DialogResult rs = MessageBox.Show("Are You Sure You Want To Restore This Invoice?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == System.Windows.Forms.DialogResult.OK)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        int InvoiceId = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(InvoicerefNo);
                        Invoice obj = unitOfWork.InvoiceRepository.GetInvoiceToDelete(InvoiceId);
                        obj.IsActive = true;
                        obj.DeletedAt = null;
                        foreach (InvoiceItem i in obj.InvoiceItems)
                        {
                            i.IsActive = true;
                            i.DeletedAt = null;
                            unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                        }
                        obj.StockConsumption.IsActive = true;
                        unitOfWork.GetDbContext().Entry(obj.StockConsumption).State = System.Data.Entity.EntityState.Modified;
                        foreach (StockConsumptionItem si in obj.StockConsumption.StockConsumptions)
                        {
                            si.IsActive = true;
                            si.DeletedAt = null;
                            unitOfWork.GetDbContext().Entry(si).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (InvoicePayment ip in obj.InvoicePayments)
                        {
                            ip.IsActive = true;
                            ip.DeletedAt = null;

                            unitOfWork.GetDbContext().Entry(ip).State = System.Data.Entity.EntityState.Modified;
                        }
                        //SOT
                        UpdateDB("Update SalesReturnItems set IsActive = 1 where InvoiceRefNo=" + obj.InvoiceId);
                        UpdateDB("Update ProfitStatementTransactions set IsDeleted = 0 where InvoiceId=" + obj.InvoiceRefNo);
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, obj, ModuleName.Invoice, ModuleAction.Update, obj.InvoiceId);
                        unitOfWork.InvoiceRepository.Update(obj);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + obj.InvoiceRefNo, AuditModule.Invoice, AuditAction.Restore_invoice);
                        unitOfWork.Save();
                        MessageBox.Show("Invoice Restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRefresh.PerformClick();
                    }
                }
            }
            else if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                int InvoiceId = -1;
                int InvoicerefNo = Convert.ToInt32(grdData.Rows[e.RowIndex].Cells[0].Value.ToString());
                using (unitOfWork = new UnitOfWork())
                {
                    InvoiceId = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(InvoicerefNo);
                }
                if (InvoiceId != -1)
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true , IsDeletedInvoice:true);
                    //v.Show();
                    v.Show();
                }
                else
                {
                    MessageBox.Show("Something Went Wrong, Please Try Again");
                    return;
                }
            }
        }
    }
}