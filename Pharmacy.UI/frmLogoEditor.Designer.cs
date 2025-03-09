
namespace Pharmacy.UI
{
    partial class frmLogoEditor
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
            this.radImageEditor1 = new Telerik.WinControls.UI.RadImageEditor();
            ((System.ComponentModel.ISupportInitialize)(this.radImageEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // radImageEditor1
            // 
            this.radImageEditor1.AllowDrop = true;
            this.radImageEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radImageEditor1.Location = new System.Drawing.Point(0, 0);
            this.radImageEditor1.Name = "radImageEditor1";
            this.radImageEditor1.ShowItemToolTips = false;
            this.radImageEditor1.Size = new System.Drawing.Size(858, 510);
            this.radImageEditor1.TabIndex = 0;
            this.radImageEditor1.Text = "radImageEditor1";
            // 
            // frmLogoEditor
            // 
            this.ClientSize = new System.Drawing.Size(858, 510);
            this.Controls.Add(this.radImageEditor1);
            this.Name = "frmLogoEditor";
            ((System.ComponentModel.ISupportInitialize)(this.radImageEditor1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private Telerik.WinControls.UI.RadImageEditor radImageEditor1;
    }
}