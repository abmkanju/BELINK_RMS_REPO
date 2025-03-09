using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmDepartments : Form
    {
        UnitOfWork unitOfWork;
        public frmDepartments()
        {
            InitializeComponent();
        }

        private void btnAddDepartment_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddDepartment(), this.MdiParent).FormClosed += ChildForm_Closed;
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            flowPanel.Controls.Clear();
            LoadDepartments();
            flowPanel_SizeChanged(null, null);
        }

        private void frmDepartments_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { this.btnRefreshForm, this.btnAddDepartment });
                LoadDepartments();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while loading form data", ex.Message, "Load Failed");
            }
        }
        private void LoadDepartments()
        {
            try
            {
                List<DepartmentVM> Deps;
                using (unitOfWork = new UnitOfWork())
                {
                    Deps = unitOfWork.DepratmentRepository.GetAllDepsWithActiveSubDeps();
                }
                foreach (DepartmentVM dp in Deps)
                {
                    BuildPanel(dp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BuildPanel(DepartmentVM s)
        {
            int ConsumptionsCount = 0;
            Panel pnlHeader = new Panel();

            // 
            // lblSupplier
            // 
            Label lblDepartment = new Label();
            lblDepartment.AutoSize = true;
            lblDepartment.Location = new System.Drawing.Point(14, 15);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new System.Drawing.Size(35, 13);
            lblDepartment.TabIndex = 1;
            lblDepartment.Text = s.Name;
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(523, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = s.CreateDate.ToString();
            // 
            // btnPrint
            // 
            //Button btnPrint = new Button();
            //btnPrint.Location = new System.Drawing.Point(1205, 0);
            //btnPrint.Name = "btnPrint";
            //btnPrint.Size = new System.Drawing.Size(51, 39);
            //btnPrint.TabIndex = 1;
            //btnPrint.Text = "Print";
            //btnPrint.UseVisualStyleBackColor = true;
            //btnPrint.Tag = s.DepartmentId;
            //btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1205, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = s.DepartmentId;
            btnDelete.Enabled = ConsumptionsCount > 0 ? false : true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1152, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = s.DepartmentId;
            //btnEdit.Enabled = ConsumptionsCount == s.StockItems.Count ? false : true;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnEdit);
            pnlHeader.Controls.Add(btnDelete);
            //pnlHeader.Controls.Add(btnPrint);
            pnlHeader.Controls.Add(lblDate);
            pnlHeader.Controls.Add(lblDepartment);
            lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdSubdeps = new DataGridView();
            GenerateDatagridView(grdSubdeps);
            grdSubdeps.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grdSubdeps.ColumnHeadersHeight = 35;
            grdSubdeps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdSubdeps.EnableHeadersVisualStyles = false;
            grdSubdeps.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdSubdeps.RowTemplate.Height = 35;
            grdSubdeps.RowHeadersVisible = false;
            grdSubdeps.BackgroundColor = Color.White;
            grdSubdeps.Dock = System.Windows.Forms.DockStyle.Fill;
            grdSubdeps.Location = new System.Drawing.Point(0, 39);
            grdSubdeps.Name = "grdStockItems";
            grdSubdeps.Size = new System.Drawing.Size(1258, 201);
            grdSubdeps.TabIndex = 1;
            grdSubdeps.AllowUserToAddRows = false;
            grdSubdeps.AllowUserToDeleteRows = false;
            grdSubdeps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (SubDepartment i in s.SubDepartments)
            {
                grdSubdeps.Rows.Add(i.Name);
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = s.DepartmentId;

            pnlDetail.Controls.Add(grdSubdeps);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1258, 240);
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            flowPanel.Refresh();
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            //DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colSubDep;
            //DataGridViewTextBoxColumn colCreatedAt;


            colSubDep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {            
            colSubDep
            });
            dataGridView1.Location = new System.Drawing.Point(15, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1258, 150);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;

            // 
            // colSubDep
            // 
            colSubDep.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colSubDep.HeaderText = "Sub Departments";
            colSubDep.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colSubDep.Name = "colSubDep";
            colSubDep.ReadOnly = true;

            //colCreatedAt.HeaderText = "Create At";
            //colCreatedAt.Name = "colCreatedAt";
            //colCreatedAt.ReadOnly = true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Record", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (rs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            long DepartmentId = (long)((Button)sender).Tag;
            unitOfWork.DepratmentRepository.SetDepartmentInActive(DepartmentId);
            foreach (Panel p in flowPanel.Controls)
            {
                if (Convert.ToInt64(p.Tag) == DepartmentId)
                {
                    flowPanel.Controls.Remove(p);
                }
            }
            btnRefreshForm.PerformClick();
            //flowPanel_SizeChanged(null, null);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            long DepartmentId = (long)((Button)sender).Tag;
            frmAddDepartment f = new frmAddDepartment(DepartmentId);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.Show();
        }

        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control c in flowPanel.Controls)
            {
                if (c is Panel)
                {
                    c.Width = flowPanel.Width - 23;
                }
            }
        }
    }
}