namespace Pharmacy.UI.Reports.Pharmacy
{
    partial class dlgPrintOptions
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
            this.cmbOrientation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveAsPdf = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbOrientation
            // 
            this.cmbOrientation.FormattingEnabled = true;
            this.cmbOrientation.Items.AddRange(new object[] {
            "Portrait",
            "Landscape"});
            this.cmbOrientation.Location = new System.Drawing.Point(47, 62);
            this.cmbOrientation.Name = "cmbOrientation";
            this.cmbOrientation.Size = new System.Drawing.Size(422, 21);
            this.cmbOrientation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Orientation";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(306, 106);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 32);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSaveAsPdf
            // 
            this.btnSaveAsPdf.Location = new System.Drawing.Point(218, 106);
            this.btnSaveAsPdf.Name = "btnSaveAsPdf";
            this.btnSaveAsPdf.Size = new System.Drawing.Size(82, 32);
            this.btnSaveAsPdf.TabIndex = 2;
            this.btnSaveAsPdf.Text = "Save As PDF";
            this.btnSaveAsPdf.UseVisualStyleBackColor = true;
            this.btnSaveAsPdf.Click += new System.EventHandler(this.btnSaveAsPdf_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(387, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dlgPrintOptions
            // 
            this.AcceptButton = this.btnPrint;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(515, 170);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveAsPdf);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbOrientation);
            this.Name = "dlgPrintOptions";
            this.Text = "Print or Save As PDF";
            this.Load += new System.EventHandler(this.dlgPrintOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbOrientation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSaveAsPdf;
        private System.Windows.Forms.Button btnCancel;
    }
}