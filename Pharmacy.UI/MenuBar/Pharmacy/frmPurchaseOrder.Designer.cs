namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmPurchaseOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPurchaseOrder));
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.ErrUnitCost = new System.Windows.Forms.Label();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numUnitCost = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.ErrSupplier = new System.Windows.Forms.Label();
            this.ErrPurchaseOrderNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPurchaseOrderNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseOrderItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManufId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManuf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrossAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisc = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colDiscType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colSupplierId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.ErrItem = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpPurchaseOrderDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.txtPurchaseOrderItemId = new System.Windows.Forms.TextBox();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.numConversionUnit = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.errConvUnit = new System.Windows.Forms.Label();
            this.errUnit = new System.Windows.Forms.Label();
            this.numAvailableQty = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.btnLoadLSI = new System.Windows.Forms.Button();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.btnSupLSI = new System.Windows.Forms.Button();
            this.btnSupAI = new System.Windows.Forms.Button();
            this.btnLoadPrvSoldItems = new System.Windows.Forms.Button();
            this.cmbItems = new SergeUtils.EasyCompletionComboBox();
            this.unitIndexCombo = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmManufecturersList = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nextremove = new System.Windows.Forms.Label();
            this.pnlManufecturers = new System.Windows.Forms.FlowLayoutPanel();
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
            this.uC_SearchItems1 = new UC_SearchItems();
            this.label14 = new System.Windows.Forms.Label();
            this.dtDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearchItems = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConversionUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvailableQty)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Location = new System.Drawing.Point(1079, 577);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(96, 44);
            this.btnClearAll.TabIndex = 9;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(968, 577);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 44);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(284, 174);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(70, 22);
            this.btnAddItem.TabIndex = 5;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(358, 173);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(46, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear ";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClearForm_Click);
            // 
            // ErrUnitCost
            // 
            this.ErrUnitCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrUnitCost.ForeColor = System.Drawing.Color.Red;
            this.ErrUnitCost.Location = new System.Drawing.Point(896, 157);
            this.ErrUnitCost.Name = "ErrUnitCost";
            this.ErrUnitCost.Size = new System.Drawing.Size(13, 13);
            this.ErrUnitCost.TabIndex = 95;
            this.ErrUnitCost.Text = "*";
            this.ErrUnitCost.Visible = false;
            // 
            // ErrQuantity
            // 
            this.ErrQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrQuantity.ForeColor = System.Drawing.Color.Red;
            this.ErrQuantity.Location = new System.Drawing.Point(773, 154);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(13, 13);
            this.ErrQuantity.TabIndex = 94;
            this.ErrQuantity.Text = "*";
            this.ErrQuantity.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(793, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 14);
            this.label7.TabIndex = 93;
            this.label7.Text = "Unit Cost";
            this.label7.Visible = false;
            // 
            // numUnitCost
            // 
            this.numUnitCost.DecimalPlaces = 2;
            this.numUnitCost.Location = new System.Drawing.Point(796, 174);
            this.numUnitCost.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.numUnitCost.Name = "numUnitCost";
            this.numUnitCost.Size = new System.Drawing.Size(113, 21);
            this.numUnitCost.TabIndex = 3;
            this.numUnitCost.Visible = false;
            this.numUnitCost.ValueChanged += new System.EventHandler(this.numUnitCost_ValueChanged);
            this.numUnitCost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numUnitCost_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(674, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 14);
            this.label6.TabIndex = 92;
            this.label6.Text = "Quantity";
            this.label6.Visible = false;
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(677, 174);
            this.numQuantity.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(113, 21);
            this.numQuantity.TabIndex = 2;
            this.numQuantity.ThousandsSeparator = true;
            this.numQuantity.Visible = false;
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            this.numQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numQuantity_KeyDown);
            // 
            // ErrSupplier
            // 
            this.ErrSupplier.AutoSize = true;
            this.ErrSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSupplier.ForeColor = System.Drawing.Color.Red;
            this.ErrSupplier.Location = new System.Drawing.Point(397, 73);
            this.ErrSupplier.Name = "ErrSupplier";
            this.ErrSupplier.Size = new System.Drawing.Size(21, 25);
            this.ErrSupplier.TabIndex = 80;
            this.ErrSupplier.Text = "*";
            this.ErrSupplier.Visible = false;
            // 
            // ErrPurchaseOrderNum
            // 
            this.ErrPurchaseOrderNum.AutoSize = true;
            this.ErrPurchaseOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPurchaseOrderNum.ForeColor = System.Drawing.Color.Red;
            this.ErrPurchaseOrderNum.Location = new System.Drawing.Point(397, 44);
            this.ErrPurchaseOrderNum.Name = "ErrPurchaseOrderNum";
            this.ErrPurchaseOrderNum.Size = new System.Drawing.Size(21, 25);
            this.ErrPurchaseOrderNum.TabIndex = 79;
            this.ErrPurchaseOrderNum.Text = "*";
            this.ErrPurchaseOrderNum.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 14);
            this.label2.TabIndex = 78;
            this.label2.Text = "Select Supplier";
            // 
            // txtPurchaseOrderNo
            // 
            this.txtPurchaseOrderNo.Enabled = false;
            this.txtPurchaseOrderNo.Location = new System.Drawing.Point(148, 44);
            this.txtPurchaseOrderNo.Name = "txtPurchaseOrderNo";
            this.txtPurchaseOrderNo.Size = new System.Drawing.Size(246, 21);
            this.txtPurchaseOrderNo.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 14);
            this.label1.TabIndex = 75;
            this.label1.Text = "Purchase Order#";
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(21, 15);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 74;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // grdItems
            // 
            this.grdItems.AllowUserToAddRows = false;
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.BackgroundColor = System.Drawing.Color.White;
            this.grdItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdItems.ColumnHeadersHeight = 35;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colPurchaseOrderItemId,
            this.colItemID,
            this.colItemName,
            this.colManufId,
            this.colManuf,
            this.colUnit,
            this.colConvUnit,
            this.colAvQty,
            this.colQuantity,
            this.colUnitCost,
            this.colGrossAmount,
            this.colDisc,
            this.colDiscType,
            this.colBonus,
            this.colTotalAmount,
            this.colEdit,
            this.colRemove,
            this.colSupplierId});
            this.grdItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 244);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1163, 306);
            this.grdItems.TabIndex = 7;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            this.grdItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellEndEdit);
            this.grdItems.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdItems_CurrentCellDirtyStateChanged);
            this.grdItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdItems_DataError);
            this.grdItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdItems_EditingControlShowing);
            this.grdItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdItems_RowsAdded);
            this.grdItems.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdItems_UserDeletingRow);
            this.grdItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdItems_KeyDown);
            this.grdItems.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.grdItems_PreviewKeyDown);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            // 
            // colPurchaseOrderItemId
            // 
            this.colPurchaseOrderItemId.HeaderText = "PurchaseOrderItemId";
            this.colPurchaseOrderItemId.Name = "colPurchaseOrderItemId";
            this.colPurchaseOrderItemId.ReadOnly = true;
            this.colPurchaseOrderItemId.Visible = false;
            // 
            // colItemID
            // 
            this.colItemID.HeaderText = "Item ID";
            this.colItemID.Name = "colItemID";
            this.colItemID.ReadOnly = true;
            this.colItemID.Visible = false;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.HeaderText = "ItemName";
            this.colItemName.MinimumWidth = 160;
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colManufId
            // 
            this.colManufId.HeaderText = "ManufecturerId";
            this.colManufId.Name = "colManufId";
            this.colManufId.ReadOnly = true;
            this.colManufId.Visible = false;
            // 
            // colManuf
            // 
            this.colManuf.HeaderText = "Manufecturer";
            this.colManuf.MinimumWidth = 160;
            this.colManuf.Name = "colManuf";
            this.colManuf.ReadOnly = true;
            this.colManuf.Width = 160;
            // 
            // colUnit
            // 
            this.colUnit.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colConvUnit
            // 
            this.colConvUnit.HeaderText = "Conv. Unit";
            this.colConvUnit.Name = "colConvUnit";
            this.colConvUnit.ReadOnly = true;
            // 
            // colAvQty
            // 
            this.colAvQty.HeaderText = "Available Qty.";
            this.colAvQty.Name = "colAvQty";
            this.colAvQty.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            // 
            // colUnitCost
            // 
            this.colUnitCost.HeaderText = "Unit Cost";
            this.colUnitCost.Name = "colUnitCost";
            // 
            // colGrossAmount
            // 
            this.colGrossAmount.HeaderText = "Gross Amount";
            this.colGrossAmount.Name = "colGrossAmount";
            this.colGrossAmount.ReadOnly = true;
            // 
            // colDisc
            // 
            // 
            // 
            // 
            this.colDisc.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colDisc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colDisc.HeaderText = "Discount";
            this.colDisc.Increment = 1D;
            this.colDisc.Name = "colDisc";
            this.colDisc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colDiscType
            // 
            this.colDiscType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colDiscType.HeaderText = "Discount Type";
            this.colDiscType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.colDiscType.Name = "colDiscType";
            this.colDiscType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDiscType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colBonus
            // 
            this.colBonus.HeaderText = "Bonus";
            this.colBonus.Name = "colBonus";
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.HeaderText = "TotalAmount";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 40;
            // 
            // colRemove
            // 
            this.colRemove.HeaderText = "Remove";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 50;
            // 
            // colSupplierId
            // 
            this.colSupplierId.HeaderText = "SupplierId";
            this.colSupplierId.Name = "colSupplierId";
            this.colSupplierId.ReadOnly = true;
            this.colSupplierId.Visible = false;
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(148, 73);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(246, 22);
            this.cmbSuppliers.TabIndex = 11;
            this.cmbSuppliers.SelectedIndexChanged += new System.EventHandler(this.cmbSuppliers_SelectedIndexChanged);
            // 
            // ErrItem
            // 
            this.ErrItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrItem.ForeColor = System.Drawing.Color.Red;
            this.ErrItem.Location = new System.Drawing.Point(237, 159);
            this.ErrItem.Name = "ErrItem";
            this.ErrItem.Size = new System.Drawing.Size(13, 13);
            this.ErrItem.TabIndex = 112;
            this.ErrItem.Text = "*";
            this.ErrItem.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 14);
            this.label4.TabIndex = 111;
            this.label4.Text = "Search Item";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Enabled = false;
            this.txtTotalAmount.Location = new System.Drawing.Point(915, 175);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(134, 21);
            this.txtTotalAmount.TabIndex = 4;
            this.txtTotalAmount.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(912, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 14);
            this.label5.TabIndex = 115;
            this.label5.Text = "Total Amount";
            this.label5.Visible = false;
            // 
            // dtpPurchaseOrderDate
            // 
            this.dtpPurchaseOrderDate.Location = new System.Drawing.Point(554, 39);
            this.dtpPurchaseOrderDate.Name = "dtpPurchaseOrderDate";
            this.dtpPurchaseOrderDate.Size = new System.Drawing.Size(191, 21);
            this.dtpPurchaseOrderDate.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(475, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 14);
            this.label8.TabIndex = 117;
            this.label8.Text = "Order Date";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(680, 583);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 14);
            this.label3.TabIndex = 119;
            this.label3.Text = "GrandTotal";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.Location = new System.Drawing.Point(683, 601);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.Size = new System.Drawing.Size(246, 21);
            this.txtGrandTotal.TabIndex = 118;
            this.txtGrandTotal.Text = "0";
            // 
            // txtPurchaseOrderItemId
            // 
            this.txtPurchaseOrderItemId.Location = new System.Drawing.Point(677, 130);
            this.txtPurchaseOrderItemId.Name = "txtPurchaseOrderItemId";
            this.txtPurchaseOrderItemId.Size = new System.Drawing.Size(100, 21);
            this.txtPurchaseOrderItemId.TabIndex = 120;
            this.txtPurchaseOrderItemId.Text = "0";
            this.txtPurchaseOrderItemId.Visible = false;
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1157, 0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 30);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 120;
            this.pbInfo.TabStop = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // cmbUnit
            // 
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.Location = new System.Drawing.Point(477, 172);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(194, 22);
            this.cmbUnit.TabIndex = 1;
            this.cmbUnit.Visible = false;
            this.cmbUnit.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged);
            // 
            // numConversionUnit
            // 
            this.numConversionUnit.Enabled = false;
            this.numConversionUnit.Location = new System.Drawing.Point(477, 214);
            this.numConversionUnit.Name = "numConversionUnit";
            this.numConversionUnit.Size = new System.Drawing.Size(194, 21);
            this.numConversionUnit.TabIndex = 124;
            this.numConversionUnit.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(477, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 14);
            this.label9.TabIndex = 92;
            this.label9.Text = "Select Unit";
            this.label9.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(474, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 14);
            this.label10.TabIndex = 92;
            this.label10.Text = "Conversion Unit";
            this.label10.Visible = false;
            // 
            // errConvUnit
            // 
            this.errConvUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errConvUnit.ForeColor = System.Drawing.Color.Red;
            this.errConvUnit.Location = new System.Drawing.Point(658, 198);
            this.errConvUnit.Name = "errConvUnit";
            this.errConvUnit.Size = new System.Drawing.Size(13, 13);
            this.errConvUnit.TabIndex = 125;
            this.errConvUnit.Text = "*";
            this.errConvUnit.Visible = false;
            // 
            // errUnit
            // 
            this.errUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errUnit.ForeColor = System.Drawing.Color.Red;
            this.errUnit.Location = new System.Drawing.Point(655, 154);
            this.errUnit.Name = "errUnit";
            this.errUnit.Size = new System.Drawing.Size(13, 13);
            this.errUnit.TabIndex = 125;
            this.errUnit.Text = "*";
            this.errUnit.Visible = false;
            // 
            // numAvailableQty
            // 
            this.numAvailableQty.Enabled = false;
            this.numAvailableQty.Location = new System.Drawing.Point(677, 214);
            this.numAvailableQty.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numAvailableQty.Name = "numAvailableQty";
            this.numAvailableQty.Size = new System.Drawing.Size(113, 21);
            this.numAvailableQty.TabIndex = 126;
            this.numAvailableQty.ThousandsSeparator = true;
            this.numAvailableQty.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(674, 199);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 14);
            this.label11.TabIndex = 127;
            this.label11.Text = "Available Quantity";
            this.label11.Visible = false;
            // 
            // btnLoadLSI
            // 
            this.btnLoadLSI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadLSI.Location = new System.Drawing.Point(1055, 184);
            this.btnLoadLSI.Name = "btnLoadLSI";
            this.btnLoadLSI.Size = new System.Drawing.Size(120, 53);
            this.btnLoadLSI.TabIndex = 5;
            this.btnLoadLSI.Text = "Load Low Stock Items";
            this.btnLoadLSI.UseVisualStyleBackColor = true;
            this.btnLoadLSI.Click += new System.EventHandler(this.btnLoadLSI_Click);
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.Location = new System.Drawing.Point(427, 70);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(32, 25);
            this.btnAddSupplier.TabIndex = 128;
            this.btnAddSupplier.Text = "+";
            this.btnAddSupplier.UseVisualStyleBackColor = true;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            // 
            // btnSupLSI
            // 
            this.btnSupLSI.Location = new System.Drawing.Point(148, 111);
            this.btnSupLSI.Name = "btnSupLSI";
            this.btnSupLSI.Size = new System.Drawing.Size(112, 40);
            this.btnSupLSI.TabIndex = 128;
            this.btnSupLSI.Text = "Supplier Low Stock Items";
            this.btnSupLSI.UseVisualStyleBackColor = true;
            this.btnSupLSI.Click += new System.EventHandler(this.btnSupLSI_Click);
            // 
            // btnSupAI
            // 
            this.btnSupAI.Location = new System.Drawing.Point(281, 111);
            this.btnSupAI.Name = "btnSupAI";
            this.btnSupAI.Size = new System.Drawing.Size(113, 40);
            this.btnSupAI.TabIndex = 128;
            this.btnSupAI.Text = "All Supplier Items";
            this.btnSupAI.UseVisualStyleBackColor = true;
            this.btnSupAI.Click += new System.EventHandler(this.btnSupAI_Click);
            // 
            // btnLoadPrvSoldItems
            // 
            this.btnLoadPrvSoldItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadPrvSoldItems.Location = new System.Drawing.Point(1055, 121);
            this.btnLoadPrvSoldItems.Name = "btnLoadPrvSoldItems";
            this.btnLoadPrvSoldItems.Size = new System.Drawing.Size(120, 57);
            this.btnLoadPrvSoldItems.TabIndex = 5;
            this.btnLoadPrvSoldItems.Text = "Load Previously Sold Items";
            this.btnLoadPrvSoldItems.UseVisualStyleBackColor = true;
            this.btnLoadPrvSoldItems.Click += new System.EventHandler(this.btnLoadPrvSoldItems_Click);
            // 
            // cmbItems
            // 
            this.cmbItems.DropDownWidth = 350;
            this.cmbItems.FormattingEnabled = true;
            this.cmbItems.Location = new System.Drawing.Point(24, 216);
            this.cmbItems.Name = "cmbItems";
            this.cmbItems.Size = new System.Drawing.Size(226, 22);
            this.cmbItems.TabIndex = 122;
            this.cmbItems.Visible = false;
            // 
            // unitIndexCombo
            // 
            this.unitIndexCombo.FormattingEnabled = true;
            this.unitIndexCombo.Items.AddRange(new object[] {
            "Units",
            "Pack"});
            this.unitIndexCombo.Location = new System.Drawing.Point(832, 36);
            this.unitIndexCombo.Name = "unitIndexCombo";
            this.unitIndexCombo.Size = new System.Drawing.Size(121, 22);
            this.unitIndexCombo.TabIndex = 129;
            this.unitIndexCombo.SelectedIndexChanged += new System.EventHandler(this.unitIndexCombo_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(764, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 14);
            this.label12.TabIndex = 130;
            this.label12.Text = "Select Unit";
            // 
            // cmManufecturersList
            // 
            this.cmManufecturersList.FormattingEnabled = true;
            this.cmManufecturersList.Location = new System.Drawing.Point(468, 129);
            this.cmManufecturersList.Name = "cmManufecturersList";
            this.cmManufecturersList.Size = new System.Drawing.Size(200, 22);
            this.cmManufecturersList.TabIndex = 133;
            this.cmManufecturersList.Visible = false;
            this.cmManufecturersList.SelectedIndexChanged += new System.EventHandler(this.cmManufecturersList_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(466, 111);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(127, 14);
            this.label13.TabIndex = 135;
            this.label13.Text = "Choose Manufacturers ";
            this.label13.Visible = false;
            // 
            // nextremove
            // 
            this.nextremove.AutoSize = true;
            this.nextremove.ForeColor = System.Drawing.Color.Blue;
            this.nextremove.Location = new System.Drawing.Point(529, 156);
            this.nextremove.Name = "nextremove";
            this.nextremove.Size = new System.Drawing.Size(139, 14);
            this.nextremove.TabIndex = 138;
            this.nextremove.Text = "Double Click to remove it.";
            this.nextremove.Visible = false;
            // 
            // pnlManufecturers
            // 
            this.pnlManufecturers.AutoScroll = true;
            this.pnlManufecturers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlManufecturers.Location = new System.Drawing.Point(690, 128);
            this.pnlManufecturers.Name = "pnlManufecturers";
            this.pnlManufecturers.Size = new System.Drawing.Size(242, 41);
            this.pnlManufecturers.TabIndex = 137;
            this.pnlManufecturers.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "PurchaseOrderItemId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Item ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "ItemName";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 160;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Unit Cost";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "TotalAmount";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 160;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 160;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "SupplierId";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "TotalAmount";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "SupplierId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Unit Cost";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Gross Amount";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Discount";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Bonus";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "TotalAmount";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "SupplierId";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // uC_SearchItems1
            // 
            this.uC_SearchItems1.BackColor = System.Drawing.Color.White;
            this.uC_SearchItems1.Location = new System.Drawing.Point(24, 174);
            this.uC_SearchItems1.Name = "uC_SearchItems1";
            this.uC_SearchItems1.PageNo = 0;
            this.uC_SearchItems1.SelectedItemId = 0;
            this.uC_SearchItems1.SelectedItemName = null;
            this.uC_SearchItems1.Size = new System.Drawing.Size(226, 25);
            this.uC_SearchItems1.StoreId = 0;
            this.uC_SearchItems1.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(474, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 14);
            this.label14.TabIndex = 140;
            this.label14.Text = "Delivery Date";
            // 
            // dtDeliveryDate
            // 
            this.dtDeliveryDate.Location = new System.Drawing.Point(554, 74);
            this.dtDeliveryDate.Name = "dtDeliveryDate";
            this.dtDeliveryDate.Size = new System.Drawing.Size(190, 21);
            this.dtDeliveryDate.TabIndex = 139;
            // 
            // btnSearchItems
            // 
            this.btnSearchItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnSearchItems.FlatAppearance.BorderSize = 0;
            this.btnSearchItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSearchItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchItems.ForeColor = System.Drawing.Color.White;
            this.btnSearchItems.Location = new System.Drawing.Point(255, 175);
            this.btnSearchItems.Name = "btnSearchItems";
            this.btnSearchItems.Size = new System.Drawing.Size(23, 21);
            this.btnSearchItems.TabIndex = 1018;
            this.btnSearchItems.Text = "?";
            this.btnSearchItems.UseVisualStyleBackColor = false;
            this.btnSearchItems.Click += new System.EventHandler(this.btnSearchItems_Click);
            // 
            // frmPurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1187, 634);
            this.Controls.Add(this.btnSearchItems);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dtDeliveryDate);
            this.Controls.Add(this.nextremove);
            this.Controls.Add(this.pnlManufecturers);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmManufecturersList);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.unitIndexCombo);
            this.Controls.Add(this.btnLoadPrvSoldItems);
            this.Controls.Add(this.btnLoadLSI);
            this.Controls.Add(this.btnSupAI);
            this.Controls.Add(this.btnSupLSI);
            this.Controls.Add(this.btnAddSupplier);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numAvailableQty);
            this.Controls.Add(this.errUnit);
            this.Controls.Add(this.errConvUnit);
            this.Controls.Add(this.numConversionUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.cmbItems);
            this.Controls.Add(this.uC_SearchItems1);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.txtPurchaseOrderItemId);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtGrandTotal);
            this.Controls.Add(this.numUnitCost);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpPurchaseOrderDate);
            this.Controls.Add(this.ErrQuantity);
            this.Controls.Add(this.cmbSuppliers);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.ErrUnitCost);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.ErrSupplier);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.ErrPurchaseOrderNum);
            this.Controls.Add(this.ErrItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPurchaseOrderNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.grdItems);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmPurchaseOrder";
            this.Text = "Purchase Order";
            this.Load += new System.EventHandler(this.frmPurchaseOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numUnitCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConversionUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvailableQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label ErrUnitCost;
        private System.Windows.Forms.Label ErrQuantity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numUnitCost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label ErrSupplier;
        private System.Windows.Forms.Label ErrPurchaseOrderNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPurchaseOrderNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.Label ErrItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpPurchaseOrderDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGrandTotal;
        private System.Windows.Forms.TextBox txtPurchaseOrderItemId;
        private System.Windows.Forms.PictureBox pbInfo;
        private SergeUtils.EasyCompletionComboBox cmbItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private UC_SearchItems uC_SearchItems1;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.NumericUpDown numConversionUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label errConvUnit;
        private System.Windows.Forms.Label errUnit;
        private System.Windows.Forms.NumericUpDown numAvailableQty;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnLoadLSI;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Button btnSupLSI;
        private System.Windows.Forms.Button btnSupAI;
        private System.Windows.Forms.Button btnLoadPrvSoldItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.ComboBox unitIndexCombo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmManufecturersList;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label nextremove;
        private System.Windows.Forms.FlowLayoutPanel pnlManufecturers;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseOrderItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManufId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManuf;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrossAmount;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colDisc;
        private System.Windows.Forms.DataGridViewComboBoxColumn colDiscType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtDeliveryDate;
        private System.Windows.Forms.Button btnSearchItems;
    }
}