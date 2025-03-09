namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmWard
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
            this.txtWardName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbDepartments = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.errDep = new System.Windows.Forms.Label();
            this.errName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtWardName
            // 
            this.txtWardName.Location = new System.Drawing.Point(174, 82);
            this.txtWardName.Name = "txtWardName";
            this.txtWardName.Size = new System.Drawing.Size(400, 20);
            this.txtWardName.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(412, 144);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 31);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbDepartments
            // 
            this.cmbDepartments.FormattingEnabled = true;
            this.cmbDepartments.Location = new System.Drawing.Point(174, 117);
            this.cmbDepartments.Name = "cmbDepartments";
            this.cmbDepartments.Size = new System.Drawing.Size(400, 21);
            this.cmbDepartments.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(496, 144);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 31);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Department";
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(76, 46);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 53;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // errDep
            // 
            this.errDep.AutoSize = true;
            this.errDep.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errDep.ForeColor = System.Drawing.Color.Red;
            this.errDep.Location = new System.Drawing.Point(580, 121);
            this.errDep.MaximumSize = new System.Drawing.Size(15, 15);
            this.errDep.MinimumSize = new System.Drawing.Size(15, 15);
            this.errDep.Name = "errDep";
            this.errDep.Size = new System.Drawing.Size(15, 15);
            this.errDep.TabIndex = 52;
            this.errDep.Text = "*";
            this.errDep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errDep.Visible = false;
            // 
            // errName
            // 
            this.errName.AutoSize = true;
            this.errName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errName.ForeColor = System.Drawing.Color.Red;
            this.errName.Location = new System.Drawing.Point(580, 83);
            this.errName.MaximumSize = new System.Drawing.Size(15, 15);
            this.errName.MinimumSize = new System.Drawing.Size(15, 15);
            this.errName.Name = "errName";
            this.errName.Size = new System.Drawing.Size(15, 15);
            this.errName.TabIndex = 52;
            this.errName.Text = "*";
            this.errName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.errName.Visible = false;
            // 
            // frmWard
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(668, 220);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.errName);
            this.Controls.Add(this.errDep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDepartments);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtWardName);
            this.Name = "frmWard";
            this.Text = "Ward";
            this.Load += new System.EventHandler(this.frmWard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWardName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbDepartments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label errDep;
        private System.Windows.Forms.Label errName;
    }
}