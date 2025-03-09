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
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;

namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmWards : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<Ward> Wards;
        int PageNo = 1;
        public frmWards()
        {
            InitializeComponent();
        }

        private void frmWards_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddWard, btnRefreshForm });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnNext, btnLastPage, btnFirstPage, btnPrevious });
            LoadWards();
        }

        private void LoadWards()
        {            
            using(unitOfWork = new UnitOfWork ())
            {
                Wards = unitOfWork.WardRepository.GetWardsWithSubDepartment(PageNo, SharedVariables.PageSize);
            }
            grdWards.Rows.Clear();
            foreach(Ward w in Wards)
            {
                grdWards.Rows.Add(w.WardId, w.Name, w.SubDepratment.Name);
            }
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, Wards.PageCount);
            btnFirstPage.Enabled = btnPrevious.Enabled = Wards.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = Wards.HasNextPage;
        }
        private void btnAddWard_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmWard(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadWards();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Wards.PageCount;
            LoadWards();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadWards();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadWards();
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            this.LoadWards();
        }

        private void grdWards_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long WardId_ = Convert.ToInt64(grdWards.Rows[e.RowIndex].Cells["colWardId"].Value);
            if(e.ColumnIndex == grdWards.Columns["colEdit"].Index)
            {
                Form f= SharedFunctions.OpenForm(new frmWard(WardId_), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
            }
            else if(e.ColumnIndex == grdWards.Columns["colDelete"].Index)
            {
                DialogResult res = MessageBox.Show("Are youo sure, you want to delete this ward?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if(res == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Ward ObjWard = unitOfWork.WardRepository.GetById(WardId_);
                        ObjWard.IsActive = false;
                        ObjWard.IsUpdate = true;
                        ObjWard.IsSynced = false;
                        ObjWard.UpdateDate = DateTime.Now;
                        unitOfWork.WardRepository.Update(ObjWard);
                        unitOfWork.Save();
                        MessageBox.Show("Ward deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error occurred while deleting ward, please try again.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }    
    }
}