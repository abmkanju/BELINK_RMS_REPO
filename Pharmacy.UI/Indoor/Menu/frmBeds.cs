using Pharmacy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmBeds : Form
    {
        IPagedList<Bed> Beds;
        UnitOfWork unitOfWork; 
        int pageNo = 1;
        public frmBeds()
        {
            InitializeComponent();
        }

        private void btnAddBed_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmBed(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void frmBeds_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddBed, btnRefreshForm });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnNext, btnLastPage, btnFirstPage, btnPrevious });
            LoadBeds();
        }

        private void LoadBeds()
        {
            using(unitOfWork = new UnitOfWork ())
            {
                Beds = unitOfWork.BedRepository.GetAllActiveBeds(pageNo, SharedVariables.PageSize);
            }
            grdWards.Rows.Clear();
            foreach(Bed b in Beds)
            {
                grdWards.Rows.Add(b.BedId, b.Ward.WardId, b.Ward.Name, b.RoomNo, b.ChargesPerDay);
            }
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, Beds.PageCount);
            btnFirstPage.Enabled = btnPrevious.Enabled = Beds.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = Beds.HasNextPage;
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadBeds();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo += 1;
            LoadBeds();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = Beds.PageCount;
            LoadBeds();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo -= 1;
            LoadBeds();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadBeds();
        }

        private void grdWards_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long BedId = Convert.ToInt64(grdWards.Rows[e.RowIndex].Cells["colBedId"].Value);
            if (e.ColumnIndex == grdWards.Columns["colDelete"].Index)
            {

                DialogResult res = MessageBox.Show("Are you sure, you want to delete this bed?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Cancel) return;
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Bed obj = unitOfWork.BedRepository.GetById(grdWards.Rows[e.RowIndex].Cells["colBedId"].Value);
                        if (obj != null)
                        {
                            obj.IsActive = false;
                            obj.IsSynced = false;
                            obj.IsUpdate = true;
                            obj.UpdateDate = DateTime.Now;
                            unitOfWork.BedRepository.Update(obj);
                            unitOfWork.Save();
                            MessageBox.Show("Bed deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete selected bed, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            if (e.ColumnIndex == grdWards.Columns["colEdit"].Index)
            {
                frmUpdateBed f = new frmUpdateBed(BedId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                return;
            }
        }        
    }
}