
namespace Pharmacy.UI
{
    partial class frmUpdateNotification
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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.radRepeatButton1 = new Telerik.WinControls.UI.RadRepeatButton();
            this.radRepeatButton2 = new Telerik.WinControls.UI.RadRepeatButton();
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Update Application";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.White;
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.White;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.White;
            this.linkLabel1.Location = new System.Drawing.Point(28, 86);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(316, 116);
            this.linkLabel1.TabIndex = 16;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "* New Updates with Version 1.0.0 is Available, Please Click on Check for Update B" +
    "utton to Update the Application.";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // radRepeatButton1
            // 
            this.radRepeatButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radRepeatButton1.Location = new System.Drawing.Point(186, 214);
            this.radRepeatButton1.Name = "radRepeatButton1";
            this.radRepeatButton1.Size = new System.Drawing.Size(163, 34);
            this.radRepeatButton1.TabIndex = 18;
            this.radRepeatButton1.Text = "Check for Update";
            this.radRepeatButton1.Click += new System.EventHandler(this.radRepeatButton1_Click);
            // 
            // radRepeatButton2
            // 
            this.radRepeatButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radRepeatButton2.Location = new System.Drawing.Point(103, 214);
            this.radRepeatButton2.Name = "radRepeatButton2";
            this.radRepeatButton2.Size = new System.Drawing.Size(77, 34);
            this.radRepeatButton2.TabIndex = 18;
            this.radRepeatButton2.Text = "Exit";
            this.radRepeatButton2.Click += new System.EventHandler(this.radRepeatButton2_Click);
            // 
            // frmUpdateNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.ClientSize = new System.Drawing.Size(361, 260);
            this.Controls.Add(this.radRepeatButton2);
            this.Controls.Add(this.radRepeatButton1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmUpdateNotification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmUpdateNotification";
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Telerik.WinControls.UI.RadRepeatButton radRepeatButton1;
        private Telerik.WinControls.UI.RadRepeatButton radRepeatButton2;
    }
}