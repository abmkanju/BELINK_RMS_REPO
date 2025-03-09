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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmSearchItem : Form
    {
        UnitOfWork unitOfWork;
        public delegate void ItemSelected(int ItemId, int Quantity);
        public ItemSelected OnItemSelected;
        bool checkQty = false;
        public int SelectedItemId = 0;
        int QuantityValue = 0;
        DataView dataView = new DataView();
        private int PageNo = 1;
        int StoreId = 0;
        private IPagedList<BulkItemsVM> itemsList;
        private bool IsStockAudit = false;
        private bool IsInvoiceModule = false;
        bool isAdvancedSearch = false;
        public frmSearchItem(int StoreId = 0, bool IsStockAudit = false, bool IsInvoice = false)
        {
            InitializeComponent();
            this.StoreId = StoreId;
            this.IsStockAudit = IsStockAudit;
            this.IsInvoiceModule = IsInvoice;
        }

        #region LoadItems
        private void frmSearchItem_Load(object sender, EventArgs e)
        {
            if (IsStockAudit)
            {
                grdItems.Columns["colRack"].Visible = true;
                grdItems.Columns["colSuppliers"].Visible = true;
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    DarkMode.EnableDarkMode(this);
                }
            }
            cmbSearchBy.SelectedIndex = 0;
            SharedFunctions.SetGridStyle(grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnClose, btnRefresh });
            unitOfWork = new UnitOfWork();
            LoadDetials();
            LoadItemsGrid();

            chkAvailbleQty.Checked = this.IsInvoiceModule;
        }

        private void LoadDetials()
        {
            try
            {
                cmbManufacturers.SelectedIndexChanged -= cmbManufacturers_SelectedIndexChanged;
                cmbRacks.SelectedIndexChanged -= cmbRacks_SelectedIndexChanged;
                cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
                LoadRacks(unitOfWork);
                LoadSuppliers(unitOfWork);
                LoadManufactures(unitOfWork);
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbManufacturers.SelectedIndexChanged += cmbManufacturers_SelectedIndexChanged;
                cmbRacks.SelectedIndexChanged += cmbRacks_SelectedIndexChanged;
                cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            }
        }

        private void LoadManufactures(UnitOfWork unitOfWork)
        {

            List<Manufacturer> manfacturers = new List<Manufacturer>();
            manfacturers = unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();
            Manufacturer c = new Manufacturer();
            c.ManufacturerId = 0;
            c.Name = "Select Manufacturer";
            manfacturers.Insert(0, c);
            cmbManufacturers.DataSource = manfacturers;
            cmbManufacturers.ValueMember = "ManufacturerId";
            cmbManufacturers.DisplayMember = "Name";
        }

        private void LoadSuppliers(UnitOfWork unitOfWork)
        {
            List<Supplier> suppliers = new List<Supplier>();
            suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            Supplier supplier = new Supplier();
            supplier.SupplierID = 0;
            supplier.Name = "Select Supplier";
            suppliers.Insert(0, supplier);
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "Name";
        }

        private void LoadRacks(UnitOfWork unitOfWork)
        {
            List<Rack> Racks = new List<Rack>();
            Racks = unitOfWork.RackRepository.GetAllActiveRacks();
            Rack r = new Rack();
            r.RackId = 0;
            r.Name = "Select Rack";
            Racks.Insert(0, r);
            cmbRacks.DataSource = Racks;
            cmbRacks.DisplayMember = "Name";
            cmbRacks.ValueMember = "RackId";
        }
        private void LoadItemsGrid()
        {
            try
            {
                grdItems.Rows.Clear();
                int RackId = 0;// SharedFunctions.StringToInt(cmbRacks.SelectedValue);
                int ManufacturerId = 0;// SharedFunctions.StringToInt(cmbManufacturers.SelectedValue);
                int SupplierId = 0;// SharedFunctions.StringToInt(cmbSuppliers.SelectedValue);
                int SeletedIndex = SharedFunctions.StringToInt(cmbSearchBy.SelectedIndex);
                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    itemsList = unitOfWork.ItemRspository.GetActiveFilterItems
                    (
                        txtSearch.Text.ToLower(),
                        isAdvancedSearch ? SeletedIndex : 0,
                        RackId,
                        ManufacturerId,
                        SupplierId,
                        StoreId,
                        PageNo,
                        10,
                        checkQty
                    );
                }
                else
                {
                    itemsList = unitOfWork.ItemRspository.GetActiveFilterItems
                    (
                        txtSearch.Text.ToLower(),
                        isAdvancedSearch ? SeletedIndex : 0,
                        RackId,
                        ManufacturerId,
                        SupplierId,
                        0,
                        PageNo,
                        10,
                        checkQty
                    );
                }
                if (itemsList == null)
                    return;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
                foreach (var i in itemsList)
                {
                    SharedFunctions.CalculateItemAvailableQtyOnSearch(i);
                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                    {
                        //if (i.AvailableStock == 0 && chkAvailbleQty.Checked)
                        //{
                        //    //grdItems.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, string.Join(",", i.SupplierNames), i.RackName, i.CostPrice * i.ConversionUnit, i.RetailPrice * i.ConversionUnit, 1, i.AvailableStock / i.ConversionUnit);
                        //}
                        //else
                        //{
                            grdItems.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, string.Join(",", i.SupplierNames), i.RackName, i.CostPrice * i.ConversionUnit, i.RetailPrice * i.ConversionUnit, 1, i.AvailableStock / i.ConversionUnit);
                        //}
                    }
                    else
                    {
                        //if (i.AvailableStock == 0 && chkAvailbleQty.Checked)
                        //{
                        //    //grdItems.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, string.Join(",", i.SupplierNames), i.RackName, i.CostPrice * i.ConversionUnit, i.RetailPrice * i.ConversionUnit, 1, i.AvailableStock / i.ConversionUnit);
                        //}
                        //else
                        //{
                            i.Unit = "Units";
                            grdItems.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, string.Join(",", i.SupplierNames), i.RackName, i.CostPrice, i.RetailPrice, 1, i.AvailableStock);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region RefreshButton
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I) || keyData == (Keys.Shift | Keys.OemQuestion))
            {
                txtSearch.Focus();
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
            if (keyData == (Keys.Enter))
            {
               int rowIndex = grdItems.SelectedRows[0].Cells["colLoad"].RowIndex;
                if (txtSearch.Focused)
                {
                    rowIndex = grdItems.SelectedRows[0].Cells["colLoad"].RowIndex;
                    grdItems.CurrentCell = grdItems.Rows[rowIndex].Cells["Quantity"];
                }
                else if (grdItems.CurrentCell == grdItems.Rows[rowIndex].Cells["Quantity"])
                {
                    int colIndex = grdItems.Columns["colLoad"].Index;
                    rowIndex = grdItems.SelectedRows[0].Cells["colLoad"].RowIndex;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);

                    QuantityValue = Convert.ToInt32(grdItems.Rows[rowIndex].Cells["Quantity"].Value);
                    grdItems.CurrentCell = grdItems.Rows[rowIndex].Cells["colLoad"];
                    grdItems_CellContentClick(grdItems, e);
                }
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                txtSearch.Text = "";
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.Text = "";
            lblAdvancedSearch.Text = "Advanced Search";
            isAdvancedSearch = false;
            cmbSearchBy.Visible = false;
            cmbSearchBy.SelectedIndex = 0;
            LoadItemsGrid();
            txtSearch.TextChanged += txtSearch_TextChanged;

            #region CommenetedCode
            //try
            //{
            //    grdItems.DataSource = null;
            //    grdItems.Columns.Clear();
            //    DataTable dt = new DataTable();
            //    dt.Columns.Add("ItemId", typeof(int));
            //    dt.Columns.Add("Item Name", typeof(string));
            //    dt.Columns.Add("Generic Name", typeof(string));
            //    dt.Columns.Add("Manufacturer", typeof(string));
            //    dt.Columns.Add("Cost Price", typeof(double));
            //    dt.Columns.Add("Retail Price", typeof(double));

            //    List<BulkItemsVM> bulkList = new List<BulkItemsVM>();
            //    using (unitOfWork = new UnitOfWork())
            //    {
            //        bulkList = unitOfWork.ItemRspository.GetBulkAllActiveItems();
            //    }
            //    foreach (var i in bulkList)
            //    {
            //        if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            //        {
            //            i.AvailableStock = i.TotalStock - (i.ExpiredStock - i.ExpiredConsumedStock) - i.ConsumedStock + i.AdjustedStock;
            //        }
            //        else
            //        {
            //            i.AvailableStock = i.TotalStock - (i.ExpiredStock - i.ExpiredConsumedStock) - i.ConsumedStock + i.AdjustedStock + i.HoldStock;
            //        }
            //        dt.Rows.Add(i.ItemId, i.ItemName, i.GenericName, i.Manufacturer, i.CostPrice, i.RetailPrice);
            //        SharedVariables.BulkLoadedItemsDataView = new DataView(dt);
            //    }
            //    SharedVariables.BulkLoadedItemsList = bulkList;
            //    LoadItemsGrid();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            #endregion
        }
        #endregion

        #region SearchButton
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsGrid();
        }
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsGrid();
        }

        private void cmbRacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsGrid();
        }
        private void cmbManufacturers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsGrid();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["Quantity"];
                grdItems.BeginEdit(true);
            }
        }

        #endregion

        #region DataGridEvents
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ItemId"].Value);
                int Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["Quantity"].Value);
                if (e.ColumnIndex == grdItems.Columns["colLoad"].Index)
                {
                    this.OnItemSelected(itemId, Quantity);
                    txtSearch.Focus();
                }
                if (e.ColumnIndex == grdItems.Columns["colMissedSale"].Index)
                {
                    try
                    {
                        if (!unitOfWork.MissedsalesRepository.ItemMissedSaleExists(itemId))
                        {
                            MissedSale ms = new MissedSale();
                            ms.CreatedAt = DateTime.Now;
                            ms.UpdatedAt = DateTime.Now;
                            ms.IsActive = true;
                            ms.IsSynced = false;
                            ms.IsNew = true;
                            ms.IsUpdate = false;
                            ms.Item = unitOfWork.ItemRspository.GetById(itemId);
                            ms.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            unitOfWork.MissedsalesRepository.Insert(ms);
                            unitOfWork.Save();
                        }
                        MessageBox.Show("Recorded as missed sale.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while recording item as missed sale." + ex.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!this.chkHold.Checked)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("An error occurred while loading item, please try again." + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ItemId"].Value);
            int Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["Quantity"].Value);
            this.OnItemSelected(itemId, Quantity);
            if (!this.chkHold.Checked)
            {
                this.Close();
            }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar)
            //        && !char.IsNumber(e.KeyChar))
            //{
            //    e.Handled = true;
            //}

            //// only allow one decimal point
            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = true;
            //}

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }


            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            var value = grdItems.Columns["Quantity"];
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["Quantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress); // to allow onyl integer input here in the datagridview               
            }
        }

        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colLoad"].Index)
                {
                    int itemId = Convert.ToInt32(grdItems.CurrentRow.Cells["ItemId"].Value);
                    int Quantity = Convert.ToInt32(grdItems.CurrentRow.Cells["Quantity"].Value);
                    if (itemId > 0)
                    {
                        this.OnItemSelected(itemId, Quantity);
                    }
                }
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region FormButtons
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmItemSearchShortCuts f = new ShortCutDialogs.frmItemSearchShortCuts();
            f.ShowDialog();
        }
        #endregion

        #region Pagination
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = itemsList.PageCount;
            LoadItemsGrid();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsGrid();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadItemsGrid();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadItemsGrid();
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
                    LoadItemsGrid();
                }
            }
        }
        #endregion

        private void lblAdvancedSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!isAdvancedSearch)
            {
                lblAdvancedSearch.Text = "General Search";
                isAdvancedSearch = true;
                cmbSearchBy.Visible = true;
            }
            else
            {
                lblAdvancedSearch.Text = "Advanced Search";
                isAdvancedSearch = false;
                cmbSearchBy.Visible = false;
            }
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkAvailbleQty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAvailbleQty.Checked == true)
            {
                checkQty = true;            
            }
            else
            {
                checkQty = false;
            }
            LoadItemsGrid();
        }

        #region CommentedCode
        //if (SharedVariables.BulkLoadedItemsDataView != null)
        //{
        //    SharedVariables.BulkLoadedItemsDataView.RowFilter = "";
        //    grdItems.DataSource = SharedVariables.BulkLoadedItemsDataView;
        //    grdItems.Columns["ItemId"].Visible = false;
        //    grdItems.Columns["Item Name"].MinimumWidth = 100;
        //    grdItems.Columns["Item Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    grdItems.Columns["Generic Name"].MinimumWidth = 100;
        //    grdItems.Columns["Generic Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    grdItems.Columns["Retail Price"].MinimumWidth = 60;
        //    grdItems.Columns["Cost Price"].MinimumWidth = 60;
        //    grdItems.Columns["Retail Price"].Width = 60;
        //    grdItems.Columns["Cost Price"].Width = 60;
        //    foreach (DataGridViewColumn col in grdItems.Columns)
        //    {
        //        if (col.Name != "Quantity")
        //            col.ReadOnly = true;
        //    }
        //    DataGridViewButtonColumn c = new DataGridViewButtonColumn();
        //    c.Name = "colLoad";
        //    c.Text = "Load";
        //    c.HeaderText = "Load";
        //    c.UseColumnTextForButtonValue = true;
        //    c.Width = 60;
        //    grdItems.Columns.Add(c);
        //    DataGridViewButtonColumn c2 = new DataGridViewButtonColumn();
        //    c2.Name = "colMissedSale";
        //    c2.Text = "Missed Sale";
        //    c2.HeaderText = "Missed Sale";
        //    c2.UseColumnTextForButtonValue = true;
        //    c2.Width = 80;
        //    grdItems.Columns.Add(c2);
        //}
        //else
        //{
        //    MessageBox.Show("No items could be found, please press reresh button, to latest items", "Items Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        #endregion

        #region CommenetedCode
        //try
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (itemsLoaded && txtSearch.Text.Trim() == "")
        //        {
        //            return;
        //        }
        //        grdItems.Rows.Clear();
        //        if (txtSearch.Text.Trim() == "")
        //        {
        //            foreach (BulkItemsVM i in SharedVariables.BulkLoadedItems)
        //            {
        //                grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
        //            }
        //            itemsLoaded = true;
        //        }
        //        else
        //        {
        //            var res = SharedVariables.BulkLoadedItems.Where(i => i.ItemName.ToLower().Contains(txtSearch.Text.Trim()));
        //            foreach (BulkItemsVM i in res)
        //            {
        //                grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
        //            }
        //            itemsLoaded = false;
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("An error occurred while performing search, please try again.", "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}
        #endregion

        #region CommenetedCode
        ////grdItems.Rows.Clear();
        //try
        //{
        //    //var res = SharedVariables.BulkLoadedItems.Where(i => i.ItemName.ToLower().StartsWith(txtSearch.Text.Trim()));
        //    //grdItems.Rows.Clear();
        //    //foreach (BulkItemsVM i in res)
        //    //{
        //    //    grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
        //    //}
        //    SharedVariables.BulkLoadedItemsDataView.RowFilter = "[Item Name] Like '" + txtSearch.Text.Trim() + "*' OR [Generic Name] Like '" + txtSearch.Text.Trim() + "*' " + "OR [Manufacturer] Like'" + txtSearch.Text.Trim() + "*' " + "OR [Barcode] ='" + txtSearch.Text.Trim() + "'";
        //    //foreach(var r in dataView)
        //    //{
        //    //    grdItems.Rows.Add(r["ItemId"], r["ItemName"], r[""]);
        //    //}
        //    grdItems.DataSource = SharedVariables.BulkLoadedItemsDataView;
        //    //grdItems.DataSource = dataView;
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("An error occurred while performing search, please try again.", "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}
        #endregion
    }
}