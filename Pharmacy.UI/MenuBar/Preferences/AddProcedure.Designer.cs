namespace Pharmacy.UI.MenuBar.Preferences
{
    partial class AddProcedure
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
            this.txtProcName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.numProcPrice = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrProcName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numProcPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Procedure Name";
            // 
            // txtProcName
            // 
            this.txtProcName.Location = new System.Drawing.Point(137, 61);
            this.txtProcName.Name = "txtProcName";
            this.txtProcName.Size = new System.Drawing.Size(369, 20);
            this.txtProcName.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(414, 150);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 39);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // numProcPrice
            // 
            this.numProcPrice.Location = new System.Drawing.Point(137, 100);
            this.numProcPrice.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.numProcPrice.Name = "numProcPrice";
            this.numProcPrice.Size = new System.Drawing.Size(369, 20);
            this.numProcPrice.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Procedure Price";
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(30, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(282, 15);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrProcName
            // 
            this.ErrProcName.AutoSize = true;
            this.ErrProcName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrProcName.ForeColor = System.Drawing.Color.Red;
            this.ErrProcName.Location = new System.Drawing.Point(509, 61);
            this.ErrProcName.Name = "ErrProcName";
            this.ErrProcName.Size = new System.Drawing.Size(21, 25);
            this.ErrProcName.TabIndex = 36;
            this.ErrProcName.Text = "*";
            this.ErrProcName.Visible = false;
            // 
            // AddProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(563, 222);
            this.Controls.Add(this.ErrProcName);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.numProcPrice);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtProcName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddProcedure";
            this.Text = "Add Procedure";
            this.Load += new System.EventHandler(this.AddProcedure_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numProcPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProcName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown numProcPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrProcName;
    }
}