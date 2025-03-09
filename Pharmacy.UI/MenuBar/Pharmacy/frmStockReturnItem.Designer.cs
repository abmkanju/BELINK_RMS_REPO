namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmStockReturnItem
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtDocumentNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.hlpGrd = new System.Windows.Forms.HelpProvider();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtTotalDiscount = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtTotalSaleTax = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockIReturnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvailableQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReturnQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReturnAvailableQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercDiscType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercSalesTaxType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesTaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbStockNo = new System.Windows.Forms.ComboBox();
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
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboSelectBatch = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.uC_SearchItems1 = new UC_SearchItems();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDocumentNo
            // 
            this.txtDocumentNo.Enabled = false;
            this.txtDocumentNo.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.txtDocumentNo.Location = new System.Drawing.Point(204, 20);
            this.txtDocumentNo.Name = "txtDocumentNo";
            this.txtDocumentNo.Size = new System.Drawing.Size(236, 28);
            this.txtDocumentNo.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.label3.Location = new System.Drawing.Point(484, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Stock Document #";
            // 
            // line1
            // 
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(15, 155);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1230, 10);
            this.line1.TabIndex = 1009;
            this.line1.Text = "line1";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(1008, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(237, 40);
            this.btnSave.TabIndex = 1026;
            this.btnSave.Text = "Save All";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotal.Location = new System.Drawing.Point(757, 16);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.Size = new System.Drawing.Size(232, 26);
            this.txtGrandTotal.TabIndex = 1029;
            this.txtGrandTotal.Text = "0";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(662, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 20);
            this.label16.TabIndex = 1028;
            this.label16.Text = "GrandTotal";
            // 
            // txtTotalDiscount
            // 
            this.txtTotalDiscount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTotalDiscount.Enabled = false;
            this.txtTotalDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalDiscount.Location = new System.Drawing.Point(151, 17);
            this.txtTotalDiscount.Name = "txtTotalDiscount";
            this.txtTotalDiscount.Size = new System.Drawing.Size(181, 26);
            this.txtTotalDiscount.TabIndex = 1030;
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(38, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(107, 20);
            this.label20.TabIndex = 1031;
            this.label20.Text = "TotalDiscount";
            // 
            // txtTotalSaleTax
            // 
            this.txtTotalSaleTax.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTotalSaleTax.Enabled = false;
            this.txtTotalSaleTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalSaleTax.Location = new System.Drawing.Point(420, 17);
            this.txtTotalSaleTax.Name = "txtTotalSaleTax";
            this.txtTotalSaleTax.Size = new System.Drawing.Size(232, 26);
            this.txtTotalSaleTax.TabIndex = 1040;
            // 
            // label24
            // 
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(338, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(69, 20);
            this.label24.TabIndex = 1041;
            this.label24.Text = "TotalTax";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.txtTotalSaleTax);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.txtTotalDiscount);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.txtGrandTotal);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Location = new System.Drawing.Point(0, 558);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1262, 61);
            this.panel3.TabIndex = 1026;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.label1.Location = new System.Drawing.Point(28, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 22);
            this.label1.TabIndex = 1027;
            this.label1.Text = "Return Document #";
            // 
            // grdItems
            // 
            this.grdItems.AllowUserToAddRows = false;
            this.grdItems.AllowUserToDeleteRows = false;
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.BackgroundColor = System.Drawing.Color.White;
            this.grdItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colStockIReturnId,
            this.colItemId,
            this.colBatchId,
            this.colItem,
            this.colUnit,
            this.colConvUnit,
            this.colAvailableQty,
            this.colReturnQuantity,
            this.colReturnAvailableQty,
            this.colBatchName,
            this.colUnitCost,
            this.colDiscount,
            this.colPercDiscType,
            this.colSalesTax,
            this.colPercSalesTaxType,
            this.colNetValue,
            this.colDiscountValue,
            this.colSalesTaxValue});
            this.grdItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(7, 106);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowHeadersWidth = 20;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1238, 436);
            this.grdItems.TabIndex = 1028;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            this.grdItems.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellEnter);
            this.grdItems.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdItems_CurrentCellDirtyStateChanged);
            this.grdItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdItems_DataError);
            this.grdItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdItems_EditingControlShowing);
            this.grdItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdItems_RowsAdded);
            this.grdItems.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdItems_RowsRemoved);
            this.grdItems.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdItems_UserDeletingRow);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            // 
            // colStockIReturnId
            // 
            this.colStockIReturnId.HeaderText = "StockItemId";
            this.colStockIReturnId.Name = "colStockIReturnId";
            this.colStockIReturnId.ReadOnly = true;
            this.colStockIReturnId.Visible = false;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "Item Id";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            this.colItemId.Visible = false;
            // 
            // colBatchId
            // 
            this.colBatchId.HeaderText = "Batch Id";
            this.colBatchId.Name = "colBatchId";
            this.colBatchId.ReadOnly = true;
            this.colBatchId.Visible = false;
            // 
            // colItem
            // 
            this.colItem.HeaderText = "Item";
            this.colItem.MinimumWidth = 140;
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItem.Width = 300;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colConvUnit
            // 
            this.colConvUnit.HeaderText = "Conv. Unit";
            this.colConvUnit.Name = "colConvUnit";
            this.colConvUnit.ReadOnly = true;
            // 
            // colAvailableQty
            // 
            this.colAvailableQty.HeaderText = "Available Qty";
            this.colAvailableQty.Name = "colAvailableQty";
            this.colAvailableQty.ReadOnly = true;
            // 
            // colReturnQuantity
            // 
            this.colReturnQuantity.HeaderText = "Return Quantity";
            this.colReturnQuantity.Name = "colReturnQuantity";
            this.colReturnQuantity.Width = 105;
            // 
            // colReturnAvailableQty
            // 
            this.colReturnAvailableQty.HeaderText = "Return Available Qty";
            this.colReturnAvailableQty.Name = "colReturnAvailableQty";
            this.colReturnAvailableQty.ReadOnly = true;
            this.colReturnAvailableQty.Visible = false;
            this.colReturnAvailableQty.Width = 150;
            // 
            // colBatchName
            // 
            this.colBatchName.HeaderText = "Batch Name";
            this.colBatchName.Name = "colBatchName";
            this.colBatchName.ReadOnly = true;
            this.colBatchName.Width = 150;
            // 
            // colUnitCost
            // 
            this.colUnitCost.HeaderText = "Unit Cost";
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.ReadOnly = true;
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.Width = 70;
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
            // colNetValue
            // 
            this.colNetValue.HeaderText = "Net Value";
            this.colNetValue.Name = "colNetValue";
            this.colNetValue.ReadOnly = true;
            this.colNetValue.Width = 60;
            // 
            // colDiscountValue
            // 
            this.colDiscountValue.HeaderText = "Discount Value";
            this.colDiscountValue.Name = "colDiscountValue";
            // 
            // colSalesTaxValue
            // 
            this.colSalesTaxValue.HeaderText = "Sale Tax Value";
            this.colSalesTaxValue.Name = "colSalesTaxValue";
            // 
            // cmbStockNo
            // 
            this.cmbStockNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStockNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStockNo.DropDownWidth = 350;
            this.cmbStockNo.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.cmbStockNo.FormattingEnabled = true;
            this.cmbStockNo.Location = new System.Drawing.Point(643, 25);
            this.cmbStockNo.Name = "cmbStockNo";
            this.cmbStockNo.Size = new System.Drawing.Size(203, 29);
            this.cmbStockNo.TabIndex = 1029;
            this.cmbStockNo.SelectedIndexChanged += new System.EventHandler(this.cmbStockNo_SelectedIndexChanged);
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
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Retail Price";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = "1";
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn5.HeaderText = "Batch";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = "0";
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn6.HeaderText = "Expiry";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Discount Type";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 80;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "Discount";
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
            dataGridViewCellStyle7.NullValue = null;
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle7;
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
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridViewTextBoxColumn13.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn13.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn13.Width = 50;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "NET VALUE";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 70;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "DISCOUNT TYPE";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
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
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.HeaderText = "Sales Tax Value";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Visible = false;
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
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            this.dataGridViewTextBoxColumn21.Width = 60;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn22.HeaderText = "Item";
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
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1005, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 14);
            this.label2.TabIndex = 1031;
            this.label2.Text = "Supplier Name #";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1006, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 14);
            this.label4.TabIndex = 1030;
            this.label4.Text = "Invoice #";
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Location = new System.Drawing.Point(1064, 12);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(26, 14);
            this.lblInvoiceNo.TabIndex = 1032;
            this.lblInvoiceNo.Text = "N/A";
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Location = new System.Drawing.Point(1100, 36);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(26, 14);
            this.lblSupplierName.TabIndex = 1033;
            this.lblSupplierName.Text = "N/A";
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddItem.ForeColor = System.Drawing.Color.Black;
            this.btnAddItem.Location = new System.Drawing.Point(855, 64);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(102, 30);
            this.btnAddItem.TabIndex = 1035;
            this.btnAddItem.Text = "Add";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1164, 72);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(54, 23);
            this.btnClear.TabIndex = 1036;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btn_Clear);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.label5.Location = new System.Drawing.Point(56, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 22);
            this.label5.TabIndex = 1037;
            this.label5.Text = "Select Product";
            // 
            // comboSelectBatch
            // 
            this.comboSelectBatch.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.comboSelectBatch.FormattingEnabled = true;
            this.comboSelectBatch.Location = new System.Drawing.Point(643, 65);
            this.comboSelectBatch.Name = "comboSelectBatch";
            this.comboSelectBatch.Size = new System.Drawing.Size(203, 29);
            this.comboSelectBatch.TabIndex = 1039;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.label6.Location = new System.Drawing.Point(532, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 22);
            this.label6.TabIndex = 1040;
            this.label6.Text = "Select Batch";
            // 
            // uC_SearchItems1
            // 
            this.uC_SearchItems1.BackColor = System.Drawing.Color.White;
            this.uC_SearchItems1.Font = new System.Drawing.Font("Microsoft Tai Le", 12.25F);
            this.uC_SearchItems1.Location = new System.Drawing.Point(200, 64);
            this.uC_SearchItems1.Name = "uC_SearchItems1";
            this.uC_SearchItems1.PageNo = 0;
            this.uC_SearchItems1.SelectedItemId = 0;
            this.uC_SearchItems1.SelectedItemName = null;
            this.uC_SearchItems1.Size = new System.Drawing.Size(240, 31);
            this.uC_SearchItems1.StoreId = 0;
            this.uC_SearchItems1.TabIndex = 1041;
            // 
            // frmStockReturnItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1262, 619);
            this.Controls.Add(this.uC_SearchItems1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboSelectBatch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.lblInvoiceNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStockNo);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDocumentNo);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStockReturnItem";
            this.Text = "Add New Stock";
            this.Load += new System.EventHandler(this.frmStockReturnItem_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDocumentNo;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.HelpProvider hlpGrd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtGrandTotal;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtTotalDiscount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtTotalSaleTax;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.ComboBox cmbStockNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboSelectBatch;
        private System.Windows.Forms.Label label6;
        private UC_SearchItems uC_SearchItems1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockIReturnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailableQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReturnQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReturnAvailableQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPercDiscType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTax;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPercSalesTaxType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTaxValue;
    }
}