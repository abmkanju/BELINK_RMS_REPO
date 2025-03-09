namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmPatientInvoices
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatientInvoices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdInvoices = new System.Windows.Forms.DataGridView();
            this.lblPatientInfo = new System.Windows.Forms.Label();
            this.btnCreateInvoice = new System.Windows.Forms.Button();
            this.btnPrintInvoiceHistory = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRefund = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colPrintInvoice = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colPrintInvoiceRefund = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).BeginInit();
            this.SuspendLayout();
            // 
            // grdInvoices
            // 
            this.grdInvoices.AllowUserToAddRows = false;
            this.grdInvoices.AllowUserToDeleteRows = false;
            this.grdInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdInvoices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.grdInvoices.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.grdInvoices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdInvoices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colInvoiceId,
            this.colInvoiceNo,
            this.colDescription,
            this.colTotal,
            this.colPaid,
            this.colDues,
            this.colPaymentDate,
            this.colModifiedDate,
            this.colUser,
            this.colEmail,
            this.colEdit,
            this.colRefund,
            this.colPrintInvoice,
            this.colPrintInvoiceRefund,
            this.colDelete});
            this.grdInvoices.Location = new System.Drawing.Point(12, 68);
            this.grdInvoices.Name = "grdInvoices";
            this.grdInvoices.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdInvoices.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdInvoices.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.grdInvoices.Size = new System.Drawing.Size(1199, 459);
            this.grdInvoices.TabIndex = 0;
            this.grdInvoices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdInvoices_CellContentClick);
            // 
            // lblPatientInfo
            // 
            this.lblPatientInfo.AutoSize = true;
            this.lblPatientInfo.Location = new System.Drawing.Point(17, 23);
            this.lblPatientInfo.Name = "lblPatientInfo";
            this.lblPatientInfo.Size = new System.Drawing.Size(47, 14);
            this.lblPatientInfo.TabIndex = 1;
            this.lblPatientInfo.Text = "000000-";
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateInvoice.Location = new System.Drawing.Point(993, 23);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Size = new System.Drawing.Size(137, 29);
            this.btnCreateInvoice.TabIndex = 2;
            this.btnCreateInvoice.Text = "Create Invoice";
            this.btnCreateInvoice.UseVisualStyleBackColor = true;
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // btnPrintInvoiceHistory
            // 
            this.btnPrintInvoiceHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintInvoiceHistory.Location = new System.Drawing.Point(850, 23);
            this.btnPrintInvoiceHistory.Name = "btnPrintInvoiceHistory";
            this.btnPrintInvoiceHistory.Size = new System.Drawing.Size(137, 29);
            this.btnPrintInvoiceHistory.TabIndex = 2;
            this.btnPrintInvoiceHistory.Text = "Print Invoice History";
            this.btnPrintInvoiceHistory.UseVisualStyleBackColor = true;
            this.btnPrintInvoiceHistory.Click += new System.EventHandler(this.btnPrintInvoiceHistory_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1113, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 34);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(1136, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 36);
            this.btnRefresh.TabIndex = 40;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Sr#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "InvoiceNo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "Total";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn5.Frozen = true;
            this.dataGridViewTextBoxColumn5.HeaderText = "Paid";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn6.Frozen = true;
            this.dataGridViewTextBoxColumn6.HeaderText = "Dues";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.Frozen = true;
            this.dataGridViewTextBoxColumn7.HeaderText = "Payment Date";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.Frozen = true;
            this.dataGridViewTextBoxColumn8.HeaderText = "Modified Date";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.Frozen = true;
            this.dataGridViewTextBoxColumn9.HeaderText = "User";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // colSrNo
            // 
            this.colSrNo.Frozen = true;
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            this.colSrNo.Width = 50;
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.HeaderText = "InvoiceId";
            this.colInvoiceId.Name = "colInvoiceId";
            this.colInvoiceId.ReadOnly = true;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.HeaderText = "InvoiceNo";
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colDescription.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colTotal
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colTotal.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            // 
            // colPaid
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colPaid.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPaid.HeaderText = "Paid";
            this.colPaid.Name = "colPaid";
            this.colPaid.ReadOnly = true;
            // 
            // colDues
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.colDues.DefaultCellStyle = dataGridViewCellStyle5;
            this.colDues.HeaderText = "Dues";
            this.colDues.Name = "colDues";
            this.colDues.ReadOnly = true;
            // 
            // colPaymentDate
            // 
            this.colPaymentDate.HeaderText = "Payment Date";
            this.colPaymentDate.Name = "colPaymentDate";
            this.colPaymentDate.ReadOnly = true;
            // 
            // colModifiedDate
            // 
            this.colModifiedDate.HeaderText = "Modified Date";
            this.colModifiedDate.Name = "colModifiedDate";
            this.colModifiedDate.ReadOnly = true;
            // 
            // colUser
            // 
            this.colUser.HeaderText = "User";
            this.colUser.Name = "colUser";
            this.colUser.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEmail.DefaultCellStyle = dataGridViewCellStyle6;
            this.colEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colEmail.Text = "Email";
            this.colEmail.UseColumnTextForButtonValue = true;
            this.colEmail.Width = 43;
            // 
            // colEdit
            // 
            this.colEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 33;
            // 
            // colRefund
            // 
            this.colRefund.HeaderText = "Refund";
            this.colRefund.Name = "colRefund";
            this.colRefund.ReadOnly = true;
            this.colRefund.Text = "Refund";
            this.colRefund.UseColumnTextForButtonValue = true;
            this.colRefund.Width = 50;
            // 
            // colPrintInvoice
            // 
            this.colPrintInvoice.HeaderText = "Print Invoice";
            this.colPrintInvoice.Name = "colPrintInvoice";
            this.colPrintInvoice.ReadOnly = true;
            this.colPrintInvoice.Text = "Print Incvoice";
            this.colPrintInvoice.UseColumnTextForButtonValue = true;
            this.colPrintInvoice.Width = 80;
            // 
            // colPrintInvoiceRefund
            // 
            this.colPrintInvoiceRefund.HeaderText = "Print Refunfd";
            this.colPrintInvoiceRefund.Name = "colPrintInvoiceRefund";
            this.colPrintInvoiceRefund.ReadOnly = true;
            this.colPrintInvoiceRefund.Text = "Print Refund";
            this.colPrintInvoiceRefund.UseColumnTextForButtonValue = true;
            this.colPrintInvoiceRefund.Width = 60;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "Delete";
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 60;
            // 
            // frmPatientInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 580);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrintInvoiceHistory);
            this.Controls.Add(this.btnCreateInvoice);
            this.Controls.Add(this.lblPatientInfo);
            this.Controls.Add(this.grdInvoices);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmPatientInvoices";
            this.Text = "Patient Invoices";
            this.Load += new System.EventHandler(this.frmPatientInvoices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdInvoices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdInvoices;
        private System.Windows.Forms.Label lblPatientInfo;
        private System.Windows.Forms.Button btnCreateInvoice;
        private System.Windows.Forms.Button btnPrintInvoiceHistory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDues;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifiedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewButtonColumn colEmail;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRefund;
        private System.Windows.Forms.DataGridViewButtonColumn colPrintInvoice;
        private System.Windows.Forms.DataGridViewButtonColumn colPrintInvoiceRefund;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
    }
}