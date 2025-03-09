namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmNoteCalculator
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
            this.grdNotes = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // grdNotes
            // 
            this.grdNotes.AllowUserToAddRows = false;
            this.grdNotes.BackgroundColor = System.Drawing.Color.White;
            this.grdNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn18,
            this.colNote,
            this.colCount,
            this.colTotal});
            this.grdNotes.Location = new System.Drawing.Point(12, 12);
            this.grdNotes.Name = "grdNotes";
            this.grdNotes.RowHeadersVisible = false;
            this.grdNotes.Size = new System.Drawing.Size(303, 286);
            this.grdNotes.TabIndex = 8;
            this.grdNotes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdNotes_CellContentClick);
            this.grdNotes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdNotes_CellValueChanged);
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.HeaderText = "Id";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Visible = false;
            // 
            // colNote
            // 
            this.colNote.HeaderText = "Note Name";
            this.colNote.Name = "colNote";
            this.colNote.ReadOnly = true;
            // 
            // colCount
            // 
            this.colCount.HeaderText = "Count";
            this.colCount.Name = "colCount";
            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(159, 371);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(240, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(176, 304);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(139, 20);
            this.txtTotal.TabIndex = 10;
            // 
            // frmNoteCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 406);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grdNotes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(341, 445);
            this.Name = "frmNoteCalculator";
            this.Text = "Not Calculator";
            this.Load += new System.EventHandler(this.frmNoteCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtTotal;
    }
}