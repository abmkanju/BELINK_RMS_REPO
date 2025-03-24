
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
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(37, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Your Plan";
            // 
            // packagesgroup
            // 
            this.packagesgroup.Controls.Add(this.cmbcustomtype);
            this.packagesgroup.Controls.Add(this.numcustomvalue);
            this.packagesgroup.Controls.Add(this.radiocustom);
            this.packagesgroup.Controls.Add(this.radio6month);
            this.packagesgroup.Controls.Add(this.radio1year);
            this.packagesgroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.packagesgroup.ForeColor = System.Drawing.Color.Black;
            this.packagesgroup.Location = new System.Drawing.Point(44, 134);
            this.packagesgroup.Name = "packagesgroup";
            this.packagesgroup.Size = new System.Drawing.Size(464, 133);
            this.packagesgroup.TabIndex = 1;
            this.packagesgroup.TabStop = false;
            this.packagesgroup.Text = "Choose You Plan ...... ";
            // 
            // cmbcustomtype
            // 
            this.cmbcustomtype.BackColor = System.Drawing.Color.AliceBlue;
            this.cmbcustomtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbcustomtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbcustomtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
            this.cmbcustomtype.ForeColor = System.Drawing.Color.Black;
            this.cmbcustomtype.FormattingEnabled = true;
            this.cmbcustomtype.Items.AddRange(new object[] {
            "Days",
            "Months",
            "Years"});
            this.cmbcustomtype.Location = new System.Drawing.Point(250, 83);
            this.cmbcustomtype.Name = "cmbcustomtype";
            this.cmbcustomtype.Size = new System.Drawing.Size(208, 30);
            this.cmbcustomtype.TabIndex = 6;
            this.cmbcustomtype.Visible = false;
            // 
            // numcustomvalue
            // 
            this.numcustomvalue.BackColor = System.Drawing.Color.AliceBlue;
            this.numcustomvalue.ForeColor = System.Drawing.Color.Black;
            this.numcustomvalue.Location = new System.Drawing.Point(116, 82);
            this.numcustomvalue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numcustomvalue.Name = "numcustomvalue";
            this.numcustomvalue.Size = new System.Drawing.Size(128, 29);
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
            this.radiocustom.ForeColor = System.Drawing.Color.Black;
            this.radiocustom.Location = new System.Drawing.Point(18, 83);
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
            this.radio6month.ForeColor = System.Drawing.Color.Black;
            this.radio6month.Location = new System.Drawing.Point(230, 37);
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
            this.radio1year.ForeColor = System.Drawing.Color.Black;
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
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(703, 468);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chktrail
            // 
            this.chktrail.AutoSize = true;
            this.chktrail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.chktrail.ForeColor = System.Drawing.Color.Black;
            this.chktrail.Location = new System.Drawing.Point(44, 452);
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
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(703, 423);
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
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 495);
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
            this.networkgroup.ForeColor = System.Drawing.Color.Black;
            this.networkgroup.Location = new System.Drawing.Point(44, 273);
            this.networkgroup.Name = "networkgroup";
            this.networkgroup.Size = new System.Drawing.Size(464, 160);
            this.networkgroup.TabIndex = 7;
            this.networkgroup.TabStop = false;
            this.networkgroup.Text = "Model Type ...... ";
            // 
            // txtipaddress
            // 
            this.txtipaddress.BackColor = System.Drawing.Color.AliceBlue;
            this.txtipaddress.ForeColor = System.Drawing.Color.Black;
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
            this.label4.ForeColor = System.Drawing.Color.Black;
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
            this.label3.ForeColor = System.Drawing.Color.Black;
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
            this.chknetworking.ForeColor = System.Drawing.Color.Black;
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
            this.numnoofsystems.BackColor = System.Drawing.Color.AliceBlue;
            this.numnoofsystems.ForeColor = System.Drawing.Color.Black;
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
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(527, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(225, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Terms && Conditions:";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label6.ForeColor = System.Drawing.Color.Black;
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
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Red;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.linkLabel1.ForeColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.Red;
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
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(527, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(364, 55);
            this.label7.TabIndex = 16;
            this.label7.Text = "2. The activation is granted for a specified period as indicated during the purch" +
    "ase process. Upon expiration, the user must renew the plan to continue using the" +
    " application.";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(528, 164);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(364, 49);
            this.label9.TabIndex = 18;
            this.label9.Text = "3. Any misuse or violation of the application\'s terms and conditions may result i" +
    "n immediate termination of the activation without refund.";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(529, 217);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(364, 50);
            this.label10.TabIndex = 19;
            this.label10.Text = "4. The user agrees not to reverse engineer, modify, or distribute the application" +
    " or any part thereof without explicit written permission from the application pr" +
    "ovider.";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(529, 268);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(364, 48);
            this.label11.TabIndex = 20;
            this.label11.Text = "5. The user is responsible for maintaining the confidentiality of the activation " +
    "key and ensuring its secure usage to prevent unauthorized access.";
            // 
            // chktermsandconditions
            // 
            this.chktermsandconditions.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chktermsandconditions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.chktermsandconditions.ForeColor = System.Drawing.Color.Black;
            this.chktermsandconditions.Location = new System.Drawing.Point(532, 342);
            this.chktermsandconditions.Name = "chktermsandconditions";
            this.chktermsandconditions.Size = new System.Drawing.Size(351, 80);
            this.chktermsandconditions.TabIndex = 21;
            this.chktermsandconditions.Text = "By checking this box, I confirm that I have read and agreed to the above terms an" +
    "d conditions for the RMS Management System Desktop Application Plan Activation.";
            this.chktermsandconditions.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chktermsandconditions.UseVisualStyleBackColor = true;
            // 
            // frmPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(893, 528);
            this.Controls.Add(this.chktermsandconditions);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chktermsandconditions;
    }
}