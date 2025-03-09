namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmProcedureInvoice2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcedureInvoice2));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colInvoiceItemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcedureId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcedureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsNewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReturnQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReturnQtyInitial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colReturnToggle = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ErrSelectItem = new System.Windows.Forms.Label();
            this.ErrSelectBatch = new System.Windows.Forms.Label();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numRate = new System.Windows.Forms.NumericUpDown();
            this.errRate = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDiscountType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numTotalDiscount = new System.Windows.Forms.NumericUpDown();
            this.cmbTotalDiscountType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.dtpPmntType1 = new System.Windows.Forms.DateTimePicker();
            this.cmbPmntType1 = new System.Windows.Forms.ComboBox();
            this.numPmnt1 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblAdvance = new System.Windows.Forms.Label();
            this.lblDue = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlPmnt1 = new System.Windows.Forms.Panel();
            this.txtInvPmntId1 = new System.Windows.Forms.TextBox();
            this.txtChequeInfoId1 = new System.Windows.Forms.TextBox();
            this.pnlPmnt2 = new System.Windows.Forms.Panel();
            this.txtInvPmntId2 = new System.Windows.Forms.TextBox();
            this.txtChequeInfoId2 = new System.Windows.Forms.TextBox();
            this.dtpPmntType2 = new System.Windows.Forms.DateTimePicker();
            this.cmbPmntType2 = new System.Windows.Forms.ComboBox();
            this.numPmnt2 = new System.Windows.Forms.NumericUpDown();
            this.pnlPmnt3 = new System.Windows.Forms.Panel();
            this.txtInvPmntId3 = new System.Windows.Forms.TextBox();
            this.txtChequeInfoId3 = new System.Windows.Forms.TextBox();
            this.dtpPmntType3 = new System.Windows.Forms.DateTimePicker();
            this.cmbPmntType3 = new System.Windows.Forms.ComboBox();
            this.numPmnt3 = new System.Windows.Forms.NumericUpDown();
            this.chkPmnt1 = new System.Windows.Forms.CheckBox();
            this.chkPmnt2 = new System.Windows.Forms.CheckBox();
            this.chkPmnt3 = new System.Windows.Forms.CheckBox();
            this.btnAddPatient = new System.Windows.Forms.Button();
            this.txtSearchPatient = new System.Windows.Forms.TextBox();
            this.pnlLoadedInvoiceData = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.pnlRefund = new System.Windows.Forms.Panel();
            this.txtCurrentRefund = new System.Windows.Forms.TextBox();
            this.txtRefundAmount = new System.Windows.Forms.TextBox();
            this.dtpRedundDate = new System.Windows.Forms.DateTimePicker();
            this.txtRefundReason = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtInvoiceNote = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.grdSuggestion = new System.Windows.Forms.DataGridView();
            this.colPatientID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearchByName = new System.Windows.Forms.TextBox();
            this.txtSearchByPhone = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.pnlAddItem = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblSelectBatch = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProcedure = new System.Windows.Forms.ComboBox();
            this.Errdisc = new System.Windows.Forms.Label();
            this.txtNetAmount = new System.Windows.Forms.TextBox();
            this.txtAvlQty = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.pnlPatientDetails = new System.Windows.Forms.GroupBox();
            this.chkPmnt4 = new System.Windows.Forms.CheckBox();
            this.numPmnt4 = new System.Windows.Forms.NumericUpDown();
            this.cmbPmntType4 = new System.Windows.Forms.ComboBox();
            this.dtpPmntType4 = new System.Windows.Forms.DateTimePicker();
            this.txtChequeInfoId4 = new System.Windows.Forms.TextBox();
            this.txtInvPmntId4 = new System.Windows.Forms.TextBox();
            this.pnlPmnt4 = new System.Windows.Forms.Panel();
            this.chkPmnt5 = new System.Windows.Forms.CheckBox();
            this.pnlPmnt5 = new System.Windows.Forms.Panel();
            this.txtInvPmntId5 = new System.Windows.Forms.TextBox();
            this.txtChequeInfoId5 = new System.Windows.Forms.TextBox();
            this.dtpPmntType5 = new System.Windows.Forms.DateTimePicker();
            this.cmbPmntType5 = new System.Windows.Forms.ComboBox();
            this.numPmnt5 = new System.Windows.Forms.NumericUpDown();
            this.btnSaveAndPrint = new System.Windows.Forms.Button();
            this.pnlDues = new System.Windows.Forms.Panel();
            this.pnlDue = new System.Windows.Forms.Panel();
            this.pnlAdvance = new System.Windows.Forms.Panel();
            this.btnAddNewProcedure = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.numModifiedDiscount = new System.Windows.Forms.NumericUpDown();
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
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt1)).BeginInit();
            this.pnlPmnt1.SuspendLayout();
            this.pnlPmnt2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt2)).BeginInit();
            this.pnlPmnt3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt3)).BeginInit();
            this.pnlLoadedInvoiceData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).BeginInit();
            this.pnlRefund.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSuggestion)).BeginInit();
            this.pnlAddItem.SuspendLayout();
            this.pnlPatientDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt4)).BeginInit();
            this.pnlPmnt4.SuspendLayout();
            this.pnlPmnt5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt5)).BeginInit();
            this.pnlDues.SuspendLayout();
            this.pnlDue.SuspendLayout();
            this.pnlAdvance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numModifiedDiscount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1070, 76);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(46, 25);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAddItem.FlatAppearance.BorderSize = 0;
            this.btnAddItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Location = new System.Drawing.Point(993, 76);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(75, 25);
            this.btnAddItem.TabIndex = 9;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(635, 54);
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
            this.numQuantity.Size = new System.Drawing.Size(120, 20);
            this.numQuantity.TabIndex = 5;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            this.numQuantity.Enter += new System.EventHandler(this.numQuantity_Enter);
            this.numQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numQuantity_KeyDown);
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(632, 38);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(46, 13);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity";
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdItems.ColumnHeadersHeight = 35;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceItemID,
            this.colProcedureId,
            this.colProcedureName,
            this.colDescription,
            this.colRate,
            this.colQuantity,
            this.colAmount,
            this.colNetAmount,
            this.colDiscount,
            this.colDiscountType,
            this.colDiscountTypeId,
            this.colIsNewRow,
            this.colReturnQuantity,
            this.colReturnQtyInitial,
            this.colEdit,
            this.colRemove,
            this.colReturnToggle});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(15, 178);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1215, 224);
            this.grdItems.TabIndex = 3;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            this.grdItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellValueChanged);
            this.grdItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdItems_EditingControlShowing);
            this.grdItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdItems_RowsAdded);
            this.grdItems.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdItems_RowsRemoved);
            // 
            // colInvoiceItemID
            // 
            this.colInvoiceItemID.HeaderText = "Invoice Item Id";
            this.colInvoiceItemID.Name = "colInvoiceItemID";
            this.colInvoiceItemID.ReadOnly = true;
            // 
            // colProcedureId
            // 
            this.colProcedureId.HeaderText = "ItemId";
            this.colProcedureId.Name = "colProcedureId";
            this.colProcedureId.ReadOnly = true;
            this.colProcedureId.Visible = false;
            // 
            // colProcedureName
            // 
            this.colProcedureName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProcedureName.HeaderText = "Procedure Name";
            this.colProcedureName.Name = "colProcedureName";
            this.colProcedureName.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colRate
            // 
            this.colRate.HeaderText = "Rate";
            this.colRate.Name = "colRate";
            this.colRate.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            // 
            // colAmount
            // 
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // colNetAmount
            // 
            this.colNetAmount.HeaderText = "Net Amount";
            this.colNetAmount.Name = "colNetAmount";
            this.colNetAmount.ReadOnly = true;
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            // 
            // colDiscountType
            // 
            this.colDiscountType.HeaderText = "Discount Type";
            this.colDiscountType.Name = "colDiscountType";
            this.colDiscountType.ReadOnly = true;
            // 
            // colDiscountTypeId
            // 
            this.colDiscountTypeId.HeaderText = "DiscountTypeId";
            this.colDiscountTypeId.Name = "colDiscountTypeId";
            this.colDiscountTypeId.ReadOnly = true;
            this.colDiscountTypeId.Visible = false;
            // 
            // colIsNewRow
            // 
            this.colIsNewRow.HeaderText = "IsNewRow";
            this.colIsNewRow.Name = "colIsNewRow";
            this.colIsNewRow.ReadOnly = true;
            this.colIsNewRow.Visible = false;
            // 
            // colReturnQuantity
            // 
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.colReturnQuantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.colReturnQuantity.HeaderText = "Return";
            this.colReturnQuantity.Name = "colReturnQuantity";
            this.colReturnQuantity.Visible = false;
            // 
            // colReturnQtyInitial
            // 
            this.colReturnQtyInitial.HeaderText = "Initial Return";
            this.colReturnQtyInitial.Name = "colReturnQtyInitial";
            this.colReturnQtyInitial.ReadOnly = true;
            this.colReturnQtyInitial.Visible = false;
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
            // colReturnToggle
            // 
            this.colReturnToggle.HeaderText = "Return";
            this.colReturnToggle.Name = "colReturnToggle";
            this.colReturnToggle.ReadOnly = true;
            this.colReturnToggle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colReturnToggle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colReturnToggle.Text = "Return";
            this.colReturnToggle.UseColumnTextForButtonValue = true;
            this.colReturnToggle.Visible = false;
            this.colReturnToggle.Width = 60;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClearAll.FlatAppearance.BorderSize = 0;
            this.btnClearAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAll.ForeColor = System.Drawing.Color.White;
            this.btnClearAll.Location = new System.Drawing.Point(1148, 589);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(80, 36);
            this.btnClearAll.TabIndex = 20;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(970, 589);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 36);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ErrSelectItem
            // 
            this.ErrSelectItem.AutoSize = true;
            this.ErrSelectItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectItem.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectItem.Location = new System.Drawing.Point(200, 37);
            this.ErrSelectItem.MaximumSize = new System.Drawing.Size(15, 15);
            this.ErrSelectItem.MinimumSize = new System.Drawing.Size(15, 15);
            this.ErrSelectItem.Name = "ErrSelectItem";
            this.ErrSelectItem.Size = new System.Drawing.Size(15, 15);
            this.ErrSelectItem.TabIndex = 47;
            this.ErrSelectItem.Text = "*";
            this.ErrSelectItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrSelectItem.Visible = false;
            // 
            // ErrSelectBatch
            // 
            this.ErrSelectBatch.AutoSize = true;
            this.ErrSelectBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectBatch.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectBatch.Location = new System.Drawing.Point(491, 37);
            this.ErrSelectBatch.MaximumSize = new System.Drawing.Size(15, 15);
            this.ErrSelectBatch.MinimumSize = new System.Drawing.Size(15, 15);
            this.ErrSelectBatch.Name = "ErrSelectBatch";
            this.ErrSelectBatch.Size = new System.Drawing.Size(15, 15);
            this.ErrSelectBatch.TabIndex = 48;
            this.ErrSelectBatch.Text = "*";
            this.ErrSelectBatch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrSelectBatch.Visible = false;
            // 
            // ErrQuantity
            // 
            this.ErrQuantity.AutoSize = true;
            this.ErrQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrQuantity.ForeColor = System.Drawing.Color.Red;
            this.ErrQuantity.Location = new System.Drawing.Point(740, 34);
            this.ErrQuantity.MaximumSize = new System.Drawing.Size(15, 15);
            this.ErrQuantity.MinimumSize = new System.Drawing.Size(15, 15);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(15, 15);
            this.ErrQuantity.TabIndex = 49;
            this.ErrQuantity.Text = "*";
            this.ErrQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrQuantity.Visible = false;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(5, 16);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 51;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(508, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Rate";
            // 
            // numRate
            // 
            this.numRate.DecimalPlaces = 2;
            this.numRate.Location = new System.Drawing.Point(511, 53);
            this.numRate.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.numRate.Name = "numRate";
            this.numRate.Size = new System.Drawing.Size(120, 20);
            this.numRate.TabIndex = 4;
            this.numRate.ValueChanged += new System.EventHandler(this.numRate_ValueChanged);
            this.numRate.Enter += new System.EventHandler(this.numRate_Enter);
            this.numRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numRate_KeyDown);
            // 
            // errRate
            // 
            this.errRate.AutoSize = true;
            this.errRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errRate.ForeColor = System.Drawing.Color.Red;
            this.errRate.Location = new System.Drawing.Point(616, 37);
            this.errRate.MaximumSize = new System.Drawing.Size(15, 15);
            this.errRate.MinimumSize = new System.Drawing.Size(15, 15);
            this.errRate.Name = "errRate";
            this.errRate.Size = new System.Drawing.Size(15, 15);
            this.errRate.TabIndex = 48;
            this.errRate.Text = "*";
            this.errRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errRate.Visible = false;
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Location = new System.Drawing.Point(759, 53);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(123, 20);
            this.txtAmount.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(759, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Amount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(883, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Discount";
            // 
            // cmbDiscountType
            // 
            this.cmbDiscountType.FormattingEnabled = true;
            this.cmbDiscountType.Items.AddRange(new object[] {
            "Value",
            "%"});
            this.cmbDiscountType.Location = new System.Drawing.Point(1009, 52);
            this.cmbDiscountType.Name = "cmbDiscountType";
            this.cmbDiscountType.Size = new System.Drawing.Size(107, 21);
            this.cmbDiscountType.TabIndex = 8;
            this.cmbDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbDiscountType_SelectedIndexChanged);
            this.cmbDiscountType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbDiscountType_KeyDown);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(394, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Current Refund";
            // 
            // numTotalDiscount
            // 
            this.numTotalDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numTotalDiscount.Location = new System.Drawing.Point(693, 408);
            this.numTotalDiscount.Maximum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            0});
            this.numTotalDiscount.Name = "numTotalDiscount";
            this.numTotalDiscount.Size = new System.Drawing.Size(120, 20);
            this.numTotalDiscount.TabIndex = 5;
            this.numTotalDiscount.ValueChanged += new System.EventHandler(this.numTotalDiscount_ValueChanged);
            this.numTotalDiscount.Enter += new System.EventHandler(this.numTotalDiscount_Enter);
            this.numTotalDiscount.Leave += new System.EventHandler(this.numTotalDiscount_Leave);
            // 
            // cmbTotalDiscountType
            // 
            this.cmbTotalDiscountType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTotalDiscountType.FormattingEnabled = true;
            this.cmbTotalDiscountType.Items.AddRange(new object[] {
            "Value",
            "%"});
            this.cmbTotalDiscountType.Location = new System.Drawing.Point(819, 408);
            this.cmbTotalDiscountType.Name = "cmbTotalDiscountType";
            this.cmbTotalDiscountType.Size = new System.Drawing.Size(107, 21);
            this.cmbTotalDiscountType.TabIndex = 6;
            this.cmbTotalDiscountType.SelectedIndexChanged += new System.EventHandler(this.cmbTotalDiscountType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1046, 451);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Sub-Total:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1046, 473);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Grand Total:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1121, 452);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Rs.";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1121, 473);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Rs.";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Location = new System.Drawing.Point(1150, 452);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(13, 13);
            this.lblSubTotal.TabIndex = 2;
            this.lblSubTotal.Text = "0";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Location = new System.Drawing.Point(1149, 473);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(13, 13);
            this.lblGrandTotal.TabIndex = 2;
            this.lblGrandTotal.Text = "0";
            // 
            // dtpPmntType1
            // 
            this.dtpPmntType1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPmntType1.Location = new System.Drawing.Point(91, 3);
            this.dtpPmntType1.Name = "dtpPmntType1";
            this.dtpPmntType1.Size = new System.Drawing.Size(198, 20);
            this.dtpPmntType1.TabIndex = 3;
            // 
            // cmbPmntType1
            // 
            this.cmbPmntType1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPmntType1.FormattingEnabled = true;
            this.cmbPmntType1.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.cmbPmntType1.Location = new System.Drawing.Point(295, 3);
            this.cmbPmntType1.Name = "cmbPmntType1";
            this.cmbPmntType1.Size = new System.Drawing.Size(175, 21);
            this.cmbPmntType1.TabIndex = 2;
            this.cmbPmntType1.SelectedIndexChanged += new System.EventHandler(this.cmbPmntType1_SelectedIndexChanged);
            // 
            // numPmnt1
            // 
            this.numPmnt1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPmnt1.Location = new System.Drawing.Point(476, 3);
            this.numPmnt1.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.numPmnt1.Minimum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            -2147483648});
            this.numPmnt1.Name = "numPmnt1";
            this.numPmnt1.Size = new System.Drawing.Size(120, 20);
            this.numPmnt1.TabIndex = 1;
            this.numPmnt1.ValueChanged += new System.EventHandler(this.Payments_ValueChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(1044, 495);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Payment:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(3, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(61, 13);
            this.label18.TabIndex = 60;
            this.label18.Text = "Advance:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(3, 4);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 61;
            this.label19.Text = "Due:";
            // 
            // lblAdvance
            // 
            this.lblAdvance.AutoSize = true;
            this.lblAdvance.Location = new System.Drawing.Point(107, 3);
            this.lblAdvance.Name = "lblAdvance";
            this.lblAdvance.Size = new System.Drawing.Size(13, 13);
            this.lblAdvance.TabIndex = 62;
            this.lblAdvance.Text = "0";
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.Location = new System.Drawing.Point(107, 7);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(13, 13);
            this.lblDue.TabIndex = 63;
            this.lblDue.Text = "0";
            this.lblDue.TextChanged += new System.EventHandler(this.lblDue_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(80, 3);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(23, 13);
            this.label22.TabIndex = 64;
            this.label22.Text = "Rs.";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(80, 7);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(23, 13);
            this.label23.TabIndex = 65;
            this.label23.Text = "Rs.";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1099, 496);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 13);
            this.label11.TabIndex = 65;
            this.label11.Text = "Rs.";
            // 
            // pnlPmnt1
            // 
            this.pnlPmnt1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPmnt1.Controls.Add(this.txtInvPmntId1);
            this.pnlPmnt1.Controls.Add(this.txtChequeInfoId1);
            this.pnlPmnt1.Controls.Add(this.dtpPmntType1);
            this.pnlPmnt1.Controls.Add(this.cmbPmntType1);
            this.pnlPmnt1.Controls.Add(this.numPmnt1);
            this.pnlPmnt1.Location = new System.Drawing.Point(319, 433);
            this.pnlPmnt1.Name = "pnlPmnt1";
            this.pnlPmnt1.Size = new System.Drawing.Size(608, 27);
            this.pnlPmnt1.TabIndex = 8;
            // 
            // txtInvPmntId1
            // 
            this.txtInvPmntId1.Location = new System.Drawing.Point(10, 4);
            this.txtInvPmntId1.Name = "txtInvPmntId1";
            this.txtInvPmntId1.Size = new System.Drawing.Size(34, 20);
            this.txtInvPmntId1.TabIndex = 78;
            this.txtInvPmntId1.Text = "0";
            this.txtInvPmntId1.Visible = false;
            // 
            // txtChequeInfoId1
            // 
            this.txtChequeInfoId1.Location = new System.Drawing.Point(50, 4);
            this.txtChequeInfoId1.Name = "txtChequeInfoId1";
            this.txtChequeInfoId1.Size = new System.Drawing.Size(34, 20);
            this.txtChequeInfoId1.TabIndex = 78;
            this.txtChequeInfoId1.Visible = false;
            // 
            // pnlPmnt2
            // 
            this.pnlPmnt2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPmnt2.Controls.Add(this.txtInvPmntId2);
            this.pnlPmnt2.Controls.Add(this.txtChequeInfoId2);
            this.pnlPmnt2.Controls.Add(this.dtpPmntType2);
            this.pnlPmnt2.Controls.Add(this.cmbPmntType2);
            this.pnlPmnt2.Controls.Add(this.numPmnt2);
            this.pnlPmnt2.Location = new System.Drawing.Point(319, 462);
            this.pnlPmnt2.Name = "pnlPmnt2";
            this.pnlPmnt2.Size = new System.Drawing.Size(608, 27);
            this.pnlPmnt2.TabIndex = 10;
            this.pnlPmnt2.Visible = false;
            // 
            // txtInvPmntId2
            // 
            this.txtInvPmntId2.Location = new System.Drawing.Point(10, 5);
            this.txtInvPmntId2.Name = "txtInvPmntId2";
            this.txtInvPmntId2.Size = new System.Drawing.Size(34, 20);
            this.txtInvPmntId2.TabIndex = 78;
            this.txtInvPmntId2.Text = "0";
            this.txtInvPmntId2.Visible = false;
            // 
            // txtChequeInfoId2
            // 
            this.txtChequeInfoId2.Location = new System.Drawing.Point(50, 5);
            this.txtChequeInfoId2.Name = "txtChequeInfoId2";
            this.txtChequeInfoId2.Size = new System.Drawing.Size(34, 20);
            this.txtChequeInfoId2.TabIndex = 78;
            this.txtChequeInfoId2.Visible = false;
            // 
            // dtpPmntType2
            // 
            this.dtpPmntType2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPmntType2.Location = new System.Drawing.Point(90, 3);
            this.dtpPmntType2.Name = "dtpPmntType2";
            this.dtpPmntType2.Size = new System.Drawing.Size(199, 20);
            this.dtpPmntType2.TabIndex = 3;
            // 
            // cmbPmntType2
            // 
            this.cmbPmntType2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPmntType2.FormattingEnabled = true;
            this.cmbPmntType2.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.cmbPmntType2.Location = new System.Drawing.Point(295, 3);
            this.cmbPmntType2.Name = "cmbPmntType2";
            this.cmbPmntType2.Size = new System.Drawing.Size(175, 21);
            this.cmbPmntType2.TabIndex = 2;
            this.cmbPmntType2.SelectedIndexChanged += new System.EventHandler(this.cmbPmntType2_SelectedIndexChanged);
            // 
            // numPmnt2
            // 
            this.numPmnt2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPmnt2.Location = new System.Drawing.Point(476, 3);
            this.numPmnt2.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.numPmnt2.Minimum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            -2147483648});
            this.numPmnt2.Name = "numPmnt2";
            this.numPmnt2.Size = new System.Drawing.Size(120, 20);
            this.numPmnt2.TabIndex = 1;
            this.numPmnt2.ValueChanged += new System.EventHandler(this.Payments_ValueChanged);
            // 
            // pnlPmnt3
            // 
            this.pnlPmnt3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPmnt3.Controls.Add(this.txtInvPmntId3);
            this.pnlPmnt3.Controls.Add(this.txtChequeInfoId3);
            this.pnlPmnt3.Controls.Add(this.dtpPmntType3);
            this.pnlPmnt3.Controls.Add(this.cmbPmntType3);
            this.pnlPmnt3.Controls.Add(this.numPmnt3);
            this.pnlPmnt3.Location = new System.Drawing.Point(319, 492);
            this.pnlPmnt3.Name = "pnlPmnt3";
            this.pnlPmnt3.Size = new System.Drawing.Size(608, 27);
            this.pnlPmnt3.TabIndex = 12;
            this.pnlPmnt3.Visible = false;
            // 
            // txtInvPmntId3
            // 
            this.txtInvPmntId3.Location = new System.Drawing.Point(10, 3);
            this.txtInvPmntId3.Name = "txtInvPmntId3";
            this.txtInvPmntId3.Size = new System.Drawing.Size(34, 20);
            this.txtInvPmntId3.TabIndex = 78;
            this.txtInvPmntId3.Text = "0";
            this.txtInvPmntId3.Visible = false;
            // 
            // txtChequeInfoId3
            // 
            this.txtChequeInfoId3.Location = new System.Drawing.Point(50, 3);
            this.txtChequeInfoId3.Name = "txtChequeInfoId3";
            this.txtChequeInfoId3.Size = new System.Drawing.Size(34, 20);
            this.txtChequeInfoId3.TabIndex = 78;
            this.txtChequeInfoId3.Visible = false;
            // 
            // dtpPmntType3
            // 
            this.dtpPmntType3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPmntType3.Location = new System.Drawing.Point(90, 3);
            this.dtpPmntType3.Name = "dtpPmntType3";
            this.dtpPmntType3.Size = new System.Drawing.Size(199, 20);
            this.dtpPmntType3.TabIndex = 3;
            // 
            // cmbPmntType3
            // 
            this.cmbPmntType3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPmntType3.FormattingEnabled = true;
            this.cmbPmntType3.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.cmbPmntType3.Location = new System.Drawing.Point(295, 3);
            this.cmbPmntType3.Name = "cmbPmntType3";
            this.cmbPmntType3.Size = new System.Drawing.Size(175, 21);
            this.cmbPmntType3.TabIndex = 2;
            this.cmbPmntType3.SelectedIndexChanged += new System.EventHandler(this.cmbPmntType3_SelectedIndexChanged);
            // 
            // numPmnt3
            // 
            this.numPmnt3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPmnt3.Location = new System.Drawing.Point(476, 3);
            this.numPmnt3.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.numPmnt3.Minimum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            -2147483648});
            this.numPmnt3.Name = "numPmnt3";
            this.numPmnt3.Size = new System.Drawing.Size(120, 20);
            this.numPmnt3.TabIndex = 1;
            this.numPmnt3.ValueChanged += new System.EventHandler(this.Payments_ValueChanged);
            // 
            // chkPmnt1
            // 
            this.chkPmnt1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPmnt1.AutoSize = true;
            this.chkPmnt1.Checked = true;
            this.chkPmnt1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPmnt1.Enabled = false;
            this.chkPmnt1.Location = new System.Drawing.Point(933, 439);
            this.chkPmnt1.Name = "chkPmnt1";
            this.chkPmnt1.Size = new System.Drawing.Size(15, 14);
            this.chkPmnt1.TabIndex = 7;
            this.chkPmnt1.UseVisualStyleBackColor = true;
            // 
            // chkPmnt2
            // 
            this.chkPmnt2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPmnt2.AutoSize = true;
            this.chkPmnt2.Location = new System.Drawing.Point(933, 467);
            this.chkPmnt2.Name = "chkPmnt2";
            this.chkPmnt2.Size = new System.Drawing.Size(89, 17);
            this.chkPmnt2.TabIndex = 9;
            this.chkPmnt2.Text = "Add Payment";
            this.chkPmnt2.UseVisualStyleBackColor = true;
            this.chkPmnt2.CheckedChanged += new System.EventHandler(this.chkPmnt2_CheckedChanged);
            // 
            // chkPmnt3
            // 
            this.chkPmnt3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPmnt3.AutoSize = true;
            this.chkPmnt3.Location = new System.Drawing.Point(933, 496);
            this.chkPmnt3.Name = "chkPmnt3";
            this.chkPmnt3.Size = new System.Drawing.Size(89, 17);
            this.chkPmnt3.TabIndex = 11;
            this.chkPmnt3.Text = "Add Payment";
            this.chkPmnt3.UseVisualStyleBackColor = true;
            this.chkPmnt3.Visible = false;
            this.chkPmnt3.CheckedChanged += new System.EventHandler(this.chkPmnt3_CheckedChanged);
            // 
            // btnAddPatient
            // 
            this.btnAddPatient.Location = new System.Drawing.Point(924, 31);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(75, 26);
            this.btnAddPatient.TabIndex = 79;
            this.btnAddPatient.Text = "Add Patient";
            this.btnAddPatient.UseVisualStyleBackColor = true;
            this.btnAddPatient.Visible = false;
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
            // 
            // txtSearchPatient
            // 
            this.txtSearchPatient.Location = new System.Drawing.Point(722, 35);
            this.txtSearchPatient.Name = "txtSearchPatient";
            this.txtSearchPatient.Size = new System.Drawing.Size(201, 20);
            this.txtSearchPatient.TabIndex = 81;
            this.txtSearchPatient.Visible = false;
            this.txtSearchPatient.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchPatient_KeyDown);
            this.txtSearchPatient.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearchPatient_MouseDoubleClick);
            // 
            // pnlLoadedInvoiceData
            // 
            this.pnlLoadedInvoiceData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoadedInvoiceData.Controls.Add(this.label13);
            this.pnlLoadedInvoiceData.Controls.Add(this.lblInvoiceNo);
            this.pnlLoadedInvoiceData.Location = new System.Drawing.Point(1030, 9);
            this.pnlLoadedInvoiceData.Name = "pnlLoadedInvoiceData";
            this.pnlLoadedInvoiceData.Size = new System.Drawing.Size(176, 43);
            this.pnlLoadedInvoiceData.TabIndex = 83;
            this.pnlLoadedInvoiceData.Visible = false;
            this.pnlLoadedInvoiceData.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLoadedInvoiceData_Paint);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 17);
            this.label13.TabIndex = 2;
            this.label13.Text = "Invoice#";
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceNo.Location = new System.Drawing.Point(76, 17);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(0, 17);
            this.lblInvoiceNo.TabIndex = 2;
            this.lblInvoiceNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numDiscount
            // 
            this.numDiscount.Location = new System.Drawing.Point(886, 53);
            this.numDiscount.Name = "numDiscount";
            this.numDiscount.Size = new System.Drawing.Size(120, 20);
            this.numDiscount.TabIndex = 7;
            this.numDiscount.ValueChanged += new System.EventHandler(this.numDiscount_ValueChanged);
            this.numDiscount.Enter += new System.EventHandler(this.numDiscount_Enter);
            this.numDiscount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numDiscount_KeyDown);
            // 
            // pnlRefund
            // 
            this.pnlRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRefund.Controls.Add(this.txtCurrentRefund);
            this.pnlRefund.Controls.Add(this.txtRefundAmount);
            this.pnlRefund.Controls.Add(this.dtpRedundDate);
            this.pnlRefund.Controls.Add(this.txtRefundReason);
            this.pnlRefund.Controls.Add(this.label15);
            this.pnlRefund.Controls.Add(this.label21);
            this.pnlRefund.Controls.Add(this.label5);
            this.pnlRefund.Location = new System.Drawing.Point(319, 581);
            this.pnlRefund.Name = "pnlRefund";
            this.pnlRefund.Size = new System.Drawing.Size(608, 44);
            this.pnlRefund.TabIndex = 17;
            this.pnlRefund.Visible = false;
            // 
            // txtCurrentRefund
            // 
            this.txtCurrentRefund.Enabled = false;
            this.txtCurrentRefund.Location = new System.Drawing.Point(475, 2);
            this.txtCurrentRefund.Name = "txtCurrentRefund";
            this.txtCurrentRefund.Size = new System.Drawing.Size(120, 20);
            this.txtCurrentRefund.TabIndex = 54;
            // 
            // txtRefundAmount
            // 
            this.txtRefundAmount.Enabled = false;
            this.txtRefundAmount.Location = new System.Drawing.Point(475, 23);
            this.txtRefundAmount.Name = "txtRefundAmount";
            this.txtRefundAmount.Size = new System.Drawing.Size(120, 20);
            this.txtRefundAmount.TabIndex = 53;
            // 
            // dtpRedundDate
            // 
            this.dtpRedundDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpRedundDate.Location = new System.Drawing.Point(4, 20);
            this.dtpRedundDate.Name = "dtpRedundDate";
            this.dtpRedundDate.Size = new System.Drawing.Size(198, 20);
            this.dtpRedundDate.TabIndex = 2;
            // 
            // txtRefundReason
            // 
            this.txtRefundReason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRefundReason.Location = new System.Drawing.Point(208, 21);
            this.txtRefundReason.Name = "txtRefundReason";
            this.txtRefundReason.Size = new System.Drawing.Size(174, 20);
            this.txtRefundReason.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(208, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Reason For Refund";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(394, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 13);
            this.label21.TabIndex = 2;
            this.label21.Text = "Total Refund";
            // 
            // txtInvoiceNote
            // 
            this.txtInvoiceNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtInvoiceNote.Location = new System.Drawing.Point(21, 462);
            this.txtInvoiceNote.Multiline = true;
            this.txtInvoiceNote.Name = "txtInvoiceNote";
            this.txtInvoiceNote.Size = new System.Drawing.Size(382, 117);
            this.txtInvoiceNote.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 446);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Invoice Note";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(719, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(165, 13);
            this.label12.TabIndex = 80;
            this.label12.Text = "Search  by Name, MR# or Phone";
            this.label12.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 87;
            this.label16.Text = "Name";
            // 
            // grdSuggestion
            // 
            this.grdSuggestion.AllowUserToAddRows = false;
            this.grdSuggestion.AllowUserToDeleteRows = false;
            this.grdSuggestion.AllowUserToResizeColumns = false;
            this.grdSuggestion.AllowUserToResizeRows = false;
            this.grdSuggestion.BackgroundColor = System.Drawing.Color.White;
            this.grdSuggestion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.grdSuggestion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSuggestion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPatientID,
            this.colMrNo,
            this.colPatient,
            this.colPhone});
            this.grdSuggestion.GridColor = System.Drawing.Color.Silver;
            this.grdSuggestion.Location = new System.Drawing.Point(727, 50);
            this.grdSuggestion.Name = "grdSuggestion";
            this.grdSuggestion.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.grdSuggestion.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.grdSuggestion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSuggestion.Size = new System.Drawing.Size(317, 147);
            this.grdSuggestion.TabIndex = 88;
            this.grdSuggestion.Visible = false;
            this.grdSuggestion.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSuggestion_CellDoubleClick);
            this.grdSuggestion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdSuggestion_KeyDown);
            this.grdSuggestion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdSuggestion_KeyPress);
            // 
            // colPatientID
            // 
            this.colPatientID.HeaderText = "PatientId";
            this.colPatientID.Name = "colPatientID";
            this.colPatientID.ReadOnly = true;
            this.colPatientID.Visible = false;
            // 
            // colMrNo
            // 
            this.colMrNo.HeaderText = "MR#";
            this.colMrNo.Name = "colMrNo";
            this.colMrNo.ReadOnly = true;
            this.colMrNo.Width = 60;
            // 
            // colPatient
            // 
            this.colPatient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPatient.HeaderText = "Customer";
            this.colPatient.Name = "colPatient";
            this.colPatient.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "PHONE";
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // txtSearchByName
            // 
            this.txtSearchByName.Location = new System.Drawing.Point(9, 35);
            this.txtSearchByName.Name = "txtSearchByName";
            this.txtSearchByName.Size = new System.Drawing.Size(213, 20);
            this.txtSearchByName.TabIndex = 101;
            this.txtSearchByName.TextChanged += new System.EventHandler(this.txtSearchByName_TextChanged);
            this.txtSearchByName.Enter += new System.EventHandler(this.txtSearchByName_Enter);
            this.txtSearchByName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchByName_KeyDown);
            this.txtSearchByName.Leave += new System.EventHandler(this.txtSearchByName_Leave);
            // 
            // txtSearchByPhone
            // 
            this.txtSearchByPhone.Location = new System.Drawing.Point(226, 35);
            this.txtSearchByPhone.Name = "txtSearchByPhone";
            this.txtSearchByPhone.Size = new System.Drawing.Size(213, 20);
            this.txtSearchByPhone.TabIndex = 102;
            this.txtSearchByPhone.TextChanged += new System.EventHandler(this.txtSearchByPhone_TextChanged);
            this.txtSearchByPhone.Enter += new System.EventHandler(this.txtSearchByPhone_Enter);
            this.txtSearchByPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchByPhone_KeyDown);
            this.txtSearchByPhone.Leave += new System.EventHandler(this.txtSearchByPhone_Leave);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(223, 14);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 89;
            this.label17.Text = "Phone";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(443, 35);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(213, 20);
            this.txtAddress.TabIndex = 103;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(440, 14);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(45, 13);
            this.label20.TabIndex = 91;
            this.label20.Text = "Address";
            // 
            // pnlAddItem
            // 
            this.pnlAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAddItem.Controls.Add(this.txtDescription);
            this.pnlAddItem.Controls.Add(this.lblSelectBatch);
            this.pnlAddItem.Controls.Add(this.label1);
            this.pnlAddItem.Controls.Add(this.cmbProcedure);
            this.pnlAddItem.Controls.Add(this.Errdisc);
            this.pnlAddItem.Controls.Add(this.txtNetAmount);
            this.pnlAddItem.Controls.Add(this.txtAvlQty);
            this.pnlAddItem.Controls.Add(this.label24);
            this.pnlAddItem.Controls.Add(this.btnClear);
            this.pnlAddItem.Controls.Add(this.btnAddItem);
            this.pnlAddItem.Controls.Add(this.numDiscount);
            this.pnlAddItem.Controls.Add(this.ErrMessage);
            this.pnlAddItem.Controls.Add(this.cmbDiscountType);
            this.pnlAddItem.Controls.Add(this.txtAmount);
            this.pnlAddItem.Controls.Add(this.ErrQuantity);
            this.pnlAddItem.Controls.Add(this.errRate);
            this.pnlAddItem.Controls.Add(this.ErrSelectBatch);
            this.pnlAddItem.Controls.Add(this.ErrSelectItem);
            this.pnlAddItem.Controls.Add(this.numRate);
            this.pnlAddItem.Controls.Add(this.lblQuantity);
            this.pnlAddItem.Controls.Add(this.numQuantity);
            this.pnlAddItem.Controls.Add(this.label3);
            this.pnlAddItem.Controls.Add(this.label2);
            this.pnlAddItem.Controls.Add(this.label4);
            this.pnlAddItem.Location = new System.Drawing.Point(15, 64);
            this.pnlAddItem.Name = "pnlAddItem";
            this.pnlAddItem.Size = new System.Drawing.Size(1215, 108);
            this.pnlAddItem.TabIndex = 1;
            this.pnlAddItem.TabStop = false;
            this.pnlAddItem.Text = "Item Details";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(221, 53);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(283, 20);
            this.txtDescription.TabIndex = 60;
            // 
            // lblSelectBatch
            // 
            this.lblSelectBatch.AutoSize = true;
            this.lblSelectBatch.Location = new System.Drawing.Point(218, 36);
            this.lblSelectBatch.Name = "lblSelectBatch";
            this.lblSelectBatch.Size = new System.Drawing.Size(60, 13);
            this.lblSelectBatch.TabIndex = 61;
            this.lblSelectBatch.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Select Procedure";
            // 
            // cmbProcedure
            // 
            this.cmbProcedure.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbProcedure.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbProcedure.DropDownHeight = 120;
            this.cmbProcedure.FormattingEnabled = true;
            this.cmbProcedure.IntegralHeight = false;
            this.cmbProcedure.Location = new System.Drawing.Point(5, 53);
            this.cmbProcedure.Name = "cmbProcedure";
            this.cmbProcedure.Size = new System.Drawing.Size(210, 21);
            this.cmbProcedure.TabIndex = 58;
            this.cmbProcedure.SelectedIndexChanged += new System.EventHandler(this.cmbProcedure_SelectedIndexChanged);
            // 
            // Errdisc
            // 
            this.Errdisc.AutoSize = true;
            this.Errdisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Errdisc.ForeColor = System.Drawing.Color.Red;
            this.Errdisc.Location = new System.Drawing.Point(991, 35);
            this.Errdisc.MaximumSize = new System.Drawing.Size(15, 15);
            this.Errdisc.MinimumSize = new System.Drawing.Size(15, 15);
            this.Errdisc.Name = "Errdisc";
            this.Errdisc.Size = new System.Drawing.Size(15, 15);
            this.Errdisc.TabIndex = 57;
            this.Errdisc.Text = "*";
            this.Errdisc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Errdisc.Visible = false;
            // 
            // txtNetAmount
            // 
            this.txtNetAmount.Enabled = false;
            this.txtNetAmount.Location = new System.Drawing.Point(759, 76);
            this.txtNetAmount.Name = "txtNetAmount";
            this.txtNetAmount.Size = new System.Drawing.Size(123, 20);
            this.txtNetAmount.TabIndex = 56;
            // 
            // txtAvlQty
            // 
            this.txtAvlQty.Enabled = false;
            this.txtAvlQty.Location = new System.Drawing.Point(393, 21);
            this.txtAvlQty.Name = "txtAvlQty";
            this.txtAvlQty.Size = new System.Drawing.Size(90, 20);
            this.txtAvlQty.TabIndex = 3;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(392, 4);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(92, 13);
            this.label24.TabIndex = 54;
            this.label24.Text = "Available Quantity";
            // 
            // pnlPatientDetails
            // 
            this.pnlPatientDetails.BackColor = System.Drawing.Color.PaleTurquoise;
            this.pnlPatientDetails.Controls.Add(this.txtSearchByPhone);
            this.pnlPatientDetails.Controls.Add(this.btnAddPatient);
            this.pnlPatientDetails.Controls.Add(this.txtAddress);
            this.pnlPatientDetails.Controls.Add(this.label12);
            this.pnlPatientDetails.Controls.Add(this.label20);
            this.pnlPatientDetails.Controls.Add(this.txtSearchPatient);
            this.pnlPatientDetails.Controls.Add(this.label16);
            this.pnlPatientDetails.Controls.Add(this.label17);
            this.pnlPatientDetails.Controls.Add(this.txtSearchByName);
            this.pnlPatientDetails.Location = new System.Drawing.Point(15, 3);
            this.pnlPatientDetails.Name = "pnlPatientDetails";
            this.pnlPatientDetails.Size = new System.Drawing.Size(707, 62);
            this.pnlPatientDetails.TabIndex = 100;
            this.pnlPatientDetails.TabStop = false;
            this.pnlPatientDetails.Text = "Patient Details";
            // 
            // chkPmnt4
            // 
            this.chkPmnt4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPmnt4.AutoSize = true;
            this.chkPmnt4.Location = new System.Drawing.Point(933, 527);
            this.chkPmnt4.Name = "chkPmnt4";
            this.chkPmnt4.Size = new System.Drawing.Size(89, 17);
            this.chkPmnt4.TabIndex = 13;
            this.chkPmnt4.Text = "Add Payment";
            this.chkPmnt4.UseVisualStyleBackColor = true;
            this.chkPmnt4.Visible = false;
            this.chkPmnt4.CheckedChanged += new System.EventHandler(this.chkPmnt4_CheckedChanged);
            // 
            // numPmnt4
            // 
            this.numPmnt4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPmnt4.Location = new System.Drawing.Point(476, 3);
            this.numPmnt4.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.numPmnt4.Minimum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            -2147483648});
            this.numPmnt4.Name = "numPmnt4";
            this.numPmnt4.Size = new System.Drawing.Size(120, 20);
            this.numPmnt4.TabIndex = 1;
            this.numPmnt4.ValueChanged += new System.EventHandler(this.Payments_ValueChanged);
            // 
            // cmbPmntType4
            // 
            this.cmbPmntType4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPmntType4.FormattingEnabled = true;
            this.cmbPmntType4.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.cmbPmntType4.Location = new System.Drawing.Point(295, 3);
            this.cmbPmntType4.Name = "cmbPmntType4";
            this.cmbPmntType4.Size = new System.Drawing.Size(175, 21);
            this.cmbPmntType4.TabIndex = 2;
            this.cmbPmntType4.SelectedIndexChanged += new System.EventHandler(this.cmbPmntType4_SelectedIndexChanged);
            // 
            // dtpPmntType4
            // 
            this.dtpPmntType4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPmntType4.Location = new System.Drawing.Point(90, 3);
            this.dtpPmntType4.Name = "dtpPmntType4";
            this.dtpPmntType4.Size = new System.Drawing.Size(199, 20);
            this.dtpPmntType4.TabIndex = 3;
            // 
            // txtChequeInfoId4
            // 
            this.txtChequeInfoId4.Location = new System.Drawing.Point(50, 3);
            this.txtChequeInfoId4.Name = "txtChequeInfoId4";
            this.txtChequeInfoId4.Size = new System.Drawing.Size(34, 20);
            this.txtChequeInfoId4.TabIndex = 78;
            this.txtChequeInfoId4.Visible = false;
            // 
            // txtInvPmntId4
            // 
            this.txtInvPmntId4.Location = new System.Drawing.Point(10, 3);
            this.txtInvPmntId4.Name = "txtInvPmntId4";
            this.txtInvPmntId4.Size = new System.Drawing.Size(34, 20);
            this.txtInvPmntId4.TabIndex = 78;
            this.txtInvPmntId4.Text = "0";
            this.txtInvPmntId4.Visible = false;
            // 
            // pnlPmnt4
            // 
            this.pnlPmnt4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPmnt4.Controls.Add(this.txtInvPmntId4);
            this.pnlPmnt4.Controls.Add(this.txtChequeInfoId4);
            this.pnlPmnt4.Controls.Add(this.dtpPmntType4);
            this.pnlPmnt4.Controls.Add(this.cmbPmntType4);
            this.pnlPmnt4.Controls.Add(this.numPmnt4);
            this.pnlPmnt4.Location = new System.Drawing.Point(319, 523);
            this.pnlPmnt4.Name = "pnlPmnt4";
            this.pnlPmnt4.Size = new System.Drawing.Size(608, 27);
            this.pnlPmnt4.TabIndex = 14;
            this.pnlPmnt4.Visible = false;
            // 
            // chkPmnt5
            // 
            this.chkPmnt5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPmnt5.AutoSize = true;
            this.chkPmnt5.Location = new System.Drawing.Point(933, 557);
            this.chkPmnt5.Name = "chkPmnt5";
            this.chkPmnt5.Size = new System.Drawing.Size(89, 17);
            this.chkPmnt5.TabIndex = 15;
            this.chkPmnt5.Text = "Add Payment";
            this.chkPmnt5.UseVisualStyleBackColor = true;
            this.chkPmnt5.Visible = false;
            this.chkPmnt5.CheckedChanged += new System.EventHandler(this.chkPmnt5_CheckedChanged);
            // 
            // pnlPmnt5
            // 
            this.pnlPmnt5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPmnt5.Controls.Add(this.txtInvPmntId5);
            this.pnlPmnt5.Controls.Add(this.txtChequeInfoId5);
            this.pnlPmnt5.Controls.Add(this.dtpPmntType5);
            this.pnlPmnt5.Controls.Add(this.cmbPmntType5);
            this.pnlPmnt5.Controls.Add(this.numPmnt5);
            this.pnlPmnt5.Location = new System.Drawing.Point(319, 553);
            this.pnlPmnt5.Name = "pnlPmnt5";
            this.pnlPmnt5.Size = new System.Drawing.Size(608, 27);
            this.pnlPmnt5.TabIndex = 16;
            this.pnlPmnt5.Visible = false;
            // 
            // txtInvPmntId5
            // 
            this.txtInvPmntId5.Location = new System.Drawing.Point(10, 3);
            this.txtInvPmntId5.Name = "txtInvPmntId5";
            this.txtInvPmntId5.Size = new System.Drawing.Size(34, 20);
            this.txtInvPmntId5.TabIndex = 78;
            this.txtInvPmntId5.Text = "0";
            this.txtInvPmntId5.Visible = false;
            // 
            // txtChequeInfoId5
            // 
            this.txtChequeInfoId5.Location = new System.Drawing.Point(50, 3);
            this.txtChequeInfoId5.Name = "txtChequeInfoId5";
            this.txtChequeInfoId5.Size = new System.Drawing.Size(34, 20);
            this.txtChequeInfoId5.TabIndex = 78;
            this.txtChequeInfoId5.Visible = false;
            // 
            // dtpPmntType5
            // 
            this.dtpPmntType5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpPmntType5.Location = new System.Drawing.Point(90, 3);
            this.dtpPmntType5.Name = "dtpPmntType5";
            this.dtpPmntType5.Size = new System.Drawing.Size(199, 20);
            this.dtpPmntType5.TabIndex = 3;
            // 
            // cmbPmntType5
            // 
            this.cmbPmntType5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPmntType5.FormattingEnabled = true;
            this.cmbPmntType5.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.cmbPmntType5.Location = new System.Drawing.Point(295, 3);
            this.cmbPmntType5.Name = "cmbPmntType5";
            this.cmbPmntType5.Size = new System.Drawing.Size(175, 21);
            this.cmbPmntType5.TabIndex = 2;
            this.cmbPmntType5.SelectedIndexChanged += new System.EventHandler(this.cmbPmntType5_SelectedIndexChanged);
            // 
            // numPmnt5
            // 
            this.numPmnt5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPmnt5.Location = new System.Drawing.Point(476, 3);
            this.numPmnt5.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.numPmnt5.Minimum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            -2147483648});
            this.numPmnt5.Name = "numPmnt5";
            this.numPmnt5.Size = new System.Drawing.Size(120, 20);
            this.numPmnt5.TabIndex = 1;
            this.numPmnt5.ValueChanged += new System.EventHandler(this.numPmnt5_ValueChanged);
            // 
            // btnSaveAndPrint
            // 
            this.btnSaveAndPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnSaveAndPrint.FlatAppearance.BorderSize = 0;
            this.btnSaveAndPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSaveAndPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAndPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAndPrint.ForeColor = System.Drawing.Color.White;
            this.btnSaveAndPrint.Location = new System.Drawing.Point(1041, 589);
            this.btnSaveAndPrint.Name = "btnSaveAndPrint";
            this.btnSaveAndPrint.Size = new System.Drawing.Size(101, 36);
            this.btnSaveAndPrint.TabIndex = 19;
            this.btnSaveAndPrint.Text = "Save && Print";
            this.btnSaveAndPrint.UseVisualStyleBackColor = false;
            this.btnSaveAndPrint.Click += new System.EventHandler(this.btnSaveAndPrint_Click);
            // 
            // pnlDues
            // 
            this.pnlDues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDues.Controls.Add(this.pnlDue);
            this.pnlDues.Controls.Add(this.pnlAdvance);
            this.pnlDues.Location = new System.Drawing.Point(1036, 512);
            this.pnlDues.Name = "pnlDues";
            this.pnlDues.Size = new System.Drawing.Size(191, 62);
            this.pnlDues.TabIndex = 97;
            // 
            // pnlDue
            // 
            this.pnlDue.Controls.Add(this.label19);
            this.pnlDue.Controls.Add(this.label23);
            this.pnlDue.Controls.Add(this.lblDue);
            this.pnlDue.Location = new System.Drawing.Point(4, 17);
            this.pnlDue.Name = "pnlDue";
            this.pnlDue.Size = new System.Drawing.Size(187, 25);
            this.pnlDue.TabIndex = 98;
            // 
            // pnlAdvance
            // 
            this.pnlAdvance.Controls.Add(this.label18);
            this.pnlAdvance.Controls.Add(this.label22);
            this.pnlAdvance.Controls.Add(this.lblAdvance);
            this.pnlAdvance.Location = new System.Drawing.Point(4, 41);
            this.pnlAdvance.Name = "pnlAdvance";
            this.pnlAdvance.Size = new System.Drawing.Size(185, 21);
            this.pnlAdvance.TabIndex = 66;
            // 
            // btnAddNewProcedure
            // 
            this.btnAddNewProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewProcedure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAddNewProcedure.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnAddNewProcedure.FlatAppearance.BorderSize = 0;
            this.btnAddNewProcedure.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddNewProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewProcedure.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewProcedure.ForeColor = System.Drawing.Color.White;
            this.btnAddNewProcedure.Location = new System.Drawing.Point(905, 8);
            this.btnAddNewProcedure.Name = "btnAddNewProcedure";
            this.btnAddNewProcedure.Size = new System.Drawing.Size(125, 36);
            this.btnAddNewProcedure.TabIndex = 22;
            this.btnAddNewProcedure.Text = "Add Procedure";
            this.btnAddNewProcedure.UseVisualStyleBackColor = false;
            this.btnAddNewProcedure.Click += new System.EventHandler(this.btnAddNewProcedure_Click);
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1208, 1);
            this.pbInfo.Name = "pbInfo";
            this.helpProvider1.SetShowHelp(this.pbInfo, true);
            this.pbInfo.Size = new System.Drawing.Size(29, 28);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 99;
            this.pbInfo.TabStop = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // numModifiedDiscount
            // 
            this.numModifiedDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numModifiedDiscount.Location = new System.Drawing.Point(567, 408);
            this.numModifiedDiscount.Maximum = new decimal(new int[] {
            268435456,
            1042612833,
            542101086,
            0});
            this.numModifiedDiscount.Name = "numModifiedDiscount";
            this.numModifiedDiscount.Size = new System.Drawing.Size(120, 20);
            this.numModifiedDiscount.TabIndex = 5;
            this.numModifiedDiscount.Visible = false;
            this.numModifiedDiscount.ValueChanged += new System.EventHandler(this.numTotalDiscount_ValueChanged);
            this.numModifiedDiscount.Enter += new System.EventHandler(this.numTotalDiscount_Enter);
            this.numModifiedDiscount.Leave += new System.EventHandler(this.numTotalDiscount_Leave);
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
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            this.dataGridViewTextBoxColumn4.HeaderText = "BATCH";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "QUANTITY";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "ConsumptionTypeId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
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
            this.dataGridViewTextBoxColumn9.HeaderText = "DISCOUNT TYPE";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "DiscountTypeId";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "IsNewRow";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.dataGridViewTextBoxColumn12.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn12.HeaderText = "Return";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "PatientId";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.dataGridViewTextBoxColumn14.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn14.HeaderText = "Mr#";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 60;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn15.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn16.HeaderText = "Phone";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Visible = false;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.HeaderText = "PHONE";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Width = 60;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn18.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.HeaderText = "PHONE";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            // 
            // frmProcedureInvoice2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1237, 627);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.btnAddNewProcedure);
            this.Controls.Add(this.pnlDues);
            this.Controls.Add(this.chkPmnt5);
            this.Controls.Add(this.pnlPmnt5);
            this.Controls.Add(this.pnlPatientDetails);
            this.Controls.Add(this.pnlAddItem);
            this.Controls.Add(this.grdSuggestion);
            this.Controls.Add(this.pnlRefund);
            this.Controls.Add(this.pnlLoadedInvoiceData);
            this.Controls.Add(this.chkPmnt4);
            this.Controls.Add(this.chkPmnt3);
            this.Controls.Add(this.chkPmnt2);
            this.Controls.Add(this.chkPmnt1);
            this.Controls.Add(this.pnlPmnt4);
            this.Controls.Add(this.pnlPmnt3);
            this.Controls.Add(this.pnlPmnt2);
            this.Controls.Add(this.pnlPmnt1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numModifiedDiscount);
            this.Controls.Add(this.numTotalDiscount);
            this.Controls.Add(this.cmbTotalDiscountType);
            this.Controls.Add(this.btnSaveAndPrint);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblGrandTotal);
            this.Controls.Add(this.lblSubTotal);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtInvoiceNote);
            this.Name = "frmProcedureInvoice2";
            this.Text = "Procedure Invoice";
            this.Load += new System.EventHandler(this.frmConsumeStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt1)).EndInit();
            this.pnlPmnt1.ResumeLayout(false);
            this.pnlPmnt1.PerformLayout();
            this.pnlPmnt2.ResumeLayout(false);
            this.pnlPmnt2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt2)).EndInit();
            this.pnlPmnt3.ResumeLayout(false);
            this.pnlPmnt3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt3)).EndInit();
            this.pnlLoadedInvoiceData.ResumeLayout(false);
            this.pnlLoadedInvoiceData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).EndInit();
            this.pnlRefund.ResumeLayout(false);
            this.pnlRefund.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSuggestion)).EndInit();
            this.pnlAddItem.ResumeLayout(false);
            this.pnlAddItem.PerformLayout();
            this.pnlPatientDetails.ResumeLayout(false);
            this.pnlPatientDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt4)).EndInit();
            this.pnlPmnt4.ResumeLayout(false);
            this.pnlPmnt4.PerformLayout();
            this.pnlPmnt5.ResumeLayout(false);
            this.pnlPmnt5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPmnt5)).EndInit();
            this.pnlDues.ResumeLayout(false);
            this.pnlDue.ResumeLayout(false);
            this.pnlDue.PerformLayout();
            this.pnlAdvance.ResumeLayout(false);
            this.pnlAdvance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numModifiedDiscount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label ErrSelectItem;
        private System.Windows.Forms.Label ErrSelectBatch;
        private System.Windows.Forms.Label ErrQuantity;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numRate;
        private System.Windows.Forms.Label errRate;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numTotalDiscount;
        private System.Windows.Forms.ComboBox cmbTotalDiscountType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.DateTimePicker dtpPmntType1;
        private System.Windows.Forms.ComboBox cmbPmntType1;
        private System.Windows.Forms.NumericUpDown numPmnt1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblAdvance;
        private System.Windows.Forms.Label lblDue;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel pnlPmnt1;
        private System.Windows.Forms.Panel pnlPmnt2;
        private System.Windows.Forms.DateTimePicker dtpPmntType2;
        private System.Windows.Forms.ComboBox cmbPmntType2;
        private System.Windows.Forms.NumericUpDown numPmnt2;
        private System.Windows.Forms.Panel pnlPmnt3;
        private System.Windows.Forms.DateTimePicker dtpPmntType3;
        private System.Windows.Forms.ComboBox cmbPmntType3;
        private System.Windows.Forms.NumericUpDown numPmnt3;
        private System.Windows.Forms.CheckBox chkPmnt1;
        private System.Windows.Forms.CheckBox chkPmnt2;
        private System.Windows.Forms.CheckBox chkPmnt3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.TextBox txtChequeInfoId1;
        private System.Windows.Forms.TextBox txtChequeInfoId2;
        private System.Windows.Forms.TextBox txtChequeInfoId3;
        private System.Windows.Forms.Button btnAddPatient;
        private System.Windows.Forms.TextBox txtSearchPatient;
        private System.Windows.Forms.Panel pnlLoadedInvoiceData;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.TextBox txtInvPmntId1;
        private System.Windows.Forms.TextBox txtInvPmntId2;
        private System.Windows.Forms.TextBox txtInvPmntId3;
        private System.Windows.Forms.Panel pnlRefund;
        private System.Windows.Forms.DateTimePicker dtpRedundDate;
        private System.Windows.Forms.TextBox txtInvoiceNote;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtRefundReason;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRefundAmount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridView grdSuggestion;
        private System.Windows.Forms.TextBox txtSearchByName;
        private System.Windows.Forms.TextBox txtSearchByPhone;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.NumericUpDown numDiscount;
        private System.Windows.Forms.GroupBox pnlAddItem;
        private System.Windows.Forms.GroupBox pnlPatientDetails;
        private System.Windows.Forms.CheckBox chkPmnt4;
        private System.Windows.Forms.NumericUpDown numPmnt4;
        private System.Windows.Forms.ComboBox cmbPmntType4;
        private System.Windows.Forms.DateTimePicker dtpPmntType4;
        private System.Windows.Forms.TextBox txtChequeInfoId4;
        private System.Windows.Forms.TextBox txtInvPmntId4;
        private System.Windows.Forms.Panel pnlPmnt4;
        private System.Windows.Forms.CheckBox chkPmnt5;
        private System.Windows.Forms.Panel pnlPmnt5;
        private System.Windows.Forms.TextBox txtInvPmntId5;
        private System.Windows.Forms.TextBox txtChequeInfoId5;
        private System.Windows.Forms.DateTimePicker dtpPmntType5;
        private System.Windows.Forms.ComboBox cmbPmntType5;
        private System.Windows.Forms.NumericUpDown numPmnt5;
        private System.Windows.Forms.Button btnSaveAndPrint;
        private System.Windows.Forms.TextBox txtCurrentRefund;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtAvlQty;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Panel pnlDues;
        private System.Windows.Forms.Panel pnlDue;
        private System.Windows.Forms.Panel pnlAdvance;
        private System.Windows.Forms.Button btnAddNewProcedure;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.TextBox txtNetAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.NumericUpDown numModifiedDiscount;
        private System.Windows.Forms.Label Errdisc;
        private System.Windows.Forms.ComboBox cmbProcedure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblSelectBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceItemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcedureId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcedureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsNewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReturnQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReturnQtyInitial;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.DataGridViewButtonColumn colReturnToggle;
    }
}