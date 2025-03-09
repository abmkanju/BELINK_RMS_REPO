
namespace Pharmacy.UI.MenuBar.OrderVoucherPayment
{
    partial class frmVoucherPayments
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
            this.grdInvoices = new System.Windows.Forms.DataGridView();
            this.colInvoiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDueAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaidAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPaidAmount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdInvoices
            // 
            this.grdInvoices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceId,
            this.colUserId,
            this.colSupplierInvoiceNo,
            this.colSupplierInvoiceDate,
            this.colTotalAmount,
            this.colPaymentMode,
            this.colDueAmount,
            this.colPaidAmount});
            this.grdInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInvoices.Location = new System.Drawing.Point(0, 0);
            this.grdInvoices.Name = "grdInvoices";
            this.grdInvoices.Size = new System.Drawing.Size(1030, 591);
            this.grdInvoices.TabIndex = 0;
            this.grdInvoices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdInvoices_CellContentClick);
            this.grdInvoices.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdInvoices_CellValueChanged);
            this.grdInvoices.ClientSizeChanged += new System.EventHandler(this.grdInvoices_ClientSizeChanged);
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.HeaderText = "InvoiceId";
            this.colInvoiceId.Name = "colInvoiceId";
            this.colInvoiceId.ReadOnly = true;
            this.colInvoiceId.Visible = false;
            // 
            // colUserId
            // 
            this.colUserId.HeaderText = "UserId";
            this.colUserId.Name = "colUserId";
            this.colUserId.ReadOnly = true;
            this.colUserId.Visible = false;
            // 
            // colSupplierInvoiceNo
            // 
            this.colSupplierInvoiceNo.HeaderText = "Invoice#";
            this.colSupplierInvoiceNo.Name = "colSupplierInvoiceNo";
            this.colSupplierInvoiceNo.ReadOnly = true;
            // 
            // colSupplierInvoiceDate
            // 
            this.colSupplierInvoiceDate.HeaderText = "Invoice Date";
            this.colSupplierInvoiceDate.Name = "colSupplierInvoiceDate";
            this.colSupplierInvoiceDate.ReadOnly = true;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.HeaderText = "Grand Total";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.ReadOnly = true;
            // 
            // colPaymentMode
            // 
            this.colPaymentMode.HeaderText = "Payment Mode";
            this.colPaymentMode.Name = "colPaymentMode";
            // 
            // colDueAmount
            // 
            this.colDueAmount.HeaderText = "Due  Amount";
            this.colDueAmount.Name = "colDueAmount";
            this.colDueAmount.ReadOnly = true;
            this.colDueAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDueAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPaidAmount
            // 
            this.colPaidAmount.HeaderText = "Paid Amount";
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grand Total:";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lblGrandTotal.Location = new System.Drawing.Point(118, 26);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(16, 17);
            this.lblGrandTotal.TabIndex = 4;
            this.lblGrandTotal.Text = "0";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(830, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 42);
            this.btnSave.TabIndex = 7;
            this.btnSave.Tag = "";
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDue);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblPaidAmount);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.lblGrandTotal);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 520);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1030, 71);
            this.panel1.TabIndex = 8;
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lblDue.Location = new System.Drawing.Point(474, 26);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(16, 17);
            this.lblDue.TabIndex = 12;
            this.lblDue.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(374, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Due Amount:";
            // 
            // lblPaidAmount
            // 
            this.lblPaidAmount.AutoSize = true;
            this.lblPaidAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lblPaidAmount.Location = new System.Drawing.Point(279, 26);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(16, 17);
            this.lblPaidAmount.TabIndex = 10;
            this.lblPaidAmount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(179, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Total Paid:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(930, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 42);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmVoucherPayments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1030, 591);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grdInvoices);
            this.Name = "frmVoucherPayments";
            this.Text = "frmVoucherPayment";
            this.Load += new System.EventHandler(this.frmVoucherPayments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdInvoices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPaymentMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaidAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPaidAmount;
        private System.Windows.Forms.Label label3;
    }
}