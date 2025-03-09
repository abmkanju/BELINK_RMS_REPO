
namespace Pharmacy.UI
{
    partial class frmReportIssue
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
            this.txtpharmacyname = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txtmessage = new System.Windows.Forms.TextBox();
            this.txtattachement = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtcontactnumber = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F);
            this.label1.Location = new System.Drawing.Point(132, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Issue";
            // 
            // txtpharmacyname
            // 
            this.txtpharmacyname.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtpharmacyname.Location = new System.Drawing.Point(29, 79);
            this.txtpharmacyname.Name = "txtpharmacyname";
            this.txtpharmacyname.Size = new System.Drawing.Size(400, 29);
            this.txtpharmacyname.TabIndex = 1;
            // 
            // txtemail
            // 
            this.txtemail.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtemail.Location = new System.Drawing.Point(29, 135);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(400, 29);
            this.txtemail.TabIndex = 2;
            // 
            // txtmessage
            // 
            this.txtmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtmessage.Location = new System.Drawing.Point(29, 253);
            this.txtmessage.Multiline = true;
            this.txtmessage.Name = "txtmessage";
            this.txtmessage.Size = new System.Drawing.Size(400, 260);
            this.txtmessage.TabIndex = 5;
            // 
            // txtattachement
            // 
            this.txtattachement.Enabled = false;
            this.txtattachement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtattachement.Location = new System.Drawing.Point(29, 543);
            this.txtattachement.Name = "txtattachement";
            this.txtattachement.Size = new System.Drawing.Size(400, 29);
            this.txtattachement.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label2.Location = new System.Drawing.Point(29, 520);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Screenshot/Attachment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label3.Location = new System.Drawing.Point(25, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Detail About Issue:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label4.Location = new System.Drawing.Point(29, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Contact Number*:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label5.Location = new System.Drawing.Point(29, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Email:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label6.Location = new System.Drawing.Point(29, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Pharmacy Name*:";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(301, 578);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 43);
            this.button1.TabIndex = 12;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(167, 578);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 43);
            this.button2.TabIndex = 13;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(301, 543);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 29);
            this.button3.TabIndex = 14;
            this.button3.Text = "Choose.....";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtcontactnumber
            // 
            this.txtcontactnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtcontactnumber.Location = new System.Drawing.Point(29, 196);
            this.txtcontactnumber.Mask = "0000-0000000";
            this.txtcontactnumber.Name = "txtcontactnumber";
            this.txtcontactnumber.Size = new System.Drawing.Size(400, 29);
            this.txtcontactnumber.TabIndex = 15;
            // 
            // frmReportIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 633);
            this.Controls.Add(this.txtcontactnumber);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtattachement);
            this.Controls.Add(this.txtmessage);
            this.Controls.Add(this.txtemail);
            this.Controls.Add(this.txtpharmacyname);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(457, 633);
            this.MinimumSize = new System.Drawing.Size(457, 633);
            this.Name = "frmReportIssue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Issue";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtpharmacyname;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txtmessage;
        private System.Windows.Forms.TextBox txtattachement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.MaskedTextBox txtcontactnumber;
    }
}