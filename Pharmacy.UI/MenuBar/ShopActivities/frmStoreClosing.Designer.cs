namespace Pharmacy.UI.MenuBar.ShopActivities
{
    partial class frmStoreClosing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStoreClosing));
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpClosingDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCS_Detail = new System.Windows.Forms.Button();
            this.btnCS_ShowDetail = new System.Windows.Forms.Button();
            this.txxCS_CashDiff = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txxCS_PhyCash = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txxCS_SysCash = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txxCS_TOutFlow = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txxCS_TInflow = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txxCS_OpCash = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtClosingCash = new System.Windows.Forms.TextBox();
            this.txtTCashSubmitted = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtHOSubmitted = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBankSubmitted = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefreshForm = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.radLabel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // line1
            // 
            this.line1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.line1.Location = new System.Drawing.Point(-1, 47);
            this.line1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1198, 10);
            this.line1.TabIndex = 102;
            this.line1.Text = "line1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(112, 17);
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
            this.label4.Location = new System.Drawing.Point(125, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 104;
            this.label4.Text = "Store Closing";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 105;
            this.label3.Text = "Shop Activities";
            this.label3.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(777, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 24);
            this.label1.TabIndex = 106;
            this.label1.Text = "Closing Date";
            // 
            // dtpClosingDate
            // 
            this.dtpClosingDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpClosingDate.CustomFormat = "yyyy-MMM-dd HH:mm:ss";
            this.dtpClosingDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.dtpClosingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpClosingDate.Location = new System.Drawing.Point(899, 63);
            this.dtpClosingDate.Name = "dtpClosingDate";
            this.dtpClosingDate.Size = new System.Drawing.Size(271, 29);
            this.dtpClosingDate.TabIndex = 107;
            this.dtpClosingDate.ValueChanged += new System.EventHandler(this.dtpClosingDate_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox1.Controls.Add(this.btnCS_Detail);
            this.groupBox1.Controls.Add(this.btnCS_ShowDetail);
            this.groupBox1.Controls.Add(this.txxCS_CashDiff);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txxCS_PhyCash);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txxCS_SysCash);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txxCS_TOutFlow);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txxCS_TInflow);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txxCS_OpCash);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(18, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 345);
            this.groupBox1.TabIndex = 108;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cash Summary";
            // 
            // btnCS_Detail
            // 
            this.btnCS_Detail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCS_Detail.Location = new System.Drawing.Point(565, 173);
            this.btnCS_Detail.Name = "btnCS_Detail";
            this.btnCS_Detail.Size = new System.Drawing.Size(87, 32);
            this.btnCS_Detail.TabIndex = 117;
            this.btnCS_Detail.Text = "detail";
            this.btnCS_Detail.UseVisualStyleBackColor = true;
            this.btnCS_Detail.Visible = false;
            this.btnCS_Detail.Click += new System.EventHandler(this.btnCS_Detail_Click);
            // 
            // btnCS_ShowDetail
            // 
            this.btnCS_ShowDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCS_ShowDetail.Location = new System.Drawing.Point(441, 280);
            this.btnCS_ShowDetail.Name = "btnCS_ShowDetail";
            this.btnCS_ShowDetail.Size = new System.Drawing.Size(211, 47);
            this.btnCS_ShowDetail.TabIndex = 117;
            this.btnCS_ShowDetail.Tag = "0";
            this.btnCS_ShowDetail.Text = "Show Detail";
            this.btnCS_ShowDetail.UseVisualStyleBackColor = true;
            this.btnCS_ShowDetail.Visible = false;
            this.btnCS_ShowDetail.Click += new System.EventHandler(this.btnCS_ShowDetail_Click);
            // 
            // txxCS_CashDiff
            // 
            this.txxCS_CashDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_CashDiff.Location = new System.Drawing.Point(110, 235);
            this.txxCS_CashDiff.Name = "txxCS_CashDiff";
            this.txxCS_CashDiff.ReadOnly = true;
            this.txxCS_CashDiff.Size = new System.Drawing.Size(542, 29);
            this.txxCS_CashDiff.TabIndex = 115;
            this.txxCS_CashDiff.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 116;
            this.label10.Text = "Cash Difference";
            // 
            // txxCS_PhyCash
            // 
            this.txxCS_PhyCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_PhyCash.Location = new System.Drawing.Point(321, 176);
            this.txxCS_PhyCash.Name = "txxCS_PhyCash";
            this.txxCS_PhyCash.Size = new System.Drawing.Size(238, 29);
            this.txxCS_PhyCash.TabIndex = 113;
            this.txxCS_PhyCash.TextChanged += new System.EventHandler(this.txxCS_PhyCash_TextChanged);
            this.txxCS_PhyCash.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txxCS_PhyCash_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(318, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 114;
            this.label9.Text = "Physical Cash";
            // 
            // txxCS_SysCash
            // 
            this.txxCS_SysCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_SysCash.Location = new System.Drawing.Point(18, 185);
            this.txxCS_SysCash.Name = "txxCS_SysCash";
            this.txxCS_SysCash.ReadOnly = true;
            this.txxCS_SysCash.Size = new System.Drawing.Size(278, 29);
            this.txxCS_SysCash.TabIndex = 111;
            this.txxCS_SysCash.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 112;
            this.label8.Text = "System Cash";
            // 
            // txxCS_TOutFlow
            // 
            this.txxCS_TOutFlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_TOutFlow.Location = new System.Drawing.Point(321, 117);
            this.txxCS_TOutFlow.Name = "txxCS_TOutFlow";
            this.txxCS_TOutFlow.ReadOnly = true;
            this.txxCS_TOutFlow.Size = new System.Drawing.Size(331, 29);
            this.txxCS_TOutFlow.TabIndex = 109;
            this.txxCS_TOutFlow.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(318, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 110;
            this.label7.Text = "Total Outflow";
            // 
            // txxCS_TInflow
            // 
            this.txxCS_TInflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_TInflow.Location = new System.Drawing.Point(18, 117);
            this.txxCS_TInflow.Name = "txxCS_TInflow";
            this.txxCS_TInflow.ReadOnly = true;
            this.txxCS_TInflow.Size = new System.Drawing.Size(278, 29);
            this.txxCS_TInflow.TabIndex = 107;
            this.txxCS_TInflow.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(15, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 108;
            this.label6.Text = "Total Inflow";
            // 
            // txxCS_OpCash
            // 
            this.txxCS_OpCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txxCS_OpCash.Location = new System.Drawing.Point(18, 56);
            this.txxCS_OpCash.Name = "txxCS_OpCash";
            this.txxCS_OpCash.Size = new System.Drawing.Size(278, 29);
            this.txxCS_OpCash.TabIndex = 0;
            this.txxCS_OpCash.Leave += new System.EventHandler(this.txxCS_OpCash_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 106;
            this.label2.Text = "Opening Cash";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox2.Controls.Add(this.txtClosingCash);
            this.groupBox2.Controls.Add(this.txtTCashSubmitted);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtHOSubmitted);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtBankSubmitted);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(697, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(473, 345);
            this.groupBox2.TabIndex = 118;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cash Submission";
            // 
            // txtClosingCash
            // 
            this.txtClosingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtClosingCash.Location = new System.Drawing.Point(18, 235);
            this.txtClosingCash.Name = "txtClosingCash";
            this.txtClosingCash.ReadOnly = true;
            this.txtClosingCash.Size = new System.Drawing.Size(416, 29);
            this.txtClosingCash.TabIndex = 115;
            // 
            // txtTCashSubmitted
            // 
            this.txtTCashSubmitted.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtTCashSubmitted.Location = new System.Drawing.Point(16, 173);
            this.txtTCashSubmitted.Name = "txtTCashSubmitted";
            this.txtTCashSubmitted.ReadOnly = true;
            this.txtTCashSubmitted.Size = new System.Drawing.Size(418, 29);
            this.txtTCashSubmitted.TabIndex = 115;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 214);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 13);
            this.label13.TabIndex = 116;
            this.label13.Text = "Closing Cash(Manager)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 116;
            this.label11.Text = "Total Cash Submitted";
            // 
            // txtHOSubmitted
            // 
            this.txtHOSubmitted.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtHOSubmitted.Location = new System.Drawing.Point(16, 117);
            this.txtHOSubmitted.Name = "txtHOSubmitted";
            this.txtHOSubmitted.Size = new System.Drawing.Size(418, 29);
            this.txtHOSubmitted.TabIndex = 113;
            this.txtHOSubmitted.TextChanged += new System.EventHandler(this.txtHOSubmitted_TextChanged);
            this.txtHOSubmitted.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHOSubmitted_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 114;
            this.label12.Text = "Submitted to HO";
            // 
            // txtBankSubmitted
            // 
            this.txtBankSubmitted.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtBankSubmitted.Location = new System.Drawing.Point(18, 56);
            this.txtBankSubmitted.Name = "txtBankSubmitted";
            this.txtBankSubmitted.Size = new System.Drawing.Size(418, 29);
            this.txtBankSubmitted.TabIndex = 0;
            this.txtBankSubmitted.TextChanged += new System.EventHandler(this.txtBankSubmitted_TextChanged);
            this.txtBankSubmitted.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBankSubmitted_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 35);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 13);
            this.label16.TabIndex = 106;
            this.label16.Text = "Submitted to Bank";
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(899, 466);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(271, 33);
            this.btnSave.TabIndex = 117;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(899, 514);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(271, 33);
            this.btnCancel.TabIndex = 117;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRefreshForm
            // 
            this.btnRefreshForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshForm.Font = new System.Drawing.Font("Segoe UI", 12.25F);
            this.btnRefreshForm.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshForm.Image")));
            this.btnRefreshForm.Location = new System.Drawing.Point(1023, 12);
            this.btnRefreshForm.Name = "btnRefreshForm";
            this.btnRefreshForm.Size = new System.Drawing.Size(147, 33);
            this.btnRefreshForm.TabIndex = 1030;
            this.btnRefreshForm.Text = "Refresh";
            this.btnRefreshForm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefreshForm.UseVisualStyleBackColor = true;
            this.btnRefreshForm.Click += new System.EventHandler(this.btnRefreshForm_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.radLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1196, 57);
            this.panel1.TabIndex = 1031;
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Controls.Add(this.btnRefreshForm);
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 25.25F);
            this.radLabel1.Location = new System.Drawing.Point(0, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(1196, 57);
            this.radLabel1.TabIndex = 113;
            this.radLabel1.Text = "STORE CLOSING";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmStoreClosing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1196, 605);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dtpClosingDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.line1);
            this.Name = "frmStoreClosing";
            this.Text = "Store Closing";
            this.Load += new System.EventHandler(this.frmStoreClosing_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.radLabel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpClosingDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txxCS_CashDiff;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txxCS_PhyCash;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txxCS_SysCash;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txxCS_TOutFlow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txxCS_TInflow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txxCS_OpCash;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCS_ShowDetail;
        private System.Windows.Forms.Button btnCS_Detail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTCashSubmitted;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtHOSubmitted;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBankSubmitted;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtClosingCash;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefreshForm;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}