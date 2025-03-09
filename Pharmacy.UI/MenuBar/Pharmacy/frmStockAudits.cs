using PagedList;
using Pharmacy.Core;
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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockAudits : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<StockAuditsScreenVM> audits;
        public frmStockAudits()
        {
            InitializeComponent();
        }

        private void frmStockAudits_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddAudit, btnRefreshForm , btnUpdate});
                SharedFunctions.SetSmallButtonsStyle(new[] { btnPrint, btnExcel });
                SharedFunctions.SetGridStyle(grdItems);
                LoadAudits();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form, please try again", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAudits()
        {
            using(unitOfWork = new UnitOfWork ())
            {
                 audits = unitOfWork.StockAuditRepository.getActiveAudits(pageNo, SharedVariables.PageSize);
            }
            grdItems.Rows.Clear();
            if (audits!=null && audits.Count > 0)
            {
                foreach (var a in audits)
                {
                    grdItems.Rows.Add(a.StockAuditId, a.AuditDate, a.TotalDifference, a.CreatedAt);
                }

                btnPrevious.Enabled = btnFirstPage.Enabled = audits.HasPreviousPage;
                btnNext.Enabled = btnLastPage.Enabled = audits.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, audits.PageCount);
            }
        }

        private void btnAddStockAudit_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddStockAudit(), this.MdiParent);
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if(e.ColumnIndex == grdItems.Columns["colBtnEdit"].Index)
            {
                int id = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAuditId"].Value);
                SharedFunctions.OpenForm(new frmAddStockAudit(id), this.MdiParent);
                return;
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAudits();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo += 1;
            LoadAudits();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = audits.PageCount;
            LoadAudits();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo -= 1;
            LoadAudits();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAudits();
        }

        private void btnAddAudit_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddStockAudit(), this.MdiParent);
        }
    }
}