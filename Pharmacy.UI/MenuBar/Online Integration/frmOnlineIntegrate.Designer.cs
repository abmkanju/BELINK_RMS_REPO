
namespace Pharmacy.UI.FranchiseModuels
{
    partial class frmOnlineIntegrate
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.CustName1 = new System.Windows.Forms.TextBox();
            this.OrderChange = new System.Windows.Forms.TextBox();
            this.grdOnline = new System.Windows.Forms.DataGridView();
            this.IsDelivered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cancel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsPack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDispatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyDispatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWalletAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrandTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPack = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDispatch = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelivered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOnline)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(594, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Search by Order #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Search by Customer Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(416, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Search by Phone#";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(823, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "To:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.txtPhone);
            this.panel1.Controls.Add(this.CustName1);
            this.panel1.Controls.Add(this.OrderChange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1474, 89);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1332, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 40);
            this.button1.TabIndex = 6;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(827, 54);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(221, 26);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(596, 54);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(225, 26);
            this.dtpFrom.TabIndex = 4;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Location = new System.Drawing.Point(420, 54);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(170, 26);
            this.txtPhone.TabIndex = 3;
            this.txtPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPhone_KeyDown);
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // CustName1
            // 
            this.CustName1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustName1.Location = new System.Drawing.Point(182, 54);
            this.CustName1.Name = "CustName1";
            this.CustName1.Size = new System.Drawing.Size(233, 26);
            this.CustName1.TabIndex = 2;
            this.CustName1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustName1_KeyDown);
            // 
            // OrderChange
            // 
            this.OrderChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderChange.Location = new System.Drawing.Point(13, 54);
            this.OrderChange.Name = "OrderChange";
            this.OrderChange.Size = new System.Drawing.Size(165, 26);
            this.OrderChange.TabIndex = 1;
            this.OrderChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OrderChange_KeyDown);
            this.OrderChange.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OrderChange_KeyPress);
            // 
            // grdOnline
            // 
            this.grdOnline.AllowUserToAddRows = false;
            this.grdOnline.AllowUserToDeleteRows = false;
            this.grdOnline.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdOnline.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdOnline.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOnline.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsDelivered,
            this.colOrderValue,
            this.colInvoiceId,
            this.Cancel,
            this.IsPack,
            this.IsDispatch,
            this.colItemId,
            this.colItemName,
            this.colQtyDispatch,
            this.colOrderId,
            this.colCustName,
            this.colAddress,
            this.colPhone,
            this.colOrigin,
            this.colDiscount,
            this.colWalletAmount,
            this.colGrandTotal,
            this.colCancelReason,
            this.colCreateDate,
            this.colPack,
            this.colDispatch,
            this.colDelivered,
            this.colCancel});
            this.grdOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOnline.Location = new System.Drawing.Point(0, 89);
            this.grdOnline.Name = "grdOnline";
            this.grdOnline.ReadOnly = true;
            this.grdOnline.Size = new System.Drawing.Size(1474, 717);
            this.grdOnline.TabIndex = 2;
            this.grdOnline.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdOnline_CellContentClick);
            // 
            // IsDelivered
            // 
            this.IsDelivered.HeaderText = "IsDelivered";
            this.IsDelivered.Name = "IsDelivered";
            this.IsDelivered.ReadOnly = true;
            this.IsDelivered.Visible = false;
            // 
            // colOrderValue
            // 
            this.colOrderValue.HeaderText = "Order Number";
            this.colOrderValue.Name = "colOrderValue";
            this.colOrderValue.ReadOnly = true;
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.HeaderText = "InvoiceId";
            this.colInvoiceId.Name = "colInvoiceId";
            this.colInvoiceId.ReadOnly = true;
            this.colInvoiceId.Visible = false;
            // 
            // Cancel
            // 
            this.Cancel.HeaderText = "Cancel";
            this.Cancel.Name = "Cancel";
            this.Cancel.ReadOnly = true;
            this.Cancel.Visible = false;
            // 
            // IsPack
            // 
            this.IsPack.HeaderText = "Pack";
            this.IsPack.Name = "IsPack";
            this.IsPack.ReadOnly = true;
            this.IsPack.Visible = false;
            // 
            // IsDispatch
            // 
            this.IsDispatch.HeaderText = "Dispatch";
            this.IsDispatch.Name = "IsDispatch";
            this.IsDispatch.ReadOnly = true;
            this.IsDispatch.Visible = false;
            // 
            // colItemId
            // 
            this.colItemId.HeaderText = "IteemsId";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            this.colItemId.Visible = false;
            // 
            // colItemName
            // 
            this.colItemName.HeaderText = "ItemName";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            this.colItemName.Visible = false;
            // 
            // colQtyDispatch
            // 
            this.colQtyDispatch.HeaderText = "QtyDispatch";
            this.colQtyDispatch.Name = "colQtyDispatch";
            this.colQtyDispatch.ReadOnly = true;
            this.colQtyDispatch.Visible = false;
            // 
            // colOrderId
            // 
            this.colOrderId.HeaderText = "OrderId";
            this.colOrderId.Name = "colOrderId";
            this.colOrderId.ReadOnly = true;
            this.colOrderId.Visible = false;
            // 
            // colCustName
            // 
            this.colCustName.HeaderText = "Customer Name";
            this.colCustName.Name = "colCustName";
            this.colCustName.ReadOnly = true;
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colOrigin
            // 
            this.colOrigin.HeaderText = "Origin";
            this.colOrigin.Name = "colOrigin";
            this.colOrigin.ReadOnly = true;
            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            // 
            // colWalletAmount
            // 
            this.colWalletAmount.HeaderText = "Wallet Credit";
            this.colWalletAmount.Name = "colWalletAmount";
            this.colWalletAmount.ReadOnly = true;
            // 
            // colGrandTotal
            // 
            this.colGrandTotal.HeaderText = "GrandTotal";
            this.colGrandTotal.Name = "colGrandTotal";
            this.colGrandTotal.ReadOnly = true;
            // 
            // colCancelReason
            // 
            this.colCancelReason.HeaderText = "CancelReason";
            this.colCancelReason.Name = "colCancelReason";
            this.colCancelReason.ReadOnly = true;
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "Create Date";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            // 
            // colPack
            // 
            this.colPack.HeaderText = "Pack";
            this.colPack.Name = "colPack";
            this.colPack.ReadOnly = true;
            this.colPack.Text = "Pack";
            this.colPack.ToolTipText = "Pack";
            this.colPack.UseColumnTextForButtonValue = true;
            // 
            // colDispatch
            // 
            this.colDispatch.HeaderText = "Dispatch";
            this.colDispatch.Name = "colDispatch";
            this.colDispatch.ReadOnly = true;
            this.colDispatch.Text = "Dispatch";
            this.colDispatch.ToolTipText = "Dispatch";
            this.colDispatch.UseColumnTextForButtonValue = true;
            // 
            // colDelivered
            // 
            this.colDelivered.HeaderText = "Delivered";
            this.colDelivered.Name = "colDelivered";
            this.colDelivered.ReadOnly = true;
            this.colDelivered.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDelivered.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCancel
            // 
            this.colCancel.HeaderText = "Cancel";
            this.colCancel.Name = "colCancel";
            this.colCancel.ReadOnly = true;
            this.colCancel.Text = "Cancel";
            this.colCancel.ToolTipText = "Cancel";
            this.colCancel.UseColumnTextForButtonValue = true;
            // 
            // frmOnlineIntegrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1474, 806);
            this.Controls.Add(this.grdOnline);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmOnlineIntegrate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Online Orders";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOnlineIntegrate_FormClosed);
            this.Load += new System.EventHandler(this.frmOnlineIntegrate_Load);
            this.VisibleChanged += new System.EventHandler(this.frmOnlineIntegrate_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOnline)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox CustName1;
        private System.Windows.Forms.TextBox OrderChange;
        private System.Windows.Forms.DataGridView grdOnline;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDelivered;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsPack;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDispatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyDispatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWalletAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrandTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewButtonColumn colPack;
        private System.Windows.Forms.DataGridViewButtonColumn colDispatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDelivered;
        private System.Windows.Forms.DataGridViewButtonColumn colCancel;
    }
}