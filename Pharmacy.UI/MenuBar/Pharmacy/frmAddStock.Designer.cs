namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmAddStock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddStock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDocumentNo = new System.Windows.Forms.TextBox();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSupplierInvoiceNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpSupplierInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.btnAttachImage = new System.Windows.Forms.Button();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStores = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvailableQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBonusQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountedPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpiry = new CustomCode.CalendarColumn();
            this.colBonusDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercDiscType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDiscType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercSalesTaxType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSalesTaxType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colWHT = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colWHTType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colAdvanceWHT = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colAdvanceWHTType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrossMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesTaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsRowUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.lblImagePath = new System.Windows.Forms.Label();
            this.lblRemoveFile = new System.Windows.Forms.LinkLabel();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.hlpGrd = new System.Windows.Forms.HelpProvider();
            this.btnSearchItems = new System.Windows.Forms.Button();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.lblLoadingSuppliers = new System.Windows.Forms.Label();
            this.lblClearExpiry = new System.Windows.Forms.LinkLabel();
            this.txtStockItemId = new System.Windows.Forms.TextBox();
            this.numRetailPrice = new System.Windows.Forms.NumericUpDown();
            this.numSalesTax = new System.Windows.Forms.NumericUpDown();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.numUnitCost = new System.Windows.Forms.NumericUpDown();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.ErrNetValue = new System.Windows.Forms.Label();
            this.ErrSalesTax = new System.Windows.Forms.Label();
            this.ErrDiscount = new System.Windows.Forms.Label();
            this.ErrExpiry = new System.Windows.Forms.Label();
            this.ErrBatchName = new System.Windows.Forms.Label();
            this.ErrRetailPrice = new System.Windows.Forms.Label();
            this.ErrTotalCost = new System.Windows.Forms.Label();
            this.ErrUnitCost = new System.Windows.Forms.Label();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.ErrSelectItem = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtNetValue = new System.Windows.Forms.TextBox();
            this.cmbSalesTaxType = new System.Windows.Forms.ComboBox();
            this.cmbDiscountType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.dtpExpiry = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTotalCost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.numConvUnit = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.errConvUnit = new System.Windows.Forms.Label();
            this.btlLoadSupProds = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label19 = new System.Windows.Forms.Label();
            this.btnExpApply = new System.Windows.Forms.Button();
            this.btnExpCancel = new System.Windows.Forms.Button();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.btnLoadMissedSales = new System.Windows.Forms.Button();
            this.btnSupLowStockItems = new System.Windows.Forms.Button();
            this.btnLPrvSoldItems = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbWithAdvanceHoldingTaxType = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txtAdvanceWithHoldingTax = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txttotalofbonusqty = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.txttotalofqty = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.txtTotalofCost = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txttotalofrate = new System.Windows.Forms.TextBox();
            this.grpInvoiceHolding = new System.Windows.Forms.GroupBox();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancelHold = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.cmbWithHoldingTaxType = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtWithHoldingTax = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtTotalSaleTax = new System.Windows.Forms.TextBox();
            this.cmbNetSaleTaxType = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtNetSaleTax = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbNetDiscountType = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtNetDiscount = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtTotalDiscount = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbouom = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new CustomCode.CalendarColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uC_SearchItems1 = new UC_SearchItems();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetailPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grpInvoiceHolding.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Document Number";
            // 
            // txtDocumentNo
            // 
            this.txtDocumentNo.Enabled = false;
            this.txtDocumentNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentNo.Location = new System.Drawing.Point(161, 37);
            this.txtDocumentNo.Name = "txtDocumentNo";
            this.txtDocumentNo.Size = new System.Drawing.Size(344, 26);
            this.txtDocumentNo.TabIndex = 5;
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSuppliers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSuppliers.DropDownWidth = 350;
            this.cmbSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(161, 70);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(344, 28);
            this.cmbSuppliers.TabIndex = 6;
            this.cmbSuppliers.SelectedIndexChanged += new System.EventHandler(this.cmbSuppliers_SelectedIndexChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(1035, 19);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 21);
            this.dtpDate.TabIndex = 12;
            this.dtpDate.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select Supplier";
            // 
            // txtSupplierInvoiceNo
            // 
            this.txtSupplierInvoiceNo.Location = new System.Drawing.Point(892, 22);
            this.txtSupplierInvoiceNo.Name = "txtSupplierInvoiceNo";
            this.txtSupplierInvoiceNo.Size = new System.Drawing.Size(178, 21);
            this.txtSupplierInvoiceNo.TabIndex = 9;
            this.txtSupplierInvoiceNo.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(787, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "Supplier Invoice#";
            this.label3.Visible = false;
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSupplier.ForeColor = System.Drawing.Color.Black;
            this.btnAddSupplier.Location = new System.Drawing.Point(527, 70);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(54, 28);
            this.btnAddSupplier.TabIndex = 7;
            this.btnAddSupplier.Text = "+";
            this.btnAddSupplier.UseVisualStyleBackColor = false;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(892, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Date";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(892, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 14);
            this.label5.TabIndex = 10;
            this.label5.Text = "Supplier Invoice Date";
            this.label5.Visible = false;
            // 
            // dtpSupplierInvoiceDate
            // 
            this.dtpSupplierInvoiceDate.Location = new System.Drawing.Point(1035, 59);
            this.dtpSupplierInvoiceDate.Name = "dtpSupplierInvoiceDate";
            this.dtpSupplierInvoiceDate.Size = new System.Drawing.Size(200, 21);
            this.dtpSupplierInvoiceDate.TabIndex = 13;
            this.dtpSupplierInvoiceDate.Visible = false;
            // 
            // btnAttachImage
            // 
            this.btnAttachImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAttachImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAttachImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttachImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttachImage.ForeColor = System.Drawing.Color.White;
            this.btnAttachImage.Location = new System.Drawing.Point(1076, 20);
            this.btnAttachImage.Name = "btnAttachImage";
            this.btnAttachImage.Size = new System.Drawing.Size(59, 24);
            this.btnAttachImage.TabIndex = 10;
            this.btnAttachImage.Text = "Attach Image";
            this.btnAttachImage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAttachImage.UseVisualStyleBackColor = false;
            this.btnAttachImage.Visible = false;
            this.btnAttachImage.Click += new System.EventHandler(this.btnAttachImage_Click);
            // 
            // grdItems
            // 
            this.grdItems.AllowUserToAddRows = false;
            this.grdItems.AllowUserToDeleteRows = false;
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.grdItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colStockItemId,
            this.colItemId,
            this.colItem,
            this.colUnit,
            this.colStores,
            this.colConvUnit,
            this.colAvailableQty,
            this.colQuantity,
            this.colBonusQty,
            this.colUnitCost,
            this.DiscountedPrice,
            this.colTotalCost,
            this.colRetailPrice,
            this.colNetProfit,
            this.colBatch,
            this.colExpiry,
            this.colBonusDiscount,
            this.colDiscount,
            this.colPercDiscType,
            this.colDiscType,
            this.colSalesTax,
            this.colPercSalesTaxType,
            this.colSalesTaxType,
            this.colWHT,
            this.colWHTType,
            this.colAdvanceWHT,
            this.colAdvanceWHTType,
            this.colNetValue,
            this.colGrossMargin,
            this.colConsumedQty,
            this.colDiscountValue,
            this.colSalesTaxValue,
            this.colIsRowUpdated,
            this.colEdit,
            this.colRemove});
            this.grdItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 196);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowHeadersWidth = 20;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1483, 500);
            this.grdItems.TabIndex = 23;
            this.grdItems.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdItems_CellBeginEdit);
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            this.grdItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellEndEdit);
            this.grdItems.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellEnter);
            this.grdItems.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdItems_CurrentCellDirtyStateChanged);
            this.grdItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdItems_DataError);
            this.grdItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdItems_EditingControlShowing);
            this.grdItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdItems_RowsAdded);
            this.grdItems.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdItems_RowsRemoved);
            this.grdItems.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdItems_UserDeletingRow);
            this.grdItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdItems_KeyDown);
            this.grdItems.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.grdItems_PreviewKeyDown);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            // 
            // colStockItemId
            // 
            this.colStockItemId.HeaderText = "StockItemId";
            this.colStockItemId.Name = "colStockItemId";
            this.colStockItemId.ReadOnly = true;
            this.colStockItemId.Visible = false;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "Item ID";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            this.colItemId.Visible = false;
            // 
            // colItem
            // 
            this.colItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItem.HeaderText = "Item";
            this.colItem.MinimumWidth = 140;
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colUnit
            // 
            this.colUnit.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colUnit.HeaderText = "Unit";
            this.colUnit.MinimumWidth = 50;
            this.colUnit.Name = "colUnit";
            this.colUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUnit.Width = 70;
            // 
            // colStores
            // 
            this.colStores.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colStores.HeaderText = "Store";
            this.colStores.Name = "colStores";
            this.colStores.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStores.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colConvUnit
            // 
            this.colConvUnit.HeaderText = "Conv. Unit";
            this.colConvUnit.Name = "colConvUnit";
            this.colConvUnit.ReadOnly = true;
            this.colConvUnit.Width = 40;
            // 
            // colAvailableQty
            // 
            this.colAvailableQty.HeaderText = "Available Qty.";
            this.colAvailableQty.Name = "colAvailableQty";
            this.colAvailableQty.ReadOnly = true;
            // 
            // colQuantity
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = "0";
            this.colQuantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colQuantity.Width = 50;
            // 
            // colBonusQty
            // 
            this.colBonusQty.HeaderText = "BonusQty";
            this.colBonusQty.MinimumWidth = 40;
            this.colBonusQty.Name = "colBonusQty";
            this.colBonusQty.Width = 50;
            // 
            // colUnitCost
            // 
            this.colUnitCost.HeaderText = "Unit Cost";
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.Width = 60;
            // 
            // DiscountedPrice
            // 
            this.DiscountedPrice.HeaderText = "Disc Price";
            this.DiscountedPrice.Name = "DiscountedPrice";
            this.DiscountedPrice.ReadOnly = true;
            // 
            // colTotalCost
            // 
            this.colTotalCost.HeaderText = "Total Cost";
            this.colTotalCost.Name = "colTotalCost";
            this.colTotalCost.ReadOnly = true;
            this.colTotalCost.Width = 50;
            // 
            // colRetailPrice
            // 
            this.colRetailPrice.HeaderText = "Retail Price";
            this.colRetailPrice.Name = "colRetailPrice";
            this.colRetailPrice.Width = 80;
            // 
            // colNetProfit
            // 
            this.colNetProfit.HeaderText = "Profit Margin%";
            this.colNetProfit.Name = "colNetProfit";
            this.colNetProfit.Width = 60;
            // 
            // colBatch
            // 
            this.colBatch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBatch.HeaderText = "Batch";
            this.colBatch.MinimumWidth = 60;
            this.colBatch.Name = "colBatch";
            // 
            // colExpiry
            // 
            this.colExpiry.HeaderText = "Expiry";
            this.colExpiry.MinimumWidth = 140;
            this.colExpiry.Name = "colExpiry";
            this.colExpiry.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colExpiry.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colExpiry.Width = 140;
            // 
            // colBonusDiscount
            // 
            this.colBonusDiscount.HeaderText = "Bonus Discount";
            this.colBonusDiscount.Name = "colBonusDiscount";
            this.colBonusDiscount.ReadOnly = true;
            this.colBonusDiscount.Width = 50;
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.Width = 50;
            // 
            // colPercDiscType
            // 
            this.colPercDiscType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colPercDiscType.HeaderText = "Discount Type";
            this.colPercDiscType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.colPercDiscType.Name = "colPercDiscType";
            this.colPercDiscType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPercDiscType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPercDiscType.Width = 70;
            // 
            // colDiscType
            // 
            this.colDiscType.HeaderText = "LumpSum Disc.";
            this.colDiscType.Items.AddRange(new object[] {
            "Lumpsum",
            "Per Item"});
            this.colDiscType.Name = "colDiscType";
            this.colDiscType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDiscType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colSalesTax
            // 
            this.colSalesTax.HeaderText = "Sales Tax";
            this.colSalesTax.Name = "colSalesTax";
            this.colSalesTax.Width = 50;
            // 
            // colPercSalesTaxType
            // 
            this.colPercSalesTaxType.HeaderText = "Sales Tax Type";
            this.colPercSalesTaxType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.colPercSalesTaxType.Name = "colPercSalesTaxType";
            this.colPercSalesTaxType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPercSalesTaxType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colSalesTaxType
            // 
            this.colSalesTaxType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colSalesTaxType.HeaderText = "Lumpsum Sales Tax Type";
            this.colSalesTaxType.Items.AddRange(new object[] {
            "Lumpsum",
            "Per Item"});
            this.colSalesTaxType.Name = "colSalesTaxType";
            this.colSalesTaxType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSalesTaxType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSalesTaxType.Width = 70;
            // 
            // colWHT
            // 
            // 
            // 
            // 
            this.colWHT.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colWHT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colWHT.HeaderText = "With Holding Tax";
            this.colWHT.Increment = 1D;
            this.colWHT.Name = "colWHT";
            this.colWHT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colWHTType
            // 
            this.colWHTType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colWHTType.HeaderText = "WHT Type";
            this.colWHTType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.colWHTType.Name = "colWHTType";
            this.colWHTType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colAdvanceWHT
            // 
            // 
            // 
            // 
            this.colAdvanceWHT.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colAdvanceWHT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colAdvanceWHT.HeaderText = "Advance Tax";
            this.colAdvanceWHT.Increment = 1D;
            this.colAdvanceWHT.Name = "colAdvanceWHT";
            this.colAdvanceWHT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colAdvanceWHTType
            // 
            this.colAdvanceWHTType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colAdvanceWHTType.HeaderText = "Advance Tax Type";
            this.colAdvanceWHTType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.colAdvanceWHTType.Name = "colAdvanceWHTType";
            this.colAdvanceWHTType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAdvanceWHTType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colNetValue
            // 
            this.colNetValue.HeaderText = "Net Value";
            this.colNetValue.Name = "colNetValue";
            this.colNetValue.ReadOnly = true;
            this.colNetValue.Width = 60;
            // 
            // colGrossMargin
            // 
            this.colGrossMargin.HeaderText = "Gross Margin";
            this.colGrossMargin.Name = "colGrossMargin";
            this.colGrossMargin.ReadOnly = true;
            // 
            // colConsumedQty
            // 
            this.colConsumedQty.HeaderText = "ConsumedQty";
            this.colConsumedQty.Name = "colConsumedQty";
            this.colConsumedQty.ReadOnly = true;
            this.colConsumedQty.Visible = false;
            // 
            // colDiscountValue
            // 
            this.colDiscountValue.HeaderText = "Discount Value";
            this.colDiscountValue.MinimumWidth = 40;
            this.colDiscountValue.Name = "colDiscountValue";
            this.colDiscountValue.ReadOnly = true;
            this.colDiscountValue.Width = 50;
            // 
            // colSalesTaxValue
            // 
            this.colSalesTaxValue.HeaderText = "Sales Tax Value";
            this.colSalesTaxValue.Name = "colSalesTaxValue";
            this.colSalesTaxValue.ReadOnly = true;
            this.colSalesTaxValue.Width = 60;
            // 
            // colIsRowUpdated
            // 
            this.colIsRowUpdated.HeaderText = "IsRowUpdated";
            this.colIsRowUpdated.Name = "colIsRowUpdated";
            this.colIsRowUpdated.ReadOnly = true;
            this.colIsRowUpdated.Visible = false;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Visible = false;
            this.colEdit.Width = 60;
            // 
            // colRemove
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.NullValue = null;
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRemove.HeaderText = "Remove";
            this.colRemove.MinimumWidth = 40;
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 55;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Red;
            this.ErrMessage.Location = new System.Drawing.Point(12, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            this.ErrMessage.Click += new System.EventHandler(this.ErrMessage_Click);
            // 
            // lblImagePath
            // 
            this.lblImagePath.AutoSize = true;
            this.lblImagePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImagePath.Location = new System.Drawing.Point(1148, 8);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(85, 13);
            this.lblImagePath.TabIndex = 15;
            this.lblImagePath.Text = "No File Choosen";
            this.lblImagePath.Visible = false;
            // 
            // lblRemoveFile
            // 
            this.lblRemoveFile.AutoSize = true;
            this.lblRemoveFile.Location = new System.Drawing.Point(1150, 23);
            this.lblRemoveFile.Name = "lblRemoveFile";
            this.lblRemoveFile.Size = new System.Drawing.Size(68, 14);
            this.lblRemoveFile.TabIndex = 11;
            this.lblRemoveFile.TabStop = true;
            this.lblRemoveFile.Text = "Remove File";
            this.lblRemoveFile.Visible = false;
            this.lblRemoveFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRemoveFile_LinkClicked);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(511, 41);
            this.label25.MaximumSize = new System.Drawing.Size(15, 16);
            this.label25.MinimumSize = new System.Drawing.Size(15, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(15, 16);
            this.label25.TabIndex = 45;
            this.label25.Text = "*";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label25.Visible = false;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(511, 76);
            this.label26.MaximumSize = new System.Drawing.Size(15, 16);
            this.label26.MinimumSize = new System.Drawing.Size(15, 16);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(15, 16);
            this.label26.TabIndex = 46;
            this.label26.Text = "*";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label26.Visible = false;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(852, 42);
            this.label28.MaximumSize = new System.Drawing.Size(15, 16);
            this.label28.MinimumSize = new System.Drawing.Size(15, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(15, 16);
            this.label28.TabIndex = 48;
            this.label28.Text = "*";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label28.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Red;
            this.label29.Location = new System.Drawing.Point(1214, 9);
            this.label29.MaximumSize = new System.Drawing.Size(15, 16);
            this.label29.MinimumSize = new System.Drawing.Size(15, 16);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(15, 16);
            this.label29.TabIndex = 49;
            this.label29.Text = "*";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label29.Visible = false;
            // 
            // btnSearchItems
            // 
            this.btnSearchItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchItems.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSearchItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchItems.ForeColor = System.Drawing.Color.Black;
            this.btnSearchItems.Location = new System.Drawing.Point(972, 128);
            this.btnSearchItems.Name = "btnSearchItems";
            this.hlpGrd.SetShowHelp(this.btnSearchItems, false);
            this.btnSearchItems.Size = new System.Drawing.Size(111, 62);
            this.btnSearchItems.TabIndex = 1;
            this.btnSearchItems.Text = "Show Product Detail";
            this.btnSearchItems.UseMnemonic = false;
            this.btnSearchItems.UseVisualStyleBackColor = false;
            this.btnSearchItems.Click += new System.EventHandler(this.btnSearchItems_Click);
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1478, 0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 30);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 100;
            this.pbInfo.TabStop = false;
            this.pbInfo.Visible = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // lblLoadingSuppliers
            // 
            this.lblLoadingSuppliers.AutoSize = true;
            this.lblLoadingSuppliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingSuppliers.Location = new System.Drawing.Point(158, 101);
            this.lblLoadingSuppliers.Name = "lblLoadingSuppliers";
            this.lblLoadingSuppliers.Size = new System.Drawing.Size(148, 20);
            this.lblLoadingSuppliers.TabIndex = 101;
            this.lblLoadingSuppliers.Text = "Loading Suppliers...";
            this.lblLoadingSuppliers.Visible = false;
            // 
            // lblClearExpiry
            // 
            this.lblClearExpiry.AutoSize = true;
            this.lblClearExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearExpiry.Location = new System.Drawing.Point(481, 63);
            this.lblClearExpiry.Name = "lblClearExpiry";
            this.lblClearExpiry.Size = new System.Drawing.Size(57, 13);
            this.lblClearExpiry.TabIndex = 16;
            this.lblClearExpiry.TabStop = true;
            this.lblClearExpiry.Text = "Clear Date";
            this.lblClearExpiry.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblClearExpiry_LinkClicked);
            // 
            // txtStockItemId
            // 
            this.txtStockItemId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStockItemId.Location = new System.Drawing.Point(182, 68);
            this.txtStockItemId.Name = "txtStockItemId";
            this.txtStockItemId.Size = new System.Drawing.Size(100, 20);
            this.txtStockItemId.TabIndex = 138;
            this.txtStockItemId.Text = "0";
            this.txtStockItemId.Visible = false;
            // 
            // numRetailPrice
            // 
            this.numRetailPrice.DecimalPlaces = 2;
            this.numRetailPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numRetailPrice.Location = new System.Drawing.Point(233, 38);
            this.numRetailPrice.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.numRetailPrice.Name = "numRetailPrice";
            this.numRetailPrice.Size = new System.Drawing.Size(73, 20);
            this.numRetailPrice.TabIndex = 13;
            this.numRetailPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numRetailPrice_KeyDown);
            // 
            // numSalesTax
            // 
            this.numSalesTax.DecimalPlaces = 2;
            this.numSalesTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSalesTax.Location = new System.Drawing.Point(659, 39);
            this.numSalesTax.Maximum = new decimal(new int[] {
            -1593835520,
            466537709,
            54210,
            0});
            this.numSalesTax.Name = "numSalesTax";
            this.numSalesTax.Size = new System.Drawing.Size(99, 20);
            this.numSalesTax.TabIndex = 19;
            this.numSalesTax.ValueChanged += new System.EventHandler(this.numSalesTax_ValueChanged);
            this.numSalesTax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numSalesTax_KeyDown);
            // 
            // numDiscount
            // 
            this.numDiscount.DecimalPlaces = 2;
            this.numDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numDiscount.Location = new System.Drawing.Point(554, 38);
            this.numDiscount.Maximum = new decimal(new int[] {
            -159383552,
            46653770,
            5421,
            0});
            this.numDiscount.Name = "numDiscount";
            this.numDiscount.Size = new System.Drawing.Size(99, 20);
            this.numDiscount.TabIndex = 17;
            this.numDiscount.ValueChanged += new System.EventHandler(this.numDiscount_ValueChanged);
            this.numDiscount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numDiscount_KeyDown);
            // 
            // numUnitCost
            // 
            this.numUnitCost.DecimalPlaces = 2;
            this.numUnitCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUnitCost.Location = new System.Drawing.Point(89, 38);
            this.numUnitCost.Maximum = new decimal(new int[] {
            -159383552,
            46653770,
            5421,
            0});
            this.numUnitCost.Name = "numUnitCost";
            this.numUnitCost.Size = new System.Drawing.Size(67, 20);
            this.numUnitCost.TabIndex = 11;
            this.numUnitCost.ValueChanged += new System.EventHandler(this.numUnitCost_ValueChanged);
            // 
            // numQty
            // 
            this.numQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQty.Location = new System.Drawing.Point(16, 38);
            this.numQty.Maximum = new decimal(new int[] {
            -1530494976,
            232830,
            0,
            0});
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(67, 20);
            this.numQty.TabIndex = 10;
            this.numQty.ValueChanged += new System.EventHandler(this.numQty_ValueChanged);
            this.numQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numQty_KeyDown);
            // 
            // ErrNetValue
            // 
            this.ErrNetValue.AutoSize = true;
            this.ErrNetValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrNetValue.ForeColor = System.Drawing.Color.Red;
            this.ErrNetValue.Location = new System.Drawing.Point(638, 87);
            this.ErrNetValue.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrNetValue.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrNetValue.Name = "ErrNetValue";
            this.ErrNetValue.Size = new System.Drawing.Size(15, 16);
            this.ErrNetValue.TabIndex = 137;
            this.ErrNetValue.Text = "*";
            this.ErrNetValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrNetValue.Visible = false;
            // 
            // ErrSalesTax
            // 
            this.ErrSalesTax.AutoSize = true;
            this.ErrSalesTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSalesTax.ForeColor = System.Drawing.Color.Red;
            this.ErrSalesTax.Location = new System.Drawing.Point(740, 20);
            this.ErrSalesTax.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSalesTax.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSalesTax.Name = "ErrSalesTax";
            this.ErrSalesTax.Size = new System.Drawing.Size(15, 16);
            this.ErrSalesTax.TabIndex = 136;
            this.ErrSalesTax.Text = "*";
            this.ErrSalesTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrSalesTax.Visible = false;
            // 
            // ErrDiscount
            // 
            this.ErrDiscount.AutoSize = true;
            this.ErrDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrDiscount.ForeColor = System.Drawing.Color.Red;
            this.ErrDiscount.Location = new System.Drawing.Point(635, 18);
            this.ErrDiscount.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrDiscount.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrDiscount.Name = "ErrDiscount";
            this.ErrDiscount.Size = new System.Drawing.Size(15, 16);
            this.ErrDiscount.TabIndex = 135;
            this.ErrDiscount.Text = "*";
            this.ErrDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrDiscount.Visible = false;
            // 
            // ErrExpiry
            // 
            this.ErrExpiry.AutoSize = true;
            this.ErrExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrExpiry.ForeColor = System.Drawing.Color.Red;
            this.ErrExpiry.Location = new System.Drawing.Point(524, 22);
            this.ErrExpiry.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrExpiry.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrExpiry.Name = "ErrExpiry";
            this.ErrExpiry.Size = new System.Drawing.Size(15, 16);
            this.ErrExpiry.TabIndex = 134;
            this.ErrExpiry.Text = "*";
            this.ErrExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrExpiry.Visible = false;
            // 
            // ErrBatchName
            // 
            this.ErrBatchName.AutoSize = true;
            this.ErrBatchName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrBatchName.ForeColor = System.Drawing.Color.Red;
            this.ErrBatchName.Location = new System.Drawing.Point(413, 22);
            this.ErrBatchName.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrBatchName.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrBatchName.Name = "ErrBatchName";
            this.ErrBatchName.Size = new System.Drawing.Size(15, 16);
            this.ErrBatchName.TabIndex = 133;
            this.ErrBatchName.Text = "*";
            this.ErrBatchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrBatchName.Visible = false;
            // 
            // ErrRetailPrice
            // 
            this.ErrRetailPrice.AutoSize = true;
            this.ErrRetailPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrRetailPrice.ForeColor = System.Drawing.Color.Red;
            this.ErrRetailPrice.Location = new System.Drawing.Point(291, 20);
            this.ErrRetailPrice.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrRetailPrice.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrRetailPrice.Name = "ErrRetailPrice";
            this.ErrRetailPrice.Size = new System.Drawing.Size(15, 16);
            this.ErrRetailPrice.TabIndex = 132;
            this.ErrRetailPrice.Text = "*";
            this.ErrRetailPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrRetailPrice.Visible = false;
            // 
            // ErrTotalCost
            // 
            this.ErrTotalCost.AutoSize = true;
            this.ErrTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrTotalCost.ForeColor = System.Drawing.Color.Red;
            this.ErrTotalCost.Location = new System.Drawing.Point(212, 21);
            this.ErrTotalCost.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrTotalCost.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrTotalCost.Name = "ErrTotalCost";
            this.ErrTotalCost.Size = new System.Drawing.Size(15, 16);
            this.ErrTotalCost.TabIndex = 131;
            this.ErrTotalCost.Text = "*";
            this.ErrTotalCost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrTotalCost.Visible = false;
            // 
            // ErrUnitCost
            // 
            this.ErrUnitCost.AutoSize = true;
            this.ErrUnitCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrUnitCost.ForeColor = System.Drawing.Color.Red;
            this.ErrUnitCost.Location = new System.Drawing.Point(134, 19);
            this.ErrUnitCost.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrUnitCost.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrUnitCost.Name = "ErrUnitCost";
            this.ErrUnitCost.Size = new System.Drawing.Size(15, 16);
            this.ErrUnitCost.TabIndex = 130;
            this.ErrUnitCost.Text = "*";
            this.ErrUnitCost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrUnitCost.Visible = false;
            // 
            // ErrQuantity
            // 
            this.ErrQuantity.AutoSize = true;
            this.ErrQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrQuantity.ForeColor = System.Drawing.Color.Red;
            this.ErrQuantity.Location = new System.Drawing.Point(57, 19);
            this.ErrQuantity.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(15, 16);
            this.ErrQuantity.TabIndex = 129;
            this.ErrQuantity.Text = "*";
            this.ErrQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrQuantity.Visible = false;
            // 
            // ErrSelectItem
            // 
            this.ErrSelectItem.AutoSize = true;
            this.ErrSelectItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectItem.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectItem.Location = new System.Drawing.Point(271, 132);
            this.ErrSelectItem.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.Name = "ErrSelectItem";
            this.ErrSelectItem.Size = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.TabIndex = 128;
            this.ErrSelectItem.Text = "*";
            this.ErrSelectItem.Visible = false;
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAddItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Location = new System.Drawing.Point(104, 27);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(78, 25);
            this.btnAddItem.TabIndex = 2;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Visible = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(551, 87);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 127;
            this.label14.Text = "Net Value";
            // 
            // txtNetValue
            // 
            this.txtNetValue.Enabled = false;
            this.txtNetValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNetValue.Location = new System.Drawing.Point(554, 105);
            this.txtNetValue.Name = "txtNetValue";
            this.txtNetValue.Size = new System.Drawing.Size(99, 20);
            this.txtNetValue.TabIndex = 120;
            this.txtNetValue.Text = "0";
            // 
            // cmbSalesTaxType
            // 
            this.cmbSalesTaxType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSalesTaxType.FormattingEnabled = true;
            this.cmbSalesTaxType.Items.AddRange(new object[] {
            "Lumpsum",
            "Per Item"});
            this.cmbSalesTaxType.Location = new System.Drawing.Point(659, 62);
            this.cmbSalesTaxType.Name = "cmbSalesTaxType";
            this.cmbSalesTaxType.Size = new System.Drawing.Size(99, 21);
            this.cmbSalesTaxType.TabIndex = 20;
            this.cmbSalesTaxType.SelectedIndexChanged += new System.EventHandler(this.cmbSalesTaxType_SelectedIndexChanged);
            this.cmbSalesTaxType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSalesTaxType_KeyDown);
            // 
            // cmbDiscountType
            // 
            this.cmbDiscountType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDiscountType.FormattingEnabled = true;
            this.cmbDiscountType.Items.AddRange(new object[] {
            "Lumpsum",
            "Per Item"});
            this.cmbDiscountType.Location = new System.Drawing.Point(554, 61);
            this.cmbDiscountType.Name = "cmbDiscountType";
            this.cmbDiscountType.Size = new System.Drawing.Size(99, 21);
            this.cmbDiscountType.TabIndex = 18;
            this.cmbDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbDiscountType_SelectedIndexChanged);
            this.cmbDiscountType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbDiscountType_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(656, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 126;
            this.label13.Text = "Sales Tax";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(434, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 125;
            this.label15.Text = "Expiry";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(551, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 13);
            this.label12.TabIndex = 124;
            this.label12.Text = "Discount";
            // 
            // dtpExpiry
            // 
            this.dtpExpiry.CustomFormat = "    ";
            this.dtpExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpExpiry.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpiry.Location = new System.Drawing.Point(437, 38);
            this.dtpExpiry.Name = "dtpExpiry";
            this.dtpExpiry.Size = new System.Drawing.Size(102, 20);
            this.dtpExpiry.TabIndex = 15;
            this.dtpExpiry.Tag = "0";
            this.dtpExpiry.ValueChanged += new System.EventHandler(this.dtpExpiry_ValueChanged);
            this.dtpExpiry.Enter += new System.EventHandler(this.dtpExpiry_Enter);
            this.dtpExpiry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpExpiry_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(312, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 121;
            this.label11.Text = "Batch";
            // 
            // txtBatch
            // 
            this.txtBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatch.Location = new System.Drawing.Point(315, 39);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(116, 20);
            this.txtBatch.TabIndex = 14;
            this.txtBatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBatch_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(233, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 117;
            this.label10.Text = "Retail Price";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(162, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 115;
            this.label9.Text = "Total Cost";
            // 
            // txtTotalCost
            // 
            this.txtTotalCost.Enabled = false;
            this.txtTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalCost.Location = new System.Drawing.Point(165, 38);
            this.txtTotalCost.Name = "txtTotalCost";
            this.txtTotalCost.Size = new System.Drawing.Size(62, 20);
            this.txtTotalCost.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(86, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 111;
            this.label8.Text = "Unit Cost";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 108;
            this.label7.Text = "Quantity";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(238, 20);
            this.label6.TabIndex = 105;
            this.label6.Text = "Search By Name, Generic Name";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(184, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 25);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // numConvUnit
            // 
            this.numConvUnit.Enabled = false;
            this.numConvUnit.Location = new System.Drawing.Point(14, 106);
            this.numConvUnit.Name = "numConvUnit";
            this.numConvUnit.Size = new System.Drawing.Size(152, 21);
            this.numConvUnit.TabIndex = 145;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(13, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 13);
            this.label18.TabIndex = 144;
            this.label18.Text = "Conversion Unit";
            // 
            // errConvUnit
            // 
            this.errConvUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errConvUnit.ForeColor = System.Drawing.Color.Red;
            this.errConvUnit.Location = new System.Drawing.Point(151, 91);
            this.errConvUnit.Name = "errConvUnit";
            this.errConvUnit.Size = new System.Drawing.Size(15, 11);
            this.errConvUnit.TabIndex = 146;
            this.errConvUnit.Text = "*";
            this.errConvUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errConvUnit.Visible = false;
            // 
            // btlLoadSupProds
            // 
            this.btlLoadSupProds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btlLoadSupProds.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btlLoadSupProds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btlLoadSupProds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btlLoadSupProds.ForeColor = System.Drawing.Color.White;
            this.btlLoadSupProds.Location = new System.Drawing.Point(1076, 23);
            this.btlLoadSupProds.Name = "btlLoadSupProds";
            this.btlLoadSupProds.Size = new System.Drawing.Size(122, 24);
            this.btlLoadSupProds.TabIndex = 8;
            this.btlLoadSupProds.Text = "All Items";
            this.btlLoadSupProds.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btlLoadSupProds.UseVisualStyleBackColor = false;
            this.btlLoadSupProds.Visible = false;
            this.btlLoadSupProds.Click += new System.EventHandler(this.btlLoadSupProds_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numQty);
            this.panel1.Controls.Add(this.errConvUnit);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.numConvUnit);
            this.panel1.Controls.Add(this.ErrUnitCost);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.ErrTotalCost);
            this.panel1.Controls.Add(this.txtNetValue);
            this.panel1.Controls.Add(this.ErrQuantity);
            this.panel1.Controls.Add(this.ErrRetailPrice);
            this.panel1.Controls.Add(this.cmbSalesTaxType);
            this.panel1.Controls.Add(this.ErrBatchName);
            this.panel1.Controls.Add(this.cmbDiscountType);
            this.panel1.Controls.Add(this.ErrExpiry);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.ErrDiscount);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.ErrSalesTax);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.ErrNetValue);
            this.panel1.Controls.Add(this.lblClearExpiry);
            this.panel1.Controls.Add(this.dtpExpiry);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.numUnitCost);
            this.panel1.Controls.Add(this.txtBatch);
            this.panel1.Controls.Add(this.txtStockItemId);
            this.panel1.Controls.Add(this.numDiscount);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtTotalCost);
            this.panel1.Controls.Add(this.numSalesTax);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.numRetailPrice);
            this.panel1.Location = new System.Drawing.Point(150, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 143);
            this.panel1.TabIndex = 147;
            this.panel1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.btnExpApply);
            this.panel2.Controls.Add(this.btnExpCancel);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Location = new System.Drawing.Point(844, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 79);
            this.panel2.TabIndex = 1008;
            this.panel2.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 21);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(15, 4);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 14);
            this.label19.TabIndex = 8;
            this.label19.Text = "Select Expiry";
            // 
            // btnExpApply
            // 
            this.btnExpApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnExpApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnExpApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpApply.ForeColor = System.Drawing.Color.White;
            this.btnExpApply.Location = new System.Drawing.Point(61, 48);
            this.btnExpApply.Name = "btnExpApply";
            this.btnExpApply.Size = new System.Drawing.Size(78, 25);
            this.btnExpApply.TabIndex = 22;
            this.btnExpApply.Text = "Apply";
            this.btnExpApply.UseVisualStyleBackColor = false;
            this.btnExpApply.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExpCancel
            // 
            this.btnExpCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnExpCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnExpCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpCancel.ForeColor = System.Drawing.Color.White;
            this.btnExpCancel.Location = new System.Drawing.Point(140, 48);
            this.btnExpCancel.Name = "btnExpCancel";
            this.btnExpCancel.Size = new System.Drawing.Size(78, 25);
            this.btnExpCancel.TabIndex = 22;
            this.btnExpCancel.Text = "Clear";
            this.btnExpCancel.UseVisualStyleBackColor = false;
            this.btnExpCancel.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // line1
            // 
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(508, 8);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1113, 10);
            this.line1.TabIndex = 1009;
            this.line1.Text = "line1";
            this.line1.Visible = false;
            // 
            // btnLoadMissedSales
            // 
            this.btnLoadMissedSales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadMissedSales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnLoadMissedSales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLoadMissedSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadMissedSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadMissedSales.ForeColor = System.Drawing.Color.White;
            this.btnLoadMissedSales.Location = new System.Drawing.Point(1352, 45);
            this.btnLoadMissedSales.Name = "btnLoadMissedSales";
            this.btnLoadMissedSales.Size = new System.Drawing.Size(111, 39);
            this.btnLoadMissedSales.TabIndex = 1010;
            this.btnLoadMissedSales.Text = "Load Missed Sales";
            this.btnLoadMissedSales.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadMissedSales.UseVisualStyleBackColor = false;
            this.btnLoadMissedSales.Visible = false;
            this.btnLoadMissedSales.Click += new System.EventHandler(this.btnLoadMissedSales_Click);
            // 
            // btnSupLowStockItems
            // 
            this.btnSupLowStockItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSupLowStockItems.BackColor = System.Drawing.Color.Transparent;
            this.btnSupLowStockItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSupLowStockItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSupLowStockItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSupLowStockItems.ForeColor = System.Drawing.Color.Black;
            this.btnSupLowStockItems.Location = new System.Drawing.Point(1289, 128);
            this.btnSupLowStockItems.Name = "btnSupLowStockItems";
            this.btnSupLowStockItems.Size = new System.Drawing.Size(201, 33);
            this.btnSupLowStockItems.TabIndex = 8;
            this.btnSupLowStockItems.Text = "Low Stock Items";
            this.btnSupLowStockItems.UseVisualStyleBackColor = false;
            this.btnSupLowStockItems.Click += new System.EventHandler(this.btnSupLowStockItems_Click);
            // 
            // btnLPrvSoldItems
            // 
            this.btnLPrvSoldItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLPrvSoldItems.BackColor = System.Drawing.Color.Transparent;
            this.btnLPrvSoldItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLPrvSoldItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLPrvSoldItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLPrvSoldItems.ForeColor = System.Drawing.Color.Black;
            this.btnLPrvSoldItems.Location = new System.Drawing.Point(1089, 128);
            this.btnLPrvSoldItems.Name = "btnLPrvSoldItems";
            this.btnLPrvSoldItems.Size = new System.Drawing.Size(194, 33);
            this.btnLPrvSoldItems.TabIndex = 1010;
            this.btnLPrvSoldItems.Text = "Load Previously Sold Items";
            this.btnLPrvSoldItems.UseVisualStyleBackColor = false;
            this.btnLPrvSoldItems.Click += new System.EventHandler(this.btnLPrvSoldItems_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbWithAdvanceHoldingTaxType);
            this.panel3.Controls.Add(this.label36);
            this.panel3.Controls.Add(this.txtAdvanceWithHoldingTax);
            this.panel3.Controls.Add(this.label35);
            this.panel3.Controls.Add(this.txttotalofbonusqty);
            this.panel3.Controls.Add(this.label34);
            this.panel3.Controls.Add(this.txttotalofqty);
            this.panel3.Controls.Add(this.label33);
            this.panel3.Controls.Add(this.label32);
            this.panel3.Controls.Add(this.txtTotalofCost);
            this.panel3.Controls.Add(this.label31);
            this.panel3.Controls.Add(this.txttotalofrate);
            this.panel3.Controls.Add(this.grpInvoiceHolding);
            this.panel3.Controls.Add(this.cmbWithHoldingTaxType);
            this.panel3.Controls.Add(this.label27);
            this.panel3.Controls.Add(this.txtWithHoldingTax);
            this.panel3.Controls.Add(this.label30);
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.txtTotalSaleTax);
            this.panel3.Controls.Add(this.cmbNetSaleTaxType);
            this.panel3.Controls.Add(this.label22);
            this.panel3.Controls.Add(this.txtNetSaleTax);
            this.panel3.Controls.Add(this.label23);
            this.panel3.Controls.Add(this.cmbNetDiscountType);
            this.panel3.Controls.Add(this.label21);
            this.panel3.Controls.Add(this.txtNetDiscount);
            this.panel3.Controls.Add(this.label17);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.txtTotalDiscount);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.txtGrandTotal);
            this.panel3.Controls.Add(this.btnClearAll);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 702);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1507, 103);
            this.panel3.TabIndex = 1026;
            // 
            // cmbWithAdvanceHoldingTaxType
            // 
            this.cmbWithAdvanceHoldingTaxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWithAdvanceHoldingTaxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbWithAdvanceHoldingTaxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWithAdvanceHoldingTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWithAdvanceHoldingTaxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWithAdvanceHoldingTaxType.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWithAdvanceHoldingTaxType.FormattingEnabled = true;
            this.cmbWithAdvanceHoldingTaxType.Location = new System.Drawing.Point(682, 66);
            this.cmbWithAdvanceHoldingTaxType.Name = "cmbWithAdvanceHoldingTaxType";
            this.cmbWithAdvanceHoldingTaxType.Size = new System.Drawing.Size(78, 21);
            this.cmbWithAdvanceHoldingTaxType.TabIndex = 1058;
            this.cmbWithAdvanceHoldingTaxType.SelectedIndexChanged += new System.EventHandler(this.cmbWithAdvanceHoldingTaxType_SelectedIndexChanged);
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(607, 69);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(74, 13);
            this.label36.TabIndex = 1057;
            this.label36.Text = "Adv.Tax Type";
            // 
            // txtAdvanceWithHoldingTax
            // 
            this.txtAdvanceWithHoldingTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdvanceWithHoldingTax.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdvanceWithHoldingTax.Location = new System.Drawing.Point(681, 29);
            this.txtAdvanceWithHoldingTax.Name = "txtAdvanceWithHoldingTax";
            this.txtAdvanceWithHoldingTax.Size = new System.Drawing.Size(78, 19);
            this.txtAdvanceWithHoldingTax.TabIndex = 1056;
            this.txtAdvanceWithHoldingTax.EnabledChanged += new System.EventHandler(this.txtAdvanceWithHoldingTax_EnabledChanged);
            this.txtAdvanceWithHoldingTax.TextChanged += new System.EventHandler(this.txtAdvanceWithHoldingTax_TextChanged);
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(608, 32);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(71, 13);
            this.label35.TabIndex = 1055;
            this.label35.Text = "Advance Tax";
            // 
            // txttotalofbonusqty
            // 
            this.txttotalofbonusqty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txttotalofbonusqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttotalofbonusqty.Enabled = false;
            this.txttotalofbonusqty.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotalofbonusqty.Location = new System.Drawing.Point(368, 67);
            this.txttotalofbonusqty.Name = "txttotalofbonusqty";
            this.txttotalofbonusqty.ReadOnly = true;
            this.txttotalofbonusqty.Size = new System.Drawing.Size(83, 19);
            this.txttotalofbonusqty.TabIndex = 1051;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(289, 26);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(50, 13);
            this.label34.TabIndex = 1054;
            this.label34.Text = "Total Qty";
            // 
            // txttotalofqty
            // 
            this.txttotalofqty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txttotalofqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttotalofqty.Enabled = false;
            this.txttotalofqty.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotalofqty.Location = new System.Drawing.Point(368, 25);
            this.txttotalofqty.Name = "txttotalofqty";
            this.txttotalofqty.ReadOnly = true;
            this.txttotalofqty.Size = new System.Drawing.Size(83, 19);
            this.txttotalofqty.TabIndex = 1053;
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(285, 71);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(83, 13);
            this.label33.TabIndex = 1052;
            this.label33.Text = "Total Bonus Qty";
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(106, 25);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(55, 13);
            this.label32.TabIndex = 1050;
            this.label32.Text = "Total Cost";
            // 
            // txtTotalofCost
            // 
            this.txtTotalofCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalofCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalofCost.Enabled = false;
            this.txtTotalofCost.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalofCost.Location = new System.Drawing.Point(165, 22);
            this.txtTotalofCost.Name = "txtTotalofCost";
            this.txtTotalofCost.ReadOnly = true;
            this.txtTotalofCost.Size = new System.Drawing.Size(118, 19);
            this.txtTotalofCost.TabIndex = 1049;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(104, 73);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(57, 13);
            this.label31.TabIndex = 1048;
            this.label31.Text = "Total Rate";
            // 
            // txttotalofrate
            // 
            this.txttotalofrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txttotalofrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttotalofrate.Enabled = false;
            this.txttotalofrate.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotalofrate.Location = new System.Drawing.Point(165, 69);
            this.txttotalofrate.Name = "txttotalofrate";
            this.txttotalofrate.ReadOnly = true;
            this.txttotalofrate.Size = new System.Drawing.Size(118, 19);
            this.txttotalofrate.TabIndex = 1047;
            // 
            // grpInvoiceHolding
            // 
            this.grpInvoiceHolding.Controls.Add(this.btnLastPage);
            this.grpInvoiceHolding.Controls.Add(this.btnFirstPage);
            this.grpInvoiceHolding.Controls.Add(this.btnPrevious);
            this.grpInvoiceHolding.Controls.Add(this.btnNext);
            this.grpInvoiceHolding.Controls.Add(this.btnCancelHold);
            this.grpInvoiceHolding.Controls.Add(this.btnHold);
            this.grpInvoiceHolding.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpInvoiceHolding.Location = new System.Drawing.Point(0, 0);
            this.grpInvoiceHolding.Name = "grpInvoiceHolding";
            this.grpInvoiceHolding.Size = new System.Drawing.Size(92, 103);
            this.grpInvoiceHolding.TabIndex = 1046;
            this.grpInvoiceHolding.TabStop = false;
            this.grpInvoiceHolding.Text = "Receipt Holding";
            this.grpInvoiceHolding.Visible = false;
            // 
            // btnLastPage
            // 
            this.btnLastPage.BackColor = System.Drawing.Color.White;
            this.btnLastPage.Enabled = false;
            this.btnLastPage.FlatAppearance.BorderSize = 0;
            this.btnLastPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastPage.ForeColor = System.Drawing.Color.Black;
            this.btnLastPage.Location = new System.Drawing.Point(113, 18);
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(35, 24);
            this.btnLastPage.TabIndex = 16;
            this.btnLastPage.Text = ">>";
            this.btnLastPage.UseVisualStyleBackColor = false;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.BackColor = System.Drawing.Color.White;
            this.btnFirstPage.Enabled = false;
            this.btnFirstPage.FlatAppearance.BorderSize = 0;
            this.btnFirstPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstPage.ForeColor = System.Drawing.Color.Black;
            this.btnFirstPage.Location = new System.Drawing.Point(14, 18);
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(33, 24);
            this.btnFirstPage.TabIndex = 18;
            this.btnFirstPage.Text = "<<";
            this.btnFirstPage.UseVisualStyleBackColor = false;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.White;
            this.btnPrevious.Enabled = false;
            this.btnPrevious.FlatAppearance.BorderSize = 0;
            this.btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.ForeColor = System.Drawing.Color.Black;
            this.btnPrevious.Location = new System.Drawing.Point(50, 18);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(27, 24);
            this.btnPrevious.TabIndex = 17;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.Enabled = false;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Location = new System.Drawing.Point(83, 18);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(27, 24);
            this.btnNext.TabIndex = 15;
            this.btnNext.Text = "> ";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancelHold
            // 
            this.btnCancelHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnCancelHold.FlatAppearance.BorderSize = 0;
            this.btnCancelHold.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnCancelHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelHold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelHold.ForeColor = System.Drawing.Color.White;
            this.btnCancelHold.Location = new System.Drawing.Point(14, 65);
            this.btnCancelHold.Name = "btnCancelHold";
            this.btnCancelHold.Size = new System.Drawing.Size(64, 32);
            this.btnCancelHold.TabIndex = 31;
            this.btnCancelHold.Text = "Cancel";
            this.btnCancelHold.UseVisualStyleBackColor = false;
            this.btnCancelHold.Click += new System.EventHandler(this.btnCancelHold_Click);
            // 
            // btnHold
            // 
            this.btnHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnHold.FlatAppearance.BorderSize = 0;
            this.btnHold.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHold.ForeColor = System.Drawing.Color.White;
            this.btnHold.Location = new System.Drawing.Point(-58, 65);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(64, 32);
            this.btnHold.TabIndex = 31;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = false;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // cmbWithHoldingTaxType
            // 
            this.cmbWithHoldingTaxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWithHoldingTaxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbWithHoldingTaxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWithHoldingTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWithHoldingTaxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWithHoldingTaxType.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWithHoldingTaxType.FormattingEnabled = true;
            this.cmbWithHoldingTaxType.Location = new System.Drawing.Point(516, 64);
            this.cmbWithHoldingTaxType.Name = "cmbWithHoldingTaxType";
            this.cmbWithHoldingTaxType.Size = new System.Drawing.Size(88, 21);
            this.cmbWithHoldingTaxType.TabIndex = 1045;
            this.cmbWithHoldingTaxType.SelectedIndexChanged += new System.EventHandler(this.cmbWithHoldingTaxType_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(454, 71);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(60, 13);
            this.label27.TabIndex = 1044;
            this.label27.Text = "WHT Type";
            // 
            // txtWithHoldingTax
            // 
            this.txtWithHoldingTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWithHoldingTax.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWithHoldingTax.Location = new System.Drawing.Point(516, 26);
            this.txtWithHoldingTax.Name = "txtWithHoldingTax";
            this.txtWithHoldingTax.Size = new System.Drawing.Size(88, 19);
            this.txtWithHoldingTax.TabIndex = 1043;
            this.txtWithHoldingTax.EnabledChanged += new System.EventHandler(this.txtWithHoldingTax_EnabledChanged);
            this.txtWithHoldingTax.TextChanged += new System.EventHandler(this.txtWithHoldingTax_TextChanged);
            this.txtWithHoldingTax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWithHoldingTax_KeyPress);
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(458, 30);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(33, 13);
            this.label30.TabIndex = 1042;
            this.label30.Text = "WHT";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(1180, 33);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(49, 13);
            this.label24.TabIndex = 1041;
            this.label24.Text = "TotalTax";
            // 
            // txtTotalSaleTax
            // 
            this.txtTotalSaleTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalSaleTax.Enabled = false;
            this.txtTotalSaleTax.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalSaleTax.Location = new System.Drawing.Point(1235, 31);
            this.txtTotalSaleTax.Name = "txtTotalSaleTax";
            this.txtTotalSaleTax.Size = new System.Drawing.Size(81, 19);
            this.txtTotalSaleTax.TabIndex = 1040;
            // 
            // cmbNetSaleTaxType
            // 
            this.cmbNetSaleTaxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNetSaleTaxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbNetSaleTaxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNetSaleTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetSaleTaxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbNetSaleTaxType.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbNetSaleTaxType.FormattingEnabled = true;
            this.cmbNetSaleTaxType.Location = new System.Drawing.Point(954, 65);
            this.cmbNetSaleTaxType.Name = "cmbNetSaleTaxType";
            this.cmbNetSaleTaxType.Size = new System.Drawing.Size(74, 21);
            this.cmbNetSaleTaxType.TabIndex = 1039;
            this.cmbNetSaleTaxType.SelectedIndexChanged += new System.EventHandler(this.cmbNetSaleTaxType_SelectedIndexChanged);
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(894, 69);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(60, 13);
            this.label22.TabIndex = 1038;
            this.label22.Text = "Sales Type";
            // 
            // txtNetSaleTax
            // 
            this.txtNetSaleTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetSaleTax.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNetSaleTax.Location = new System.Drawing.Point(954, 29);
            this.txtNetSaleTax.Name = "txtNetSaleTax";
            this.txtNetSaleTax.Size = new System.Drawing.Size(74, 19);
            this.txtNetSaleTax.TabIndex = 1037;
            this.txtNetSaleTax.TextChanged += new System.EventHandler(this.txtNetSaleTax_TextChanged);
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(894, 33);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 13);
            this.label23.TabIndex = 1036;
            this.label23.Text = "Sales Tax";
            // 
            // cmbNetDiscountType
            // 
            this.cmbNetDiscountType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNetDiscountType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbNetDiscountType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNetDiscountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetDiscountType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbNetDiscountType.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbNetDiscountType.FormattingEnabled = true;
            this.cmbNetDiscountType.Location = new System.Drawing.Point(826, 63);
            this.cmbNetDiscountType.Name = "cmbNetDiscountType";
            this.cmbNetDiscountType.Size = new System.Drawing.Size(65, 21);
            this.cmbNetDiscountType.TabIndex = 1035;
            this.cmbNetDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbNetDiscountType_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(764, 67);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(58, 13);
            this.label21.TabIndex = 1034;
            this.label21.Text = "Disc. Type";
            // 
            // txtNetDiscount
            // 
            this.txtNetDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetDiscount.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNetDiscount.Location = new System.Drawing.Point(826, 27);
            this.txtNetDiscount.Name = "txtNetDiscount";
            this.txtNetDiscount.Size = new System.Drawing.Size(65, 19);
            this.txtNetDiscount.TabIndex = 1033;
            this.txtNetDiscount.EnabledChanged += new System.EventHandler(this.txtNetDiscount_EnabledChanged);
            this.txtNetDiscount.TextChanged += new System.EventHandler(this.txtNetDiscount_TextChanged);
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(764, 31);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(31, 13);
            this.label17.TabIndex = 1032;
            this.label17.Text = "Disc.";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(1032, 33);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(52, 13);
            this.label20.TabIndex = 1031;
            this.label20.Text = "TotalDisc";
            // 
            // txtTotalDiscount
            // 
            this.txtTotalDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalDiscount.Enabled = false;
            this.txtTotalDiscount.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalDiscount.Location = new System.Drawing.Point(1098, 31);
            this.txtTotalDiscount.Name = "txtTotalDiscount";
            this.txtTotalDiscount.Size = new System.Drawing.Size(80, 19);
            this.txtTotalDiscount.TabIndex = 1030;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(1032, 70);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 13);
            this.label16.TabIndex = 1028;
            this.label16.Text = "GrandTotal";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.Font = new System.Drawing.Font("Microsoft Tai Le", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotal.Location = new System.Drawing.Point(1098, 67);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.Size = new System.Drawing.Size(218, 19);
            this.txtGrandTotal.TabIndex = 1029;
            this.txtGrandTotal.Text = "0";
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAll.ForeColor = System.Drawing.Color.Black;
            this.btnClearAll.Location = new System.Drawing.Point(1322, 44);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(173, 38);
            this.btnClearAll.TabIndex = 1027;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(1322, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(173, 38);
            this.btnSave.TabIndex = 1026;
            this.btnSave.Text = "Save All";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbouom
            // 
            this.cbouom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbouom.FormattingEnabled = true;
            this.cbouom.Items.AddRange(new object[] {
            "Pack",
            "Units"});
            this.cbouom.Location = new System.Drawing.Point(1089, 170);
            this.cbouom.Name = "cbouom";
            this.cbouom.Size = new System.Drawing.Size(194, 22);
            this.cbouom.TabIndex = 1027;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(1289, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(201, 25);
            this.button1.TabIndex = 1028;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Unit Cost";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Total Cost";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 140;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Retail Price";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 140;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "1";
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn5.HeaderText = "Batch";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = "0";
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn6.HeaderText = "Expiry";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = "0";
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn7.HeaderText = "Discount Type";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn7.Width = 80;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "Discount";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn9.HeaderText = "Sales Tax Type";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.NullValue = null;
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn10.HeaderText = "Sales Tax";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 50;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn12.HeaderText = "Sales Tax";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn13
            // 
            dataGridViewCellStyle7.NullValue = null;
            this.dataGridViewTextBoxColumn13.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn13.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn13.Width = 50;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn14.HeaderText = "NET VALUE";
            this.dataGridViewTextBoxColumn14.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.HeaderText = "Expiry";
            this.calendarColumn1.MinimumWidth = 140;
            this.calendarColumn1.Name = "calendarColumn1";
            this.calendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.calendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.calendarColumn1.Width = 140;
            // 
            // dataGridViewTextBoxColumn15
            // 
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridViewTextBoxColumn15.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn15.HeaderText = "DISCOUNT TYPE";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn15.Visible = false;
            this.dataGridViewTextBoxColumn15.Width = 50;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "SALES TAX TYPE";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Visible = false;
            this.dataGridViewTextBoxColumn16.Width = 70;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.HeaderText = "ConsumedQty";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            this.dataGridViewTextBoxColumn17.Width = 60;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.HeaderText = "Discount Value";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Visible = false;
            this.dataGridViewTextBoxColumn18.Width = 50;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.HeaderText = "Sales Tax Value";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Visible = false;
            this.dataGridViewTextBoxColumn19.Width = 60;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.HeaderText = "IsRowUpdated";
            this.dataGridViewTextBoxColumn20.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            this.dataGridViewTextBoxColumn20.Visible = false;
            this.dataGridViewTextBoxColumn20.Width = 50;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.HeaderText = "Item Id";
            this.dataGridViewTextBoxColumn21.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            this.dataGridViewTextBoxColumn21.Visible = false;
            this.dataGridViewTextBoxColumn21.Width = 60;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn22.HeaderText = "Item";
            this.dataGridViewTextBoxColumn22.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            this.dataGridViewTextBoxColumn22.Visible = false;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.HeaderText = "IsRowUpdated";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            this.dataGridViewTextBoxColumn23.Visible = false;
            this.dataGridViewTextBoxColumn23.Width = 60;
            // 
            // uC_SearchItems1
            // 
            this.uC_SearchItems1.BackColor = System.Drawing.Color.White;
            this.uC_SearchItems1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uC_SearchItems1.Location = new System.Drawing.Point(18, 151);
            this.uC_SearchItems1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uC_SearchItems1.Name = "uC_SearchItems1";
            this.uC_SearchItems1.PageNo = 0;
            this.uC_SearchItems1.SelectedItemId = 0;
            this.uC_SearchItems1.SelectedItemName = null;
            this.uC_SearchItems1.Size = new System.Drawing.Size(487, 30);
            this.uC_SearchItems1.StoreId = 0;
            this.uC_SearchItems1.TabIndex = 0;
            this.uC_SearchItems1.Load += new System.EventHandler(this.uC_SearchItems1_Load);
            this.uC_SearchItems1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uC_SearchItems1_KeyDown);
            this.uC_SearchItems1.Leave += new System.EventHandler(this.uC_SearchItems1_Leave);
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.HeaderText = "IsRowUpdated";
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.ReadOnly = true;
            this.dataGridViewTextBoxColumn24.Visible = false;
            // 
            // frmAddStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1507, 805);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbouom);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnLPrvSoldItems);
            this.Controls.Add(this.btnLoadMissedSales);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnSearchItems);
            this.Controls.Add(this.uC_SearchItems1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblLoadingSuppliers);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.lblRemoveFile);
            this.Controls.Add(this.lblImagePath);
            this.Controls.Add(this.btnAttachImage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpSupplierInvoiceDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSupLowStockItems);
            this.Controls.Add(this.btlLoadSupProds);
            this.Controls.Add(this.btnAddSupplier);
            this.Controls.Add(this.txtSupplierInvoiceNo);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.cmbSuppliers);
            this.Controls.Add(this.txtDocumentNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrSelectItem);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "frmAddStock";
            this.Text = "Add New Stock";
            this.Load += new System.EventHandler(this.frmAddStock_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddStock_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetailPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grpInvoiceHolding.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDocumentNo;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSupplierInvoiceNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpSupplierInvoiceDate;
        private System.Windows.Forms.Button btnAttachImage;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.Label lblImagePath;
        private System.Windows.Forms.LinkLabel lblRemoveFile;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.HelpProvider hlpGrd;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lblLoadingSuppliers;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private UC_SearchItems uC_SearchItems1;
        private System.Windows.Forms.LinkLabel lblClearExpiry;
        private System.Windows.Forms.TextBox txtStockItemId;
        private System.Windows.Forms.NumericUpDown numRetailPrice;
        private System.Windows.Forms.NumericUpDown numSalesTax;
        private System.Windows.Forms.NumericUpDown numDiscount;
        private System.Windows.Forms.NumericUpDown numUnitCost;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Label ErrNetValue;
        private System.Windows.Forms.Label ErrSalesTax;
        private System.Windows.Forms.Label ErrDiscount;
        private System.Windows.Forms.Label ErrExpiry;
        private System.Windows.Forms.Label ErrBatchName;
        private System.Windows.Forms.Label ErrRetailPrice;
        private System.Windows.Forms.Label ErrTotalCost;
        private System.Windows.Forms.Label ErrUnitCost;
        private System.Windows.Forms.Label ErrQuantity;
        private System.Windows.Forms.Label ErrSelectItem;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtNetValue;
        private System.Windows.Forms.ComboBox cmbSalesTaxType;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTotalCost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown numConvUnit;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label errConvUnit;
        private System.Windows.Forms.Button btlLoadSupProds;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearchItems;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnExpApply;
        private System.Windows.Forms.Button btnExpCancel;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Button btnLoadMissedSales;
        private System.Windows.Forms.Button btnSupLowStockItems;
        private System.Windows.Forms.Button btnLPrvSoldItems;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtTotalSaleTax;
        private System.Windows.Forms.ComboBox cmbNetSaleTaxType;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtNetSaleTax;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmbNetDiscountType;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtNetDiscount;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtTotalDiscount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtGrandTotal;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbWithHoldingTaxType;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtWithHoldingTax;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.GroupBox grpInvoiceHolding;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancelHold;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txttotalofqty;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txttotalofbonusqty;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtTotalofCost;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txttotalofrate;
        private System.Windows.Forms.ComboBox cmbWithAdvanceHoldingTaxType;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtAdvanceWithHoldingTax;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStores;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailableQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBonusQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountedPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private CustomCode.CalendarColumn colExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBonusDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPercDiscType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colDiscType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTax;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPercSalesTaxType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSalesTaxType;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colWHT;
        private System.Windows.Forms.DataGridViewComboBoxColumn colWHTType;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colAdvanceWHT;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAdvanceWHTType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrossMargin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsRowUpdated;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.ComboBox cbouom;
        private System.Windows.Forms.Button button1;
        private CustomCode.CalendarColumn calendarColumn1;
    }
}