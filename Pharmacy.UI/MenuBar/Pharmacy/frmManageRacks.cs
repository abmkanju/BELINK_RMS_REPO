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
using System.Data.Entity;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmManageRacks : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<Rack> racks;
        public frmManageRacks()
        {
            InitializeComponent();
        }

        private void frmRacks_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddRack, btnRefreshForm });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnPrint, btnExcel });
            SharedFunctions.SetGridStyle(grdItems);
            Loadracks();
        }

        private void Loadracks()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //racks = unitOfWork.RackRepository.GetActiveRacks(pageNo, SharedVariables.PageSize);
            }
            grdItems.Rows.Clear();
            if (racks != null && racks.Count > 0)
            {
                foreach (var a in racks)
                {
                    grdItems.Rows.Add(a.RackId, a.Name);
                }

                btnPrevious.Enabled = btnFirstPage.Enabled = racks.HasPreviousPage;
                btnNext.Enabled = btnLastPage.Enabled = racks.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, racks.PageCount);
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                int RackId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colRackId"].Value);
                if (RackId <= 0)
                {
                    SharedFunctions.MessageBoxShow("Unable to get rack detail", MessageType.Warning);
                    return;
                }
                if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                {
                    SharedFunctions.OpenForm(new frmAddRack(RackId), this.MdiParent);
                    return;
                }
                if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Rack?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            Rack rack = unitOfWork.RackRepository.GetRackDetailsById(RackId);
                            if (rack == null)
                            {
                                SharedFunctions.MessageBoxShow("Unable to get rack detail", MessageType.Warning);
                                return;
                            }
                            rack.IsActive = false;
                            rack.Expiry = DateTime.Now;
                            if (rack.RackItem != null && rack.RackItem.Count() > 0)
                            {
                                rack.RackItem.ToList().ForEach(x => { x.IsActive = false; x.DeletedAt = DateTime.Now; });
                            }
                            unitOfWork.Save();
                            //SOT
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }
            return false;
        }
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            Loadracks();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo += 1;
            Loadracks();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = racks.PageCount;
            Loadracks();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo -= 1;
            Loadracks();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            Loadracks();
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddRack(0), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(this.childForm_Closed);
        }

        private void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            this.btnRefreshForm.PerformClick();
        }
    }
}
