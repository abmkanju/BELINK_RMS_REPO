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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmMissedSales : Form
    {
        private UnitOfWork unitOfWork;
        enum FilterType { Default = 1, Date = 2, Item = 3 };
        FilterType filter = FilterType.Default;
        IPagedList<MissedSale> MissedSales;
        int PageNo = 1;
        private int SelectedItemId = 0;
        public frmMissedSales()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void frmMissedSales_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            LoadMissedSales();
        }
        private void uC_SearchItems1_OnItemSelectionChanged()        
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;            
            if (this.SelectedItemId > 0)
            {
                this.PageNo = 1;
                filter = FilterType.Item;
                LoadMissedSales();
            }        
            else
            {
                this.PageNo = 1;
                filter = FilterType.Default;
                LoadMissedSales();
            }
        }

        //protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        //{
        //    if (keyData == (Keys.F5))
        //    {
        //        btnRefresh.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Control | Keys.Shift | Keys.R))
        //    {
        //        btnAddPurchaseOrder.PerformClick();
        //        return true;
        //    }
        //    #region GridView Events
        //    if (keyData == (Keys.Alt | Keys.G))
        //    {
        //        grdMissedSales.Focus();
        //        if (grdMissedSales.Rows.Count >= 1)
        //        {
        //            grdMissedSales.Rows[0].Selected = true;
        //        }
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.V))
        //    {
        //        if (grdMissedSales.SelectedRows.Count > 0)
        //        {
        //            int colIndex = grdMissedSales.Columns["colViewDetail"].Index;
        //            int rowIndex = grdMissedSales.SelectedRows[0].Index;
        //            DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
        //            grdMissedSales_CellContentClick(grdMissedSales, e);
        //            //cmbSelectBatch.Focus();
        //        }
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.A))
        //    {
        //        if (grdMissedSales.SelectedRows.Count > 0)
        //        {
        //            int colIndex = grdMissedSales.Columns["colAddStock"].Index;
        //            int rowIndex = grdMissedSales.SelectedRows[0].Index;
        //            DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
        //            grdMissedSales_CellContentClick(grdMissedSales, e);
        //            //cmbSelectBatch.Focus();
        //        }
        //        return true;
        //    }

        //    if (keyData == (Keys.Alt | Keys.E))
        //    {
        //        if (grdMissedSales.SelectedRows.Count > 0)
        //        {
        //            int colIndex = grdMissedSales.Columns["colEdit"].Index;
        //            int rowIndex = grdMissedSales.SelectedRows[0].Index;
        //            DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
        //            grdMissedSales_CellContentClick(grdMissedSales, e);
        //            //cmbSelectBatch.Focus();
        //        }
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D))
        //    {
        //        if (grdMissedSales.SelectedRows.Count > 0)
        //        {
        //            int colIndex = grdMissedSales.Columns["colDelete"].Index;
        //            int rowIndex = grdMissedSales.SelectedRows[0].Index;
        //            DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
        //            grdMissedSales_CellContentClick(grdMissedSales, e);
        //            //cmbSelectBatch.Focus();
        //        }
        //        return true;
        //    } if (keyData == (Keys.Alt | Keys.P))
        //    {
        //        if (grdMissedSales.SelectedRows.Count > 0)
        //        {
        //            int colIndex = grdMissedSales.Columns["colPrint"].Index;
        //            int rowIndex = grdMissedSales.SelectedRows[0].Index;
        //            DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
        //            grdMissedSales_CellContentClick(grdMissedSales, e);
        //            //cmbSelectBatch.Focus();
        //        }
        //        return true;
        //    }
        //    #endregion GridView Events
        //    if (keyData == (Keys.Control | Keys.D))
        //    {
        //        btnClearAll.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Control | Keys.Shift | Keys.R))
        //    {
        //        btnRefresh.PerformClick();
        //        return true;
        //    }
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
        private void LoadMissedSales()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Default)
                    {
                        MissedSales = unitOfWork.MissedsalesRepository.GetAllActiveMissedSale(this.PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Date)
                    {
                        MissedSales = unitOfWork.MissedsalesRepository.GetMissedSalesBWDate(dtpFrom.Value, dtpTo.Value, this.PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Item)
                    {
                        if (this.SelectedItemId > 0)
                        {
                            MissedSales = unitOfWork.MissedsalesRepository.GetMissedSalesByItem(this.SelectedItemId);
                        }
                    }
                }
                foreach (MissedSale s in MissedSales)
                {
                    grdItems.Rows.Add(s.MissedSaleId, s.Item.ItemID, s.Item.ItemName, s.User.UserName, s.CreatedAt);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = MissedSales.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = MissedSales.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, MissedSales.PageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading purchase orders data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grdMissedSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                {
                    try
                    {
                        DialogResult r = MessageBox.Show("Are you sure you want to delete selected missed sale", "Please make sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if(r != System.Windows.Forms.DialogResult.Yes)
                        { return; }
                        List<MissedSale> ms = new List<MissedSale>();
                        using (unitOfWork = new UnitOfWork())
                        {
                            ms = unitOfWork.MissedsalesRepository.GetAllActiveMissedSalesByItem(itemId);
                            foreach (var s in ms)
                            {
                                s.IsUpdate = true;
                                s.UpdatedAt = DateTime.Now;
                                s.IsActive = false;
                                s.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                                unitOfWork.GetDbContext().Entry(s).State = System.Data.Entity.EntityState.Modified;
                                unitOfWork.MissedsalesRepository.Update(s);
                            }
                            unitOfWork.Save();
                            MessageBox.Show("Missed Sale deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnRefresh.PerformClick();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while deleting missed sale.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (MissedSales.HasNextPage)
            {
                ++PageNo;
                LoadMissedSales();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            --PageNo;
            LoadMissedSales();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = MissedSales.PageCount;
            LoadMissedSales();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadMissedSales();
        }

        private void btnAddPurchaseOrder_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmPurchaseOrder(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(childForm_Closed);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadMissedSales();
        }

        private void frmMissedSales_Shown(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            RefreshForm();
        }

        private void RefreshForm()
        {
            LoadMissedSales();
        }


        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadMissedSales();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnRefresh.PerformClick();
                return true;
            }
            return false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            filter = FilterType.Default;
            this.uC_SearchItems1.SetText = "";
            LoadMissedSales();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
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
                    if (_pageNo <= 0 || _pageNo > MissedSales.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadMissedSales();
                }
            }
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            this.PageNo = 1;
            LoadMissedSales();
        }
    }
}
