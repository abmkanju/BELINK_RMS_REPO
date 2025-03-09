namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmAddSupplier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddSupplier));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSuppName = new System.Windows.Forms.TextBox();
            this.numOpBal = new System.Windows.Forms.NumericUpDown();
            this.txtAddress = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrimContName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPrimaryContPh = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrSuppName = new System.Windows.Forms.Label();
            this.ErrPhoneNo = new System.Windows.Forms.Label();
            this.ErrAddress = new System.Windows.Forms.Label();
            this.ErrPrimContName = new System.Windows.Forms.Label();
            this.ErrPrimContPh = new System.Windows.Forms.Label();
            this.ErrOpBal = new System.Windows.Forms.Label();
            this.pnlSupplierItems = new System.Windows.Forms.Panel();
            this.grdSupplierItems = new System.Windows.Forms.DataGridView();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.txtntn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtstn = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numcreditlimit = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numOpBal)).BeginInit();
            this.pnlSupplierItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupplierItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numcreditlimit)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Supplier Name";
            // 
            // txtSuppName
            // 
            this.txtSuppName.Location = new System.Drawing.Point(150, 22);
            this.txtSuppName.Name = "txtSuppName";
            this.txtSuppName.Size = new System.Drawing.Size(221, 21);
            this.txtSuppName.TabIndex = 1;
            // 
            // numOpBal
            // 
            this.numOpBal.Location = new System.Drawing.Point(150, 219);
            this.numOpBal.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.numOpBal.Name = "numOpBal";
            this.numOpBal.Size = new System.Drawing.Size(221, 21);
            this.numOpBal.TabIndex = 6;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(150, 100);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(541, 58);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Phone#";
            // 
            // txtPrimContName
            // 
            this.txtPrimContName.Location = new System.Drawing.Point(150, 176);
            this.txtPrimContName.Name = "txtPrimContName";
            this.txtPrimContName.Size = new System.Drawing.Size(221, 21);
            this.txtPrimContName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Contact Person";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(386, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "Person Phone";
            // 
            // txtPrimaryContPh
            // 
            this.txtPrimaryContPh.Location = new System.Drawing.Point(470, 176);
            this.txtPrimaryContPh.Name = "txtPrimaryContPh";
            this.txtPrimaryContPh.Size = new System.Drawing.Size(221, 21);
            this.txtPrimaryContPh.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 14);
            this.label8.TabIndex = 14;
            this.label8.Text = "Opening Balance";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(470, 307);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(221, 36);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSupplier.ForeColor = System.Drawing.Color.Black;
            this.btnAddSupplier.Location = new System.Drawing.Point(470, 265);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(221, 36);
            this.btnAddSupplier.TabIndex = 7;
            this.btnAddSupplier.Text = "Add";
            this.btnAddSupplier.UseVisualStyleBackColor = false;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Red;
            this.ErrMessage.Location = new System.Drawing.Point(30, 276);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrSuppName
            // 
            this.ErrSuppName.AutoSize = true;
            this.ErrSuppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSuppName.ForeColor = System.Drawing.Color.Red;
            this.ErrSuppName.Location = new System.Drawing.Point(374, 22);
            this.ErrSuppName.Name = "ErrSuppName";
            this.ErrSuppName.Size = new System.Drawing.Size(21, 25);
            this.ErrSuppName.TabIndex = 36;
            this.ErrSuppName.Text = "*";
            this.ErrSuppName.Visible = false;
            // 
            // ErrPhoneNo
            // 
            this.ErrPhoneNo.AutoSize = true;
            this.ErrPhoneNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPhoneNo.ForeColor = System.Drawing.Color.Red;
            this.ErrPhoneNo.Location = new System.Drawing.Point(694, 62);
            this.ErrPhoneNo.Name = "ErrPhoneNo";
            this.ErrPhoneNo.Size = new System.Drawing.Size(21, 25);
            this.ErrPhoneNo.TabIndex = 37;
            this.ErrPhoneNo.Text = "*";
            this.ErrPhoneNo.Visible = false;
            // 
            // ErrAddress
            // 
            this.ErrAddress.AutoSize = true;
            this.ErrAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrAddress.ForeColor = System.Drawing.Color.Red;
            this.ErrAddress.Location = new System.Drawing.Point(123, 103);
            this.ErrAddress.Name = "ErrAddress";
            this.ErrAddress.Size = new System.Drawing.Size(21, 25);
            this.ErrAddress.TabIndex = 38;
            this.ErrAddress.Text = "*";
            this.ErrAddress.Visible = false;
            // 
            // ErrPrimContName
            // 
            this.ErrPrimContName.AutoSize = true;
            this.ErrPrimContName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPrimContName.ForeColor = System.Drawing.Color.Red;
            this.ErrPrimContName.Location = new System.Drawing.Point(372, 175);
            this.ErrPrimContName.Name = "ErrPrimContName";
            this.ErrPrimContName.Size = new System.Drawing.Size(21, 25);
            this.ErrPrimContName.TabIndex = 39;
            this.ErrPrimContName.Text = "*";
            this.ErrPrimContName.Visible = false;
            // 
            // ErrPrimContPh
            // 
            this.ErrPrimContPh.AutoSize = true;
            this.ErrPrimContPh.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPrimContPh.ForeColor = System.Drawing.Color.Red;
            this.ErrPrimContPh.Location = new System.Drawing.Point(694, 176);
            this.ErrPrimContPh.Name = "ErrPrimContPh";
            this.ErrPrimContPh.Size = new System.Drawing.Size(21, 25);
            this.ErrPrimContPh.TabIndex = 40;
            this.ErrPrimContPh.Text = "*";
            this.ErrPrimContPh.Visible = false;
            // 
            // ErrOpBal
            // 
            this.ErrOpBal.AutoSize = true;
            this.ErrOpBal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrOpBal.ForeColor = System.Drawing.Color.Red;
            this.ErrOpBal.Location = new System.Drawing.Point(371, 219);
            this.ErrOpBal.Name = "ErrOpBal";
            this.ErrOpBal.Size = new System.Drawing.Size(21, 25);
            this.ErrOpBal.TabIndex = 41;
            this.ErrOpBal.Text = "*";
            this.ErrOpBal.Visible = false;
            // 
            // pnlSupplierItems
            // 
            this.pnlSupplierItems.Controls.Add(this.grdSupplierItems);
            this.pnlSupplierItems.Controls.Add(this.label9);
            this.pnlSupplierItems.Location = new System.Drawing.Point(33, 355);
            this.pnlSupplierItems.Name = "pnlSupplierItems";
            this.pnlSupplierItems.Size = new System.Drawing.Size(658, 319);
            this.pnlSupplierItems.TabIndex = 42;
            this.pnlSupplierItems.Visible = false;
            // 
            // grdSupplierItems
            // 
            this.grdSupplierItems.AllowUserToAddRows = false;
            this.grdSupplierItems.AllowUserToDeleteRows = false;
            this.grdSupplierItems.BackgroundColor = System.Drawing.Color.White;
            this.grdSupplierItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdSupplierItems.ColumnHeadersHeight = 34;
            this.grdSupplierItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemName,
            this.colItemType});
            this.grdSupplierItems.EnableHeadersVisualStyles = false;
            this.grdSupplierItems.Location = new System.Drawing.Point(1, 38);
            this.grdSupplierItems.Name = "grdSupplierItems";
            this.grdSupplierItems.RowHeadersVisible = false;
            this.grdSupplierItems.RowTemplate.Height = 35;
            this.grdSupplierItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSupplierItems.Size = new System.Drawing.Size(601, 326);
            this.grdSupplierItems.TabIndex = 9;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colItemType
            // 
            this.colItemType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemType.HeaderText = "Item Type";
            this.colItemType.Name = "colItemType";
            this.colItemType.ReadOnly = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(3, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(604, 37);
            this.label9.TabIndex = 2;
            this.label9.Text = "Supplier Items";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Item Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1048, 0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 30);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 100;
            this.pbInfo.TabStop = false;
            this.pbInfo.Visible = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(470, 63);
            this.txtPhone.Mask = "00000000000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(221, 21);
            this.txtPhone.TabIndex = 101;
            // 
            // txtntn
            // 
            this.txtntn.Location = new System.Drawing.Point(470, 18);
            this.txtntn.Name = "txtntn";
            this.txtntn.Size = new System.Drawing.Size(221, 21);
            this.txtntn.TabIndex = 103;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(413, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 14);
            this.label10.TabIndex = 102;
            this.label10.Text = "NTN#";
            // 
            // txtstn
            // 
            this.txtstn.Location = new System.Drawing.Point(150, 60);
            this.txtstn.Name = "txtstn";
            this.txtstn.Size = new System.Drawing.Size(221, 21);
            this.txtstn.TabIndex = 105;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 14);
            this.label11.TabIndex = 104;
            this.label11.Text = "STN#";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(395, 221);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 14);
            this.label12.TabIndex = 107;
            this.label12.Text = "Credit Limit";
            // 
            // numcreditlimit
            // 
            this.numcreditlimit.Location = new System.Drawing.Point(470, 214);
            this.numcreditlimit.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.numcreditlimit.Name = "numcreditlimit";
            this.numcreditlimit.Size = new System.Drawing.Size(185, 21);
            this.numcreditlimit.TabIndex = 106;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(661, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 14);
            this.label13.TabIndex = 108;
            this.label13.Text = "days";
            // 
            // frmAddSupplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1077, 579);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numcreditlimit);
            this.Controls.Add(this.txtstn);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtntn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.pnlSupplierItems);
            this.Controls.Add(this.ErrOpBal);
            this.Controls.Add(this.ErrPrimContPh);
            this.Controls.Add(this.ErrPrimContName);
            this.Controls.Add(this.ErrAddress);
            this.Controls.Add(this.ErrPhoneNo);
            this.Controls.Add(this.ErrSuppName);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAddSupplier);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPrimaryContPh);
            this.Controls.Add(this.txtPrimContName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.numOpBal);
            this.Controls.Add(this.txtSuppName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmAddSupplier";
            this.Text = "Add Supplier";
            this.Load += new System.EventHandler(this.frmAddSupplier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numOpBal)).EndInit();
            this.pnlSupplierItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSupplierItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numcreditlimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSuppName;
        private System.Windows.Forms.NumericUpDown numOpBal;
        private System.Windows.Forms.RichTextBox txtAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrimContName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPrimaryContPh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrSuppName;
        private System.Windows.Forms.Label ErrPhoneNo;
        private System.Windows.Forms.Label ErrAddress;
        private System.Windows.Forms.Label ErrPrimContName;
        private System.Windows.Forms.Label ErrPrimContPh;
        private System.Windows.Forms.Label ErrOpBal;
        private System.Windows.Forms.Panel pnlSupplierItems;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView grdSupplierItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.TextBox txtntn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtstn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numcreditlimit;
        private System.Windows.Forms.Label label13;
    }
}