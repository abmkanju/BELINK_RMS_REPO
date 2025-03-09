namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmBed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBed));
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddBed = new System.Windows.Forms.Button();
            this.btnClearForm = new System.Windows.Forms.Button();
            this.errRoomNo = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.grdItems = new System.Windows.Forms.DataGridView();
            this.colBedId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoomNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChargesPerDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.errWard = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCharges = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.errCharges = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoomNo = new System.Windows.Forms.TextBox();
            this.cmbWard = new System.Windows.Forms.ComboBox();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Location = new System.Drawing.Point(1070, 536);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(96, 41);
            this.btnClearAll.TabIndex = 3;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(959, 536);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 41);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddBed
            // 
            this.btnAddBed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBed.Location = new System.Drawing.Point(822, 27);
            this.btnAddBed.Name = "btnAddBed";
            this.btnAddBed.Size = new System.Drawing.Size(84, 23);
            this.btnAddBed.TabIndex = 4;
            this.btnAddBed.Text = "Add Bed";
            this.btnAddBed.UseVisualStyleBackColor = true;
            this.btnAddBed.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnClearForm
            // 
            this.btnClearForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearForm.Location = new System.Drawing.Point(910, 26);
            this.btnClearForm.Name = "btnClearForm";
            this.btnClearForm.Size = new System.Drawing.Size(70, 24);
            this.btnClearForm.TabIndex = 5;
            this.btnClearForm.Text = "Clear ";
            this.btnClearForm.UseVisualStyleBackColor = true;
            this.btnClearForm.Click += new System.EventHandler(this.btnClearForm_Click);
            // 
            // errRoomNo
            // 
            this.errRoomNo.AutoSize = true;
            this.errRoomNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errRoomNo.ForeColor = System.Drawing.Color.Red;
            this.errRoomNo.Location = new System.Drawing.Point(524, 32);
            this.errRoomNo.MaximumSize = new System.Drawing.Size(15, 15);
            this.errRoomNo.Name = "errRoomNo";
            this.errRoomNo.Size = new System.Drawing.Size(15, 15);
            this.errRoomNo.TabIndex = 95;
            this.errRoomNo.Text = "*";
            this.errRoomNo.Visible = false;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(21, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 74;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // grdItems
            // 
            this.grdItems.AllowUserToAddRows = false;
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.BackgroundColor = System.Drawing.Color.White;
            this.grdItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdItems.ColumnHeadersHeight = 35;
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBedId,
            this.colWardId,
            this.colWard,
            this.colRoomNo,
            this.colChargesPerDay,
            this.colEdit,
            this.colRemove});
            this.grdItems.EnableHeadersVisualStyles = false;
            this.grdItems.Location = new System.Drawing.Point(12, 112);
            this.grdItems.Name = "grdItems";
            this.grdItems.RowHeadersVisible = false;
            this.grdItems.RowTemplate.Height = 35;
            this.grdItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdItems.Size = new System.Drawing.Size(1154, 418);
            this.grdItems.TabIndex = 1;
            this.grdItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdItems_CellContentClick);
            // 
            // colBedId
            // 
            this.colBedId.HeaderText = "BedId";
            this.colBedId.Name = "colBedId";
            this.colBedId.ReadOnly = true;
            // 
            // colWardId
            // 
            this.colWardId.HeaderText = "WardId";
            this.colWardId.Name = "colWardId";
            this.colWardId.ReadOnly = true;
            // 
            // colWard
            // 
            this.colWard.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colWard.HeaderText = "Ward";
            this.colWard.Name = "colWard";
            this.colWard.ReadOnly = true;
            // 
            // colRoomNo
            // 
            this.colRoomNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRoomNo.HeaderText = "Bed/Room#";
            this.colRoomNo.Name = "colRoomNo";
            this.colRoomNo.ReadOnly = true;
            // 
            // colChargesPerDay
            // 
            this.colChargesPerDay.HeaderText = "Charges Per Day";
            this.colChargesPerDay.Name = "colChargesPerDay";
            this.colChargesPerDay.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 40;
            // 
            // colRemove
            // 
            this.colRemove.HeaderText = "Remove";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 50;
            // 
            // errWard
            // 
            this.errWard.AutoSize = true;
            this.errWard.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errWard.ForeColor = System.Drawing.Color.Red;
            this.errWard.Location = new System.Drawing.Point(258, 32);
            this.errWard.MaximumSize = new System.Drawing.Size(15, 15);
            this.errWard.Name = "errWard";
            this.errWard.Size = new System.Drawing.Size(15, 15);
            this.errWard.TabIndex = 112;
            this.errWard.Text = "*";
            this.errWard.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 111;
            this.label4.Text = "Select Ward";
            // 
            // txtCharges
            // 
            this.txtCharges.Location = new System.Drawing.Point(551, 29);
            this.txtCharges.Name = "txtCharges";
            this.txtCharges.Size = new System.Drawing.Size(246, 20);
            this.txtCharges.TabIndex = 3;
            this.txtCharges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCharges_KeyDown);
            this.txtCharges.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharges_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(548, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 115;
            this.label5.Text = "Charger Per Day";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.errCharges);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtRoomNo);
            this.panel1.Controls.Add(this.cmbWard);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.errRoomNo);
            this.panel1.Controls.Add(this.txtCharges);
            this.panel1.Controls.Add(this.btnClearForm);
            this.panel1.Controls.Add(this.btnAddBed);
            this.panel1.Controls.Add(this.errWard);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(12, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1154, 72);
            this.panel1.TabIndex = 0;
            // 
            // errCharges
            // 
            this.errCharges.AutoSize = true;
            this.errCharges.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errCharges.ForeColor = System.Drawing.Color.Red;
            this.errCharges.Location = new System.Drawing.Point(801, 31);
            this.errCharges.MaximumSize = new System.Drawing.Size(15, 15);
            this.errCharges.Name = "errCharges";
            this.errCharges.Size = new System.Drawing.Size(15, 15);
            this.errCharges.TabIndex = 125;
            this.errCharges.Text = "*";
            this.errCharges.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(272, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 124;
            this.label1.Text = "Bed/Room#";
            // 
            // txtRoomNo
            // 
            this.txtRoomNo.Location = new System.Drawing.Point(275, 31);
            this.txtRoomNo.Name = "txtRoomNo";
            this.txtRoomNo.Size = new System.Drawing.Size(246, 20);
            this.txtRoomNo.TabIndex = 2;
            // 
            // cmbWard
            // 
            this.cmbWard.FormattingEnabled = true;
            this.cmbWard.Location = new System.Drawing.Point(12, 31);
            this.cmbWard.Name = "cmbWard";
            this.cmbWard.Size = new System.Drawing.Size(243, 21);
            this.cmbWard.TabIndex = 1;
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1157, 0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 28);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 120;
            this.pbInfo.TabStop = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // frmBed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1187, 589);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.grdItems);
            this.Name = "frmBed";
            this.Text = "Purchase Order";
            this.Load += new System.EventHandler(this.frmPurchaseOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddBed;
        private System.Windows.Forms.Button btnClearForm;
        private System.Windows.Forms.Label errRoomNo;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.DataGridView grdItems;
        private System.Windows.Forms.Label errWard;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCharges;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.ComboBox cmbWard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoomNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWard;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoomNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChargesPerDay;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.Label errCharges;
    }
}