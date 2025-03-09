namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmConsumeStock
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
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbSelectItems = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSelectBatch = new System.Windows.Forms.ComboBox();
            this.cmbConsumtionType = new System.Windows.Forms.ComboBox();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.lblSelectBatch = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colStockConsumptionItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumptionTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumptionTypeString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ErrSelectItem = new System.Windows.Forms.Label();
            this.ErrSelectBatch = new System.Windows.Forms.Label();
            this.ErrQuantity = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.txtAvlQty = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(568, 130);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbSelectItems
            // 
            this.cmbSelectItems.FormattingEnabled = true;
            this.cmbSelectItems.Location = new System.Drawing.Point(12, 58);
            this.cmbSelectItems.Name = "cmbSelectItems";
            this.cmbSelectItems.Size = new System.Drawing.Size(210, 22);
            this.cmbSelectItems.TabIndex = 1;
            this.cmbSelectItems.SelectedIndexChanged += new System.EventHandler(this.cmbSelectItems_SelectedIndexChanged);
            this.cmbSelectItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSelectItems_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Item";
            // 
            // cmbSelectBatch
            // 
            this.cmbSelectBatch.FormattingEnabled = true;
            this.cmbSelectBatch.Location = new System.Drawing.Point(228, 58);
            this.cmbSelectBatch.Name = "cmbSelectBatch";
            this.cmbSelectBatch.Size = new System.Drawing.Size(359, 22);
            this.cmbSelectBatch.TabIndex = 1;
            this.cmbSelectBatch.SelectedIndexChanged += new System.EventHandler(this.cmbSelectBatch_SelectedIndexChanged);
            // 
            // cmbConsumtionType
            // 
            this.cmbConsumtionType.FormattingEnabled = true;
            this.cmbConsumtionType.Items.AddRange(new object[] {
            "Services",
            "Sales",
            "Damages",
            "Returned"});
            this.cmbConsumtionType.Location = new System.Drawing.Point(812, 58);
            this.cmbConsumtionType.Name = "cmbConsumtionType";
            this.cmbConsumtionType.Size = new System.Drawing.Size(176, 22);
            this.cmbConsumtionType.TabIndex = 1;
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAddItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Location = new System.Drawing.Point(489, 130);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(75, 25);
            this.btnAddItem.TabIndex = 0;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // lblSelectBatch
            // 
            this.lblSelectBatch.AutoSize = true;
            this.lblSelectBatch.Location = new System.Drawing.Point(225, 41);
            this.lblSelectBatch.Name = "lblSelectBatch";
            this.lblSelectBatch.Size = new System.Drawing.Size(69, 14);
            this.lblSelectBatch.TabIndex = 2;
            this.lblSelectBatch.Text = "Select Batch";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(707, 59);
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
            this.numQuantity.Size = new System.Drawing.Size(99, 21);
            this.numQuantity.TabIndex = 3;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(704, 42);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(52, 14);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(809, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Consumption Type";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(15, 112);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(468, 43);
            this.txtComment.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 96);
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
            this.colStockConsumptionItemId,
            this.colItemId,
            this.colItemName,
            this.colBatchId,
            this.colBatch,
            this.colQuantity,
            this.colConsumptionTypeId,
            this.colConsumptionTypeString,
            this.colComment,
            this.colEdit,
            this.colRemove});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 176);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.Size = new System.Drawing.Size(1120, 365);
            this.grdItems.TabIndex = 5;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            this.grdItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdItems_RowsAdded);
            // 
            // colStockConsumptionItemId
            // 
            this.colStockConsumptionItemId.HeaderText = "ConsumptionItemId";
            this.colStockConsumptionItemId.Name = "colStockConsumptionItemId";
            this.colStockConsumptionItemId.ReadOnly = true;
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
            this.colItemName.HeaderText = "Item Name";
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
            this.colConsumptionTypeId.Visible = false;
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
            // ErrSelectItem
            // 
            this.ErrSelectItem.AutoSize = true;
            this.ErrSelectItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSelectItem.ForeColor = System.Drawing.Color.Red;
            this.ErrSelectItem.Location = new System.Drawing.Point(207, 41);
            this.ErrSelectItem.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectItem.Name = "ErrSelectItem";
            this.ErrSelectItem.Size = new System.Drawing.Size(15, 16);
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
            this.ErrSelectBatch.Location = new System.Drawing.Point(572, 39);
            this.ErrSelectBatch.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectBatch.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrSelectBatch.Name = "ErrSelectBatch";
            this.ErrSelectBatch.Size = new System.Drawing.Size(15, 16);
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
            this.ErrQuantity.Location = new System.Drawing.Point(791, 38);
            this.ErrQuantity.MaximumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.MinimumSize = new System.Drawing.Size(15, 16);
            this.ErrQuantity.Name = "ErrQuantity";
            this.ErrQuantity.Size = new System.Drawing.Size(15, 16);
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
            this.ErrMessage.Location = new System.Drawing.Point(12, 10);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 51;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // txtAvlQty
            // 
            this.txtAvlQty.Enabled = false;
            this.txtAvlQty.Location = new System.Drawing.Point(593, 58);
            this.txtAvlQty.Name = "txtAvlQty";
            this.txtAvlQty.Size = new System.Drawing.Size(108, 21);
            this.txtAvlQty.TabIndex = 57;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(595, 41);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(101, 14);
            this.label24.TabIndex = 56;
            this.label24.Text = "Available Quantity";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(976, 547);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 34);
            this.btnSave.TabIndex = 58;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClearAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAll.ForeColor = System.Drawing.Color.White;
            this.btnClearAll.Location = new System.Drawing.Point(1057, 547);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 34);
            this.btnClearAll.TabIndex = 59;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
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
            // 
            // dataGridViewTextBoxColumn3
            // 
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
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "QUANTITY";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "ConsumptionTypeId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
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
            // frmConsumeStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1144, 593);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.txtAvlQty);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.ErrQuantity);
            this.Controls.Add(this.ErrSelectBatch);
            this.Controls.Add(this.ErrSelectItem);
            this.Controls.Add(this.grdItems);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblSelectBatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbConsumtionType);
            this.Controls.Add(this.cmbSelectBatch);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.cmbSelectItems);
            this.Controls.Add(this.btnClear);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmConsumeStock";
            this.Text = "Consume Stock";
            this.Load += new System.EventHandler(this.frmConsumeStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbSelectItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSelectBatch;
        private System.Windows.Forms.ComboBox cmbConsumtionType;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label lblSelectBatch;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView grdItems;
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
        private System.Windows.Forms.TextBox txtAvlQty;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockConsumptionItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumptionTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumptionTypeString;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
    }
}