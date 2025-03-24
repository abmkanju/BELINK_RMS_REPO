namespace Pharmacy.UI.MenuBar.Pharmacy
{
    partial class frmAddUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUser));
            this.cmbUserType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlRights = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrUserType = new System.Windows.Forms.Label();
            this.ErrPassword = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ErrName = new System.Windows.Forms.Label();
            this.errEmail = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.pnlRoles = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.lblShift = new System.Windows.Forms.Label();
            this.cmbShifts = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlDiscPermission = new System.Windows.Forms.Panel();
            this.pnlDiscLimit = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.numDiscLimit = new System.Windows.Forms.NumericUpDown();
            this.chkCanGiveDiscount = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblNumericValues = new System.Windows.Forms.Label();
            this.txtpin = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SelectcAllheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.pnlDiscPermission.SuspendLayout();
            this.pnlDiscLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbUserType
            // 
            this.cmbUserType.FormattingEnabled = true;
            this.cmbUserType.Location = new System.Drawing.Point(1018, 7);
            this.cmbUserType.Name = "cmbUserType";
            this.cmbUserType.Size = new System.Drawing.Size(53, 22);
            this.cmbUserType.TabIndex = 0;
            this.cmbUserType.Visible = false;
            this.cmbUserType.SelectedIndexChanged += new System.EventHandler(this.cmbUserType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(956, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Role";
            this.label1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(133, 65);
            this.txtEmail.MaxLength = 64;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(231, 21);
            this.txtEmail.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(133, 102);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(231, 21);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // pnlRights
            // 
            this.pnlRights.AutoScroll = true;
            this.pnlRights.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlRights.Location = new System.Drawing.Point(133, 262);
            this.pnlRights.Name = "pnlRights";
            this.pnlRights.Size = new System.Drawing.Size(761, 175);
            this.pnlRights.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(130, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Select Permissions";
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Red;
            this.ErrMessage.Location = new System.Drawing.Point(91, 200);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrUserType
            // 
            this.ErrUserType.AutoSize = true;
            this.ErrUserType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrUserType.ForeColor = System.Drawing.Color.Red;
            this.ErrUserType.Location = new System.Drawing.Point(106, 154);
            this.ErrUserType.Name = "ErrUserType";
            this.ErrUserType.Size = new System.Drawing.Size(21, 25);
            this.ErrUserType.TabIndex = 36;
            this.ErrUserType.Text = "*";
            this.ErrUserType.Visible = false;
            // 
            // ErrPassword
            // 
            this.ErrPassword.AutoSize = true;
            this.ErrPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPassword.ForeColor = System.Drawing.Color.Red;
            this.ErrPassword.Location = new System.Drawing.Point(93, 220);
            this.ErrPassword.Name = "ErrPassword";
            this.ErrPassword.Size = new System.Drawing.Size(181, 13);
            this.ErrPassword.TabIndex = 37;
            this.ErrPassword.Text = "Password must be 8 characters long.";
            this.ErrPassword.Visible = false;
            this.ErrPassword.Click += new System.EventHandler(this.ErrPassword_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phone";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(502, 27);
            this.txtPhone.MaxLength = 10;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(231, 21);
            this.txtPhone.TabIndex = 2;
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(133, 27);
            this.txtName.MaxLength = 60;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(231, 21);
            this.txtName.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(91, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 14);
            this.label7.TabIndex = 38;
            this.label7.Text = "Name";
            // 
            // ErrName
            // 
            this.ErrName.AutoSize = true;
            this.ErrName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrName.ForeColor = System.Drawing.Color.Red;
            this.ErrName.Location = new System.Drawing.Point(370, 27);
            this.ErrName.Name = "ErrName";
            this.ErrName.Size = new System.Drawing.Size(21, 25);
            this.ErrName.TabIndex = 40;
            this.ErrName.Text = "*";
            this.ErrName.Visible = false;
            this.ErrName.Click += new System.EventHandler(this.ErrName_Click);
            // 
            // errEmail
            // 
            this.errEmail.AutoSize = true;
            this.errEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errEmail.ForeColor = System.Drawing.Color.Red;
            this.errEmail.Location = new System.Drawing.Point(370, 68);
            this.errEmail.Name = "errEmail";
            this.errEmail.Size = new System.Drawing.Size(21, 25);
            this.errEmail.TabIndex = 40;
            this.errEmail.Text = "*";
            this.errEmail.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.AliceBlue;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(913, 54);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(150, 34);
            this.btnAdd.TabIndex = 41;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.AliceBlue;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(913, 94);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 34);
            this.btnCancel.TabIndex = 42;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.FlatAppearance.BorderSize = 0;
            this.btnPreview.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnPreview.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.Location = new System.Drawing.Point(370, 100);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(35, 22);
            this.btnPreview.TabIndex = 43;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            this.btnPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPreview_MouseDown);
            this.btnPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPreview_MouseUp);
            // 
            // pnlRoles
            // 
            this.pnlRoles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRoles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlRoles.Location = new System.Drawing.Point(133, 139);
            this.pnlRoles.Name = "pnlRoles";
            this.pnlRoles.Size = new System.Drawing.Size(600, 49);
            this.pnlRoles.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "Select Roles";
            // 
            // lblShift
            // 
            this.lblShift.AutoSize = true;
            this.lblShift.Location = new System.Drawing.Point(465, 65);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(31, 14);
            this.lblShift.TabIndex = 46;
            this.lblShift.Text = "Shift";
            this.lblShift.Visible = false;
            // 
            // cmbShifts
            // 
            this.cmbShifts.FormattingEnabled = true;
            this.cmbShifts.Location = new System.Drawing.Point(502, 65);
            this.cmbShifts.Name = "cmbShifts";
            this.cmbShifts.Size = new System.Drawing.Size(231, 22);
            this.cmbShifts.TabIndex = 45;
            this.cmbShifts.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlDiscPermission);
            this.panel1.Location = new System.Drawing.Point(120, 631);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 120);
            this.panel1.TabIndex = 47;
            this.panel1.Visible = false;
            // 
            // pnlDiscPermission
            // 
            this.pnlDiscPermission.Controls.Add(this.pnlDiscLimit);
            this.pnlDiscPermission.Controls.Add(this.chkCanGiveDiscount);
            this.pnlDiscPermission.Location = new System.Drawing.Point(3, 6);
            this.pnlDiscPermission.Name = "pnlDiscPermission";
            this.pnlDiscPermission.Size = new System.Drawing.Size(352, 26);
            this.pnlDiscPermission.TabIndex = 5;
            // 
            // pnlDiscLimit
            // 
            this.pnlDiscLimit.Controls.Add(this.label9);
            this.pnlDiscLimit.Controls.Add(this.numDiscLimit);
            this.pnlDiscLimit.Location = new System.Drawing.Point(156, 1);
            this.pnlDiscLimit.Name = "pnlDiscLimit";
            this.pnlDiscLimit.Size = new System.Drawing.Size(195, 24);
            this.pnlDiscLimit.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 14);
            this.label9.TabIndex = 2;
            this.label9.Text = "Max Discount %";
            // 
            // numDiscLimit
            // 
            this.numDiscLimit.Location = new System.Drawing.Point(100, 2);
            this.numDiscLimit.Name = "numDiscLimit";
            this.numDiscLimit.Size = new System.Drawing.Size(93, 21);
            this.numDiscLimit.TabIndex = 3;
            this.numDiscLimit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chkCanGiveDiscount
            // 
            this.chkCanGiveDiscount.AutoSize = true;
            this.chkCanGiveDiscount.Checked = true;
            this.chkCanGiveDiscount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCanGiveDiscount.Location = new System.Drawing.Point(3, 5);
            this.chkCanGiveDiscount.Name = "chkCanGiveDiscount";
            this.chkCanGiveDiscount.Size = new System.Drawing.Size(120, 18);
            this.chkCanGiveDiscount.TabIndex = 0;
            this.chkCanGiveDiscount.Text = "Can Give Discount";
            this.chkCanGiveDiscount.UseVisualStyleBackColor = true;
            this.chkCanGiveDiscount.Visible = false;
            this.chkCanGiveDiscount.CheckedChanged += new System.EventHandler(this.chkCanGiveDiscount_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(117, 610);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(138, 15);
            this.label8.TabIndex = 48;
            this.label8.Text = "Special Permissions";
            this.label8.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(424, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 14);
            this.label10.TabIndex = 49;
            this.label10.Text = "PIN (4 Digits)";
            // 
            // lblNumericValues
            // 
            this.lblNumericValues.AutoSize = true;
            this.lblNumericValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumericValues.ForeColor = System.Drawing.Color.Red;
            this.lblNumericValues.Location = new System.Drawing.Point(780, 103);
            this.lblNumericValues.Name = "lblNumericValues";
            this.lblNumericValues.Size = new System.Drawing.Size(0, 13);
            this.lblNumericValues.TabIndex = 51;
            // 
            // txtpin
            // 
            this.txtpin.Location = new System.Drawing.Point(502, 100);
            this.txtpin.MaxLength = 4;
            this.txtpin.Name = "txtpin";
            this.txtpin.Size = new System.Drawing.Size(231, 21);
            this.txtpin.TabIndex = 52;
            this.txtpin.UseSystemPasswordChar = true;
            this.txtpin.TextChanged += new System.EventHandler(this.txtpin_TextChanged);
            this.txtpin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtpin_KeyPress);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(739, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 22);
            this.button1.TabIndex = 53;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            this.button1.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // SelectcAllheckBox
            // 
            this.SelectcAllheckBox.AutoSize = true;
            this.SelectcAllheckBox.Location = new System.Drawing.Point(822, 240);
            this.SelectcAllheckBox.Name = "SelectcAllheckBox";
            this.SelectcAllheckBox.Size = new System.Drawing.Size(72, 18);
            this.SelectcAllheckBox.TabIndex = 55;
            this.SelectcAllheckBox.Text = "Select All";
            this.SelectcAllheckBox.UseVisualStyleBackColor = true;
            this.SelectcAllheckBox.CheckedChanged += new System.EventHandler(this.SelectcAllheckBox_CheckedChanged);
            // 
            // frmAddUser
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1092, 749);
            this.Controls.Add(this.SelectcAllheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtpin);
            this.Controls.Add(this.lblNumericValues);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblShift);
            this.Controls.Add(this.cmbShifts);
            this.Controls.Add(this.pnlRoles);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.errEmail);
            this.Controls.Add(this.ErrName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ErrPassword);
            this.Controls.Add(this.ErrUserType);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlRights);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUserType);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmAddUser";
            this.Text = "Add User";
            this.Load += new System.EventHandler(this.frmAddUser_Load);
            this.panel1.ResumeLayout(false);
            this.pnlDiscPermission.ResumeLayout(false);
            this.pnlDiscPermission.PerformLayout();
            this.pnlDiscLimit.ResumeLayout(false);
            this.pnlDiscLimit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUserType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.FlowLayoutPanel pnlRights;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrUserType;
        private System.Windows.Forms.Label ErrPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label ErrName;
        private System.Windows.Forms.Label errEmail;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.FlowLayoutPanel pnlRoles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.ComboBox cmbShifts;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkCanGiveDiscount;
        private System.Windows.Forms.NumericUpDown numDiscLimit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlDiscLimit;
        private System.Windows.Forms.Panel pnlDiscPermission;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblNumericValues;
        private System.Windows.Forms.TextBox txtpin;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox SelectcAllheckBox;
    }
}