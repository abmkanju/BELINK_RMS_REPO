namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmAssignBed
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
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReasonForAdm = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.numAdmNo = new System.Windows.Forms.NumericUpDown();
            this.cmbWard = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBedNo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConsultant = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAnesthetist = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpOpDate = new System.Windows.Forms.DateTimePicker();
            this.btnAddPatient = new System.Windows.Forms.Button();
            this.lbDoctors = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.errPatient = new System.Windows.Forms.Label();
            this.errWard = new System.Windows.Forms.Label();
            this.errBedNo = new System.Windows.Forms.Label();
            this.errAdmNo = new System.Windows.Forms.Label();
            this.errBeds = new System.Windows.Forms.Label();
            this.errPatientAdmitted = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numAdmNo)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPatient
            // 
            this.cmbPatient.FormattingEnabled = true;
            this.cmbPatient.Location = new System.Drawing.Point(166, 55);
            this.cmbPatient.Name = "cmbPatient";
            this.cmbPatient.Size = new System.Drawing.Size(277, 21);
            this.cmbPatient.TabIndex = 0;
            this.cmbPatient.SelectedIndexChanged += new System.EventHandler(this.cmbPatient_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Patient";
            // 
            // txtReasonForAdm
            // 
            this.txtReasonForAdm.Location = new System.Drawing.Point(166, 164);
            this.txtReasonForAdm.Name = "txtReasonForAdm";
            this.txtReasonForAdm.Size = new System.Drawing.Size(277, 20);
            this.txtReasonForAdm.TabIndex = 2;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(368, 505);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 31);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // numAdmNo
            // 
            this.numAdmNo.Location = new System.Drawing.Point(166, 200);
            this.numAdmNo.Name = "numAdmNo";
            this.numAdmNo.Size = new System.Drawing.Size(277, 20);
            this.numAdmNo.TabIndex = 4;
            // 
            // cmbWard
            // 
            this.cmbWard.FormattingEnabled = true;
            this.cmbWard.Location = new System.Drawing.Point(166, 91);
            this.cmbWard.Name = "cmbWard";
            this.cmbWard.Size = new System.Drawing.Size(277, 21);
            this.cmbWard.TabIndex = 0;
            this.cmbWard.SelectedIndexChanged += new System.EventHandler(this.cmbWard_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ward";
            // 
            // cmbBedNo
            // 
            this.cmbBedNo.FormattingEnabled = true;
            this.cmbBedNo.Location = new System.Drawing.Point(166, 127);
            this.cmbBedNo.Name = "cmbBedNo";
            this.cmbBedNo.Size = new System.Drawing.Size(277, 21);
            this.cmbBedNo.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Bed#";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(58, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 31);
            this.label4.TabIndex = 1;
            this.label4.Text = "Reason For Admission";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Admission No";
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.Location = new System.Drawing.Point(166, 236);
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Size = new System.Drawing.Size(277, 20);
            this.txtDiagnosis.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Diagnosis";
            // 
            // txtConsultant
            // 
            this.txtConsultant.Location = new System.Drawing.Point(166, 273);
            this.txtConsultant.Name = "txtConsultant";
            this.txtConsultant.Size = new System.Drawing.Size(277, 20);
            this.txtConsultant.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 280);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Consultant";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(58, 318);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Doctor";
            // 
            // txtAnesthetist
            // 
            this.txtAnesthetist.Location = new System.Drawing.Point(166, 438);
            this.txtAnesthetist.Name = "txtAnesthetist";
            this.txtAnesthetist.Size = new System.Drawing.Size(277, 20);
            this.txtAnesthetist.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(58, 445);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Anesthetist";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(58, 481);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Operation Date";
            // 
            // dtpOpDate
            // 
            this.dtpOpDate.Location = new System.Drawing.Point(166, 475);
            this.dtpOpDate.Name = "dtpOpDate";
            this.dtpOpDate.Size = new System.Drawing.Size(277, 20);
            this.dtpOpDate.TabIndex = 13;
            // 
            // btnAddPatient
            // 
            this.btnAddPatient.Location = new System.Drawing.Point(449, 53);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(75, 23);
            this.btnAddPatient.TabIndex = 3;
            this.btnAddPatient.Text = "Add Patient";
            this.btnAddPatient.UseVisualStyleBackColor = true;
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
            // 
            // lbDoctors
            // 
            this.lbDoctors.FormattingEnabled = true;
            this.lbDoctors.Location = new System.Drawing.Point(166, 309);
            this.lbDoctors.Name = "lbDoctors";
            this.lbDoctors.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbDoctors.Size = new System.Drawing.Size(277, 121);
            this.lbDoctors.TabIndex = 14;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(287, 505);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 31);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(58, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 138;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // errPatient
            // 
            this.errPatient.AutoSize = true;
            this.errPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errPatient.ForeColor = System.Drawing.Color.Red;
            this.errPatient.Location = new System.Drawing.Point(530, 56);
            this.errPatient.MaximumSize = new System.Drawing.Size(15, 15);
            this.errPatient.Name = "errPatient";
            this.errPatient.Size = new System.Drawing.Size(15, 15);
            this.errPatient.TabIndex = 139;
            this.errPatient.Text = "*";
            this.errPatient.Visible = false;
            // 
            // errWard
            // 
            this.errWard.AutoSize = true;
            this.errWard.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errWard.ForeColor = System.Drawing.Color.Red;
            this.errWard.Location = new System.Drawing.Point(449, 94);
            this.errWard.MaximumSize = new System.Drawing.Size(15, 15);
            this.errWard.Name = "errWard";
            this.errWard.Size = new System.Drawing.Size(15, 15);
            this.errWard.TabIndex = 139;
            this.errWard.Text = "*";
            this.errWard.Visible = false;
            // 
            // errBedNo
            // 
            this.errBedNo.AutoSize = true;
            this.errBedNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errBedNo.ForeColor = System.Drawing.Color.Red;
            this.errBedNo.Location = new System.Drawing.Point(449, 130);
            this.errBedNo.MaximumSize = new System.Drawing.Size(15, 15);
            this.errBedNo.Name = "errBedNo";
            this.errBedNo.Size = new System.Drawing.Size(15, 15);
            this.errBedNo.TabIndex = 139;
            this.errBedNo.Text = "*";
            this.errBedNo.Visible = false;
            // 
            // errAdmNo
            // 
            this.errAdmNo.AutoSize = true;
            this.errAdmNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errAdmNo.ForeColor = System.Drawing.Color.Red;
            this.errAdmNo.Location = new System.Drawing.Point(449, 202);
            this.errAdmNo.MaximumSize = new System.Drawing.Size(15, 15);
            this.errAdmNo.Name = "errAdmNo";
            this.errAdmNo.Size = new System.Drawing.Size(15, 15);
            this.errAdmNo.TabIndex = 139;
            this.errAdmNo.Text = "*";
            this.errAdmNo.Visible = false;
            // 
            // errBeds
            // 
            this.errBeds.AutoSize = true;
            this.errBeds.ForeColor = System.Drawing.Color.Red;
            this.errBeds.Location = new System.Drawing.Point(167, 129);
            this.errBeds.Name = "errBeds";
            this.errBeds.Size = new System.Drawing.Size(201, 13);
            this.errBeds.TabIndex = 140;
            this.errBeds.Text = "Please add bed details for selected ward.";
            this.errBeds.Visible = false;
            // 
            // errPatientAdmitted
            // 
            this.errPatientAdmitted.AutoSize = true;
            this.errPatientAdmitted.ForeColor = System.Drawing.Color.Red;
            this.errPatientAdmitted.Location = new System.Drawing.Point(163, 39);
            this.errPatientAdmitted.Name = "errPatientAdmitted";
            this.errPatientAdmitted.Size = new System.Drawing.Size(155, 13);
            this.errPatientAdmitted.TabIndex = 141;
            this.errPatientAdmitted.Text = "This patient is already admitted.";
            this.errPatientAdmitted.Visible = false;
            // 
            // frmAssignBed
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(613, 548);
            this.Controls.Add(this.errPatientAdmitted);
            this.Controls.Add(this.errBeds);
            this.Controls.Add(this.errAdmNo);
            this.Controls.Add(this.errBedNo);
            this.Controls.Add(this.errWard);
            this.Controls.Add(this.errPatient);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.lbDoctors);
            this.Controls.Add(this.dtpOpDate);
            this.Controls.Add(this.txtAnesthetist);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtConsultant);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDiagnosis);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numAdmNo);
            this.Controls.Add(this.btnAddPatient);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtReasonForAdm);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBedNo);
            this.Controls.Add(this.cmbWard);
            this.Controls.Add(this.cmbPatient);
            this.Name = "frmAssignBed";
            this.Text = "Assign Bed";
            this.Load += new System.EventHandler(this.frmAssignBed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numAdmNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPatient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReasonForAdm;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown numAdmNo;
        private System.Windows.Forms.ComboBox cmbWard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBedNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConsultant;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAnesthetist;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpOpDate;
        private System.Windows.Forms.Button btnAddPatient;
        private System.Windows.Forms.ListBox lbDoctors;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label errPatient;
        private System.Windows.Forms.Label errWard;
        private System.Windows.Forms.Label errBedNo;
        private System.Windows.Forms.Label errAdmNo;
        private System.Windows.Forms.Label errBeds;
        private System.Windows.Forms.Label errPatientAdmitted;
    }
}