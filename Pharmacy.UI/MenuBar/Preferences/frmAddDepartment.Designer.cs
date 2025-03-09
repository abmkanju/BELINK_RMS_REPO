namespace Pharmacy.UI.MenuBar.Preferences
{
    partial class frmAddDepartment
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
            this.cmbDepartments = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.grdsp = new System.Windows.Forms.DataGridView();
            this.colSubDepartmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtSubDep = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrItemName = new System.Windows.Forms.Label();
            this.errSelectDep = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdsp)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDepartments
            // 
            this.cmbDepartments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDepartments.FormattingEnabled = true;
            this.cmbDepartments.Location = new System.Drawing.Point(110, 46);
            this.cmbDepartments.Name = "cmbDepartments";
            this.cmbDepartments.Size = new System.Drawing.Size(580, 21);
            this.cmbDepartments.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Department";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(696, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 37);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdsp
            // 
            this.grdsp.AllowUserToAddRows = false;
            this.grdsp.AllowUserToDeleteRows = false;
            this.grdsp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdsp.BackgroundColor = System.Drawing.Color.White;
            this.grdsp.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdsp.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdsp.ColumnHeadersHeight = 35;
            this.grdsp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSubDepartmentId,
            this.colDepartmentName,
            this.colEdit,
            this.colRemove});
            this.grdsp.EnableHeadersVisualStyles = false;
            this.grdsp.Location = new System.Drawing.Point(12, 134);
            this.grdsp.Name = "grdsp";
            this.grdsp.RowHeadersVisible = false;
            this.grdsp.RowTemplate.Height = 35;
            this.grdsp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdsp.Size = new System.Drawing.Size(759, 286);
            this.grdsp.TabIndex = 10;
            this.grdsp.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdsp_CellContentClick);
            // 
            // colSubDepartmentId
            // 
            this.colSubDepartmentId.HeaderText = "Department ID";
            this.colSubDepartmentId.Name = "colSubDepartmentId";
            this.colSubDepartmentId.ReadOnly = true;
            this.colSubDepartmentId.Visible = false;
            // 
            // colDepartmentName
            // 
            this.colDepartmentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDepartmentName.HeaderText = "Department Name";
            this.colDepartmentName.Name = "colDepartmentName";
            this.colDepartmentName.ReadOnly = true;
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
            // colRemove
            // 
            this.colRemove.HeaderText = "Remove";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 50;
            // 
            // txtSubDep
            // 
            this.txtSubDep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubDep.Location = new System.Drawing.Point(12, 100);
            this.txtSubDep.Name = "txtSubDep";
            this.txtSubDep.Size = new System.Drawing.Size(678, 20);
            this.txtSubDep.TabIndex = 11;
            this.txtSubDep.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSubDep_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(696, 100);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Department ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Department Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(9, 8);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrItemName
            // 
            this.ErrItemName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrItemName.AutoSize = true;
            this.ErrItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrItemName.ForeColor = System.Drawing.Color.Red;
            this.ErrItemName.Location = new System.Drawing.Point(669, 85);
            this.ErrItemName.MaximumSize = new System.Drawing.Size(21, 12);
            this.ErrItemName.Name = "ErrItemName";
            this.ErrItemName.Size = new System.Drawing.Size(21, 12);
            this.ErrItemName.TabIndex = 36;
            this.ErrItemName.Text = "*";
            this.ErrItemName.Visible = false;
            // 
            // errSelectDep
            // 
            this.errSelectDep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errSelectDep.AutoSize = true;
            this.errSelectDep.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errSelectDep.ForeColor = System.Drawing.Color.Red;
            this.errSelectDep.Location = new System.Drawing.Point(669, 31);
            this.errSelectDep.MaximumSize = new System.Drawing.Size(21, 12);
            this.errSelectDep.Name = "errSelectDep";
            this.errSelectDep.Size = new System.Drawing.Size(21, 12);
            this.errSelectDep.TabIndex = 37;
            this.errSelectDep.Text = "*";
            this.errSelectDep.Visible = false;
            // 
            // frmAddDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(783, 471);
            this.Controls.Add(this.errSelectDep);
            this.Controls.Add(this.ErrItemName);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtSubDep);
            this.Controls.Add(this.grdsp);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDepartments);
            this.Name = "frmAddDepartment";
            this.Text = "Add Department";
            this.Load += new System.EventHandler(this.frmAddDepartment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdsp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDepartments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView grdsp;
        private System.Windows.Forms.TextBox txtSubDep;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrItemName;
        private System.Windows.Forms.Label errSelectDep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubDepartmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepartmentName;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
    }
}