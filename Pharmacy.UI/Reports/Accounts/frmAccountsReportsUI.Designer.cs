namespace Pharmacy.UI.Reports.Accounts
{
    partial class frmAccountsReportsUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountsReportsUI));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnAccoutns = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBillPayments = new System.Windows.Forms.Button();
            this.btnPatientLedger = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.radTileElement1 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement2 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement3 = new Telerik.WinControls.UI.RadTileElement();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "default-patient.png");
            this.imageList1.Images.SetKeyName(1, "Refresh_16.png");
            // 
            // btnAccoutns
            // 
            this.btnAccoutns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccoutns.AutoSize = true;
            this.btnAccoutns.BackColor = System.Drawing.Color.White;
            this.btnAccoutns.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAccoutns.FlatAppearance.BorderSize = 0;
            this.btnAccoutns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccoutns.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnAccoutns.Image = ((System.Drawing.Image)(resources.GetObject("btnAccoutns.Image")));
            this.btnAccoutns.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAccoutns.Location = new System.Drawing.Point(20, 5);
            this.btnAccoutns.Margin = new System.Windows.Forms.Padding(20, 5, 20, 3);
            this.btnAccoutns.Name = "btnAccoutns";
            this.btnAccoutns.Size = new System.Drawing.Size(227, 107);
            this.btnAccoutns.TabIndex = 0;
            this.btnAccoutns.Text = "Accounts";
            this.btnAccoutns.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnAccoutns.UseCompatibleTextRendering = true;
            this.btnAccoutns.UseVisualStyleBackColor = false;
            this.btnAccoutns.Click += new System.EventHandler(this.btnTransaction_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnBillPayments, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAccoutns, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPatientLedger, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 274);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(802, 115);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Visible = false;
            // 
            // btnBillPayments
            // 
            this.btnBillPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBillPayments.AutoSize = true;
            this.btnBillPayments.BackColor = System.Drawing.Color.White;
            this.btnBillPayments.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnBillPayments.FlatAppearance.BorderSize = 0;
            this.btnBillPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBillPayments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnBillPayments.Image = ((System.Drawing.Image)(resources.GetObject("btnBillPayments.Image")));
            this.btnBillPayments.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBillPayments.Location = new System.Drawing.Point(287, 5);
            this.btnBillPayments.Margin = new System.Windows.Forms.Padding(20, 5, 20, 3);
            this.btnBillPayments.Name = "btnBillPayments";
            this.btnBillPayments.Size = new System.Drawing.Size(227, 107);
            this.btnBillPayments.TabIndex = 1;
            this.btnBillPayments.Text = "Bill Payments";
            this.btnBillPayments.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnBillPayments.UseCompatibleTextRendering = true;
            this.btnBillPayments.UseVisualStyleBackColor = false;
            this.btnBillPayments.Click += new System.EventHandler(this.btnShift_Click);
            // 
            // btnPatientLedger
            // 
            this.btnPatientLedger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatientLedger.AutoSize = true;
            this.btnPatientLedger.BackColor = System.Drawing.Color.White;
            this.btnPatientLedger.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnPatientLedger.FlatAppearance.BorderSize = 0;
            this.btnPatientLedger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPatientLedger.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnPatientLedger.Image = ((System.Drawing.Image)(resources.GetObject("btnPatientLedger.Image")));
            this.btnPatientLedger.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPatientLedger.Location = new System.Drawing.Point(554, 5);
            this.btnPatientLedger.Margin = new System.Windows.Forms.Padding(20, 5, 20, 3);
            this.btnPatientLedger.Name = "btnPatientLedger";
            this.btnPatientLedger.Size = new System.Drawing.Size(228, 107);
            this.btnPatientLedger.TabIndex = 0;
            this.btnPatientLedger.Text = "Patient Ledger";
            this.btnPatientLedger.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnPatientLedger.UseCompatibleTextRendering = true;
            this.btnPatientLedger.UseVisualStyleBackColor = false;
            this.btnPatientLedger.Click += new System.EventHandler(this.btnPatientLedger_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.radLabel1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.line1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 57);
            this.panel1.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(71, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 109;
            this.label6.Text = ">";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(184)))), ((int)(((byte)(224)))));
            this.label7.Location = new System.Drawing.Point(84, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 17);
            this.label7.TabIndex = 110;
            this.label7.Text = "Account Reports";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 17);
            this.label8.TabIndex = 111;
            this.label8.Text = "Reports";
            // 
            // line1
            // 
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(1, 46);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1103, 10);
            this.line1.TabIndex = 106;
            this.line1.Text = "line1";
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.BackColor = System.Drawing.Color.AliceBlue;
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 25.25F);
            this.radLabel1.Location = new System.Drawing.Point(0, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(1100, 57);
            this.radLabel1.TabIndex = 114;
            this.radLabel1.Text = "Account Reports";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radPanorama1
            // 
            this.radPanorama1.CellSize = new System.Drawing.Size(250, 130);
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanorama1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radTileElement1,
            this.radTileElement2,
            this.radTileElement3});
            this.radPanorama1.Location = new System.Drawing.Point(0, 57);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.Size = new System.Drawing.Size(1100, 197);
            this.radPanorama1.TabIndex = 4;
            // 
            // radTileElement1
            // 
            this.radTileElement1.Image = ((System.Drawing.Image)(resources.GetObject("radTileElement1.Image")));
            this.radTileElement1.Name = "radTileElement1";
            this.radTileElement1.Text = "Accounts";
            this.radTileElement1.Click += new System.EventHandler(this.radTileElement1_Click);
            // 
            // radTileElement2
            // 
            this.radTileElement2.Column = 1;
            this.radTileElement2.Image = ((System.Drawing.Image)(resources.GetObject("radTileElement2.Image")));
            this.radTileElement2.Name = "radTileElement2";
            this.radTileElement2.Text = "Bill Payments";
            this.radTileElement2.Click += new System.EventHandler(this.radTileElement2_Click);
            // 
            // radTileElement3
            // 
            this.radTileElement3.Column = 2;
            this.radTileElement3.Image = ((System.Drawing.Image)(resources.GetObject("radTileElement3.Image")));
            this.radTileElement3.Name = "radTileElement3";
            this.radTileElement3.Text = "Customer Ledger";
            this.radTileElement3.Click += new System.EventHandler(this.radTileElement3_Click);
            // 
            // frmAccountsReportsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1100, 411);
            this.Controls.Add(this.radPanorama1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmAccountsReportsUI";
            this.Text = "Account Reports";
            this.Load += new System.EventHandler(this.frmFinancialReportsUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnAccoutns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnBillPayments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Button btnPatientLedger;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadTileElement radTileElement1;
        private Telerik.WinControls.UI.RadTileElement radTileElement2;
        private Telerik.WinControls.UI.RadTileElement radTileElement3;
    }
}