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
using System.IO;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAllItems : Form
    {
        int PageNo = 1;
        private bool IsHoldingStock = false;
        UnitOfWork unitOfWork;
        IPagedList<ItemsVM> itemsList;

        #region LoadDetails
        public frmAllItems()
        {
            InitializeComponent();
            RenameButtons();
        }
        public frmAllItems(bool refresh)
        {
            InitializeComponent();
            btnRefreshForm.PerformClick();
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            if (SharedVariables.AdminPharmacySetting.ShowRackNoInPOS)
            {
                grdItems.Columns["colRackNo"].Visible = true;
            }

            grdItems.Columns["colPCTCode"].Visible = SharedVariables.AdminPharmacySetting.IsFBRUnable;
            // default items are being loaded on frm activation event.
            SharedFunctions.SetGridStyle(grdItems);
            //SharedFunctions.UpdateCommonStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            LoadFilters();
            LoadItems();
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbStockUnit.SelectedIndexChanged += cmbStockUnit_SelectedIndexChanged;
        }
        private void LoadFilters()
        {
            try
            {
                cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
                cmbStockUnit.SelectedIndexChanged -= cmbStockUnit_SelectedIndexChanged;
                cmbShowItems.SelectedIndexChanged -= cmbShowItems_SelectedIndexChanged;
                cmbRack.SelectedIndexChanged -= cmbRack_SelectedIndexChanged;
                cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
                cmbStockUnit.SelectedIndex = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    loadSuppliers();
                    loadCategories();
                    loadManufacturers();
                    loadSelectedItems();
                    loadRack();
                    loadStore();
                }
                cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
                cmbStockUnit.SelectedIndexChanged += cmbStockUnit_SelectedIndexChanged;
                cmbShowItems.SelectedIndexChanged += cmbShowItems_SelectedIndexChanged;
                cmbRack.SelectedIndexChanged += cmbRack_SelectedIndexChanged;
                cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void loadStore()
        {
            if (!SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                lblStore.Visible = false;
                cmbStore.Visible = false;
                return;
            }
            List<StoreVM> store = new List<StoreVM>();
            store = unitOfWork.StoreRepository.GetAllStores();
            store.Insert(0, new StoreVM { StoreId = 0, Name = "Select Store" });
            cmbStore.DataSource = store;
            cmbStore.ValueMember = "StoreId";
            cmbStore.DisplayMember = "Name";
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
        private void loadSelectedItems()
        {
            Dictionary<int, string> manfs = new Dictionary<int, string>();
            manfs.Add(0, "Select Item");
            manfs.Add(1, "Available Stock");
            manfs.Add(2, "Low Stock");
            manfs.Add(3, "Expired Stock");
            cmbShowItems.DataSource = manfs.ToList();
            cmbShowItems.ValueMember = "Key";
            cmbShowItems.DisplayMember = "Value";
        }
        private void loadRack()
        {
            List<Rack> racks = new List<Rack>();
            racks = unitOfWork.RackRepository.GetAllActiveRacks();
            racks.Insert(0, new Rack { RackId = 0, Name = "Select Rack" });
            cmbRack.DataSource = racks;
            cmbRack.ValueMember = "RackId";
            cmbRack.DisplayMember = "Name";
        }
        private void LoadItems()
        {
            try
            {
                double retail = 0;
                grdItems.Rows.Clear();
                if (SharedVariables.AdminPharmacySetting != null)
                    IsHoldingStock = SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold;
                using (unitOfWork = new UnitOfWork())
                {
                    DateTime? dt = null;
                    if (dtpStockDate.Checked)
                        dt = dtpStockDate.Value;
                    itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems(
                        FilterByName: txtNameFilter.Text.Trim(),
                        RackId: SharedFunctions.StringToInt(cmbRack.SelectedValue),
                        ManunfId: SharedFunctions.StringToInt(cmbManuf.SelectedValue),
                        SuppId: SharedFunctions.StringToInt(cmbSupplier.SelectedValue),
                        CatId: SharedFunctions.StringToInt(cmbCategory.SelectedValue),
                        StockWiseItems: SharedFunctions.StringToInt(cmbShowItems.SelectedIndex),
                        StoreId: !SharedVariables.AdminPharmacySetting.IsEnableStores ? 0 : SharedFunctions.StringToInt(cmbStore.SelectedValue),
                        CreatedDate: dt,
                        PageNo: PageNo,
                        PageSize: SharedVariables.PageSize,
                        IsHoldingStock: IsHoldingStock
                        );
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
                string Low = "";
                foreach (ItemsVM item in itemsList)
                {
                    foreach (Supplier s in item.SuppliersList)
                    {
                        item.Suppliers += s.Name + ", ";
                    }
                    if (!string.IsNullOrEmpty(item.Suppliers))
                    {
                        item.Suppliers = item.Suppliers.Trim().TrimEnd(',');
                    }
                    Low = "";

                    if (item.ConsumedStock < 0) { item.ConsumedStock = System.Math.Abs(item.ConsumedStock); }
                    //-------CalculateAvailableQty----------//
                    SharedFunctions.CalculateItemAvailableQty(item);
                    item.TotalStock = item.TotalStock + item.AdjustedStock;
                    if (item.ReorderingLevel > 0)
                    {
                        Low = item.AvailableStock <= item.ReorderingLevel ? "LOW" : "";
                    }
                    string Expired = item.ExpiredStock > 0 && item.ConsumedStock != item.TotalStock ? "EXPIRED" : "";
                    item.ItemType = item.IsNarcotic ? "Narcotic" : "Drug";
                    if (cmbStockUnit.SelectedIndex == 0)
                    {
                        item.TotalStock = Convert.ToInt32(Math.Floor((double)(item.TotalStock / item.ConversionUnit)));
                        item.AvailableStock = Convert.ToInt32(Math.Floor((double)(item.AvailableStock / item.ConversionUnit)));
                        //avlValue = item.AvailableStock;
                        //avlValue = (avlValue / item.ConversionUnit);
                        item.ExpiredStock = Convert.ToInt32(Math.Floor((double)(item.ExpiredStock / item.ConversionUnit)));
                        item.UnitCost = Math.Floor((double)(item.UnitCost * item.ConversionUnit));
                        //item.RetailPrice = Math.Floor((double)(item.RetailPrice * item.ConversionUnit));
                        retail = item.RetailPrice;
                        item.RetailPrice = retail * item.ConversionUnit;
                    }
                    else
                    {
                        item.Unit = "Units";
                    }
                    if (cmbShowItems.SelectedIndex == 1)
                    {
                        if (item.AvailableStock > 0)
                        {
                            grdItems.Rows.Add(item.ItemId, item.ItemName, item.Barcode, item.PCTCode, item.RackNo, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, item.UnitCost, item.RetailPrice, item.ReorderingLevel, item.TotalStock, item.AvailableStock, Low, item.ExpiredStock, Expired, item.Unit, item.ConversionUnit,  item.NoOfStrips,  item.ConsumedStock);
                        }
                    }
                    else
                    {
                        grdItems.Rows.Add(item.ItemId, item.ItemName, item.Barcode, item.PCTCode, item.RackNo, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, item.UnitCost, item.RetailPrice, item.ReorderingLevel, item.TotalStock, item.AvailableStock, Low, item.ExpiredStock, Expired, item.Unit, item.ConversionUnit, item.NoOfStrips, item.ConsumedStock);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        #region HeaderButtons
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
            if (!SharedFunctions.IsActionallowed("Add Stock"))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadItems();
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
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            LoadFilters();
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            dtpStockDate.Value = DateTime.Now;
            dtpStockDate.Checked = false;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            txtNameFilter.Text = "";
            PageNo = 1;
            LoadItems();
        }
        private List<ItemsVM> GetReportData()
        {
            try
            {
                List<ItemsVM> itemsList = new List<ItemsVM>();
                if (SharedVariables.AdminPharmacySetting != null)
                    IsHoldingStock = SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold;
                using (unitOfWork = new UnitOfWork())
                {
                    DateTime? dt = null;
                    if (dtpStockDate.Checked)
                        dt = dtpStockDate.Value;
                    itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems_Rpt(
                        FilterByName: txtNameFilter.Text.Trim(),
                        RackId: SharedFunctions.StringToInt(cmbRack.SelectedValue),
                        ManunfId: SharedFunctions.StringToInt(cmbManuf.SelectedValue),
                        SuppId: SharedFunctions.StringToInt(cmbSupplier.SelectedValue),
                        CatId: SharedFunctions.StringToInt(cmbCategory.SelectedValue),
                        StockWiseItems: SharedFunctions.StringToInt(cmbShowItems.SelectedIndex),
                        CreatedDate: dt,
                        IsHoldingStock: IsHoldingStock
                        );
                }
                string suppliers = "";
                foreach (ItemsVM i in itemsList)
                {
                    suppliers = "";
                    SharedFunctions.CalculateItemAvailableQty(i);
                    //i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.AdjustedStock;
                    foreach (var s in i.SuppliersList)
                    {
                        suppliers += s.Name + ", ";
                    }
                    suppliers = suppliers.TrimEnd(',');
                    i.Suppliers = suppliers;
                }
                return itemsList;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
                return null;
            }
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
        }
        private void btnEditDetails_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmEditDetails(), this.MdiParent);
        }
        #endregion

        #region Filters
        private void dtpStockDate_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }
        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            //dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged -= cmbShowItems_SelectedIndexChanged;

            //dtpStockDate.Value = DateTime.Today;
            //cmbCategory.SelectedIndex = 0;
            //cmbSupplier.SelectedIndex = 0;
            //cmbManuf.SelectedIndex = 0;
            //cmbShowItems.SelectedIndex = 0;

            //dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged += cmbShowItems_SelectedIndexChanged;

            if (e.KeyData == Keys.Enter)
            {
                //if (txtNameFilter.Text.Trim() != "")
                {
                    PageNo = 1;
                    LoadItems();
                }
            }
        }
        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
            //dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            //cmbShowItems.SelectedIndexChanged -= cmbShowItems_SelectedIndexChanged;

            //txtNameFilter.Text = "";
            //cmbCategory.SelectedIndex = 0;
            //cmbManuf.SelectedIndex = 0;
            //dtpStockDate.Value = DateTime.Today;
            //cmbShowItems.SelectedIndex = 0;

            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
            //dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            //cmbShowItems.SelectedIndexChanged += cmbShowItems_SelectedIndexChanged;

            PageNo = 1;
            LoadItems();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged -= cmbShowItems_SelectedIndexChanged;

            //dtpStockDate.Value = DateTime.Today;
            //txtNameFilter.Text = "";
            //cmbSupplier.SelectedIndex = 0;
            //cmbManuf.SelectedIndex = 0;
            //cmbShowItems.SelectedIndex = 0;

            //dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged += cmbShowItems_SelectedIndexChanged;

            PageNo = 1;
            LoadItems();

        }
        private void cmbManuf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged -= cmbShowItems_SelectedIndexChanged;

            //dtpStockDate.Value = DateTime.Today;
            //txtNameFilter.Text = "";
            //cmbCategory.SelectedIndex = 0;
            //cmbSupplier.SelectedIndex = 0;
            //cmbShowItems.SelectedIndex = 0;

            //dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbShowItems.SelectedIndexChanged += cmbShowItems_SelectedIndexChanged;

            PageNo = 1;
            LoadItems();
        }
        private void cmbShowItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            //cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
            //cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            //cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;

            //dtpStockDate.Value = DateTime.Today;
            //txtNameFilter.Text = "";
            //cmbCategory.SelectedIndex = 0;
            //cmbSupplier.SelectedIndex = 0;

            //dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            //txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            //cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            //cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;

            PageNo = 1;
            LoadItems();
        }
        private void cmbRack_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }
        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }
        #endregion

        #region PaginationButtons
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadItems();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadItems();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = itemsList.PageCount;
            LoadItems();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
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
                    LoadItems();
                }
            }
        }
        #endregion

        #region DataGridEvents
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    string ItemName = SharedFunctions.ObjectToString(grdItems.Rows[e.RowIndex].Cells["colItem"].Value);
                    int TotalStock = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colTotalStock"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            MessageBox.Show("Franchise Items Can't be edited....");
                            return;
                        }
                        if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        frmItems f = new frmItems(ItemId, TotalStock);
                        f.MdiParent = this.MdiParent;
                        f.FormClosed += new FormClosedEventHandler(ChildFormUpdate_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                        if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                     
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        int ConsumedQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumedStock"].Value);
                        if (ConsumedQty > 0)
                        {
                            MessageBox.Show("Item Has Consumed Quantity, it Can't be Deleted", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        using (unitOfWork = new UnitOfWork())
                        {
                            if (unitOfWork.ItemRspository.IsItemInStockOrPurchaseOrder(ItemId).Any(x => x == true))
                            {
                                SharedFunctions.MessageBoxShow("Item exists in purchase order items or stock items", MessageType.Error);
                                return;
                            }
                        }
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            //Item i = unitOfWork.ItemRspository.GetById(ItemId);
                            //i.IsActive = false;
                            //unitOfWork.ItemRspository.Update(i);
                            using (unitOfWork = new UnitOfWork())
                            {
                                //WWW If item have a stock and has one stockitem with the same name
                                //List<Stock> stocks = unitOfWork.ItemRspository.IsItemHaveOneStock(ItemId);
                                //foreach (var stock in stocks)
                                //{
                                //    List<StockItem> stockList = unitOfWork.StockRepository.GetStockItemsFromStock(stock.StockId);
                                //    if (stockList != null && stockList.Count() == 1)
                                //    {
                                //        stock.IsActive = false;
                                //        stock.IsNew = false;
                                //        stock.IsUpdate = false;
                                //        stock.DeletedAt = DateTime.Now;
                                //        unitOfWork.StockRepository.Update(stock);
                                //    }
                                //}
                                //-----------Audit Log
                                AuditLogTask.CreateAuditLog(unitOfWork, ItemName, AuditModule.Item, AuditAction.deleted);
                                unitOfWork.ItemRspository.SetItemDataInactive(ItemId);
                                unitOfWork.Save();
                                //SOT
                                Item objItem = new Item() { ItemID = ItemId };
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objItem, ModuleName.Item, ModuleAction.Delete, ItemId);
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Item Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colView"].Index)
                    {
                        frmItemInfo f = new frmItemInfo(ItemId);
                        f.MdiParent = this.MdiParent;
                        f.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        #endregion

        #region ShortCutKeys
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

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                btnAddMultipleItems.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.I))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddStock.PerformClick();
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
                    grdItems_CellContentClick(grdItems, e);
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
                    grdItems_CellContentClick(grdItems, e);
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
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        private void RenameButtons()
        {
            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                btnAddItem.Text = "Request Item";
                btnAddMultipleItems.Text = "Request Multiple Items";
            }
            else
            {
                btnAddItem.Text = "Add Item";
                btnAddMultipleItems.Text = "add Multiple Items";
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtNameFilter_TextChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }




        #region CommentedCode
        //private List<ItemsVM> GetReportData()
        //{
        //    try
        //    {
        //        List<ItemsVM> itemsList = new List<ItemsVM>();
        //        using (unitOfWork = new UnitOfWork())
        //        {
        //            string FilterString = txtNameFilter.Text.Trim();
        //            if (filter == FilterType.Generic)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter_Rpt(FilterString.ToLower());
        //            }
        //            else if (filter == FilterType.Date)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate_Rpt(dtpStockDate.Value);
        //            }
        //            else if (filter == FilterType.Manufacturer)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByManuf_Rpt(Convert.ToInt32(cmbManuf.SelectedValue));
        //            }
        //            else if (filter == FilterType.Supplier)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataBySupp_Rpt(Convert.ToInt32(cmbSupplier.SelectedValue));
        //            }
        //            else if (filter == FilterType.Category)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByCat_Rpt(Convert.ToInt32(cmbCategory.SelectedValue));
        //            }
        //            else if (filter == FilterType.SelectItems)
        //            {
        //                switch (cmbShowItems.SelectedIndex)
        //                {
        //                    case 1:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithAvailableStock_Rpt(SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold);
        //                        break;
        //                    case 2:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithLowStock_Rpt(SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold);
        //                        break;
        //                    case 3:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithExpiredStock_Rpt();
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }
        //            else
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockData_Rpt();
        //            }
        //        }
        //        string suppliers = "";
        //        foreach (ItemsVM i in itemsList)
        //        {
        //            suppliers = "";
        //            i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
        //            i.TotalStock = i.TotalStock + i.AdjustedStock;
        //            foreach (var s in i.SuppliersList)
        //            {
        //                suppliers += s.Name + ", ";
        //            }
        //            suppliers = suppliers.TrimEnd(',');
        //            i.Suppliers = suppliers;
        //        }
        //        return itemsList;
        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
        //        return null;
        //    }
        //}

        //private void AppendSuppliers(List<ItemsVM> itemsList)
        //{
        //    List<Item_SuppliersVM> itemSupList = repItems.GetSuppliersByItemIds(itemsList);
        //    foreach (ItemsVM i in itemsList)
        //    {
        //        foreach (Item_SuppliersVM ii in itemSupList)
        //        {
        //            if (i.ItemId == ii.ItemId)
        //            {
        //                i.Suppliers += ii.SupplierName.ToString() + ",";
        //            }
        //        }
        //        string suppliers = i.Suppliers.Length > 0 ? i.Suppliers.TrimEnd(',') : "";
        //        i.Suppliers = suppliers;
        //    }
        //}

        //private void LoadItems()
        //{
        //    try
        //    {
        //        grdItems.Rows.Clear();
        //        using (unitOfWork = new UnitOfWork())
        //        {
        //            string FilterString = txtNameFilter.Text.Trim();
        //            if (filter == FilterType.Generic)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower(), PageNo, SharedVariables.PageSize);
        //            }
        //            else if (filter == FilterType.Date)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value, PageNo, SharedVariables.PageSize);
        //            }
        //            else if (filter == FilterType.Manufacturer)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByManuf(Convert.ToInt32(cmbManuf.SelectedValue), PageNo, SharedVariables.PageSize);
        //            }
        //            else if (filter == FilterType.Supplier)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataBySupp(Convert.ToInt32(cmbSupplier.SelectedValue), PageNo, SharedVariables.PageSize);
        //            }
        //            else if (filter == FilterType.Category)
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByCat(Convert.ToInt32(cmbCategory.SelectedValue), PageNo, SharedVariables.PageSize);
        //            }
        //            else if (filter == FilterType.SelectItems)
        //            {
        //                switch (cmbShowItems.SelectedIndex)
        //                {
        //                    case 1:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithAvailableStock(PageNo, SharedVariables.PageSize, SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold);
        //                        break;
        //                    case 2:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithLowStock(PageNo, SharedVariables.PageSize, SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold);
        //                        break;
        //                    case 3:
        //                        itemsList = unitOfWork.ItemRspository.GetItemsWithExpiredStock(PageNo, SharedVariables.PageSize);
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }
        //            else
        //            {
        //                itemsList = unitOfWork.ItemRspository.GetItemsWithStockData(PageNo, SharedVariables.PageSize);
        //            }
        //        }
        //        SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
        //        btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
        //        btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
        //        string Low = "";
        //        foreach (ItemsVM item in itemsList)
        //        {
        //            foreach (Supplier s in item.SuppliersList)
        //            {
        //                item.Suppliers += s.Name + ", ";
        //            }
        //            if (!string.IsNullOrEmpty(item.Suppliers))
        //            {
        //                item.Suppliers = item.Suppliers.Trim().TrimEnd(',');
        //            }
        //            Low = "";
        //            //-------CalculateAvailableQty----------//
        //            SharedFunctions.CalculateItemAvailableQty(item);
        //            item.TotalStock = item.TotalStock + item.AdjustedStock;
        //            if (item.ReorderingLevel > 0)
        //            {
        //                Low = item.AvailableStock <= item.ReorderingLevel ? "LOW" : "";
        //            }
        //            string Expired = item.ExpiredStock > 0 ? "EXPIRED" : "";
        //            item.ItemType = item.IsNarcotic ? "Narcotic" : "Drug";
        //            if (cmbStockUnit.SelectedIndex == 0)
        //            {
        //                item.TotalStock = Convert.ToInt32(Math.Floor((double)(item.TotalStock / item.ConversionUnit)));

        //                item.AvailableStock = Convert.ToInt32(Math.Floor((double)(item.AvailableStock / item.ConversionUnit)));
        //                item.ExpiredStock = Convert.ToInt32(Math.Floor((double)(item.ExpiredStock / item.ConversionUnit)));
        //            }
        //            else
        //            {
        //                item.Unit = "Units";
        //            }
        //            grdItems.Rows.Add(item.ItemId, item.ItemName, item.Barcode, item.RackNo, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, item.ReorderingLevel, item.TotalStock, item.AvailableStock, Low, item.ExpiredStock, Expired, item.Unit, item.ConversionUnit, item.ConsumedStock);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
        //    }
        //}
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}