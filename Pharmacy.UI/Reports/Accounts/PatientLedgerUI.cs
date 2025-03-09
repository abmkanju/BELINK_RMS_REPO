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
using Pharmacy.UI.MenuBar.Preferences;
using Pharmacy.UI.Reports.Accounts;
using System.Data.SqlClient;
namespace Pharmacy.UI.Reports.Accounts
{
    public partial class PatientLedgerUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<PatientLedgerVM> patientledList;
        IPagedList<PatientLedgerVM> patientledListcheck;
        List<Item> Items;
        PatientLedgerSumVM patientledListSum;
        List<PatientLedgerVM> patientledListStat;

        int PageNo = 1;

        #region LoadDetails
        public PatientLedgerUI()
        {
            InitializeComponent();
        }
        private void TransactionUI_Load(object sender, EventArgs e)
        {
            LoadDetials();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
            SharedFunctions.SetGridStyle(grdData);
        }
        private DataTable get(string qry)
        {

            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;

        }
        private void Calculatevaluesforlabels()
        {


        }
        private void LoadDetials()
        {
            cmbPatientName.SelectedIndexChanged -= cmbPatientName_SelectedIndexChanged;
            try
            {
                LoadPatientNames();
                LoadPatientLedger();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbPatientName.SelectedIndexChanged += cmbPatientName_SelectedIndexChanged;
            }
        }
        //private void LoadExpenseCategories()
        //{
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        List<AccountExpenseCategory> accountExpenseCategories = new List<AccountExpenseCategory>();
        //        accountExpenseCategories = unitOfWork.AccountExpenseCategoryRepository.GetAllActiveDetails();
        //        AccountExpenseCategory model = new AccountExpenseCategory();
        //        model.AccountExpenseCategoryId = 0;
        //        model.Name = "Select Category";
        //        accountExpenseCategories.Insert(0, model);
        //        cmbExpenseCategory.DataSource = accountExpenseCategories;
        //        cmbExpenseCategory.ValueMember = "AccountExpenseCategoryId";
        //        cmbExpenseCategory.DisplayMember = "Name";
        //    }
        //}
        private void LoadPatientNames()
        {
            using (unitOfWork = new UnitOfWork())
            {
                List<Patient> patients = new List<Patient>();
                patients = unitOfWork.PatientRepository.GetAllList();
                cmbPatientName.DataSource = patients;
                cmbPatientName.ValueMember = "PatientId";
                cmbPatientName.DisplayMember = "Name";
            }
        }

        private void LoadPatientLedger()
        {
            try
            {
                if (grdData.Columns.Count <= 0)
                    return;
                //using (unitOfWork = new UnitOfWork())
                //{

                //    lbldebit.Text = unitOfWork.AccountExpenseRepository.GenericSearchForExpensesSum_debit
                //                           (
                //                              fromDate: dtpFrom.Value,
                //                              toDate: dtpTo.Value,
                //                              ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue)
                //                           ).ToString();
                //    lblcredit.Text = unitOfWork.AccountExpenseRepository.GenericSearchForExpensesSum
                //                           (
                //                              fromDate: dtpFrom.Value,
                //                              toDate: dtpTo.Value,
                //                              ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue)
                //                           ).ToString();
                //    expensesList = unitOfWork.AccountExpenseRepository.GenericSearchForAccountUI
                //                   (
                //                        fromDate: dtpFrom.Value,
                //                        toDate: dtpTo.Value,
                //                        ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue),
                //                        PageNo: PageNo,
                //                        PageSize: SharedVariables.PageSize
                //                   );
                //    lblTotalExpense.Text = (Convert.ToDouble(lbldebit.Text) - Convert.ToDouble(lblcredit.Text)).ToString();
                //}
                //Items = new List<Item>();
                using (unitOfWork = new UnitOfWork())
                {
                    Items = unitOfWork.ItemRspository.GetActiveItems();
                    if (dtpFrom.Checked && dtpTo.Checked)
                    {
                        var ftfr = Convert.ToDateTime(dtpFrom.Value.ToLongTimeString());
                        var ft = Convert.ToDateTime("12:00:00 AM");
                        var sub = ftfr - ft;
                        var tt = dtpFrom.Value.Subtract(sub);
                        patientledList = unitOfWork.InvoiceRepository.GetPatientLedger(Convert.ToInt64(cmbPatientName.SelectedValue), tt, dtpTo.Value, PageNo, SharedVariables.PageSize);
                        patientledListSum = unitOfWork.InvoiceRepository.GetPatientLedgerSum(Convert.ToInt64(cmbPatientName.SelectedValue), tt, dtpTo.Value);
                    }
                    else
                    {
                        patientledList = unitOfWork.InvoiceRepository.GetPatientLedger(Convert.ToInt64(cmbPatientName.SelectedValue), PageNo, SharedVariables.PageSize);
                        patientledListSum = unitOfWork.InvoiceRepository.GetPatientLedgerSum(Convert.ToInt64(cmbPatientName.SelectedValue));
                    }
                }
                if (patientledListSum != null)
                {
                    
                        lblDue.Text = patientledListSum.TotalDue.ToString();
                        lblGrossTotal.Text = patientledListSum.GrossTotal.ToString();
                        lblPaid.Text = patientledListSum.TotalPaid.ToString();
                        lblTotalRefund.Text = patientledListSum.TotalRefund.ToString();
                    
                }
                else
                {
                    lblDue.Text = "0";
                    lblGrossTotal.Text = "0";
                    lblPaid.Text = "0";
                    lblTotalRefund.Text = "0";

                }
                grdData.Rows.Clear();
                if (patientledList != null)
                {
                    List<long> invoiceRefList = new List<long>();
                    foreach (var pt in patientledList)
                    {
                      
                        foreach (var item in pt.InvoiceItems)
                        {
                            if (pt.Description == null)
                            {
                                pt.Description += item.Item.ItemName;
                            }
                            else
                            {
                                pt.Description += ", " + item.Item.ItemName;
                            }
                           
                        }
                        if (dtpFrom.Checked && dtpTo.Checked)
                        {
                            if (invoiceRefList.Contains(pt.InvoiceRefNo))
                            {
                                continue;
                            }
                            else
                            {
                                invoiceRefList.Add(pt.InvoiceRefNo);
                            }
                        }
                        grdData.Rows.Add(pt.InvoiceRefNo, pt.Description, pt.PaymentDate, pt.GrossTotal, pt.GrandTotal, pt.TotalPaid - pt.Refund, pt.Due, pt.TotalDiscount, pt.SalesTax, pt.Refund);
                    }
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = patientledList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = patientledList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, patientledList.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        
        #endregion

        #region ShortCutKeys
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                ReLoadDetails();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ReLoadDetails()
        {
            //dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            //dtpTo.ValueChanged -= dtpTo_ValueChanged;
            ////cmbPatientName.SelectedIndexChanged -= cmbPatientName_SelectedIndexChanged;
            //dtpFrom.Value = DateTime.Now;
            //dtpTo.Value = DateTime.Now;
            //cmbPatientName.SelectedIndex = 0;
            LoadPatientLedger();
            //dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            //dtpTo.ValueChanged += dtpTo_ValueChanged;
            ////cmbPatientName.SelectedIndexChanged += cmbPatientName_SelectedIndexChanged;
        }
        #endregion

        #region Filters
        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddAccountExpenses(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildFormAdd_Closed);
            f.Show();
        }
        private void ChildFormAdd_Closed(object sender, FormClosedEventArgs e)
        {
            ReLoadDetails();
        }
        private void cmbPatientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPatientLedger();
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPatientLedger();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPatientLedger();
        }
        #endregion

        #region DatagridEvents
        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                int ExpenseId = SharedFunctions.StringToInt(grdData.Rows[e.RowIndex].Cells["colId"].Value);
                if (ExpenseId <= 0)
                    return;
                if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
                {
                    Form f = SharedFunctions.OpenForm(new frmAddAccountExpenses(ExpenseId), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(ChildFormUpdate_Closed);
                    f.Show();
                }
                else if (e.ColumnIndex == grdData.Columns["colRemove"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are you sure you want to delete this?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.AccountExpenseRepository.SetExpenseInActive(ExpenseId);
                        unitOfWork.Save();
                    }
                    LoadPatientLedger();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadPatientLedger();
        }
        #endregion

        #region LoadReports
        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<PatientLedgerVM> objList = LoadPatientLedgerReports();
            PatientLedherViewer v = new PatientLedherViewer(0, this.dtpFrom.Value, this.dtpTo.Value, objList);
            //v.Show();

        }
        //private void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<PatientLedgerVM> objList = LoadPatientLedgerReports();
        //        AccountRpt_Landscape rpt = new AccountRpt_Landscape();
        //        rpt.Database.Tables[0].SetDataSource(objList);
        //        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //        {
        //            rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //        }
        //        rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //        rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //        rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
        //        rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
        //        rpt.SetParameterValue("DateTo", this.dtpTo.Value);
        //        rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
        //        if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
        //            MessageBox.Show("Expenses Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
        //    }
        //}
        //private void SavePdf(int Orientation, List<InvoiceTransactionsVM> PaymentsList)
        //{
        //    try
        //    {
        //        if (Orientation == 0)
        //        {
        //            Reports.Financial.PendingPaymentsRpt_Portrait rpt = new PendingPaymentsRpt_Portrait();
        //            rpt.SetDataSource(PaymentsList);
        //            rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
        //            rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
        //            rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
        //            rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
        //            rpt.SetParameterValue("DateTo", this.dtpTo.Value);
        //            rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
        //            if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
        //                MessageBox.Show("Financial Pending Payments Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //        else
        //        {
        //            Reports.Financial.PendingPaymentsRpt_Landscape rpt = new PendingPaymentsRpt_Landscape();
        //            rpt.SetDataSource(PaymentsList);
        //            rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
        //            rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
        //            rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
        //            rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
        //            rpt.SetParameterValue("DateTo", this.dtpTo.Value);
        //            rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
        //            if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
        //                MessageBox.Show("Financial Pending Payments Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
        //    }
        //}
        private List<PatientLedgerVM> LoadPatientLedgerReports()
        {
            List<PatientLedgerVM> patientledList = new List<PatientLedgerVM>(); ;
            using (unitOfWork = new UnitOfWork())
            {
                if (dtpFrom.Checked && dtpTo.Checked)
                {
                    var ftfr = Convert.ToDateTime(dtpFrom.Value.ToLongTimeString());
                    var ft = Convert.ToDateTime("12:00:00 AM");
                    var sub = ftfr - ft;
                    var tt = dtpFrom.Value.Subtract(sub);
                    patientledList = unitOfWork.InvoiceRepository.GenericSearchForPatientLedgerRpt(Convert.ToInt64(cmbPatientName.SelectedValue), tt , dtpTo.Value);
                }
                else
                {
                    patientledList = unitOfWork.InvoiceRepository.GenericSearchForPatientLedgerRpt(Convert.ToInt64(cmbPatientName.SelectedValue));
                }
            }
            List<long> invoiceRefList = new List<long>();
            List<PatientLedgerVM> VerifiedPatient = new List<PatientLedgerVM>();

            foreach (PatientLedgerVM pt in patientledList)
            {
                foreach (var item in pt.InvoiceItemsV)
                {
                    if (pt.Description == null)
                    {
                        pt.Description += item.ItemName;
                    }
                    else
                    {
                        pt.Description += ", " + item.ItemName;
                    }
                    if (dtpFrom.Checked && dtpTo.Checked)
                    {
                        if (invoiceRefList.Contains(pt.InvoiceRefNo))
                        {
                            continue;
                        }
                        else
                        {
                            invoiceRefList.Add(pt.InvoiceRefNo);
                        }
                    }
                    VerifiedPatient.Add(pt);
                }
            }
            return VerifiedPatient;
        }
        #endregion

        #region Pagination
        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > patientledList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadPatientLedger();
                }
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadPatientLedger();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadPatientLedger();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPatientLedger();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = patientledList.PageCount;
            LoadPatientLedger();
        }
        #endregion

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            ReLoadDetails();
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            List<PatientLedgerVM> objList = LoadPatientLedgerReports();
            PatientLedherViewer v = new PatientLedherViewer(0, this.dtpFrom.Value, this.dtpTo.Value, objList);
            v.Show();

        }
    }
}