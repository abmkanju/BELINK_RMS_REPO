using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core;
using System.Transactions;
using PagedList;
using Pharmacy.UI.ShortCutDialogs;
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Data.SqlClient;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddStock : Form
    {
        int a = 0;
        int b = 0;
        public int isholdchekc = 0;
        public int iii = 0;
        private int StockAddItemId = 0;
        private string controlVal;
        UnitOfWork unitOfWork;
        private long StockEditID = 0;
        int EditingRowIndex = -1;
        long PurchaseOrderId = 0;
        bool IsStockPricesEditable = false;
        List<Supplier> Suppliers = new List<Supplier>();
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        BackgroundWorker bgWSuppliersLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        int SelectedItemId = 0;
        string SelectedItemName = "";
        private bool isStockLoading = false;
        private bool IsLoadingPrvSoldItems = false;
        List<StoreVM> loadStores = null;


        public frmAddStock()
        {
            InitializeComponent();
            ShowHoldingStock(0);
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            //checkAdvanceTax();
        }
        public frmAddStock(int StockAdd_ItemID)
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.StockAddItemId = StockAdd_ItemID;
        }

        //private void checkAdvanceTax()
        //{
        //    if (SharedVariables.AdminPharmacySetting.IsAdvanceTaxStock)
        //    {
        //        grdItems.Columns["colAdvanceWHT"].Visible = true;
        //        grdItems.Columns["colAdvanceWHTType"].Visible = true;
        //        label35.Visible = true;
        //        label36.Visible = true;
        //        cmbWithAdvanceHoldingTaxType.Visible = true;
        //        txtAdvanceWithHoldingTax.Visible = true;
        //    }
        //    else
        //    {
        //        grdItems.Columns["colAdvanceWHT"].Visible = false;
        //        grdItems.Columns["colAdvanceWHTType"].Visible = false;
        //        label35.Visible = false;
        //        label36.Visible = false;
        //        cmbWithAdvanceHoldingTaxType.Visible = false;
        //        txtAdvanceWithHoldingTax.Visible = false;
        //    }
        //}

        public int totalholds = 0;
        private void ShowHoldingStock(int p_pageNo)
        {
            try
            {

                //btnClearAll.PerformClick();
                if (grdItems.Rows.Count > 0)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    ///StockItemsList.Clear();
                    this.ShowNewDocumentNumber();
                    this.dtpDate.Value = DateTime.Now;
                    this.dtpSupplierInvoiceDate.Value = DateTime.Now;
                    this.cmbSuppliers.SelectedIndex = -1;
                    this.txtSupplierInvoiceNo.Text = "";
                    this.txtGrandTotal.Text = "0";
                    AssignGrandTotalWithHoldingTax();
                    this.isStockLoading = false;
                    this.IsLoadingPrvSoldItems = false;

                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    grdItems.Columns["colDiscount"].ReadOnly = false;
                    grdItems.Columns["colPercDiscType"].ReadOnly = false;
                    grdItems.Columns["colSalesTax"].ReadOnly = false;
                    grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;
                }
                DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='false' or IsActive='False')");

                if (totalholds != dt.Rows.Count)
                {
                    btnNext.Enabled = true;
                    btnLastPage.Enabled = true;
                }
                if (totalholds == dt.Rows.Count)
                {
                    btnNext.Enabled = false;
                    btnLastPage.Enabled = false;
                }
                
                if (p_pageNo <= dt.Rows.Count && p_pageNo >= 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        grpInvoiceHolding.Text = "Receipt Holding" + "[" + p_pageNo + "/" + (dt.Rows.Count) + "]";
                    }
                    else
                    {
                        grpInvoiceHolding.Text = "Receipt Holding" + "[" + p_pageNo + "/" + 0 + "]";
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading next invoice, please try after reloading invoice screen.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public frmAddStock(long StockEditID, long PurchaseOrderId, bool IsStockPricesEditable = false)
        {
            InitializeComponent();
            this.StockEditID = StockEditID;
            this.IsStockPricesEditable = IsStockPricesEditable;
            this.PurchaseOrderId = PurchaseOrderId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.BringToFront();
            //checkAdvanceTax();
        }
        private DataTable get(string qry)
        {

            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;

        }
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            loadStores = new List<StoreVM>();
            grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.WindowState = FormWindowState.Maximized;
            grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            SharedFunctions.SetGridStyle(this.grdItems);
            
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnClearAll, btnSave });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddSupplier, btnAttachImage, btnAddItem, btnClear });
            try
            {
                LoadSalesDicountCmb();
                LoadStores();
                LoadItemsAndSuppliers();      // calls backGorundWorker : //  also loadsStock in case of edit on RunWorker_Completed
                setNumericUD_toEmpty();
                cmbDiscountType.SelectedIndex = 0;
                cmbSalesTaxType.SelectedIndex = 0;
                //cmbItems.SelectedIndex = -1;
                if (SharedVariables.AdminPharmacySetting.IsStockEditableUnitRetailPrice && this.StockEditID > 0 && this.IsStockPricesEditable)
                {
                    DisableControls();
                }
                if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                {
                    grdItems.Columns["colBatch"].Visible = false;
                    grdItems.Columns["colExpiry"].Visible = false;
                }
                if (this.StockAddItemId > 0)
                {
                    this.SelectedItemId = this.StockAddItemId;
                    btnAddItem.PerformClick();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                isStockLoading = false;
            }
            //grdItems.Columns["colRemove"].DefaultCellStyle.ForeColor = Color.Red;
            //grdItems.Rows[0].Cells["colRemove"].Value = "X";
            //CustomCode.CalendarColumn col = new CustomCode.CalendarColumn();
            //col.Name = "colExpiryDate";
            //col.HeaderText = "Expiry";
            //grdItems.Columns.Insert(6, col);    
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
        }

        private void LoadStores()
        {
            grdItems.Columns["colStores"].Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            using (unitOfWork = new UnitOfWork())
            {
                loadStores.Add(new StoreVM { StoreId = 0, Name = "Select Store" });
                loadStores.AddRange(unitOfWork.StoreRepository.GetAllStores());
            }
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;

            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            //if (this.SelectedItemId > 0)
            //{
            //    LoadRates();
            //}
            if (this.SelectedItemId > 0)
            {
                btnAddItem.PerformClick();
               
            }
            //this.uC_SearchItems1.BringToFront();
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I))
            {
                this.uC_SearchItems1.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
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
            if (keyData == (Keys.F1))
            {
                btnSearchItems.PerformClick();
                return true;
            }
            //Functionality is not implemented
            //if (keyData == (Keys.Alt | Keys.E))
            //{
            //    if (grdItems.SelectedRows != null && grdItems.SelectedRows.Count > 0)
            //    {
            //        int colIndex = grdItems.Columns["colEdit"].Index;
            //        int rowIndex = grdItems.SelectedRows[0].Index;
            //        DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
            //        grdItems_CellContentClick(grdItems, e);
            //    }
            //    return true;
            //}
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (grdItems.SelectedRows != null && grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    grdItems.CurrentCell = grdItems.Rows[rowIndex].Cells["colRemove"];
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                controlVal = "";
                btnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.P))
            {
                controlVal = "StockPaymentShow";
                btnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (this.ActiveControl.Name == grdItems.Name)
                {
                    if (grdItems.Rows != null && grdItems.Rows.Count > 0)
                    {
                        DataGridViewRow r = grdItems.SelectedRows[0];
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
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void setNumericUD_toEmpty()
        {
            numUnitCost.ResetText();
            numQty.ResetText();
            numRetailPrice.ResetText();
            numDiscount.ResetText();
            numSalesTax.ResetText();
        }
        private void UpdateUsersAndSuppliers()
        {
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            Suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.Enabled = true;
            lblLoadingSuppliers.Visible = false;
        }
        private void LoadSalesDicountCmb()
        {
            try
            {
                cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                cmbWithHoldingTaxType.SelectedIndexChanged -= cmbWithHoldingTaxType_SelectedIndexChanged;
                cmbWithAdvanceHoldingTaxType.SelectedIndexChanged -= cmbWithAdvanceHoldingTaxType_SelectedIndexChanged;

                List<KeyValueVM> salesList = new List<KeyValueVM>()
                {
                    new KeyValueVM() { Id = 0, Name = "%"},
                    new KeyValueVM() { Id = 1, Name = "Value"},
                };
                cmbNetSaleTaxType.DataSource = salesList;
                cmbNetSaleTaxType.ValueMember = "Id";
                cmbNetSaleTaxType.DisplayMember = "Name";

                List<KeyValueVM> withholdingTax = new List<KeyValueVM>()
                {
                    new KeyValueVM() {Id = 0, Name = "%"},
                    new KeyValueVM() {Id = 1, Name = "Value"},
                };
                cmbWithHoldingTaxType.DataSource = withholdingTax;
                cmbWithHoldingTaxType.ValueMember = "Id";
                cmbWithHoldingTaxType.DisplayMember = "Name";

                List<KeyValueVM> withAdvanceholdingTax = new List<KeyValueVM>()
                {
                    new KeyValueVM() {Id = 0, Name = "%"},
                    new KeyValueVM() {Id = 1, Name = "Value"},
                };
                cmbWithAdvanceHoldingTaxType.DataSource = withAdvanceholdingTax;
                cmbWithAdvanceHoldingTaxType.ValueMember = "Id";
                cmbWithAdvanceHoldingTaxType.DisplayMember = "Name";

                List<KeyValueVM> discountList = new List<KeyValueVM>()
                {
                    new KeyValueVM() { Id = 0, Name = "%"},
                    new KeyValueVM() { Id = 1, Name = "Value"},
                };
                cmbNetDiscountType.DataSource = discountList;
                cmbNetDiscountType.ValueMember = "Id";
                cmbNetDiscountType.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                cmbWithHoldingTaxType.SelectedIndexChanged += cmbWithHoldingTaxType_SelectedIndexChanged;
            }

        }

        private void DisableControls()
        {
            try
            {
                cmbSuppliers.Enabled = false;
                btnAddSupplier.Enabled = false;
                btlLoadSupProds.Enabled = false;
                btnSupLowStockItems.Enabled = false;
                txtSupplierInvoiceNo.Enabled = false;
                btnAttachImage.Enabled = false;
                lblRemoveFile.Enabled = false;
                uC_SearchItems1.Enabled = false;
                btnSearchItems.Enabled = false;
                btnAddItem.Enabled = false;
                btnClear.Enabled = false;
                dtpDate.Enabled = false;
                dtpSupplierInvoiceDate.Enabled = false;
                btnLPrvSoldItems.Enabled = false;
                btnLoadMissedSales.Enabled = false;
                btnClearAll.Enabled = false;
                //---------DisableDataGridField----------//
                grdItems.Columns["colUnit"].ReadOnly = true;
                grdItems.Columns["colConvUnit"].ReadOnly = true;
                grdItems.Columns["colBonusQty"].ReadOnly = true;
                grdItems.Columns["colQuantity"].ReadOnly = false;
                grdItems.Columns["colBatch"].ReadOnly = true;
                grdItems.Columns["colExpiry"].ReadOnly = true;
                grdItems.Columns["colBonusDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colPercDiscType"].ReadOnly = true;
                grdItems.Columns["colDiscType"].ReadOnly = true;
                grdItems.Columns["colSalesTax"].ReadOnly = true;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = true;
                grdItems.Columns["colSalesTaxType"].ReadOnly = true;
                grdItems.Columns["colNetValue"].ReadOnly = true;
                grdItems.Columns["colConsumedQty"].ReadOnly = true;
                grdItems.Columns["colDiscountValue"].ReadOnly = true;
                grdItems.Columns["colSalesTaxValue"].ReadOnly = true;
                //---------DisableFooterField----------//
                txtNetDiscount.Enabled = false;
                cmbNetDiscountType.Enabled = false;
                txtNetSaleTax.Enabled = false;
                cmbNetSaleTaxType.Enabled = false;
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }
        private void LoadPurchaseOrder()
        {
            AddPOStockVM Po;
            ItemsVM vm = new ItemsVM();
            using (unitOfWork = new UnitOfWork())
            {
                Po = unitOfWork.PurchaseOrderRepository.GetPOById(this.PurchaseOrderId);
            }
            
            if (Po.SupId > 0)
            {
                cmbSuppliers.SelectedValue = Po.SupId;
            }

            isStockLoading = true;
            foreach (ItemsVM i in Po.Items)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.ItemId);
                }
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    //i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsumedStock);
                    i.AvailableStock = vm.AvailableStock;

                }
                else
                {
                    //i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsumedStock)  + i.HoldStock;
                    i.AvailableStock = vm.AvailableStock;

                }
                i.Unit = i.UnitValue == 0 ? i.Unit : "Units";
                i.ItemQuantity = i.UnitValue == 0 ? i.ItemQuantity / i.ConversionUnit : i.ItemQuantity;
                i.UnitCost = i.UnitValue == 0 ? i.UnitCost * i.ConversionUnit : i.UnitCost;
                i.RetailPrice = i.UnitValue == 0 ? i.RetailPrice * i.ConversionUnit : i.RetailPrice;
                object[] gr = {
                                      0,
                                      0,
                                      i.ItemId, 
                                      i.ItemName,
                                      i.Unit,
                                      0, //Store
                                      i.ConversionUnit,
                                      i.AvailableStock / (i.Unit == "Pack" || i.Unit == "pack" ? i.ConversionUnit : 1), // available quantity
                                      i.ItemQuantity,
                                      i.Bonus, // bonus quantiy
                                      i.UnitCost,
                                      i.UnitCost,
                                      i.ItemQuantity,
                                      i.RetailPrice ,
                                      Profit(i.RetailPrice, i.UnitCost),
                                      "",
                                      "",
                                      0, // bonus discount
                                      i.Discount, //discount
                                      i.DiscountType ==  0 ? "%" : "Value",
                                      "Lumpsum", //discoutn type
                                      0, // salestax
                                      "%",
                                      "Lumpsum",  // salestax type
                                      0,
                                      "%",
                                      0,
                                      "%",
                                      0,
                                      "5",
                                      i.ItemQuantity ,
                                      GrossMargin(i.AvailableStock, i.RetailPrice, i.ItemQuantity * i.UnitCost),
                                      0,
                                      0,
                                      0

                                  };
                grdItems.Rows.Add(gr);
               
                calculatteGrandValues(grdItems);
                calculateDiscPrice(grdItems);
                LoadSerial();
            }
            isStockLoading = false;
            txtGrandTotal.Text = GetGrandTotal().ToString();
            AssignGrandTotalWithHoldingTax();
        }
        private void LoadHoldStock(long stockId)
        {
            ItemsVM vm34 = new ItemsVM();

           
            StockVM s;
            using (unitOfWork = new UnitOfWork())
            {
                s = unitOfWork.StockRepository.GetStockById_ShowforHold(stockId);
            }
            txtDocumentNo.Text = s.DocumentNo.ToString();
            if (s.Supplier != null)
            {
                cmbSuppliers.SelectedValue = s.Supplier.SupplierID;
            }
            else
            {
                cmbSuppliers.SelectedIndex = -1;
            }
            AssingWithholdingInvoice(s.WithholdingTax, s.WithholdingTaxType);
            txtSupplierInvoiceNo.Text = s.SupplierInvoiceNo;
            dtpDate.Value = (DateTime)s.CreateDate;
            dtpSupplierInvoiceDate.Value = (DateTime)s.SupplierIvoiceDate;
            isStockLoading = true;
            bool hidefeild = false; 
            
            foreach (StockItemVM i in s.StockItems)
            {
                if (i.Quantity > 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        vm34 = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.Item.ItemID);
                    }
                    i.Quantity = i.Unit == 0 ? (i.Quantity / i.ConversionUnit) : i.Quantity; // convert quantity back and forth depending on unit.
                    
                    i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                    i.Discount = i.Discount;//i.Unit == 0 ? i.Discount * i.Item.ConversionUnit : i.Discount;
                    i.SalesTax = i.SalesTax;//i.Unit == 0 ? i.SalesTax * i.Item.ConversionUnit : i.SalesTax;
                    i.BonusQuantity = i.Unit == 0 ? i.BonusQuantity / i.ConversionUnit : i.BonusQuantity;
                    i.BonusDiscount = i.BonusDiscount;// i.Unit == 0 ? i.BonusDiscount * i.ConversionUnit : i.BonusDiscount;
                    i.RetailPrice = i.Unit == 0 ? i.RetailPrice * i.Item.ConversionUnit : i.RetailPrice;
                    string exp = i.Batch.Expiry == null ? "" : i.Batch.Expiry.Value.ToShortDateString();
                    // 0 : available quantity
                    grdItems.Rows.Add(
                        0,
                        i.StockItemId,
                        i.Item.ItemID,
                        i.Item.ItemName,
                        i.UnitString,
                        i.Store == null ? 0 : i.Store.StoreId,
                        i.Item.ConversionUnit,
                        i.UnitString == "Unit" ? vm34.AvailableStock : vm34.AvailableStock/i.ConversionUnit,
                        i.Quantity,
                        i.BonusQuantity,
                        i.UnitCost,
                        i.PercDiscType == 1 ? i.UnitCost - i.Discount : i.UnitCost - (((i.Discount / 100) * (i.Quantity * i.UnitCost)) / i.Quantity),
                        i.TotalCost,
                        i.RetailPrice,
                        Profit(i.RetailPrice, i.UnitCost),
                        i.Batch.BatchName,
                        exp,
                        i.BonusDiscount,
                        i.Discount,
                        i.PercDiscType == 0 ? "%" : "Value",
                        i.DiscountType == 0 ? "Lumpsum" : "Per Item",
                        i.SalesTax,
                        i.PercSalesTaxType == 0 ? "%" : "Value",
                        i.SalesTaxType == 0 ? "Lumpsum" : "Per Item",
                        i.WHT,
                        i.WHTType == 0 ? "%" : "Value",
                        i.NetValue,
                        GrossMargin(i.Quantity + i.BonusQuantity, i.RetailPrice, i.NetValue),
                        i.ConsumedStock,
                        i.DiscountVal,
                        i.SalesTaxVal,
                        
                        0);

                    LoadSerial();
                    
                }
                calculatteGrandValues(grdItems);
                if (i.Discount >0 || i.SalesTax > 0 || Convert.ToDouble(i.WHT) > 0)
                {
                    hidefeild = true;
                }
            }
            if (hidefeild)
            {
                txtWithHoldingTax.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                txtNetDiscount.Enabled = false;
                txtNetSaleTax.Enabled = false;
            }
            else
            {
                txtWithHoldingTax.Enabled = true;
                txtAdvanceWithHoldingTax.Enabled = true;
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
            }
            isStockLoading = false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colConsumedQty"].Value) > 0)
                {
                    r.DefaultCellStyle.BackColor = SharedVariables.DisabledColor;
                }
            }
            //------------------UnRegister Events -----------------//
            txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
            txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
            cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;

            txtNetDiscount.Text = s.NetDiscount.ToString();
            txtNetSaleTax.Text = s.NetSaleTax.ToString();

            cmbNetDiscountType.SelectedIndex = s.NetDiscountType;
            cmbNetSaleTaxType.SelectedIndex = s.NetSaleTaxType;



            txtGrandTotal.Text = GetGrandTotal().ToString();
            AssignGrandTotalWithHoldingTax();

            //------------------Register Events -----------------//
            txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
            txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
            cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;

            #region ShowHideFields
            bool grdDiscount = false, grdSaleTax = false;
            if (grdItems != null && grdItems.Rows != null && grdItems.Rows.Count > 0)
            {
                grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
            }
            //----------------------------- Did not assign any value -----------------------------//
            if (grdDiscount || grdSaleTax)
            {
                txtNetDiscount.Enabled = false;
                txtNetSaleTax.Enabled = false;
                cmbNetDiscountType.Enabled = false;
                cmbNetSaleTaxType.Enabled = false;
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colPercDiscType"].ReadOnly = false;
                grdItems.Columns["colSalesTax"].ReadOnly = false;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;
            }
            if (SharedFunctions.StringToDouble(txtNetDiscount.Text) > 0 || SharedFunctions.StringToDouble(txtNetSaleTax.Text) > 0)
            {
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colPercDiscType"].ReadOnly = true;
                grdItems.Columns["colSalesTax"].ReadOnly = true;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = true;
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;
                cmbNetDiscountType.Enabled = true;
                cmbNetSaleTaxType.Enabled = true;
                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
            }
            StockEditID = stockId;
            #endregion
        }
        private void LoadStockForEdit()
        {
            cbouom.Visible = false;
            button1.Visible = false;
            StockVM s;
            using (unitOfWork = new UnitOfWork())
            {
                s = unitOfWork.StockRepository.GetStockById_Show(this.StockEditID);
            }
            txtDocumentNo.Text = s.DocumentNo.ToString();
            if (s.Supplier != null)
            {
                cmbSuppliers.SelectedValue = s.Supplier.SupplierID;
            }
            else
            {
                cmbSuppliers.SelectedIndex = -1;
            }
            AssingWithholdingInvoice(s.WithholdingTax, s.WithholdingTaxType);
            AssingAdvanceWithholdingInvoice(s.AdvanceWithholdingTax, s.AdvanceWithholdingTaxType);

            txtSupplierInvoiceNo.Text = s.SupplierInvoiceNo;
            dtpDate.Value = (DateTime)s.CreateDate;
            dtpSupplierInvoiceDate.Value = (DateTime)s.SupplierIvoiceDate;
            isStockLoading = true;
            foreach (StockItemVM i in s.StockItems)
            {
                if (i.Quantity > 0)
                {
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity; // convert quantity back and forth depending on unit.
                    i.AvailableStock = i.Unit == 0 ? i.AvailableStock / i.ConversionUnit : i.AvailableStock;
                    i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                    i.Discount = i.Discount;//i.Unit == 0 ? i.Discount * i.Item.ConversionUnit : i.Discount;
                    i.SalesTax = i.SalesTax;//i.Unit == 0 ? i.SalesTax * i.Item.ConversionUnit : i.SalesTax;
                    i.BonusQuantity = i.Unit == 0 ? i.BonusQuantity / i.ConversionUnit : i.BonusQuantity;
                    i.BonusDiscount = i.BonusDiscount;// i.Unit == 0 ? i.BonusDiscount * i.ConversionUnit : i.BonusDiscount;
                    i.RetailPrice = i.Unit == 0 ? i.RetailPrice * i.Item.ConversionUnit : i.RetailPrice;
                    string exp = i.Batch.Expiry == null ? "" : i.Batch.Expiry.Value.ToString();
                    // 0 : available quantity
                    grdItems.Rows.Add(
                        1,
                        i.StockItemId,
                        i.Item.ItemID,
                        i.Item.ItemName,
                        i.UnitString,
                        i.Store == null ? 0 : i.Store.StoreId,
                        i.Item.ConversionUnit,
                        0,
                        i.Quantity,
                        i.BonusQuantity,
                        i.UnitCost,
                        0,
                        i.TotalCost,
                        i.RetailPrice,
                        Profit(i.RetailPrice, i.UnitCost),
                        i.Batch.BatchName,
                        exp,
                        i.BonusDiscount,
                        i.Discount,
                        i.PercDiscType == 0 ? "%" : "Value",
                        i.DiscountType == 0 ? "Lumpsum" : "Per Item",
                        i.SalesTax,
                        i.PercSalesTaxType == 0 ? "%" : "Value",
                        i.SalesTaxType == 0 ? "Lumpsum" : "Per Item",
                        i.WHT,
                        i.WHTType == 1 ? "%" : "Value",
                        i.AdWHT,
                        i.AdWHTType == 1 ? "%" : "Value",
                        i.NetValue,
                        GrossMargin(i.Quantity + i.BonusQuantity, i.RetailPrice, i.NetValue),
                        i.StockConsumed,
                        i.DiscountVal,
                        i.SalesTaxVal,
                        0
                        );

                    LoadSerial();
                }
                LoadDiscPrice(grdItems.Rows.Count-1);

                //int index = grdItems.SelectedRows[0].Index; ;
                //grdItems.FirstDisplayedScrollingRowIndex = index;
                //grdItems.Rows[index].Selected = true;
            }
            isStockLoading = false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colConsumedQty"].Value) > 0)
                {
                    r.DefaultCellStyle.BackColor = SharedVariables.DisabledColor;
                }
            }
            //------------------UnRegister Events -----------------//
            txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
            txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
            cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;

            txtNetDiscount.Text = s.NetDiscount.ToString();
            txtNetSaleTax.Text = s.NetSaleTax.ToString();

            cmbNetDiscountType.SelectedIndex = s.NetDiscountType;
            cmbNetSaleTaxType.SelectedIndex = s.NetSaleTaxType;

            //zahid added
            if (txtWithHoldingTax.Text == "0") { txtWithHoldingTax.Enabled = true; }
            else { txtWithHoldingTax.Enabled = false; }

            if (txtAdvanceWithHoldingTax.Text == "0") { txtAdvanceWithHoldingTax.Enabled = true; }
            else { txtAdvanceWithHoldingTax.Enabled = false; }

            //txtWithHoldingTax.Enabled = false;
            //txtAdvanceWithHoldingTax.Enabled = false;
            calculatteGrandValues(grdItems);

            txtGrandTotal.Text = GetGrandTotal().ToString();
            AssignGrandTotalWithHoldingTax();

            //------------------Register Events -----------------//
            txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
            txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
            cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
            btnHold.Enabled = false;
            btnCancelHold.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnLastPage.Enabled = false;
            btnFirstPage.Enabled = false;
            #region ShowHideFields
            bool grdDiscount = false, grdSaleTax = false , grdwht = false;
            if (grdItems != null && grdItems.Rows != null && grdItems.Rows.Count > 0)
            {
                grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
                grdwht = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colWHT"].Value)).Any(x => x > 0);
            }
            //----------------------------- Did not assign any value -----------------------------//
            if (grdDiscount || grdSaleTax || grdwht)
            {
                txtNetDiscount.Enabled = false;
                txtNetSaleTax.Enabled = false;
                cmbNetDiscountType.Enabled = false;
                cmbNetSaleTaxType.Enabled = false;
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colPercDiscType"].ReadOnly = false;
                grdItems.Columns["colSalesTax"].ReadOnly = false;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;
            }
            if (SharedFunctions.StringToDouble(txtNetDiscount.Text) > 0 || SharedFunctions.StringToDouble(txtNetSaleTax.Text) > 0)
            {
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colPercDiscType"].ReadOnly = true;
                grdItems.Columns["colSalesTax"].ReadOnly = true;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = true;
                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;
                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
                cmbNetDiscountType.Enabled = true;
                cmbNetSaleTaxType.Enabled = true;
            }
            txtGrandTotal.Text = GetGrandTotal().ToString();

            #endregion
            txtGrandTotal.Text = GetGrandTotal().ToString();

        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                cmbSuppliers.Enabled = false;
                lblLoadingSuppliers.Visible = true;
            });

            using (unitOfWork = new UnitOfWork())
            {
                //Items = unitOfWork.ItemRspository.GetActiveItems().ToList(); don't load , now these will be loaded at runtime
                Suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            }
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
                //cmbItems.DataSource = Items;
                //cmbItems.ValueMember = "ItemId";
                //cmbItems.DisplayMember = "ItemName";
                //cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
                //cmbItems.Enabled = true;
                //txtSearchItems.TextChanged -= txtSearchItems_TextChanged;
                //txtSearchItems.Enabled = true;
                //txtSearchItems.TextChanged += txtSearchItems_TextChanged;

                Suppliers.Insert(0, new Supplier { SupplierID = 0, Name = "Select Supplier" });
                cmbSuppliers.DataSource = Suppliers;
                cmbSuppliers.ValueMember = "SupplierId";
                cmbSuppliers.DisplayMember = "Name";
                lblLoadingSuppliers.Visible = false;
                cmbSuppliers.Enabled = true;
                if (this.StockEditID > 0)
                {
                    LoadStockForEdit();
                }
                else
                {
                    AssingWithholdingInvoice(SharedVariables.AdminPharmacySetting.WithholdingTax, SharedVariables.AdminPharmacySetting.WithholdingTaxType);
                    AssingAdvanceWithholdingInvoice(SharedVariables.AdminPharmacySetting.AdvanceTax, SharedVariables.AdminPharmacySetting.AdvanceTaxType);
                    
                    if (this.PurchaseOrderId > 0)
                    {
                        LoadPurchaseOrder();
                    }
                    this.dtpExpiry.MinDate = DateTime.Now;
                    this.dtpExpiry.Format = DateTimePickerFormat.Custom;
                    this.dtpExpiry.CustomFormat = " ";
                    dtpExpiry.Tag = 0;
                    ShowNewDocumentNumber();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AssingWithholdingInvoice(double WithholdingTax, int WithholdingTaxType)
        {
            cmbWithHoldingTaxType.SelectedIndexChanged -= cmbWithHoldingTaxType_SelectedIndexChanged;
            txtWithHoldingTax.TextChanged -= txtWithHoldingTax_TextChanged;
            try
            {
                txtWithHoldingTax.Text = WithholdingTax.ToString();
                cmbWithHoldingTaxType.SelectedIndex = WithholdingTaxType;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                txtWithHoldingTax.TextChanged += txtWithHoldingTax_TextChanged;
                cmbWithHoldingTaxType.SelectedIndexChanged += cmbWithHoldingTaxType_SelectedIndexChanged;
            }
        }

        private void AssingAdvanceWithholdingInvoice(double AdvanceWithholdingTax, int AdvanceWithholdingTaxType)
        {
            cmbWithAdvanceHoldingTaxType.SelectedIndexChanged -= cmbWithAdvanceHoldingTaxType_SelectedIndexChanged;
            txtAdvanceWithHoldingTax.TextChanged -= txtAdvanceWithHoldingTax_TextChanged;
            try
            {
                txtAdvanceWithHoldingTax.Text = AdvanceWithholdingTax.ToString();
                cmbWithAdvanceHoldingTaxType.SelectedIndex = AdvanceWithholdingTaxType;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                txtAdvanceWithHoldingTax.TextChanged += txtAdvanceWithHoldingTax_TextChanged;
                cmbWithAdvanceHoldingTaxType.SelectedIndexChanged += cmbWithAdvanceHoldingTaxType_SelectedIndexChanged;
            }
        }
        private void bgWSupplierssLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                cmbSuppliers.Enabled = false;
                lblLoadingSuppliers.Visible = true;
            });

            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            }

        }
        private void bgWSuppliersLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            Suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.Enabled = true;
            lblLoadingSuppliers.Visible = false;
        }
        private void LoadItemsAndSuppliers()
        {
            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
        }
        private void LoadSuppliers()
        {
            this.bgWSuppliersLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWSupplierssLoader_DoWork);
            this.bgWSuppliersLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWSuppliersLoader_RunWokrerCompleted);
            this.bgWSuppliersLoader.RunWorkerAsync();
        }
        private void ShowNewDocumentNumber()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtDocumentNo.Text = unitOfWork.StockRepository.GetNewDocumentNumber().ToString();
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
                if (this.IsStockPricesEditable)
                    return;
                if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                {
                    int ConsumedQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumedQty"].Value );
                    if (ConsumedQty > 0 && !(PurchaseOrderId > 0))
                    {
                        MessageBox.Show("Item Has Consumed Quantity, it Can't be Updated/Removed.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                    else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                    long StockItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockItemId"].Value);

                    double nv = 0, sv = 0, dv = 0;
                  
                        nv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value.ToString());
                        sv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colSalesTaxValue"].Value.ToString());
                        dv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscountValue"].Value.ToString());
                    
                    txtGrandTotal.Text = (SharedFunctions.StringToDouble(txtGrandTotal.Text) - nv).ToString();
                    AssignGrandTotalWithHoldingTax();
                    txtTotalSaleTax.Text = (SharedFunctions.StringToDouble(txtTotalSaleTax.Text) - sv).ToString();
                    txtTotalDiscount.Text = (SharedFunctions.StringToDouble(txtTotalDiscount.Text) - dv).ToString();

                    #region ShowHideField
                    bool grdDiscount = false, grdSaleTax = false;
                    //--------- Register Event -----------//
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                    txtAdvanceWithHoldingTax.TextChanged -= txtAdvanceWithHoldingTax_TextChanged;
                    txtWithHoldingTax.TextChanged -= txtWithHoldingTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;

                    grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                    grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
                    if (grdDiscount || grdSaleTax)
                    {
                        txtNetDiscount.Enabled = false;
                        txtNetSaleTax.Enabled = false;
                        cmbNetDiscountType.Enabled = false;
                        cmbNetSaleTaxType.Enabled = false;
                        txtNetDiscount.Text = "";
                        txtNetSaleTax.Text = "";
                        cmbNetDiscountType.Text = "%";
                        cmbNetSaleTaxType.Text = "%";
                        txtWithHoldingTax.Enabled = false;
                        cmbWithHoldingTaxType.Enabled = false;
                        txtAdvanceWithHoldingTax.Enabled = false;
                        cmbWithAdvanceHoldingTaxType.Enabled = false;
                        grdItems.Columns["colDiscount"].ReadOnly = false;
                        grdItems.Columns["colPercDiscType"].ReadOnly = false;
                        grdItems.Columns["colSalesTax"].ReadOnly = false;
                        grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;
                    }
                    else
                    {
                        txtNetDiscount.Enabled = true;
                        txtNetSaleTax.Enabled = true;
                        cmbNetDiscountType.Enabled = true;
                        cmbNetSaleTaxType.Enabled = true;
                        txtWithHoldingTax.Enabled = true;
                        cmbWithHoldingTaxType.Enabled = true;

                        txtAdvanceWithHoldingTax.Enabled = true;
                        cmbWithAdvanceHoldingTaxType.Enabled = true;
                        txtNetDiscount.Text = "";
                        txtNetSaleTax.Text = "";
                        cmbNetDiscountType.SelectedIndex = 0;
                        cmbNetSaleTaxType.SelectedIndex = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            r.Cells["colDiscount"].Value = "0";
                            r.Cells["colPercDiscType"].Value = "%";
                            r.Cells["colSalesTax"].Value = "0";
                            r.Cells["colPercSalesTaxType"].Value = "%";
                        }
                    }
                    //--------- Un Register Event -----------//
                    txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                    txtAdvanceWithHoldingTax.TextChanged += txtAdvanceWithHoldingTax_TextChanged;
                    txtWithHoldingTax.TextChanged += txtWithHoldingTax_TextChanged;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    #endregion

                    if (StockItemId > 0)
                    {
                        grdItems.Rows[e.RowIndex].Visible = false;
                        EleminateRow(e, "Update");
                    }
                    else { EleminateRow(e, "Add"); /*grdItems.Rows.RemoveAt(e.RowIndex);*/ }

                    //grdItems.Rows[e.RowIndex].Visible = false;
                    if (e.RowIndex == 0) { grdItems.Rows[e.RowIndex + 1].Selected = true; }
                    else { grdItems.Rows[e.RowIndex - 1].Selected = true; }

                    txtGrandTotal.Text = GetGrandTotal().ToString();
                    //grdItems.Rows.RemoveAt(e.RowIndex);
                    //StockItemsList.RemoveAt(e.RowIndex);
                }
                if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                {
                    Clear();
                    //int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    FillDataForEdit(grdItems.Rows[e.RowIndex]);
                    btnAddItem.Text = "Update Item";
                    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                    EditingRowIndex = e.RowIndex;
                    //cmbItems.Enabled = false;
                    this.uC_SearchItems1.Enabled = false;
                    long StockItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockItemId"].Value);
                    //if (StockItemId > 0)
                    //{
                    //    numRetailPrice.Enabled = false;
                    //}
                    //grdItems.Rows.RemoveAt(e.RowIndex);
                    // StockItemsList.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillDataForEdit(DataGridViewRow r)
        {
            txtStockItemId.Text = r.Cells["colStockItemId"].Value.ToString();
            //cmbItems.SelectedValue = r.Cells["colItemId"].Value;
            this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            using (unitOfWork = new UnitOfWork())
            {
                string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
                //cmbUnit.Items.Add(unit);
                //cmbUnit.Items.Add("Units");
            }
            this.SelectedItemName = r.Cells["colItem"].Value.ToString();
            //cmbUnit.SelectedIndex = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            numConvUnit.Value = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = r.Cells["colItem"].Value.ToString();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            numQty.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
            numUnitCost.Value = Convert.ToDecimal(r.Cells["colUnitCost"].Value);
            txtTotalCost.Text = r.Cells["colTotalCost"].Value.ToString();
            numRetailPrice.Value = Convert.ToDecimal(r.Cells["colRetailPrice"].Value);
            txtBatch.Text = string.IsNullOrEmpty(r.Cells["colBatch"].Value.ToString()) ? "" : r.Cells["colBatch"].Value.ToString();
            if (r.Cells["colExpiry"].Value == null) { dtpExpiry.CustomFormat = " "; }
            else
            {
                dtpExpiry.CustomFormat = "dd/MM/yyyy";
                if (!string.IsNullOrEmpty(r.Cells["colExpiry"].Value.ToString()))
                {
                    dtpExpiry.Value = Convert.ToDateTime(r.Cells["colExpiry"].Value);
                }
            }
            numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
            //cmbDiscountType.SelectedItem = string.IsNullOrEmpty(r.Cells["colDiscountTypeString"].Value.ToString()) ? "Value" : r.Cells["colDiscountTypeString"].Value.ToString();
            cmbDiscountType.SelectedItem = string.IsNullOrEmpty(r.Cells["colPercDiscType"].Value.ToString()) ? "Value" : r.Cells["colPercDiscType"].Value.ToString();
            numSalesTax.Value = Convert.ToDecimal(r.Cells["colSalesTax"].Value == DBNull.Value ? 0 : r.Cells["colSalesTax"].Value);
            //cmbSalesTaxType.SelectedItem = r.Cells["colSalesTaxTypeString"].Value == null ? "Value" : r.Cells["colSalesTaxTypeString"].Value.ToString();
            cmbSalesTaxType.SelectedItem = r.Cells["colPercSalesTaxType"].Value == null ? "Value" : r.Cells["colPercSalesTaxType"].Value.ToString();

            txtNetValue.Text = r.Cells["colNetValue"].Value.ToString();
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            frmAddSupplier f = new frmAddSupplier();
            f.Show();
            f.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
        }
        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            LoadSuppliers();
        }
        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                lblImagePath.Text = selectedFile;
            }
        }
        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblImagePath.Text = "No File Choosen";
        }

        private void UpdateRow(StockItem ObjStockItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colStockItemId"].Value = ObjStockItem.StockItemId;
            r.Cells["colItemId"].Value = ObjStockItem.ItemId;
            r.Cells["colItem"].Value = ObjStockItem.ItemName;

            r.Cells["colUnit"].Value = ObjStockItem.UnitString;
            r.Cells["colConvUnit"].Value = ObjStockItem.ConversionUnit;
            r.Cells["colQuantity"].Value = ObjStockItem.Quantity;
            r.Cells["colUnitCost"].Value = ObjStockItem.UnitCost;
            r.Cells["DiscountedPrice"].Value = ObjStockItem.UnitCost;
            calculateDiscPrice();
            r.Cells["colTotalCost"].Value = ObjStockItem.TotalCost;
            r.Cells["colRetailPrice"].Value = ObjStockItem.RetailPrice;
            r.Cells["colBatch"].Value = ObjStockItem.BatchName;
            r.Cells["colExpiry"].Value = ObjStockItem.BatchExpiry;
            r.Cells["colDiscount"].Value = ObjStockItem.Discount;
            r.Cells["colDiscountTypeString"].Value = ObjStockItem.DiscoutnTypeString;
            r.Cells["colSalesTax"].Value = ObjStockItem.SalesTax;
            r.Cells["colSalesTaxTypeString"].Value = ObjStockItem.SalesTaxTypeString;
            r.Cells["colNetValue"].Value = ObjStockItem.NetValue;
            r.Cells["colDiscountType"].Value = ObjStockItem.DiscountType;
            r.Cells["colSalesTaxType"].Value = ObjStockItem.SalesTaxType;
            r.Cells["colDiscountValue"].Value = ObjStockItem.DiscountValue;
            r.Cells["colSalesTaxValue"].Value = ObjStockItem.SalesTaxValue;
            r.Cells["colIsRowUpdated"].Value = true;
            r.DefaultCellStyle.BackColor = Color.White;
        }
        private bool IsItemAlreadyAdded(int ItemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsSameItemDiffBatches)
                return false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)//Convert.ToInt32(cmbItems.SelectedValue))
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            return false;
        }
        private void Clear()
        {
            //cmbItems.SelectedIndex = -1;
            //cmbItems.Enabled = true;
            this.uC_SearchItems1.Enabled = true;
            numRetailPrice.Enabled = true;
            numQty.Value = 0;
            numUnitCost.Value = 0;
            txtTotalCost.Text = "";
            numRetailPrice.Value = 0;
            txtBatch.Text = "";
            dtpExpiry.CustomFormat = " ";
            dtpExpiry.Tag = 0;
            numDiscount.Value = 0;
            cmbDiscountType.SelectedIndex = 0;
            numSalesTax.Value = 0;
            cmbSalesTaxType.SelectedIndex = 0;
            txtNetValue.Text = "0";
            btnAddItem.Text = "Add Item";
            //cmbUnit.SelectedIndex = -1;
            //cmbUnit.Items.Clear();
            numConvUnit.Value = 0;
            uC_SearchItems1.SetText = ""; // this will reset selectedItemId and SelectedItemName
            if (EditingRowIndex >= 0)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            EditingRowIndex = -1;

            //------------- Unreginter event ----------//
            txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
            txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
            cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;

            txtNetDiscount.Text = "";
            txtNetSaleTax.Text = "";
            txtTotalSaleTax.Text = "";
            txtTotalDiscount.Text = "";
            cmbNetSaleTaxType.SelectedIndex = 0;
            cmbDiscountType.SelectedIndex = 0;

            //------------------Register Events -----------------//
            txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
            txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
            cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
            cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;

            txtNetDiscount.Text = "";
            txtNetSaleTax.Text = "";
            txtTotalSaleTax.Text = "";
            txtTotalDiscount.Text = "";
            cmbNetSaleTaxType.SelectedIndex = 0;
            cmbDiscountType.SelectedIndex = 0;
            setNumericUD_toEmpty();
        }
        private bool IsValidInput()
        {

            bool ErrFound = false;
            if (this.SelectedItemId <= 0)//(cmbItems.SelectedIndex < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                this.uC_SearchItems1.Focus();
                return false;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (numQty.Value <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                numQty.Focus();
                return false;
            }
            else
            {
                ErrQuantity.Visible = false;
            }

            if (numConvUnit.Value <= 0)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                numConvUnit.Focus();
                return false;
            }
            else
            {
                errConvUnit.Visible = false;
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
        private void FillObject(StockItem objStockItem)
        {
            objStockItem.StockItemId = int.Parse(txtStockItemId.Text);
            objStockItem.ItemId = this.SelectedItemId;//int.Parse(cmbItems.SelectedValue.ToString());
            objStockItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);
            //objStockItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objStockItem.ConversionUnit = (int)numConvUnit.Value;
            objStockItem.Quantity = (int)numQty.Value;
            objStockItem.UnitCost = (double)numUnitCost.Value;
            objStockItem.TotalCost = double.Parse(txtTotalCost.Text);
            objStockItem.RetailPrice = (double)numRetailPrice.Value;
            objStockItem.BatchName = txtBatch.Text.Trim() == "" ? "No Batch" : txtBatch.Text.Trim();
            if (dtpExpiry.Tag.ToString().Equals("1"))
            {
                objStockItem.BatchExpiry = dtpExpiry.Value;
            }
            else
            {
                objStockItem.BatchExpiry = null;
            }
            objStockItem.Discount = (double)numDiscount.Value;
            string DiscountTypeString = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            objStockItem.DiscoutnTypeString = DiscountTypeString;
            objStockItem.DiscountType = DiscountTypeString.ToLower() == "lumpsum" ? 0 : 1;
            string SalesTaxTypeString = cmbSalesTaxType.GetItemText(cmbSalesTaxType.SelectedItem);
            objStockItem.SalesTaxTypeString = SalesTaxTypeString;
            objStockItem.SalesTaxType = SalesTaxTypeString.ToLower() == "lumpsum" ? 0 : 1;
            objStockItem.SalesTax = (double)numSalesTax.Value;
            objStockItem.NetValue = double.Parse(txtNetValue.Text);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            if (objStockItem.DiscountType == 1)
            {
                objStockItem.DiscountValue = objStockItem.Discount;
            }
            else
            {
                objStockItem.DiscountValue = objStockItem.Discount * objStockItem.Quantity;
            }

            if (objStockItem.SalesTaxType == 1)
            {
                objStockItem.SalesTaxValue = objStockItem.SalesTax;
            }
            else
            {
                objStockItem.SalesTaxValue = objStockItem.SalesTax * objStockItem.Quantity;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void cmbSalesTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numUnitCost_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void CalculateValues(DataGridViewRow r)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            double TotalCost = 0;
            int qty = SharedFunctions.StringToInt(r.Cells["colQuantity"].Value); //(int)numQty.Value;
            int Bonqty = SharedFunctions.StringToInt(r.Cells["colBonusQty"].Value); //(int)numQty.Value;
            double UnitCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);//(double)numUnitCost.Value;
            double Disc = SharedFunctions.StringToDouble(r.Cells["colDiscount"].Value);//(double)numDiscount.Value;
            double SalesTax = SharedFunctions.StringToDouble(r.Cells["colSalesTax"].Value);
            double WHT = SharedFunctions.StringToDouble(r.Cells["colWHT"].Value);//(double)numSalesTax.Value;
            double AdvanceWHT = SharedFunctions.StringToDouble(r.Cells["colAdvanceWHT"].Value);
            //TotalCost = TotalCost - ((TotalCost / qty) * Bonqty);
            //txtTotalCost.Text = TotalCost.ToString();
            TotalCost = qty * UnitCost;
            r.Cells["colTotalCost"].Value = TotalCost;
            string DiscType = r.Cells["colDiscType"].Value.ToString();
            string PercDiscType = r.Cells["colPercDiscType"].Value.ToString();
            string StType = r.Cells["colSaleStaxType"].Value.ToString();
            string PercStType = r.Cells["colPercSaleStaxType"].Value.ToString();
            string WHTType = r.Cells["colWHTType"].Value.ToString();
            string AdvanceWHTType = r.Cells["colAdvanceWHTType"].Value.ToString();
            //int netValue1 = Convert.ToInt32(r.Cells["colNetValue"].Value);
            
            if (DiscType.ToLower() == "per item")
            {
                Disc = qty * Disc;
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }
            else
            {
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }

            if (StType.ToLower() == "per item")
            {
                SalesTax = qty * SalesTax;
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * SalesTax);
                }
            }
            else
            {
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * UnitCost);
                }
            }
            double newnetvalue = TotalCost - Disc + SalesTax;
            if (WHTType == "%")
            {
                WHT = (WHT / 100) * newnetvalue;   
            }
            else
            {
                    WHT = WHT;
            }

            if (AdvanceWHTType == "%")
            {
                AdvanceWHT = (AdvanceWHT / 100) * newnetvalue;
            }
            else
            {
                AdvanceWHT = AdvanceWHT;
            }
            double bonDisc = Bonqty * UnitCost;
            r.Cells["colBonusDiscount"].Value = bonDisc;
            //get discount values for bonus quantity

            double netValue = newnetvalue; 
            r.Cells["colNetValue"].Value = netValue + WHT + AdvanceWHT;
            r.Cells["colDiscountValue"].Value = Disc;
            r.Cells["colSalesTaxValue"].Value = SalesTax;
            double RetailPrice = 0, CostPrice = 0;
            RetailPrice = SharedFunctions.StringToDouble(r.Cells["colRetailPrice"].Value);
            CostPrice = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);
            //double.TryParse(r.Cells["colRetailPrice"].Value.ToString(),out RetailPrice);
            //double.TryParse(r.Cells["colUnitCost"].Value.ToString(), out CostPrice);
            r.Cells["colNetProfit"].Value = Profit(RetailPrice, CostPrice);
            r.Cells["colGrossMargin"].Value = GrossMargin(qty, RetailPrice, netValue);

            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void CalculateRetail(DataGridViewRow r)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            double TotalCost = 0;
            double Profit = SharedFunctions.StringToDouble(r.Cells["colNetProfit"].Value);
            double UnitCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);//(double)numUnitCost.Value;

            r.Cells["colRetailPrice"].Value = GetRetailPrice(UnitCost, Profit);
            double RetailPrice = SharedFunctions.StringToDouble(r.Cells["colRetailPrice"].Value);
            int qty = SharedFunctions.StringToInt(r.Cells["colQuantity"].Value); //(int)numQty.Value;
            int Bonqty = SharedFunctions.StringToInt(r.Cells["colBonusQty"].Value); //(int)numQty.Value;
            double Disc = SharedFunctions.StringToDouble(r.Cells["colDiscount"].Value);//(double)numDiscount.Value;
            double SalesTax = SharedFunctions.StringToDouble(r.Cells["colSalesTax"].Value);//(double)numSalesTax.Value;
            //TotalCost = TotalCost - ((TotalCost / qty) * Bonqty);
            //txtTotalCost.Text = TotalCost.ToString();
            TotalCost = qty * UnitCost;
            r.Cells["colTotalCost"].Value = TotalCost;
            string DiscType = r.Cells["colDiscType"].Value.ToString();
            string PercDiscType = r.Cells["colPercDiscType"].Value.ToString();
            string StType = r.Cells["colSaleStaxType"].Value.ToString();
            string PercStType = r.Cells["colPercSaleStaxType"].Value.ToString();
            if (DiscType.ToLower() == "per item")
            {
                Disc = qty * Disc;
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }
            else
            {
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }

            if (StType.ToLower() == "per item")
            {
                SalesTax = qty * SalesTax;
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * SalesTax);
                }
            }
            else
            {
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * UnitCost);
                }
            }
            double bonDisc = Bonqty * UnitCost;
            r.Cells["colBonusDiscount"].Value = bonDisc;
            //get discount values for bonus quantity
            double netValue = TotalCost - Disc + SalesTax;
            r.Cells["colNetValue"].Value = netValue;
            r.Cells["colDiscountValue"].Value = Disc;
            r.Cells["colSalesTaxValue"].Value = SalesTax;
            //double CostPrice = 0;
            //CostPrice = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);
            //double.TryParse(r.Cells["colRetailPrice"].Value.ToString(),out RetailPrice);
            //double.TryParse(r.Cells["colUnitCost"].Value.ToString(), out CostPrice);
            //r.Cells["colNetProfit"].Value = Profit(RetailPrice, CostPrice);
            r.Cells["colGrossMargin"].Value = GrossMargin(qty, RetailPrice, netValue);
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    ///StockItemsList.Clear();
                    this.ShowNewDocumentNumber();
                    this.dtpDate.Value = DateTime.Now;
                    this.dtpSupplierInvoiceDate.Value = DateTime.Now;
                    this.cmbSuppliers.SelectedIndex = -1;
                    this.txtSupplierInvoiceNo.Text = "";
                    this.txtGrandTotal.Text = "0";
                    AssignGrandTotalWithHoldingTax();
                    this.isStockLoading = false;
                    this.IsLoadingPrvSoldItems = false;
                    #region ShowHideField
                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    grdItems.Columns["colDiscount"].ReadOnly = false;
                    grdItems.Columns["colPercDiscType"].ReadOnly = false;
                    grdItems.Columns["colSalesTax"].ReadOnly = false;
                    grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;

                    txttotalofqty.Clear();
                    txttotalofrate.Clear();
                    txtTotalofCost.Clear();
                    txttotalofbonusqty.Clear();
                    #endregion
                    isholdchekc = 0;
                }
            }
        }
        private bool checkWHT(DataGridView r)
        {
            bool st = false;
            try
            {
                foreach (DataGridViewRow row in r.Rows)
                {
                    if (Convert.ToDouble(row.Cells["colWHT"].Value) > 0)
                    {
                        st = true;

                    }
                }
            }
            catch (Exception)
            {
                st = false;
            }
            return st;
           
        }
        private bool checkAdvanceWHT(DataGridView r)
        {
            bool st = false;
            try
            {
                foreach (DataGridViewRow row in r.Rows)
                {
                    if (Convert.ToDouble(row.Cells["colAdvanceWHT"].Value) > 0)
                    {
                        st = true;

                    }
                }
            }
            catch (Exception)
            {
                st = false;
            }
            return st;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int invisiblerows = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                if (!row.Visible)
                {
                    invisiblerows++;
                }
            }
            if ((grdItems.Rows.Count - invisiblerows) <= 0)
            {
                MessageBox.Show("Please Add Some Items Before Saving", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            invisiblerows = 0;
            if (cmbSuppliers.Text == "Select Supplier" || cmbSuppliers.Text == "")
            {
                MessageBox.Show("Please Select Supplier it must be mandatory", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                int supplierId = getSupplierID();
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (Convert.ToInt32(r.Cells["colQuantity"].Value) < 1 && r.Visible)
                    {
                        var indx = r.Cells["colQuantity"].RowIndex;
                        grdItems.CurrentCell = grdItems.Rows[Convert.ToInt32(indx)].Cells["colQuantity"];
                        grdItems.BeginEdit(true);
                        int index = grdItems.CurrentRow.Index;
                        MessageBox.Show("Some items have 0 quantity, please check before proceeding.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                //if (grdItems.Rows.Count > 50)
                //{
                //    label31.Visible = true;
                //    MessageBox.Show("Cann't add more than 50 items in one stock.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                if (IsMandatoryFields())
                    return;

                if (this.StockEditID > 0)
                {
                    UpdateStock();
                    isholdchekc = 0;
                }
                else// this will be used in case of ADD-STOCK from purchase order as well.
                {
                    InsertStock();
                    isholdchekc = 0;
                }

                //ctrl+P working
                if(controlVal == "StockPaymentShow")
                {
                    showSupplierPayment(supplierId);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private bool IsMandatoryFields()
        {
            bool IsRequired = false;
            if (SharedVariables.AdminPharmacySetting.IsStockFieldMandatory)
            {
                if ((from DataGridViewRow r in grdItems.Rows select SharedFunctions.ObjectToString(r.Cells["colExpiry"].Value)).Any(x => x == ""))
                {
                    SharedFunctions.MessageBoxShow("Please add Batch and Expiry.", MessageType.Info);
                    return IsRequired = true;
                }
                if ((from DataGridViewRow r in grdItems.Rows select SharedFunctions.ObjectToString(r.Cells["colBatch"].Value)).Any(x => x == ""))
                {
                    SharedFunctions.MessageBoxShow("Please add Batch and Expiry.", MessageType.Info);
                    return IsRequired = true;
                }
            }
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                foreach (DataGridViewRow item in grdItems.Rows)
                {
                    DataGridViewComboBoxCell re = item.Cells["colStores"] as DataGridViewComboBoxCell;
                    if (re != null)
                        if (re.Value.ToString() == "0")
                        {
                            SharedFunctions.MessageBoxShow("Store is required on all items.", MessageType.Info);
                            return IsRequired = true;
                        }
                }
            }
            //if(grdItems.Rows.Count > 50)
            //{
            //    SharedFunctions.MessageBoxShow("You can't add more then 50 items in one stock.", MessageType.Warning);
            //    return IsRequired = true;
            //}
            if (SharedVariables.AdminPharmacySetting.IsSameItemDiffBatches)
            {
                List<StockItem> list = new List<StockItem>();
                foreach (DataGridViewRow item in grdItems.Rows)
                {
                    int ItemId = SharedFunctions.StringToInt(item.Cells["colItemId"].Value);
                    string BatchName = SharedFunctions.ObjectToString(item.Cells["colBatch"].Value).ToLower().Trim();
                    if (item.Visible == true)
                    {
                        list.Add(new StockItem() { ItemId = ItemId, BatchName = BatchName });
                    }
                }
                if(list.GroupBy(x => new { x.ItemId, x.BatchName }).Where(g => g.Count() > 1).Count() > 0)
                {
                    SharedFunctions.MessageBoxShow("Same item should have different batches", MessageType.Warning);
                    return IsRequired = true;
                }
            }
                        
            return IsRequired;
        }
        private void InsertStock()
        {
            List<Item> addItems = new List<Item>();
            List<Item> updateItems = new List<Item>();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock objStock = new Stock();
                        objStock.Supplier = unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                        objStock.IsHolding = 0;
                        objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                        if (lblImagePath.Text.ToLower() == "no file choosen")
                            objStock.ImagePath = null;
                        else
                            objStock.ImagePath = lblImagePath.Text;
                        if (objStock.ImagePath != null)
                        {
                            objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                        }
                        objStock.StockDate = dtpDate.Value;
                        objStock.CreateDate = DateTime.Now;
                        objStock.UpdateDate = DateTime.Now;

                        //-------- Discount--------//
                        objStock.NetDiscount = SharedFunctions.StringToDouble(txtNetDiscount.Text);
                        objStock.NetDiscountType = cmbNetDiscountType.Text == "%" ? 0 : 1;
                        objStock.TotalDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                        //-------- Sales Tax----------//
                        objStock.NetSaleTax = SharedFunctions.StringToDouble(txtNetSaleTax.Text);
                        objStock.NetSaleTaxType = cmbNetSaleTaxType.Text == "%" ? 0 : 1;
                        objStock.TotalSaleTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                        //-------- Withholding Tax----------//
                        objStock.WithholdingTax = SharedFunctions.StringToDouble(txtWithHoldingTax.Text);
                        objStock.WithholdingTaxType = SharedFunctions.StringToInt(cmbWithHoldingTaxType.SelectedIndex);
                        //-------- Advanceholding Tax----------//
                        objStock.AdvanceWithholdingTax = SharedFunctions.StringToDouble(txtAdvanceWithHoldingTax.Text);
                        objStock.AdvanceWithholdingTaxType = SharedFunctions.StringToInt(cmbWithAdvanceHoldingTaxType.SelectedIndex);
                        
                        objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                        //objStock.DocumentNo = decimal.Parse(txtDocumentNo.Text);
                        objStock.IsNew = true;
                        objStock.IsActive = true;
                        objStock.IsHolding = 0;
                        objStock.Ishold = false;
                        objStock.GrandTotal = Math.Round(double.Parse(txtGrandTotal.Text), 3);
                        objStock.TotalPaid = 0;
                        objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objStock.PurchaseOrder = unitOfWork.PurchaseOrderRepository.GetById(this.PurchaseOrderId);
                        objStock.StockItems = new List<StockItem>();
                        List<int> itemIds = new List<int>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            if (r.Visible)
                            {
                                StockItem objStockItem = new StockItem();

                                FillObject(r, objStockItem);

                                if (SharedFunctions.IsDoubleFormat(objStockItem.RetailPrice, "Stock Item Retail Price"))
                                    return;
                                if (SharedFunctions.IsDoubleFormat(objStockItem.UnitCost, "Stock Item Unit Price"))
                                    return;
                                if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && objStockItem.RetailPrice < objStockItem.UnitCost)
                                {
                                    MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //update item for retail price
                                Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemID);
                                if (!objItem.IsSyncable)
                                {
                                    objItem.IsSyncable = true;
                                }
                                else if (objItem.IsSynced)
                                {
                                    objItem.IsNew = false;
                                    objItem.IsUpdate = true;
                                    if (r.Visible)
                                    {
                                        objItem.IsActive = true;
                                    }
                                    else
                                    {
                                        objItem.IsActive = false;
                                    }
                                    objItem.IsSynced = false;
                                }
                                objItem.UpdateDate = DateTime.Now;
                                objItem.RetailPrice = objStockItem.RetailPrice;
                                objItem.UnitCostPrice = objStockItem.UnitCost;
                                if (objItem.IsFileItem)
                                {
                                    objItem.IsFileItem = false;
                                    addItems.Add(objItem);
                                }
                                else
                                {
                                    updateItems.Add(objItem);
                                }
                                unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                                //unitOfWork.ItemRspository.Update(objItem);
                                //end update item for retail price
                                objStock.StockItems.Add(objStockItem);
                                itemIds.Add(objStockItem.ItemId);

                            }
                        }
                        List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                        foreach (var i in msList)
                        {
                            i.UpdatedAt = DateTime.Now;
                            i.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            i.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            unitOfWork.MissedsalesRepository.Update(i);
                        }
                        objStock.DocumentNo = unitOfWork.StockRepository.GetNewDocumentNumber();
                        unitOfWork.StockRepository.Insert(objStock);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + SharedFunctions.StringToLong(objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.created);
                        unitOfWork.Save();
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {

                        }
                        else
                        {
                            //SOT --- Manage Items
                            if (addItems != null && addItems.Count() > 0)
                            {
                                var itemAddBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                                foreach (var item in itemAddBatches)
                                {
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                                }
                            }
                            if (updateItems != null && updateItems.Count() > 0)
                            {
                                var itemUpdateBatches = SharedFunctions.ChunkBy(updateItems, SharedVariables.ItemBatch);
                                foreach (var item in itemUpdateBatches)
                                {
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Update, 0);
                                }
                            }
                        }
                        //SOT --- Manage batches
                        List<Batch> batchList = objStock.StockItems.Select(x => x.Batch).ToList();
                        if (batchList != null && batchList.Count() > 0)
                        {
                            List<Batch> createBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "0").ToList();
                            if (createBatchList != null && createBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, createBatchList, ModuleName.BulkBatches, ModuleAction.Create, 0);
                            List<Batch> updatedBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "1").ToList();
                            if (updatedBatchList != null && updatedBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, updatedBatchList, ModuleName.BulkBatches, ModuleAction.Update, 0);
                        }
                        //SOT --- Manage Stock
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Create, objStock.StockId);

                        if (this.PurchaseOrderId > 0)
                        {
                            PurchaseOrder po = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(this.PurchaseOrderId);
                            if (po != null)
                            {
                                po.StockId = objStock.StockId;
                                unitOfWork.PurchaseOrderRepository.Update(po);
                                unitOfWork.Save();
                                //SOT
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, po, ModuleName.PurchaseOrder, ModuleAction.Update, po.PurchaseOrderId);
                            }
                        }
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
            MessageBox.Show("Items Stock Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SharedVariables.manageStockRefr = true;
            this.Close();
        }
        private void UpdateStock()
        {

            List<Item> addItems = new List<Item>();
            List<Item> updateItems = new List<Item>();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock objStock = unitOfWork.StockRepository.GetStockById_Edit(this.StockEditID);
                        objStock.IsHolding = 0;
                        objStock.Supplier = unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                        objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                        if (lblImagePath.Text.ToLower() == "no file choosen")
                            objStock.ImagePath = null;
                        else
                            objStock.ImagePath = lblImagePath.Text;
                        if (objStock.ImagePath != null)
                        {
                            objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                        }
                        //-------- Discount--------//
                        objStock.NetDiscount = SharedFunctions.StringToDouble(txtNetDiscount.Text);
                        objStock.NetDiscountType = cmbNetDiscountType.Text == "%" ? 0 : 1;
                        objStock.TotalDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                        //-------- Sales Tax----------//
                        objStock.NetSaleTax = SharedFunctions.StringToDouble(txtNetSaleTax.Text);
                        objStock.NetSaleTaxType = cmbNetSaleTaxType.Text == "%" ? 0 : 1;
                        objStock.TotalSaleTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                        //-------- Withholding Tax----------//
                        objStock.WithholdingTax = SharedFunctions.StringToDouble(txtWithHoldingTax.Text);
                        objStock.WithholdingTaxType = SharedFunctions.StringToInt(cmbWithHoldingTaxType.SelectedIndex);
                        //-------- Advanceholding Tax----------//
                        objStock.AdvanceWithholdingTax = SharedFunctions.StringToDouble(txtAdvanceWithHoldingTax.Text);
                        objStock.AdvanceWithholdingTaxType = SharedFunctions.StringToInt(cmbWithAdvanceHoldingTaxType.SelectedIndex);

                        objStock.UpdateDate = DateTime.Now;
                        if (objStock.IsSynced)
                        {
                            objStock.IsNew = false;
                            objStock.IsUpdate = true;
                            objStock.IsSynced = false;
                        }
                        objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                        objStock.GrandTotal = Math.Round(double.Parse(txtGrandTotal.Text), 3);
                        objStock.TotalPaid = 0;
                        objStock.IsActive = true;
                        objStock.Ishold = false;
                        objStock.IsHolding = 0;
                        objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        bool isExistingItem = false;
                        bool isDeleted = false;
                        long StockItemId = -1;
                        bool IsRowUpdated = false;
                        List<int> itemIds = new List<int>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            
                                IsRowUpdated = false;
                                if (r.Cells["colIsRowUpdated"].Value != null)
                                {
                                    bool.TryParse(r.Cells["colIsRowUpdated"].Value.ToString(), out IsRowUpdated);
                                }
                                isDeleted = r.Visible ? false : true;
                                //if (!IsRowUpdated && !isDeleted)
                                //{
                                //    continue;
                                //}
                                isExistingItem = false;
                                StockItemId = Convert.ToInt32(r.Cells["colStockItemId"].Value);
                                StockItemId = StockItemId == 0 ? -1 : StockItemId; // because each new item has default value of 0 from stockitemid
                                StockItem objStockItem = new StockItem();
                                objStockItem.StockItemId = StockItemId;

                                objStockItem.IsActive = true;

                                //if (r.Visible)
                                {
                                    FillObject(r, objStockItem);
                                }
                                if (SharedFunctions.IsDoubleFormat(objStockItem.RetailPrice, "Stock Item Retail Price"))
                                    return;
                                if (SharedFunctions.IsDoubleFormat(objStockItem.UnitCost, "Stock Item Unit Price"))
                                    return;
                                foreach (StockItem i in objStock.StockItems)
                                {
                                    if (i.StockItemId == StockItemId && i.StockItemId != -1) // -1 shows newly added item
                                    {
                                        i.Batch = objStockItem.Batch;
                                        i.Unit = objStockItem.Unit;
                                        i.Quantity = objStockItem.Quantity;
                                        i.BonusQuantity = objStockItem.BonusQuantity;
                                        i.UnitCost = objStockItem.UnitCost;
                                        i.TotalCost = objStockItem.TotalCost;
                                        i.RetailPrice = objStockItem.RetailPrice;
                                        i.Discount = objStockItem.Discount;
                                        i.DiscountType = objStockItem.DiscountType;
                                        i.PercDiscType = objStockItem.PercDiscType;
                                        i.AdvanceWHT = objStockItem.AdvanceWHT;
                                        i.AdvanceWHTType = objStockItem.AdvanceWHTType;
                                        i.WHT = objStockItem.WHT;
                                        i.WHTType = objStockItem.WHTType;
                                        i.SalesTax = objStockItem.SalesTax;
                                        i.SalesTaxType = objStockItem.SalesTaxType;
                                        i.PercSalesTaxType = objStockItem.PercSalesTaxType;
                                        i.NetValue = objStockItem.NetValue;
                                        i.DiscountValue = objStockItem.DiscountValue;
                                        i.SalesTaxValue = objStockItem.SalesTaxValue;
                                        i.UpdatedAt = objStockItem.UpdatedAt;
                                        i.IsUpdate = true;
                                        i.IsActive = true;
                                        i.BatchExpiry = objStockItem.BatchExpiry;
                                        i.Store = objStockItem.Store;
                                        if (isDeleted)
                                        {
                                            i.IsActive = r.Visible;
                                            i.DeletedAt = DateTime.Now;
                                        }
                                        i.IsSynced = false;
                                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                        //update item for retail price
                                        Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.ItemId);
                                        objItem.IsSynced = false;
                                        objItem.IsUpdate = true;
                                        objItem.UpdateDate = DateTime.Now;
                                        objItem.RetailPrice = objStockItem.RetailPrice;
                                        objItem.UnitCostPrice = objStockItem.UnitCost;
                                        if (objItem.IsFileItem)
                                        {
                                            objItem.IsFileItem = false;
                                            addItems.Add(objItem);
                                        }
                                        else
                                        {
                                            updateItems.Add(objItem);
                                        }
                                        unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                                        unitOfWork.ItemRspository.Update(objItem);
                                        //end update item for retail price
                                        isExistingItem = true;
                                        itemIds.Add(objStockItem.ItemId);
                                        break;
                                    }
                                }
                                if (!isExistingItem)
                                {
                                    //update item for retail price
                                    Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemID);
                                    if (!objItem.IsSyncable)
                                    {
                                        objItem.IsSyncable = true;
                                    }
                                    else if (objItem.IsSynced)
                                    {
                                        objItem.IsNew = false;
                                        objItem.IsUpdate = true;
                                        objItem.IsSynced = false;
                                    }

                                    if (objItem.IsFileItem)
                                    {
                                        objItem.IsFileItem = false;
                                        addItems.Add(objItem);
                                    }
                                    else
                                    {
                                        updateItems.Add(objItem);
                                    }

                                    objItem.UpdateDate = DateTime.Now;
                                    objItem.RetailPrice = objStockItem.RetailPrice;
                                    unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                                    unitOfWork.ItemRspository.Update(objItem);
                                    //end update item for retail price
                                    objStock.StockItems.Add(objStockItem);
                                }

                           
                        }
                        List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                        foreach (var i in msList)
                        {
                            i.UpdatedAt = DateTime.Now;
                            i.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            i.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            unitOfWork.MissedsalesRepository.Update(i);
                        }
                        unitOfWork.StockRepository.Update(objStock);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + ((long)objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.updated);
                        unitOfWork.Save();
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
                        {

                        }
                        else
                        {
                            //SOT ----- Manage Items
                            if (addItems != null && addItems.Count() > 0)
                            {
                                var itemAddBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                                foreach (var item in itemAddBatches)
                                {
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                                }
                            }
                            if (updateItems != null && updateItems.Count() > 0)
                            {
                                var itemUpdateBatches = SharedFunctions.ChunkBy(updateItems, SharedVariables.ItemBatch);
                                foreach (var item in itemUpdateBatches)
                                {
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Update, 0);
                                }
                            }
                        }
                        //SOT ----- Manage Batches
                        List<Batch> batchList = objStock.StockItems.Select(x => x.Batch).ToList();
                        if (batchList != null && batchList.Count() > 0)
                        {
                            List<Batch> createBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "0").ToList();
                            if (createBatchList != null && createBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, createBatchList, ModuleName.BulkBatches, ModuleAction.Create, 0);
                            List<Batch> updatedBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "1").ToList();
                            if (updatedBatchList != null && updatedBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, updatedBatchList, ModuleName.BulkBatches, ModuleAction.Update, 0);
                        }
                        //SOT ----- Manage Stock
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Update, objStock.StockId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
            if (iii == 0)
            {
                MessageBox.Show("Items Stock Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Items Stock Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                iii = 0;
            }
            SharedVariables.manageStockRefr = true;
            this.Close();

        }
        private void HoldStock()
        {
            List<Item> addItems = new List<Item>();
            List<Item> updateItems = new List<Item>();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock objStock = new Stock();
                        objStock.Supplier = unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                        objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                        if (lblImagePath.Text.ToLower() == "no file choosen")
                            objStock.ImagePath = null;
                        else
                            objStock.ImagePath = lblImagePath.Text;
                        if (objStock.ImagePath != null)
                        {
                            objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                        }
                        objStock.StockDate = dtpDate.Value;
                        objStock.CreateDate = DateTime.Now;
                        objStock.UpdateDate = DateTime.Now;
                        objStock.Ishold = true;

                        //-------- Discount--------//
                        objStock.NetDiscount = SharedFunctions.StringToDouble(txtNetDiscount.Text);
                        objStock.NetDiscountType = cmbNetDiscountType.Text == "%" ? 0 : 1;
                        objStock.TotalDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                        //-------- Sales Tax----------//
                        objStock.NetSaleTax = SharedFunctions.StringToDouble(txtNetSaleTax.Text);
                        objStock.NetSaleTaxType = cmbNetSaleTaxType.Text == "%" ? 0 : 1;
                        objStock.TotalSaleTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                        //-------- Withholding Tax----------//
                        objStock.WithholdingTax = SharedFunctions.StringToDouble(txtWithHoldingTax.Text);
                        objStock.WithholdingTaxType = SharedFunctions.StringToInt(cmbWithHoldingTaxType.SelectedIndex);

                        objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                        objStock.DocumentNo = decimal.Parse(txtDocumentNo.Text);
                        objStock.IsNew = true;
                        objStock.IsActive = false;
                        objStock.GrandTotal = Math.Round(double.Parse(txtGrandTotal.Text), 3);
                        objStock.TotalPaid = 0;
                        objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objStock.PurchaseOrder = unitOfWork.PurchaseOrderRepository.GetById(this.PurchaseOrderId);
                        objStock.IsHolding = 1;
                        objStock.StockItems = new List<StockItem>();
                        List<int> itemIds = new List<int>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            StockItem objStockItem = new StockItem();
                            FillObject1(r, objStockItem);
                            if (SharedFunctions.IsDoubleFormat(objStockItem.RetailPrice, "Stock Item Retail Price"))
                                return;
                            if (SharedFunctions.IsDoubleFormat(objStockItem.UnitCost, "Stock Item Unit Price"))
                                return;
                            if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && objStockItem.RetailPrice < objStockItem.UnitCost)
                            {
                                MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //update item for retail price
                            Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemID);
                            if (!objItem.IsSyncable)
                            {
                                objItem.IsSyncable = true;
                            }
                            else if (objItem.IsSynced)
                            {
                                objItem.IsNew = false;
                                objItem.IsUpdate = true;
                                objItem.IsSynced = false;
                            }
                            objItem.UpdateDate = DateTime.Now;
                            //objItem.RetailPrice = objStockItem.RetailPrice;
                            //objItem.UnitCostPrice = objStockItem.UnitCost;
                            if (objItem.IsFileItem)
                            {
                                objItem.IsFileItem = false;
                                addItems.Add(objItem);
                            }
                            else
                            {
                                updateItems.Add(objItem);
                            }
                            unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                            //unitOfWork.ItemRspository.Update(objItem);
                            //end update item for retail price
                            objStock.StockItems.Add(objStockItem);
                            itemIds.Add(objStockItem.ItemId);
                        }
                        List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                        foreach (var i in msList)
                        {
                            i.UpdatedAt = DateTime.Now;
                            i.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            i.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            unitOfWork.MissedsalesRepository.Update(i);
                        }
                        unitOfWork.StockRepository.Insert(objStock);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + SharedFunctions.StringToLong(objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.created);
                        unitOfWork.Save();
                        //SOT --- Manage Items
                        if (addItems != null && addItems.Count() > 0)
                        {
                            var itemAddBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                            foreach (var item in itemAddBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                            }
                        }
                        if (updateItems != null && updateItems.Count() > 0)
                        {
                            var itemUpdateBatches = SharedFunctions.ChunkBy(updateItems, SharedVariables.ItemBatch);
                            foreach (var item in itemUpdateBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Update, 0);
                            }
                        }
                        //SOT --- Manage batches
                        List<Batch> batchList = objStock.StockItems.Select(x => x.Batch).ToList();
                        if (batchList != null && batchList.Count() > 0)
                        {
                            List<Batch> createBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "0").ToList();
                            if (createBatchList != null && createBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, createBatchList, ModuleName.BulkBatches, ModuleAction.Create, 0);
                            List<Batch> updatedBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "1").ToList();
                            if (updatedBatchList != null && updatedBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, updatedBatchList, ModuleName.BulkBatches, ModuleAction.Update, 0);
                        }
                        //SOT --- Manage Stock
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Create, objStock.StockId);

                        if (this.PurchaseOrderId > 0)
                        {
                            PurchaseOrder po = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(this.PurchaseOrderId);
                            if (po != null)
                            {
                                po.StockId = objStock.StockId;
                                unitOfWork.PurchaseOrderRepository.Update(po);
                                unitOfWork.Save();
                                //SOT
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, po, ModuleName.PurchaseOrder, ModuleAction.Update, po.PurchaseOrderId);
                            }
                        }
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
           // MessageBox.Show("Items Stock Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            totalholds = 0;
            ShowHoldingStock(0);
            Clear();
        }
        private void UpdateHoldStock()
        {
            List<Item> addItems = new List<Item>();
            List<Item> updateItems = new List<Item>();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock objStock = unitOfWork.StockRepository.GetStockById_Edit(this.StockEditID);
                        objStock.Supplier = unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                        objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                        if (lblImagePath.Text.ToLower() == "no file choosen")
                            objStock.ImagePath = null;
                        else
                            objStock.ImagePath = lblImagePath.Text;
                        if (objStock.ImagePath != null)
                        {
                            objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                        }
                        //-------- Discount--------//
                        objStock.NetDiscount = SharedFunctions.StringToDouble(txtNetDiscount.Text);
                        objStock.NetDiscountType = cmbNetDiscountType.Text == "%" ? 0 : 1;
                        objStock.TotalDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                        //-------- Sales Tax----------//
                        objStock.NetSaleTax = SharedFunctions.StringToDouble(txtNetSaleTax.Text);
                        objStock.NetSaleTaxType = cmbNetSaleTaxType.Text == "%" ? 0 : 1;
                        objStock.TotalSaleTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                        //-------- Withholding Tax----------//
                        objStock.WithholdingTax = SharedFunctions.StringToDouble(txtWithHoldingTax.Text);
                        objStock.WithholdingTaxType = SharedFunctions.StringToInt(cmbWithHoldingTaxType.SelectedIndex);

                        objStock.UpdateDate = DateTime.Now;
                        if (objStock.IsSynced)
                        {
                            objStock.IsNew = false;
                            objStock.IsUpdate = true;
                            objStock.IsSynced = false;
                        }
                        objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                        objStock.GrandTotal = Math.Round(double.Parse(txtGrandTotal.Text), 3);
                        objStock.TotalPaid = 0;
                        objStock.Ishold = true;
                        objStock.IsHolding = 1;
                        objStock.IsActive = false;
                        objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        bool isExistingItem = false;
                        bool isDeleted = false;
                        long StockItemId = -1;
                        bool IsRowUpdated = false;
                        List<int> itemIds = new List<int>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            IsRowUpdated = false;
                            if (r.Cells["colIsRowUpdated"].Value != null)
                            {
                                bool.TryParse(r.Cells["colIsRowUpdated"].Value.ToString(), out IsRowUpdated);
                            }
                            isDeleted = r.Visible ? false : true;
                            //if (!IsRowUpdated && !isDeleted)
                            //{
                            //    continue;
                            //}
                            isExistingItem = false;
                            StockItemId = Convert.ToInt32(r.Cells["colStockItemId"].Value);
                            StockItemId = StockItemId == 0 ? -1 : StockItemId; // because each new item has default value of 0 from stockitemid
                            StockItem objStockItem = new StockItem();
                            objStockItem.IsActive = false;
                            objStockItem.StockItemId = StockItemId;
                          
                            FillObject1(r, objStockItem);
                            if (SharedFunctions.IsDoubleFormat(objStockItem.RetailPrice, "Stock Item Retail Price"))
                                return;
                            if (SharedFunctions.IsDoubleFormat(objStockItem.UnitCost, "Stock Item Unit Price"))
                                return;
                            foreach (StockItem i in objStock.StockItems)
                            {
                                if (i.StockItemId == StockItemId && i.StockItemId != -1) // -1 shows newly added item
                                {
                                    i.Batch = objStockItem.Batch;
                                    
                                    i.Unit = objStockItem.Unit;
                                    i.Quantity = objStockItem.Quantity;
                                    i.BonusQuantity = objStockItem.BonusQuantity;
                                    i.UnitCost = objStockItem.UnitCost;
                                    i.TotalCost = objStockItem.TotalCost;
                                    i.RetailPrice = objStockItem.RetailPrice;
                                    i.Discount = objStockItem.Discount;
                                    i.DiscountType = objStockItem.DiscountType;
                                    i.PercDiscType = objStockItem.PercDiscType;
                                    i.SalesTax = objStockItem.SalesTax;
                                    i.SalesTaxType = objStockItem.SalesTaxType;
                                    i.PercSalesTaxType = objStockItem.PercSalesTaxType;
                                    i.NetValue = objStockItem.NetValue;
                                    i.DiscountValue = objStockItem.DiscountValue;
                                    i.SalesTaxValue = objStockItem.SalesTaxValue;
                                    i.UpdatedAt = objStockItem.UpdatedAt;
                                    i.WHT = objStockItem.WHT;
                                    i.WHTType = objStockItem.WHTType;
                                    i.IsUpdate = true;
                                    i.IsActive = false;
                                    i.BatchExpiry = objStockItem.BatchExpiry;
                                    i.Store = objStockItem.Store;
                                    if (isDeleted)
                                    {
                                        i.IsActive = false;
                                        i.DeletedAt = DateTime.Now;
                                    }
                                    i.IsSynced = false;
                                    unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                    //update item for retail price
                                    Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.ItemId);
                                    objItem.IsSynced = false;
                                    objItem.IsUpdate = true;
                                    objItem.UpdateDate = DateTime.Now;
                                    objItem.RetailPrice = objStockItem.RetailPrice;
                                    objItem.UnitCostPrice = objStockItem.UnitCost;
                                    if (objItem.IsFileItem)
                                    {
                                        objItem.IsFileItem = false;
                                        addItems.Add(objItem);
                                    }
                                    else
                                    {
                                        updateItems.Add(objItem);
                                    }
                                    unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                                    unitOfWork.ItemRspository.Update(objItem);
                                    //end update item for retail price
                                    isExistingItem = true;
                                    itemIds.Add(objStockItem.ItemId);
                                    break;
                                }
                            }
                            if (!isExistingItem)
                            {
                                //update item for retail price
                                Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemID);
                                if (!objItem.IsSyncable)
                                {
                                    objItem.IsSyncable = true;
                                }
                                else if (objItem.IsSynced)
                                {
                                    objItem.IsNew = false;
                                    objItem.IsUpdate = true;
                                    objItem.IsSynced = false;
                                }

                                if (objItem.IsFileItem)
                                {
                                    objItem.IsFileItem = false;
                                    addItems.Add(objItem);
                                }
                                else
                                {
                                    updateItems.Add(objItem);
                                }

                                objItem.UpdateDate = DateTime.Now;
                                //objItem.RetailPrice = objStockItem.RetailPrice;
                                unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                                unitOfWork.ItemRspository.Update(objItem);
                                //end update item for retail price
                                objStock.StockItems.Add(objStockItem);
                            }
                        }
                        List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                        foreach (var i in msList)
                        {
                            i.UpdatedAt = DateTime.Now;
                            i.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            i.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            unitOfWork.MissedsalesRepository.Update(i);
                        }
                        unitOfWork.StockRepository.Update(objStock);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + ((long)objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.updated);
                        unitOfWork.Save();
                        //SOT ----- Manage Items
                        if (addItems != null && addItems.Count() > 0)
                        {
                            var itemAddBatches = SharedFunctions.ChunkBy(addItems, SharedVariables.ItemBatch);
                            foreach (var item in itemAddBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Create, 0);
                            }
                        }
                        if (updateItems != null && updateItems.Count() > 0)
                        {
                            var itemUpdateBatches = SharedFunctions.ChunkBy(updateItems, SharedVariables.ItemBatch);
                            foreach (var item in itemUpdateBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Update, 0);
                            }
                        }
                        //SOT ----- Manage Batches
                        List<Batch> batchList = objStock.StockItems.Select(x => x.Batch).ToList();
                        if (batchList != null && batchList.Count() > 0)
                        {
                            List<Batch> createBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "0").ToList();
                            if (createBatchList != null && createBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, createBatchList, ModuleName.BulkBatches, ModuleAction.Create, 0);
                            List<Batch> updatedBatchList = batchList.Where(x => x.IsJsonCreated != null && x.IsJsonCreated == "1").ToList();
                            if (updatedBatchList != null && updatedBatchList.Count() > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, updatedBatchList, ModuleName.BulkBatches, ModuleAction.Update, 0);
                        }
                        //SOT ----- Manage Stock
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objStock, ModuleName.Stock, ModuleAction.Update, objStock.StockId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
            MessageBox.Show("Items Stock Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            totalholds = 0;
            ShowHoldingStock(0);
            Clear();

        }
        private void FillObject(DataGridViewRow r, StockItem obj)
        {
            try
            {
                int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                obj.ItemId = ItemId;
                if (r.Visible)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                obj.BatchName = SharedFunctions.ObjectToString(r.Cells["colBatch"].Value) == "" ? "No Batch" : r.Cells["colBatch"].Value.ToString();


                obj.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;

                DateTime? batchExp = null;
                if (!string.IsNullOrEmpty(SharedFunctions.ObjectToString(r.Cells["colExpiry"].Value)))
                {
                    batchExp = Convert.ToDateTime(r.Cells["colExpiry"].Value);
                }
                obj.BatchExpiry = batchExp;
                Batch objBatch = unitOfWork.BatchRepository.FindItemBatch(obj.BatchName.ToLower(), ItemId);

                if (objBatch == null)
                {
                    objBatch = new Batch();
                    objBatch.BatchName = obj.BatchName;
                    if (obj.BatchExpiry != null)
                    {
                        objBatch.Expiry = (DateTime)obj.BatchExpiry;
                    }
                    else
                    {
                        objBatch.Expiry = null;
                    }
                    objBatch.CreatedAt = DateTime.Now;
                    objBatch.UpdatedAt = DateTime.Now;
                    objBatch.IsNew = true;

                    objBatch.IsActive = true;


                    objBatch.IsJsonCreated = "0";
                    objBatch = unitOfWork.BatchRepository.InsertBatch(objBatch);
                }
                else
                {
                    objBatch.IsJsonCreated = "-1";
                    if (obj.BatchExpiry != null)
                    {
                        objBatch.Expiry = (DateTime)obj.BatchExpiry;
                        objBatch.UpdatedAt = DateTime.Now;
                        objBatch.IsUpdate = true;
                        objBatch.IsSynced = false;
                        objBatch.IsJsonCreated = "1";
                        unitOfWork.GetDbContext().Entry(objBatch).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                obj.Batch = objBatch;

                if (obj.StockItemId <= 0) // don't get item from db when stock is being updated, because item updation is not allowed
                {
                    obj.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                }

                obj.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);

                obj.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                obj.Quantity = obj.Unit == 0 ? obj.Quantity * obj.ConversionUnit : obj.Quantity; // to save quantity always in broken form.
                obj.BonusQuantity = Convert.ToInt32(r.Cells["colBonusQty"].Value);
                obj.BonusQuantity = obj.Unit == 0 ? obj.BonusQuantity * obj.ConversionUnit : obj.BonusQuantity; // to save quantity always in broken form.
                obj.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);

                if (r.Cells["colWHT"].Value == null)
                {
                    obj.WHT = "0";
                    obj.WHTType = r.Cells["colWHTType"].Value.ToString() == "%" ? 1 : 0;
                }
                else
                {
                    obj.WHT = r.Cells["colWHT"].Value.ToString();
                    obj.WHTType = r.Cells["colWHTType"].Value.ToString() == "%" ? 1 : 0;
                }

                if (r.Cells["colAdvanceWHT"].Value == null)
                {
                    obj.AdvanceWHT = "0";
                    obj.AdvanceWHTType = r.Cells["colAdvanceWHTType"].Value.ToString() == "%" ? 1 : 0;
                }
                else
                {
                    obj.AdvanceWHT = r.Cells["colAdvanceWHT"].Value.ToString();
                    obj.AdvanceWHTType = r.Cells["colAdvanceWHTType"].Value.ToString() == "%" ? 1 : 0;
                }


                obj.UnitCost = Math.Round(obj.Unit == 0 ? obj.UnitCost / obj.ConversionUnit : obj.UnitCost, 3);// to save quantity always in broken form.
                obj.DiscountedPrice = Convert.ToDouble(r.Cells["DiscountedPrice"].Value.ToString());
                obj.TotalCost = Math.Round(Convert.ToDouble(r.Cells["colTotalCost"].Value), 3);
                obj.RetailPrice = Math.Round(Convert.ToDouble(r.Cells["colRetailPrice"].Value), 3);
                obj.RetailPrice = Math.Round(obj.Unit == 0 ? obj.RetailPrice / obj.ConversionUnit : obj.RetailPrice, 3); // to save quantity always in broken form.
                obj.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);

                ///obj.Discount = obj.Unit == 0 ? obj.Discount / obj.ConversionUnit : obj.Discount; // to save quantity always in broken form.
                obj.BonusDiscount = Convert.ToDouble(r.Cells["colBonusDiscount"].Value); // no need to break discount
                //obj.BonusDiscount = obj.Unit == 0 ? obj.BonusDiscount / obj.ConversionUnit : obj.BonusDiscount; // to save quantity always in broken form.
                obj.PercDiscType = r.Cells["colPercDiscType"].Value.ToString().ToLower() == "%" ? 0 : 1;
                obj.DiscountType = r.Cells["colDiscType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
                obj.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);

                //obj.SalesTax = obj.Unit == 0 ? obj.SalesTax / obj.ConversionUnit : obj.SalesTax; // to save quantity always in broken form.
                obj.PercSalesTaxType = r.Cells["colPercSalesTaxType"].Value.ToString().ToLower() == "%" ? 0 : 1;
                obj.SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
                obj.NetValue = Math.Round(Convert.ToDouble(r.Cells["colNetValue"].Value), 3);
                obj.CreatedAt = DateTime.Now;
                obj.UpdatedAt = DateTime.Now;

                obj.IsNew = true;
                if (r.Visible)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;

                }
                if (r.Visible)
                {
                    obj.DiscountValue = Convert.ToDouble(r.Cells["colDiscountValue"].Value);
                    obj.SalesTaxValue = Convert.ToDouble(r.Cells["colSalesTaxValue"].Value);
                }
                //----Assign Stores

                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    int StoreId = SharedFunctions.StringToInt(r.Cells["colStores"].Value);
                    if (StoreId > 0)
                    {
                        obj.Store = unitOfWork.StoreRepository.GetById(StoreId);
                    }
                }
            }
            catch (Exception) { }
        }
        private void FillObject1(DataGridViewRow r, StockItem obj)
        {
            int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            obj.ItemId = ItemId;
            
            obj.BatchName = SharedFunctions.ObjectToString(r.Cells["colBatch"].Value) == "" ? "No Batch" : r.Cells["colBatch"].Value.ToString();
            obj.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            DateTime? batchExp = null;
            if (!string.IsNullOrEmpty(SharedFunctions.ObjectToString(r.Cells["colExpiry"].Value)))
            {
                batchExp = Convert.ToDateTime(r.Cells["colExpiry"].Value);
            }
            obj.BatchExpiry = batchExp;

            Batch objBatch = unitOfWork.BatchRepository.FindItemBatch(obj.BatchName.ToLower(), ItemId);
            
            if (objBatch == null)
            {
                objBatch = new Batch();
                objBatch.BatchName = obj.BatchName;
                if (obj.BatchExpiry != null)
                {
                    objBatch.Expiry = (DateTime)obj.BatchExpiry;
                }
                else
                {
                    objBatch.Expiry = null;
                }
                objBatch.CreatedAt = DateTime.Now;
                objBatch.UpdatedAt = DateTime.Now;
                objBatch.IsNew = true;
                objBatch.IsActive = false;
                objBatch.IsJsonCreated = "0";
                objBatch = unitOfWork.BatchRepository.InsertBatch(objBatch);
            }
            else
            {
                objBatch.IsJsonCreated = "-1";
                if (obj.BatchExpiry != null)
                {
                    objBatch.Expiry = (DateTime)obj.BatchExpiry;
                    objBatch.UpdatedAt = DateTime.Now;
                    objBatch.IsUpdate = true;
                    objBatch.IsSynced = false;
                    objBatch.IsJsonCreated = "1";
                    unitOfWork.GetDbContext().Entry(objBatch).State = System.Data.Entity.EntityState.Modified;
                }
            }
            obj.Batch = objBatch;
            if (obj.StockItemId <= 0) // don't get item from db when stock is being updated, because item updation is not allowed
            {
                obj.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
            }
            obj.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            obj.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
            obj.Quantity = obj.Unit == 0 ? obj.Quantity * obj.ConversionUnit : obj.Quantity; // to save quantity always in broken form.
            obj.BonusQuantity = Convert.ToInt32(r.Cells["colBonusQty"].Value);
            obj.BonusQuantity = obj.Unit == 0 ? obj.BonusQuantity * obj.ConversionUnit : obj.BonusQuantity; // to save quantity always in broken form.
            obj.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
            obj.UnitCost = Math.Round(obj.Unit == 0 ? obj.UnitCost / obj.ConversionUnit : obj.UnitCost, 3); // to save quantity always in broken form.
            obj.TotalCost = Math.Round(Convert.ToDouble(r.Cells["colTotalCost"].Value), 3);
            obj.RetailPrice = Math.Round(Convert.ToDouble(r.Cells["colRetailPrice"].Value), 3);
            obj.RetailPrice = Math.Round(obj.Unit == 0 ? obj.RetailPrice / obj.ConversionUnit : obj.RetailPrice, 3); // to save quantity always in broken form.
            obj.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            ///obj.Discount = obj.Unit == 0 ? obj.Discount / obj.ConversionUnit : obj.Discount; // to save quantity always in broken form.
            obj.BonusDiscount = Convert.ToDouble(r.Cells["colBonusDiscount"].Value); // no need to break discount
            //obj.BonusDiscount = obj.Unit == 0 ? obj.BonusDiscount / obj.ConversionUnit : obj.BonusDiscount; // to save quantity always in broken form.
            obj.PercDiscType = r.Cells["colPercDiscType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.DiscountType = r.Cells["colDiscType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
            obj.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            //obj.SalesTax = obj.Unit == 0 ? obj.SalesTax / obj.ConversionUnit : obj.SalesTax; // to save quantity always in broken form.
            obj.PercSalesTaxType = r.Cells["colPercSalesTaxType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
            obj.NetValue = Math.Round(Convert.ToDouble(r.Cells["colNetValue"].Value), 3);
            obj.CreatedAt = DateTime.Now;
            obj.UpdatedAt = DateTime.Now;
            obj.IsNew = true;
            obj.IsActive = false;
            obj.WHT = r.Cells["colWHT"].Value.ToString();
            obj.WHTType = r.Cells["colWHTType"].Value.ToString() == "%" ? 0 : 1;
            obj.DiscountValue = Convert.ToDouble(r.Cells["colDiscountValue"].Value);
            obj.SalesTaxValue = Convert.ToDouble(r.Cells["colSalesTaxValue"].Value);
            //----Assign Stores
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                int StoreId = SharedFunctions.StringToInt(r.Cells["colStores"].Value);
                if (StoreId > 0)
                {
                    obj.Store = unitOfWork.StoreRepository.GetById(StoreId);
                }
            }
        }
        private void dtpExpiry_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.CustomFormat = "dd/MM/yyyy";
            dtp.Tag = 1;
        }
        private void lblClearExpiry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dtpExpiry.CustomFormat = " ";
            dtpExpiry.Tag = 0;
        }
        private void numSalesTax_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }

        private void GetITtemUnit(DataGridViewRow r, int itemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(itemId);
                if (objItem == null)
                {
                    MessageBox.Show("No item selected, please try loading item again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string unit = objItem.Unit;
                DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                dgCmbBatch.Items.Add(unit);
                dgCmbBatch.Items.Add("Units");
            }
        }
        private void LoadItemData(DataGridViewRow r)
        {
        }
        private void dtpExpiry_Enter(object sender, EventArgs e)
        {
            dtpExpiry.Select();
            SendKeys.Send("%{DOWN}");
        }
        private void dtpExpiry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lblClearExpiry_LinkClicked(null, null);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numRetailPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void txtBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbDiscountType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numSalesTax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numWHT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        
        private void cmbSalesTaxType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbWHTType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridViewRow r = grdItems.Rows[e.RowIndex];
                if (this.IsLoadingPrvSoldItems)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    string unit = r.Cells["colUnit"].Value.ToString();
                    DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    dgCmbUnit.Value = "Units";
                    //double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                    //txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) + NetValue).ToString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                else if (isStockLoading)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    GetITtemUnit(grdItems.Rows[e.RowIndex], Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value));
                    DataGridViewComboBoxCell dgCmbStore = (DataGridViewComboBoxCell)r.Cells["colStores"];
                    dgCmbStore.DataSource = new BindingSource(loadStores, null);
                    dgCmbStore.ValueMember = "StoreId";
                    dgCmbStore.DisplayMember = "Name";
                    //if (loadStores != null && loadStores.Count() > 0)
                    //    r.Cells["colStores"].Value = loadStores[0].StoreId;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                else
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    string unit = r.Cells["colUnit"].Value.ToString();
                    DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    dgCmbUnit.Items.Add(r.Cells["colUnit"].Value.ToString());
                    dgCmbUnit.Items.Add("Units");
                    dgCmbUnit.Value = unit;

                    if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                    {
                        DataGridViewComboBoxCell dgCmbStore = (DataGridViewComboBoxCell)r.Cells["colStores"];
                        dgCmbStore.DataSource = new BindingSource(loadStores, null);
                        dgCmbStore.ValueMember = "StoreId";
                        dgCmbStore.DisplayMember = "Name";
                        if (loadStores != null && loadStores.Count() > 0)
                            r.Cells["colStores"].Value = loadStores[0].StoreId;
                    }
                    string value = grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value.ToString();
                    double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                    txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) + NetValue).ToString();
                    AssignGrandTotalWithHoldingTax();
                    string value1 = grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value.ToString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    calculatteGrandValues(grdItems);

                    
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void calculatteGrandValues(DataGridView dgv)
        {
            double TotalCost = 0;
            double TotalRate = 0;
            double TotalQty = 0;
            double TotalBonusQty = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Visible)
                {
                    if (row.Cells["colQuantity"].Value == null || row.Cells["colQuantity"].Value == "") { row.Cells["colQuantity"].Value = 0; }
                    if (row.Cells["colBonusQty"].Value == null) { row.Cells["colBonusQty"].Value = 0; }
                    TotalCost += Convert.ToDouble(row.Cells["colTotalCost"].Value.ToString());
                    TotalRate += Convert.ToDouble(row.Cells["colRetailPrice"].Value.ToString());
                    TotalQty += Convert.ToDouble(row.Cells["colQuantity"].Value.ToString());
                    TotalBonusQty += Convert.ToDouble(row.Cells["colBonusQty"].Value.ToString());
                }
            }
            txtTotalofCost.Text = TotalCost.ToString();
            txttotalofrate.Text = TotalRate.ToString();
            txttotalofqty.Text = TotalQty.ToString();
            txttotalofbonusqty.Text = TotalBonusQty.ToString();
        }
        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            
        } 
        private void EleminateRow(DataGridViewCellEventArgs e, string status)
        {
            try
            {
                if (grdItems.Rows.Count > 0)
                {
                    double NetValue = Convert.ToDouble(grdItems.SelectedRows[0].Cells["colNetValue"].Value);
                    if (txtGrandTotal.Text == "") txtGrandTotal.Text = "0";
                    txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) - NetValue).ToString();
                    AssignGrandTotalWithHoldingTax();

                    double nv = 0, sv = 0, dv = 0;
                    nv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value.ToString());
                    sv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colSalesTaxValue"].Value.ToString());
                    dv = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscountValue"].Value.ToString());
                    txtGrandTotal.Text = (SharedFunctions.StringToDouble(txtGrandTotal.Text) - nv).ToString();
                    AssignGrandTotalWithHoldingTax();
                    txtTotalSaleTax.Text = (SharedFunctions.StringToDouble(txtTotalSaleTax.Text) - sv).ToString();
                    txtTotalDiscount.Text = (SharedFunctions.StringToDouble(txtTotalDiscount.Text) - dv).ToString();
                    if(status == "Add")
                    {
                        grdItems.Rows.RemoveAt(e.RowIndex);
                    }

                    calculatteGrandValues(grdItems);
                }
                else
                {
                    grdItems.Rows.RemoveAt(e.RowIndex);
                    txtTotalofCost.Text = "0";
                    txttotalofrate.Text = "0";
                    txttotalofqty.Text = "0";
                    txttotalofbonusqty.Text = "0";
                    isholdchekc = 0;
                    txtGrandTotal.Text = "0";

                }
            }
            catch (Exception)
            {
            }
        }
        private void grdItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                double NetValue = Convert.ToDouble(e.Row.Cells["colNetValue"].Value);
                if (txtGrandTotal.Text == "") txtGrandTotal.Text = "0";
                txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) - NetValue).ToString();
                AssignGrandTotalWithHoldingTax();
            }
            catch (Exception)
            {
            }
        }
        private void uC_SearchItems1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }


        private void LoadSupplierItems(bool LoadLowStockItems)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (cmbSuppliers.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int SupId = Convert.ToInt32(cmbSuppliers.SelectedValue);

            grdItems.Rows.Clear();

            try
            {
                this.isStockLoading = true;
                List<int> missedSaleItems = new List<int>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    vmList = unitOfWork.ItemRspository.LoadSupItems(SupId);
                }
                
                foreach (var vm in vmList)
                {
                    CheckDuplicateItems(vm.ItemId);
                    if (LoadLowStockItems)
                    {
                        if (vm.AvailableStock >= vm.ReorderingLevel)
                        {
                            continue;
                        }
                    }

                    grdItems.Rows.Add(
                        0,
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        0, //Store
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        0, //objStockItem.Quantity,
                        0, //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        vm.UnitCost * vm.ConversionUnit,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        Profit(vm.RetailPrice, vm.UnitCost),
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0,
                        "%",
                        0, //objStockItem.NetValue,
                        GrossMargin(vm.AvailableStock / vm.ConversionUnit, vm.RetailPrice * vm.ConversionUnit, 0),
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    LoadSerial();
                    // Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }
        private void btlLoadSupProds_Click(object sender, EventArgs e)
        {
            this.LoadSupplierItems(false);
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
           
            f.ShowDialog();
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (!IsItemAlreadyAdded(ItemId))
                {
                    ItemsVM vm = new ItemsVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        vm = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    grdItems.Rows.Add(
                        0,
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        0, //Store
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        Quantity,//0, //objStockItem.Quantity,
                        0, //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        vm.UnitCost * vm.ConversionUnit * Quantity,//0, //objStockItem.TotalCost, //For Total Cost
                        0,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        Profit(vm.RetailPrice, vm.UnitCost),
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Disc`ount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0,
                        "%",
                        0,
                        "%",
                        vm.UnitCost * vm.ConversionUnit * Quantity, //0, //objStockItem.NetValue,
                        GrossMargin(Quantity, vm.RetailPrice * vm.ConversionUnit, vm.UnitCost * vm.ConversionUnit * Quantity),
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    LoadSerial();
                }
                Clear();
                grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                isholdchekc = 0;

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            finally
            {
                txtGrandTotal.Text = GetGrandTotal().ToString();
                AssignGrandTotalWithHoldingTax();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                int index = grdItems.Rows[grdItems.Rows.Count - 1].Index;
                grdItems.FirstDisplayedScrollingRowIndex = index;
                grdItems.ClearSelection();
                grdItems.Rows[index].Selected = true;
            }
            
        }
        
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                //if (IsValidInput())
                //{
                //StockItem objStockItem = new StockItem();
                //FillObject(objStockItem);
                //if (EditingRowIndex >= 0)
                //{
                //UpdateRow(objStockItem);
                //}
                if (this.SelectedItemId > 0)
                {
                    if (!IsItemAlreadyAdded(this.SelectedItemId))
                    {
                        ItemsVM vm = new ItemsVM();

                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQty(this.SelectedItemId);
                        }
                        
                        grdItems.Rows.Add(
                            0,
                            0,
                            vm.ItemId,
                            vm.ItemName, //objStockItem.ItemName,
                            vm.Unit, //objStockItem.UnitString,
                            0, //Store
                            vm.ConversionUnit, //objStockItem.ConversionUnit,
                            vm.AvailableStock / vm.ConversionUnit, // available quantity
                            1, //objStockItem.Quantity,
                            0, //bonus quantity
                            vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                            vm.UnitCost * vm.ConversionUnit,
                            0, //objStockItem.TotalCost,
                            vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                            Profit(vm.RetailPrice, vm.UnitCost),
                            "", //objStockItem.BatchName,
                            "", //objStockItem.BatchExpiry,
                            0, //objStockItem.Discount,
                            0, // bonus discount
                            "%",//Percent discout type
                            "Lumpsum", //objStockItem.DiscoutnType,
                            0, //objStockItem.SalesTax,
                            "%", // percent Sales Tax Type
                             
                            "Lumpsum", //objStockItem.SalesTaxType,
                            0,
                            "%",
                            0,
                            "%",
                            vm.UnitCost * vm.ConversionUnit * 0, //0, //objStockItem.NetValue,
                            GrossMargin(0, vm.RetailPrice, vm.UnitCost * vm.ConversionUnit * 0),
                            0,
                            0, //objStockItem.DiscountValue,
                            0, //objStockItem.SalesTaxValue
                            0,
                            "%"
                           
                            );

                        LoadSerial();
                    }
                    Clear();
                    grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    isholdchekc = 0;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                txtGrandTotal.Text = GetGrandTotal().ToString();
                AssignGrandTotalWithHoldingTax();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                int index = grdItems.Rows[grdItems.Rows.Count-1].Index;
                grdItems.FirstDisplayedScrollingRowIndex = index;
                
                grdItems.ClearSelection();
                grdItems.Rows[index].Selected = true;

            }
            
            uC_SearchItems1.SetFocus();
        }

        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (e.RowIndex < 0) return;
            int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);

            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscType"].Index
                || e.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
                || e.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                || e.ColumnIndex == grdItems.Columns["colBonusQty"].Index
                )
            {
                double Totalcost = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colTotalCost"].Value);
                double discount = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value);
                string discountType = (grdItems.Rows[e.RowIndex].Cells["colPercDiscType"].Value).ToString();

                if (discount > Totalcost && discountType == "Value")
                {
                    MessageBox.Show("Discount Value is not more than the Total Cost", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = 0;
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    return;
                }
                else if (discountType == "%")
                {
                    if (discount > 100)
                    {
                        MessageBox.Show("Discount Value is not more than 100", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = 0;
                        grdItems.CellValueChanged -= grdItems_CellValueChanged;
                        return;
                    }
                }

                CalculateValues(grdItems.Rows[e.RowIndex]);
                #region ShowHideField
                bool grdDiscount = false, grdSaleTax = false;
                grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
                if (grdDiscount || grdSaleTax)
                {
                    txtNetDiscount.Enabled = false;
                    txtNetSaleTax.Enabled = false;
                    cmbNetDiscountType.Enabled = false;
                    cmbNetSaleTaxType.Enabled = false;
                    txtWithHoldingTax.Enabled = false;
                    cmbWithHoldingTaxType.Enabled = false;

                    txtAdvanceWithHoldingTax.Enabled = false;
                    cmbWithAdvanceHoldingTaxType.Enabled = false;
                    //--------- Register Event -----------//
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                    txtNetDiscount.Text = "";
                    txtNetSaleTax.Text = "";
                    cmbNetDiscountType.SelectedIndex = 0;
                    cmbNetSaleTaxType.SelectedIndex = 0;
                    //--------- Un Register Event -----------//
                    txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                else
                {
                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    txtWithHoldingTax.Enabled = true;
                    cmbWithHoldingTaxType.Enabled = true;

                    txtAdvanceWithHoldingTax.Enabled = true;
                    cmbWithAdvanceHoldingTaxType.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    //--------- Un Register Event -----------//
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                    txtNetDiscount.Text = "";
                    txtNetSaleTax.Text = "";
                    cmbNetDiscountType.SelectedIndex = 0;
                    cmbNetSaleTaxType.SelectedIndex = 0;
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        r.Cells["colDiscount"].Value = "0";
                        //r.Cells["colPercDiscType"].Value = "%";
                        r.Cells["colSalesTax"].Value = "0";
                        r.Cells["colPercSalesTaxType"].Value = "%";
                    }
                    //--------- Register Event -----------//
                    txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                #endregion
                txtGrandTotal.Text = GetGrandTotal().ToString();
                //AssignGrandTotalWithHoldingTax();
                calculateDiscPrice();
            }
            else if (e.ColumnIndex == grdItems.Columns["colWHT"].Index || e.ColumnIndex == grdItems.Columns["colWHTType"].Index)
            {
                bool grdWHTTax = false;
                grdWHTTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colWHT"].Value)).Any(x => x > 0);
                if (grdWHTTax)
                {
                    txtNetDiscount.Enabled = false;
                    txtNetSaleTax.Enabled = false;
                    cmbNetDiscountType.Enabled = false;
                    cmbNetSaleTaxType.Enabled = false;
                    txtWithHoldingTax.Enabled = false;
                    cmbWithHoldingTaxType.Enabled = false;
                    txtAdvanceWithHoldingTax.Enabled = false;
                    cmbWithAdvanceHoldingTaxType.Enabled = false;
                }
                else
                {
                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    txtWithHoldingTax.Enabled = true;
                    cmbWithHoldingTaxType.Enabled = true;
                    txtAdvanceWithHoldingTax.Enabled = true;
                    cmbWithAdvanceHoldingTaxType.Enabled = true;
                }
                calculatteGrandValues(grdItems);
                CalculateValues(grdItems.Rows[e.RowIndex]);
                GetGrandTotal();     
            }
            else if (e.ColumnIndex == grdItems.Columns["colAdvanceWHTType"].Index || e.ColumnIndex == grdItems.Columns["colAdvanceWHT"].Index)
            {
                bool grdAdvanceTax = false;
                grdAdvanceTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colAdvanceWHT"].Value)).Any(x => x > 0);
                if (grdAdvanceTax)
                {
                    txtNetDiscount.Enabled = false;
                    txtNetSaleTax.Enabled = false;
                    cmbNetDiscountType.Enabled = false;
                    cmbNetSaleTaxType.Enabled = false;
                    txtWithHoldingTax.Enabled = false;
                    cmbWithHoldingTaxType.Enabled = false;
                    txtAdvanceWithHoldingTax.Enabled = false;
                    cmbWithAdvanceHoldingTaxType.Enabled = false;
                }
                else
                {
                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    txtWithHoldingTax.Enabled = true;
                    cmbWithHoldingTaxType.Enabled = true;
                    txtAdvanceWithHoldingTax.Enabled = true;
                    cmbWithAdvanceHoldingTaxType.Enabled = true;
                }
                calculatteGrandValues(grdItems);
                CalculateValues(grdItems.Rows[e.RowIndex]);
                GetGrandTotal();
            }
            else if(e.ColumnIndex == grdItems.Columns["colNetProfit"].Index)
            {
                if (a == 1)
                {
                    b = 0;
                }
                else
                {
                    b = 1;
                }

                int Profit = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colNetProfit"].Value);
                if (Profit > 100)
                {
                 MessageBox.Show("Profit Margin is not more than 100", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 grdItems.Rows[e.RowIndex].Cells["colNetProfit"].Value = 0;
                 grdItems.CellValueChanged -= grdItems_CellValueChanged;
                 return;
                }
                else if (Profit == 100)
                {
                    MessageBox.Show("Profit Margin is not Upto 100", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[e.RowIndex].Cells["colNetProfit"].Value = 0;
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    return;
                }

                //grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].ReadOnly = true;
                if (b == 1)
                {
                    CalculateRetail(grdItems.Rows[e.RowIndex]);
                }
                #region ShowHideField
                bool grdDiscount = false, grdSaleTax = false;
                grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
                if (grdDiscount || grdSaleTax)
                {
                    txtNetDiscount.Enabled = false;
                    txtNetSaleTax.Enabled = false;
                    cmbNetDiscountType.Enabled = false;
                    cmbNetSaleTaxType.Enabled = false;
                    txtWithHoldingTax.Enabled = false;
                    cmbWithHoldingTaxType.Enabled = false;
                    txtAdvanceWithHoldingTax.Enabled = false;
                    cmbWithAdvanceHoldingTaxType.Enabled = false;
                    //--------- Register Event -----------//
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                    txtNetDiscount.Text = "";
                    txtNetSaleTax.Text = "";
                    cmbNetDiscountType.SelectedIndex = 0;
                    cmbNetSaleTaxType.SelectedIndex = 0;
                    //--------- Un Register Event -----------//
                    txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                else
                {
                    txtNetDiscount.Enabled = true;
                    txtNetSaleTax.Enabled = true;
                    cmbNetDiscountType.Enabled = true;
                    txtWithHoldingTax.Enabled = true;
                    cmbWithHoldingTaxType.Enabled = true;
                    txtAdvanceWithHoldingTax.Enabled = true;
                    cmbWithAdvanceHoldingTaxType.Enabled = true;
                    cmbNetSaleTaxType.Enabled = true;
                    //--------- Un Register Event -----------//
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                    txtNetDiscount.Text = "";
                    txtNetSaleTax.Text = "";
                    cmbNetDiscountType.SelectedIndex = 0;
                    cmbNetSaleTaxType.SelectedIndex = 0;
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        r.Cells["colDiscount"].Value = "0";
                        r.Cells["colPercDiscType"].Value = "%";
                        r.Cells["colSalesTax"].Value = "0";
                        r.Cells["colPercSalesTaxType"].Value = "%";
                    }
                    //--------- Register Event -----------//
                    txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                    txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                    cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                    cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                #endregion
                txtGrandTotal.Text = GetGrandTotal().ToString();
            }
            else if (e.ColumnIndex == grdItems.Columns["colBonusQty"].Index)
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                int bq = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBonusQty"].Value);
                if (bq > 0)
                {
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].ReadOnly = true;
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = 0;
                }
                else
                {
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].ReadOnly = false;
                }
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            else if (e.ColumnIndex == grdItems.Columns["colUnitCost"].Index)
            {
                double unitCost = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
                double retailPrice = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value);
                if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && retailPrice < unitCost)
                {
                    StockItemDetailRunTimeVM objItem = new StockItemDetailRunTimeVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        objItem = unitOfWork.ItemRspository.GetItemDetail(itemId);
                    }
                    if (objItem == null)
                    {
                        MessageBox.Show("No item selected, please try loading item again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    // here re-assigining values from database to reset values to defaults.
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice;
                    MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CalculateValues(grdItems.Rows[e.RowIndex]);
                    txtGrandTotal.Text = GetGrandTotal().ToString();
                    AssignGrandTotalWithHoldingTax();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    return;
                }
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                double RetailPrice, CostPrice, NetValue = 0;
                int Qty, BQty = 0;

                RetailPrice = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value);
                CostPrice = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
                NetValue = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                Qty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                BQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colBonusQty"].Value);

                grdItems.Rows[e.RowIndex].Cells["colNetProfit"].Value = Profit(RetailPrice, CostPrice);
                grdItems.Rows[e.RowIndex].Cells["colGrossMargin"].Value = GrossMargin(Qty + BQty, RetailPrice, NetValue);
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            else if (e.ColumnIndex == grdItems.Columns["colRetailPrice"].Index)
            {
                a = 1;
                //grdItems.Rows[e.RowIndex].Cells["colNetProfit"].ReadOnly = true;

                double unitCost = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
                double retailPrice = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value);
                if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && retailPrice < unitCost)
                {
                    StockItemDetailRunTimeVM objItem = new StockItemDetailRunTimeVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        objItem = unitOfWork.ItemRspository.GetItemDetail(itemId);
                    }
                    if (objItem == null)
                    {
                        MessageBox.Show("No item selected, please try loading item again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    // here re-assigining values from database to reset values to defaults.
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice;
                    MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CalculateValues(grdItems.Rows[e.RowIndex]);
                    txtGrandTotal.Text = GetGrandTotal().ToString();
                    AssignGrandTotalWithHoldingTax();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    return;
                }
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                double RetailPrice, CostPrice, NetValue = 0;
                int Qty, BQty = 0;

                RetailPrice = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value);
                CostPrice = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
                NetValue = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                Qty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                BQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colBonusQty"].Value);

                grdItems.Rows[e.RowIndex].Cells["colNetProfit"].Value = Profit(RetailPrice, CostPrice);
                //grdItems.CellValueChanged -= grdItems_CellValueChanged;

                grdItems.Rows[e.RowIndex].Cells["colGrossMargin"].Value = GrossMargin(Qty + BQty, RetailPrice, NetValue);
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                a = 0;
            }
            else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                ItemsVM vm34 = new ItemsVM();
                StockItemDetailRunTimeVM objItem = new StockItemDetailRunTimeVM();
                string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                using (unitOfWork = new UnitOfWork())
                {
                    objItem = unitOfWork.ItemRspository.GetItemDetail(itemId);
                }
                if (objItem == null)
                {
                    MessageBox.Show("Failed to load item stock details, please try again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
              
                using (unitOfWork = new UnitOfWork())
                {
                    vm34 = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold_1(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ColItemId"].Value));
                }
                int avStock = objItem.TotalStock + objItem.AdjustedStock - objItem.ExpiredStock - vm34.ConsumedStock;
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (unit.ToLower() != "units")
                {
                    grdItems.Rows[e.RowIndex].Cells["ColItem"].Value = objItem.ItemName;
                    grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value = objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice * objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice * objItem.ConversionUnit;
                    //if (isholdchekc == 0)
                    //{
                    //    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = (int)Math.Floor((double)avStock / objItem.ConversionUnit);
                    //}
                    //else
                    //{
                        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = (int)Math.Floor((double)vm34.AvailableStock / objItem.ConversionUnit);
                    //}

                    //grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value)).ToString();
                    
                }
                else
                {
                    grdItems.Rows[e.RowIndex].Cells["ColItem"].Value = objItem.ItemName;
                    grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value = objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice;
                    //grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value)).ToString();
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice;
                    //if (isholdchekc == 0)
                    //{
                    //    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = avStock;
                    //}
                    //else
                    //{
                        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = vm34.AvailableStock;
                    //}

                    
                }
                //if (isholdchekc == 1)
                //{
                //    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == unit567)
                //    {
                //        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = avqty567.ToString();
                //    }
                //    else
                //    {
                //        if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Units")
                //        {
                //            grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = avqty567 * Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value);
                //        }
                //        else
                //        {
                //            grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = avqty567 / Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value);
                //        }
                //    }
                //}
                CalculateValues(grdItems.Rows[e.RowIndex]);
                calculateDiscPrice();
                //bool grdDiscount = false, grdSaleTax = false;
                //grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"])).Any(x => x > 0);
                //grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"])).Any(x => x > 0);
                //UnableDisableSalesDicountFields(false, grdDiscount == true || grdSaleTax == true ? true : false);
                txtGrandTotal.Text = GetGrandTotal().ToString();
                AssignGrandTotalWithHoldingTax();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            CalculateValues(grdItems.Rows[e.RowIndex]);
            grdItems.CellValueChanged += grdItems_CellValueChanged;
           
        }

        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colWHTType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colStores"].Index
                )
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colWHT"].Index)
            {
                CalculateValues(grdItems.SelectedRows[0]);
                calculatteGrandValues(grdItems);
                
                GetGrandTotal();
            }
            
        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //e.Control.KeyPress -= new KeyPressEventHandler(Control_LKeyPress);
            //if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index
            //    || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscount"].Index
            //    || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTax"].Index
            //    || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnitCost"].Index
            //    || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colRetailPrice"].Index
            //    || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBonusQty"].Index
            //    )
            //{
            //    e.Control.KeyPress += new KeyPressEventHandler(Control_LKeyPress); // to allow onyl integer input here in the datagridview
            //}
            TextBox tb = e.Control as TextBox;
            e.Control.KeyPress -= new KeyPressEventHandler((obj, arg) => Control_KeyPress(sender, arg, tb,
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index 
                ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBonusQty"].Index ? "int" : "double"
                
                ));
            e.Control.KeyPress -= new KeyPressEventHandler(Control_LKeyPress);
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colRetailPrice"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBonusQty"].Index 
                
                )
            {
                e.Control.KeyPress += new KeyPressEventHandler((obj, arg) => Control_KeyPress(sender, arg, tb,
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBonusQty"].Index ? "int" : "double"

                
                )); // to allow onyl integer input here in the datagridview
            }
            else
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_LKeyPress);
            }
            if ((grdItems.Columns[grdItems.CurrentCell.ColumnIndex].Name == "colSaleTax") ||
            (grdItems.Columns[grdItems.CurrentCell.ColumnIndex].Name == "colSaleTaxType") || grdItems.Columns[grdItems.CurrentCell.ColumnIndex].Name == "colWHT" || grdItems.Columns[grdItems.CurrentCell.ColumnIndex].Name == "colWHTType")
            {
                /// Workarround para que estando editando en las columnas del grid Clave y Nombre
                /// podamos detectar cuando se dio F4 para lanzar el dialogo de busqueda del
                /// articulo.
                e.Control.KeyDown += new KeyEventHandler(grdItems_KeyDown);
               
            }
        }

        private void Control_LKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void LoadSerial()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                //if (row.Visible == true)
                //{
                    grdItems.Rows[row.Index].Cells["colSrNo"].ReadOnly = true;
                    grdItems.Rows[row.Index].Cells["colSrNo"].Value = string.Format("{0}  ", row.Index + 1).ToString();
                    rowCount();
                //}
            }
        }
        public void rowCount()
        {
            int count = 1;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                if (row.Visible)
                {
                    row.Cells["colSrNo"].Value = count.ToString();
                    count++;
                }
            }
            count = 1;
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e, TextBox tb, string value = "")
        {
            if (value == "int")
            {
                SharedFunctions.isValidIntegerKey(sender, e);
            }
            else if(value == "double")
            {
                SharedFunctions.IsValidDoubleNumericDatagridview(sender, e, tb);
            }
        }

        private void grdItems_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (this.IsStockPricesEditable) return;
            //if (e.ColumnIndex == grdItems.Columns["colExpiry"].Index)
            //{
            //    dlgExpiry d = new dlgExpiry();
            //    int x = grdItems.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).X + 231 + 20; //231 : left panel width, 20 : extra width to fill the left gap
            //    int y = grdItems.Location.Y;//grdItems.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Y + ;// +grdItems.ColumnHeadersHeight + 48 + 10; //48 : top panel height : 10 : extra height
            //    d.StartPosition = FormStartPosition.Manual;
            //    d.Location = new Point(x, y);
            //    //d.ShowDialog();
            //    DateTime? exp = d.selectedDate;
            //    if (exp != null)
            //    {
            //        grdItems.CellValueChanged -= grdItems_CellValueChanged;
            //        //grdItems.Rows[e.RowIndex].Cells["colExpiry"].Value = exp.Value.ToShortDateString();
            //        grdItems.CellValueChanged += grdItems_CellValueChanged;
            //    }
            //}
        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void btnLoadMissedSales_Click(object sender, EventArgs e)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                this.isStockLoading = true;
                List<int> missedSaleItems = new List<int>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    missedSaleItems = unitOfWork.MissedsalesRepository.GetMissedSalesItemsIds();
                    vmList = unitOfWork.ItemRspository.GetMissedSalesItems(missedSaleItems);
                }

                foreach (var vm in vmList)
                {
                    CheckDuplicateItems(vm.ItemId);
                    grdItems.Rows.Add(
                        0,
                        0,
                        vm.ItemId,
                        vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        0, //Store
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        0, //objStockItem.Quantity,
                        0, //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        vm.UnitCost * vm.ConversionUnit,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        Profit(vm.RetailPrice, vm.UnitCost),
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0,
                        "%",
                        0, //objStockItem.NetValue,
                        GrossMargin(0, vm.RetailPrice, 0),
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    LoadSerial();
                    calculatteGrandValues(grdItems);
                }
                Clear();
                isholdchekc = 0;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void btnSupLowStockItems_Click(object sender, EventArgs e)
        {
            this.LoadSupplierItems(true);
            isholdchekc = 0;
        }

        private void btnLPrvSoldItems_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsLoadingPrvSoldItems = true; // this is needed to makes "Units" slected by default on this action
                DlgAddStockFilter f = new DlgAddStockFilter();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                List<StockAddFilterVM> lst = f.SelectedItems;

                foreach (var vm in lst)
                {
                    CheckDuplicateItems(vm.ItemId);
                    grdItems.Rows.Add(
                        0,
                        0,
                        vm.ItemId,
                        vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        0,//Store
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.Unit.ToLower() == "pack" ? vm.AvailableStock : vm.AvailableStock / vm.ConversionUnit, // available quantity
                        vm.Quantity, //objStockItem.Quantity,
                        0, //bonus quantity
                        vm.Unit.ToLower() == "pack" ? vm.UnitCost : vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        vm.Unit.ToLower() == "pack" ? vm.UnitCost : vm.UnitCost * vm.ConversionUnit, //disc price
                        vm.Quantity * vm.RetailPrice, //objStockItem.TotalCost,
                        vm.Unit.ToLower() == "pack" ? vm.RetailPrice : vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        Profit(vm.RetailPrice, vm.UnitCost),
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0,
                        "%",
                        0, //objStockItem.NetValue,
                        GrossMargin(vm.Quantity, vm.RetailPrice, 0),
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    calculatteGrandValues(grdItems);
                    LoadSerial();
                    isholdchekc = 0;
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
        public string Profit(double RetailPrice, double UnitPrice)
        {
            string profit = "";
            try
            {
                double result = (((RetailPrice - UnitPrice) / RetailPrice) * 100);
                //double result = (Double.IsInfinity(UnitPrice / RetailPrice) ? Math.Round(UnitPrice / RetailPrice) + 1 : UnitPrice / RetailPrice - 1) * (-100);
                result = Math.Round(result, 2);
                if (double.IsInfinity(result))
                    return profit;
                profit = double.IsNaN(result) ? "" : result.ToString();
            }
            catch (Exception ex)
            {
            }
            return profit;
        }

        public double GetRetailPrice(double unitCost, double Profit)
        {
            double retailCost = 0;
            double result = 0;
            try
            {
                   
                    result = (unitCost / (1 - (Profit/100)));
                    result = Math.Round(result, 2);
                    if (double.IsInfinity(result))
                        return retailCost;
                retailCost = result;
            }
            catch (Exception ex)
            {
            }
            return retailCost;
        }
        public string GrossMargin(int qty, double RetailPrice, double NetValue)
        {
            string margin = "";
            try
            {
                double result = (qty * RetailPrice) - NetValue;
                if (double.IsInfinity(result))
                    return margin;
                margin = double.IsNaN(result) ? "" : result.ToString();
            }
            catch (Exception ex)
            {
            }
            return margin;
        }

        public int getSupplierID()
        {
            System.Reflection.PropertyInfo getId = cmbSuppliers.SelectedItem.GetType().GetProperty("SupplierID");
            int supplierID = Convert.ToInt32(getId.GetValue(cmbSuppliers.SelectedItem, null));
            return supplierID;
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            new frmAddStockShortCusts().ShowDialog();
        }

        private void txtNetDiscount_TextChanged(object sender, EventArgs e)
        {
            UnableDisableSalesDicountFields(true);
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }

        private void cmbNetDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }

        private void txtNetSaleTax_TextChanged(object sender, EventArgs e)
        {
            UnableDisableSalesDicountFields(true);
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }

        private void cmbNetSaleTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }

        private void UnableDisableSalesDicountFields(bool IsNetSalesTax, [CallerFilePath] string sourceFilePath = "")
        {
            bool grdDiscount = false, grdSaleTax = false;
            if (grdItems != null && grdItems.Rows != null && grdItems.Rows.Count > 0)
            {
                grdDiscount = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value)).Any(x => x > 0);
                grdSaleTax = (from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value)).Any(x => x > 0);
            }
            //----------------------------- Did not assign any value -----------------------------//
            if (SharedFunctions.StringToDouble(txtNetDiscount.Text) == 0 &&
                SharedFunctions.StringToDouble(txtNetSaleTax.Text) == 0 &&
                grdDiscount == false &&
                grdSaleTax == false)
            {
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
                cmbNetDiscountType.Enabled = true;
                cmbNetSaleTaxType.Enabled = true;
                //--------- Register Event -----------//
                txtNetDiscount.TextChanged -= txtNetDiscount_TextChanged;
                txtNetSaleTax.TextChanged -= txtNetSaleTax_TextChanged;
                cmbNetDiscountType.SelectedIndexChanged -= cmbNetDiscountType_SelectedIndexChanged;
                cmbNetSaleTaxType.SelectedIndexChanged -= cmbNetSaleTaxType_SelectedIndexChanged;
                txtNetDiscount.Text = "";
                txtNetSaleTax.Text = "";
                cmbNetDiscountType.SelectedIndex = 0;
                cmbNetSaleTaxType.SelectedIndex = 0;
                //--------- Un Register Event -----------//
                txtNetDiscount.TextChanged += txtNetDiscount_TextChanged;
                txtNetSaleTax.TextChanged += txtNetSaleTax_TextChanged;
                cmbNetDiscountType.SelectedIndexChanged += cmbNetDiscountType_SelectedIndexChanged;
                cmbNetSaleTaxType.SelectedIndexChanged += cmbNetSaleTaxType_SelectedIndexChanged;
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    r.Cells["colDiscount"].Value = "0";
                    r.Cells["colPercDiscType"].Value = "%";
                    r.Cells["colSalesTax"].Value = "0";
                    r.Cells["colPercSalesTaxType"].Value = "%";
                }
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colPercDiscType"].ReadOnly = false;
                grdItems.Columns["colSalesTax"].ReadOnly = false;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = false;
                return;
            }

            if (IsNetSalesTax)
            {
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colPercDiscType"].ReadOnly = true;
                grdItems.Columns["colSalesTax"].ReadOnly = true;
                grdItems.Columns["colPercSalesTaxType"].ReadOnly = true;
                grdItems.CellContentClick -= grdItems_CellContentClick;
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    r.Cells["colDiscount"].Value = "0";
                    r.Cells["colPercDiscType"].Value = "%";
                    r.Cells["colSalesTax"].Value = "0";
                    r.Cells["colPercSalesTaxType"].Value = "%";
                }
                grdItems.CellContentClick += grdItems_CellContentClick;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
                cmbNetDiscountType.Enabled = true;
                cmbNetSaleTaxType.Enabled = true;
            }

        }

        private void CheckDuplicateItems(long ItemId)
        {
            if ((from DataGridViewRow r in grdItems.Rows select SharedFunctions.StringToLong(r.Cells["colItemId"].Value)).Any(x => x == ItemId))
            {
                DataGridViewRow row = grdItems.Rows.Cast<DataGridViewRow>().Where(r => SharedFunctions.StringToLong(r.Cells["colItemId"].Value).Equals(ItemId)).FirstOrDefault();
                grdItems.Rows.Remove(row);
            }
        }

        private void cmbWithHoldingTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }
        private void txtWithHoldingTax_TextChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
            //AssignGrandTotalWithHoldingTax();
        }

        private double GetGrandTotal([CallerFilePath] string sourceFilePath = "")
        {
            double gt = 0, dt = 0, st = 0;
            //----------Total Discount From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Visible)
                {
                    dt += SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value);
                }
            }

            //----------Total Sales Tax  From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Visible)
                {
                    st += SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value);
                }
            }
            //----------Grand Total From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Visible)
                {
                    gt += Convert.ToDouble(r.Cells["colNetValue"].Value);
                }
            }

            double Disc = SharedFunctions.StringToDouble(txtNetDiscount.Text);
            double SalesTax = SharedFunctions.StringToDouble(txtNetSaleTax.Text);
            double HoldingTax = SharedFunctions.StringToDouble(txtWithHoldingTax.Text);
            double AdvanceHoldingTax = SharedFunctions.StringToDouble(txtAdvanceWithHoldingTax.Text);

            string DiscType = cmbNetDiscountType.Text.ToString();
            string SaleTaxType = cmbNetSaleTaxType.Text.ToString();
            if (DiscType == "%" && Disc > 0)
            {
                dt += ((Disc / 100) * gt);
                gt = gt - dt;
            }
            if (SaleTaxType == "%" && SalesTax > 0)
            {
                st += ((SalesTax / 100) * gt);
                gt = gt + st;
            }
            if (txtWithHoldingTax.Enabled == true)
            {
                if (gt > 0 && HoldingTax > 0)
                {
                    if (cmbWithHoldingTaxType.SelectedIndex == 0)
                    {
                        double value = (HoldingTax / 100)* gt ;
                        gt = gt + value;
                    }
                }
            }
            if (txtAdvanceWithHoldingTax.Enabled == true)
            {
                if (gt > 0 && AdvanceHoldingTax > 0)
                {
                    if (cmbWithAdvanceHoldingTaxType.SelectedIndex == 0)
                    {
                        double value = (AdvanceHoldingTax / 100) * gt;
                        gt = gt + value;
                    }
                }
            }
            if (DiscType == "Value" && Disc > 0)
            {
                dt += Disc;
                gt = gt - Disc;
            }
            if (SaleTaxType == "Value" && SalesTax > 0)
            {
                st += SalesTax;
                gt = gt + st;
            }
            if (txtWithHoldingTax.Enabled == true)
            {
                if (gt > 0 && HoldingTax > 0)
                {
                    if (cmbWithHoldingTaxType.SelectedIndex != 0)
                    {
                        gt = gt + HoldingTax;
                    }
                }
            }

            if (txtAdvanceWithHoldingTax.Enabled == true)
            {
                if (gt > 0 && AdvanceHoldingTax > 0)
                {
                    if (cmbWithAdvanceHoldingTaxType.SelectedIndex != 0)
                    {
                        gt = gt + AdvanceHoldingTax;
                    }
                }
            }
            txtTotalDiscount.Text = dt.ToString();
            txtTotalSaleTax.Text = st.ToString();
            return gt;
        }
        public void showSupplierPayment(int supplierId)
        {
            frmStockPayments f = new frmStockPayments(supplierId);
            f.Show();
            return;
        }
        public double AssignGrandTotalWithHoldingTax()
        {
            double grandTotal = 0;
            try
            {
                //grandTotal = SharedFunctions.StringToDouble(txtGrandTotal.Text);
                //int withholdingTax = SharedFunctions.StringToInt(txtWithHoldingTax.Text);
                //if (grandTotal > 0 && withholdingTax > 0)
                //{
                //    if (cmbWithHoldingTaxType.SelectedIndex == 0)
                //    {
                //        double value = (grandTotal * withholdingTax) / 100;
                //        grandTotal = grandTotal + value;
                //        txtGrandTotal.Text = (grandTotal).ToString();
                //    }
                //    else
                //    {
                //        grandTotal = grandTotal + withholdingTax;
                //        txtGrandTotal.Text = (grandTotal).ToString();
                //    }
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return grandTotal;
        }

        private void txtWithHoldingTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void ErrMessage_Click(object sender, EventArgs e)
        {

        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uC_SearchItems1_Load(object sender, EventArgs e)
        {

        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some Items Before Holding", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbSuppliers.Text == "Select Supplier" || cmbSuppliers.Text == "")
            {
                MessageBox.Show("Please Select Supplier it must be mandatory", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                int supplierId = getSupplierID();
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (Convert.ToInt32(r.Cells["colQuantity"].Value) < 1)
                    {
                        MessageBox.Show("Some items have 0 quantity, please check before proceeding.", "Invaliud Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                //if (grdItems.Rows.Count > 50)
                //{
                //    label31.Visible = true;
                //    MessageBox.Show("Cann't add more than 50 items in one stock.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                if (IsMandatoryFields())
                    return;

                if (this.StockEditID > 0)
                {
                    UpdateHoldStock();
                    isholdchekc = 0;
                }
                else// this will be used in case of ADD-STOCK from purchase order as well.
                {
                    HoldStock();
                    isholdchekc = 0;
                }

                //ctrl+P working
                if (controlVal == "StockPaymentShow")
                {
                    showSupplierPayment(supplierId);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='False' or IsActive='false')");
            ShowHoldingStock(1);
            if (totalholds <= dt.Rows.Count)
            {
                LoadHoldStock(Convert.ToInt64(dt.Rows[0][0].ToString()));
                iii = 1;
            }
            btnLastPage.Enabled = true;
            isholdchekc = 1;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='False' or IsActive='false')");
            if (totalholds != 0)
            {
                totalholds = totalholds - 1;
            }
            ShowHoldingStock(totalholds);
            if (totalholds != 0 && totalholds <= dt.Rows.Count)
            {
                LoadHoldStock(Convert.ToInt64(dt.Rows[totalholds-1][0].ToString()));
                iii = 1;
            }
            btnNext.Enabled = true;
            isholdchekc = 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='False' or IsActive='false')");
            if (totalholds <= dt.Rows.Count)
            {
                totalholds = totalholds + 1;
            }
            ShowHoldingStock(totalholds);
            if (totalholds != 0 && totalholds<=dt.Rows.Count)
            {
                
                LoadHoldStock(Convert.ToInt64(dt.Rows[totalholds-1][0].ToString()));
                iii = 1;
            }
            btnPrevious.Enabled = true;
            isholdchekc = 1;
            
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='False' or IsActive='false')");
            ShowHoldingStock(dt.Rows.Count-1);
            if (totalholds <= dt.Rows.Count)
            {
                LoadHoldStock(Convert.ToInt64(dt.Rows[dt.Rows.Count-1][0].ToString()));
                iii = 1;
            }
            btnFirstPage.Enabled = true;
            isholdchekc = 1;
        }

        private void btnCancelHold_Click(object sender, EventArgs e)
        {
            DataTable dt = get("Select StockId from Stocks where IsHolding = 1 and (IsActive='False' or IsActive='false')");
            if (dt.Rows.Count > 0 && totalholds !=0)
            {
                long stockid = Convert.ToInt64(dt.Rows[totalholds-1]["StockId"].ToString());
                cancelhold(stockid);
                totalholds = 0;
                ShowHoldingStock(0);
                this.StockEditID = 0;
                isholdchekc = 0;
                
            }
        }
        private void cancelhold(long holdid)
        {
            try
            {
                if (holdid > 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock obj = unitOfWork.StockRepository.GetStockById_Edit(holdid);
                        obj.IsActive = false;
                        obj.Ishold = false;
                        obj.IsHolding = 0;
                        foreach (StockItem i in obj.StockItems)
                        {
                            i.IsActive = false;
                            i.Stock.IsHolding = 0;
                            i.Stock.Ishold = false;
                            i.Stock.IsActive = false;
                            unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                        }
                        
                        unitOfWork.Save();
                    }
                    MessageBox.Show("Stock deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClearAll.PerformClick();
                    ShowHoldingStock(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while cancelling holding invoice.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void grdItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            calculatteGrandValues(grdItems);
            calculateDiscPrice();
            if (checkWHT(grdItems) || checkAdvanceWHT(grdItems))
            {
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtWithHoldingTax.Text = "0";
                cmbWithHoldingTaxType.Text = "Value";
                double sum = 0;
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    if (row.Cells["colWHTType"].Value.ToString() != "%")
                    {
                        sum += Convert.ToDouble(row.Cells["colWHT"].Value);
                    }
                    else
                    {
                        sum += ((Convert.ToDouble(row.Cells["colWHT"].Value)*Convert.ToDouble(row.Cells["colTotalCost"].Value)) / 100) ;
                    }
                }
                txtWithHoldingTax.Text = sum.ToString();

                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Text = "0";
                cmbWithAdvanceHoldingTaxType.Text = "Value";
                double sum1 = 0;
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    if (row.Cells["colAdvanceWHTType"].Value.ToString() != "%")
                    {
                        sum1 += Convert.ToDouble(row.Cells["colAdvanceWHT"].Value);
                    }
                    else
                    {
                        sum1 += ((Convert.ToDouble(row.Cells["colAdvanceWHT"].Value) * Convert.ToDouble(row.Cells["colTotalCost"].Value)) / 100);
                    }
                }
                txtAdvanceWithHoldingTax.Text = sum1.ToString();
            }
            else if(txtNetDiscount.Enabled)
            {
                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;
                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
                txtWithHoldingTax.Text = "0";
                txtAdvanceWithHoldingTax.Text = "0";
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;   
            }
            else
            {
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                txtWithHoldingTax.Text = "0";
                txtAdvanceWithHoldingTax.Text = "0";
                txtNetDiscount.Enabled = false;
                txtNetSaleTax.Enabled = false;  
            }
            if (Properties.Settings.Default.IsUsingBarcode)
            {
                uC_SearchItems1.SetFocus();
                Properties.Settings.Default.IsUsingBarcode = false;
            }
           
        }
        private void LoadDiscPrice(int Index)
        {
            try
            {
                Double UnitCost = Convert.ToDouble(grdItems.Rows[Index].Cells["colUnitCost"].Value);
                Double Discount = Convert.ToDouble(grdItems.Rows[Index].Cells["colDiscount"].Value);
                //Double RetailPrice = Convert.ToDouble(grdItems.Rows[Index].Cells["colRetailPrice"].Value);
                int qty = Convert.ToInt32(grdItems.Rows[Index].Cells["colQuantity"].Value);
                string DiscType = grdItems.Rows[Index].Cells["colPercDiscType"].Value.ToString();
                string Unit = grdItems.Rows[Index].Cells["colUnit"].Value.ToString();
                int ConvUnit = Convert.ToInt32(grdItems.Rows[Index].Cells["colConvUnit"].Value);

                Double DiscPrice = UnitCost;
                if (DiscType != "%")
                {
                    DiscPrice = UnitCost - Discount;
                }
                else
                {
                    Discount = ((Discount / 100) * (qty * UnitCost))/qty;
                    DiscPrice = UnitCost - Discount;
                }
                //if (Unit == "Pack" || Unit == "pack")
                //{
                grdItems.Rows[Index].Cells["DiscountedPrice"].Value = DiscPrice.ToString();
                //grdItems.Rows[Index].Cells["colNetProfit"].Value = Profit(RetailPrice, DiscPrice);
                //}
                //else
                //{
                //    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = (DiscPrice/ConvUnit).ToString();
                //}
            }
            catch (Exception) { }
        }
        private void calculateDiscPrice()
        {
            try
            {
                Double UnitCost = Convert.ToDouble(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colUnitCost"].Value);
                Double Discount = Convert.ToDouble(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colDiscount"].Value);
                //Double RetailPrice = Convert.ToDouble(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colRetailPrice"].Value);
                int qty = Convert.ToInt32(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colQuantity"].Value);
                string DiscType = grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colPercDiscType"].Value.ToString();
                string Unit = grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colUnit"].Value.ToString();
                int ConvUnit = Convert.ToInt32(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colConvUnit"].Value);
                
                Double DiscPrice = UnitCost; 
                if (DiscType != "%")
                {
                    DiscPrice = UnitCost - Discount/qty;
                }
                else
                {
                    Discount = ((Discount / 100) * (qty * UnitCost))/qty;
                    DiscPrice = UnitCost - Discount;
                }
                //if (Unit == "Pack" || Unit == "pack")
                //{
                    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = DiscPrice.ToString();
                    //grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colNetProfit"].Value = Profit(RetailPrice, DiscPrice);
                //}
                //else
                //{
                //    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = (DiscPrice/ConvUnit).ToString();
                //}
            }
            catch (Exception) { }
        }
        private void calculateDiscPrice1()
        {
            try
            {
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    Double UnitCost = Convert.ToDouble(row.Cells["colUnitCost"].Value);
                    Double Discount = Convert.ToDouble(row.Cells["colDiscount"].Value);
                    //Double RetailPrice = Convert.ToDouble(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colRetailPrice"].Value);
                    int qty = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    string DiscType = row.Cells["colPercDiscType"].Value.ToString();
                    string Unit = row.Cells["colUnit"].Value.ToString();
                    int ConvUnit = Convert.ToInt32(row.Cells["colConvUnit"].Value);

                    Double DiscPrice = UnitCost;
                    if (DiscType != "%")
                    {
                        DiscPrice = UnitCost - Discount / qty;
                    }
                    else
                    {
                        Discount = ((Discount / 100) * (qty * UnitCost)) / qty;
                        DiscPrice = UnitCost - Discount;
                    }
                    //if (Unit == "Pack" || Unit == "pack")
                    //{
                    row.Cells["DiscountedPrice"].Value = DiscPrice.ToString();
                    //grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colNetProfit"].Value = Profit(RetailPrice, DiscPrice);
                    //}
                    //else
                    //{
                    //    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = (DiscPrice/ConvUnit).ToString();
                    //}
                }
            }
            catch (Exception) { }
        }
        private void calculateDiscPrice(DataGridView dgv)
        {
            try
            {
                foreach(DataGridViewRow row in dgv.Rows)
                {
                    Double UnitCost = Convert.ToDouble(row.Cells["colUnitCost"].Value);
                    Double Discount = Convert.ToDouble(row.Cells["colDiscount"].Value);
                    //Double RetailPrice = Convert.ToDouble(grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colRetailPrice"].Value);
                    int qty = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    string DiscType = row.Cells["colPercDiscType"].Value.ToString();
                    string Unit = row.Cells["colUnit"].Value.ToString();
                    int ConvUnit = Convert.ToInt32(row.Cells["colConvUnit"].Value);

                    Double DiscPrice = UnitCost;
                    if (DiscType != "%")
                    {
                        DiscPrice = UnitCost - Discount / qty;
                    }
                    else
                    {
                        Discount = ((Discount / 100) * (qty * UnitCost)) / qty;
                        DiscPrice = UnitCost - Discount;
                    }
                    //if (Unit == "Pack" || Unit == "pack")
                    //{
                    row.Cells["DiscountedPrice"].Value = DiscPrice.ToString();
                }
                //grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["colNetProfit"].Value = Profit(RetailPrice, DiscPrice);
                //}
                //else
                //{
                //    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = (DiscPrice/ConvUnit).ToString();
                //}
            }
            catch (Exception) { }
        }

        private void uC_SearchItems1_Leave(object sender, EventArgs e)
        {
            uC_SearchItems1.SetText = "";
            if (Properties.Settings.Default.IsUsingBarcode)
            {
                uC_SearchItems1.SetFocus();
                Properties.Settings.Default.IsUsingBarcode = false;
            }

        }
        public long avqty567 = 0;
        public string unit567 = "";
        private void grdItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (isholdchekc == 1)
            {
                avqty567 = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                unit567 = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
               
            }
        }

        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (grdItems.Rows.Count > 0)
                {
                    if (grdItems.CurrentCell == grdItems.SelectedRows[0].Cells["colWHT"])
                    {
                        CalculateValues(grdItems.SelectedRows[0]);
                        calculatteGrandValues(grdItems);

                        GetGrandTotal();
                    }
                }
            }
            
        }

        private void txtNetDiscount_EnabledChanged(object sender, EventArgs e)
        {
            if (txtNetDiscount.Enabled)
            {
                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;

                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
            }
            else
            {
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;

                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
            }
        }

        private void grdItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colWHT"].Index)
                    uC_SearchItems1.SetFocus();

            }
        }

        private void frmAddStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (grdItems.Focused)
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (grdItems.CurrentCell == null)
                        return;
                    if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colWHT"].Index)
                        uC_SearchItems1.SetFocus();

                }

            }
        }

        private void txtWithHoldingTax_EnabledChanged(object sender, EventArgs e)
        {
            if (txtWithHoldingTax.Enabled)
            {
                txtNetSaleTax.Enabled = true;
                txtNetDiscount.Enabled = true;

                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
            }
            else
            {
                txtNetSaleTax.Enabled = false;
                txtNetDiscount.Enabled = false;

                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
            }
        }

        private void txtAdvanceWithHoldingTax_TextChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
        }

        private void cmbWithAdvanceHoldingTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGrandTotal.Text = GetGrandTotal().ToString();
        }

        private void txtAdvanceWithHoldingTax_EnabledChanged(object sender, EventArgs e)
        {
            if (txtAdvanceWithHoldingTax.Enabled)
            {
                txtNetSaleTax.Enabled = true;
                txtNetDiscount.Enabled = true;

                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;
            }
            else
            {
                txtNetSaleTax.Enabled = false;
                txtNetDiscount.Enabled = false;

                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cbouom.SelectedIndex == 0)
            {
                
                foreach(DataGridViewRow row in grdItems.Rows)
                {
                    row.Cells["colUnit"].Value = "Pack";
                }
                EndEdit();


            }
            else if(cbouom.SelectedIndex == 1)
            {
                
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    row.Cells["colUnit"].Value = "Units";
                }
                EndEdit();


            }
            else
            {
                return;
            }
        }
        private void EndEdit()
        {
            calculatteGrandValues(grdItems);
            calculateDiscPrice1();
            if (checkWHT(grdItems) || checkAdvanceWHT(grdItems))
            {
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtWithHoldingTax.Text = "0";
                cmbWithHoldingTaxType.Text = "Value";
                double sum = 0;
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    if (row.Cells["colWHTType"].Value.ToString() != "%")
                    {
                        sum += Convert.ToDouble(row.Cells["colWHT"].Value);
                    }
                    else
                    {
                        sum += ((Convert.ToDouble(row.Cells["colWHT"].Value) * Convert.ToDouble(row.Cells["colTotalCost"].Value)) / 100);
                    }
                }
                txtWithHoldingTax.Text = sum.ToString();

                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Text = "0";
                cmbWithAdvanceHoldingTaxType.Text = "Value";
                double sum1 = 0;
                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    if (row.Cells["colAdvanceWHTType"].Value.ToString() != "%")
                    {
                        sum1 += Convert.ToDouble(row.Cells["colAdvanceWHT"].Value);
                    }
                    else
                    {
                        sum1 += ((Convert.ToDouble(row.Cells["colAdvanceWHT"].Value) * Convert.ToDouble(row.Cells["colTotalCost"].Value)) / 100);
                    }
                }
                txtAdvanceWithHoldingTax.Text = sum1.ToString();
            }
            else if (txtNetDiscount.Enabled)
            {
                txtWithHoldingTax.Enabled = true;
                cmbWithHoldingTaxType.Enabled = true;
                txtAdvanceWithHoldingTax.Enabled = true;
                cmbWithAdvanceHoldingTaxType.Enabled = true;
                txtWithHoldingTax.Text = "0";
                txtAdvanceWithHoldingTax.Text = "0";
                txtNetDiscount.Enabled = true;
                txtNetSaleTax.Enabled = true;
            }
            else
            {
                txtWithHoldingTax.Enabled = false;
                cmbWithHoldingTaxType.Enabled = false;
                txtAdvanceWithHoldingTax.Enabled = false;
                cmbWithAdvanceHoldingTaxType.Enabled = false;
                txtWithHoldingTax.Text = "0";
                txtAdvanceWithHoldingTax.Text = "0";
                txtNetDiscount.Enabled = false;
                txtNetSaleTax.Enabled = false;
            }
            if (Properties.Settings.Default.IsUsingBarcode)
            {
                uC_SearchItems1.SetFocus();
                Properties.Settings.Default.IsUsingBarcode = false;
            }
        }
    }
}