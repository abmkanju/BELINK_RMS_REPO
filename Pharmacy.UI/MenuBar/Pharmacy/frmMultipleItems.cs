using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmMultipleItems : Form
    {
        private UnitOfWork unitOfWork;
        private List<Item> ItemsList = new List<Item>();
        int EditingRowIndex = -1;
        Service obj = new Service();
        public frmMultipleItems()
        {
            InitializeComponent();
        }
        private void frmMultipleItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddItem, btnClearForm, btnClearAll, btnSave });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddSupplier, btnAddCategory, btnAddManufacturer, btnAddRack });
            txtPCTCode.Enabled = SharedVariables.AdminPharmacySetting.IsFBRUnable;
            grdItems.Columns["colPCTCode"].Visible = SharedVariables.AdminPharmacySetting.IsFBRUnable;

            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                btnSave.Text = "Request All Items";
            }

            

            using (unitOfWork = new UnitOfWork())
            {
                LoadSuppliers(unitOfWork);
                LoadCategories(unitOfWork);
                LoadManufacturers(unitOfWork);
                LoadRacks(unitOfWork);
            }
            SharedFunctions.SetGridStyle(grdItems);
        }

        private void LoadRacks(UnitOfWork p_unitOfWork)
        {
            var racks = p_unitOfWork.RackRepository.GetAllActiveRacks();
            Rack r = new Rack { RackId = 0, Name = "Select Rack" };
            racks.Insert(0, r);
            cmbRacks.DataSource = racks;
            cmbRacks.ValueMember = "RackId";
            cmbRacks.DisplayMember = "Name";
        }
        private void LoadCategories(UnitOfWork p_unitOfWork)
        {
            try
            {
                List<Category> cts = new List<Category>();
                cts = p_unitOfWork.CategoryRepository.GetAllActiveCategories();
                Category c = new Category();
                c.CategoryId = 0;
                c.Name = "Select Category";
                cts.Insert(0, c);
                cmbCategory.DataSource = cts;
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories, please try again.", "Categories not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadManufacturers(UnitOfWork p_unitOfWork)
        {
            try
            {
                List<Manufacturer> cts = new List<Manufacturer>();
                cts = p_unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();
                Manufacturer c = new Manufacturer();
                c.ManufacturerId = 0;
                c.Name = "Select Manufacturer";
                cts.Insert(0, c);
                cmbManufacturer.DataSource = cts;
                cmbManufacturer.ValueMember = "ManufacturerId";
                cmbManufacturer.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories, please try again.", "Categories not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.I))
            {
                txtItemName.Focus();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClearForm.PerformClick();
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

            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
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
                    txtItemName.Focus();
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.R))
            {
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    txtItemName.Focus();
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadSuppliers(UnitOfWork p_unitOfWork)
        {
            List<Supplier> suppliers = new List<Supplier>();
            suppliers = p_unitOfWork.SupplierRepository.GetAll().ToList();
            Supplier supp = new Supplier();
            supp.SupplierID = 0;
            supp.Name = "Select Supplier";
            suppliers.Insert(0, supp);
            cSupplierCombo1.DataSource = suppliers;
            cSupplierCombo1.ValueMember = "SupplierID";
            cSupplierCombo1.DisplayMember = "Name";
        }
        private void Clear()
        {
            this.btnAddItem.Text = "Add Item";
            pnlSupplier1.Controls.Clear();
            
            txtItemName.Text = "";
            cmbRacks.SelectedValue = 0;
            txtBarcode.Text = "";
             
            cmbCategory.SelectedIndex = 0;
            cmbManufacturer.SelectedIndex = 0;
            txtUnit.Text = "pack";
            numConvUnit.Value = 1;
            numReOrderingLevel.Value = 0;
            numOpeningStock.Value = 0;
            txtChemicalName.Text = "";
            txtUnitCostPrice.Text = "";
            chkNarcotic.Checked = false;

            ErrMessage.Visible = false;
            ErrItemName.Visible = false;
            ErrBarcode.Visible = false;
            ErrSupplier.Visible = false;
            ErrUnit.Visible = false;
            ErrReOrderingLevel.Visible = false;
            ErrOpeningStock.Visible = false;
            ErrChemicalName.Visible = false;
            ErrUnitCostPrice.Visible = false;
            ErrNarcotic.Visible = false;
            if (EditingRowIndex >= 0)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                EditingRowIndex = -1;
            }
        }
        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            frmAddSupplier fSup = new frmAddSupplier();
            Form f = SharedFunctions.IsFormOpen(fSup.Name);
            if (f == null)
            {
                fSup.MdiParent = this.MdiParent;
                fSup.Show();
                fSup.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
            }
            else
            {
                f.Focus();
            }
        }
        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadSuppliers(unitOfWork);
            }
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtItemName.Text.Trim()))
            {
                ErrItemName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrItemName.Visible = false;
            }

            if (cmbCategory.SelectedIndex <= 0)
            {
                errCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCategory.Visible = false;
            }
            if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
            {
                if (string.IsNullOrEmpty(txtPCTCode.Text))
                {
                    ErrPctCode.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    ErrPctCode.Visible = false;
                }
            }
            if (string.IsNullOrEmpty(txtUnit.Text.Trim()))
            {
                ErrUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrUnit.Visible = false;
            }
            if (numConvUnit.Value < 1)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errConvUnit.Visible = false;
            }
            //if (string.IsNullOrEmpty(txtRackNo.Text.Trim()))
            //{
            //    ErrRackNo.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrRackNo.Visible = false;
            //}
            //if (string.IsNullOrEmpty(txtBarcode.Text.Trim()))
            //{
            //    ErrBarcode.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrBarcode.Visible = false;
            //}
            if (txtUnitCostPrice.Text.Trim() == "")
            {
                txtUnitCostPrice.Text = "0";
            }
            else
            {
                double unit = 0;
                bool success = double.TryParse(txtUnitCostPrice.Text, out unit);
                if (!success)
                {
                    label11.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    label11.Visible = false;
                }
            }

            if (txtRetailPrice.Text.Trim() == "")
            {
                txtRetailPrice.Text = "0";
            }
            else
            {
                double unit = 0;
                bool success = double.TryParse(txtRetailPrice.Text, out unit);
                if (!success)
                {
                    lblInvalidRetailPrice.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    lblInvalidRetailPrice.Visible = false;
                }
            }
            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    Item objItem = new Item();
                    FillObject(objItem);
                    //No of strps check
                    if (objItem.ConversionUnit < objItem.NoOfStrips)
                    {
                        MessageBox.Show("No of Strips can't be greater than Conversion Unit", "No of Strips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    double unitCost = Math.Round(SharedFunctions.StringToDouble(txtUnitCostPrice.Text), 3);
                    double retailPrice = Math.Round(SharedFunctions.StringToDouble(txtRetailPrice.Text), 3);

                    if (txtUnitCostPrice.Text == "0")
                    {
                        MessageBox.Show("Please Add Unit Cost", "Unit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    if (txtRetailPrice.Text == "0")
                    {
                        MessageBox.Show("Please Add Ratail Price", "Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    int Result = ItemNameAlreadyExists(objItem.ItemName, objItem.Barcode);
                    if (Result == 0)
                    {
                        if (EditingRowIndex >= 0)
                        {
                            UpdateRow(objItem);
                        }
                        else
                        {
                            AddItemToGrid(objItem);
                        }
                        Clear();
                    }
                    else if (Result == 1)
                    {
                        MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Result == 2)
                    {
                        MessageBox.Show("Item With Given Barcode Already Added, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                if (SharedVariables.ShowActualExceptionMessages)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void UpdateRow(Item objItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colItemName"].Value = objItem.ItemName;
            r.Cells["colRackId"].Value = objItem.RackId;
            r.Cells["colRackNo"].Value = objItem.RackNo;
            r.Cells["colBarcode"].Value = objItem.Barcode;
            r.Cells["colSuppliers"].Value = objItem.SupplierNames;
            r.Cells["colSupplierIds"].Value = objItem.SupplierIds;

            r.Cells["colManufacturerId"].Value = objItem.ManufacturerId;
            r.Cells["colManufacturer"].Value = objItem.ManufacturerName;

            r.Cells["colCategoryId"].Value = objItem.CategoryId;
            r.Cells["colCategory"].Value = objItem.CategoryName;
            r.Cells["colStrips"].Value = objItem.NoOfStrips;
            r.Cells["colReOrderingLevel"].Value = objItem.ReOrderingLevel;
            r.Cells["colUnit"].Value = objItem.Unit;
            r.Cells["colConvUnit"].Value = objItem.ConversionUnit;
            r.Cells["colRetailPrice"].Value = objItem.RetailPrice;
            r.Cells["colOpeningStock"].Value = objItem.OpeningStock;
            r.Cells["colChemicalName"].Value = objItem.ChecmicalName;
            r.Cells["colUnitCostPrice"].Value = objItem.UnitCostPrice;
            r.Cells["colIsNarcotic"].Value = objItem.IsNarcotic;
            grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        private int ItemNameAlreadyExists(string Name, string Barcode)
        {
            try
            {
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (EditingRowIndex >= 0)
                    {
                        if (r.Cells["colItemName"].Value.ToString() == Name && r.Index != EditingRowIndex)
                        {
                            return 1;
                        }
                        if (r.Cells["colBarcode"].Value.ToString() == Barcode && r.Index != EditingRowIndex && r.Cells["colBarcode"].Value.ToString() != "")
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        if (r.Cells["colItemName"].Value.ToString() == Name)
                        {
                            return 1;
                        }
                        if (r.Cells["colBarcode"].Value.ToString() == Barcode && r.Cells["colBarcode"].Value.ToString() != "")
                        {
                            return 2;
                        }
                    }
                }
                using (unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.ItemRspository.ItemAlreadyExists(Name.ToLower(), Barcode.ToLower());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Ocurred While Checking For Duplicate Values, Please Try Again");
            }
        }
        private void FillObject(Item objItem)
        {
            objItem.ItemName = txtItemName.Text.Trim();
            objItem.RackId = Convert.ToInt32(cmbRacks.SelectedValue);
            objItem.RackNo = cmbRacks.SelectedIndex > 0 ? cmbRacks.GetItemText(cmbRacks.SelectedItem) : "";
            objItem.Barcode = txtBarcode.Text.Trim();
            objItem.Suppliers = new List<Supplier>();
            foreach (Label supp in pnlSupplier1.Controls)
            {
                objItem.SupplierNames += supp.Text + ",";
                objItem.SupplierIds += supp.TabIndex + ",";
            }

            objItem.Unit = txtUnit.Text;
            objItem.PCTCode = txtPCTCode.Text;
            objItem.ConversionUnit = Convert.ToInt32(numConvUnit.Value);
            objItem.ReOrderingLevel = (int)numReOrderingLevel.Value;
            objItem.RetailPrice = double.Parse(txtRetailPrice.Text);
            objItem.OpeningStock = (int)numOpeningStock.Value;
            objItem.ChecmicalName = txtChemicalName.Text.Trim();
            objItem.UnitCostPrice = double.Parse(txtUnitCostPrice.Text);
            objItem.IsNarcotic = chkNarcotic.Checked;
            objItem.SupplierNames = string.IsNullOrEmpty(objItem.SupplierNames) ? "" : objItem.SupplierNames.TrimEnd(',');
            objItem.SupplierIds = string.IsNullOrEmpty(objItem.SupplierIds) ? "" : objItem.SupplierIds.TrimEnd(',');
            objItem.IsActive = true;
            objItem.NoOfStrips = Convert.ToInt32(numStrips.Value);
            objItem.CategoryId = Convert.ToInt64(cmbCategory.SelectedValue);
            objItem.ManufacturerId = Convert.ToInt32(cmbManufacturer.SelectedValue);
            if (objItem.ManufacturerId > 0)
            {
                objItem.ManufacturerName = cmbManufacturer.GetItemText(cmbManufacturer.SelectedItem);
            }
            else
            {
                objItem.ManufacturerName = "";

            }
            if (objItem.CategoryId > 0)
            {
                objItem.CategoryName = cmbCategory.GetItemText(cmbCategory.SelectedItem);
            }
            else
            {
                objItem.CategoryName = "";
            }
        }
        private void FillObject(DataGridViewRow r, Item objItem)
        {
            objItem.ItemName = r.Cells["colItemName"].Value.ToString();
            //objItem.Rack = unitOfWork.RackRepository.GetById(Convert.ToInt32(r.Cells["colRackId"].Value));
            objItem.Barcode = r.Cells["colBarcode"].Value.ToString();
            objItem.SupplierIds += r.Cells["colSupplierIds"].Value.ToString();
            objItem.CategoryId = Convert.ToInt64(r.Cells["colCategoryId"].Value);
            objItem.ManufacturerId = Convert.ToInt32(r.Cells["colManufacturerId"].Value);
            objItem.Unit = r.Cells["colUnit"].Value.ToString();
            objItem.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            objItem.ReOrderingLevel = Convert.ToInt32(r.Cells["colReOrderingLevel"].Value.ToString());
            objItem.OpeningStock = Convert.ToInt32(r.Cells["colOpeningStock"].Value.ToString());
            objItem.ChecmicalName = r.Cells["colChemicalName"].Value.ToString();
            objItem.RetailPrice = Math.Round(SharedFunctions.StringToDouble(r.Cells["colRetailPrice"].Value.ToString()), 3);
            objItem.UnitCostPrice = Math.Round(SharedFunctions.StringToDouble(r.Cells["colUnitCostPrice"].Value.ToString()), 3);
            objItem.IsNarcotic = Convert.ToBoolean(r.Cells["colIsNarcotic"].Value);
            objItem.SupplierIds = objItem.SupplierIds.TrimEnd(',');
            objItem.IsActive = true;
            objItem.NoOfStrips = Convert.ToInt32(r.Cells["colStrips"].Value);
            objItem.IsNew = true;
            objItem.PCTCode = SharedFunctions.ObjectToString(r.Cells["colPCTCode"].Value.ToString());
            objItem.CreateDate = DateTime.Now;
            objItem.UpdateDate = DateTime.Now;
            objItem.IsSyncable = true;
        }
        private void AddItemToGrid(Item objItem)
        {
            object[] ItemData = { objItem.ItemName, objItem.RackId, objItem.RackNo, objItem.Barcode, objItem.SupplierNames, objItem.SupplierIds, objItem.ManufacturerId, objItem.ManufacturerName, objItem.CategoryId, objItem.CategoryName, objItem.Unit, objItem.ConversionUnit, objItem.NoOfStrips ,objItem.ReOrderingLevel, objItem.RetailPrice, objItem.OpeningStock, objItem.PCTCode, objItem.ChecmicalName, objItem.UnitCostPrice, objItem.IsNarcotic };
            grdItems.Rows.Add(ItemData);
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                grdItems.Rows.RemoveAt(e.RowIndex);
                // ItemsList.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                EditItem(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;
                //grdItems.Rows.RemoveAt(e.RowIndex);
                //ItemsList.RemoveAt(e.RowIndex);
            }
        }
        private void EditItem(DataGridViewRow r)
        {
            txtItemName.Text = r.Cells["ColItemName"].Value.ToString();
            cmbRacks.SelectedValue = Convert.ToInt32(r.Cells["colRackId"].Value);
            txtBarcode.Text = r.Cells["colBarcode"].Value.ToString();
            
            //remove labelSuplier Tag
            pnlSupplier1.Controls.Clear();
            string[] SelectedSuppliers = r.Cells["colSupplierIds"].Value.ToString().Split(',');
            for (int a = 0; a < SelectedSuppliers.Length; a++)
            {
                for (int b = 0; b < cSupplierCombo1.Items.Count; b++)
                {
                    if (((Supplier)cSupplierCombo1.Items[b]).SupplierID.ToString() == SelectedSuppliers[a].ToString())
                    {
                        GenerateSupplier(((Supplier)cSupplierCombo1.Items[b]).Name.ToString(), ((Supplier)cSupplierCombo1.Items[b]).SupplierID.ToString());
                    }
                }
            }
            long catId = Convert.ToInt64(r.Cells["colCategoryId"].Value);
            //cmbCategory.SelectedValue = 1;
            cmbCategory.SelectedValue = catId;
            int ManufacId = Convert.ToInt32(r.Cells["colManufacturerId"].Value);
            cmbManufacturer.SelectedValue = catId;
            txtUnit.Text = r.Cells["colUnit"].Value.ToString();
            numConvUnit.Value = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            numReOrderingLevel.Value = Convert.ToDecimal(r.Cells["colReOrderingLevel"].Value);
            txtRetailPrice.Text = r.Cells["colRetailPrice"].Value.ToString();
            numOpeningStock.Value = Convert.ToDecimal(r.Cells["colOpeningStock"].Value);
            txtChemicalName.Text = r.Cells["colChemicalName"].Value.ToString();
            txtUnitCostPrice.Text = r.Cells["colUnitCostPrice"].Value.ToString();
            chkNarcotic.Checked = Convert.ToBoolean(r.Cells["colIsNarcotic"].Value);
        }
        private void RefreshBulkItems(UnitOfWork _unitOfwork)
        {
            //SharedVariables.BulkLoadedItemsList = _unitOfwork.ItemRspository.GetBulkAllActiveItems();
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There are Some Un-Saved Items In Grid, are you Sure you Want to clear", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Cancel)
                {
                    return;
                }
            }
            Clear();
            grdItems.Rows.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Response check = new Response();
            Item objectSet = new Item();
            long userId = SharedVariables.LoggedInUser.UserId;

            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                this.Cursor = Cursors.WaitCursor;
            }

            //Get_Credential
            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                obj.getCredential();
            }
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some New Items", "No Items Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<int> openingStock = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToInt(r.Cells["colOpeningStock"].Value)).Where(x => x > 0).ToList();
            if(openingStock.Count > 1)
            {
                MessageBox.Show("Cann't Add Items More than 50 that has Opening Stock", "No Items Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Stock objStock = new Stock();
                List<Item> objItemList = new List<Item>();
                List<RackItem> objRackItem = new List<RackItem>();
                using (unitOfWork = new UnitOfWork())
                {
                    objStock.Supplier = null;
                    objStock.SupplierInvoiceNo = null;
                    objStock.ImagePath = null;
                    objStock.SupplierIvoiceDate = DateTime.Now;
                    objStock.StockDate = DateTime.Now;
                    objStock.CreateDate = DateTime.Now;
                    objStock.UpdateDate = DateTime.Now;
                    objStock.IsActive = true;
                    objStock.IsNew = true;
                    objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    objStock.DocumentNo = unitOfWork.StockRepository.GetNewDocumentNumber();
                    objStock.StockItems = new List<StockItem>();
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        Item objItem = new Item();
                        FillObject(r, objItem);
                        int result = -1;
                        result = unitOfWork.ItemRspository.ItemAlreadyExists(objItem.ItemName, objItem.Barcode);
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                        {
                            if (result == 1)
                            {
                                MessageBox.Show("Item With Given Name {" + objItem.ItemName + "}  Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else if (result == 2)
                            {
                                MessageBox.Show("Item With Given Barcode {" + objItem.Barcode + "} Already Exists, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        else if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            check = obj.GetItemWithSameName(objItem.ItemName);
                            if (check.status == "Failure")
                            {
                                //if (dt.Rows.Count == 0)
                                //{
                                MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                                //}
                            }
                        }
                     
                        objItem.User = unitOfWork.UserRepository.GetById(userId);
                        objItem.Suppliers = new List<Supplier>();
                        string[] SupplierIds = objItem.SupplierIds.Split(',');
                        for (int a = 0; a < SupplierIds.Length; a++)
                        {
                            int supID = 0;
                            if (int.TryParse(SupplierIds[a], out supID) && supID > 0)
                            {
                                Supplier objSupp = unitOfWork.SupplierRepository.GetById(supID);
                                objItem.Suppliers.Add(objSupp);
                            }
                        }
                        objItem.Category = unitOfWork.CategoryRepository.GetById(objItem.CategoryId);
                        objItem.Manufacturer = unitOfWork.ManufacturerRepository.GetById(objItem.ManufacturerId);
                        if (objItem.OpeningStock > 0)
                        {
                            MakeOpeningStockEntry(objStock, objItem, unitOfWork);
                            objStock.GrandTotal = Math.Round(objStock.StockItems.Sum(x => x.NetValue), 3);
                            AuditLogTask.CreateAuditLog(unitOfWork, "Doucment# " + SharedFunctions.StringToInt32(objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.created);

                        }
                        objItemList.Add(objItem);
                        int rackId = SharedFunctions.StringToInt(r.Cells["colRackId"].Value);
                        if (rackId > 0)
                        {
                            RackItem rackItem = new RackItem();
                            rackItem.IsActive = true;
                            rackItem.Item = objItem;
                            rackItem.Rack = unitOfWork.RackRepository.GetById(rackId);
                            rackItem.User = unitOfWork.UserRepository.GetById(userId);
                            rackItem.CreatedAt = DateTime.Now;
                            rackItem.UpdatedAt = DateTime.Now;
                            objRackItem.Add(rackItem);
                        }
                    }

                    //SOT ---- Manage Items
                    if (objItemList != null && objItemList.Count() > 0)
                    {
                        var itemBatches = SharedFunctions.ChunkBy(objItemList, SharedVariables.ItemBatch);
                        foreach (var item in itemBatches)
                        {
                            unitOfWork.ItemRspository.InsertRange(item);
                            AuditLogTask.CreateAuditLog(unitOfWork, item.Select(x => x.ItemName).ToList(), AuditModule.Item, AuditAction.created);
                            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                            {
                                unitOfWork.Save();
                            }
                            unitOfWork.RackRepository.InsertRackItemRange(objRackItem); //--- Add Rack Range
                            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                            {
                                unitOfWork.Save();
                            }

                            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                            {
                                //SOT
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                            }

                            else
                            {
                                //setItem on Web
                                foreach (var setItem in item)
                                {
                                    //Approve Items
                                    if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                                    {
                                        setItem.IsApproved = 1;
                                        obj.PostApproveItems(setItem);
                                    }
                                }
                            }
                        }
                    }

                    if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                    {
                        MessageBox.Show("Items requested successfully", "Items Processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        this.Close();
                        return;
                    }

                    //SOT ---- Manage Stock
                    if (objStock != null && SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                    {
                        //SOT ---- Manage Batches
                        List<Batch> objBatch = objStock.StockItems.Select(x => x.Batch).ToList();
                        if (objBatch != null && objBatch.Count() > 0)
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, objBatch, ModuleName.BulkBatches, ModuleAction.Create, 0);
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Create, objStock.StockId);
                    }
                }

                if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                {
                    this.Cursor = Cursors.Default;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                string InnerExc = "";
                if (ex.InnerException != null)
                {
                    InnerExc = ex.InnerException.InnerException.Message;
                }
                if (InnerExc.Contains("IX_ItemName"))
                {
                    MessageBox.Show(InnerExc, "Item Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (InnerExc.Contains("IX_Barcode"))
                {
                    MessageBox.Show(InnerExc, "Barcode Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void MakeOpeningStockEntry(Stock objStock, Item objItem, UnitOfWork _unitOfWork)
        {
            StockItem objStockItem = new StockItem();

            objStockItem.Batch = new Batch();
            objStockItem.Batch.BatchName = "Opening Stock";
            objStockItem.Batch.Expiry = null;
            objStockItem.Batch.CreatedAt = DateTime.Now;
            objStockItem.Batch.UpdatedAt = DateTime.Now;
            objStockItem.Batch.IsNew = true;
            objStockItem.Batch.IsActive = true;
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.UnitCost = Math.Round(objItem.UnitCostPrice, 3);
            objStockItem.NetValue = Math.Round(objStockItem.TotalCost = objStockItem.Quantity * objStockItem.UnitCost, 3);
            objStockItem.RetailPrice = Math.Round(objItem.RetailPrice, 3);
            objStockItem.Item = objItem;
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStockItem.IsNew = true;
            objStock.StockItems.Add(objStockItem);
            _unitOfWork.StockRepository.Insert(objStock);
        }
        private void txtUnitCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.IsValidDoubleNumerickey(sender, e);
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmMultipleItemsShortCuts f = new ShortCutDialogs.frmMultipleItemsShortCuts();
            f.ShowDialog();
        }
        private void frmAddCategory_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadCategories(unitOfWork);
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            frmAddCategory f = new frmAddCategory();
            f.FormClosed += new FormClosedEventHandler(frmAddCategory_Closed);
            f.ShowDialog();
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            frmAddManufacturer f = new frmAddManufacturer();
            f.FormClosed += new FormClosedEventHandler(frmManufacturer_Closed);
            f.ShowDialog();
        }

        private void frmManufacturer_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddRack(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(rack_FormClosed);
        }

        private void rack_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadRacks(unitOfWork);
            }
        }
        private bool isSupplierAdded(int supplierID)
        {
            foreach (Label l in pnlSupplier1.Controls)
            {
                if (l.TabIndex.ToString() == supplierID.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        private void GenerateSupplier(string SupplierText, string SupplierIndex)
        {
            Label lblTag = new Label();
            lblTag.AutoSize = true;
            lblTag.BackColor = System.Drawing.SystemColors.Highlight;
            lblTag.ForeColor = System.Drawing.Color.White;
            lblTag.Location = new System.Drawing.Point(333, 354);
            var margin = lblTag.Margin;
            margin.Top = 5;
            lblTag.Margin = margin;
            lblTag.MinimumSize = new System.Drawing.Size(0, 18);
            lblTag.Name = "lblTag";
            lblTag.Size = new System.Drawing.Size(41, 18);
            lblTag.TabIndex = 20;
            lblTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTag.Text = SupplierText;
            lblTag.TabIndex = Convert.ToInt32(SupplierIndex);
            lblTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTag.DoubleClick += new EventHandler(lblTag_DoubleClicked);
            pnlSupplier1.Controls.Add(lblTag);
        }

        private void lblTag_DoubleClicked(object sender, EventArgs e)
        {
            Label ClickedLbl = (Label)sender;
            pnlSupplier1.Controls.Remove(ClickedLbl);
        }

        private void cSupplierCombo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var supplierID = 0;
            //get id from object
            if (cSupplierCombo1.SelectedItem != null)
            {
                //get id from object
                System.Reflection.PropertyInfo getId = cSupplierCombo1.SelectedItem.GetType().GetProperty("SupplierID");
                supplierID = Convert.ToInt32(getId.GetValue(cSupplierCombo1.SelectedItem, null));
            }

            if (isSupplierAdded(supplierID))
            {
                ToolTip tt = new ToolTip();
                tt.Show("Supplier Already Add", cSupplierCombo1, 800);
                return;
            }
            else if (cSupplierCombo1.SelectedIndex == -1)
            {
                return;
            }
            else if (cSupplierCombo1.SelectedIndex.ToString() != "0")
            {
                GenerateSupplier(cSupplierCombo1.Text, supplierID.ToString());
            }
            else
            {
                return;
            }
        }

        private void pnlSupplier1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}