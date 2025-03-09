
namespace Pharmacy.UI
{
    partial class frmPackages
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
            this.packagesgroup = new System.Windows.Forms.GroupBox();
            this.cmbcustomtype = new System.Windows.Forms.ComboBox();
            this.numcustomvalue = new System.Windows.Forms.NumericUpDown();
            this.radiocustom = new System.Windows.Forms.RadioButton();
            this.radio6month = new System.Windows.Forms.RadioButton();
            this.radio1year = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.chktrail = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.networkgroup = new System.Windows.Forms.GroupBox();
            this.txtipaddress = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chknetworking = new System.Windows.Forms.CheckBox();
            this.numnoofsystems = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chktermsandconditions = new System.Windows.Forms.CheckBox();
            this.packagesgroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numcustomvalue)).BeginInit();
            this.networkgroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numnoofsystems)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(37, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Upgrade Your Plan";
            // 
            // packagesgroup
            // 
            this.packagesgroup.Controls.Add(this.cmbcustomtype);
            this.packagesgroup.Controls.Add(this.numcustomvalue);
            this.packagesgroup.Controls.Add(this.radiocustom);
            this.packagesgroup.Controls.Add(this.radio6month);
            this.packagesgroup.Controls.Add(this.radio1year);
            this.packagesgroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.packagesgroup.ForeColor = System.Drawing.Color.White;
            this.packagesgroup.Location = new System.Drawing.Point(44, 134);
            this.packagesgroup.Name = "packagesgroup";
            this.packagesgroup.Size = new System.Drawing.Size(464, 178);
            this.packagesgroup.TabIndex = 1;
            this.packagesgroup.TabStop = false;
            this.packagesgroup.Text = "Choose You Plan ...... ";
            // 
            // cmbcustomtype
            // 
            this.cmbcustomtype.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.cmbcustomtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbcustomtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbcustomtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
            this.cmbcustomtype.ForeColor = System.Drawing.Color.White;
            this.cmbcustomtype.FormattingEnabled = true;
            this.cmbcustomtype.Items.AddRange(new object[] {
            "Days",
            "Months",
            "Years"});
            this.cmbcustomtype.Location = new System.Drawing.Point(252, 126);
            this.cmbcustomtype.Name = "cmbcustomtype";
            this.cmbcustomtype.Size = new System.Drawing.Size(121, 30);
            this.cmbcustomtype.TabIndex = 6;
            this.cmbcustomtype.Visible = false;
            // 
            // numcustomvalue
            // 
            this.numcustomvalue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.numcustomvalue.ForeColor = System.Drawing.Color.White;
            this.numcustomvalue.Location = new System.Drawing.Point(18, 126);
            this.numcustomvalue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numcustomvalue.Name = "numcustomvalue";
            this.numcustomvalue.Size = new System.Drawing.Size(228, 29);
            this.numcustomvalue.TabIndex = 5;
            this.numcustomvalue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numcustomvalue.Visible = false;
            // 
            // radiocustom
            // 
            this.radiocustom.AutoSize = true;
            this.radiocustom.Location = new System.Drawing.Point(18, 96);
            this.radiocustom.Name = "radiocustom";
            this.radiocustom.Size = new System.Drawing.Size(92, 28);
            this.radiocustom.TabIndex = 4;
            this.radiocustom.TabStop = true;
            this.radiocustom.Text = "Custom";
            this.radiocustom.UseVisualStyleBackColor = true;
            this.radiocustom.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radio6month
            // 
            this.radio6month.AutoSize = true;
            this.radio6month.Location = new System.Drawing.Point(19, 66);
            this.radio6month.Name = "radio6month";
            this.radio6month.Size = new System.Drawing.Size(228, 28);
            this.radio6month.TabIndex = 2;
            this.radio6month.TabStop = true;
            this.radio6month.Text = "6 Months Premium Plan";
            this.radio6month.UseVisualStyleBackColor = true;
            // 
            // radio1year
            // 
            this.radio1year.AutoSize = true;
            this.radio1year.Location = new System.Drawing.Point(19, 37);
            this.radio1year.Name = "radio1year";
            this.radio1year.Size = new System.Drawing.Size(205, 28);
            this.radio1year.TabIndex = 1;
            this.radio1year.TabStop = true;
            this.radio1year.Text = "1 Year Premium Plan";
            this.radio1year.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(703, 468);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "Hide This Pop up!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chktrail
            // 
            this.chktrail.AutoSize = true;
            this.chktrail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.chktrail.ForeColor = System.Drawing.Color.White;
            this.chktrail.Location = new System.Drawing.Point(44, 485);
            this.chktrail.Name = "chktrail";
            this.chktrail.Size = new System.Drawing.Size(310, 24);
            this.chktrail.TabIndex = 3;
            this.chktrail.Text = "Or Continue Using 15 Days Free Trial";
            this.chktrail.UseVisualStyleBackColor = true;
            this.chktrail.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(531, 468);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 39);
            this.button2.TabIndex = 4;
            this.button2.Text = "Request Package Activation";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(612, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(269, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "* Internet Connection Required";
            // 
            // networkgroup
            // 
            this.networkgroup.Controls.Add(this.txtipaddress);
            this.networkgroup.Controls.Add(this.label4);
            this.networkgroup.Controls.Add(this.label3);
            this.networkgroup.Controls.Add(this.chknetworking);
            this.networkgroup.Controls.Add(this.numnoofsystems);
            this.networkgroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.networkgroup.ForeColor = System.Drawing.Color.White;
            this.networkgroup.Location = new System.Drawing.Point(44, 318);
            this.networkgroup.Name = "networkgroup";
            this.networkgroup.Size = new System.Drawing.Size(464, 160);
            this.networkgroup.TabIndex = 7;
            this.networkgroup.TabStop = false;
            this.networkgroup.Text = "Model Type ...... ";
            // 
            // txtipaddress
            // 
            this.txtipaddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.txtipaddress.ForeColor = System.Drawing.Color.White;
            this.txtipaddress.Location = new System.Drawing.Point(196, 123);
            this.txtipaddress.Name = "txtipaddress";
            this.txtipaddress.Size = new System.Drawing.Size(262, 29);
            this.txtipaddress.TabIndex = 11;
            this.txtipaddress.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "Server IP Address:";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Number of Systems";
            this.label3.Visible = false;
            // 
            // chknetworking
            // 
            this.chknetworking.AutoSize = true;
            this.chknetworking.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.chknetworking.ForeColor = System.Drawing.Color.White;
            this.chknetworking.Location = new System.Drawing.Point(18, 36);
            this.chknetworking.Name = "chknetworking";
            this.chknetworking.Size = new System.Drawing.Size(261, 24);
            this.chknetworking.TabIndex = 8;
            this.chknetworking.Text = "Want to use Networking  Model";
            this.chknetworking.UseVisualStyleBackColor = true;
            this.chknetworking.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // numnoofsystems
            // 
            this.numnoofsystems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.numnoofsystems.ForeColor = System.Drawing.Color.White;
            this.numnoofsystems.Location = new System.Drawing.Point(196, 80);
            this.numnoofsystems.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numnoofsystems.Name = "numnoofsystems";
            this.numnoofsystems.Size = new System.Drawing.Size(262, 29);
            this.numnoofsystems.TabIndex = 5;
            this.numnoofsystems.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numnoofsystems.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(527, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(225, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Terms && Conditions:";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(527, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(364, 34);
            this.label6.TabIndex = 12;
            this.label6.Text = "1. The activation is valid for a single user and cannot be transferred or shared " +
    "with any other individual or entity.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Yellow;
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Yellow;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.Yellow;
            this.linkLabel1.Location = new System.Drawing.Point(40, 80);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(341, 23);
            this.linkLabel1.TabIndex = 15;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "* Enter The Product Activation  Key";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Yellow;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(527, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(364, 55);
            this.label7.TabIndex = 16;
            this.label7.Text = "2. The activation is granted for a specified period as indicated during the purch" +
    "ase process. Upon expiration, the user must renew the plan to continue using the" +
    " application.";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(527, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(364, 51);
            this.label8.TabIndex = 17;
            this.label8.Text = "3. The Pharmacy Management System Desktop Application Plan Activation entitles th" +
    "e user to access all features and updates available during the plan period.";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(528, 215);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(364, 49);
            this.label9.TabIndex = 18;
            this.label9.Text = "4. Any misuse or violation of the application\'s terms and conditions may result i" +
    "n immediate termination of the activation without refund.";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(529, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(364, 50);
            this.label10.TabIndex = 19;
            this.label10.Text = "5. The user agrees not to reverse engineer, modify, or distribute the application" +
    " or any part thereof without explicit written permission from the application pr" +
    "ovider.";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(529, 319);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(364, 48);
            this.label11.TabIndex = 20;
            this.label11.Text = "6. The user is responsible for maintaining the confidentiality of the activation " +
    "key and ensuring its secure usage to prevent unauthorized access.";
            // 
            // chktermsandconditions
            // 
            this.chktermsandconditions.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chktermsandconditions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.chktermsandconditions.ForeColor = System.Drawing.Color.White;
            this.chktermsandconditions.Location = new System.Drawing.Point(530, 378);
            this.chktermsandconditions.Name = "chktermsandconditions";
            this.chktermsandconditions.Size = new System.Drawing.Size(351, 80);
            this.chktermsandconditions.TabIndex = 21;
            this.chktermsandconditions.Text = "By checking this box, I confirm that I have read and agreed to the above terms an" +
    "d conditions for the Pharmacy Management System Desktop Application Plan Activat" +
    "ion.";
            this.chktermsandconditions.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chktermsandconditions.UseVisualStyleBackColor = true;
            // 
            // frmPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            this.ClientSize = new System.Drawing.Size(893, 528);
            this.Controls.Add(this.chktermsandconditions);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.networkgroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chktrail);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.packagesgroup);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(893, 528);
            this.MinimumSize = new System.Drawing.Size(893, 528);
            this.Name = "frmPackages";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upgrade Application ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPackages_FormClosing);
            this.packagesgroup.ResumeLayout(false);
            this.packagesgroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numcustomvalue)).EndInit();
            this.networkgroup.ResumeLayout(false);
            this.networkgroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numnoofsystems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox packagesgroup;
        private System.Windows.Forms.RadioButton radiocustom;
        private System.Windows.Forms.RadioButton radio6month;
        private System.Windows.Forms.RadioButton radio1year;
        private System.Windows.Forms.NumericUpDown numcustomvalue;
        private System.Windows.Forms.ComboBox cmbcustomtype;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chktrail;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox networkgroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chknetworking;
        private System.Windows.Forms.NumericUpDown numnoofsystems;
        private System.Windows.Forms.MaskedTextBox txtipaddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chktermsandconditions;
    }
}