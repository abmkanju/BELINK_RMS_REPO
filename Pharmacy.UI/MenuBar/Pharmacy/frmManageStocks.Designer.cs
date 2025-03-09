namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmManageStocks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageStocks));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddNewStock = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.dlgSaveExcel = new System.Windows.Forms.SaveFileDialog();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.txtSearchByName = new System.Windows.Forms.TextBox();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSupplierInvoice = new System.Windows.Forms.TextBox();
            this.cmbStore = new System.Windows.Forms.ComboBox();
            this.txtStore = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDocNo = new System.Windows.Forms.TextBox();
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
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblPageNo = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.txtTotalPurchase = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtGotoPage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountedPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscounttype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWHT = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(7, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(146, 36);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Reload Record";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAddNewStock
            // 
            this.btnAddNewStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewStock.Location = new System.Drawing.Point(159, 11);
            this.btnAddNewStock.Name = "btnAddNewStock";
            this.btnAddNewStock.Size = new System.Drawing.Size(131, 36);
            this.btnAddNewStock.TabIndex = 7;
            this.btnAddNewStock.Text = "Add New Stock";
            this.btnAddNewStock.UseVisualStyleBackColor = true;
            this.btnAddNewStock.Click += new System.EventHandler(this.btnAddNewStock_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(364, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(57, 36);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(301, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(57, 36);
            this.btnExcel.TabIndex = 5;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Visible = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // dlgSaveExcel
            // 
            this.dlgSaveExcel.FileName = "StocksList";
            this.dlgSaveExcel.Filter = "Excel|*.xls";
            // 
            // lblTo
            // 
            this.lblTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(817, 8);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(19, 14);
            this.lblTo.TabIndex = 21;
            this.lblTo.Text = "To";
            // 
            // lblFrom
            // 
            this.lblFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(611, 8);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 14);
            this.lblFrom.TabIndex = 18;
            this.lblFrom.Text = "From";
            // 
            // dtpTo
            // 
            this.dtpTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpTo.Checked = false;
            this.dtpTo.Location = new System.Drawing.Point(820, 26);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.Size = new System.Drawing.Size(200, 21);
            this.dtpTo.TabIndex = 2;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFrom.Checked = false;
            this.dtpFrom.Location = new System.Drawing.Point(614, 26);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.Size = new System.Drawing.Size(200, 21);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // txtSearchByName
            // 
            this.txtSearchByName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchByName.Location = new System.Drawing.Point(1026, 26);
            this.txtSearchByName.Name = "txtSearchByName";
            this.txtSearchByName.Size = new System.Drawing.Size(160, 21);
            this.txtSearchByName.TabIndex = 3;
            this.txtSearchByName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchByName_KeyDown);
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSuppliers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(1192, 25);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(160, 22);
            this.cmbSuppliers.TabIndex = 4;
            this.cmbSuppliers.SelectedIndexChanged += new System.EventHandler(this.cmbSuppliers_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1022, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 14);
            this.label2.TabIndex = 18;
            this.label2.Text = "Search By Item Name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1189, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 14);
            this.label3.TabIndex = 18;
            this.label3.Text = "Search By Supplier";
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1373, 34);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 30);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 100;
            this.pbInfo.TabStop = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(77, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 17);
            this.label5.TabIndex = 103;
            this.label5.Text = ">";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(184)))), ((int)(((byte)(224)))));
            this.label4.Location = new System.Drawing.Point(90, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 17);
            this.label4.TabIndex = 104;
            this.label4.Text = "Manage Stocks";
            this.label4.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 105;
            this.label1.Text = "RMS";
            this.label1.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(928, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 14);
            this.label7.TabIndex = 107;
            this.label7.Text = "Search By Supplier Invoice";
            this.label7.Visible = false;
            // 
            // txtSupplierInvoice
            // 
            this.txtSupplierInvoice.Location = new System.Drawing.Point(931, 76);
            this.txtSupplierInvoice.Name = "txtSupplierInvoice";
            this.txtSupplierInvoice.Size = new System.Drawing.Size(160, 21);
            this.txtSupplierInvoice.TabIndex = 108;
            this.txtSupplierInvoice.Visible = false;
            this.txtSupplierInvoice.TextChanged += new System.EventHandler(this.txtSupplierInvoice_TextChanged);
            this.txtSupplierInvoice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSupplierInvoice_KeyDown);
            // 
            // cmbStore
            // 
            this.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStore.FormattingEnabled = true;
            this.cmbStore.Location = new System.Drawing.Point(1100, 76);
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.Size = new System.Drawing.Size(160, 22);
            this.cmbStore.TabIndex = 109;
            this.cmbStore.Visible = false;
            this.cmbStore.SelectedIndexChanged += new System.EventHandler(this.cmbStore_SelectedIndexChanged);
            // 
            // txtStore
            // 
            this.txtStore.AutoSize = true;
            this.txtStore.Location = new System.Drawing.Point(1097, 59);
            this.txtStore.Name = "txtStore";
            this.txtStore.Size = new System.Drawing.Size(86, 14);
            this.txtStore.TabIndex = 110;
            this.txtStore.Text = "Search By Store";
            this.txtStore.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(751, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 14);
            this.label9.TabIndex = 112;
            this.label9.Text = "Search By Stock Document No";
            this.label9.Visible = false;
            // 
            // txtDocNo
            // 
            this.txtDocNo.Location = new System.Drawing.Point(754, 78);
            this.txtDocNo.Name = "txtDocNo";
            this.txtDocNo.Size = new System.Drawing.Size(158, 21);
            this.txtDocNo.TabIndex = 113;
            this.txtDocNo.Visible = false;
            this.txtDocNo.TextChanged += new System.EventHandler(this.txtDocNo_TextChanged);
            this.txtDocNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDocNo_KeyDown);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ItemId";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Item";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "UnitCost";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Total Cost";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Retail Price";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Batch";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Discount";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Sales Tax";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Create At";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Net Value";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Create At";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Create At";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddNewStock);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.dtpFrom);
            this.panel3.Controls.Add(this.dtpTo);
            this.panel3.Controls.Add(this.lblFrom);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblTo);
            this.panel3.Controls.Add(this.txtSearchByName);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.cmbSuppliers);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1370, 56);
            this.panel3.TabIndex = 114;
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(0, 56);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1370, 10);
            this.line1.TabIndex = 115;
            this.line1.Text = "line1";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.txtTotalPurchase);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.flowPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 66);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1370, 495);
            this.panel2.TabIndex = 116;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblPageNo);
            this.panel4.Controls.Add(this.btnPrevious);
            this.panel4.Controls.Add(this.btnNext);
            this.panel4.Controls.Add(this.btnLastPage);
            this.panel4.Controls.Add(this.btnFirstPage);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 452);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1370, 43);
            this.panel4.TabIndex = 21;
            // 
            // lblPageNo
            // 
            this.lblPageNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPageNo.BackColor = System.Drawing.Color.Transparent;
            this.lblPageNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageNo.ForeColor = System.Drawing.Color.Black;
            this.lblPageNo.Location = new System.Drawing.Point(77, 7);
            this.lblPageNo.MinimumSize = new System.Drawing.Size(0, 22);
            this.lblPageNo.Name = "lblPageNo";
            this.lblPageNo.Size = new System.Drawing.Size(90, 22);
            this.lblPageNo.TabIndex = 7;
            this.lblPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevious.Enabled = false;
            this.btnPrevious.FlatAppearance.BorderSize = 0;
            this.btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.ForeColor = System.Drawing.Color.Black;
            this.btnPrevious.Location = new System.Drawing.Point(43, 6);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(28, 25);
            this.btnPrevious.TabIndex = 13;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.Enabled = false;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Location = new System.Drawing.Point(173, 6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(27, 25);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "> ";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLastPage.BackColor = System.Drawing.Color.Transparent;
            this.btnLastPage.FlatAppearance.BorderSize = 0;
            this.btnLastPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastPage.ForeColor = System.Drawing.Color.Black;
            this.btnLastPage.Location = new System.Drawing.Point(204, 6);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(35, 25);
            this.btnLastPage.TabIndex = 11;
            this.btnLastPage.Text = ">>";
            this.btnLastPage.UseVisualStyleBackColor = false;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFirstPage.BackColor = System.Drawing.Color.Transparent;
            this.btnFirstPage.FlatAppearance.BorderSize = 0;
            this.btnFirstPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstPage.ForeColor = System.Drawing.Color.Black;
            this.btnFirstPage.Location = new System.Drawing.Point(6, 6);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(33, 25);
            this.btnFirstPage.TabIndex = 14;
            this.btnFirstPage.Text = "<<";
            this.btnFirstPage.UseVisualStyleBackColor = false;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // txtTotalPurchase
            // 
            this.txtTotalPurchase.AutoSize = true;
            this.txtTotalPurchase.Location = new System.Drawing.Point(94, 9);
            this.txtTotalPurchase.Name = "txtTotalPurchase";
            this.txtTotalPurchase.Size = new System.Drawing.Size(0, 14);
            this.txtTotalPurchase.TabIndex = 20;
            this.txtTotalPurchase.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 14);
            this.label8.TabIndex = 19;
            this.label8.Text = "Total Purchase:";
            this.label8.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtGotoPage);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(844, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 31);
            this.panel1.TabIndex = 16;
            this.panel1.Visible = false;
            // 
            // txtGotoPage
            // 
            this.txtGotoPage.Location = new System.Drawing.Point(433, 4);
            this.txtGotoPage.Name = "txtGotoPage";
            this.txtGotoPage.Size = new System.Drawing.Size(75, 21);
            this.txtGotoPage.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(373, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 14);
            this.label6.TabIndex = 0;
            this.label6.Text = "go to page";
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.BackColor = System.Drawing.Color.White;
            this.flowPanel.Controls.Add(this.dataGridView1);
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.Location = new System.Drawing.Point(0, 0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(1370, 495);
            this.flowPanel.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colItemId,
            this.colItemName,
            this.colQuantity,
            this.colUnitCost,
            this.DiscountedPrice,
            this.colToalCost,
            this.colRetailPrice,
            this.colBatch,
            this.colDiscount,
            this.colDiscounttype,
            this.colSalesTax,
            this.colWHT,
            this.colNetValue,
            this.colCreatedAt});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.Size = new System.Drawing.Size(1363, 0);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.Visible = false;
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr#";
            this.colSrNo.Name = "colSrNo";
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
            // DiscountedPrice
            // 
            this.DiscountedPrice.HeaderText = "Disc Price";
            this.DiscountedPrice.Name = "DiscountedPrice";
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
            // colDiscounttype
            // 
            this.colDiscounttype.HeaderText = "Discount type";
            this.colDiscounttype.Name = "colDiscounttype";
            // 
            // colSalesTax
            // 
            this.colSalesTax.HeaderText = "Sales Tax";
            this.colSalesTax.Name = "colSalesTax";
            this.colSalesTax.ReadOnly = true;
            // 
            // colWHT
            // 
            // 
            // 
            // 
            this.colWHT.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.colWHT.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colWHT.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colWHT.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.colWHT.HeaderText = "WHT";
            this.colWHT.Increment = 1D;
            this.colWHT.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.colWHT.Name = "colWHT";
            this.colWHT.ReadOnly = true;
            this.colWHT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            // frmManageStocks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1370, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.txtDocNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.txtStore);
            this.Controls.Add(this.txtSupplierInvoice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pbInfo);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmManageStocks";
            this.Text = "Manage Stock";
            this.Load += new System.EventHandler(this.frmManageStocks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddNewStock;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.SaveFileDialog dlgSaveExcel;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TextBox txtSearchByName;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSupplierInvoice;
        private System.Windows.Forms.ComboBox cmbStore;
        private System.Windows.Forms.Label txtStore;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label txtTotalPurchase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtGotoPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountedPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscounttype;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTax;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colWHT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Panel panel4;
    }
}