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
namespace Pharmacy.UI.Reports.Financial
{
    public partial class AccountUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<AccountExpenseVM> expensesList;
        int PageNo = 1;

        #region LoadDetails
        public AccountUI()
        {
            InitializeComponent();
        }
        private void TransactionUI_Load(object sender, EventArgs e)
        {
            LoadDetials();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddExpense, btnExcel, btnPrint, btnRefreshForm});
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
            cmbExpenseCategory.SelectedIndexChanged -= cmbExpenseCategory_SelectedIndexChanged;
            try
            {
                LoadExpenseCategories();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbExpenseCategory.SelectedIndexChanged += cmbExpenseCategory_SelectedIndexChanged;
            }
        }
        private void LoadExpenseCategories()
        {
            using (unitOfWork = new UnitOfWork())
            {
                List<AccountExpenseCategory> accountExpenseCategories = new List<AccountExpenseCategory>();
                accountExpenseCategories = unitOfWork.AccountExpenseCategoryRepository.GetAllActiveDetails();
                AccountExpenseCategory model = new AccountExpenseCategory();
                model.AccountExpenseCategoryId = 0;
                model.Name = "Select Category";
                accountExpenseCategories.Insert(0, model);
                cmbExpenseCategory.DataSource = accountExpenseCategories;
                cmbExpenseCategory.ValueMember = "AccountExpenseCategoryId";
                cmbExpenseCategory.DisplayMember = "Name";
            }
        }
        private void LoadExpenses()
        {
            try
            {
                if (grdData.Columns.Count <= 0)
                    return;
                using (unitOfWork = new UnitOfWork())
                {
                    
                    lbldebit.Text = unitOfWork.AccountExpenseRepository.GenericSearchForExpensesSum_debit
                                           (
                                              fromDate: dtpFrom.Value,
                                              toDate: dtpTo.Value,
                                              ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue)
                                           ).ToString();
                    lblcredit.Text = unitOfWork.AccountExpenseRepository.GenericSearchForExpensesSum
                                           (
                                              fromDate: dtpFrom.Value,
                                              toDate: dtpTo.Value,
                                              ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue)
                                           ).ToString();
                    expensesList = unitOfWork.AccountExpenseRepository.GenericSearchForAccountUI
                                   (
                                        fromDate: dtpFrom.Value,
                                        toDate: dtpTo.Value,
                                        ExpenseCategoryId: SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue),
                                        PageNo: PageNo,
                                        PageSize: SharedVariables.PageSize
                                   );
                    lblTotalExpense.Text = (Convert.ToDouble(lbldebit.Text) - Convert.ToDouble(lblcredit.Text)).ToString() ;
                }

                grdData.Rows.Clear();
                foreach (AccountExpenseVM p in expensesList)
                {
                    switch (p.PaymentMode)
                    {
                        case 0:
                            p.PaymentName = "Cash";
                            break;
                        case 1:
                            p.PaymentName = "Cheque";
                            break;
                        case 2:
                            p.PaymentName = "Debit/Credit Card";
                            break;
                        case 3:
                            p.PaymentName = "Online Payment";
                            break;
                        default:
                            break;
                    }
                    grdData.Rows.Add(p.AccountExpenseId, p.VoucherNo, p.Description, p.DepositDate.ToString("dd/MM/yyyy"), p.AccountExpenseCategoryName, Math.Round(p.Amount, 2),Math.Round(p.debit, 2), p.PaymentName, p.CreatedAt.ToString("dd/MM/yyyy"), p.CreatedBy);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = expensesList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = expensesList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, expensesList.PageCount);
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
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbExpenseCategory.SelectedIndexChanged -= cmbExpenseCategory_SelectedIndexChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            cmbExpenseCategory.SelectedIndex = 0;
            LoadExpenses();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            cmbExpenseCategory.SelectedIndexChanged += cmbExpenseCategory_SelectedIndexChanged;
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
        private void cmbExpenseCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenses();
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenses();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenses();
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
                    LoadExpenses();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadExpenses();
        }
        #endregion

        #region LoadReports
        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<AccountExpenseRptVM> objList = LoadAccountReports();
            AccountViewer v = new AccountViewer(0, this.dtpFrom.Value, this.dtpTo.Value, objList);
            v.Show();

        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<AccountExpenseRptVM> objList = LoadAccountReports();
                AccountRpt_Landscape rpt = new AccountRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(objList);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Expenses Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
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
        private List<AccountExpenseRptVM> LoadAccountReports()
        {
            List<AccountExpenseRptVM> objList = new List<AccountExpenseRptVM>(); ;
            using (unitOfWork = new UnitOfWork())
            {
                objList = unitOfWork.AccountExpenseRepository.GenericSearchForAccountUIRpt(dtpFrom.Value, dtpTo.Value, SharedFunctions.StringToInt(cmbExpenseCategory.SelectedValue));
            }
            foreach (AccountExpenseRptVM item in objList)
            {
                switch (item.PaymentMode)
                {
                    case 0:
                        item.PaymentName = "Cash";
                        break;
                    case 1:
                        item.PaymentName = "Cheque";
                        break;
                    case 2:
                        item.PaymentName = "Debit/Credit Card";
                        break;
                    case 3:
                        item.PaymentName = "Online Payment";
                        break;
                    default:
                        break;
                }
                item.CreatedAt = item.CreatedAt;
            }
            return objList;
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
                    if (_pageNo <= 0 || _pageNo > expensesList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadExpenses();
                }
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadExpenses();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadExpenses();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpenses();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = expensesList.PageCount;
            LoadExpenses();
        }
        #endregion

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            ReLoadDetails();
        }
    }
}