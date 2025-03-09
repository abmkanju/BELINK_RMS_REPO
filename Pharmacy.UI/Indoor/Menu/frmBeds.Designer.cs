namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmBeds
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBeds));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblPageNo = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.grdWards = new System.Windows.Forms.DataGridView();
            this.btnRefreshForm = new System.Windows.Forms.Button();
            this.btnAddBed = new System.Windows.Forms.Button();
            this.colBedId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoomNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChargesPerDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWards)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnLastPage);
            this.panel1.Controls.Add(this.btnFirstPage);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.lblPageNo);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Location = new System.Drawing.Point(252, 477);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 29);
            this.panel1.TabIndex = 23;
            // 
            // btnLastPage
            // 
            this.btnLastPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnLastPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastPage.ForeColor = System.Drawing.Color.White;
            this.btnLastPage.Location = new System.Drawing.Point(333, 2);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(32, 23);
            this.btnLastPage.TabIndex = 11;
            this.btnLastPage.Text = ">>";
            this.btnLastPage.UseVisualStyleBackColor = false;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnFirstPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstPage.ForeColor = System.Drawing.Color.White;
            this.btnFirstPage.Location = new System.Drawing.Point(32, 2);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(33, 23);
            this.btnFirstPage.TabIndex = 14;
            this.btnFirstPage.Text = "<<";
            this.btnFirstPage.UseVisualStyleBackColor = false;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnNext.Enabled = false;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(305, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(27, 23);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "> ";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPageNo
            // 
            this.lblPageNo.BackColor = System.Drawing.SystemColors.Desktop;
            this.lblPageNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageNo.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblPageNo.Location = new System.Drawing.Point(100, 3);
            this.lblPageNo.MinimumSize = new System.Drawing.Size(0, 20);
            this.lblPageNo.Name = "lblPageNo";
            this.lblPageNo.Size = new System.Drawing.Size(199, 20);
            this.lblPageNo.TabIndex = 7;
            this.lblPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnPrevious.Enabled = false;
            this.btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevious.ForeColor = System.Drawing.Color.White;
            this.btnPrevious.Location = new System.Drawing.Point(66, 2);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(28, 23);
            this.btnPrevious.TabIndex = 13;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // grdWards
            // 
            this.grdWards.AllowUserToAddRows = false;
            this.grdWards.AllowUserToDeleteRows = false;
            this.grdWards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdWards.BackgroundColor = System.Drawing.Color.White;
            this.grdWards.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdWards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdWards.ColumnHeadersHeight = 35;
            this.grdWards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBedId,
            this.colWardId,
            this.colWard,
            this.colRoomNo,
            this.colChargesPerDay,
            this.colEdit,
            this.colDelete});
            this.grdWards.EnableHeadersVisualStyles = false;
            this.grdWards.Location = new System.Drawing.Point(8, 45);
            this.grdWards.Name = "grdWards";
            this.grdWards.RowHeadersVisible = false;
            this.grdWards.RowTemplate.Height = 35;
            this.grdWards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdWards.Size = new System.Drawing.Size(888, 421);
            this.grdWards.TabIndex = 22;
            this.grdWards.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdWards_CellContentClick);
            // 
            // btnRefreshForm
            // 
            this.btnRefreshForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshForm.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshForm.Image")));
            this.btnRefreshForm.Location = new System.Drawing.Point(796, 6);
            this.btnRefreshForm.Name = "btnRefreshForm";
            this.btnRefreshForm.Size = new System.Drawing.Size(100, 33);
            this.btnRefreshForm.TabIndex = 21;
            this.btnRefreshForm.Text = "Refresh";
            this.btnRefreshForm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnRefreshForm.UseVisualStyleBackColor = true;
            this.btnRefreshForm.Click += new System.EventHandler(this.btnRefreshForm_Click);
            // 
            // btnAddBed
            // 
            this.btnAddBed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBed.Location = new System.Drawing.Point(686, 6);
            this.btnAddBed.Name = "btnAddBed";
            this.btnAddBed.Size = new System.Drawing.Size(104, 33);
            this.btnAddBed.TabIndex = 20;
            this.btnAddBed.Text = "Add Bed";
            this.btnAddBed.UseVisualStyleBackColor = true;
            this.btnAddBed.Click += new System.EventHandler(this.btnAddBed_Click);
            // 
            // colBedId
            // 
            this.colBedId.HeaderText = "Bed Id";
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
            this.colEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 50;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "Delete";
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 50;
            // 
            // frmBeds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(904, 518);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grdWards);
            this.Controls.Add(this.btnRefreshForm);
            this.Controls.Add(this.btnAddBed);
            this.Name = "frmBeds";
            this.Text = "Beds";
            this.Load += new System.EventHandler(this.frmBeds_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdWards)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.DataGridView grdWards;
        private System.Windows.Forms.Button btnRefreshForm;
        private System.Windows.Forms.Button btnAddBed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWardId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWard;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoomNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChargesPerDay;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
    }
}