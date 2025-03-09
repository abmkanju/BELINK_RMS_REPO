using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Reports;
using PagedList;
using Pharmacy.Core;

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class ProcedureList : Form
    {
        int PageNo = 1;
        UnitOfWork unitOfWork;
        IPagedList<Procedure> ProceduresList;
        public ProcedureList()
        {
            InitializeComponent();
        }
        private void btnAddProc_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new AddProcedure(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void LoadProcedures()
        {
            using (unitOfWork = new UnitOfWork())
            {
                ProceduresList = unitOfWork.ProcedureRepository.GetActiveProcedures(txtNameFilter.Text.Trim(), PageNo, SharedVariables.PageSize);
            }
            grdItems.Rows.Clear();
            foreach (Procedure p in ProceduresList)
            {
                grdItems.Rows.Add(p.ProcedureId, p.ProcedureName, p.Price, p.CreatedAt, p.UpdatedAt);
            }
            btnFirstPage.Enabled = btnPrevious.Enabled = ProceduresList.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = ProceduresList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, ProceduresList.PageCount);
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            LoadProcedures();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadProcedures();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadProcedures();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ItemssRpt rpt = new ItemssRpt();
                //rpt.SetDataSource(this.itemsList);
                rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Items Date Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //ItemsViewer f = new ItemsViewer(this.itemsList);
            //f.Show();
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            txtNameFilter.Text = "";
            LoadProcedures();
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //if (txtNameFilter.Text.Trim() != "")
                //{
                LoadProcedures();
                //}
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colProcedureId"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        AddProcedure f = new AddProcedure(ItemId);
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        //int ProcedureId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumedStock"].Value);
                        //if (ConsumedQty > 0)
                        //{
                        //    MessageBox.Show("Item Has Consumed Quantity, it Can't be Deleted", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Procedure?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                using (unitOfWork = new UnitOfWork())
                                {
                                    Procedure p = unitOfWork.ProcedureRepository.GetById(ItemId);
                                    p.UpdatedAt = DateTime.Now;
                                    p.IsActive = false;
                                    p.IsSynced = false;
                                    p.IsUpdate = true;
                                    unitOfWork.Save();
                                    grdItems.Rows.RemoveAt(e.RowIndex);
                                    MessageBox.Show("Procedure Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                SharedFunctions.ShowErrorMessage("Error Occurred While Deleting Procedure, Please Try Again.", ex.Message, "Error");
                            }
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = ProceduresList.PageCount;
            LoadProcedures();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProcedures();
        }
    }
}