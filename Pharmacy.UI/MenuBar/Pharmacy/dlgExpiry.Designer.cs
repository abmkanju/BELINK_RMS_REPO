namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class dlgExpiry
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
            this.label19 = new System.Windows.Forms.Label();
            this.btnExpApply = new System.Windows.Forms.Button();
            this.btnExpCancel = new System.Windows.Forms.Button();
            this.dtpExpiry = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(68, 13);
            this.label19.TabIndex = 24;
            this.label19.Text = "Select Expiry";
            // 
            // btnExpApply
            // 
            this.btnExpApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnExpApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnExpApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpApply.ForeColor = System.Drawing.Color.White;
            this.btnExpApply.Location = new System.Drawing.Point(74, 200);
            this.btnExpApply.Name = "btnExpApply";
            this.btnExpApply.Size = new System.Drawing.Size(78, 25);
            this.btnExpApply.TabIndex = 1;
            this.btnExpApply.Text = "Apply";
            this.btnExpApply.UseVisualStyleBackColor = false;
            this.btnExpApply.Click += new System.EventHandler(this.btnExpApply_Click);
            // 
            // btnExpCancel
            // 
            this.btnExpCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnExpCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExpCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnExpCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpCancel.ForeColor = System.Drawing.Color.White;
            this.btnExpCancel.Location = new System.Drawing.Point(153, 200);
            this.btnExpCancel.Name = "btnExpCancel";
            this.btnExpCancel.Size = new System.Drawing.Size(78, 25);
            this.btnExpCancel.TabIndex = 2;
            this.btnExpCancel.Text = "Cancel";
            this.btnExpCancel.UseVisualStyleBackColor = false;
            this.btnExpCancel.Click += new System.EventHandler(this.btnExpCancel_Click);
            // 
            // dtpExpiry
            // 
            this.dtpExpiry.Location = new System.Drawing.Point(4, 26);
            this.dtpExpiry.MaxSelectionCount = 1;
            this.dtpExpiry.Name = "dtpExpiry";
            this.dtpExpiry.TabIndex = 25;
            // 
            // dlgExpiry
            // 
            this.AcceptButton = this.btnExpApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExpCancel;
            this.ClientSize = new System.Drawing.Size(234, 230);
            this.Controls.Add(this.dtpExpiry);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.btnExpApply);
            this.Controls.Add(this.btnExpCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "dlgExpiry";
            this.Text = "dlgExpiry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnExpApply;
        private System.Windows.Forms.Button btnExpCancel;
        private System.Windows.Forms.MonthCalendar dtpExpiry;
    }
}