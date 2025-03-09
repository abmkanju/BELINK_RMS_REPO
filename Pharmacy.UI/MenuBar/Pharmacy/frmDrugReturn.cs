using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.Core;


namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmDrugReturn : Form
    {
        enum FilterType { Default = 1, Date = 2, Patient = 3, User = 4, Invoice = 5 }
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int PageNo = 1;
        IPagedList<InvoiceReturnVM> InvoiceReturnList;
        public frmDrugReturn()
        {
            InitializeComponent();
        }

        private void frmDrugReturn_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnClose });
            SharedFunctions.SetGridStyle(grdInvoices);
            this.dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            this.dtpTo.ValueChanged -= dtpTo_ValueChanged;
            this.WindowState = FormWindowState.Maximized;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            LoadDrugReturns();
            this.dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
            LoadUsers();
            cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            this.dtpTo.ValueChanged += dtpTo_ValueChanged;

        }
        private void LoadUsers()
        {
            try
            {
                List<UserSelectVM> us = new List<UserSelectVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    us = unitOfWork.UserRepository.GetAllActiveUsers();
                }
                UserSelectVM v = new UserSelectVM
                {
                    UserId = 0,
                    UserName = "Select All"
                };
                us.Insert(0, v);
                cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
                cmbUsers.DataSource = us;
                cmbUsers.DisplayMember = "UserName";
                cmbUsers.ValueMember = "UserId";
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users data. Please try again.", "User Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadDrugReturns()
        {
            grdInvoices.Rows.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Date)
                {
                    InvoiceReturnList = unitOfWork.InvoiceRepository.GetInvoiceReturns(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Invoice)
                {
                    InvoiceReturnList = unitOfWork.InvoiceRepository.GetInvoiceReturns(dtpFrom.Value, dtpTo.Value, Convert.ToInt64(txtSearchByInvoice.Text), PageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Patient)
                {
                    InvoiceReturnList = unitOfWork.InvoiceRepository.GetInvoiceReturns(dtpFrom.Value, dtpTo.Value, txtGeneralSearch.Text.Trim().ToLower(), PageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.User)
                {
                    InvoiceReturnList = unitOfWork.InvoiceRepository.GetInvoiceReturns(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue), PageNo, SharedVariables.PageSize);
                }
                else
                {
                    InvoiceReturnList = unitOfWork.InvoiceRepository.GetInvoiceReturns(PageNo, SharedVariables.PageSize);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = InvoiceReturnList.HasPreviousPage;
                btnNext.Enabled = btnLastPage.Enabled = InvoiceReturnList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, InvoiceReturnList.PageCount);
            }

            foreach (InvoiceReturnVM r in InvoiceReturnList)
            {
                if (r.Patient != null)
                {
                    grdInvoices.Rows.Add(r.InvoiceId, r.InvoiceRefNo, r.Patient.MRNo, r.Patient.Name, r.Total, r.Paid, r.Paid, r.PaymentDate, r.IsProcedureInvoice, r.CreateDate, r.UserName);
                }
                else
                {
                    grdInvoices.Rows.Add(r.InvoiceId, r.InvoiceRefNo, "", "", r.Total, r.Paid, r.Paid, r.PaymentDate, r.IsProcedureInvoice, r.CreateDate, r.UserName);
                }
            }
        }

        private void btnAddNewAdjustment_Click(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            txtSearchByInvoice.Text = "";
            txtGeneralSearch.Text = "";
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            dtpTo.Value = DateTime.Now;
            grdInvoices.Rows.Clear();
            filter = FilterType.Default;
            LoadUsers();
            LoadDrugReturns();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;

        }

        private void txtSearchByInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.PageNo = 1;
                cmbUsers.SelectedIndex = 0;
                if (!txtSearchByInvoice.Text.Trim().Equals(""))
                {
                    filter = FilterType.Invoice;
                }
                else
                {
                    filter = FilterType.Default;
                }
                LoadDrugReturns();
            }
        }

        private void txtGeneralSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearchByInvoice.Text = "";
                cmbUsers.SelectedIndex = 0;
                this.PageNo = 1;
                if (!txtGeneralSearch.Text.Trim().Equals(""))
                {
                    filter = FilterType.Patient;
                }
                else
                {
                    filter = FilterType.Default;
                }
                LoadDrugReturns();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            this.PageNo = 1;
            filter = FilterType.Date;
            LoadDrugReturns();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.PageNo = 1;
            filter = FilterType.Date;
            LoadDrugReturns();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long invoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcInvoice = Convert.ToBoolean(grdInvoices.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoice"].Index)
            {
                if (IsProcInvoice)
                {
                    Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new Reports.ProcedureInvoices.ProcedureInvoiceViewer(invoiceId, true);
                    v.Show();
                }
                else
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(invoiceId, true, true);
                    v.Show();
                }
                //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(invoiceId);
                //v.Show();               
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDrugReturns();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = InvoiceReturnList.PageCount;
            LoadDrugReturns();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDrugReturns();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDrugReturns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    if (_pageNo <= 0 || _pageNo > InvoiceReturnList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDrugReturns();
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchByInvoice.Text = "";
            txtGeneralSearch.Text = "";
            this.PageNo = 1;
            if (cmbUsers.SelectedIndex == 0)
            {
                filter = FilterType.Default;
            }
            else
            {
                filter = FilterType.User;
            }
            LoadDrugReturns();
        }
    }
}