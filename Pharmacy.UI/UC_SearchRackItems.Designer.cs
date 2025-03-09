namespace Pharmacy.UI
{
    partial class UC_SearchRackItems
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdSearchItems = new System.Windows.Forms.DataGridView();
            this.colSItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearchItems = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearchItems)).BeginInit();
            this.SuspendLayout();
            // 
            // grdSearchItems
            // 
            this.grdSearchItems.AllowUserToAddRows = false;
            this.grdSearchItems.AllowUserToDeleteRows = false;
            this.grdSearchItems.AllowUserToOrderColumns = true;
            this.grdSearchItems.AllowUserToResizeColumns = false;
            this.grdSearchItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSearchItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.grdSearchItems.BackgroundColor = System.Drawing.Color.White;
            this.grdSearchItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdSearchItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdSearchItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdSearchItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSearchItems.ColumnHeadersVisible = false;
            this.grdSearchItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSItemId,
            this.colSItemName});
            this.grdSearchItems.GridColor = System.Drawing.Color.White;
            this.grdSearchItems.Location = new System.Drawing.Point(1, 26);
            this.grdSearchItems.MultiSelect = false;
            this.grdSearchItems.Name = "grdSearchItems";
            this.grdSearchItems.ReadOnly = true;
            this.grdSearchItems.RowHeadersVisible = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdSearchItems.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdSearchItems.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdSearchItems.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.grdSearchItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSearchItems.Size = new System.Drawing.Size(269, 160);
            this.grdSearchItems.TabIndex = 0;
            this.grdSearchItems.TabStop = false;
            this.grdSearchItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSearchItems_CellDoubleClick);
            this.grdSearchItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdSearchItems_KeyDown);
            this.grdSearchItems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdSearchItems_KeyPress);
            // 
            // colSItemId
            // 
            this.colSItemId.HeaderText = "Item Id";
            this.colSItemId.Name = "colSItemId";
            this.colSItemId.ReadOnly = true;
            this.colSItemId.Visible = false;
            // 
            // colSItemName
            // 
            this.colSItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSItemName.HeaderText = "Item";
            this.colSItemName.Name = "colSItemName";
            this.colSItemName.ReadOnly = true;
            // 
            // txtSearchItems
            // 
            this.txtSearchItems.AcceptsReturn = true;
            this.txtSearchItems.AcceptsTab = true;
            this.txtSearchItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchItems.Location = new System.Drawing.Point(1, 1);
            this.txtSearchItems.Name = "txtSearchItems";
            this.txtSearchItems.Size = new System.Drawing.Size(272, 20);
            this.txtSearchItems.TabIndex = 0;
            this.txtSearchItems.TextChanged += new System.EventHandler(this.txtSearchItems_TextChanged);
            this.txtSearchItems.Enter += new System.EventHandler(this.txtSearchItems_Enter);
            this.txtSearchItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchItems_KeyDown);
            this.txtSearchItems.Leave += new System.EventHandler(this.txtSearchItems_Leave);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Item";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // UC_SearchItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.grdSearchItems);
            this.Controls.Add(this.txtSearchItems);
            this.Name = "UC_SearchItems";
            this.Size = new System.Drawing.Size(273, 22);
            ((System.ComponentModel.ISupportInitialize)(this.grdSearchItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdSearchItems;
        private System.Windows.Forms.TextBox txtSearchItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSItemName;
    }
}
