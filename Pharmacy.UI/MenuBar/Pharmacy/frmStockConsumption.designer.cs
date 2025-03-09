namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmStockConsumption
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
            this.btnAddItem = new System.Windows.Forms.Button();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockConsumptionItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumptionTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumptionTypeString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.numConvUnit = new System.Windows.Forms.NumericUpDown();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSelectBatch = new System.Windows.Forms.ComboBox();
            this.ErrSelectItem = new System.Windows.Forms.Label();
            this.ErrSelectBatch = new System.Windows.Forms.Label();
            this.lblSelectBatch = new System.Windows.Forms.Label();
            this.cmbConsumtionType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtAvlQty = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmbSelectItems = new SergeUtils.EasyCompletionComboBox();
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
            this.uC_SearchItems1 = new UC_SearchItems();
            this.unitCost = new System.Windows.Forms.Label();
            this.discPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Location = new System.Drawing.Point(698, 76);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(170, 31);
            this.btnAddItem.TabIndex = 6;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(12, 131);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(390, 54);
            this.txtComment.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 14);
            this.label5.TabIndex = 2;
            this.label5.Text = "Comment";
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
            this.grdItems.ColumnHeadersHeight = 35;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colStockConsumptionItemId,
            this.colItemId,
            this.colItemName,
            this.colBatchId,
            this.colBatch,
            this.colUnit,
            this.colConvUnit,
            this.colCostPrice,
            this.colDisPrice,
            this.colQuantity,
            this.colConsumptionTypeId,
            this.colConsumptionTypeString,
            this.colComment,
            this.colEdit,
            this.colRemove});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 215);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1197, 325);
            this.grdItems.TabIndex = 8;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            // 
            // colStockConsumptionItemId
            // 
            this.colStockConsumptionItemId.HeaderText = "Stock Consumption Item Id";
            this.colStockConsumptionItemId.Name = "colStockConsumptionItemId";
            this.colStockConsumptionItemId.ReadOnly = true;
            this.colStockConsumptionItemId.Visible = false;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "ItemId";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            this.colItemId.Visible = false;
            // 
            // colItemName
            // 
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.MinimumWidth = 100;
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colBatchId
            // 
            this.colBatchId.HeaderText = "BacthId";
            this.colBatchId.Name = "colBatchId";
            this.colBatchId.ReadOnly = true;
            this.colBatchId.Visible = false;
            // 
            // colBatch
            // 
            this.colBatch.HeaderText = "Batch";
            this.colBatch.Name = "colBatch";
            this.colBatch.ReadOnly = true;
            this.colBatch.Width = 200;
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
            // colCostPrice
            // 
            this.colCostPrice.HeaderText = "Unit Cost";
            this.colCostPrice.Name = "colCostPrice";
            this.colCostPrice.ReadOnly = true;
            // 
            // colDisPrice
            // 
            this.colDisPrice.HeaderText = "Disc. Price";
            this.colDisPrice.Name = "colDisPrice";
            this.colDisPrice.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            // 
            // colConsumptionTypeId
            // 
            this.colConsumptionTypeId.HeaderText = "Consumption Type Id";
            this.colConsumptionTypeId.Name = "colConsumptionTypeId";
            this.colConsumptionTypeId.ReadOnly = true;
            // 
            // colConsumptionTypeString
            // 
            this.colConsumptionTypeString.HeaderText = "Consumption Type";
            this.colConsumptionTypeString.Name = "colConsumptionTypeString";
            this.colConsumptionTypeString.ReadOnly = true;
            // 
            // colComment
            // 
            this.colComment.HeaderText = "Comment";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 50;
            // 
            // colRemove
            // 
            this.colRemove.HeaderText = "Remove";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 60;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Conversion Unit";
            // 
            // numConvUnit
            // 
            this.numConvUnit.Enabled = false;
            this.numConvUnit.Location = new System.Drawing.Point(411, 126);
            this.numConvUnit.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.numConvUnit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numConvUnit.Name = "numConvUnit";
            this.numConvUnit.Size = new System.Drawing.Size(125, 21);
            this.numConvUnit.TabIndex = 3;
            this.numConvUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Location = new System.Drawing.Point(994, 547);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(215, 42);
            this.btnClearAll.TabIndex = 10;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(698, 113);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(170, 31);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(780, 547);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(208, 42);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // line1
            // 
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(-3, 191);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1228, 10);
            this.line1.TabIndex = 1005;
            this.line1.Text = "line1";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1221, 42);
            this.panel1.TabIndex = 1008;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Red;
            this.ErrMessage.Location = new System.Drawing.Point(9, 569);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 52;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 14);
            this.label1.TabIndex = 1012;
            this.label1.Text = "Select Item";
            // 
            // cmbSelectBatch
            // 
            this.cmbSelectBatch.FormattingEnabled = true;
            this.cmbSelectBatch.Location = new System.Drawing.Point(208, 78);
            this.cmbSelectBatch.Name = "cmbSelectBatch";
            this.cmbSelectBatch.Size = new System.Drawing.Size(194, 22);
            this.cmbSelectBatch.TabIndex = 1010;
            // 
            // ErrSelectItem
            // 
            this.ErrSelectItem.AutoSize = true;
            this.ErrSelectItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectItem.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectItem.Location = new System.Drawing.Point(179, 59);
            this.ErrSelectItem.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.Name = "ErrSelectItem";
            this.ErrSelectItem.Size = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.TabIndex = 1020;
            this.ErrSelectItem.Text = "*";
            this.ErrSelectItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrSelectItem.Visible = false;
            // 
            // ErrSelectBatch
            // 
            this.ErrSelectBatch.AutoSize = true;
            this.ErrSelectBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectBatch.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectBatch.Location = new System.Drawing.Point(384, 59);
            this.ErrSelectBatch.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectBatch.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectBatch.Name = "ErrSelectBatch";
            this.ErrSelectBatch.Size = new System.Drawing.Size(15, 16);
            this.ErrSelectBatch.TabIndex = 1021;
            this.ErrSelectBatch.Text = "*";
            this.ErrSelectBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrSelectBatch.Visible = false;
            // 
            // lblSelectBatch
            // 
            this.lblSelectBatch.AutoSize = true;
            this.lblSelectBatch.Location = new System.Drawing.Point(208, 60);
            this.lblSelectBatch.Name = "lblSelectBatch";
            this.lblSelectBatch.Size = new System.Drawing.Size(69, 14);
            this.lblSelectBatch.TabIndex = 1015;
            this.lblSelectBatch.Text = "Select Batch";
            // 
            // cmbConsumtionType
            // 
            this.cmbConsumtionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbConsumtionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbConsumtionType.FormattingEnabled = true;
            this.cmbConsumtionType.Items.AddRange(new object[] {
            "Services",
            "Sales",
            "Damages",
            "Returned"});
            this.cmbConsumtionType.Location = new System.Drawing.Point(542, 125);
            this.cmbConsumtionType.Name = "cmbConsumtionType";
            this.cmbConsumtionType.Size = new System.Drawing.Size(141, 22);
            this.cmbConsumtionType.TabIndex = 1019;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(539, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 14);
            this.label4.TabIndex = 1017;
            this.label4.Text = "Consumption Type";
            // 
            // cmbUnit
            // 
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Location = new System.Drawing.Point(408, 78);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(128, 22);
            this.cmbUnit.TabIndex = 1011;
            this.cmbUnit.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged_1);
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(542, 78);
            this.numQuantity.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(141, 21);
            this.numQuantity.TabIndex = 1018;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ErrQuantity
            // 
            this.ErrQuantity.AutoSize = true;
            this.ErrQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrQuantity.ForeColor = System.Drawing.Color.Red;
            this.ErrQuantity.Location = new System.Drawing.Point(597, 60);
            this.ErrQuantity.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(15, 16);
            this.ErrQuantity.TabIndex = 1022;
            this.ErrQuantity.Text = "*";
            this.ErrQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrQuantity.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(408, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 14);
            this.label2.TabIndex = 1014;
            this.label2.Text = "Unit";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(539, 62);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(52, 14);
            this.lblQuantity.TabIndex = 1013;
            this.lblQuantity.Text = "Quantity";
            // 
            // txtAvlQty
            // 
            this.txtAvlQty.Enabled = false;
            this.txtAvlQty.Location = new System.Drawing.Point(411, 167);
            this.txtAvlQty.Name = "txtAvlQty";
            this.txtAvlQty.Size = new System.Drawing.Size(125, 21);
            this.txtAvlQty.TabIndex = 1016;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(408, 150);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(101, 14);
            this.label24.TabIndex = 1023;
            this.label24.Text = "Available Quantity";
            // 
            // cmbSelectItems
            // 
            this.cmbSelectItems.DropDownWidth = 350;
            this.cmbSelectItems.FormattingEnabled = true;
            this.cmbSelectItems.Location = new System.Drawing.Point(291, 4);
            this.cmbSelectItems.Name = "cmbSelectItems";
            this.cmbSelectItems.Size = new System.Drawing.Size(204, 21);
            this.cmbSelectItems.TabIndex = 60;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ItemId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ITEM NAME";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "BacthId";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "BATCH";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "QUANTITY";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "ConsumptionTypeId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "CONSUMPTION TYPE";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "COMMENT";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Comment";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Consumption Type";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Comment";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Comment";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // uC_SearchItems1
            // 
            this.uC_SearchItems1.BackColor = System.Drawing.Color.White;
            this.uC_SearchItems1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uC_SearchItems1.Location = new System.Drawing.Point(16, 77);
            this.uC_SearchItems1.Name = "uC_SearchItems1";
            this.uC_SearchItems1.PageNo = 0;
            this.uC_SearchItems1.SelectedItemId = 0;
            this.uC_SearchItems1.SelectedItemName = null;
            this.uC_SearchItems1.Size = new System.Drawing.Size(185, 22);
            this.uC_SearchItems1.StoreId = 0;
            this.uC_SearchItems1.TabIndex = 1009;
            this.uC_SearchItems1.Load += new System.EventHandler(this.uC_SearchItems1_Load_1);
            // 
            // unitCost
            // 
            this.unitCost.AutoSize = true;
            this.unitCost.Location = new System.Drawing.Point(645, 131);
            this.unitCost.Name = "unitCost";
            this.unitCost.Size = new System.Drawing.Size(0, 14);
            this.unitCost.TabIndex = 1024;
            this.unitCost.Visible = false;
            // 
            // discPrice
            // 
            this.discPrice.AutoSize = true;
            this.discPrice.Location = new System.Drawing.Point(648, 153);
            this.discPrice.Name = "discPrice";
            this.discPrice.Size = new System.Drawing.Size(0, 14);
            this.discPrice.TabIndex = 1025;
            this.discPrice.Visible = false;
            // 
            // frmStockConsumption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1221, 593);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.discPrice);
            this.Controls.Add(this.unitCost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSelectBatch);
            this.Controls.Add(this.ErrSelectItem);
            this.Controls.Add(this.ErrSelectBatch);
            this.Controls.Add(this.uC_SearchItems1);
            this.Controls.Add(this.lblSelectBatch);
            this.Controls.Add(this.cmbConsumtionType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.ErrQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtAvlQty);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.numConvUnit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStockConsumption";
            this.Text = "Consume Stock";
            this.Load += new System.EventHandler(this.frmConsumeStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label label5;
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
        private SergeUtils.EasyCompletionComboBox cmbSelectItems;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSelectBatch;
        private System.Windows.Forms.Label ErrSelectItem;
        private System.Windows.Forms.Label ErrSelectBatch;
        private UC_SearchItems uC_SearchItems1;
        private System.Windows.Forms.Label lblSelectBatch;
        private System.Windows.Forms.ComboBox cmbConsumtionType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label ErrQuantity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtAvlQty;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.Label unitCost;
        private System.Windows.Forms.Label discPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockConsumptionItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumptionTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumptionTypeString;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
    }
}