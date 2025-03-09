namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmUpdateBed
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
            this.errCharges = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoomNo = new System.Windows.Forms.TextBox();
            this.cmbWard = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.errRoomNo = new System.Windows.Forms.Label();
            this.txtCharges = new System.Windows.Forms.TextBox();
            this.btnClearForm = new System.Windows.Forms.Button();
            this.btnUpdateBed = new System.Windows.Forms.Button();
            this.errWard = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errCharges
            // 
            this.errCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errCharges.AutoSize = true;
            this.errCharges.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errCharges.ForeColor = System.Drawing.Color.Red;
            this.errCharges.Location = new System.Drawing.Point(655, 155);
            this.errCharges.MaximumSize = new System.Drawing.Size(15, 15);
            this.errCharges.Name = "errCharges";
            this.errCharges.Size = new System.Drawing.Size(15, 15);
            this.errCharges.TabIndex = 136;
            this.errCharges.Text = "*";
            this.errCharges.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 135;
            this.label1.Text = "Bed/Room#";
            // 
            // txtRoomNo
            // 
            this.txtRoomNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRoomNo.Location = new System.Drawing.Point(68, 104);
            this.txtRoomNo.Name = "txtRoomNo";
            this.txtRoomNo.Size = new System.Drawing.Size(580, 20);
            this.txtRoomNo.TabIndex = 127;
            // 
            // cmbWard
            // 
            this.cmbWard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWard.FormattingEnabled = true;
            this.cmbWard.Location = new System.Drawing.Point(68, 55);
            this.cmbWard.Name = "cmbWard";
            this.cmbWard.Size = new System.Drawing.Size(577, 21);
            this.cmbWard.TabIndex = 126;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(65, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 134;
            this.label5.Text = "Charger Per Day";
            // 
            // errRoomNo
            // 
            this.errRoomNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errRoomNo.AutoSize = true;
            this.errRoomNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errRoomNo.ForeColor = System.Drawing.Color.Red;
            this.errRoomNo.Location = new System.Drawing.Point(654, 107);
            this.errRoomNo.MaximumSize = new System.Drawing.Size(15, 15);
            this.errRoomNo.Name = "errRoomNo";
            this.errRoomNo.Size = new System.Drawing.Size(15, 15);
            this.errRoomNo.TabIndex = 131;
            this.errRoomNo.Text = "*";
            this.errRoomNo.Visible = false;
            // 
            // txtCharges
            // 
            this.txtCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCharges.Location = new System.Drawing.Point(68, 151);
            this.txtCharges.Name = "txtCharges";
            this.txtCharges.Size = new System.Drawing.Size(580, 20);
            this.txtCharges.TabIndex = 128;
            this.txtCharges.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharges_KeyPress);
            // 
            // btnClearForm
            // 
            this.btnClearForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearForm.Location = new System.Drawing.Point(578, 198);
            this.btnClearForm.Name = "btnClearForm";
            this.btnClearForm.Size = new System.Drawing.Size(70, 37);
            this.btnClearForm.TabIndex = 130;
            this.btnClearForm.Text = "Clear ";
            this.btnClearForm.UseVisualStyleBackColor = true;
            this.btnClearForm.Click += new System.EventHandler(this.btnClearForm_Click);
            // 
            // btnUpdateBed
            // 
            this.btnUpdateBed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateBed.Location = new System.Drawing.Point(467, 199);
            this.btnUpdateBed.Name = "btnUpdateBed";
            this.btnUpdateBed.Size = new System.Drawing.Size(107, 36);
            this.btnUpdateBed.TabIndex = 129;
            this.btnUpdateBed.Text = "Update Bed";
            this.btnUpdateBed.UseVisualStyleBackColor = true;
            this.btnUpdateBed.Click += new System.EventHandler(this.btnUpdateBed_Click);
            // 
            // errWard
            // 
            this.errWard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errWard.AutoSize = true;
            this.errWard.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errWard.ForeColor = System.Drawing.Color.Red;
            this.errWard.Location = new System.Drawing.Point(654, 57);
            this.errWard.MaximumSize = new System.Drawing.Size(15, 15);
            this.errWard.Name = "errWard";
            this.errWard.Size = new System.Drawing.Size(15, 15);
            this.errWard.TabIndex = 133;
            this.errWard.Text = "*";
            this.errWard.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 132;
            this.label4.Text = "Select Ward";
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(65, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 137;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // frmUpdateBed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(715, 254);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.errCharges);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRoomNo);
            this.Controls.Add(this.cmbWard);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.errRoomNo);
            this.Controls.Add(this.txtCharges);
            this.Controls.Add(this.btnClearForm);
            this.Controls.Add(this.btnUpdateBed);
            this.Controls.Add(this.errWard);
            this.Controls.Add(this.label4);
            this.Name = "frmUpdateBed";
            this.Text = "Update Bed";
            this.Load += new System.EventHandler(this.frmUpdateBed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label errCharges;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoomNo;
        private System.Windows.Forms.ComboBox cmbWard;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label errRoomNo;
        private System.Windows.Forms.TextBox txtCharges;
        private System.Windows.Forms.Button btnClearForm;
        private System.Windows.Forms.Button btnUpdateBed;
        private System.Windows.Forms.Label errWard;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ErrMessage;
    }
}