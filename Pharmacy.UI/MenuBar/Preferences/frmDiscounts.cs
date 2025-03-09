using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
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

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmDiscounts : Form
    {
        UnitOfWork unitOfWork;
        public frmDiscounts()
        {
            InitializeComponent();
        }
        int pageNo = 1;
        private IPagedList<FlatDiscount> discounts;
        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddDiscount(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmAddDiscount_Closed);
        }

        private void frmAddDiscount_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void frmDiscounts_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddDiscount, btnRefreshForm, btnPrint, btnExcel });
                SharedFunctions.SetGridStyle(grdItems);
                LoadDiscounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDiscounts()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    discounts = unitOfWork.FlatDiscountRepository.GetAllACtiveDiscounts(pageNo, SharedVariables.PageSize);
                }
                foreach (var d in discounts)
                {
                    grdItems.Rows.Add(d.FlatDiscountId, d.Name, d.Code, d.StartDate.ToShortDateString(), d.EndDate.ToShortDateString(), d.StartTime.ToShortTimeString(), d.EndTime.ToShortTimeString(), d.Discount, d.DiscountType == 0 ? "%" : "Value");
                }
                btnNext.Enabled = btnLastPage.Enabled = discounts.HasNextPage;
                btnPrevious.Enabled = btnFirstPage.Enabled = discounts.HasPreviousPage;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, discounts.PageCount);
            }
            catch (Exception)
            {
                throw new Exception("an error occurred while loading discounts data");
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            this.pageNo = 1;
            LoadDiscounts();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            this.pageNo -= 1;
            LoadDiscounts();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.pageNo += 1;
            LoadDiscounts();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            this.pageNo = this.discounts.PageCount;
            LoadDiscounts();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > discounts.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadDiscounts();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    btnRefreshForm.PerformClick();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            this.pageNo = 1;
            LoadDiscounts();
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int discId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colDiscountId"].Value);
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                frmAddDiscount f = new frmAddDiscount(discId);
                f.MdiParent = this.MdiParent;
                f.FormClosed += new FormClosedEventHandler(frmAddDiscount_Closed);
                f.Show();
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
            {
                try
                {
                    DialogResult r = MessageBox.Show("Are you sur you want to delete this discount", "Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (r == System.Windows.Forms.DialogResult.Yes)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.FlatDiscountRepository.SetInActive(discId);
                            RefreshTodayDiscoutns(unitOfWork);
                        }
                        MessageBox.Show("Discount data deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting discount data, please try again,", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void RefreshTodayDiscoutns(UnitOfWork _unitOfWork)
        {
            SharedVariables.TodayDiscounts = _unitOfWork.FlatDiscountRepository.getTodayDiscounts();
        }
    }
}