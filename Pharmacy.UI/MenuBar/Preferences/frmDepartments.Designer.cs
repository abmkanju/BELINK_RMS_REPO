namespace Pharmacy.UI.MenuBar.Preferences
{
    partial class frmDepartments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDepartments));
            this.btnAddDepartment = new System.Windows.Forms.Button();
            this.btnRefreshForm = new System.Windows.Forms.Button();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddDepartment
            // 
            this.btnAddDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDepartment.Location = new System.Drawing.Point(966, 14);
            this.btnAddDepartment.Name = "btnAddDepartment";
            this.btnAddDepartment.Size = new System.Drawing.Size(125, 33);
            this.btnAddDepartment.TabIndex = 0;
            this.btnAddDepartment.Text = "Add Department";
            this.btnAddDepartment.UseVisualStyleBackColor = true;
            this.btnAddDepartment.Click += new System.EventHandler(this.btnAddDepartment_Click);
            // 
            // btnRefreshForm
            // 
            this.btnRefreshForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnRefreshForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnRefreshForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshForm.ForeColor = System.Drawing.Color.White;
            this.btnRefreshForm.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshForm.Image")));
            this.btnRefreshForm.Location = new System.Drawing.Point(1097, 14);
            this.btnRefreshForm.Name = "btnRefreshForm";
            this.btnRefreshForm.Size = new System.Drawing.Size(118, 33);
            this.btnRefreshForm.TabIndex = 7;
            this.btnRefreshForm.Text = "Refresh";
            this.btnRefreshForm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefreshForm.UseVisualStyleBackColor = false;
            this.btnRefreshForm.Click += new System.EventHandler(this.btnRefreshForm_Click);
            // 
            // flowPanel
            // 
            this.flowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel.AutoScroll = true;
            this.flowPanel.BackColor = System.Drawing.Color.White;
            this.flowPanel.Controls.Add(this.dataGridView1);
            this.flowPanel.Location = new System.Drawing.Point(3, 53);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(1223, 399);
            this.flowPanel.TabIndex = 10;
            this.flowPanel.SizeChanged += new System.EventHandler(this.flowPanel_SizeChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemId,
            this.colItemName,
            this.colQuantity,
            this.colUnitCost,
            this.colToalCost,
            this.colRetailPrice,
            this.colBatch,
            this.colDiscount,
            this.colSalesTax,
            this.colNetValue,
            this.colCreatedAt});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.Size = new System.Drawing.Size(1258, 150);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.Visible = false;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "ItemId";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.HeaderText = "Item";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            // 
            // colUnitCost
            // 
            this.colUnitCost.HeaderText = "UnitCost";
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.ReadOnly = true;
            // 
            // colToalCost
            // 
            this.colToalCost.HeaderText = "Total Cost";
            this.colToalCost.Name = "colToalCost";
            this.colToalCost.ReadOnly = true;
            // 
            // colRetailPrice
            // 
            this.colRetailPrice.HeaderText = "Retail Price";
            this.colRetailPrice.Name = "colRetailPrice";
            this.colRetailPrice.ReadOnly = true;
            // 
            // colBatch
            // 
            this.colBatch.HeaderText = "Batch";
            this.colBatch.Name = "colBatch";
            this.colBatch.ReadOnly = true;
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            // 
            // colSalesTax
            // 
            this.colSalesTax.HeaderText = "Sales Tax";
            this.colSalesTax.Name = "colSalesTax";
            this.colSalesTax.ReadOnly = true;
            // 
            // colNetValue
            // 
            this.colNetValue.HeaderText = "Net Value";
            this.colNetValue.Name = "colNetValue";
            this.colNetValue.ReadOnly = true;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.HeaderText = "Create At";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            // 
            // frmDepartments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1227, 505);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.btnRefreshForm);
            this.Controls.Add(this.btnAddDepartment);
            this.Name = "frmDepartments";
            this.Text = "Departments";
            this.Load += new System.EventHandler(this.frmDepartments_Load);
            this.flowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddDepartment;
        private System.Windows.Forms.Button btnRefreshForm;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
    }
}