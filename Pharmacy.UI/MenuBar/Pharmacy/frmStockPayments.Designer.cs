using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmStockPayments
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.grdInvoices = new System.Windows.Forms.DataGridView();
            this.colStockId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDueAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaidAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckedPaidAmount = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.lblPaidAmount = new System.Windows.Forms.Label();
            this.lblDue = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DueAmountAfterCheckedTextbox = new System.Windows.Forms.Label();
            this.CheckedPaidTotalText = new System.Windows.Forms.Label();
            this.DueAmountAfterCheckedLabel = new System.Windows.Forms.Label();
            this.CheckedTotalPaymentLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(391, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "COLLECT STOCK PAYMENT";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1051, 354);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdInvoices
            // 
            this.grdInvoices.AllowUserToAddRows = false;
            this.grdInvoices.AllowUserToDeleteRows = false;
            this.grdInvoices.BackgroundColor = System.Drawing.Color.White;
            this.grdInvoices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdInvoices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdInvoices.ColumnHeadersHeight = 35;
            this.grdInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStockId,
            this.colDocumentNo,
            this.colSupplierInvoiceNo,
            this.colSupplierInvoiceDate,
            this.colTotalAmount,
            this.colPaymentMode,
            this.colDueAmount,
            this.colPaidAmount,
            this.colCheckedPaidAmount});
            this.grdInvoices.EnableHeadersVisualStyles = false;
            this.grdInvoices.Location = new System.Drawing.Point(47, 176);
            this.grdInvoices.Name = "grdInvoices";
            this.grdInvoices.RowHeadersVisible = false;
            this.grdInvoices.RowTemplate.Height = 35;
            this.grdInvoices.Size = new System.Drawing.Size(950, 351);
            this.grdInvoices.TabIndex = 2;
            this.grdInvoices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdInvoices_CellContentClick);
            this.grdInvoices.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdInvoices_CellValueChanged);
            // 
            // colStockId
            // 
            this.colStockId.HeaderText = "STOCK ID";
            this.colStockId.Name = "colStockId";
            this.colStockId.ReadOnly = true;
            this.colStockId.Visible = false;
            // 
            // colDocumentNo
            // 
            this.colDocumentNo.HeaderText = "Document No.";
            this.colDocumentNo.Name = "colDocumentNo";
            this.colDocumentNo.ReadOnly = true;
            // 
            // colSupplierInvoiceNo
            // 
            this.colSupplierInvoiceNo.HeaderText = "Invoice#";
            this.colSupplierInvoiceNo.Name = "colSupplierInvoiceNo";
            this.colSupplierInvoiceNo.ReadOnly = true;
            // 
            // colSupplierInvoiceDate
            // 
            this.colSupplierInvoiceDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSupplierInvoiceDate.HeaderText = "Invoice Date";
            this.colSupplierInvoiceDate.Name = "colSupplierInvoiceDate";
            this.colSupplierInvoiceDate.ReadOnly = true;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.HeaderText = "Total Amount";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.ReadOnly = true;
            // 
            // colPaymentMode
            // 
            this.colPaymentMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPaymentMode.HeaderText = "Payment Mode";
            this.colPaymentMode.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Debit/Credit Card",
            "Online Payment"});
            this.colPaymentMode.Name = "colPaymentMode";
            // 
            // colDueAmount
            // 
            this.colDueAmount.HeaderText = "Due Amount";
            this.colDueAmount.Name = "colDueAmount";
            this.colDueAmount.ReadOnly = true;
            this.colDueAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDueAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPaidAmount
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = "0";
            this.colPaidAmount.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPaidAmount.HeaderText = "Paid Aamount";
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colCheckedPaidAmount
            // 
            this.colCheckedPaidAmount.HeaderText = "Checked Paid Amount";
            this.colCheckedPaidAmount.MinimumWidth = 6;
            this.colCheckedPaidAmount.Name = "colCheckedPaidAmount";
            this.colCheckedPaidAmount.CellTemplate = new DataGridViewCheckBoxCell();
            this.colCheckedPaidAmount.Width = 125;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Supplier:";
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierName.Location = new System.Drawing.Point(123, 80);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(40, 24);
            this.lblSupplierName.TabIndex = 3;
            this.lblSupplierName.Text = ". . . ";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(438, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "Supplier Invoices";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1132, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1023, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Grand Total:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1023, 274);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Total Paid:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1023, 307);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Due:";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Location = new System.Drawing.Point(1098, 243);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(38, 14);
            this.lblGrandTotal.TabIndex = 5;
            this.lblGrandTotal.Text = "label5";
            // 
            // lblPaidAmount
            // 
            this.lblPaidAmount.AutoSize = true;
            this.lblPaidAmount.Location = new System.Drawing.Point(1091, 274);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(38, 14);
            this.lblPaidAmount.TabIndex = 5;
            this.lblPaidAmount.Text = "label5";
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.Location = new System.Drawing.Point(1057, 307);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(38, 14);
            this.lblDue.TabIndex = 5;
            this.lblDue.Text = "label5";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "STOCK ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "DOCUMENT NO";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "INVOICE#";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "INVOICE DATE";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "TOTAL AMOUNT";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "DUE AMOUNT";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "PAID AMOUNT";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // DueAmountAfterCheckedTextbox
            // 
            this.DueAmountAfterCheckedTextbox.AutoSize = true;
            this.DueAmountAfterCheckedTextbox.Location = new System.Drawing.Point(1158, 445);
            this.DueAmountAfterCheckedTextbox.Name = "DueAmountAfterCheckedTextbox";
            this.DueAmountAfterCheckedTextbox.Size = new System.Drawing.Size(44, 18);
            this.DueAmountAfterCheckedTextbox.TabIndex = 6;
            this.DueAmountAfterCheckedTextbox.Text = "label5";
            // 
            // CheckedPaidTotalText
            // 
            this.CheckedPaidTotalText.AutoSize = true;
            this.CheckedPaidTotalText.Location = new System.Drawing.Point(1131, 412);
            this.CheckedPaidTotalText.Name = "CheckedPaidTotalText";
            this.CheckedPaidTotalText.Size = new System.Drawing.Size(44, 18);
            this.CheckedPaidTotalText.TabIndex = 7;
            this.CheckedPaidTotalText.Text = "label5";
            // 
            // DueAmountAfterCheckedLabel
            // 
            this.DueAmountAfterCheckedLabel.AutoSize = true;
            this.DueAmountAfterCheckedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DueAmountAfterCheckedLabel.Location = new System.Drawing.Point(1003, 444);
            this.DueAmountAfterCheckedLabel.Name = "DueAmountAfterCheckedLabel";
            this.DueAmountAfterCheckedLabel.Size = new System.Drawing.Size(149, 17);
            this.DueAmountAfterCheckedLabel.TabIndex = 8;
            this.DueAmountAfterCheckedLabel.Text = "Due After Checked:";
            // 
            // CheckedTotalPaymentLabel
            // 
            this.CheckedTotalPaymentLabel.AutoSize = true;
            this.CheckedTotalPaymentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckedTotalPaymentLabel.Location = new System.Drawing.Point(1003, 413);
            this.CheckedTotalPaymentLabel.Name = "CheckedTotalPaymentLabel";
            this.CheckedTotalPaymentLabel.Size = new System.Drawing.Size(122, 17);
            this.CheckedTotalPaymentLabel.TabIndex = 9;
            this.CheckedTotalPaymentLabel.Text = "Checked Total :";
            // 
            // frmStockPayments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1235, 540);
            this.Controls.Add(this.DueAmountAfterCheckedTextbox);
            this.Controls.Add(this.CheckedPaidTotalText);
            this.Controls.Add(this.DueAmountAfterCheckedLabel);
            this.Controls.Add(this.CheckedTotalPaymentLabel);
            this.Controls.Add(this.lblDue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblPaidAmount);
            this.Controls.Add(this.lblGrandTotal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grdInvoices);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStockPayments";
            this.Text = "Collect Stock Payments";
            this.Load += new System.EventHandler(this.frmStockPayments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView grdInvoices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblPaidAmount;
        private System.Windows.Forms.Label lblDue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPaymentMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaidAmount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckedPaidAmount;
        private System.Windows.Forms.Label DueAmountAfterCheckedTextbox;
        private System.Windows.Forms.Label CheckedPaidTotalText;
        private System.Windows.Forms.Label DueAmountAfterCheckedLabel;
        private System.Windows.Forms.Label CheckedTotalPaymentLabel;
    }
}