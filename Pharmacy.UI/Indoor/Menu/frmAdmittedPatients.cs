using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.Reports;
using Pharmacy.Core.ViewModels;
using System.IO;
using Pharmacy.UI.MenuBar.Pharmacy;


namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmAdmittedPatients : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<IGrouping<Ward, AdmittedPatient>> AdmittedPatients;
        public frmAdmittedPatients()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefreshForm, btnAssignBed });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnNext, btnLastPage, btnFirstPage, btnPrevious });
            LoadAdmittedPatients();
        }

        private void LoadAdmittedPatients()
        {
            flowPanel.Controls.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                AdmittedPatients = unitOfWork.AdmitPatientsRepository.GetAdmittedPatients(pageNo, SharedVariables.PageSize);
                foreach (var res in AdmittedPatients)
                {
                    BuildPanel(res.Key, res);
                }
            }
            //btnLastPage.Enabled = btnNext.Enabled = StocksList.HasNextPage ? true : false;
            //btnFirstPage.Enabled = btnPrevious.Enabled = StocksList.HasPreviousPage ? true : false;
            //SharedFunctions.ShowPageNo(lblPageNo, pageNo, StocksList.PageCount);
        }

        private void BuildPanel(Ward Key, IGrouping<Ward, AdmittedPatient> Group)
        {
            Panel pnlHeader = new Panel();

            // 
            // lblWard Information
            // 
            Label lblWardInfo = new Label();
            lblWardInfo.Size = new System.Drawing.Size(35, 13);
            lblWardInfo.AutoSize = true;
            lblWardInfo.Location = new System.Drawing.Point(14, 8);
            lblWardInfo.Name = "lblWardInfo";
            lblWardInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            lblWardInfo.TabIndex = 1;
            lblWardInfo.Text = Key.Name;

            // 
            // lblWardDetails
            // 
            Label lblWardDetails = new Label();
            lblWardDetails.AutoSize = true;
            lblWardDetails.Location = new System.Drawing.Point(14, 42);
            lblWardDetails.Name = "lblWardDetails";
            lblWardDetails.Size = new System.Drawing.Size(35, 13);
            lblWardDetails.TabIndex = 1;
            lblWardDetails.Text = "(Total Beds :" + Key.Beds.Count + " | Available Beds :" + Key.Beds.Where(b => b.IsAvailable).Count() + ")";

            // 
            // btnPrint
            // 
            Button btnToggleView = new Button();
            btnToggleView.Location = new System.Drawing.Point(1205, 0);
            btnToggleView.Name = "btnPrint";
            btnToggleView.Size = new System.Drawing.Size(51, 39);
            btnToggleView.TabIndex = 1;
            btnToggleView.Text = "Print";
            btnToggleView.UseVisualStyleBackColor = true;
            btnToggleView.Tag = new Tuple<long, string>(Key.WardId, "IsDetailView");
            btnToggleView.FlatStyle = FlatStyle.Flat;
            btnToggleView.FlatAppearance.BorderSize = 0;
            btnToggleView.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Theme", "gridview36.png"));
            btnToggleView.Text = "";
            btnToggleView.ImageAlign = ContentAlignment.MiddleCenter;
            btnToggleView.Click += new EventHandler(btnToggleView_Click);
            // 
            // btnDelete
            // 
            //Button btnDelete = new Button();
            //btnDelete.Location = new System.Drawing.Point(1152, 0);
            //btnDelete.Name = "btnDelete";
            //btnDelete.Size = new System.Drawing.Size(51, 39);
            //btnDelete.TabIndex = 1;
            //btnDelete.Text = "Delete";
            //btnDelete.UseVisualStyleBackColor = true;
            //btnDelete.Tag = s.StockId;
            //btnDelete.Enabled = ConsumptionsCount > 0 ? false : true;
            //btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // btnEdit
            //
            //Button btnEdit = new Button();
            //btnEdit.Location = new System.Drawing.Point(1099, 0);
            //btnEdit.Name = "btnEdit";
            //btnEdit.Size = new System.Drawing.Size(51, 39);
            //btnEdit.TabIndex = 1;
            //btnEdit.Text = "Edit";
            //btnEdit.UseVisualStyleBackColor = true;
            //btnEdit.Tag = s.StockId;
            //btnEdit.Enabled = ConsumptionsCount == s.StockItems.Count ? false : true;
            //btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 70);
            pnlHeader.TabIndex = 0;
            //pnlHeader.Controls.Add(btnEdit);
            //pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnToggleView);
            pnlHeader.Controls.Add(lblWardInfo);
            pnlHeader.Controls.Add(lblWardDetails);
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            // btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnToggleView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdAdmittedPatients = new DataGridView();
            GenerateDatagridView(grdAdmittedPatients);
            grdAdmittedPatients.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grdAdmittedPatients.ColumnHeadersHeight = 35;
            grdAdmittedPatients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdAdmittedPatients.EnableHeadersVisualStyles = false;
            grdAdmittedPatients.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdAdmittedPatients.RowTemplate.Height = 35;
            grdAdmittedPatients.RowHeadersVisible = false;
            grdAdmittedPatients.BackgroundColor = Color.White;
            grdAdmittedPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            grdAdmittedPatients.Location = new System.Drawing.Point(0, 39);
            grdAdmittedPatients.Name = "grdStockItems";
            grdAdmittedPatients.Size = new System.Drawing.Size(1258, 201);
            grdAdmittedPatients.TabIndex = 1;
            grdAdmittedPatients.AllowUserToAddRows = false;
            grdAdmittedPatients.AllowUserToDeleteRows = false;
            grdAdmittedPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdAdmittedPatients.CellContentClick += new DataGridViewCellEventHandler(grdAdmittedPAtients_CellContentClick);
            foreach (AdmittedPatient a in Group)
            {
                grdAdmittedPatients.Rows.Add(a.AdmittedPatientId, a.Bed.RoomNo, a.Bed.BedId, a.Patient == null ? 0 : a.Patient.PatientId, a.Patient == null ? "" : a.Patient.MRNo.ToString(), a.Patient == null ? "" : a.Patient.Name, a.AdmissionNo, a.CreateDate,  "" );
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = Key.WardId;

            pnlDetail.Controls.Add(grdAdmittedPatients);
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
            DataGridViewTextBoxColumn colAdmittedPatientId;
            DataGridViewTextBoxColumn colBedNo;
            DataGridViewTextBoxColumn colBedID;
            DataGridViewTextBoxColumn colPatientId;            
            DataGridViewTextBoxColumn colMrNo;
            DataGridViewTextBoxColumn colName;
            DataGridViewTextBoxColumn colAdmissionNo;
            //DataGridViewTextBoxColumn colInsurance;
            DataGridViewTextBoxColumn colCreatedAt;
            DataGridViewTextBoxColumn colUser;
            DataGridViewButtonColumn colView;
            DataGridViewButtonColumn colTransfer;
            DataGridViewButtonColumn colAddInvoice;
            DataGridViewButtonColumn colDischarge;

            colAdmittedPatientId = new DataGridViewTextBoxColumn();
            colBedNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBedID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPatientId = new DataGridViewTextBoxColumn();
            colMrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAdmissionNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colInsurance= new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colView = new DataGridViewButtonColumn();
            colTransfer = new DataGridViewButtonColumn();
            colAddInvoice = new DataGridViewButtonColumn();
            colDischarge = new DataGridViewButtonColumn();


            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colAdmittedPatientId,
            colBedNo,
            colBedID,
            colPatientId,
            colMrNo,
            colName,
            colAdmissionNo,
            //colInsurance,
            colCreatedAt,
            colUser,
            colView, 
            colTransfer,
            colAddInvoice,
            colDischarge
            });
            dataGridView1.Location = new System.Drawing.Point(15, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1258, 150);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;

            // 
            // colAdmittedPatientId
            // 
            colAdmittedPatientId.HeaderText = "AdmittedPatientId";
            colAdmittedPatientId.Name = "colAdmittedPatientId";
            colAdmittedPatientId.ReadOnly = true;
            colAdmittedPatientId.Visible = true;

            // 
            // colBedNo
            // 
            colBedNo.HeaderText = "BedNo";
            colBedNo.Name = "colBedNo";
            colBedNo.ReadOnly = true;
            colBedNo.Visible = true;
            // 
            // colBedId
            // 
            colBedID.HeaderText = "BedId";
            colBedID.Name = "colBedID";
            colBedID.ReadOnly = true;
            colBedID.Visible = true;

            // 
            // colPatientId
            // 
            colBedID.HeaderText = "PatientId";
            colBedID.Name = "colPatientId";
            colBedID.ReadOnly = true;
            colBedID.Visible = true;

            // 
            // colMrNo
            // 
            colMrNo.HeaderText = "MrNo";
            colMrNo.Name = "colMrNo";
            colMrNo.ReadOnly = true;
            colMrNo.Visible = true;
            // 
            // colItemName
            // 
            colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;

            // 
            // colAdmissionNo
            // 
            colAdmissionNo.HeaderText = "Admission No";
            colAdmissionNo.Name = "colAdmissionNo";
            colAdmissionNo.ReadOnly = true;
            colAdmissionNo.Visible = true;

            // 
            // colCraetedAt
            // 
            colCreatedAt.HeaderText = "Date And Time";
            colCreatedAt.Name = "colCraetedAt";
            colCreatedAt.ReadOnly = true;
            colCreatedAt.Visible = true;

            // 
            // colUser
            // 
            colUser.HeaderText = "By";
            colUser.Name = "colUser";
            colUser.ReadOnly = true;
            colUser.Visible = true;

            // 
            // colView
            // 
            colView.HeaderText = "View";
            colView.Name = "colView";
            colView.Text = "View";
            colView.Width = 40;
            colView.UseColumnTextForButtonValue = true;
            colView.ReadOnly = true;
            colView.Visible = true;

            // 
            // colTransfer
            // 
            colTransfer.HeaderText = "Transfer";
            colTransfer.Name = "colTransfer";
            colTransfer.Text = "Transfer";
            colTransfer.Width = 60;
            colTransfer.UseColumnTextForButtonValue = true;
            colTransfer.ReadOnly = true;
            colTransfer.Visible = true;

            // 
            // colAddInvoice
            // 
            colAddInvoice.HeaderText = "Add Invoice";
            colAddInvoice.Name = "colAddInvoice";
            colAddInvoice.Text = "Add Invoice";
            colAddInvoice.Width = 70;
            colAddInvoice.UseColumnTextForButtonValue = true;
            colAddInvoice.ReadOnly = true;
            colAddInvoice.Visible = true;

            // 
            // colDischarge
            // 
            colDischarge.HeaderText = "Discharge";
            colDischarge.Name = "colDischarge";
            colDischarge.Text = "Discharge";
            colDischarge.Width = 70;
            colDischarge.UseColumnTextForButtonValue = true;
            colDischarge.ReadOnly = true;
            colDischarge.Visible = true;
        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Record", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (rs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            long Stockid = (long)((Button)sender).Tag;
            unitOfWork.StockRepository.SetInactive(Stockid);
            foreach (Panel p in flowPanel.Controls)
            {
                if (Convert.ToInt64(p.Tag) == Stockid)
                {
                    flowPanel.Controls.Remove(p);
                }
            }
            btnRefreshForm.PerformClick();
        }

        private void btnToggleView_Click(object sender, EventArgs e)
        {
            Button btnToggleView = (Button)sender;
            Tuple<long, string> tpl = (Tuple<long, string>)btnToggleView.Tag;
            if (tpl.Item2.Equals("IsDetailView"))
            {
                btnToggleView.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Theme", "list36.png"));
                btnToggleView.Tag = new Tuple<long, string>(tpl.Item1, "IsBedView");
            }
            else
            {
                btnToggleView.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Theme", "gridview36.png"));
                btnToggleView.Tag = new Tuple<long, string>(tpl.Item1, "IsDetailView");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadAdmittedPatients();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadAdmittedPatients();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAdmittedPatients();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = AdmittedPatients.PageCount;
            LoadAdmittedPatients();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAdmittedPatients();
        }

        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)e.Control;
            AddedPanel.Width = flowPanel.Width - 23;
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmManageStockShortCuts f = new ShortCutDialogs.frmManageStockShortCuts();
            f.ShowDialog();
        }

        private void btnAssignBed_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new Indoor.Menu.frmAssignBed(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAdmittedPatients();
        }

        private void grdAdmittedPAtients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                DataGridView grd = (DataGridView)sender;
                long AdmittedPatientId = Convert.ToInt64(grd.Rows[e.RowIndex].Cells["colAdmittedPatientId"].Value);
                
                if (e.ColumnIndex == grd.Columns["colView"].Index)
                {
                    SharedFunctions.OpenForm(new frmAdmittedPatientTreatment(AdmittedPatientId), this.MdiParent);                    
                }
                else if (e.ColumnIndex == grd.Columns["colTransfer"].Index)
                {

                    frmAssignBed f = new frmAssignBed(AdmittedPatientId);
                    f.Show();
                }
                else if (e.ColumnIndex == grd.Columns["colAddInvoice"].Index)
                {
                    long PatientId = Convert.ToInt64(grd.Rows[e.RowIndex].Cells["colPatientId"].Value);
                    frmInvoice f = new frmInvoice(PatientId);
                    f.Show();
                }
                else if (e.ColumnIndex == grd.Columns["colDischarge"].Index)
                {
                    MessageBox.Show("Discharger clicked");
                }
            }
            catch(Exception ex)
            {
            }        
        }
    }
}