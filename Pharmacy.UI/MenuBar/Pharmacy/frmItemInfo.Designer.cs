namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmItemInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemInfo));
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.grdStocks = new System.Windows.Forms.DataGridView();
            this.colBatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuppliers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvailableQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpiry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdAdjustments = new System.Windows.Forms.DataGridView();
            this.colAdjBatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjCreatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdConsumptions = new System.Windows.Forms.DataGridView();
            this.colCBatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStoreCon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumptionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colconsumptionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGenBC = new System.Windows.Forms.Button();
            this.btnPrintBC = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblReOrderLvl = new System.Windows.Forms.Label();
            this.lblStockingUnit = new System.Windows.Forms.Label();
            this.lblbcd = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblCtg = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblRL = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSu = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddStoc = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdStocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAdjustments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdConsumptions)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Location = new System.Drawing.Point(41, 17);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(16, 14);
            this.lblItemName.TabIndex = 0;
            this.lblItemName.Text = "...";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(3, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // grdStocks
            // 
            this.grdStocks.AllowUserToAddRows = false;
            this.grdStocks.AllowUserToDeleteRows = false;
            this.grdStocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdStocks.BackgroundColor = System.Drawing.Color.White;
            this.grdStocks.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdStocks.ColumnHeadersHeight = 35;
            this.grdStocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBatchNo,
            this.colSuppliers,
            this.colStore,
            this.colTotalQty,
            this.colAvailableQty,
            this.colExpiry,
            this.colCreatedAt});
            this.grdStocks.EnableHeadersVisualStyles = false;
            this.grdStocks.Location = new System.Drawing.Point(3, 54);
            this.grdStocks.Name = "grdStocks";
            this.grdStocks.RowHeadersVisible = false;
            this.grdStocks.RowTemplate.Height = 35;
            this.grdStocks.Size = new System.Drawing.Size(1164, 314);
            this.grdStocks.TabIndex = 0;
            // 
            // colBatchNo
            // 
            this.colBatchNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBatchNo.HeaderText = "Batch No.";
            this.colBatchNo.Name = "colBatchNo";
            this.colBatchNo.ReadOnly = true;
            // 
            // colSuppliers
            // 
            this.colSuppliers.HeaderText = "Suppliers";
            this.colSuppliers.Name = "colSuppliers";
            this.colSuppliers.ReadOnly = true;
            this.colSuppliers.Width = 420;
            // 
            // colStore
            // 
            this.colStore.HeaderText = "Store";
            this.colStore.Name = "colStore";
            // 
            // colTotalQty
            // 
            this.colTotalQty.HeaderText = "Total Qty.";
            this.colTotalQty.Name = "colTotalQty";
            this.colTotalQty.ReadOnly = true;
            this.colTotalQty.Width = 70;
            // 
            // colAvailableQty
            // 
            this.colAvailableQty.HeaderText = "Available Qty.";
            this.colAvailableQty.Name = "colAvailableQty";
            this.colAvailableQty.ReadOnly = true;
            this.colAvailableQty.Width = 70;
            // 
            // colExpiry
            // 
            this.colExpiry.HeaderText = "Expiry";
            this.colExpiry.Name = "colExpiry";
            this.colExpiry.ReadOnly = true;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.HeaderText = "Created At";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            // 
            // grdAdjustments
            // 
            this.grdAdjustments.AllowUserToAddRows = false;
            this.grdAdjustments.AllowUserToDeleteRows = false;
            this.grdAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAdjustments.BackgroundColor = System.Drawing.Color.White;
            this.grdAdjustments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdAdjustments.ColumnHeadersHeight = 35;
            this.grdAdjustments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAdjBatchName,
            this.colAdjQuantity,
            this.colAdjReason,
            this.colAdjCreatedDate});
            this.grdAdjustments.EnableHeadersVisualStyles = false;
            this.grdAdjustments.Location = new System.Drawing.Point(3, 831);
            this.grdAdjustments.Name = "grdAdjustments";
            this.grdAdjustments.RowHeadersVisible = false;
            this.grdAdjustments.RowTemplate.Height = 35;
            this.grdAdjustments.Size = new System.Drawing.Size(1164, 313);
            this.grdAdjustments.TabIndex = 4;
            // 
            // colAdjBatchName
            // 
            this.colAdjBatchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAdjBatchName.HeaderText = "Batch Name";
            this.colAdjBatchName.Name = "colAdjBatchName";
            // 
            // colAdjQuantity
            // 
            this.colAdjQuantity.HeaderText = "Quantity";
            this.colAdjQuantity.Name = "colAdjQuantity";
            // 
            // colAdjReason
            // 
            this.colAdjReason.HeaderText = "Reason";
            this.colAdjReason.Name = "colAdjReason";
            this.colAdjReason.Width = 200;
            // 
            // colAdjCreatedDate
            // 
            this.colAdjCreatedDate.HeaderText = "Created Date";
            this.colAdjCreatedDate.Name = "colAdjCreatedDate";
            this.colAdjCreatedDate.Width = 200;
            // 
            // grdConsumptions
            // 
            this.grdConsumptions.AllowUserToAddRows = false;
            this.grdConsumptions.AllowUserToDeleteRows = false;
            this.grdConsumptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdConsumptions.BackgroundColor = System.Drawing.Color.White;
            this.grdConsumptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdConsumptions.ColumnHeadersHeight = 35;
            this.grdConsumptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCBatchNo,
            this.colStoreCon,
            this.colConsumedQty,
            this.colConsumptionType,
            this.colconsumptionDate});
            this.grdConsumptions.EnableHeadersVisualStyles = false;
            this.grdConsumptions.Location = new System.Drawing.Point(3, 454);
            this.grdConsumptions.Name = "grdConsumptions";
            this.grdConsumptions.RowHeadersVisible = false;
            this.grdConsumptions.RowTemplate.Height = 35;
            this.grdConsumptions.Size = new System.Drawing.Size(1164, 314);
            this.grdConsumptions.TabIndex = 1;
            // 
            // colCBatchNo
            // 
            this.colCBatchNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCBatchNo.HeaderText = "Batch No.";
            this.colCBatchNo.Name = "colCBatchNo";
            this.colCBatchNo.ReadOnly = true;
            // 
            // colStoreCon
            // 
            this.colStoreCon.HeaderText = "Store";
            this.colStoreCon.Name = "colStoreCon";
            // 
            // colConsumedQty
            // 
            this.colConsumedQty.HeaderText = "Consumed Qty.";
            this.colConsumedQty.Name = "colConsumedQty";
            this.colConsumedQty.ReadOnly = true;
            this.colConsumedQty.Width = 70;
            // 
            // colConsumptionType
            // 
            this.colConsumptionType.HeaderText = "Consumption Type";
            this.colConsumptionType.Name = "colConsumptionType";
            this.colConsumptionType.ReadOnly = true;
            // 
            // colconsumptionDate
            // 
            this.colconsumptionDate.HeaderText = "Consumption Date";
            this.colconsumptionDate.Name = "colconsumptionDate";
            this.colconsumptionDate.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new System.Drawing.Point(195, 32767);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 37);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 28);
            this.label6.TabIndex = 0;
            this.label6.Text = "Item Info";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(711, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(126, 36);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnGenBC
            // 
            this.btnGenBC.Location = new System.Drawing.Point(269, 12);
            this.btnGenBC.Name = "btnGenBC";
            this.btnGenBC.Size = new System.Drawing.Size(146, 36);
            this.btnGenBC.TabIndex = 9;
            this.btnGenBC.Text = "Generate Barcode";
            this.btnGenBC.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnGenBC.UseVisualStyleBackColor = true;
            this.btnGenBC.Click += new System.EventHandler(this.btnGenBC_Click);
            // 
            // btnPrintBC
            // 
            this.btnPrintBC.Location = new System.Drawing.Point(432, 12);
            this.btnPrintBC.Name = "btnPrintBC";
            this.btnPrintBC.Size = new System.Drawing.Size(126, 36);
            this.btnPrintBC.TabIndex = 9;
            this.btnPrintBC.Text = "Print Barcode";
            this.btnPrintBC.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnPrintBC.UseVisualStyleBackColor = true;
            this.btnPrintBC.Click += new System.EventHandler(this.btnPrintBC_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel4.Controls.Add(this.lblBarcode);
            this.panel4.Controls.Add(this.lblCategory);
            this.panel4.Controls.Add(this.lblReOrderLvl);
            this.panel4.Controls.Add(this.lblStockingUnit);
            this.panel4.Controls.Add(this.lblbcd);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.lblCtg);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.lblRL);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.lblSu);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.lblName);
            this.panel4.Controls.Add(this.lblItemName);
            this.panel4.Location = new System.Drawing.Point(12, 69);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1167, 47);
            this.panel4.TabIndex = 10;
            // 
            // lblBarcode
            // 
            this.lblBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(1014, 17);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(16, 14);
            this.lblBarcode.TabIndex = 6;
            this.lblBarcode.Text = "...";
            // 
            // lblCategory
            // 
            this.lblCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(828, 17);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(16, 14);
            this.lblCategory.TabIndex = 5;
            this.lblCategory.Text = "...";
            // 
            // lblReOrderLvl
            // 
            this.lblReOrderLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReOrderLvl.AutoSize = true;
            this.lblReOrderLvl.Location = new System.Drawing.Point(607, 17);
            this.lblReOrderLvl.Name = "lblReOrderLvl";
            this.lblReOrderLvl.Size = new System.Drawing.Size(16, 14);
            this.lblReOrderLvl.TabIndex = 4;
            this.lblReOrderLvl.Text = "...";
            // 
            // lblStockingUnit
            // 
            this.lblStockingUnit.AutoSize = true;
            this.lblStockingUnit.Location = new System.Drawing.Point(368, 17);
            this.lblStockingUnit.Name = "lblStockingUnit";
            this.lblStockingUnit.Size = new System.Drawing.Size(16, 14);
            this.lblStockingUnit.TabIndex = 3;
            this.lblStockingUnit.Text = "...";
            // 
            // lblbcd
            // 
            this.lblbcd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblbcd.AutoSize = true;
            this.lblbcd.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblbcd.Location = new System.Drawing.Point(960, 17);
            this.lblbcd.Name = "lblbcd";
            this.lblbcd.Size = new System.Drawing.Size(52, 14);
            this.lblbcd.TabIndex = 1;
            this.lblbcd.Text = "Barcode:";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(998, 17);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 14);
            this.label16.TabIndex = 2;
            this.label16.Text = "...";
            // 
            // lblCtg
            // 
            this.lblCtg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCtg.AutoSize = true;
            this.lblCtg.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCtg.Location = new System.Drawing.Point(772, 17);
            this.lblCtg.Name = "lblCtg";
            this.lblCtg.Size = new System.Drawing.Size(57, 14);
            this.lblCtg.TabIndex = 1;
            this.lblCtg.Text = "Category:";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(810, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(16, 14);
            this.label14.TabIndex = 2;
            this.label14.Text = "...";
            // 
            // lblRL
            // 
            this.lblRL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRL.AutoSize = true;
            this.lblRL.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRL.Location = new System.Drawing.Point(523, 17);
            this.lblRL.Name = "lblRL";
            this.lblRL.Size = new System.Drawing.Size(86, 14);
            this.lblRL.TabIndex = 1;
            this.lblRL.Text = "Re-Order Level:";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(561, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 14);
            this.label12.TabIndex = 2;
            this.label12.Text = "...";
            // 
            // lblSu
            // 
            this.lblSu.AutoSize = true;
            this.lblSu.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSu.Location = new System.Drawing.Point(283, 17);
            this.lblSu.Name = "lblSu";
            this.lblSu.Size = new System.Drawing.Size(85, 14);
            this.lblSu.TabIndex = 1;
            this.lblSu.Text = "Stocking Units:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(321, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 14);
            this.label11.TabIndex = 2;
            this.label11.Text = "...";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbBatch);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.grdAdjustments);
            this.panel1.Controls.Add(this.grdStocks);
            this.panel1.Controls.Add(this.grdConsumptions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1170, 1206);
            this.panel1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 793);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Adjusted Stocks";
            // 
            // cmbBatch
            // 
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(3, 417);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(313, 22);
            this.cmbBatch.TabIndex = 6;
            this.cmbBatch.SelectedIndexChanged += new System.EventHandler(this.cmbBatch_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 389);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Consumed Stocks";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stock Detail";
            // 
            // btnAddStoc
            // 
            this.btnAddStoc.Location = new System.Drawing.Point(575, 12);
            this.btnAddStoc.Name = "btnAddStoc";
            this.btnAddStoc.Size = new System.Drawing.Size(126, 36);
            this.btnAddStoc.TabIndex = 9;
            this.btnAddStoc.Text = "Add Stock";
            this.btnAddStoc.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnAddStoc.UseVisualStyleBackColor = true;
            this.btnAddStoc.Click += new System.EventHandler(this.btnAddStoc_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "BATCH NO.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "TOTAL QTY.";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "AVAIL. QTY.";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "EXPIRY";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "CREATED AT";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "BATCH NO.";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.HeaderText = "CONSUMED QTY.";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "CONSUMPTION TYPE";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 70;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "CONSUMPTION DATE";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.HeaderText = "Batch No.";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn11.HeaderText = "Adjusted Qty.";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn12.HeaderText = "Adjustment Reason";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Adjustment Date";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 70;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Consumption Date";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "Consumption Date";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // frmItemInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1327, 641);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.btnAddStoc);
            this.Controls.Add(this.btnPrintBC);
            this.Controls.Add(this.btnGenBC);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmItemInfo";
            this.Text = "Item Info";
            this.Activated += new System.EventHandler(this.frmItemInfo_Activated);
            this.Load += new System.EventHandler(this.frmItemInfo_Load);
            this.Shown += new System.EventHandler(this.frmItemInfo_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grdStocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAdjustments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdConsumptions)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.DataGridView grdStocks;
        private System.Windows.Forms.DataGridView grdConsumptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridView grdAdjustments;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnGenBC;
        private System.Windows.Forms.Button btnPrintBC;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblSu;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblRL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblCtg;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblbcd;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblReOrderLvl;
        private System.Windows.Forms.Label lblStockingUnit;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.Button btnAddStoc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCBatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStoreCon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumptionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colconsumptionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjBatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjCreatedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuppliers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailableQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
    }
}