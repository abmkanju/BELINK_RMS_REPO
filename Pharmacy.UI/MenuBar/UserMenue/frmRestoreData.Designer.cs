namespace Pharmacy.UI.MenuBar.UserMenue
{
    partial class frmRestoreData
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
            this.txtcurrentdbname = new System.Windows.Forms.TextBox();
            this.lblRestoreProgress = new System.Windows.Forms.Label();
            this.lblRestorePercent = new System.Windows.Forms.Label();
            this.RestoreProgressBar = new System.Windows.Forms.ProgressBar();
            this.button4 = new System.Windows.Forms.Button();
            this.txtRestoreDate = new System.Windows.Forms.TextBox();
            this.label124 = new System.Windows.Forms.Label();
            this.label125 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.txtbackupfileaddress = new System.Windows.Forms.TextBox();
            this.label127 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtcurrentdbname
            // 
            this.txtcurrentdbname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtcurrentdbname.Location = new System.Drawing.Point(206, 81);
            this.txtcurrentdbname.Name = "txtcurrentdbname";
            this.txtcurrentdbname.ReadOnly = true;
            this.txtcurrentdbname.Size = new System.Drawing.Size(442, 26);
            this.txtcurrentdbname.TabIndex = 51;
            // 
            // lblRestoreProgress
            // 
            this.lblRestoreProgress.AutoSize = true;
            this.lblRestoreProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.lblRestoreProgress.ForeColor = System.Drawing.Color.Red;
            this.lblRestoreProgress.Location = new System.Drawing.Point(14, 227);
            this.lblRestoreProgress.Name = "lblRestoreProgress";
            this.lblRestoreProgress.Size = new System.Drawing.Size(0, 20);
            this.lblRestoreProgress.TabIndex = 50;
            // 
            // lblRestorePercent
            // 
            this.lblRestorePercent.AutoSize = true;
            this.lblRestorePercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.lblRestorePercent.Location = new System.Drawing.Point(9, 178);
            this.lblRestorePercent.Name = "lblRestorePercent";
            this.lblRestorePercent.Size = new System.Drawing.Size(182, 20);
            this.lblRestorePercent.TabIndex = 49;
            this.lblRestorePercent.Text = "Restore Progress (0%)";
            // 
            // RestoreProgressBar
            // 
            this.RestoreProgressBar.Location = new System.Drawing.Point(12, 201);
            this.RestoreProgressBar.Name = "RestoreProgressBar";
            this.RestoreProgressBar.Size = new System.Drawing.Size(637, 23);
            this.RestoreProgressBar.TabIndex = 48;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(547, 147);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 25);
            this.button4.TabIndex = 47;
            this.button4.Text = "Start Restore";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtRestoreDate
            // 
            this.txtRestoreDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtRestoreDate.Location = new System.Drawing.Point(206, 115);
            this.txtRestoreDate.Name = "txtRestoreDate";
            this.txtRestoreDate.ReadOnly = true;
            this.txtRestoreDate.Size = new System.Drawing.Size(442, 26);
            this.txtRestoreDate.TabIndex = 46;
            // 
            // label124
            // 
            this.label124.AutoSize = true;
            this.label124.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label124.Location = new System.Drawing.Point(9, 120);
            this.label124.Name = "label124";
            this.label124.Size = new System.Drawing.Size(114, 20);
            this.label124.TabIndex = 45;
            this.label124.Text = "Restore Date:";
            // 
            // label125
            // 
            this.label125.AutoSize = true;
            this.label125.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label125.Location = new System.Drawing.Point(8, 86);
            this.label125.Name = "label125";
            this.label125.Size = new System.Drawing.Size(152, 20);
            this.label125.TabIndex = 44;
            this.label125.Text = "Current Database :";
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(508, 48);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(138, 25);
            this.button5.TabIndex = 42;
            this.button5.Text = "Choose";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // txtbackupfileaddress
            // 
            this.txtbackupfileaddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txtbackupfileaddress.Location = new System.Drawing.Point(11, 48);
            this.txtbackupfileaddress.Name = "txtbackupfileaddress";
            this.txtbackupfileaddress.ReadOnly = true;
            this.txtbackupfileaddress.Size = new System.Drawing.Size(501, 26);
            this.txtbackupfileaddress.TabIndex = 43;
            // 
            // label127
            // 
            this.label127.AutoSize = true;
            this.label127.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label127.Location = new System.Drawing.Point(9, 24);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(204, 20);
            this.label127.TabIndex = 41;
            this.label127.Text = "Choose BackUp File(.bak)";
            // 
            // frmRestoreData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 249);
            this.Controls.Add(this.txtcurrentdbname);
            this.Controls.Add(this.lblRestoreProgress);
            this.Controls.Add(this.lblRestorePercent);
            this.Controls.Add(this.RestoreProgressBar);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtRestoreDate);
            this.Controls.Add(this.label124);
            this.Controls.Add(this.label125);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txtbackupfileaddress);
            this.Controls.Add(this.label127);
            this.Name = "frmRestoreData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restore Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtcurrentdbname;
        private System.Windows.Forms.Label lblRestoreProgress;
        private System.Windows.Forms.Label lblRestorePercent;
        private System.Windows.Forms.ProgressBar RestoreProgressBar;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtRestoreDate;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.Label label125;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtbackupfileaddress;
        private System.Windows.Forms.Label label127;
    }
}