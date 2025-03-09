using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
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
    public partial class StaffUI : Form
    {
        private UnitOfWork unitOfWork;
        int PageNo = 1;
        IPagedList<InvoiceTransactionsVM> Transactions;

        #region LoadDetails
        public StaffUI()
        {
            InitializeComponent();
        }
        private void TransactionUI_Load(object sender, EventArgs e)
        {
            LoadDetials();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh});
            SharedFunctions.SetGridStyle(grdData);
        }
        private void LoadDetials()
        {
            cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
            try
            {
                LoadUsers();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            }
        }
        private void LoadUsers()
        {
            using (unitOfWork = new UnitOfWork())
            {
                List<UserSelectVM> users = new List<UserSelectVM>();
                users = unitOfWork.UserRepository.GetStaffUsers();
                UserSelectVM model = new UserSelectVM();
                model.UserId = 0;
                model.UserName = "Select User";
                users.Insert(0, model);
                cmbUsers.DataSource = users;
                cmbUsers.ValueMember = "UserId";
                cmbUsers.DisplayMember = "UserName";
            }
        }
        private void LoadExpenses()
        {
            try
            {
                grdData.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    Transactions = unitOfWork.InvoiceRepository.GenericSearchForStaffUI
                                   (
                                      dtpFrom.Value,
                                      dtpTo.Value,
                                      SharedFunctions.StringToInt(cmbUsers.SelectedValue),
                                      PageNo,
                                      SharedVariables.PageSize
                                   );
                }
                foreach (InvoiceTransactionsVM p in Transactions)
                {
                    grdData.Rows.Add(p.UserId, p.UserName, p.TotalPaid);
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
            cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            cmbUsers.SelectedIndex = 0;
            LoadExpenses();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
        }
        #endregion

        #region Filters
        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
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
                int UserId = SharedFunctions.StringToInt(grdData.Rows[e.RowIndex].Cells["colUserId"].Value);
                if (UserId <= 0)
                    return;
                if (e.ColumnIndex == grdData.Columns["colDetails"].Index)
                {
                    SharedFunctions.OpenForm(new StaffDetailsUI(UserId, dtpFrom.Value, dtpTo.Value), this.MdiParent);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
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
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
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
            PageNo = Transactions.PageCount;
            LoadExpenses();
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            ReLoadDetails();
        }
    }
}
