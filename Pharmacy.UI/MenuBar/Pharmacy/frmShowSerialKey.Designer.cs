namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmShowSerialKey
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
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.btnGenerateKey = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.bntCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Location = new System.Drawing.Point(54, 44);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(201, 20);
            this.txtSerialNo.TabIndex = 0;
            // 
            // btnGenerateKey
            // 
            this.btnGenerateKey.Location = new System.Drawing.Point(180, 165);
            this.btnGenerateKey.Name = "btnGenerateKey";
            this.btnGenerateKey.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateKey.TabIndex = 2;
            this.btnGenerateKey.Text = "Generate Key";
            this.btnGenerateKey.UseVisualStyleBackColor = true;
            this.btnGenerateKey.Click += new System.EventHandler(this.btnGenerateKey_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(54, 139);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(201, 20);
            this.txtKey.TabIndex = 3;
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(180, 79);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(75, 23);
            this.btnGet.TabIndex = 1;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // bntCheck
            // 
            this.bntCheck.Location = new System.Drawing.Point(180, 213);
            this.bntCheck.Name = "bntCheck";
            this.bntCheck.Size = new System.Drawing.Size(75, 23);
            this.bntCheck.TabIndex = 4;
            this.bntCheck.Text = "Check Key";
            this.bntCheck.UseVisualStyleBackColor = true;
            this.bntCheck.Click += new System.EventHandler(this.bntCheck_Click);
            // 
            // frmShowSerialKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 248);
            this.Controls.Add(this.bntCheck);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnGenerateKey);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.txtSerialNo);
            this.Name = "frmShowSerialKey";
            this.Text = "frmShowSerialKey";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Button btnGenerateKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button bntCheck;
    }
}