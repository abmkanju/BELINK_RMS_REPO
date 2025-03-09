namespace Pharmacy.UI.MenuBar.Preferences
{
    partial class frmAddDiscount
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
            this.txtDiscName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectItems = new System.Windows.Forms.Button();
            this.rbAllItems = new System.Windows.Forms.RadioButton();
            this.rbSelectedItems = new System.Windows.Forms.RadioButton();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDiscCode = new System.Windows.Forms.TextBox();
            this.cmbDiscType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDiscVal = new System.Windows.Forms.Label();
            this.numDisc = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.chkAllTime = new System.Windows.Forms.CheckBox();
            this.pnlDays = new System.Windows.Forms.GroupBox();
            this.btnSelectDays = new System.Windows.Forms.Button();
            this.rbSelectedDays = new System.Windows.Forms.RadioButton();
            this.rbAllDays = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colDiscountItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscoutnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ErrDiscName = new System.Windows.Forms.Label();
            this.errDisc = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.errCode = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDisc)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlTime.SuspendLayout();
            this.pnlDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDiscName
            // 
            this.txtDiscName.Location = new System.Drawing.Point(127, 30);
            this.txtDiscName.Name = "txtDiscName";
            this.txtDiscName.Size = new System.Drawing.Size(217, 20);
            this.txtDiscName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Discount Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(837, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSelectItems);
            this.panel1.Controls.Add(this.rbAllItems);
            this.panel1.Controls.Add(this.rbSelectedItems);
            this.panel1.Location = new System.Drawing.Point(6, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 37);
            this.panel1.TabIndex = 11;
            // 
            // btnSelectItems
            // 
            this.btnSelectItems.Location = new System.Drawing.Point(221, 4);
            this.btnSelectItems.Name = "btnSelectItems";
            this.btnSelectItems.Size = new System.Drawing.Size(37, 26);
            this.btnSelectItems.TabIndex = 2;
            this.btnSelectItems.Text = ". . .";
            this.btnSelectItems.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectItems.UseVisualStyleBackColor = true;
            this.btnSelectItems.Click += new System.EventHandler(this.btnSelectItems_Click);
            // 
            // rbAllItems
            // 
            this.rbAllItems.AutoSize = true;
            this.rbAllItems.Location = new System.Drawing.Point(8, 9);
            this.rbAllItems.Name = "rbAllItems";
            this.rbAllItems.Size = new System.Drawing.Size(64, 17);
            this.rbAllItems.TabIndex = 0;
            this.rbAllItems.Text = "All Items";
            this.rbAllItems.UseVisualStyleBackColor = true;
            this.rbAllItems.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbSelectedItems
            // 
            this.rbSelectedItems.AutoSize = true;
            this.rbSelectedItems.Checked = true;
            this.rbSelectedItems.Location = new System.Drawing.Point(120, 9);
            this.rbSelectedItems.Name = "rbSelectedItems";
            this.rbSelectedItems.Size = new System.Drawing.Size(95, 17);
            this.rbSelectedItems.TabIndex = 1;
            this.rbSelectedItems.TabStop = true;
            this.rbSelectedItems.Text = "Selected Items";
            this.rbSelectedItems.UseVisualStyleBackColor = true;
            this.rbSelectedItems.CheckedChanged += new System.EventHandler(this.rbSelectedItems_CheckedChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(127, 82);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(217, 20);
            this.dtpStartDate.TabIndex = 2;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(126, 108);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(217, 20);
            this.dtpEndDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Discount Start Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Discount End Date";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(776, 443);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(208, 35);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(990, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(162, 35);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Code";
            // 
            // txtDiscCode
            // 
            this.txtDiscCode.Location = new System.Drawing.Point(127, 56);
            this.txtDiscCode.Name = "txtDiscCode";
            this.txtDiscCode.Size = new System.Drawing.Size(217, 20);
            this.txtDiscCode.TabIndex = 1;
            // 
            // cmbDiscType
            // 
            this.cmbDiscType.FormattingEnabled = true;
            this.cmbDiscType.Items.AddRange(new object[] {
            "%",
            "Value"});
            this.cmbDiscType.Location = new System.Drawing.Point(126, 134);
            this.cmbDiscType.Name = "cmbDiscType";
            this.cmbDiscType.Size = new System.Drawing.Size(218, 21);
            this.cmbDiscType.TabIndex = 4;
            this.cmbDiscType.SelectedIndexChanged += new System.EventHandler(this.cmbDiscType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Discount Type";
            // 
            // lblDiscVal
            // 
            this.lblDiscVal.AutoSize = true;
            this.lblDiscVal.Location = new System.Drawing.Point(9, 163);
            this.lblDiscVal.Name = "lblDiscVal";
            this.lblDiscVal.Size = new System.Drawing.Size(89, 13);
            this.lblDiscVal.TabIndex = 2;
            this.lblDiscVal.Text = "Discount Percent";
            // 
            // numDisc
            // 
            this.numDisc.DecimalPlaces = 2;
            this.numDisc.Location = new System.Drawing.Point(126, 161);
            this.numDisc.Name = "numDisc";
            this.numDisc.Size = new System.Drawing.Size(218, 20);
            this.numDisc.TabIndex = 5;
            this.numDisc.ValueChanged += new System.EventHandler(this.numDisc_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlTime);
            this.groupBox2.Controls.Add(this.chkAllTime);
            this.groupBox2.Location = new System.Drawing.Point(373, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 100);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Discount Time";
            // 
            // pnlTime
            // 
            this.pnlTime.Controls.Add(this.dtpEndTime);
            this.pnlTime.Controls.Add(this.label9);
            this.pnlTime.Controls.Add(this.label10);
            this.pnlTime.Controls.Add(this.dtpStartTime);
            this.pnlTime.Enabled = false;
            this.pnlTime.Location = new System.Drawing.Point(5, 40);
            this.pnlTime.Name = "pnlTime";
            this.pnlTime.Size = new System.Drawing.Size(162, 49);
            this.pnlTime.TabIndex = 17;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Location = new System.Drawing.Point(69, 25);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(89, 20);
            this.dtpEndTime.TabIndex = 2;
            this.dtpEndTime.Value = new System.DateTime(2021, 1, 15, 23, 59, 0, 0);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Start Time";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "End Time";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Location = new System.Drawing.Point(69, 2);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(89, 20);
            this.dtpStartTime.TabIndex = 1;
            this.dtpStartTime.Value = new System.DateTime(2021, 1, 15, 0, 0, 0, 0);
            // 
            // chkAllTime
            // 
            this.chkAllTime.AutoSize = true;
            this.chkAllTime.Checked = true;
            this.chkAllTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllTime.Location = new System.Drawing.Point(9, 19);
            this.chkAllTime.Name = "chkAllTime";
            this.chkAllTime.Size = new System.Drawing.Size(68, 17);
            this.chkAllTime.TabIndex = 0;
            this.chkAllTime.Text = "All Times";
            this.chkAllTime.UseVisualStyleBackColor = true;
            this.chkAllTime.CheckedChanged += new System.EventHandler(this.chkAllTime_CheckedChanged);
            // 
            // pnlDays
            // 
            this.pnlDays.Controls.Add(this.btnSelectDays);
            this.pnlDays.Controls.Add(this.rbSelectedDays);
            this.pnlDays.Controls.Add(this.rbAllDays);
            this.pnlDays.Location = new System.Drawing.Point(373, 124);
            this.pnlDays.Name = "pnlDays";
            this.pnlDays.Size = new System.Drawing.Size(172, 72);
            this.pnlDays.TabIndex = 7;
            this.pnlDays.TabStop = false;
            this.pnlDays.Text = "Discount Days";
            // 
            // btnSelectDays
            // 
            this.btnSelectDays.Enabled = false;
            this.btnSelectDays.Location = new System.Drawing.Point(114, 37);
            this.btnSelectDays.Name = "btnSelectDays";
            this.btnSelectDays.Size = new System.Drawing.Size(37, 26);
            this.btnSelectDays.TabIndex = 2;
            this.btnSelectDays.Text = ". . .";
            this.btnSelectDays.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectDays.UseVisualStyleBackColor = true;
            this.btnSelectDays.Click += new System.EventHandler(this.btnSelectDays_Click);
            // 
            // rbSelectedDays
            // 
            this.rbSelectedDays.AutoSize = true;
            this.rbSelectedDays.Location = new System.Drawing.Point(14, 39);
            this.rbSelectedDays.Name = "rbSelectedDays";
            this.rbSelectedDays.Size = new System.Drawing.Size(94, 17);
            this.rbSelectedDays.TabIndex = 1;
            this.rbSelectedDays.TabStop = true;
            this.rbSelectedDays.Text = "Selected Days";
            this.rbSelectedDays.UseVisualStyleBackColor = true;
            // 
            // rbAllDays
            // 
            this.rbAllDays.AutoSize = true;
            this.rbAllDays.Checked = true;
            this.rbAllDays.Location = new System.Drawing.Point(14, 18);
            this.rbAllDays.Name = "rbAllDays";
            this.rbAllDays.Size = new System.Drawing.Size(63, 17);
            this.rbAllDays.TabIndex = 0;
            this.rbAllDays.TabStop = true;
            this.rbAllDays.Text = "All Days";
            this.rbAllDays.UseVisualStyleBackColor = true;
            this.rbAllDays.CheckedChanged += new System.EventHandler(this.rbAllDays_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(773, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Comments";
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.Location = new System.Drawing.Point(776, 144);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(376, 70);
            this.txtComment.TabIndex = 9;
            // 
            // grdItems
            // 
            this.grdItems.AllowUserToAddRows = false;
            this.grdItems.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.grdItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.BackgroundColor = System.Drawing.Color.White;
            this.grdItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdItems.ColumnHeadersHeight = 35;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDiscountItemId,
            this.colItemId,
            this.colItemName,
            this.colRetailPrice,
            this.colDiscount,
            this.colDiscoutnType,
            this.colNewValue,
            this.colRemove});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(11, 221);
            this.grdItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1141, 215);
            this.grdItems.TabIndex = 19;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            // 
            // colDiscountItemId
            // 
            this.colDiscountItemId.HeaderText = "Discount Item Id";
            this.colDiscountItemId.Name = "colDiscountItemId";
            this.colDiscountItemId.ReadOnly = true;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "Item Id";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colRetailPrice
            // 
            this.colRetailPrice.HeaderText = "Retail Price";
            this.colRetailPrice.Name = "colRetailPrice";
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Dicount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            // 
            // colDiscoutnType
            // 
            this.colDiscoutnType.HeaderText = "Discount Type";
            this.colDiscoutnType.Name = "colDiscoutnType";
            this.colDiscoutnType.ReadOnly = true;
            // 
            // colNewValue
            // 
            this.colNewValue.HeaderText = "New Value";
            this.colNewValue.Name = "colNewValue";
            this.colNewValue.ReadOnly = true;
            // 
            // colRemove
            // 
            this.colRemove.HeaderText = "Remove";
            this.colRemove.MinimumWidth = 50;
            this.colRemove.Name = "colRemove";
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 50;
            // 
            // ErrDiscName
            // 
            this.ErrDiscName.AutoSize = true;
            this.ErrDiscName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrDiscName.ForeColor = System.Drawing.Color.Red;
            this.ErrDiscName.Location = new System.Drawing.Point(347, 30);
            this.ErrDiscName.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrDiscName.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrDiscName.Name = "ErrDiscName";
            this.ErrDiscName.Size = new System.Drawing.Size(15, 16);
            this.ErrDiscName.TabIndex = 1001;
            this.ErrDiscName.Text = "*";
            this.ErrDiscName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrDiscName.Visible = false;
            // 
            // errDisc
            // 
            this.errDisc.AutoSize = true;
            this.errDisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errDisc.ForeColor = System.Drawing.Color.Red;
            this.errDisc.Location = new System.Drawing.Point(347, 163);
            this.errDisc.MaximumSize = new System.Drawing.Size(15, 16);
            this.errDisc.MinimumSize = new System.Drawing.Size(15, 16);
            this.errDisc.Name = "errDisc";
            this.errDisc.Size = new System.Drawing.Size(15, 16);
            this.errDisc.TabIndex = 1001;
            this.errDisc.Text = "*";
            this.errDisc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errDisc.Visible = false;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(8, 3);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 1002;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // errCode
            // 
            this.errCode.AutoSize = true;
            this.errCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errCode.ForeColor = System.Drawing.Color.Red;
            this.errCode.Location = new System.Drawing.Point(347, 56);
            this.errCode.MaximumSize = new System.Drawing.Size(15, 16);
            this.errCode.MinimumSize = new System.Drawing.Size(15, 16);
            this.errCode.Name = "errCode";
            this.errCode.Size = new System.Drawing.Size(15, 16);
            this.errCode.TabIndex = 1001;
            this.errCode.Text = "*";
            this.errCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errCode.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Dicount";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Discount Type";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Dicount";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Discount Type";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "New Value";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // frmAddDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1160, 479);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.errDisc);
            this.Controls.Add(this.errCode);
            this.Controls.Add(this.ErrDiscName);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pnlDays);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.numDisc);
            this.Controls.Add(this.cmbDiscType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDiscCode);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblDiscVal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDiscName);
            this.Name = "frmAddDiscount";
            this.Text = "Add Discount";
            this.Load += new System.EventHandler(this.frmAddDiscount_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDisc)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlTime.ResumeLayout(false);
            this.pnlTime.PerformLayout();
            this.pnlDays.ResumeLayout(false);
            this.pnlDays.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDiscName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAllItems;
        private System.Windows.Forms.RadioButton rbSelectedItems;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDiscCode;
        private System.Windows.Forms.ComboBox cmbDiscType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDiscVal;
        private System.Windows.Forms.NumericUpDown numDisc;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkAllTime;
        private System.Windows.Forms.Panel pnlTime;
        private System.Windows.Forms.GroupBox pnlDays;
        private System.Windows.Forms.RadioButton rbSelectedDays;
        private System.Windows.Forms.RadioButton rbAllDays;
        private System.Windows.Forms.Button btnSelectDays;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSelectItems;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscoutnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewValue;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.Label ErrDiscName;
        private System.Windows.Forms.Label errDisc;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label errCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}