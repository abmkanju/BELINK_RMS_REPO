namespace Pharmacy.UI
{
    partial class frmWaiting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaiting));
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bgDataLoader = new System.ComponentModel.BackgroundWorker();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pbCompleted = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.pnlDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCompleted)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLoading
            // 
            this.pbLoading.Image = ((System.Drawing.Image)(resources.GetObject("pbLoading.Image")));
            this.pbLoading.Location = new System.Drawing.Point(412, 5);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(86, 75);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLoading.TabIndex = 0;
            this.pbLoading.TabStop = false;
            this.pbLoading.WaitOnLoad = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(8, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(304, 151);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "We are Setting up Things for First Use, Please Wait";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlDisplay.Controls.Add(this.linkLabel1);
            this.pnlDisplay.Controls.Add(this.label2);
            this.pnlDisplay.Controls.Add(this.label1);
            this.pnlDisplay.Controls.Add(this.progressBar1);
            this.pnlDisplay.Controls.Add(this.pbCompleted);
            this.pnlDisplay.Controls.Add(this.pbLoading);
            this.pnlDisplay.Controls.Add(this.lblStatus);
            this.pnlDisplay.Location = new System.Drawing.Point(4, 4);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Size = new System.Drawing.Size(506, 254);
            this.pnlDisplay.TabIndex = 2;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(196, 185);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(116, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Let\'s have Some Fun ..";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(429, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "00000/29532";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Starting Data Manipulation ... ";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.Black;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.ForeColor = System.Drawing.Color.Teal;
            this.progressBar1.Location = new System.Drawing.Point(0, 242);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(506, 12);
            this.progressBar1.TabIndex = 3;
            // 
            // pbCompleted
            // 
            this.pbCompleted.Image = ((System.Drawing.Image)(resources.GetObject("pbCompleted.Image")));
            this.pbCompleted.Location = new System.Drawing.Point(412, 8);
            this.pbCompleted.Name = "pbCompleted";
            this.pbCompleted.Size = new System.Drawing.Size(86, 72);
            this.pbCompleted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCompleted.TabIndex = 2;
            this.pbCompleted.TabStop = false;
            this.pbCompleted.Visible = false;
            // 
            // frmWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(514, 262);
            this.Controls.Add(this.pnlDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWaiting";
            this.Text = "frmWaiting";
            this.Load += new System.EventHandler(this.frmWaiting_Load);
            this.Shown += new System.EventHandler(this.frmWaiting_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.pnlDisplay.ResumeLayout(false);
            this.pnlDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCompleted)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgDataLoader;
        private System.Windows.Forms.Panel pnlDisplay;
        private System.Windows.Forms.PictureBox pbCompleted;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}