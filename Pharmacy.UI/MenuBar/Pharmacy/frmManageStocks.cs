using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.Reports;
using Pharmacy.Core.ViewModels;
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Drawing.Printing;
using Zen.Barcode;
using System.Data.SqlClient;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmManageStocks : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        int srno = 1;
        IPagedList<StockVM> StocksList;
        Bitmap resultImage = null;

        #region LoadDetials
        public frmManageStocks()
        {
            InitializeComponent();
            
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            cmbStore.Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            txtStore.Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewStock, btnPrint, btnExcel });
            dtpFrom.Checked = true;
            dtpTo.Checked = true;
            LoadDetails();
            LoadStocks();
        }
        private void LoadDetails()
        {
            LoadSuppliers();
            LoadStores();
        }
        private void LoadStores()
        {

            cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
            List<StoreVM> stores = new List<StoreVM>();
            using (unitOfWork = new UnitOfWork())
            {
                stores = unitOfWork.StoreRepository.GetAllStores().ToList();
            }
            StoreVM store = new StoreVM();
            store.Name = "Select";
            store.StoreId = 0;
            stores.Insert(0, store);
            cmbStore.DataSource = stores;
            cmbStore.ValueMember = "StoreId";
            cmbStore.DisplayMember = "Name";
            cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
        }
        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> suppliers = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            }
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
        }
        private void LoadStocks([CallerMemberName] string functionName = "")
        {
            flowPanel.Controls.Clear();
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (dtpFrom.Checked)
                fromDate = dtpFrom.Value;
            if (dtpTo.Checked)
                toDate = dtpTo.Value;
            using (unitOfWork = new UnitOfWork())
            {
                StocksList = unitOfWork.StockRepository.GenericSearchForManageStocks(
                    FromDate: fromDate,
                    ToDate: toDate,
                    
                    ItemNameFilter: txtSearchByName.Text.Trim(),
                    SupplierInvoiceNo: txtSupplierInvoice.Text.Trim(),
                    StockDocNo: txtDocNo.Text.Trim(),
                    SupplierId: SharedFunctions.StringToLong(cmbSuppliers.SelectedValue),
                    StoreId: SharedFunctions.StringToInt(cmbStore.SelectedValue),
                    pageNo: pageNo,
                    PageSize: SharedVariables.PageSize
                    );
                List<double> totalNetValue = unitOfWork.StockRepository.GenericSumOfManageStocks(
                    FromDate: fromDate,
                    ToDate: toDate,
                    ItemNameFilter: txtSearchByName.Text.Trim(),
                    SupplierInvoiceNo: txtSupplierInvoice.Text.Trim(),
                    StockDocNo: txtDocNo.Text.Trim(),
                    SupplierId: SharedFunctions.StringToLong(cmbSuppliers.SelectedValue),
                    StoreId: SharedFunctions.StringToInt(cmbStore.SelectedValue)
                    );
                txtTotalPurchase.Text = totalNetValue.Sum().ToString();
            }
            foreach (StockVM s in StocksList)
            {
                this.flowPanel.SuspendLayout();
                if (s.Ishold != true )
                {
                    BuildPanel(s);
                }
                this.flowPanel.ResumeLayout(true);
            }
            btnLastPage.Enabled = btnNext.Enabled = StocksList.HasNextPage ? true : false;
            btnFirstPage.Enabled = btnPrevious.Enabled = StocksList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, StocksList.PageCount);
        }
        #endregion
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
        #region LoadStockPanel
        private void BuildPanel(StockVM s)
        {
            using (unitOfWork = new UnitOfWork())
            {
                s = unitOfWork.StockRepository.GetStockById_Show(s.StockId);
            }
            int ConsumptionsCount = 0;
            foreach (StockItemVM si in s.StockItems)
            {
                if (si.StockConsumed > 0)
                {
                    ConsumptionsCount += 1;
                }
            }
            Panel pnlHeader = new Panel();
           
            // 
            // lblDocumentNo
            // 
            Label lblDocumentNo = new Label();
            lblDocumentNo.AutoSize = true;
            lblDocumentNo.Location = new System.Drawing.Point(10, 15);
            lblDocumentNo.Name = "lblDocumentNo";
            lblDocumentNo.Size = new System.Drawing.Size(35, 13);
            lblDocumentNo.TabIndex = 1;
            lblDocumentNo.Text = "Doc No: " + s.DocumentNo;

            // 
            // lblSupplier
            // 
            Label lblSupplier = new Label();
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new System.Drawing.Point(120, 15);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new System.Drawing.Size(35, 13);
            lblSupplier.TabIndex = 1;
            ImplementSupplierInvoiceNo(s, lblSupplier);


            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(523, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = s.CreateDate.ToString();


            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + s.UserName;

            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Size = new System.Drawing.Size(51, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = s.StockId;
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {

                //btnDelete.BackColor = Color.FromArgb(45, 45, 48);
                btnPrint.ForeColor = Color.Black;

            }
            btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1052, 0);
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = s.StockId;
            if (ConsumptionsCount > 0 || s.IsAutoInsertedStock)
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {

                //btnDelete.BackColor = Color.FromArgb(45, 45, 48);
                btnDelete.ForeColor = Color.Black;

            }
            //btnDelete.Enabled = ConsumptionsCount > 0 ? false : true;
            btnDelete.Click += new EventHandler((sender, e) => btnDelete_Click(sender, e, s.DocumentNo));
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1152, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = s.StockId;
            bool IsStockPricesEditable = false;
            if (ConsumptionsCount == s.StockItems.Count || s.IsAutoInsertedStock)
            {
                if (SharedVariables.AdminPharmacySetting.IsStockEditableUnitRetailPrice)
                    IsStockPricesEditable = true;
                btnEdit.Enabled = SharedVariables.AdminPharmacySetting.IsStockEditableUnitRetailPrice;
                //btnEdit.Enabled = false;
            }
            else
            {
                btnEdit.Enabled = true;
            }
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {

                //btnEdit.BackColor = Color.FromArgb(45, 45, 48);
                btnEdit.ForeColor = Color.Black;
                
            }
            //btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Click += new EventHandler((sender, e) => btnEdit_Click(sender, e, IsStockPricesEditable));

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(1152, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnEdit);
/*            pnlHeader.Controls.Add(btnDelete);*/
            pnlHeader.Controls.Add(btnPrint);
            /*pnlHeader.Controls.Add(lblDate);*/
            /*pnlHeader.Controls.Add(lblAddedBy);*/
            /*pnlHeader.Controls.Add(lblSupplier);
            pnlHeader.Controls.Add(lblDocumentNo);*/
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                pnlHeader.BackColor = Color.FromArgb(45, 45, 48);
                pnlHeader.ForeColor = Color.White;
            }

            Panel pnlButton = new Panel();
            // 
            // lblTotal
            // 
            Label lblTotal = new Label();
            lblTotal.AutoSize = true;
            lblTotal.Location = new System.Drawing.Point(1170, 10);
            lblTotal.Size = new System.Drawing.Size(50, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Text = "Total: " + s.GrandTotal.ToString();

            // 
            // lblTotalAdvanceTax
            // 
            Label lblTotalAdvanceTax = new Label();
            lblTotalAdvanceTax.AutoSize = true;
            lblTotalAdvanceTax.Location = new System.Drawing.Point(180, 10);
            lblTotalAdvanceTax.Size = new System.Drawing.Size(50, 10);
            lblTotalAdvanceTax.Name = "lblTotalAdvanceTax";
            lblTotalAdvanceTax.Text = "Total Advance Tax: " + s.AdvanceWithholdingTax;

            // 
            // lblTotalWHTTax
            // 
            Label lblTotalWHTTax = new Label();
            lblTotalWHTTax.AutoSize = true;
            lblTotalWHTTax.Location = new System.Drawing.Point(310, 10);
            lblTotalWHTTax.Size = new System.Drawing.Size(50, 10);
            lblTotalWHTTax.Name = "lblTotalWHTTax";
            lblTotalWHTTax.Text = "Total WHT: " + s.WithholdingTax;

            pnlButton.BackColor = Color.FromArgb(241, 241, 241);
            pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButton.Location = new System.Drawing.Point(1152, 0);
            pnlButton.Name = "pnlButton";
            pnlButton.Size = new System.Drawing.Size(1258, 39);
            pnlButton.TabIndex = 3;


            /*pnlButton.Controls.Add(lblTotal);
            pnlButton.Controls.Add(lblTotalAdvanceTax);
            pnlButton.Controls.Add(lblTotalWHTTax);*/

            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            //btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            btnEdit.Dock = DockStyle.Left;
            btnPrint.Dock = DockStyle.Left;
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //lblTotalAdvanceTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //lblTotalWHTTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                pnlButton.BackColor = Color.FromArgb(45, 45, 48);
                pnlButton.ForeColor = Color.White;
            }
            // 
            // grdStockItems
            // 
            DataGridView grdStockItem = new DataGridView();
            GenerateDatagridView(grdStockItem);
            grdStockItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            SharedFunctions.SetGridStyle(grdStockItem);
            
            //grdStockItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //grdStockItem.ColumnHeadersHeight = 35;
            //grdStockItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //grdStockItem.EnableHeadersVisualStyles = false;
            //grdStockItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //grdStockItem.RowTemplate.Height = 35;
            //grdStockItem.RowHeadersVisible = false;
            //grdStockItem.BackgroundColor = Color.White;
            grdStockItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdStockItem.Location = new System.Drawing.Point(0, 39);
            grdStockItem.Name = "grdStockItems";
            /*grdStockItem.Size = new System.Drawing.Size(1258, 201);*/
            grdStockItem.TabIndex = 1;
            grdStockItem.AllowUserToAddRows = false;
            grdStockItem.AllowUserToDeleteRows = false;
            grdStockItem.ScrollBars = ScrollBars.Both;
            grdStockItem.BackgroundColor = Color.AliceBlue;
            double discount = 0;
            //grdStockItem.SelectionMode = Data GridViewSelectionMode.FullRowSelect;
            foreach (StockItemVM i in s.StockItems)
            {    
                //if(i.HoldStock == 0)
                if (i.IsActive)
                {
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity;
                    i.BonusQuantity = i.Unit == 0 ? i.BonusQuantity / i.ConversionUnit : i.BonusQuantity;
                    i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                    i.RetailPrice = i.Unit == 0 ? i.RetailPrice * i.Item.ConversionUnit : i.RetailPrice;
                    i.StockReturn = i.Unit == 0 ? i.StockReturn / i.ConversionUnit : i.StockReturn;
                    grdStockItem.Rows.Add(
                        srno,
                        i.Item.ItemID,
                        i.Item.ItemName,
                        //i.UnitString,
                        i.Store != null ? i.Store.Name : "",
                        //i.Item.ConversionUnit,
                        i.Quantity,
                        //i.BonusQuantity,
                        i.UnitCost,

                        i.PercDiscType != 0 ? Math.Round(i.UnitCost - i.Discount / i.Quantity , 3) : Math.Round(i.UnitCost - (((i.Discount / 100) * (i.Quantity * i.UnitCost))) / i.Quantity , 3),
                        i.TotalCost,
                        i.RetailPrice,
                        //Profit(i.RetailPrice, i.PercDiscType != 0 ? (i.UnitCost - i.Discount/i.Quantity) : i.UnitCost - (((i.Discount / 100) * (i.Quantity * i.UnitCost))) /i.Quantity),
                        i.Batch.BatchName,
                        i.Discount,
                        i.PercDiscType == 0 ? "%" : "Value",
                        i.SalesTax,
                        i.NetValue,
                        i.WHT,
                        i.WHTType == 1 ? "%" : "Value",
                        i.AdWHT,
                        i.AdWHTType == 1 ? "%" : "Value", 
                        //GrossMargin(i.Quantity, i.RetailPrice, i.NetValue),
                        i.CreatedAt
                        //i.StockReturn,
                        );
                    //LoadDiscPrice(grdStockItem.Rows.Count - 1, grdStockItem);
                }
                srno++;
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = s.StockId;

            pnlDetail.Controls.Add(grdStockItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Controls.Add(pnlButton);
            
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1558, 240);
            pnlDetail.Dock = DockStyle.Top;
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            flowPanel.BackColor = Color.AliceBlue;
            flowPanel.ForeColor = Color.Black;
            pnlDetail.BackColor = Color.AliceBlue;
            pnlDetail.ForeColor = Color.Black;
            pnlHeader.BackColor = Color.AliceBlue;
            pnlHeader.ForeColor = Color.Black;
            pnlButton.BackColor = Color.AliceBlue;
            pnlButton.ForeColor = Color.Black;
            /*pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));*/
            pnlDetail.BorderStyle = BorderStyle.FixedSingle;
            //pnlDetail.Dock = DockStyle.Top;
            SharedFunctions.SetGridStyle(grdStockItem);
            //grdStockItem.Dock = DockStyle.Top;
            grdStockItem.BackgroundColor = Color.AliceBlue;
            //flowPanel.ScrollBars = ScrollBars.Both;
            flowPanel.Refresh();
            
        }
        private void LoadDiscPrice(int Index , DataGridView grdItems)
        {
            try
            {
                Double UnitCost = Convert.ToDouble(grdItems.Rows[Index].Cells["colUnitCost"].Value);
                Double Discount = Convert.ToDouble(grdItems.Rows[Index].Cells["colDiscount"].Value);
                Double RetailPrice = Convert.ToDouble(grdItems.Rows[Index].Cells["colRetailPrice"].Value);
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
                    Discount = ((Discount / 100) * (qty * UnitCost)) / qty;
                    DiscPrice = UnitCost - Discount;
                }
                //if (Unit == "Pack" || Unit == "pack")
                //{
                grdItems.Rows[Index].Cells["DiscountedPrice"].Value = DiscPrice.ToString();
                grdItems.Rows[Index].Cells["colNetProfit"].Value = Profit(RetailPrice, DiscPrice);
                //}
                //else
                //{
                //    grdItems.Rows[grdItems.SelectedCells[0].RowIndex].Cells["DiscountedPrice"].Value = (DiscPrice/ConvUnit).ToString();
                //}
            }
            catch (Exception) { }
        }
        private void ImplementSupplierInvoiceNo(StockVM s, Label lblSupplier)
        {
            if (s.Supplier == null && string.IsNullOrEmpty(s.SupplierInvoiceNo))
            {
                lblSupplier.Text = "N/A";
            }
            else
            {
                lblSupplier.Text = string.Concat(s.Supplier == null ? "" : s.Supplier.Name + " => ", string.IsNullOrEmpty(s.SupplierInvoiceNo) ? "" : s.SupplierInvoiceNo);// + Math.Round(s.DocumentNo, 0).ToString());
            }
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            srno = 1;
            DataGridViewTextBoxColumn colSrNo;
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            //DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colStore;
            //DataGridViewTextBoxColumn colConvUnit;
            DataGridViewTextBoxColumn colQuantity;
            //DataGridViewTextBoxColumn colBonusQuantity;
            DataGridViewTextBoxColumn colUnitCost;
            DataGridViewTextBoxColumn DiscountedPrice;
            DataGridViewTextBoxColumn colToalCost;
            DataGridViewTextBoxColumn colRetailPrice;
            //DataGridViewTextBoxColumn colProfitMargin;
            DataGridViewTextBoxColumn colBatch;
            DataGridViewTextBoxColumn colDiscount;
            DataGridViewTextBoxColumn colDiscounttype;
            DataGridViewTextBoxColumn colSalesTax;
            DataGridViewTextBoxColumn colNetValue;
            DataGridViewTextBoxColumn colWHT;
            DataGridViewTextBoxColumn colWHTType;
            DataGridViewTextBoxColumn colAdvanceWHT;
            DataGridViewTextBoxColumn colAdvanceWHTType;
            //DataGridViewTextBoxColumn colGrossMargin;
            DataGridViewTextBoxColumn colCreatedAt;

            colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSrNo.HeaderText = "Sr#";
            colSrNo.ReadOnly = true;
            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colBonusQuantity = new DataGridViewTextBoxColumn();
            colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DiscountedPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colToalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colProfitMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDiscounttype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colWHT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAdvanceWHT = new System.Windows.Forms.DataGridViewTextBoxColumn();

            //colGrossMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colWHTType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAdvanceWHTType = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colSrNo,
            colItemId,
            colItemName,
            //colUnit,
            colStore,
            //colConvUnit,
            colQuantity,
            //colBonusQuantity,
            colUnitCost,
            DiscountedPrice,
            colToalCost,
            colRetailPrice,
            //colProfitMargin,
            colBatch,
            colDiscount,
            colDiscounttype,
            colSalesTax,
            colNetValue,
            colWHT,
            colWHTType,
            colAdvanceWHT,
            colAdvanceWHTType,
            //colGrossMargin,
            colCreatedAt});
            dataGridView1.Location = new System.Drawing.Point(15, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1258, 150);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;
            // 
            // colItemId
            // 
            colSrNo.HeaderText = "Sr#";
            colSrNo.Name = "colSrNo";
            colSrNo.ReadOnly = true;
            colSrNo.Width = 30;
            colSrNo.MinimumWidth = 30;
            colSrNo.Visible = true;

            colItemId.HeaderText = "ItemId";
            colItemId.Name = "colItemId";
            colItemId.ReadOnly = true;
            
            colItemId.Visible = false;
            // 
            // colItemName
            // 
            colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colItemName.HeaderText = "Item";
            colItemName.Name = "colItemName";
            //colItemName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colItemName.MinimumWidth = 170;
            colItemName.ReadOnly = true;

            // 
            // colUnit
            // 
            /*colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colUnit.HeaderText = "Unit";
            colUnit.Visible = true;
            colUnit.MinimumWidth = 80;
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;*/

            // 
            // colStore
            // 
            colStore.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colStore.HeaderText = "Store";
            colStore.Width = SharedVariables.AdminPharmacySetting.IsEnableStores == true ? 70 : 0;
            colStore.Name = "colStore";
            colStore.Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            colStore.ReadOnly = true;

            // 
            // colConvUnit
            // 
            /*colConvUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colConvUnit.MinimumWidth = 80;
            colConvUnit.HeaderText = "Conv.Unit";
            colConvUnit.Name = "colConvUnit";
            colConvUnit.ReadOnly = true;*/
            //
            //
            colWHT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colWHT.MinimumWidth = 80;
            colWHT.Width = 80;
            colWHT.HeaderText = "WHT";
            colWHT.Name = "colWHT";
            colWHT.ReadOnly = true;
            //
            //
            //
            //
            colWHTType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colWHTType.MinimumWidth = 80;
            colWHTType.Width = 80;
            colWHTType.HeaderText = "WHT Type";
            colWHTType.Name = "colWHTType";
            colWHTType.ReadOnly = true;
            //
            //
            colAdvanceWHT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colAdvanceWHT.MinimumWidth = 80;
            colAdvanceWHT.Width = 80;
            colAdvanceWHT.HeaderText = "Advance Tax";
            colAdvanceWHT.Name = "colAdvanceWHT";
            //colAdvanceWHT.Visible = SharedVariables.AdminPharmacySetting.IsAdvanceTaxStock;
            colAdvanceWHT.ReadOnly = true;
            //
            //
            //
            //
            colAdvanceWHTType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colAdvanceWHTType.MinimumWidth = 80;
            colAdvanceWHTType.Width = 80;
            colAdvanceWHTType.HeaderText = "Advance Tax Type";
            colAdvanceWHTType.Name = "colAdvanceWHTType";
            //colAdvanceWHTType.Visible = SharedVariables.AdminPharmacySetting.IsAdvanceTaxStock;
            colAdvanceWHTType.ReadOnly = true;
            //
            //
            // 
            // colQuantity
            // 
            //colQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colQuantity.Width = 80;
            colQuantity.MinimumWidth = 80;
            colQuantity.HeaderText = "Quantity";
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            // 
            // colBonusQuantity
            //
            /*colBonusQuantity.Width = 80;
            colBonusQuantity.MinimumWidth = 80;
            colBonusQuantity.HeaderText = "Bonus Quantity";
            colBonusQuantity.Name = "colBonusQuantity";
            colBonusQuantity.ReadOnly = true;*/
            // 
            // colUnitCost
            // 
            colUnitCost.Width = 80;
            colUnitCost.MinimumWidth = 80;
            colUnitCost.HeaderText = "UnitCost";
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            // 
            // DiscPrice
            // 
            DiscountedPrice.Width = 80;
            DiscountedPrice.MinimumWidth = 80;
            DiscountedPrice.HeaderText = "Disc Price";
            DiscountedPrice.Name = "DiscountedPrice";
            DiscountedPrice.ReadOnly = true;
            // 
            // colToalCost
            // 
            colToalCost.Width = 80;
            colToalCost.MinimumWidth = 80;
            colToalCost.HeaderText = "Total Cost";
            colToalCost.Name = "colToalCost";
            colToalCost.ReadOnly = true;
            // 
            // colRetailPrice
            // 
            colRetailPrice.Width = 80;
            colRetailPrice.MinimumWidth = 80;
            colRetailPrice.HeaderText = "Retail Price";
            colRetailPrice.Name = "colRetailPrice";
            colRetailPrice.ReadOnly = true;
            // 
            // colProfitMargin
            // 
            /*colProfitMargin.Width = 80;
            colProfitMargin.MinimumWidth = 80;
            colProfitMargin.HeaderText = "Profit Margin%";
            colProfitMargin.Name = "colProfitMargin";
            colProfitMargin.ReadOnly = true;*/
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            colBatch.Width = 80;
            colBatch.MinimumWidth = 80;

            // 
            // colDiscount
            // 
            colDiscount.Width = 80;
            colDiscount.MinimumWidth = 80;
            colDiscount.HeaderText = "Discount";
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            //
            // Discount Type
            //
            colDiscounttype.Width = 80;
            colDiscounttype.MinimumWidth = 80;
            colDiscounttype.HeaderText = "Discount type";
            colDiscounttype.Name = "colDiscounttype";
            colDiscounttype.ReadOnly = true;
            // 
            // colSalesTax
            // 
            colSalesTax.Width = 80;
            colSalesTax.MinimumWidth = 80;
            colSalesTax.HeaderText = "Sales Tax";
            colSalesTax.Name = "colSalesTax";
            colSalesTax.ReadOnly = true;
            // 
            // colNetValue
            // 
            colNetValue.Width = 80;
            colNetValue.MinimumWidth = 80;
            colNetValue.HeaderText = "Net Value";
            colNetValue.Name = "colNetValue";
            colNetValue.ReadOnly = true;
            // 
            // colGrossMargin
            // 
            /*colGrossMargin.Width = 80;
            colGrossMargin.MinimumWidth = 80;
            colGrossMargin.HeaderText = "Gross Margin";
            colGrossMargin.Name = "colGrossMargin";
            colGrossMargin.ReadOnly = true;*/
            // 
            // colCreatedAt
            // 
            colCreatedAt.HeaderText = "Create At";
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.MinimumWidth = 150;
            colCreatedAt.ReadOnly = true;
            SourceGrid.ScrollBars = ScrollBars.Both;
      

        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }
        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)e.Control;
            AddedPanel.Width = flowPanel.Width - 23;
        }
        #endregion

        #region HeaderButtons
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;

            if (SharedVariables.manageStockRefr == true)
            {
                dtpFrom.Checked = true;
                dtpTo.Checked = true;
            }
            else
            {
                dtpFrom.Checked = false;
                dtpTo.Checked = false;
            }
            cmbSuppliers.SelectedIndex = 0;
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            txtSearchByName.Text = "";
            txtSupplierInvoice.Text = "";
            LoadDetails();
            LoadStocks();
            SharedVariables.manageStockRefr = false;
        }
        private void btnAddNewStock_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Stock"))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            //f.FormClosed += new FormClosedEventHandler(frmAddNewStock_Closed);
        }
        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            List<StocksReportVM> StocksList = this.GetReportData();
            StocksViewer v = new StocksViewer(this.dtpFrom.Value, this.dtpTo.Value, StocksList);
            v.Show();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StocksRpt rpt = new StocksRpt();
                rpt.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                rpt.Database.Tables[1].SetDataSource(this.GetReportData());
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("FromDate", DateTime.Now);
                rpt.SetParameterValue("ToDate", DateTime.Now);
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
        private List<StocksReportVM> GetReportData()
        {
            List<StocksReportVM> rptList = new List<StocksReportVM>();
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        rptList = unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, Convert.ToInt64(cmbSuppliers.SelectedValue));
                    }
                    else if (txtSearchByName.Text.Trim() != "")
                    {
                        rptList = unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, txtSearchByName.Text.Trim().ToLower());
                    }
                    else
                    {
                        rptList = unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while fetching report data, Please try again.", ex.Message, "Failed.");
            }
            return rptList;
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

            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddNewStock.PerformClick();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Filters
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
            flowPanel_SizeChanged(null, null);
        }
        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
            flowPanel_SizeChanged(null, null);
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pageNo = 1;
                LoadStocks();
            }
        }
        private void txtSupplierInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    pageNo = 1;
                    LoadStocks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
        }

        #endregion

        #region DataGridEvents
        private void btnEdit_Click(object sender, EventArgs e, bool IsStockPricesEditable)
        {
            try
            {
                if (!SharedFunctions.IsActionallowed("Edit Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                {
                    SharedFunctions.AccessDeniedMsg();
                    return;
                }
                long Stockid = (long)((Button)sender).Tag;
                bool IsStoreitem = false;
                using (unitOfWork = new UnitOfWork())
                {
                    IsStoreitem = unitOfWork.StockRepository.IsStoreItem(Stockid);
                }
                if (IsStoreitem == true && SharedVariables.AdminPharmacySetting.IsEnableStores == false)
                {
                    SharedFunctions.MessageBoxShow("Please enable store to view this.", MessageType.Warning);
                    return;
                }
                Form f = SharedFunctions.OpenForm(new frmAddStock(Stockid, 0, IsStockPricesEditable), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler(ChildFormUpdate_Closed);
                f.Show();
                //frmAddStock f = new frmAddStock(Stockid, 0);
                //f.MdiParent = this.MdiParent;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e, decimal DoscumentNo)
        {
            string msgContent = "Are you sure you want to delete this record";
            long StockId = SharedFunctions.StringToLong(((Button)sender).Tag);
            //----------Check Stock Return Exists
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.StockRepository.StockHasReturnStock(StockId))
                    msgContent = "This stock also has a stock return. Are you sure you want to delete this stock and stock return?";

                DialogResult rs = MessageBox.Show(msgContent, "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == System.Windows.Forms.DialogResult.Cancel)
                    return;

                unitOfWork.StockRepository.SetInactive(StockId);
                AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + ((long)DoscumentNo).ToString(), AuditModule.Stock, AuditAction.deleted);
                //SOT
                Stock stock = new Stock() { StockId = StockId };
                OfflineJsonTask.SaveOfflineTask(unitOfWork, stock, ModuleName.Stock, ModuleAction.Delete, stock.StockId);
            }
            foreach (Panel p in flowPanel.Controls)
            {
                if (Convert.ToInt64(p.Tag) == StockId)
                {
                    flowPanel.Controls.Remove(p);
                }
            }
            btnRefresh.PerformClick();
            //flowPanel_SizeChanged(null, null);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            long Stockid = (long)((Button)sender).Tag;
            if (SharedVariables.AdminPharmacySetting.IsItemPrintOnStock)
            {
                DialogResult rs = MessageBox.Show("Click Yes if you want to print items' barcode or Click No if you want to print stock invoice.", "Please Make Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == System.Windows.Forms.DialogResult.Yes)
                {
                    new frmPrintStockBarcode(Stockid).ShowDialog();
                }
                else if (rs == System.Windows.Forms.DialogResult.No)
                {
                    StockDetailViewer v = new StockDetailViewer(Stockid);
                    v.Show();
                }
            }
            else
            {
                StockDetailViewer v = new StockDetailViewer(Stockid);
                v.Show();
            }
        }

     
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmManageStockShortCuts f = new ShortCutDialogs.frmManageStockShortCuts();
            f.ShowDialog();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void ChildFormUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            LoadStocks();
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
        #endregion

        #region PaginationButtons
        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadStocks();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadStocks();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = StocksList.PageCount;
            LoadStocks();
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
                    if (_pageNo <= 0 || _pageNo > StocksList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadStocks();
                }
            }
        }
        #endregion

        private void txtSupplierInvoice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDocNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDocNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    pageNo = 1;
                    LoadStocks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}