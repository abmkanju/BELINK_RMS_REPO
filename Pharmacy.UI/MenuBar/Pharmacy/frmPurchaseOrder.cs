using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPurchaseOrder : Form
    {
        private UnitOfWork unitOfWork;
        PurchaseOrder LoadedPurchaseOrder;
        private bool IsOldOrderLoaded = false;
        long PurchaseOrderId = 0;
        string SrNo;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        private bool IsFormLoading = false;
        private bool IsLoadingLowStocks = false;
        private bool IsLoadingPrvSoldItems = false;
        private long ExpireItemId = 0;
        private bool IsFromNotification = false;
        Item LoadedItemData;

        int SelectedItemId = 0;
        string SelectedItemName = "";
        public frmPurchaseOrder()
        {

            InitializeComponent();
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmPurchaseOrder(long PurchaseOrderId)
        {
            InitializeComponent();
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
            this.PurchaseOrderId = PurchaseOrderId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmPurchaseOrder(long ExpireItemId, bool IsFromNotification)
        {
            InitializeComponent();
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
            this.ExpireItemId = ExpireItemId;
            this.IsFromNotification = IsFromNotification;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            try
            {
                this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
                if (this.SelectedItemId > 0)
                {
                    btnAddItem.PerformClick();
                    
                }
                //this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
                //if (!this.SelectedItemId > 0)
                //{
                //    btnClear.perf
                //    LoadItemData(this.SelectedItemId);
                //}
                //else
                //{
                //    btnClear.PerformClick();
                //    return;
                //}
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading selected item data", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        private void LoadItemData(int ItemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadedItemData = unitOfWork.ItemRspository.GetById(ItemId);
                int avQty = unitOfWork.ItemRspository.GetItemAvailableQty(ItemId);

                cmbUnit.Items.Add(LoadedItemData.Unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndexChanged -= cmbUnit_SelectedIndexChanged;
                cmbUnit.SelectedIndex = 0;
                cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
                if (cmbUnit.SelectedIndex == 0)
                {
                    numAvailableQty.Value = (decimal)Math.Floor((double)avQty / LoadedItemData.ConversionUnit);
                }
                else
                {
                    numAvailableQty.Value = avQty;
                }
                numConversionUnit.Value = LoadedItemData.ConversionUnit;
                numUnitCost.Value = (decimal)LoadedItemData.UnitCostPrice;
            }
        }

        private void LoadSerial()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                grdItems.Rows[row.Index].Cells["colSrNo"].Value = string.Format("{0}  ", row.Index + 1).ToString();
            }
        }
        private void LoadSerialLoad()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                SrNo = string.Format("{0}  ", row.Index + 1).ToString();
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
                this.uC_SearchItems1.Focus();
                return true;
            }
            if (keyData == (Keys.F1))
            {
                btnSearchItems.PerformClick();
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
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
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
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (this.ActiveControl.Name == grdItems.Name)
                {
                    if (grdItems.Rows != null && grdItems.Rows.Count > 0)
                    {
                        DataGridViewRow r = grdItems.SelectedRows[0];
                        grdItems.Rows[0].Selected = true;
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                    }
                }
                else
                {
                    if (grdItems.Rows != null && grdItems.Rows.Count > 0)
                    {
                        this.ActiveControl = grdItems;
                        grdItems.Rows[0].Selected = true;
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                        grdItems.CurrentCell.Selected = true;
                    }
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

            if (keyData == (Keys.Alt | Keys.R))
            {
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == Keys.Enter)
            {
                if (grdItems.CurrentCell != null)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                        {
                            uC_SearchItems1.SetFocus();
                            //grdItems.CurrentCell = this.grdItems[0, 0];

                        }
                    }));
                }
            }
            #endregion
            return base.ProcessCmdKey(ref msg, keyData);
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
            LoadSuppliers();
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                uC_SearchItems1.Enabled = false;
                //cmbItems.Enabled = false;                
            });

            //if (SharedVariables.POSItems == null)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            }
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbItems.DataSource = Items;
            cmbItems.ValueMember = "ItemId";
            cmbItems.DisplayMember = "ItemName";
            cmbItems.SelectedIndex = -1;
            //cmbItems.Enabled = true;
            uC_SearchItems1.Enabled = true;
        }
        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> suppliers = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                suppliers = unitOfWork.SupplierRepository.GetActiveSuppliers();
            }
            suppliers.Insert(0, new Supplier { SupplierID = 0, Name = "Select Supplier" });
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            cmbSuppliers.SelectedIndex = 0;
            //if (cmbSuppliers.Items.Count > 0)
            //{
            //    cmbSuppliers.SelectedIndex = 0;
            //    cmbSuppliers_SelectedIndexChanged(null, null);
            //}
        }
        private void setNumericUD_toEmpty()
        {
            numQuantity.ResetText();
            numUnitCost.ResetText();
        }
        private void LoadItems()
        {
            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
            //Thread worker = new Thread(() => { LoadItems_Async(); });
            //worker.Name = "ItemsLoader";
            //worker.IsBackground = true;
            //worker.Start(); 
        }
        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                SharedFunctions.SetGridStyle(grdItems);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll, btnLoadLSI, btnSupAI, btnSupLSI, btnLoadPrvSoldItems });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnClear, btnAddSupplier });
                LoadSuppliers();
                //LoadItems(); // this is using background worker
                if (this.PurchaseOrderId > 0)
                {
                    // PurchaseOrderItemsList.Clear();
                    LoadOrderByOrderId(this.PurchaseOrderId);
                    IsOldOrderLoaded = true;
                    txtGrandTotal.Text = GetGrandTotals().ToString();
                }
                else if (ExpireItemId > 0 && IsFromNotification)
                {
                    LoadItemByItemIdId(ExpireItemId);
                    ShowNewPurchaseOrderNumber();
                }
                else
                {
                    ShowNewPurchaseOrderNumber();
                }
                setNumericUD_toEmpty();
                grdItems.CellValueChanged += grdItems_CellValueChanged;

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

        private void LoadItemByItemIdId(long PoId)
        {
            try
            {
                this.SelectedItemId = (int)PoId;
                if (this.SelectedItemId > 0)
                {
                    btnAddItem.PerformClick();
                }
                IsFromNotification = false;
                ExpireItemId = 0;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        List<Manufacturer> LinkedManufecturers = new List<Manufacturer>();
        private void LoadOrderByOrderId(long PoId)
        {
            this.IsFormLoading = true;
            LoadedPurchaseOrder = new PurchaseOrder();
            using (unitOfWork = new UnitOfWork())
            {
                LoadedPurchaseOrder = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(PoId);
            }
            txtPurchaseOrderNo.Text = LoadedPurchaseOrder.PurchaseOrderNo.ToString();
            if (LoadedPurchaseOrder.Supplier != null)
            {
                cmbSuppliers.SelectedValue = LoadedPurchaseOrder.Supplier.SupplierID;
            }
            else
            {
                cmbSuppliers.SelectedIndex = -1;
            }
            List<Manufacturer> objManufecturer = LoadedPurchaseOrder.Manufecturers.ToList();
            foreach (Manufacturer s2 in objManufecturer)
            {
                LinkedManufecturers.Add(s2);
                GenerateSupplier(s2.Name, s2.ManufacturerId.ToString());
                //lstSuppliers.SelectedIndex = lstSuppliers.FindStringExact(s2.Name);
            }
            txtTotalAmount.Text = LoadedPurchaseOrder.TotalAmount.ToString();
            dtDeliveryDate.Value = LoadedPurchaseOrder.DeliveryDate;
            foreach (PurchaseOrderItem poi in LoadedPurchaseOrder.PurchaseOrderItems)
            {
                if (poi.IsActive)
                {
                    AddItemToGrid(poi, (decimal)LoadedPurchaseOrder.PurchaseOrderNo);
                }
            }
            this.IsFormLoading = false;
        }

        private void ShowNewPurchaseOrderNumber()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtPurchaseOrderNo.Text = unitOfWork.PurchaseOrderRepository.GetNewPurchaseOrderNumber().ToString();
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count > 0)
                {
                    DialogResult rs = MessageBox.Show("There are Some Un-Saved Items In Grid, are you Sure you Want to clear", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                unitIndexCombo.SelectedIndex = -1;
                cmbSuppliers.SelectedIndex = -1;
                grdItems.Rows.Clear();
                dtpPurchaseOrderDate.Value = DateTime.Today;
                IsOldOrderLoaded = false;
                this.PurchaseOrderId = 0;
                LoadedPurchaseOrder = null;
                txtGrandTotal.Text = 0.ToString();
                Clear();
                ShowNewPurchaseOrderNumber();
                this.IsLoadingLowStocks = false;
                this.IsLoadingPrvSoldItems = false;
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
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //long SupplierID = Convert.ToInt64(cmbSuppliers.SelectedValue);
            //cmbItems.DataSource = null;
            //if (SupplierID >= 0)
            //{
            //    IEnumerable<Item> SupplierItems `= unitOfWork.ItemRspository.GetItemsBySupplierID(SupplierID);
            //    cmbItems.DataSource = SupplierItems;
            //    cmbItems.ValueMember = "ItemID";
            //    cmbItems.DisplayMember = "ItemName";
            //}
            //if (cmbItems.Items.Count > 0)
            //{
            //    cmbItems.SelectedIndex = 0;
            //}
        }
        private void Clear()
        {
            numQuantity.ValueChanged -= numQuantity_ValueChanged;
            numUnitCost.ValueChanged -= numUnitCost_ValueChanged;
            cmbItems.SelectedIndex = -1;
            cmbUnit.SelectedIndex = -1;
            cmbUnit.Items.Clear();
            numConversionUnit.Value = 0;
            numQuantity.Value = 0;
            numUnitCost.Value = 0;
            numAvailableQty.Value = 0;
            numAvailableQty.ResetText();
            setNumericUD_toEmpty();
            txtTotalAmount.Text = "";
            //cmbItems.Enabled = true;
            uC_SearchItems1.Enabled = true;
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            EditingRowIndex = -1;
            numQuantity.ValueChanged += numQuantity_ValueChanged;
            numUnitCost.ValueChanged += numUnitCost_ValueChanged;
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = "";
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.SelectedItemId = 0;
            LoadedItemData = new Item();
            this.uC_SearchItems1.SetFocus();
            //PurchaseOrderItemsList.Clear();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (IsValidInput())
                //{
                //    PurchaseOrderItem objPItem = new PurchaseOrderItem();
                //    FillObject(objPItem);
                //    if (EditingRowIndex >= 0)
                //    {
                //        UpdateRow(objPItem);
                //    }
                //    elseawewdwdawdA   
                //    {
                //        AddItemToGrid(objPItem);
                //    }
                //    Clear();
                //}
                if (ItemAlreadyAdded(this.SelectedItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                {
                    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //uC_SearchItems1.SetText = "";                       
                    uC_SearchItems1.SetFocus();
                    return;
                }
                if (this.SelectedItemId <= 0)
                    return;
                grdItems.Rows.Add(0, 0, this.SelectedItemId);
                calculateTotal();
                //if (this.SelectedItemId > 0)
                //{
                //    grdItems.Rows.Add(0, this.SelectedItemId);
                //}
                //else
                //{
                //    MessageBox.Show("Please select an item before proceeding.", "Select Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
               
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
            this.uC_SearchItems1.SetFocus();
            grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];
            grdItems.BeginEdit(true);
            int index = grdItems.CurrentRow.Index;
            grdItems.FirstDisplayedScrollingRowIndex = index;
        }
        private void UpdateRow(PurchaseOrderItem objPItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colUnit"].Value = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            r.Cells["colConvUnit"].Value = objPItem.ConversionUnit;
            r.Cells["colQuantity"].Value = objPItem.Quantity;
            r.Cells["colUnitCost"].Value = objPItem.UnitCost;
            r.Cells["colGrossAmount"].Value = objPItem.TotalAmount;
            r.DefaultCellStyle.BackColor = Color.White;
        }
        private void AddItemToGrid(PurchaseOrderItem objPItem)
        {
            if (ItemAlreadyExists(objPItem.ItemID)) return;
            object[] objPurchaseItem = { objPItem.PurchaseOrderItemId, objPItem.ItemID, objPItem.ItemName, objPItem.Manufecturer == null ? "" : objPItem.Manufecturer.ManufacturerId.ToString(), objPItem.Manufecturer == null ? "" : objPItem.Item.Manufacturer.Name, objPItem.UnitString, objPItem.ConversionUnit, objPItem.Quantity, objPItem.UnitCost, objPItem.GrossAmount ,  objPItem.Discount, objPItem.DiscountType == 0 ? "%" : "Value", objPItem.Bonus, objPItem.TotalAmount };
            grdItems.Rows.Add(objPurchaseItem);
            //PurchaseOrderItemsList.Add(objPItem);
        }
        private void AddItemToGrid(PurchaseOrderItem objItem, decimal PurchaseOrderNo)
        {
            PurchaseOrderItem objPItem = new PurchaseOrderItem();
            //FillObject(objPItem, objItem, PurchaseOrderNo);
            if (ItemAlreadyExists(objItem.Item.ItemID)) return;
            objItem.UnitString = objItem.Unit == 0 ? "Pack" : "Units";

            objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity / objItem.Item.ConversionUnit : objItem.Quantity;
            objItem.UnitCost = objItem.Unit == 0 ? objItem.UnitCost * objItem.Item.ConversionUnit : objItem.UnitCost;
            //objItem.Quantity = objPItem.Unit == 0 ? objItem.Quantity / objItem.Item.ConversionUnit : objItem.Quantity;
            //objItem.UnitCost = objPItem.Unit == 0 ? objItem.UnitCost * objItem.Item.ConversionUnit : objItem.UnitCost;

            LoadSerialLoad();
            object[] objPurchaseItem = { SrNo, objItem.PurchaseOrderItemId, objItem.Item.ItemID, objItem.Item.ItemName, objItem.Item.Manufacturer == null ? "" : objItem.Item.Manufacturer.ManufacturerId.ToString(), objItem.Item.Manufacturer == null ? "" : objItem.Item.Manufacturer.Name, objItem.UnitString, objItem.Item.ConversionUnit, 0, objItem.Quantity, objItem.UnitCost, objItem.GrossAmount, objItem.Discount, objItem.DiscountType == 0 ? "%" : "Value", objItem.Bonus, objItem.TotalAmount };
            grdItems.Rows.Add(objPurchaseItem);
            calculateTotal();
            ///PurchaseOrderItemsList.Add(objPItem);
        }
        private bool ItemAlreadyExists(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    //MessageBox.Show("Selected Item is Already Added, Please Laod that one", "Item Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //grdItems.Rows[r.Index].Selected = true;
                    //return true;
                }
            }
            return false;
        }
        private void FillObject(PurchaseOrderItem objPItem)
        {
            objPItem.PurchaseOrderItemId = Convert.ToInt64(txtPurchaseOrderItemId.Text);
            objPItem.PurchaseOrderNo = decimal.Parse(txtPurchaseOrderNo.Text);
            objPItem.Item = new Item();
            objPItem.ItemID = this.SelectedItemId;// Convert.ToInt32(cmbItems.SelectedValue);
            objPItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);\
            objPItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objPItem.ConversionUnit = (int)numConversionUnit.Value;
            objPItem.Quantity = (int)numQuantity.Value;
            objPItem.UnitCost = (double)numUnitCost.Value;
            objPItem.TotalAmount = double.Parse(txtTotalAmount.Text);
            //objPItem.SupplierId = Convert.ToInt64(cmbSuppliers.SelectedValue);
        }
        private void FillObject(PurchaseOrderItem objPItem, PurchaseOrderItem objLoadedItem, decimal PurchaseOrderNo)
        {
            objPItem.PurchaseOrderItemId = objLoadedItem.PurchaseOrderItemId;
            objPItem.PurchaseOrderNo = PurchaseOrderNo;
            objPItem.Item = new Item();
            objPItem.ItemID = objLoadedItem.Item.ItemID;
            objPItem.ItemName = objLoadedItem.Item.ItemName;
            objPItem.Quantity = objLoadedItem.Quantity;
            objPItem.UnitCost = objLoadedItem.UnitCost;
            objPItem.TotalAmount = objLoadedItem.TotalAmount;
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            //if (cmbSuppliers.SelectedIndex < 0)
            //{
            //    ErrSupplier.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrSupplier.Visible = false;
            //}
            if (this.SelectedItemId <= 0)//(cmbItems.SelectedIndex < 0)
            {
                ErrItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrItem.Visible = false;
            }
            if (this.cmbUnit.SelectedIndex < 0)//(cmbItems.SelectedIndex < 0)
            {
                errUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errUnit.Visible = false;
            }
            if (numQuantity.Value <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
            }

            if (numConversionUnit.Value <= 0)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errConvUnit.Visible = false;
            }
            //if (numUnitCost.Value <= 0)
            //{
            //    ErrUnitCost.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrUnitCost.Visible = false;
            //}
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
        private void numUnitCost_ValueChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }
        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Add Some Purchase Items", "Empty Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //foreach (DataGridViewRow r in grdItems.Rows)
                //{
                //    if (Convert.ToString(r.Cells["colQuantity"].Value) == "0")
                //    {
                //        MessageBox.Show("Please Add Some Quanity in an Item, Quanity can't be 0", "Empty Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }
                //}
                //if (grdItems.Rows.Count > 50)
                //{
                //    MessageBox.Show("Cann't add more than 50 items.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                if (IsOldOrderLoaded)
                {
                    if ((from DataGridViewRow r in grdItems.Rows select r.Visible).All(x => x == false))
                    {
                        MessageBox.Show("Please Add Some Purchase Items", "Empty Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    UpdatePurchaseOrder(this.PurchaseOrderId);
                }
                else
                {
                    SaveNewPurchaseOrder();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void SaveNewPurchaseOrder()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    List<Item> addItems = new List<Item>();
                    List<Item> updateItems = new List<Item>();
                    using (unitOfWork = new UnitOfWork())
                    {
                        PurchaseOrder objPOrder = new PurchaseOrder();
                        objPOrder.PurchaseOrderNo = decimal.Parse(txtPurchaseOrderNo.Text);
                        objPOrder.OrderDate = dtpPurchaseOrderDate.Value;
                        objPOrder.DeliveryDate = dtDeliveryDate.Value;
                        objPOrder.Supplier = unitOfWork.SupplierRepository.GetById(Convert.ToInt64(cmbSuppliers.SelectedValue));
                        objPOrder.PurchaseOrderItems = new List<PurchaseOrderItem>();
                        objPOrder.IsActive = true;
                        objPOrder.IsNew = true;
                        objPOrder.Manufecturers = new List<Manufacturer>();
                        objPOrder.CreateDate = DateTime.Now;
                        objPOrder.UpdateDate = DateTime.Now;
                        foreach (Label manuf in pnlManufecturers.Controls)
                        {
                            Manufacturer objManufecturers = unitOfWork.ManufacturerRepository.GetById(manuf.TabIndex);
                            objPOrder.Manufecturers.Add(objManufecturers);
                        }
                        decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
                        objPOrder.TotalAmount = Math.Round(grandTotal, 3);// decimal.Parse(txtGrandTotal.Text);
                        objPOrder.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        foreach (DataGridViewRow r in grdItems.Rows.Cast<DataGridViewRow>().Where(x => SharedFunctions.StringToInt(x.Cells["colQuantity"].Value) > 0).ToList())
                        {
                            PurchaseOrderItem objPItem = new PurchaseOrderItem();
                            Item objItem = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                            if (objItem.IsFileItem)
                            {
                                addItems.Add(objItem);
                                objItem.IsFileItem = false;
                                unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                            }
                            objPItem.Item = objItem;
                            if(r.Cells["colManufId"].Value != "")
                            {
                                objPItem.Manufecturer = unitOfWork.ManufacturerRepository.GetById(Convert.ToInt32(r.Cells["colManufId"].Value)); 
                            }
                            objPItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                            objPItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                            objPItem.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                            objPItem.Quantity = objPItem.Unit == 0 ? objPItem.Quantity * objPItem.ConversionUnit : objPItem.Quantity; // to save quantity always in broken form.
                            objPItem.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
                            objPItem.UnitCost = Math.Round(objPItem.Unit == 0 ? objPItem.UnitCost / objPItem.ConversionUnit : objPItem.UnitCost, 3); // to save quantity always in broken form.
                            objPItem.TotalAmount = Convert.ToDouble(r.Cells["colTotalAmount"].Value);
                            objPItem.PurchaseOrderNo = (decimal)objPOrder.PurchaseOrderNo;
                            objPItem.IsActive = r.Visible;
                            objPItem.IsNew = true;
                            objPItem.CreateDate = DateTime.Now;
                            objPItem.UpdateDate = DateTime.Now;
                            objPItem.Discount = Convert.ToDouble(r.Cells["colDisc"].Value);
                            objPItem.DiscountType = r.Cells["colDiscType"].Value.ToString() == "%" ? 0 : 1;
                            objPItem.GrossAmount = Convert.ToDouble(r.Cells["colGrossAmount"].Value);
                            objPItem.Bonus = Convert.ToInt32(r.Cells["colBonus"].Value);
                            objPOrder.PurchaseOrderItems.Add(objPItem);
                        }
                        unitOfWork.PurchaseOrderRepository.Insert(objPOrder);
                        AuditLogTask.CreateAuditLog(unitOfWork, "PO# " + objPOrder.PurchaseOrderNo, AuditModule.Purchase_Order, AuditAction.created);
                        unitOfWork.Save();
                        //SOT
                        if (addItems != null && addItems.Count() > 0)
                        {
                            var itemBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                            foreach (var item in itemBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                            }
                        }
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objPOrder, ModuleName.PurchaseOrder, ModuleAction.Create, objPOrder.PurchaseOrderId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }

            MessageBox.Show("Purchase Order Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void UpdatePurchaseOrder(long PurchaseOrderId)
        {
            List<Item> addItems = new List<Item>();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        PurchaseOrder objPOrder = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);
                        objPOrder.OrderDate = dtpPurchaseOrderDate.Value;
                        objPOrder.DeliveryDate = dtDeliveryDate.Value;
                        objPOrder.Supplier = unitOfWork.SupplierRepository.GetById(Convert.ToInt64(cmbSuppliers.SelectedValue));
                        decimal GrandTotal = decimal.Parse(txtGrandTotal.Text);
                        objPOrder.TotalAmount = Math.Round(GrandTotal, 3);
                        if (objPOrder.IsSynced)
                        {
                            objPOrder.IsNew = false;
                            objPOrder.IsUpdate = true;
                            objPOrder.IsSynced = false;
                        }
                        objPOrder.UpdateDate = DateTime.Now;
                        objPOrder.Manufecturers = new List<Manufacturer>();
                        foreach (Label manuf in pnlManufecturers.Controls)
                        {
                            Manufacturer objManufecturers = unitOfWork.ManufacturerRepository.GetById(manuf.TabIndex);
                            objPOrder.Manufecturers.Add(objManufecturers);
                        }
                        objPOrder.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        long PurchaseOrderItemId = -1;
                        bool IsRowDeleted = false;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            PurchaseOrderItemId = Convert.ToInt64(r.Cells["colPurchaseOrderItemId"].Value);
                            PurchaseOrderItemId = PurchaseOrderItemId == 0 ? -1 : PurchaseOrderItemId; // because default value of PurchaseOrderItemId  is 0
                            IsRowDeleted = r.Visible ? false : true;
                            PurchaseOrderItem objPItem = new PurchaseOrderItem();
                            Item objItem = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                            if (objItem.IsFileItem)
                            {
                                addItems.Add(objItem);
                                objItem.IsFileItem = false;
                                unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                            }
                            objPItem.Item = objItem;
                            if (r.Visible)
                            {
                                if (r.Cells["colManufId"].Value != "")
                                {
                                    objPItem.Manufecturer = unitOfWork.ManufacturerRepository.GetById(Convert.ToInt32(r.Cells["colManufId"].Value));
                                }
                            }
                            objPItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);

                            objPItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                            objPItem.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                            objPItem.Quantity = objPItem.Unit == 0 ? objPItem.Quantity * objPItem.ConversionUnit : objPItem.Quantity; // to save quantity always in broken form.

                            objPItem.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
                            objPItem.UnitCost = Math.Round(objPItem.Unit == 0 ? objPItem.UnitCost / objPItem.ConversionUnit : objPItem.UnitCost, 3); // to save quantity always in broken form.
                            if (SharedFunctions.IsDoubleFormat(objPItem.UnitCost, "Purchase Order Unit Price"))
                                return;
                            objPItem.TotalAmount = Convert.ToDouble(r.Cells["colTotalAmount"].Value);
                            objPItem.Discount = Convert.ToDouble(r.Cells["colDisc"].Value);
                            objPItem.DiscountType = r.Cells["colDiscType"].Value.ToString() == "%" ? 0 : 1;
                            objPItem.GrossAmount = Convert.ToDouble(r.Cells["colGrossAmount"].Value);
                            objPItem.Bonus = Convert.ToInt32(r.Cells["colBonus"].Value);
                            objPItem.PurchaseOrderNo = (decimal)objPOrder.PurchaseOrderNo;
                            objPItem.IsNew = true;
                            objPItem.IsActive = r.Visible;
                            objPItem.CreateDate = DateTime.Now;
                            objPItem.UpdateDate = DateTime.Now;
                            if (PurchaseOrderItemId > 0)
                            {
                                foreach (PurchaseOrderItem i in objPOrder.PurchaseOrderItems)
                                {
                                    if (i.PurchaseOrderItemId == PurchaseOrderItemId)
                                    {
                                        i.Item = objPItem.Item;
                                        i.Quantity = objPItem.Quantity;
                                        i.Manufecturer = objPItem.Manufecturer;
                                        i.UnitCost = objPItem.UnitCost;
                                        i.Discount = objPItem.Discount;
                                        i.DiscountType = objPItem.DiscountType;
                                        i.Bonus = objPItem.Bonus;
                                        i.GrossAmount = objPItem.GrossAmount;
                                        i.TotalAmount = objPItem.TotalAmount;
                                        i.Unit = objPItem.Unit;
                                        i.IsUpdate = true;
                                        i.IsSynced = false;
                                        i.UpdateDate = DateTime.Now;
                                        if (!r.Visible || IsRowDeleted)
                                        {
                                            i.IsActive = false;
                                        }
                                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                            }
                            else
                            {
                                objPOrder.PurchaseOrderItems.Add(objPItem);
                            }
                        }
                        AuditLogTask.CreateAuditLog(unitOfWork, "PO# " + objPOrder.PurchaseOrderNo, AuditModule.Purchase_Order, AuditAction.updated);
                        unitOfWork.PurchaseOrderRepository.Update(objPOrder);
                        unitOfWork.Save();
                        //SOT
                        if (addItems != null && addItems.Count() > 0)
                        {
                            var itemBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                            foreach (var item in itemBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                            }
                        }
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objPOrder, ModuleName.PurchaseOrder, ModuleAction.Update, objPOrder.PurchaseOrderId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
            MessageBox.Show("Purchase Order Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void FillItemInfo(DataGridViewRow rItem)
        {
            try
            {
                //cmbSuppliers.SelectedValue = Convert.ToInt64(rItem.Cells["colSupplierId"].Value.ToString());
                txtPurchaseOrderItemId.Text = rItem.Cells["colPurchaseOrderItemId"].Value.ToString();
                //cmbItems.SelectedValue = Convert.ToInt32(rItem.Cells["colItemID"].Value.ToString());
                this.SelectedItemId = Convert.ToInt32(rItem.Cells["colItemID"].Value.ToString());
                using (unitOfWork = new UnitOfWork())
                {
                    string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
                    cmbUnit.Items.Add(unit);
                    cmbUnit.Items.Add("Units");
                }
                //cmbUnit.SelectedIndex = rItem.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                numConversionUnit.Value = Convert.ToInt32(rItem.Cells["colConvUnit"].Value);
                this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                this.uC_SearchItems1.SetText = this.SelectedItemName = rItem.Cells["colItemName"].Value.ToString();
                this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;

                numQuantity.Value = Convert.ToInt32(rItem.Cells["colQuantity"].Value);
                numUnitCost.Value = Convert.ToDecimal(rItem.Cells["colUnitCost"].Value);
                txtTotalAmount.Text = rItem.Cells["colGrossAmount"].Value.ToString();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }



        private void calculateTotal()
        {
            double TotalCost = 0;
            int Quantity = (int)numQuantity.Value;
            double UnitCost = (double)numUnitCost.Value;
            TotalCost = Quantity * UnitCost;
            double GrandTotals = GetGrandTotals();
            txtTotalAmount.Text = TotalCost.ToString();
            txtGrandTotal.Text = GrandTotals.ToString();
        }

        private double GetGrandTotals()
        {
            double TotalAmount = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Visible)
                {
                    if (r.Index != EditingRowIndex)
                    {
                        TotalAmount += Convert.ToDouble(r.Cells["colTotalAmount"].Value);
                    }
                }
            }
            return TotalAmount;
        }

        private void cmbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numUnitCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmPurchaseOrderShortCuts f = new ShortCutDialogs.frmPurchaseOrderShortCuts();
            f.ShowDialog();
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedItemId > 0)
                {
                    int avQty = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        avQty = unitOfWork.ItemRspository.GetItemAvailableQty(this.SelectedItemId);
                    }
                    if (cmbUnit.SelectedIndex == 0)
                    {
                        numAvailableQty.Value = (decimal)Math.Floor((double)avQty / LoadedItemData.ConversionUnit);
                    }
                    else
                    {
                        numAvailableQty.Value = avQty;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnLoadLSI_Click(object sender, EventArgs e)
        {
           try
            {
                if (cmbSuppliers.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadLowStockItems(Convert.ToInt32(cmbSuppliers.SelectedValue));
                }

                this.IsLoadingLowStocks = true;
                grdItems.Rows.Clear();
                foreach (ItemsVM i in lst)
                {
                    if (i.AvailableStock <= i.ReorderingLevel)
                    {
                        grdItems.Rows.Add(0, 0, i.ItemId, i.ItemName, i.Manufacturer == null ? "" : i.Manufacturer.ManufacturerId.ToString(), i.Manufacturer == null ? "" : i.Manufacturer.Name, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0 , 0, "%", 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading low stock items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.IsLoadingLowStocks = false;
            }
        }

        private void PopulateTotals()
        {
            double tCost = 0;
            double TotalCost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Visible)
                {
                    tCost = SharedFunctions.StringToDouble(r.Cells["colTotalAmount"].Value);
                    TotalCost += tCost;
                }
            }
            txtGrandTotal.Text = TotalCost.ToString();
        }
        // load low stock items for selected supplier
        private void btnSupLSI_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSuppliers.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadSup_LowStockItems(Convert.ToInt32(cmbSuppliers.SelectedValue));
                }
                this.IsLoadingLowStocks = true; // this variable is being used in Rows_added event.
                grdItems.Rows.Clear();
                foreach (ItemsVM i in lst)
                {
                    if (i.AvailableStock < i.ReorderingLevel)
                    {
                        grdItems.Rows.Add(0, 0, i.ItemId, i.ItemName, i.Manufacturer == null ? "" : i.Manufacturer.ManufacturerId.ToString(), i.Manufacturer == null ? "" : i.Manufacturer.Name, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0  , 0, "%" , 0, 0);
                    }
                }
                this.IsLoadingLowStocks = false;
            }
            catch (Exception)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading low stock items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // load all items for selected supplier
        private void btnSupAI_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSuppliers.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadSupItems(Convert.ToInt32(cmbSuppliers.SelectedValue));
                }
                grdItems.Rows.Clear();
                this.IsLoadingLowStocks = true; // this variable is being used in Rows_added event.
                foreach (ItemsVM i in lst)
                {
                    grdItems.Rows.Add(0, 0, i.ItemId, i.ItemName, i.Manufacturer == null ? "" : i.Manufacturer.ManufacturerId.ToString(), i.Manufacturer == null ? "" : i.Manufacturer.Name, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0 , 0 , "%", 0, 0);
                }
                this.IsLoadingLowStocks = false;

            }
            catch (Exception)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading suppier items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoadPrvSoldItems_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsLoadingPrvSoldItems = true; // this is needed to makes "Units" slected by default on this action
                DlgAddStockFilter f = new DlgAddStockFilter();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                List<StockAddFilterVM> lst = f.SelectedItems;
                foreach (var i in lst)
                {
                    grdItems.Rows.Add(0, 0, i.ItemId, i.ItemName, i.Manufecturer == null ? "" : i.Manufecturer.ManufacturerId.ToString(), i.Manufecturer == null ? "" : i.Manufecturer.Name, i.Unit, i.ConversionUnit, i.AvailableStock, i.Quantity, i.UnitCost, i.Quantity * i.UnitCost, 0, "%", 0, i.Quantity * i.UnitCost);

                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            finally
            {
                this.IsLoadingPrvSoldItems = false;
            }
        }

        #region DataGridEvents
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridViewComboBoxCell dgCmbUnit;
                DataGridViewRow row = grdItems.Rows[e.RowIndex];
                if (this.IsLoadingLowStocks)
                {
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add(unit);

                    if (unit.ToLower() == "pack") { dgCmbUnit.Items.Add("Units"); }
                    else { dgCmbUnit.Items.Add("Pack"); }

                    //set Unit/Pack Value
                    if (unitIndexCombo.SelectedIndex == 0)
                    {
                        row.Cells["colUnit"].Value = "Units";
                        unit = row.Cells["colUnit"].Value.ToString();
                    }
                    else if (unitIndexCombo.SelectedIndex == 1)
                    {
                        row.Cells["colUnit"].Value = "Pack";
                        unit = row.Cells["colUnit"].Value.ToString();
                    }
                    else { row.Cells["colUnit"].Value = unit; }

                    if (!unit.ToLower().Equals("units") && !unit.ToLower().Equals("unit"))
                    {
                        grdItems.CellValueChanged -= grdItems_CellValueChanged;
                        int qty = 0;
                        int AvailableQty = Convert.ToInt32(row.Cells["colAvQty"].Value);
                        double uCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value);
                        int ConUnit = SharedFunctions.StringToInt(row.Cells["colConvUnit"].Value);
                        row.Cells["colAvQty"].Value = AvailableQty / ConUnit;
                        row.Cells["colUnitCost"].Value = uCost * ConUnit;
                        int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                        row.Cells["colGrossAmount"].Value = qty * uCost;
                        grdItems.CellValueChanged += grdItems_CellValueChanged;
                    }
                    else
                    {
                        int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                        ItemsVM itemDataValue = new ItemsVM();
                        using (unitOfWork = new UnitOfWork())
                        {
                            itemDataValue = unitOfWork.ItemRspository.GetItemWithAvQty(itemId);
                        }
                        row.Cells["colUnitCost"].Value = itemDataValue.UnitCost;
                    }
                    row.Cells["colDiscType"].Value = "%";
                }
                else if (this.IsLoadingPrvSoldItems)
                {
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add(unit);

                    if (unit.ToLower() == "pack") { dgCmbUnit.Items.Add("Units"); }
                    else { dgCmbUnit.Items.Add("Pack"); }

                    //set Unit/Pack Value
                    if (unitIndexCombo.SelectedIndex == 0)
                    {
                        row.Cells["colUnit"].Value = "Units";
                        unit = row.Cells["colUnit"].Value.ToString();
                    }
                    else if (unitIndexCombo.SelectedIndex == 1)
                    {
                        row.Cells["colUnit"].Value = "Pack";
                        unit = row.Cells["colUnit"].Value.ToString();
                    }
                    else { row.Cells["colUnit"].Value = unit; }

                    if (!unit.ToLower().Equals("units") && !unit.ToLower().Equals("unit"))
                    {
                        grdItems.CellValueChanged -= grdItems_CellValueChanged;
                        int qty = 0;
                        int AvailableQty = Convert.ToInt32(row.Cells["colAvQty"].Value);
                        double uCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value);
                        int ConUnit = SharedFunctions.StringToInt(row.Cells["colConvUnit"].Value);
                        row.Cells["colAvQty"].Value = AvailableQty / ConUnit;
                        row.Cells["colUnitCost"].Value = uCost * ConUnit;
                        uCost = Convert.ToInt32(row.Cells["colUnitCost"].Value);
                        int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                        row.Cells["colGrossAmount"].Value = qty * uCost;
                        grdItems.CellValueChanged += grdItems_CellValueChanged;
                    }
                    else
                    {
                        int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                        ItemsVM itemDataValue = new ItemsVM();
                        using (unitOfWork = new UnitOfWork())
                        {
                            itemDataValue = unitOfWork.ItemRspository.GetItemWithAvQty(itemId);
                        }
                        row.Cells["colUnitCost"].Value = itemDataValue.UnitCost;
                    }
                    row.Cells["colDiscType"].Value = "%";
                }
                else if (this.IsFormLoading)
                {
                    int avQty = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemID"].Value);
                        avQty = unitOfWork.ItemRspository.GetItemAvailableQty(itemId);
                    }
                    row.Cells["colAvQty"].Value = avQty;
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add("Pack");
                    dgCmbUnit.Items.Add("Units");
                    row.Cells["colUnit"].Value = unit;
                    //row.Cells["colDiscType"].Value = "%";
                    
                }
                else
                {
                    row = grdItems.Rows[e.RowIndex];
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    ItemsVM itemData;
                    using (unitOfWork = new UnitOfWork())
                    {
                        itemData = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    if (itemData.Manufacturer != null)
                    {
                        row.Cells["colManuf"].Value = itemData.Manufacturer.Name;
                        row.Cells["colManufId"].Value = itemData.Manufacturer.ManufacturerId;
                    }
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    row.Cells["colItemName"].Value = itemData.ItemName;
                    row.Cells["colConvUnit"].Value = itemData.ConversionUnit;
                    
                    int avlQty = itemData.AvailableStock;
                    row.Cells["colAvQty"].Value = avlQty / itemData.ConversionUnit;
                    if (ExpireItemId > 0 && IsFromNotification)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            ItemsVM item = unitOfWork.ItemRspository.GetItemExpiredDetails(ItemId);
                            if (item != null)
                            {
                                row.Cells["colQuantity"].Value = item.ExpiredStock;
                                row.Cells["colUnitCost"].Value = itemData.UnitCost * itemData.ConversionUnit;
                                row.Cells["colGrossAmount"].Value = item.ExpiredStock * itemData.UnitCost;
                            }
                            else
                            {
                                row.Cells["colQuantity"].Value = 0;
                                row.Cells["colUnitCost"].Value = itemData.UnitCost * itemData.ConversionUnit;
                                row.Cells["colGrossAmount"].Value = 0;
                            }
                        }
                    }
                    else
                    {
                        row.Cells["colQuantity"].Value = 0;
                        if (unitIndexCombo.SelectedIndex == 1 || unitIndexCombo.SelectedIndex == -1) 
                        {
                            row.Cells["colUnitCost"].Value = itemData.UnitCost * itemData.ConversionUnit; 
                        }
                        else 
                        {
                            row.Cells["colAvQty"].Value = itemData.AvailableStock;
                            row.Cells["colUnitCost"].Value = itemData.UnitCost; 
                        }
                        row.Cells["colGrossAmount"].Value = 0;
                    }

                    string unit = itemData.Unit;
                    dgCmbUnit.Items.Add(unit);

                    if (unit.ToLower() == "pack") { dgCmbUnit.Items.Add("Units"); }
                    else { dgCmbUnit.Items.Add("Pack"); }

                    if (unitIndexCombo.SelectedIndex == 0)
                    {
                        row.Cells["colUnit"].Value = "Units";
                    }
                    else if (unitIndexCombo.SelectedIndex == 1)
                    {
                        row.Cells["colUnit"].Value = "Pack";
                    }
                    else { row.Cells["colUnit"].Value = dgCmbUnit.Items[0]; }
                    row.Cells["colDiscType"].Value = "%";
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }

                LoadSerial();
                PopulateTotals();
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public double GetDiscountValue(double Disc, string Type , double GrossAmount)
        {
            double Discount = 0;
            if (Type == "%")
            {
                Discount = Disc / 100 * GrossAmount;
            }
            else
            {
                Discount = Disc;
            }

            return Discount;
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            DataGridViewRowCancelEventArgs ev = new DataGridViewRowCancelEventArgs(grdItems.Rows[e.RowIndex]);
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                grdItems_UserDeletingRow(grdItems, ev);

                long PurchaseOrderItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colPurchaseORderItemID"].Value);
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                if (PurchaseOrderItemId > 0)
                {
                    //grdItems.Rows.RemoveAt(e.RowIndex);
                    grdItems.Rows[e.RowIndex].Visible = false;
                }
                else
                {
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                PopulateTotals();
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                numQuantity.ValueChanged -= numQuantity_ValueChanged;
                numUnitCost.ValueChanged -= numUnitCost_ValueChanged;
                FillItemInfo(grdItems.Rows[e.RowIndex]);
                EditingRowIndex = e.RowIndex;
                this.btnAddItem.Text = "Update Item";
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                //cmbItems.Enabled = false;  
                uC_SearchItems1.Enabled = false;
                numQuantity.ValueChanged += numQuantity_ValueChanged;
                numUnitCost.ValueChanged += numUnitCost_ValueChanged;
                return;
            }
        }
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscType"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
        private void grdItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Visible)
            {
                decimal RowTotal = Convert.ToDecimal(e.Row.Cells["colTotalAmount"].Value);
                decimal Grandtotal = 0;
                decimal.TryParse(txtGrandTotal.Text, out Grandtotal);

                txtGrandTotal.Text = (Grandtotal - RowTotal).ToString();
            }
            calculateTotal();
        }
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int qty = 0;
            double Discount1 = 0;
            double uCost = 0;
            try
            {
                DataGridViewRow row = grdItems.Rows[e.RowIndex];
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index)
                {
                    if (row.Cells["colQuantity"].Value == null) { row.Cells["colQuantity"].Value = 0; }

                    int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                    uCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value);
                    row.Cells["colGrossAmount"].Value = qty * uCost;
                    Discount1 = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                    row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount1).ToString();

                }
                else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
                {

                    ItemsVM itemData = new ItemsVM();
                    int Qty = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    int ItemId = Convert.ToInt32(row.Cells["colItemId"].Value);
                    using (unitOfWork = new UnitOfWork())
                    {
                        itemData = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    string unit = row.Cells["colUnit"].Value.ToString();
                    Discount1 = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                    row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount1).ToString();
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    if (!unit.ToLower().Equals("units") && !unit.ToLower().Equals("unit"))
                    {
                        itemData.AvailableStock = (int)Math.Floor((double)itemData.AvailableStock / itemData.ConversionUnit);
                        //if (this.PurchaseOrderId > 0)
                        //{
                        //    itemData.UnitCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value) * itemData.ConversionUnit;
                        //}
                        //else
                        {
                            itemData.UnitCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value) == 0 ? itemData.UnitCost * itemData.ConversionUnit : SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value) * itemData.ConversionUnit;
                        }
                    }
                    else
                    {
                        //if (this.PurchaseOrderId > 0)
                        {
                            ItemsVM itemDataValue = new ItemsVM();
                            using (unitOfWork = new UnitOfWork())
                            {
                                itemDataValue = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                            }
                            itemData.UnitCost = itemDataValue.UnitCost;
                            //itemData.UnitCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value) / itemData.ConversionUnit;
                        }
                    }
                    row.Cells["colUnitCost"].Value = itemData.UnitCost;
                    row.Cells["colAvQty"].Value = itemData.AvailableStock;


                    int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                    uCost = SharedFunctions.StringToDouble(row.Cells["colUnitCost"].Value);
                    row.Cells["colGrossAmount"].Value = qty * uCost;
                    //row.Cells["colGrossAmount"].Value = itemData.UnitCost;
                    Discount1 = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                    row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount1).ToString();
                }
                else if (e.ColumnIndex == grdItems.Columns["colDisc"].Index)
                {
                    double Discount = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                    row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount).ToString();
                    
                }
                else if (e.ColumnIndex == grdItems.Columns["colDiscType"].Index)
                {
                    double Discount = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                    row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount).ToString();
                }
                else if ((e.ColumnIndex == grdItems.Columns["colBonus"].Index))
                {
                }
                else
                {
                    row.Cells["colTotalAmount"].Value = row.Cells["colGrossAmount"].Value;
                }
                
                PopulateTotals();
                //double Discount1 = GetDiscountValue(Convert.ToDouble(row.Cells["colDisc"].Value), row.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(row.Cells["colGrossAmount"].Value));
                //row.Cells["colTotalAmount"].Value = (Convert.ToDouble(row.Cells["colGrossAmount"].Value) - Discount1).ToString();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calculcation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grdItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                    uC_SearchItems1.SetFocus();
            }
        }
        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                    uC_SearchItems1.SetFocus();
            }
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            e.Control.KeyPress -= new KeyPressEventHandler((obj, arg) => Control_KeyPress(sender, arg,
                tb,
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index ? true : false
                ));
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                )
            {
                e.Control.KeyPress += new KeyPressEventHandler((obj, arg) => Control_KeyPress(sender, arg,
                tb,
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index ? true : false
                ));
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e, TextBox tb, bool IsInteger = false)
        {
            if (IsInteger)
            {
                SharedFunctions.isValidIntegerKey(sender, e);
            }
            else
            {
                SharedFunctions.IsValidDoubleNumericDatagridview(sender, e, tb);
            }
        }
        #endregion

        private void unitIndexCombo_SelectedValueChanged(object sender, EventArgs e)
        {
  
        }

        private void unitIndexCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int qty = 0;
            double uCost = 0;
            string unitTxt, checkTxt = "";
            if (unitIndexCombo.SelectedIndex == 0) { unitTxt = "Units"; }
            else { unitTxt = "Pack"; }
            double Discount1 = 0;
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            DataGridViewComboBoxCell dgCmbUnit;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                checkTxt = r.Cells["colUnit"].Value.ToString();
                Discount1 = GetDiscountValue(Convert.ToDouble(r.Cells["colDisc"].Value), r.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(r.Cells["colGrossAmount"].Value));
                if (checkTxt.ToLower() != unitTxt.ToLower())
                {
                    int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    ItemsVM itemData;
                    using (unitOfWork = new UnitOfWork())
                    {
                        itemData = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }

                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    if (unitIndexCombo.SelectedIndex == 1)
                    {
                        itemData.AvailableStock = (int)Math.Floor((double)itemData.AvailableStock / itemData.ConversionUnit);
                        itemData.UnitCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value) * itemData.ConversionUnit;
                    }
                    else
                    {
                        itemData.UnitCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value) / itemData.ConversionUnit;
                    }

                    dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    r.Cells["colItemName"].Value = itemData.ItemName;
                    r.Cells["colConvUnit"].Value = itemData.ConversionUnit;
                    r.Cells["colAvQty"].Value = itemData.AvailableStock;
                    if (ExpireItemId > 0 && IsFromNotification)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            ItemsVM item = unitOfWork.ItemRspository.GetItemExpiredDetails(ItemId);
                            if (item != null)
                            {
                                r.Cells["colQuantity"].Value = item.ExpiredStock;
                                r.Cells["colUnitCost"].Value = itemData.UnitCost;
                                r.Cells["colGrossAmount"].Value = item.ExpiredStock * itemData.UnitCost;

                            }
                            else
                            {
                                r.Cells["colUnitCost"].Value = itemData.UnitCost;
                                //r.Cells["colQuantity"].Value = 0;
                                //r.Cells["colGrossAmount"].Value = 0;
                            }
                        }
                    }
                    else
                    {
                        r.Cells["colUnitCost"].Value = itemData.UnitCost;
                        int.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                        uCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);
                        r.Cells["colGrossAmount"].Value = qty * uCost;
                        Discount1 = GetDiscountValue(Convert.ToDouble(r.Cells["colDisc"].Value), r.Cells["colDiscType"].Value.ToString(), Convert.ToDouble(r.Cells["colGrossAmount"].Value));
                        r.Cells["colTotalAmount"].Value = (Convert.ToDouble(r.Cells["colGrossAmount"].Value) - Discount1).ToString();
                    }

                    if (unitIndexCombo.SelectedIndex == 0)
                    {
                        r.Cells["colUnit"].Value = "Units";
                    }
                    else
                    {
                        r.Cells["colUnit"].Value = "Pack";
                    }
                    PopulateTotals();
                }
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            
            
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
                cmManufecturersList.DataSource = cts;
                cmManufecturersList.ValueMember = "ManufacturerId";
                cmManufecturersList.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading manufacturers, please try again.", "Manufacturers not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool isAlreadyAdded(int ManufecturerId)
        {
            foreach (Label l in pnlManufecturers.Controls)
            {
                if (l.TabIndex.ToString() == ManufecturerId.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        private void cmManufecturersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var supplierID = 0;
            //get id from object
            if (cmManufecturersList.SelectedItem != null)
            {
                System.Reflection.PropertyInfo getId = cmManufecturersList.SelectedItem.GetType().GetProperty("ManufacturerId");
                supplierID = Convert.ToInt32(getId.GetValue(cmManufecturersList.SelectedItem, null));
            }

            if (isAlreadyAdded(supplierID))
            {
                ToolTip tt = new ToolTip();
                tt.Show("Manufecturer Already Add", cmManufecturersList, 800);
                return;
            }
            else if (cmManufecturersList.SelectedIndex == -1)
            {
                return;
            }
            else if (cmManufecturersList.SelectedIndex.ToString() != "0")
            {
                GenerateSupplier(cmManufecturersList.Text, supplierID.ToString());
            }
            else
            {
                return;
            }

        }
        private void lblTag_DoubleClicked(object sender, EventArgs e)
        {
            Label ClickedLbl = (Label)sender;
            pnlManufecturers.Controls.Remove(ClickedLbl);
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
            pnlManufecturers.Controls.Add(lblTag);
        }
        #region CommentedCode

        //private void LoadItems()
        //{
        //    List<Item> itemsList = unitOfWork.ItemRspository.GetActiveItems();
        //    cmbItems.DataSource = itemsList;
        //    cmbItems.ValueMember = "ItemId";
        //    cmbItems.DisplayMember = "ItemName";
        //}

        //private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    decimal RowTotal = Convert.ToDecimal(grdItems.Rows[e.RowIndex].Cells["colGrossAmount"].Value);
        //    decimal Grandtotal = 0;
        //    decimal.TryParse(txtGrandTotal.Text, out Grandtotal);
        //    txtGrandTotal.Text = (Grandtotal + RowTotal).ToString();
        //}
        #endregion

        private void btnSearchItems_Click(object sender, EventArgs e)
        {


            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }
        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    //&& Convert.ToInt32(r.Cells["colBatchId"].Value) == BatchId)
                    )
                {
                    return true;
                }
            }
            return false;
            //return false;
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            if (ItemAlreadyAdded(ItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
            {
                MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                //if (IsValidInput())
                //{
                //    PurchaseOrderItem objPItem = new PurchaseOrderItem();
                //    FillObject(objPItem);
                //    if (EditingRowIndex >= 0)
                //    {
                //        UpdateRow(objPItem);
                //    }
                //    elseawewdwdawdA   
                //    {
                //        AddItemToGrid(objPItem);
                //    }
                //    Clear();
                //}
                if (ItemId <= 0)
                    return;
                grdItems.Rows.Add(0, 0, ItemId);
                calculateTotal();
                //if (this.SelectedItemId > 0)
                //{
                //    grdItems.Rows.Add(0, this.SelectedItemId);
                //}
                //else
                //{
                //    MessageBox.Show("Please select an item before proceeding.", "Select Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
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
            this.uC_SearchItems1.SetFocus();
            grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"].Value = Convert.ToString(Quantity);
            grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];
            grdItems.BeginEdit(true);
            int index = grdItems.CurrentRow.Index;
            grdItems.FirstDisplayedScrollingRowIndex = index;
        }

        private void grdItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (Properties.Settings.Default.IsUsingBarcode)
            {
                uC_SearchItems1.SetText = "";
                uC_SearchItems1.SetFocus();
            }
        }
    }
}