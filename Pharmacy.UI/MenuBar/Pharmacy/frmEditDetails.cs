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
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmEditDetails : Form
    {
        enum FilterType { Default = 1, Generic = 2, Date = 3, Category = 4, Supplier = 5, Manufacturer = 6, Unit = 7 }
        FilterType filter = FilterType.Default;
        int PageNo = 1;
        bool IsFilterApplied = false;
        UnitOfWork unitOfWork;
        IPagedList<ItemsEditVM> itemsList;
        private List<Category> Categories;
        private List<Rack> Racks;


        public frmEditDetails()
        {
            InitializeComponent();
        }

        #region HeaderButtons
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
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
        private void btnAddMultipleItems_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmMultipleItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
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
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            frmAddCategory c = new frmAddCategory();
            c.ShowDialog();
        }
        #endregion

        #region FormLoad
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            //SharedFunctions.UpdateCommonStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            using (unitOfWork = new UnitOfWork())
            {
                LoadFilters(); // this will use instanciated unitofwork object.
                //thsee will be loaded when form gets activated
                //LoadCategories(unitOfWork);
                //LoadItems(unitOfWork);
            }
            SharedFunctions.SetGridStyle(grdItems);
        }
        private void LoadFilters()
        {
            try
            {
                cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;

                loadSuppliers();
                loadCategories();
                loadManufacturers();

                cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading some filters data, please try reloading current form", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void loadSuppliers()
        {
            List<Supplier> sups = new List<Supplier>();
            sups = unitOfWork.SupplierRepository.GetActiveSuppliers();
            sups.Insert(0, new Supplier() { SupplierID = 0, Name = "Select Supplier" });
            cmbSupplier.DataSource = sups;
            cmbSupplier.ValueMember = "SupplierId";
            cmbSupplier.DisplayMember = "Name";

        }
        private void loadCategories()
        {
            List<Category> cats = new List<Category>();
            cats = unitOfWork.CategoryRepository.GetAllActiveCategories();
            cats.Insert(0, new Category { CategoryId = 0, Name = "Select Category" });
            cmbCategory.DataSource = cats;
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DisplayMember = "Name";
        }
        private void loadManufacturers()
        {
            List<Manufacturer> manfs = new List<Manufacturer>();
            manfs = unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();
            manfs.Insert(0, new Manufacturer { ManufacturerId = 0, Name = "Select Manufacturer" });
            cmbManuf.DataSource = manfs;
            cmbManuf.ValueMember = "ManufacturerId";
            cmbManuf.DisplayMember = "Name";
        }
        private void LoadRacks(UnitOfWork p_unitOfWork)
        {
            Racks = p_unitOfWork.RackRepository.GetAllActiveRacks();
        }
        private void LoadCategories(UnitOfWork p_unitOfWork)
        {
            Categories = p_unitOfWork.CategoryRepository.GetAllActiveCategories();
        }
        #endregion

        #region RefreshForm
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                btnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.E))
            {
                btnExcel.PerformClick();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.I))
            {
                btnAddItem.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.G))
            {
                grdItems.Focus();
                if (grdItems.Rows.Count >= 1)
                {
                    grdItems.Rows[0].Selected = true;
                }
                return true;
            }

            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                {
                    int colIndex = grdItems.Columns["colDelete"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.V))
            {
                {
                    int colIndex = grdItems.Columns["colView"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            txtNameFilter.Text = "";
            filter = FilterType.Default;
            using (unitOfWork = new UnitOfWork())
            {
                LoadCategories(unitOfWork);
                LoadRacks(unitOfWork);
                LoadItems(unitOfWork);
            }
            PageNo = 1;
        }

        private void frmAllItems_Activated(object sender, EventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        #endregion

        #region Pagination
        private void btnNext_Click(object sender, EventArgs e)
        {
            //UpdateDetails();
            PageNo += 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            //UpdateDetails();
            PageNo -= 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            //UpdateDetails();
            PageNo = itemsList.PageCount;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            //UpdateDetails();
            PageNo = 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
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
                    if (_pageNo <= 0 || _pageNo > itemsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    using (unitOfWork = new UnitOfWork())
                    {
                        LoadItems(unitOfWork);
                    }
                }
            }
        }
        #endregion

        #region DataGridEvents
        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colCategory"];
            //cell.DataSource = Categories;
            //cell.ValueMember = "CategoryId";
            //cell.DisplayMember = "Name";
        }
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            grdItems.Rows[e.RowIndex].Cells["colIsRowUpdated"].Value = 1;
            if (e.ColumnIndex == grdItems.Columns["colCategory"].Index)
            {
                grdItems.Rows[e.RowIndex].Cells["colCategoryId"].Value = grdItems.Rows[e.RowIndex].Cells["colCategory"].Value;
            }
            else if (e.ColumnIndex == grdItems.Columns["colRackNo"].Index)
            {
                grdItems.Rows[e.RowIndex].Cells["colRackId"].Value = grdItems.Rows[e.RowIndex].Cells["colRackNo"].Value;
            }
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
                        //else
                        //{
                        //    Items = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value);
                        //}
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
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.IsCurrentCellDirty && grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colCategory"].Index
                || grdItems.IsCurrentCellDirty && grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colRackNo"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                    ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                    ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }
        #endregion

        #region Filters
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //if (txtNameFilter.Text.Trim() != "")
                //{
                //    filter = FilterType.Generic;
                //    PageNo = 1;
                //    using (unitOfWork = new UnitOfWork())
                //    {
                //        LoadItems(unitOfWork);
                //    }
                //}
                PageNo = 1;
                using (unitOfWork = new UnitOfWork())
                {
                    LoadItems(unitOfWork);
                }
            }
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;

            //txtNameFilter.Text = "";
            //cmbSupplier.SelectedIndex = 0;
            //cmbManuf.SelectedIndex = 0;

            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;

            //if (cmbCategory.SelectedIndex > 0)
            //{
            //    this.filter = FilterType.Category;
            //}
            //else
            //{
            //    this.filter = FilterType.Default;
            //}

            PageNo = 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;

            //txtNameFilter.Text = "";
            //cmbCategory.SelectedIndex = 0;
            //cmbManuf.SelectedIndex = 0;

            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;

            //if (cmbSupplier.SelectedIndex > 0)
            //{
            //    this.filter = FilterType.Supplier;
            //}
            //else
            //{
            //    this.filter = FilterType.Default; ;
            //}

            PageNo = 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        private void cmbManuf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;

            //txtNameFilter.Text = "";
            //cmbCategory.SelectedIndex = 0;
            //cmbSupplier.SelectedIndex = 0;

            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;

            //if (cmbManuf.SelectedIndex > 0)
            //{
            //    this.filter = FilterType.Manufacturer;
            //}
            //else
            //{
            //    this.filter = FilterType.Default;
            //}

            PageNo = 1;
            using (unitOfWork = new UnitOfWork())
            {
                LoadItems(unitOfWork);
            }
        }
        #endregion

        private void LoadItems(UnitOfWork p_unitOfWork, [CallerMemberName] string memberName = "")
        {
            try
            {
                grdItems.Rows.Clear();
                //Generic search to filter items record 
                itemsList = p_unitOfWork.ItemRspository.GenericSearchForBulkEditItems(
                    txtNameFilter.Text.ToLower(),
                    Convert.ToInt32(cmbCategory.SelectedValue),
                    Convert.ToInt32(cmbManuf.SelectedValue),
                    Convert.ToInt32(cmbSupplier.SelectedValue),
                    PageNo,
                    SharedVariables.PageSize);

                SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
                grdItems.CellValueChanged -= grdItems_CellValueChanged;

                DataGridViewComboBoxColumn c1 = (DataGridViewComboBoxColumn)grdItems.Columns["colCategory"];
                c1.DataSource = this.Categories;
                c1.ValueMember = "CategoryId";
                c1.DisplayMember = "Name";

                DataGridViewComboBoxColumn c2 = (DataGridViewComboBoxColumn)grdItems.Columns["colRackNo"];
                c2.DataSource = this.Racks;
                c2.ValueMember = "RackId";
                c2.DisplayMember = "Name";

                StringBuilder sups = new StringBuilder();
                string supString = "";

                foreach (ItemsEditVM item in itemsList)
                {
                    sups = new StringBuilder();
                    foreach (var s in item.Suppliers)
                    {
                        sups.Append(s.Name + ", ");
                    }
                    supString = sups.ToString().Trim().TrimEnd(',');
                    grdItems.Rows.Add(item.ItemId, item.ItemName, supString, item.CategoryName, item.CategoryId, item.UnitCost, item.RetailPrice, item.Rack == null ? "" : item.Rack.Name, item.Rack == null ? 0 : item.Rack.RackId, item.ReorderingLevel, 0);
                }
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                //if (itemsList.HasNextPage || itemsList.HasPreviousPage)
                //{
                //    btnUpdate.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateDetails();
        }

        private void UpdateDetails()
        {
            try
            {
                List<int> ids = new List<int>();
                List<Item> items = new List<Item>();
                int isUpdated = 0;
                bool isUpdationfound = false;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    isUpdated = Convert.ToInt32(r.Cells["colIsRowUpdated"].Value);
                    if (isUpdated == 1)
                    {
                        isUpdationfound = true;
                        ids.Add(Convert.ToInt32(r.Cells["colItemId"].Value));
                    }
                }
                if (!isUpdationfound)
                {
                    return;
                }
                isUpdated = 0;
                int itemId = 0;
                string ItemName = "";
                int categoryId = 0;
                int unitCost = 0;
                int retailPrice = 0;
                int reOrderingLevel = 0;
                int rackId = 0;

                using (unitOfWork = new UnitOfWork())
                {
                    items = unitOfWork.ItemRspository.getItemsByIds(ids);
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        isUpdated = Convert.ToInt32(r.Cells["colIsRowUpdated"].Value);
                        if (isUpdated == 0) continue;
                        itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                        ItemName = r.Cells["colItemName"].Value.ToString();
                        categoryId = Convert.ToInt32(r.Cells["colCategoryId"].Value);
                        unitCost = Convert.ToInt32(r.Cells["colUnitCost"].Value);
                        retailPrice = Convert.ToInt32(r.Cells["colRetailPrice"].Value);
                        reOrderingLevel = Convert.ToInt32(r.Cells["colReorderingLevel"].Value);
                        rackId = Convert.ToInt32(r.Cells["colRackId"].Value.ToString());
                        if (categoryId <= 0)
                        {
                            MessageBox.Show("No category selected for item " + ItemName + ". Please also make sure for items as well.", "Invalid Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Item item = items.Where(x => x.ItemID == itemId).FirstOrDefault();
                        if (item == null)
                        {
                            SharedFunctions.MessageBoxShow("Unable to get details of this Item (" + ItemName + ")", MessageType.Error);
                            return;
                        }
                        item.Category = unitOfWork.CategoryRepository.GetById(categoryId);
                        item.UnitCostPrice = unitCost;
                        item.RetailPrice = retailPrice;
                        item.ReOrderingLevel = reOrderingLevel;
                        item.Rack = unitOfWork.RackRepository.GetById(rackId);
                        unitOfWork.ItemRspository.Update(item);
                        AuditLogTask.CreateAuditLog(unitOfWork, item.ItemName, AuditModule.Item, AuditAction.updated);
                        //SOT
                        if (!item.IsFileItem)
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.Item, ModuleAction.Update, item.ItemID);
                        grdItems.CellValueChanged -= grdItems_CellValueChanged;
                        r.Cells["colIsRowUpdated"].Value = 0;
                        grdItems.CellValueChanged += grdItems_CellValueChanged;
                    }
                    unitOfWork.Save();
                }
                MessageBox.Show("Item data for current page updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
                //MessageBox.Show("An error while updating data, please try again.");
            }
        }

        private void btnEditMultipleItem_Click(object sender, EventArgs e)
        {
            List<int> ItemIds = new List<int>();
            try
            {
                using (new CursorWait())
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        ItemIds = unitOfWork.ItemRspository.GenericSearchForEditItems(
                                 txtNameFilter.Text.ToLower(),
                                Convert.ToInt32(cmbCategory.SelectedValue),
                                Convert.ToInt32(cmbManuf.SelectedValue),
                                Convert.ToInt32(cmbSupplier.SelectedValue)
                                );
                    }
                }
                //frmEditItemDetails f = new frmEditItemDetails(ItemIds);
                //f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                //f.ShowDialog();
                new frmEditItemDetails(ItemIds).Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }



        #region CommentedCode
        //private void LoadItems(UnitOfWork p_unitOfWork, [CallerMemberName] string memberName = "")
        //{
        //    try
        //    {
        //        grdItems.Rows.Clear();

        //        if (filter == FilterType.Generic)
        //        {
        //            string FilterString = txtNameFilter.Text.Trim();
        //            if (FilterString != "")
        //            {
        //                itemsList = p_unitOfWork.ItemRspository.GetItemsForBulkEdit(FilterString.ToLower(), PageNo, SharedVariables.PageSize);
        //            }
        //        }
        //        else if (filter == FilterType.Supplier)
        //        {
        //            itemsList = p_unitOfWork.ItemRspository.GetItemsForBulkEdit_Sup(Convert.ToInt32(cmbSupplier.SelectedValue), PageNo, SharedVariables.PageSize);
        //        }
        //        else if (filter == FilterType.Category)
        //        {
        //            itemsList = p_unitOfWork.ItemRspository.GetItemsForBulkEdit_Cat(Convert.ToInt32(cmbCategory.SelectedValue), PageNo, SharedVariables.PageSize);
        //        }
        //        else if (filter == FilterType.Manufacturer)
        //        {
        //            itemsList = p_unitOfWork.ItemRspository.GetItemsForBulkEdit_Manuf(Convert.ToInt32(cmbManuf.SelectedValue), PageNo, SharedVariables.PageSize);
        //        }
        //        else // default will be applied
        //        {
        //            itemsList = p_unitOfWork.ItemRspository.GetItemsForBulkEdit(PageNo, SharedVariables.PageSize);
        //        }

        //        SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
        //        btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
        //        btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
        //        grdItems.CellValueChanged -= grdItems_CellValueChanged;

        //        DataGridViewComboBoxColumn c1 = (DataGridViewComboBoxColumn)grdItems.Columns["colCategory"];
        //        c1.DataSource = this.Categories;
        //        c1.ValueMember = "CategoryId";
        //        c1.DisplayMember = "Name";

        //        DataGridViewComboBoxColumn c2 = (DataGridViewComboBoxColumn)grdItems.Columns["colRackNo"];
        //        c2.DataSource = this.Racks;
        //        c2.ValueMember = "RackId";
        //        c2.DisplayMember = "Name";

        //        StringBuilder sups = new StringBuilder();
        //        string supString = "";

        //        foreach (ItemsEditVM item in itemsList)
        //        {
        //            sups = new StringBuilder();
        //            foreach (var s in item.Suppliers)
        //            {
        //                sups.Append(s.Name + ", ");
        //            }
        //            supString = sups.ToString().Trim().TrimEnd(',');
        //            grdItems.Rows.Add(item.ItemId, item.ItemName, supString, item.CategoryName, item.CategoryId, item.UnitCost, item.RetailPrice, item.Rack == null ? "" : item.Rack.Name, item.Rack == null ? 0 : item.Rack.RackId, item.ReorderingLevel, item.ReorderingLevel, 0);
        //        }
        //        grdItems.CellValueChanged += grdItems_CellValueChanged;
        //        if (itemsList.HasNextPage || itemsList.HasPreviousPage)
        //        {
        //            btnUpdate.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
        //    }
        //}
        #endregion

    }
}