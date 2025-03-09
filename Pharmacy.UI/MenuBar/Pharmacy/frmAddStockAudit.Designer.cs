namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmAddStockAudit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dlgSaveExcel = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colAuditDetailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewSysQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSysQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhyQty = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.colQtyDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetPr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrAdjQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmtDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBtnAdjust = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnCopy = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnRefresh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.lblCurrentDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnSearchItems = new System.Windows.Forms.Button();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.btnDiffEnt = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.btnImplAudit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpAudDate = new System.Windows.Forms.DateTimePicker();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(184)))), ((int)(((byte)(224)))));
            this.label4.Location = new System.Drawing.Point(95, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 122;
            this.label4.Text = "Add Stock Audit";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 123;
            this.label3.Text = "RMS";
            this.label3.Visible = false;
            // 
            // dlgSaveExcel
            // 
            this.dlgSaveExcel.DefaultExt = "xls";
            this.dlgSaveExcel.FileName = "ItemsList.xls";
            this.dlgSaveExcel.Filter = "Excel|*.xls";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(82, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 17);
            this.label5.TabIndex = 121;
            this.label5.Text = ">";
            this.label5.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(5, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(209, 36);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSaveAudit_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.txtNote);
            this.panel2.Controls.Add(this.lblCurrentDate);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnImplAudit);
            this.panel2.Location = new System.Drawing.Point(2, 49);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1239, 528);
            this.panel2.TabIndex = 124;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAuditDetailId,
            this.colItemId,
            this.colItemName,
            this.colUnit,
            this.colConvUnit,
            this.colNewSysQty,
            this.colSysQty,
            this.colPhyQty,
            this.colQtyDiff,
            this.colCostPrice,
            this.colRetPr,
            this.colCurrAdjQty,
            this.colCurrQty,
            this.colAmtDiff,
            this.colBtnAdjust,
            this.colBtnCopy,
            this.colBtnRefresh,
            this.colBtnRemove});
            this.dataGridView1.Location = new System.Drawing.Point(0, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(1239, 413);
            this.dataGridView1.TabIndex = 26;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            // 
            // colAuditDetailId
            // 
            this.colAuditDetailId.HeaderText = "AuditDetailId";
            this.colAuditDetailId.Name = "colAuditDetailId";
            this.colAuditDetailId.ReadOnly = true;
            this.colAuditDetailId.Visible = false;
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
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemName.DefaultCellStyle = dataGridViewCellStyle13;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.MinimumWidth = 150;
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colUnit
            // 
            this.colUnit.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colUnit.HeaderText = "Unit";
            this.colUnit.MinimumWidth = 70;
            this.colUnit.Name = "colUnit";
            this.colUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colConvUnit
            // 
            this.colConvUnit.HeaderText = "Conv. Unit";
            this.colConvUnit.MinimumWidth = 40;
            this.colConvUnit.Name = "colConvUnit";
            this.colConvUnit.ReadOnly = true;
            this.colConvUnit.Width = 50;
            // 
            // colNewSysQty
            // 
            this.colNewSysQty.HeaderText = "New System Quantity";
            this.colNewSysQty.MinimumWidth = 70;
            this.colNewSysQty.Name = "colNewSysQty";
            this.colNewSysQty.ReadOnly = true;
            this.colNewSysQty.Visible = false;
            this.colNewSysQty.Width = 70;
            // 
            // colSysQty
            // 
            this.colSysQty.HeaderText = "System Qty";
            this.colSysQty.MinimumWidth = 50;
            this.colSysQty.Name = "colSysQty";
            this.colSysQty.ReadOnly = true;
            this.colSysQty.Width = 60;
            // 
            // colPhyQty
            // 
            // 
            // 
            // 
            this.colPhyQty.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colPhyQty.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colPhyQty.HeaderText = "Physical Qty";
            this.colPhyQty.MinimumWidth = 50;
            this.colPhyQty.Name = "colPhyQty";
            this.colPhyQty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPhyQty.Width = 60;
            // 
            // colQtyDiff
            // 
            this.colQtyDiff.HeaderText = "Quantity Diff";
            this.colQtyDiff.MinimumWidth = 50;
            this.colQtyDiff.Name = "colQtyDiff";
            this.colQtyDiff.ReadOnly = true;
            this.colQtyDiff.Width = 60;
            // 
            // colCostPrice
            // 
            this.colCostPrice.HeaderText = "Cost Price";
            this.colCostPrice.Name = "colCostPrice";
            // 
            // colRetPr
            // 
            this.colRetPr.HeaderText = "Retail Price";
            this.colRetPr.MinimumWidth = 50;
            this.colRetPr.Name = "colRetPr";
            this.colRetPr.ReadOnly = true;
            this.colRetPr.Width = 80;
            // 
            // colCurrAdjQty
            // 
            this.colCurrAdjQty.HeaderText = "Current Adjusted Qty.";
            this.colCurrAdjQty.MinimumWidth = 50;
            this.colCurrAdjQty.Name = "colCurrAdjQty";
            this.colCurrAdjQty.ReadOnly = true;
            this.colCurrAdjQty.Width = 60;
            // 
            // colCurrQty
            // 
            this.colCurrQty.HeaderText = "Current Qty";
            this.colCurrQty.MinimumWidth = 50;
            this.colCurrQty.Name = "colCurrQty";
            this.colCurrQty.ReadOnly = true;
            this.colCurrQty.Visible = false;
            this.colCurrQty.Width = 60;
            // 
            // colAmtDiff
            // 
            this.colAmtDiff.HeaderText = "Amount Diff.";
            this.colAmtDiff.Name = "colAmtDiff";
            this.colAmtDiff.ReadOnly = true;
            // 
            // colBtnAdjust
            // 
            this.colBtnAdjust.HeaderText = "Adjust";
            this.colBtnAdjust.MinimumWidth = 40;
            this.colBtnAdjust.Name = "colBtnAdjust";
            this.colBtnAdjust.ReadOnly = true;
            this.colBtnAdjust.Text = "Adjust";
            this.colBtnAdjust.UseColumnTextForButtonValue = true;
            this.colBtnAdjust.Width = 50;
            // 
            // colBtnCopy
            // 
            this.colBtnCopy.HeaderText = "Copy";
            this.colBtnCopy.MinimumWidth = 40;
            this.colBtnCopy.Name = "colBtnCopy";
            this.colBtnCopy.ReadOnly = true;
            this.colBtnCopy.Text = "Copy";
            this.colBtnCopy.UseColumnTextForButtonValue = true;
            this.colBtnCopy.Width = 50;
            // 
            // colBtnRefresh
            // 
            this.colBtnRefresh.HeaderText = "Refresh";
            this.colBtnRefresh.MinimumWidth = 40;
            this.colBtnRefresh.Name = "colBtnRefresh";
            this.colBtnRefresh.ReadOnly = true;
            this.colBtnRefresh.Text = "Refresh";
            this.colBtnRefresh.UseColumnTextForButtonValue = true;
            this.colBtnRefresh.Width = 50;
            // 
            // colBtnRemove
            // 
            this.colBtnRemove.HeaderText = "Remove";
            this.colBtnRemove.MinimumWidth = 40;
            this.colBtnRemove.Name = "colBtnRemove";
            this.colBtnRemove.ReadOnly = true;
            this.colBtnRemove.Text = "Remove";
            this.colBtnRemove.UseColumnTextForButtonValue = true;
            this.colBtnRemove.Width = 50;
            // 
            // txtNote
            // 
            this.txtNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNote.Location = new System.Drawing.Point(83, 474);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(1144, 51);
            this.txtNote.TabIndex = 25;
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.AutoSize = true;
            this.lblCurrentDate.Location = new System.Drawing.Point(135, 87);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentDate.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 506);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Audit Note";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.AliceBlue;
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Controls.Add(this.btnSearchItems);
            this.panel3.Controls.Add(this.btnShowAll);
            this.panel3.Controls.Add(this.btnDiffEnt);
            this.panel3.Controls.Add(this.btnCopy);
            this.panel3.Controls.Add(this.btnAdjust);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1239, 45);
            this.panel3.TabIndex = 18;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(1101, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(91, 17);
            this.lblStatus.TabIndex = 26;
            this.lblStatus.Text = "Showing All";
            this.lblStatus.Visible = false;
            // 
            // btnSearchItems
            // 
            this.btnSearchItems.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSearchItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchItems.ForeColor = System.Drawing.Color.Black;
            this.btnSearchItems.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchItems.Location = new System.Drawing.Point(3, 3);
            this.btnSearchItems.Name = "btnSearchItems";
            this.btnSearchItems.Size = new System.Drawing.Size(209, 37);
            this.btnSearchItems.TabIndex = 1007;
            this.btnSearchItems.Text = "Show Product Details";
            this.btnSearchItems.UseVisualStyleBackColor = false;
            this.btnSearchItems.Click += new System.EventHandler(this.btnSearchItems_Click);
            // 
            // btnShowAll
            // 
            this.btnShowAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnShowAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnShowAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAll.ForeColor = System.Drawing.Color.White;
            this.btnShowAll.Location = new System.Drawing.Point(783, -1);
            this.btnShowAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(104, 40);
            this.btnShowAll.TabIndex = 24;
            this.btnShowAll.Text = "Show All Enteries";
            this.btnShowAll.UseVisualStyleBackColor = false;
            this.btnShowAll.Visible = false;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // btnDiffEnt
            // 
            this.btnDiffEnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnDiffEnt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnDiffEnt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiffEnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiffEnt.ForeColor = System.Drawing.Color.White;
            this.btnDiffEnt.Location = new System.Drawing.Point(893, -1);
            this.btnDiffEnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDiffEnt.Name = "btnDiffEnt";
            this.btnDiffEnt.Size = new System.Drawing.Size(104, 40);
            this.btnDiffEnt.TabIndex = 24;
            this.btnDiffEnt.Text = "Enteries With Diff Only";
            this.btnDiffEnt.UseVisualStyleBackColor = false;
            this.btnDiffEnt.Visible = false;
            this.btnDiffEnt.Click += new System.EventHandler(this.btnDiffEnt_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnCopy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(673, -3);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(104, 42);
            this.btnCopy.TabIndex = 17;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnAdjust
            // 
            this.btnAdjust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAdjust.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjust.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdjust.ForeColor = System.Drawing.Color.White;
            this.btnAdjust.Location = new System.Drawing.Point(563, -2);
            this.btnAdjust.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(104, 42);
            this.btnAdjust.TabIndex = 17;
            this.btnAdjust.Text = "Adjust";
            this.btnAdjust.UseVisualStyleBackColor = false;
            this.btnAdjust.Visible = false;
            this.btnAdjust.Click += new System.EventHandler(this.btnAdjust_Click);
            // 
            // btnImplAudit
            // 
            this.btnImplAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImplAudit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnImplAudit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnImplAudit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImplAudit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImplAudit.ForeColor = System.Drawing.Color.White;
            this.btnImplAudit.Location = new System.Drawing.Point(62, 357);
            this.btnImplAudit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImplAudit.Name = "btnImplAudit";
            this.btnImplAudit.Size = new System.Drawing.Size(53, 36);
            this.btnImplAudit.TabIndex = 17;
            this.btnImplAudit.Text = "Implement Audit";
            this.btnImplAudit.UseVisualStyleBackColor = false;
            this.btnImplAudit.Visible = false;
            this.btnImplAudit.Click += new System.EventHandler(this.btnImplAudit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(672, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Current Date >>";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(935, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Stock Audit Date";
            // 
            // dtpAudDate
            // 
            this.dtpAudDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpAudDate.Location = new System.Drawing.Point(1029, 12);
            this.dtpAudDate.Name = "dtpAudDate";
            this.dtpAudDate.Size = new System.Drawing.Size(200, 20);
            this.dtpAudDate.TabIndex = 19;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "ITEMID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "ITEM";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn3.HeaderText = "BARCODE";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "SUPPLIER(S)";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "REORDERING LEVEL";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "TOTAL STOCK";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 60;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "AVAILABLE STOCK";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 60;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Red;
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn8.HeaderText = "EXPIRED STOCK";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 50;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "STOCKING UNIT";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            this.dataGridViewTextBoxColumn9.Width = 60;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.OrangeRed;
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridViewTextBoxColumn10.HeaderText = "EXPIRED";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 60;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "STOCKING UNIT";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 60;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "CONSUMED STOCK";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 40;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Consumed Stock";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpAudDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1241, 41);
            this.panel1.TabIndex = 125;
            // 
            // frmAddStockAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1241, 577);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Name = "frmAddStockAudit";
            this.Text = "Add Stock Audit";
            this.Load += new System.EventHandler(this.frmAddStockAudit_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.SaveFileDialog dlgSaveExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpAudDate;
        private System.Windows.Forms.Label lblCurrentDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearchItems;
        private System.Windows.Forms.Button btnImplAudit;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnDiffEnt;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuditDetailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewSysQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSysQty;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn colPhyQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyDiff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetPr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrAdjQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmtDiff;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnAdjust;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnCopy;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnRefresh;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnRemove;
        private System.Windows.Forms.Panel panel1;
    }
}