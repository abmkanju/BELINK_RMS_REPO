namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmAddStockAudit3
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnSearchItems = new System.Windows.Forms.Button();
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
            this.colAuditDetailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSysQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhyQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrAdjQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetPr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmtDiff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBtnAdjsut = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnCopy = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnRefresh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBtnRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAuditDetailId,
            this.colItemId,
            this.colItemName,
            this.colUnit,
            this.colConvUnit,
            this.colSysQty,
            this.colPhyQty,
            this.colQtyDiff,
            this.colCurrAdjQty,
            this.colCurrQty,
            this.colRetPr,
            this.colAmtDiff,
            this.colBtnAdjsut,
            this.colBtnCopy,
            this.colBtnRefresh,
            this.colBtnRemove});
            this.dataGridView1.Location = new System.Drawing.Point(154, 74);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1028, 411);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Controls.Add(this.btnSearchItems);
            this.panel3.Location = new System.Drawing.Point(154, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1028, 45);
            this.panel3.TabIndex = 19;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(890, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(91, 17);
            this.lblStatus.TabIndex = 26;
            this.lblStatus.Text = "Showing All";
            // 
            // btnSearchItems
            // 
            this.btnSearchItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnSearchItems.FlatAppearance.BorderSize = 0;
            this.btnSearchItems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnSearchItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchItems.ForeColor = System.Drawing.Color.White;
            this.btnSearchItems.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchItems.Location = new System.Drawing.Point(3, 10);
            this.btnSearchItems.Name = "btnSearchItems";
            this.btnSearchItems.Size = new System.Drawing.Size(149, 27);
            this.btnSearchItems.TabIndex = 1007;
            this.btnSearchItems.Text = "Search Items  ?";
            this.btnSearchItems.UseVisualStyleBackColor = false;
            this.btnSearchItems.Click += new System.EventHandler(this.btnSearchItems_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Audit detail id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ItemId";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Conv. Unit";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Sys. Qty";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Phy. Qty";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Qty. Diff";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Current Adjusted Qty";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Current Qty";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Retail Price";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Amount Diff";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // colAuditDetailId
            // 
            this.colAuditDetailId.HeaderText = "Audit detail id";
            this.colAuditDetailId.Name = "colAuditDetailId";
            this.colAuditDetailId.ReadOnly = true;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "ItemId";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            // 
            // colItemName
            // 
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            // 
            // colConvUnit
            // 
            this.colConvUnit.HeaderText = "Conv. Unit";
            this.colConvUnit.Name = "colConvUnit";
            this.colConvUnit.ReadOnly = true;
            this.colConvUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colConvUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colSysQty
            // 
            this.colSysQty.HeaderText = "Sys. Qty";
            this.colSysQty.Name = "colSysQty";
            this.colSysQty.ReadOnly = true;
            // 
            // colPhyQty
            // 
            this.colPhyQty.HeaderText = "Phy. Qty";
            this.colPhyQty.Name = "colPhyQty";
            // 
            // colQtyDiff
            // 
            this.colQtyDiff.HeaderText = "Qty. Diff";
            this.colQtyDiff.Name = "colQtyDiff";
            this.colQtyDiff.ReadOnly = true;
            // 
            // colCurrAdjQty
            // 
            this.colCurrAdjQty.HeaderText = "Current Adjusted Qty";
            this.colCurrAdjQty.Name = "colCurrAdjQty";
            this.colCurrAdjQty.ReadOnly = true;
            // 
            // colCurrQty
            // 
            this.colCurrQty.HeaderText = "Current Qty";
            this.colCurrQty.Name = "colCurrQty";
            this.colCurrQty.ReadOnly = true;
            // 
            // colRetPr
            // 
            this.colRetPr.HeaderText = "Retail Price";
            this.colRetPr.Name = "colRetPr";
            this.colRetPr.ReadOnly = true;
            // 
            // colAmtDiff
            // 
            this.colAmtDiff.HeaderText = "Amount Diff";
            this.colAmtDiff.Name = "colAmtDiff";
            this.colAmtDiff.ReadOnly = true;
            // 
            // colBtnAdjsut
            // 
            this.colBtnAdjsut.HeaderText = "Adjust";
            this.colBtnAdjsut.Name = "colBtnAdjsut";
            this.colBtnAdjsut.Text = "Adjust";
            this.colBtnAdjsut.UseColumnTextForButtonValue = true;
            // 
            // colBtnCopy
            // 
            this.colBtnCopy.HeaderText = "Copy";
            this.colBtnCopy.Name = "colBtnCopy";
            this.colBtnCopy.Text = "Copy";
            this.colBtnCopy.UseColumnTextForButtonValue = true;
            // 
            // colBtnRefresh
            // 
            this.colBtnRefresh.HeaderText = "Refresh";
            this.colBtnRefresh.Name = "colBtnRefresh";
            this.colBtnRefresh.ReadOnly = true;
            this.colBtnRefresh.Text = "Refresh";
            this.colBtnRefresh.UseColumnTextForButtonValue = true;
            // 
            // colBtnRemove
            // 
            this.colBtnRemove.HeaderText = "Remove";
            this.colBtnRemove.Name = "colBtnRemove";
            this.colBtnRemove.Text = "Remove";
            this.colBtnRemove.UseColumnTextForButtonValue = true;
            // 
            // frmAddStockAudit3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 497);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmAddStockAudit3";
            this.Text = "frmAddStockAudit3";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnSearchItems;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuditDetailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConvUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSysQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhyQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyDiff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrAdjQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetPr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmtDiff;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnAdjsut;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnCopy;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnRefresh;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnRemove;
    }
}