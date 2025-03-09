namespace Pharmacy.UI.Reports.Accounts
{
    partial class BillPaymentsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BillPaymentsUI));
            this.dlgSavePdf = new System.Windows.Forms.SaveFileDialog();
            this.dlgSaveExcel = new System.Windows.Forms.SaveFileDialog();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtGotoPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblPageNo = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.grdBills = new System.Windows.Forms.DataGridView();
            this.colStockIdBills = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentNoBills = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceNoBills = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrandTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmountDue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayment = new System.Windows.Forms.DataGridViewButtonColumn();
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
            this.btnRefreshForm = new System.Windows.Forms.Button();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBills)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgSavePdf
            // 
            this.dlgSavePdf.FileName = "Financial Transactions";
            this.dlgSavePdf.Filter = "PDF|*.pdf";
            // 
            // dlgSaveExcel
            // 
            this.dlgSaveExcel.FileName = "Financial Transactions";
            this.dlgSaveExcel.Filter = "Excel|*.xls";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Controls.Add(this.grdBills);
            this.panel5.Location = new System.Drawing.Point(12, 52);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1216, 487);
            this.panel5.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtGotoPage);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnLastPage);
            this.panel1.Controls.Add(this.btnFirstPage);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.lblPageNo);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Location = new System.Drawing.Point(351, 451);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 31);
            this.panel1.TabIndex = 17;
            // 
            // txtGotoPage
            // 
            this.txtGotoPage.Location = new System.Drawing.Point(443, 5);
            this.txtGotoPage.Name = "txtGotoPage";
            this.txtGotoPage.Size = new System.Drawing.Size(75, 22);
            this.txtGotoPage.TabIndex = 1;
            this.txtGotoPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGotoPage_KeyDown);
            this.txtGotoPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGotoPage_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(373, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "go to page";
            // 
            // btnLastPage
            // 
            this.btnLastPage.BackColor = System.Drawing.Color.White;
            this.btnLastPage.FlatAppearance.BorderSize = 0;
            this.btnLastPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastPage.ForeColor = System.Drawing.Color.Black;
            this.btnLastPage.Location = new System.Drawing.Point(333, 2);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(35, 25);
            this.btnLastPage.TabIndex = 11;
            this.btnLastPage.Text = ">>";
            this.btnLastPage.UseVisualStyleBackColor = false;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.BackColor = System.Drawing.Color.White;
            this.btnFirstPage.FlatAppearance.BorderSize = 0;
            this.btnFirstPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstPage.ForeColor = System.Drawing.Color.Black;
            this.btnFirstPage.Location = new System.Drawing.Point(26, 2);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(33, 25);
            this.btnFirstPage.TabIndex = 14;
            this.btnFirstPage.Text = "<<";
            this.btnFirstPage.UseVisualStyleBackColor = false;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.Enabled = false;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Location = new System.Drawing.Point(302, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(27, 25);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "> ";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPageNo
            // 
            this.lblPageNo.BackColor = System.Drawing.Color.White;
            this.lblPageNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageNo.ForeColor = System.Drawing.Color.Black;
            this.lblPageNo.Location = new System.Drawing.Point(97, 3);
            this.lblPageNo.MinimumSize = new System.Drawing.Size(0, 22);
            this.lblPageNo.Name = "lblPageNo";
            this.lblPageNo.Size = new System.Drawing.Size(199, 22);
            this.lblPageNo.TabIndex = 7;
            this.lblPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.White;
            this.btnPrevious.Enabled = false;
            this.btnPrevious.FlatAppearance.BorderSize = 0;
            this.btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.ForeColor = System.Drawing.Color.Black;
            this.btnPrevious.Location = new System.Drawing.Point(63, 2);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(28, 25);
            this.btnPrevious.TabIndex = 13;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // grdBills
            // 
            this.grdBills.AllowUserToAddRows = false;
            this.grdBills.AllowUserToDeleteRows = false;
            this.grdBills.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBills.BackgroundColor = System.Drawing.Color.White;
            this.grdBills.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdBills.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdBills.ColumnHeadersHeight = 35;
            this.grdBills.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStockIdBills,
            this.colDocumentNoBills,
            this.colSupplierId,
            this.colSupplier,
            this.colCreateDate,
            this.colSupplierInvoiceNoBills,
            this.colSupplierInvoiceDate,
            this.colGrandTotal,
            this.colAmountDue,
            this.colTotalPaid,
            this.colCreatedAt,
            this.colPayment});
            this.grdBills.EnableHeadersVisualStyles = false;
            this.grdBills.Location = new System.Drawing.Point(3, 3);
            this.grdBills.Name = "grdBills";
            this.grdBills.RowHeadersVisible = false;
            this.grdBills.RowHeadersWidth = 51;
            this.grdBills.RowTemplate.Height = 35;
            this.grdBills.Size = new System.Drawing.Size(1210, 442);
            this.grdBills.TabIndex = 12;
            this.grdBills.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdBills_CellContentClick);
            // 
            // colStockIdBills
            // 
            this.colStockIdBills.HeaderText = "STOCK ID";
            this.colStockIdBills.MinimumWidth = 6;
            this.colStockIdBills.Name = "colStockIdBills";
            this.colStockIdBills.ReadOnly = true;
            this.colStockIdBills.Visible = false;
            this.colStockIdBills.Width = 125;
            // 
            // colDocumentNoBills
            // 
            this.colDocumentNoBills.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDocumentNoBills.HeaderText = "DOCUMENT NO";
            this.colDocumentNoBills.MinimumWidth = 6;
            this.colDocumentNoBills.Name = "colDocumentNoBills";
            this.colDocumentNoBills.ReadOnly = true;
            // 
            // colSupplierId
            // 
            this.colSupplierId.HeaderText = "SUPPLIER ID";
            this.colSupplierId.MinimumWidth = 6;
            this.colSupplierId.Name = "colSupplierId";
            this.colSupplierId.ReadOnly = true;
            this.colSupplierId.Visible = false;
            this.colSupplierId.Width = 125;
            // 
            // colSupplier
            // 
            this.colSupplier.HeaderText = "SUPPLIER";
            this.colSupplier.MinimumWidth = 6;
            this.colSupplier.Name = "colSupplier";
            this.colSupplier.ReadOnly = true;
            this.colSupplier.Width = 125;
            // 
            // colCreateDate
            // 
            this.colCreateDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCreateDate.HeaderText = "STOCK DATE";
            this.colCreateDate.MinimumWidth = 6;
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            // 
            // colSupplierInvoiceNoBills
            // 
            this.colSupplierInvoiceNoBills.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSupplierInvoiceNoBills.HeaderText = "SUPPLIER INVOICE#";
            this.colSupplierInvoiceNoBills.MinimumWidth = 6;
            this.colSupplierInvoiceNoBills.Name = "colSupplierInvoiceNoBills";
            this.colSupplierInvoiceNoBills.ReadOnly = true;
            // 
            // colSupplierInvoiceDate
            // 
            this.colSupplierInvoiceDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSupplierInvoiceDate.HeaderText = "SUPPLIER INVOICE DATE";
            this.colSupplierInvoiceDate.MinimumWidth = 155;
            this.colSupplierInvoiceDate.Name = "colSupplierInvoiceDate";
            this.colSupplierInvoiceDate.ReadOnly = true;
            // 
            // colGrandTotal
            // 
            this.colGrandTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGrandTotal.HeaderText = "GRAND TOTAL";
            this.colGrandTotal.MinimumWidth = 70;
            this.colGrandTotal.Name = "colGrandTotal";
            this.colGrandTotal.ReadOnly = true;
            // 
            // colAmountDue
            // 
            this.colAmountDue.HeaderText = "AMOUNT DUE";
            this.colAmountDue.MinimumWidth = 100;
            this.colAmountDue.Name = "colAmountDue";
            this.colAmountDue.Width = 125;
            // 
            // colTotalPaid
            // 
            this.colTotalPaid.HeaderText = "TOTAL PAID";
            this.colTotalPaid.MinimumWidth = 100;
            this.colTotalPaid.Name = "colTotalPaid";
            this.colTotalPaid.Width = 125;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.HeaderText = "CREATED AT";
            this.colCreatedAt.MinimumWidth = 6;
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            this.colCreatedAt.Width = 125;
            // 
            // colPayment
            // 
            this.colPayment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPayment.HeaderText = "PAYMENT";
            this.colPayment.MinimumWidth = 6;
            this.colPayment.Name = "colPayment";
            this.colPayment.ReadOnly = true;
            this.colPayment.Text = "Payment";
            this.colPayment.UseColumnTextForButtonValue = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "INVOICE NO";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "TOTAL";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "PAID";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "PAYMENT DATE";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "CREATE DATE";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.HeaderText = "PRINT";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "EDIT";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "RETURN";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 125;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "TOTAL PAID";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 125;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "CREATED AT";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 125;
            // 
            // btnRefreshForm
            // 
            this.btnRefreshForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshForm.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshForm.Image")));
            this.btnRefreshForm.Location = new System.Drawing.Point(1128, 10);
            this.btnRefreshForm.Name = "btnRefreshForm";
            this.btnRefreshForm.Size = new System.Drawing.Size(100, 36);
            this.btnRefreshForm.TabIndex = 20;
            this.btnRefreshForm.Text = "Refresh";
            this.btnRefreshForm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefreshForm.UseVisualStyleBackColor = true;
            this.btnRefreshForm.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // BillPaymentsUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1240, 549);
            this.Controls.Add(this.btnRefreshForm);
            this.Controls.Add(this.panel5);
            this.Name = "BillPaymentsUI";
            this.Text = "Accounts :: Bill Payments";
            this.Load += new System.EventHandler(this.TransactionUI_Load);
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBills)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.SaveFileDialog dlgSavePdf;
        private System.Windows.Forms.SaveFileDialog dlgSaveExcel;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView grdBills;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtGotoPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockIdBills;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentNoBills;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceNoBills;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrandTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmountDue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewButtonColumn colPayment;
        private System.Windows.Forms.Button btnRefreshForm;
    }
}