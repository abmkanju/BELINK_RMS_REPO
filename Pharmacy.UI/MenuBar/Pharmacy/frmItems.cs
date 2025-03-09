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
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageOfflineTask;
using System.Transactions;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmItems : Form
    {
        private UnitOfWork unitOfWork;
        List<Supplier> LinkedSuppliers = new List<Supplier>();
        private List<Rack> Racks;
        private int ItemId = 0;
        private int AvalStock = 0;
        Service obj = new Service();
        public frmItems()
        {
            InitializeComponent();
        }

        public frmItems(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }
        public frmItems(int ItemId, int AvalStock)
        {
            InitializeComponent();
            this.ItemId = ItemId;
            this.AvalStock = AvalStock;
        }
        private void frmItems_Load(object sender, EventArgs e)
        {
            //Disable CV, strips acc to Total Stock
            //if (this.AvalStock > 0)
            //{
            //    numConvUnit.Enabled = false;
            //}
            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                this.btnAddItem.Text = "Request Item";
            }
            else
            {
                this.btnAddItem.Text = "Add Items";
            }

            //Disable CV, strips acc to Total Stock
            if (this.AvalStock > 0)
            {
                numConvUnit.Enabled = false;
                numStrips.Enabled = false;
            }
            

            txtPctCode.Enabled = SharedVariables.AdminPharmacySetting.IsFBRUnable;
            Racks = new List<Rack>();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddItem, btnCancel, btnClear, btnAddCategory, btnAddManf });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddSupplier, btnAddRack });
            using (unitOfWork = new UnitOfWork())
            {
                LoadSuppliers(unitOfWork);
                LoadCategories(unitOfWork);
                LoadManufacturers(unitOfWork);
                LoadRacks(unitOfWork);
                if (this.ItemId > 0)
                {
                    //lstSuppliers.SelectedIndex = -1;
                    LoadItemData(this.ItemId, unitOfWork);
                }
            }

            if (this.ItemId > 0)
            {

                pnlOpeningStock.Visible = false;
                btnAddItem.Text = "Update";
            }
        }

        private void LoadRacks(UnitOfWork _unitOfWork)
        {
            Racks = _unitOfWork.RackRepository.GetAllActiveRacks();
            Rack r = new Rack();
            r.RackId = 0;
            r.Name = "Select Rack";
            Racks.Insert(0, r);
            cmbRacks.DataSource = Racks;
            cmbRacks.DisplayMember = "Name";
            cmbRacks.ValueMember = "RackId";
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
                MessageBox.Show("An error occurred while loading manufacturers, please try again.", "Manufacturers not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadItemData(int ItemId, UnitOfWork p_unitOfWork)
        {
            try
            {
                Item LoadedItem = new Item();
                LoadedItem = p_unitOfWork.ItemRspository.GetItemData_ByITemId(ItemId);
                txtItemName.Text = LoadedItem.ItemName;
                cmbRacks.SelectedValue = LoadedItem.RackId;
                //if (LoadedItem.Rack != null)
                //{
                //    cmbRacks.SelectedValue = LoadedItem.Rack.RackId;
                //}
                //else
                //{
                //    cmbRacks.SelectedValue = 0;
                //}
                txtBarcode.Text = LoadedItem.Barcode;

                List<Supplier> objSuppliers = LoadedItem.Suppliers.ToList();
                foreach (Supplier s2 in objSuppliers)
                {
                    LinkedSuppliers.Add(s2);
                    GenerateSupplier(s2.Name, s2.SupplierID.ToString());
                    //lstSuppliers.SelectedIndex = lstSuppliers.FindStringExact(s2.Name);
                }
                cmbCategory.SelectedValue = LoadedItem.Category == null ? 0 : LoadedItem.Category.CategoryId;
                cmbManufacturer.SelectedValue = LoadedItem.Manufacturer == null ? 0 : LoadedItem.Manufacturer.ManufacturerId;
                txtUnit.Text = LoadedItem.Unit.ToString();
                txtPctCode.Text = LoadedItem.PCTCode;
                numConvUnit.Value = LoadedItem.ConversionUnit;
                numReOrderingLevel.Value = LoadedItem.ReOrderingLevel;
                txtRetailPrice.Text = LoadedItem.RetailPrice.ToString();
                numOpeningStock.Value = LoadedItem.OpeningStock;
                numStrips.Value = LoadedItem.NoOfStrips;
                txtChemicalName.Text = LoadedItem.ChecmicalName;
                txtUnitCostPrice.Text = LoadedItem.UnitCostPrice.ToString();
                chkNarcotic.Checked = Convert.ToBoolean(LoadedItem.IsNarcotic);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading item data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear()
        {
            //if (ItemId > 0)
            //{
            //    DialogResult rs = MessageBox.Show("You Were About to Update Product. If You Clear, New Product Will be Saved, STILL YOU WANT TO CLEAR??", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            //    if (rs != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //}
            btnAddItem.Text = "Add";
            ItemId = 0;
            txtItemName.Text = "";
            cmbRacks.SelectedIndex = 0;
            txtBarcode.Text = "";
            txtUnit.Text = "Pack";

            cmbCategory.SelectedIndex = 0;
            cmbManufacturer.SelectedIndex = 0;
            numConvUnit.Value = 1;
            numReOrderingLevel.Value = 0;
            numOpeningStock.Value = 0;
            txtChemicalName.Text = "";
            txtUnitCostPrice.Text = "";
            chkNarcotic.Checked = false;

            //remove labelSuplier Tag
            pnlSupplier.Controls.Clear();

            ErrMessage.Visible = false;
            ErrItemName.Visible = false;
            //ErrRackNo.Visible = false;
            ErrBarcode.Visible = false;
            ErrSupplier.Visible = false;
            ErrUnit.Visible = false;
            ErrReOrderingLevel.Visible = false;
            ErrOpeningStock.Visible = false;
            ErrChemicalName.Visible = false;
            ErrUnitCostPrice.Visible = false;
            ErrPctCode.Visible = false;
            ErrNarcotic.Visible = false;
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (SharedFunctions.IsDoubleFormat(SharedFunctions.StringToDouble(txtRetailPrice.Text), "Retail Price"))
            {
                return ErrFound;
            }
            if (SharedFunctions.IsDoubleFormat(SharedFunctions.StringToDouble(txtUnitCostPrice.Text), "Cost Price"))
            {
                return ErrFound;
            }
            if (string.IsNullOrEmpty(txtItemName.Text.Trim()))
            {
                ErrItemName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrItemName.Visible = false;
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
            if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
            {
                if (string.IsNullOrEmpty(txtPctCode.Text))
                {
                    ErrPctCode.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    ErrPctCode.Visible = false;
                }
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
                double unitCostPrice = 0;
                bool success = double.TryParse(txtUnitCostPrice.Text, out unitCostPrice);
                if (!success)
                {
                    lblInvlalidUnitCost.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    lblInvlalidUnitCost.Visible = false;
                }
            }
            if (txtRetailPrice.Text.Trim() == "")
            {
                txtRetailPrice.Text = "0";
            }
            else
            {
                double rp = 0;
                bool success = double.TryParse(txtRetailPrice.Text, out rp);
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

            if (cmbCategory.SelectedIndex <= 0)
            {
                errCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCategory.Visible = false;
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
        private void FillObject(Item objItem)
        {
            objItem.ItemName = txtItemName.Text;
            objItem.Barcode = txtBarcode.Text;
            objItem.Unit = "Pack";
            objItem.PCTCode = txtPctCode.Text;
            int convUnit = Convert.ToInt32(numConvUnit.Value);
            objItem.NoOfStrips = Convert.ToInt32(numStrips.Value);
            if (convUnit != objItem.ConversionUnit && this.ItemId > 0)
            {
                DialogResult res = MessageBox.Show("New conversion unit would not apply on old stocks, consumptions, invoices, adjustments, audits." + Environment.NewLine + " Are you sure you want to update conversion unit?", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    objItem.ConversionUnit = convUnit;
                }
            }
            else
            {
                objItem.ConversionUnit = convUnit;
            }
            objItem.ReOrderingLevel = (int)numReOrderingLevel.Value;
            objItem.OpeningStock = (int)numOpeningStock.Value;
            objItem.ChecmicalName = txtChemicalName.Text;
            objItem.RetailPrice = Math.Round(SharedFunctions.StringToDouble(txtRetailPrice.Text), 3);
            objItem.UnitCostPrice = Math.Round(SharedFunctions.StringToDouble(txtUnitCostPrice.Text), 3);
            objItem.IsNarcotic = chkNarcotic.Checked;
            if (this.ItemId <= 0)
            {
                objItem.IsActive = true;
                objItem.IsNew = true;
                objItem.CreateDate = DateTime.Now;
                objItem.IsSyncable = true;
            }
            objItem.UpdateDate = DateTime.Now;
            objItem.CategoryId = Convert.ToInt64(cmbCategory.SelectedValue);
            objItem.ManufacturerId = Convert.ToInt32(cmbManufacturer.SelectedValue);
        }
        private void LoadSuppliers(UnitOfWork p_unitOfWork)
        {
            List<Supplier> suppliers = new List<Supplier>();
            suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            Supplier supp = new Supplier();
            supp.SupplierID = 0;
            supp.Name = "Select Supplier";
            suppliers.Insert(0, supp);
            cSupplierCombo.DataSource = suppliers;
            cSupplierCombo.ValueMember = "SupplierID";
            cSupplierCombo.DisplayMember = "Name";

            //lstSuppliers.DataSource = suppliers;
            //lstSuppliers.ValueMember = "SupplierID";
            //lstSuppliers.DisplayMember = "Name";
        }

        private void RefreshBulkItems(UnitOfWork _unitOfwork)
        {
            //SharedVariables.BulkLoadedItemsList = _unitOfwork.ItemRspository.GetBulkAllActiveItems();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.ItemId > 0)
                    {
                        UpdateItem();
                        return;
                    }
                    InsertItem();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void InsertItem()
        {
            Response check = new Response();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Item objItem = new Item();
                        Stock objStock = new Stock();
                        FillObject(objItem);
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            this.Cursor = Cursors.WaitCursor;
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

                        //No of strips check
                        if (objItem.ConversionUnit < objItem.NoOfStrips)
                        {
                            MessageBox.Show("No of Strips can't be greater than Conversion Unit", "No of Strips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        int result = -1;
                        result = unitOfWork.ItemRspository.ItemAlreadyExists(objItem.ItemName, objItem.Barcode);

                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                        {
                            if (result == 1)
                            {
                                MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else if (result == 2)
                            {
                                MessageBox.Show("Item With Given Barcode Already Added, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        else if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            obj.getCredential();
                            check = obj.GetItemWithSameName(objItem.ItemName);
                            if (check.status == "Failure")
                            {
                                MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.Cursor = Cursors.Default;
                                return;
                            }
                        }
                        objItem.Suppliers = new List<Supplier>();
                        objItem.Category = unitOfWork.CategoryRepository.GetById(cmbCategory.SelectedValue);
                        objItem.Manufacturer = unitOfWork.ManufacturerRepository.GetById(objItem.ManufacturerId);
                        objItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objItem.Rack = unitOfWork.RackRepository.GetById(cmbRacks.SelectedValue);
                        foreach (Label supp in pnlSupplier.Controls)
                        {
                            Supplier objSupplier = unitOfWork.SupplierRepository.GetById(supp.TabIndex);
                            objItem.Suppliers.Add(objSupplier);
                        }
                        objItem.IsActive = false;
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            objItem.IsDefault = true;
                        }
                        else
                        {
                            objItem.IsDefault = false;

                        }
                        unitOfWork.ItemRspository.Insert(objItem);
                        if (objItem.OpeningStock > 0 &&
                            SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                        {
                            //------------ Audit Log
                            objStock = MakeOpeningStockEntry(objItem);
                            AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + SharedFunctions.StringToInt32(objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.created);
                        }
                        //------------ Audit Log
                        AuditLogTask.CreateAuditLog(unitOfWork, objItem.ItemName, AuditModule.Item, AuditAction.created);
                        unitOfWork.Save();
                        int rackId = SharedFunctions.StringToInt(cmbRacks.SelectedValue);
                        if (rackId > 0)
                        {
                            RackItem model = new RackItem();
                            model.Rack = unitOfWork.RackRepository.GetById(rackId);
                            model.Item = objItem; //item;
                            model.CreatedAt = DateTime.Now;
                            model.UpdatedAt = DateTime.Now;
                            
                            model.IsActive = true;
                            model.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            unitOfWork.RackRepository.InsertRackItem(model);
                            unitOfWork.Save();
                        }
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            objItem.IsApproved = 1;
                            obj.PostApproveItems(objItem);
                        }

                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {
                            MessageBox.Show("Item requested successfully", "Item Processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Cursor = Cursors.Default;
                            this.Close();
                            return;
                        }

                        //SOT
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable == false)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, objItem, ModuleName.Item, ModuleAction.Create, objItem.ItemID);
                            if (objItem.OpeningStock > 0 && objStock != null)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock.StockItems.FirstOrDefault().Batch, ModuleName.Batch, ModuleAction.Create, objStock.StockItems.FirstOrDefault().Batch.BatchId);
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Create, objStock.StockId);
                            }
                        }
                        //RefreshBulkItems(unitOfWork);
                    }
                    ts.Complete();

                    if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
            MessageBox.Show("Item Saved Successfully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void UpdateItem()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Item obj = new Item();
                obj = unitOfWork.ItemRspository.GetItemData_ByITemId(this.ItemId);
                FillObject(obj);

                if (!obj.IsSyncable)
                {
                    obj.IsSyncable = true;
                }
                else if (obj.IsSynced)
                {
                    obj.IsNew = false;
                    obj.IsUpdate = true;
                    obj.IsSynced = false;
                }
                if (obj.ConversionUnit < obj.NoOfStrips)
                {
                    MessageBox.Show("No of Strips can't be greater than Conversion Unit", "No of Strips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int result = -1;

                result = unitOfWork.ItemRspository.ItemAlreadyExists(obj.ItemID, obj.ItemName, obj.Barcode);
                obj.Category = unitOfWork.CategoryRepository.GetById(obj.CategoryId);
                obj.Manufacturer = unitOfWork.ManufacturerRepository.GetById(obj.ManufacturerId);
                obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                obj.Rack = unitOfWork.RackRepository.GetById(cmbRacks.SelectedValue);
                if (result == 1)
                {
                    MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (result == 2)
                {
                    MessageBox.Show("Item With Given Barcode Already Added, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                foreach (Supplier s in LinkedSuppliers)
                {
                    obj.Suppliers.Remove(s);
                }
                obj.Suppliers = new List<Supplier>();
                foreach (Label supp in pnlSupplier.Controls)
                {
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetById(supp.TabIndex);
                    obj.Suppliers.Add(objSupplier);
                }

                int rackId = SharedFunctions.StringToInt(cmbRacks.SelectedValue); //Assing Rack
                if (obj.RackId > 0)
                {
                    if (rackId == 0)
                    {
                        RackItem rackItem = unitOfWork.RackRepository.GetRackItem(obj.RackId);
                        rackItem.Rack = unitOfWork.RackRepository.GetById(rackId);
                        rackItem.DeletedAt = DateTime.Now;
                        rackItem.IsActive = false;
                        rackItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        unitOfWork.RackRepository.UpdateRackItem(rackItem);
                    }
                    else if (obj.RackId != rackId && rackId > 0)
                    {
                        RackItem rackItem = unitOfWork.RackRepository.GetRackItem(obj.RackId);
                        rackItem.Rack = unitOfWork.RackRepository.GetById(rackId);
                        rackItem.UpdatedAt = DateTime.Now;
                        rackItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        unitOfWork.RackRepository.UpdateRackItem(rackItem);
                    }
                }
                else
                {
                    if (rackId > 0)
                    {
                        RackItem model = new RackItem();
                        model.Rack = unitOfWork.RackRepository.GetById(rackId);
                        model.Item = obj;
                        model.CreatedAt = DateTime.Now;
                        model.UpdatedAt = DateTime.Now;
                        model.IsActive = true;
                        model.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        unitOfWork.RackRepository.InsertRackItem(model);
                    }
                }
                AuditLogTask.CreateAuditLog(unitOfWork, obj.ItemName, AuditModule.Item, AuditAction.updated);
                unitOfWork.ItemRspository.Update(obj);
                unitOfWork.Save();
                if (!obj.IsFileItem)
                {
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, obj, ModuleName.Item, ModuleAction.Update, obj.ItemID);
                }
                MessageBox.Show("Item Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }
        private Stock MakeOpeningStockEntry(Item objItem)
        {
            Stock objStock = new Stock();
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.ImagePath = null;
            objStock.StockDate = DateTime.Now;
            objStock.CreateDate = DateTime.Now;
            objStock.UpdateDate = DateTime.Now;
            objStock.IsActive = true;
            objStock.IsNew = true;
            objStock.SupplierIvoiceDate = DateTime.Now;
            objStock.DocumentNo = unitOfWork.StockRepository.GetNewDocumentNumber();
            objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            objStock.StockItems = new List<StockItem>();
            StockItem objStockItem = new StockItem();

            objStockItem.Batch = new Batch();
            objStockItem.Batch.BatchName = "Opening Stock";
            objStockItem.Batch.Expiry = null;
            objStockItem.Batch.CreatedAt = DateTime.Now;
            objStockItem.Batch.UpdatedAt = DateTime.Now;
            objStockItem.Batch.IsNew = true;
            objStockItem.Batch.IsActive = true;
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.UnitCost = objItem.UnitCostPrice;
            objStock.GrandTotal = objStockItem.TotalCost = objStockItem.NetValue = objStockItem.Quantity * objStockItem.UnitCost;
            objStockItem.RetailPrice = objItem.RetailPrice;
            objStockItem.Item = objItem;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
            return objStock;
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
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void txtUnitCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.IsValidDoubleNumerickey(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtRetailPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.IsValidDoubleNumerickey(sender, e);
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            frmAddCategory f = new frmAddCategory();
            f.FormClosed += new FormClosedEventHandler(frmAddCategory_Closed);
            f.ShowDialog();
        }

        private void frmAddCategory_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadCategories(unitOfWork);
            }
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            frmAddRack f = new frmAddRack();
            f.ShowDialog();
            using (unitOfWork = new UnitOfWork())
            {
                LoadRacks(unitOfWork);
            }
        }

        private void btnAddManf_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddManufacturer(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmManf_Closed);
        }

        private void frmManf_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
        }

        private void txtPtcCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private bool isSupplierAdded(int supplierId)
        {
            foreach (Label l in pnlSupplier.Controls)
            {
                if (l.TabIndex.ToString() == supplierId.ToString())
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
            pnlSupplier.Controls.Add(lblTag);
        }

        private void lblTag_DoubleClicked(object sender, EventArgs e)
        {
            Label ClickedLbl = (Label)sender;
            pnlSupplier.Controls.Remove(ClickedLbl);
        }

        private void cSupplierCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var supplierID = 0;
            //get id from object
            if (cSupplierCombo.SelectedItem != null)
            {
                System.Reflection.PropertyInfo getId = cSupplierCombo.SelectedItem.GetType().GetProperty("SupplierID");
                supplierID = Convert.ToInt32(getId.GetValue(cSupplierCombo.SelectedItem, null));
            }

            if (isSupplierAdded(supplierID))
            {
                ToolTip tt = new ToolTip();
                tt.Show("Supplier Already Add", cSupplierCombo, 800);
                return;
            }
            else if(cSupplierCombo.SelectedIndex == -1)
            {
                return;
            }
            else if (cSupplierCombo.SelectedIndex.ToString() != "0")
            {
                GenerateSupplier(cSupplierCombo.Text, supplierID.ToString());
            }
            else
            {
                return;
            }
        }

        private void cSupplierCombo_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void lblInvalidRetailPrice_Click(object sender, EventArgs e)
        {
        }
    }
}