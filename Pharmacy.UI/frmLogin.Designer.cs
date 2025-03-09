
namespace Pharmacy.UI
{
    partial class frmLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBox2 = new Telerik.WinControls.UI.RadTextBox();
            this.chkRememberMe = new Telerik.WinControls.UI.RadCheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRememberMe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 41.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 62);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login";
            // 
            // radTextBox1
            // 
            this.radTextBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.radTextBox1.Location = new System.Drawing.Point(23, 118);
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.NullText = "Enter Username/Email";
            this.radTextBox1.Size = new System.Drawing.Size(370, 31);
            this.radTextBox1.TabIndex = 1;
            // 
            // radTextBox2
            // 
            this.radTextBox2.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.radTextBox2.Location = new System.Drawing.Point(23, 174);
            this.radTextBox2.Name = "radTextBox2";
            this.radTextBox2.NullText = "Enter Password/PIN";
            this.radTextBox2.PasswordChar = '●';
            this.radTextBox2.Size = new System.Drawing.Size(370, 31);
            this.radTextBox2.TabIndex = 2;
            this.radTextBox2.UseSystemPasswordChar = true;
            // 
            // chkRememberMe
            // 
            this.chkRememberMe.Font = new System.Drawing.Font("Segoe UI", 12.25F);
            this.chkRememberMe.Location = new System.Drawing.Point(23, 217);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(275, 26);
            this.chkRememberMe.TabIndex = 3;
            this.chkRememberMe.Text = "Remember Username && Password";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pharmacy.UI.Properties.Resources.Removal_9541;
            this.pictureBox1.Location = new System.Drawing.Point(399, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(303, 187);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // radButton1
            // 
            this.radButton1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(399, 218);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(169, 67);
            this.radButton1.TabIndex = 5;
            this.radButton1.Text = "Login";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton2.Location = new System.Drawing.Point(574, 218);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(128, 67);
            this.radButton2.TabIndex = 6;
            this.radButton2.Text = "Exit";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(723, 297);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.chkRememberMe);
            this.Controls.Add(this.radTextBox2);
            this.Controls.Add(this.radTextBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRememberMe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
        private Telerik.WinControls.UI.RadTextBox radTextBox2;
        private Telerik.WinControls.UI.RadCheckBox chkRememberMe;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
    }
}