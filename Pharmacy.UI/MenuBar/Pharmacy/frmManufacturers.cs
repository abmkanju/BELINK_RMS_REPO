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
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmManufacturers : Form
    {
        bool isDateChanged = false;
        int PageNo = 1;
        bool IsFilterApplied = false;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        UnitOfWork unitOfWork;
        IPagedList<Manufacturer> manufList;

        public frmManufacturers()
        {
            InitializeComponent();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            LoadManufacturers();
            SharedFunctions.SetGridStyle(grdItems);
        }
        private void LoadManufacturers()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    if (IsFilterApplied)
                    {
                        string FilterString = txtNameFilter.Text.Trim();
                        if (FilterString != "")
                        {
                            manufList = unitOfWork.ManufacturerRepository.GetManufacturersByNameFilter(FilterString.ToLower(), PageNo, SharedVariables.PageSize);
                        }
                        //else
                        //{
                        //    manufList = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value, PageNo, SharedVariables.PageSize);
                        //}
                    }
                    else
                    {
                        manufList = unitOfWork.ManufacturerRepository.GetManufacturers(PageNo, SharedVariables.PageSize);
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, manufList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = manufList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = manufList.HasNextPage;
                foreach (Manufacturer m in manufList)
                {
                    grdItems.Rows.Add(m.ManufacturerId, m.Name);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadManufacturers();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadManufacturers();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ItemssRpt rpt = new ItemssRpt();
                rpt.Database.Tables[0].SetDataSource(this.GetReportData());
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }

                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
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
            ItemsViewer f = new ItemsViewer(GetReportData());
            f.Show();
        }

        private List<ItemsVM> GetReportData()
        {
            try
            {
                List<ItemsVM> Items = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (IsFilterApplied)
                    {
                        string FilterString = txtNameFilter.Text.Trim();
                        if (FilterString != "")
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower());
                        }
                        else
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value);
                        }
                    }
                    else
                    {
                        Items = unitOfWork.ItemRspository.GetItemsWithStockData();
                    }
                }
                foreach (ItemsVM i in Items)
                {
                    i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.AdjustedStock;
                }
                return Items;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
                return null;
            }
        }
        private void dtpStockDate_ValueChanged(object sender, EventArgs e)
        {
            IsFilterApplied = true;
            PageNo = 1;
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
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            dtpStockDate.Value = DateTime.Now;
            txtNameFilter.Text = "";
            isDateChanged = false;
            IsFilterApplied = false;
            LoadManufacturers();
            PageNo = 1;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtNameFilter.Text.Trim() != "")
                {
                    isDateChanged = false;
                    IsFilterApplied = true;
                    PageNo = 1;
                    LoadManufacturers();
                }
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int ManufacId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colManufacturerId"].Value);
                    string  ManufactureName = SharedFunctions.ObjectToString(grdItems.Rows[e.RowIndex].Cells["colName"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        frmAddManufacturer f = new frmAddManufacturer(ManufacId);
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                        f.ShowDialog();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Manufacturer?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            //Item i = unitOfWork.ItemRspository.GetById(ItemId);
                            //i.IsActive = false;
                            //unitOfWork.ItemRspository.Update(i);
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.ManufacturerRepository.SetManufacturerDataInactive(ManufacId);
                                AuditLogTask.CreateAuditLog(unitOfWork, ManufactureName, AuditModule.Manufacturer, AuditAction.deleted);
                                unitOfWork.Save();
                                Manufacturer objManufacturer = new Manufacturer() { ManufacturerId = ManufacId };
                                //SOT
                                //OfflineJsonTask.SaveOfflineTask(unitOfWork, objManufacturer, ModuleName.Manufacturer, ModuleAction.Delete, ManufacId);
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Manufacturer Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            PageNo = manufList.PageCount;
            LoadManufacturers();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadManufacturers();
        }

        private void frmAllItems_Activated(object sender, EventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
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
                    if (_pageNo <= 0 || _pageNo > manufList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadManufacturers();
                }
            }
        }

        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadManufacturers();
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            frmAddManufacturer f = new frmAddManufacturer();
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.ShowDialog();
        }
    }
}