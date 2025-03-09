namespace Pharmacy.UI.Indoor.Menu
{
    partial class frmAddVital
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
            this.txtTemprature = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numWeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBloodPressure = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBloodSugar = new System.Windows.Forms.TextBox();
            this.numTest = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRespiratoryRate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOxygenSaturation = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.errWeight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTest)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTemprature
            // 
            this.txtTemprature.Location = new System.Drawing.Point(184, 88);
            this.txtTemprature.Name = "txtTemprature";
            this.txtTemprature.Size = new System.Drawing.Size(353, 20);
            this.txtTemprature.TabIndex = 0;
            this.txtTemprature.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTemprature_KeyPress);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(462, 356);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 32);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Weight";
            // 
            // numWeight
            // 
            this.numWeight.Location = new System.Drawing.Point(184, 45);
            this.numWeight.Name = "numWeight";
            this.numWeight.Size = new System.Drawing.Size(353, 20);
            this.numWeight.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temperature\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Blood pressure";
            // 
            // txtBloodPressure
            // 
            this.txtBloodPressure.Location = new System.Drawing.Point(184, 132);
            this.txtBloodPressure.Name = "txtBloodPressure";
            this.txtBloodPressure.Size = new System.Drawing.Size(353, 20);
            this.txtBloodPressure.TabIndex = 4;
            this.txtBloodPressure.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBloodPressure_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Blood sugar";
            // 
            // txtBloodSugar
            // 
            this.txtBloodSugar.Location = new System.Drawing.Point(184, 176);
            this.txtBloodSugar.Name = "txtBloodSugar";
            this.txtBloodSugar.Size = new System.Drawing.Size(353, 20);
            this.txtBloodSugar.TabIndex = 6;
            this.txtBloodSugar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBloodSugar_KeyPress);
            // 
            // numTest
            // 
            this.numTest.Location = new System.Drawing.Point(184, 222);
            this.numTest.Name = "numTest";
            this.numTest.Size = new System.Drawing.Size(353, 20);
            this.numTest.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Test";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Respiratory rate";
            // 
            // txtRespiratoryRate
            // 
            this.txtRespiratoryRate.Location = new System.Drawing.Point(184, 267);
            this.txtRespiratoryRate.Name = "txtRespiratoryRate";
            this.txtRespiratoryRate.Size = new System.Drawing.Size(353, 20);
            this.txtRespiratoryRate.TabIndex = 10;
            this.txtRespiratoryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRespiratoryRate_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Oxygen Saturation";
            // 
            // txtOxygenSaturation
            // 
            this.txtOxygenSaturation.Location = new System.Drawing.Point(184, 312);
            this.txtOxygenSaturation.Name = "txtOxygenSaturation";
            this.txtOxygenSaturation.Size = new System.Drawing.Size(353, 20);
            this.txtOxygenSaturation.TabIndex = 12;
            this.txtOxygenSaturation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOxygenSaturation_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(381, 356);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 32);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(48, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // errWeight
            // 
            this.errWeight.AutoSize = true;
            this.errWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errWeight.ForeColor = System.Drawing.Color.Red;
            this.errWeight.Location = new System.Drawing.Point(540, 45);
            this.errWeight.Name = "errWeight";
            this.errWeight.Size = new System.Drawing.Size(21, 25);
            this.errWeight.TabIndex = 36;
            this.errWeight.Text = "*";
            this.errWeight.Visible = false;
            // 
            // frmAddVital
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 418);
            this.Controls.Add(this.errWeight);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtOxygenSaturation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRespiratoryRate);
            this.Controls.Add(this.numTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBloodSugar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBloodPressure);
            this.Controls.Add(this.numWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtTemprature);
            this.Name = "frmAddVital";
            this.Text = "Add Vital";
            this.Load += new System.EventHandler(this.frmAddVital_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTemprature;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBloodPressure;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBloodSugar;
        private System.Windows.Forms.NumericUpDown numTest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRespiratoryRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtOxygenSaturation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label errWeight;
    }
}