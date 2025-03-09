namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmStockAdjustment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjustmentItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjustmentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrSelectItem = new System.Windows.Forms.Label();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.errSelectBatch = new System.Windows.Forms.Label();
            this.cmbItems = new SergeUtils.EasyCompletionComboBox();
            this.lblLoadingItems = new System.Windows.Forms.Label();
            this.numConvUnit = new System.Windows.Forms.NumericUpDown();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdjustmentPrice = new System.Windows.Forms.TextBox();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
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
            this.uC_SearchItems1 = new UC_SearchItems();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).BeginInit();
            this.SuspendLayout();
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
            this.colAdjustmentItemId,
            this.colItemId,
            this.colItem,
            this.colBatchID,
            this.colBatch,
            this.colCostPrice,
            this.colUnit,
            this.colConvUnit,
            this.colQuantity,
            this.colAdjustmentPrice,
            this.colReason,
            this.colEdit,
            this.colRemove});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 125);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowHeadersWidth = 20;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1172, 433);
            this.grdItems.TabIndex = 7;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            // 
            // colAdjustmentItemId
            // 
            this.colAdjustmentItemId.HeaderText = "AdjustmentItemId";
            this.colAdjustmentItemId.Name = "colAdjustmentItemId";
            this.colAdjustmentItemId.ReadOnly = true;
            this.colAdjustmentItemId.Visible = false;
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
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBatchID
            // 
            this.colBatchID.HeaderText = "BatchId";
            this.colBatchID.Name = "colBatchID";
            this.colBatchID.ReadOnly = true;
            this.colBatchID.Visible = false;
            // 
            // colBatch
            // 
            this.colBatch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBatch.HeaderText = "Batch";
            this.colBatch.Name = "colBatch";
            this.colBatch.ReadOnly = true;
            // 
            // colCostPrice
            // 
            this.colCostPrice.HeaderText = "Cost Price";
            this.colCostPrice.Name = "colCostPrice";
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
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            this.colQuantity.Width = 60;
            // 
            // colAdjustmentPrice
            // 
            this.colAdjustmentPrice.HeaderText = "Adjustment Price";
            this.colAdjustmentPrice.Name = "colAdjustmentPrice";
            this.colAdjustmentPrice.ReadOnly = true;
            this.colAdjustmentPrice.Width = 130;
            // 
            // colReason
            // 
            this.colReason.HeaderText = "Reason";
            this.colReason.Name = "colReason";
            this.colReason.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 60;
            // 
            // colRemove
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.NullValue = null;
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRemove.HeaderText = "Remove";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.ToolTipText = "Remove Item";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 50;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(702, 567);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(216, 42);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Location = new System.Drawing.Point(924, 567);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(260, 42);
            this.btnClearAll.TabIndex = 9;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(840, 91);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(176, 25);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 1000;
            this.label6.Text = "Select Item";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(402, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(615, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Reason For Adjustment";
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.Transparent;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.Black;
            this.btnAddItem.Location = new System.Drawing.Point(840, 55);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(176, 25);
            this.btnAddItem.TabIndex = 4;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Red;
            this.ErrMessage.Location = new System.Drawing.Point(9, 594);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrSelectItem
            // 
            this.ErrSelectItem.AutoSize = true;
            this.ErrSelectItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectItem.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectItem.Location = new System.Drawing.Point(179, 37);
            this.ErrSelectItem.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.Name = "ErrSelectItem";
            this.ErrSelectItem.Size = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.TabIndex = 25;
            this.ErrSelectItem.Text = "*";
            this.ErrSelectItem.Visible = false;
            // 
            // ErrQuantity
            // 
            this.ErrQuantity.AutoSize = true;
            this.ErrQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrQuantity.ForeColor = System.Drawing.Color.Red;
            this.ErrQuantity.Location = new System.Drawing.Point(448, 77);
            this.ErrQuantity.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(15, 16);
            this.ErrQuantity.TabIndex = 36;
            this.ErrQuantity.Text = "*";
            this.ErrQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrQuantity.Visible = false;
            // 
            // numQty
            // 
            this.numQty.Location = new System.Drawing.Point(404, 95);
            this.numQty.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numQty.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(205, 21);
            this.numQty.TabIndex = 2;
            this.numQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQty.ValueChanged += new System.EventHandler(this.numQty_ValueChanged);
            // 
            // cmbBatch
            // 
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(200, 53);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(198, 22);
            this.cmbBatch.TabIndex = 1;
            this.cmbBatch.SelectedIndexChanged += new System.EventHandler(this.cmbSelectBatch_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(197, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Select Batch";
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(615, 57);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(212, 57);
            this.txtReason.TabIndex = 3;
            // 
            // errSelectBatch
            // 
            this.errSelectBatch.AutoSize = true;
            this.errSelectBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errSelectBatch.ForeColor = System.Drawing.Color.Red;
            this.errSelectBatch.Location = new System.Drawing.Point(378, 34);
            this.errSelectBatch.MaximumSize = new System.Drawing.Size(15, 16);
            this.errSelectBatch.MinimumSize = new System.Drawing.Size(15, 16);
            this.errSelectBatch.Name = "errSelectBatch";
            this.errSelectBatch.Size = new System.Drawing.Size(15, 16);
            this.errSelectBatch.TabIndex = 49;
            this.errSelectBatch.Text = "*";
            this.errSelectBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errSelectBatch.Visible = false;
            // 
            // cmbItems
            // 
            this.cmbItems.DropDownWidth = 350;
            this.cmbItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItems.FormattingEnabled = true;
            this.cmbItems.Location = new System.Drawing.Point(16, 95);
            this.cmbItems.Name = "cmbItems";
            this.cmbItems.Size = new System.Drawing.Size(180, 21);
            this.cmbItems.TabIndex = 60;
            this.cmbItems.Visible = false;
            // 
            // lblLoadingItems
            // 
            this.lblLoadingItems.AutoSize = true;
            this.lblLoadingItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingItems.Location = new System.Drawing.Point(13, 78);
            this.lblLoadingItems.Name = "lblLoadingItems";
            this.lblLoadingItems.Size = new System.Drawing.Size(82, 13);
            this.lblLoadingItems.TabIndex = 61;
            this.lblLoadingItems.Text = "Loading Items...";
            this.lblLoadingItems.Visible = false;
            // 
            // numConvUnit
            // 
            this.numConvUnit.Enabled = false;
            this.numConvUnit.Location = new System.Drawing.Point(404, 55);
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
            this.numConvUnit.ReadOnly = true;
            this.numConvUnit.Size = new System.Drawing.Size(97, 21);
            this.numConvUnit.TabIndex = 1004;
            this.numConvUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbUnit
            // 
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Location = new System.Drawing.Point(308, 94);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(90, 22);
            this.cmbUnit.TabIndex = 1001;
            this.cmbUnit.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged);
            this.cmbUnit.SelectedValueChanged += new System.EventHandler(this.cmbUnit_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 14);
            this.label3.TabIndex = 1002;
            this.label3.Text = "Conversion Unit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 14);
            this.label4.TabIndex = 1003;
            this.label4.Text = "Unit";
            // 
            // txtAdjustmentPrice
            // 
            this.txtAdjustmentPrice.Location = new System.Drawing.Point(507, 57);
            this.txtAdjustmentPrice.Name = "txtAdjustmentPrice";
            this.txtAdjustmentPrice.ReadOnly = true;
            this.txtAdjustmentPrice.Size = new System.Drawing.Size(102, 21);
            this.txtAdjustmentPrice.TabIndex = 1005;
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.Location = new System.Drawing.Point(202, 95);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.ReadOnly = true;
            this.txtCostPrice.Size = new System.Drawing.Size(100, 21);
            this.txtCostPrice.TabIndex = 1006;
            this.txtCostPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCostPrice_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(202, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 1007;
            this.label5.Text = "Cost Price";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(506, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 14);
            this.label8.TabIndex = 1008;
            this.label8.Text = "Adjustment Price";
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
            this.dataGridViewTextBoxColumn4.Visible = false;
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Batch";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Expiry";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Discount Type";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Discount";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Sales Tax Type";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 50;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Sales Tax";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 130;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 50;
            // 
            // uC_SearchItems1
            // 
            this.uC_SearchItems1.BackColor = System.Drawing.Color.White;
            this.uC_SearchItems1.Location = new System.Drawing.Point(16, 53);
            this.uC_SearchItems1.Name = "uC_SearchItems1";
            this.uC_SearchItems1.PageNo = 0;
            this.uC_SearchItems1.SelectedItemId = 0;
            this.uC_SearchItems1.SelectedItemName = null;
            this.uC_SearchItems1.Size = new System.Drawing.Size(180, 25);
            this.uC_SearchItems1.StoreId = 0;
            this.uC_SearchItems1.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Sales Tax";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // frmStockAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1191, 619);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCostPrice);
            this.Controls.Add(this.txtAdjustmentPrice);
            this.Controls.Add(this.numConvUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uC_SearchItems1);
            this.Controls.Add(this.lblLoadingItems);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.cmbItems);
            this.Controls.Add(this.errSelectBatch);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbBatch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ErrQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ErrSelectItem);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.ErrMessage);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStockAdjustment";
            this.Text = "Adjustments";
            this.Load += new System.EventHandler(this.frmAddStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.Button btnSave;
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
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrSelectItem;
        private System.Windows.Forms.Label ErrQuantity;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label errSelectBatch;
        private SergeUtils.EasyCompletionComboBox cmbItems;
        private System.Windows.Forms.Label lblLoadingItems;
        private UC_SearchItems uC_SearchItems1;
        private System.Windows.Forms.NumericUpDown numConvUnit;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAdjustmentPrice;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjustmentItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjustmentPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReason;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
    }
}