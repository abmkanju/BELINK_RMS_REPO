using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Infrastructure;
using PagedList;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPatients : Form
    {
        static PharmacyContext cxt;
        private IPagedList<Patient> PatientsList;
        PatientRepository repPatients;
        TagsRepository repTags;
        int PageNo = 1;
        int PageSize = SharedVariables.PageSize;
        bool isByTag = false;
        bool isSearch = false;
        int SelectedTagId = 0;
        string SearchString = "";
        bool isAdvancedSearch = false;
        private void InitializedObjects()
        {
            cxt = new PharmacyContext();
            repPatients = new PatientRepository(cxt);
            repTags = new TagsRepository(cxt);
        }
        public frmPatients()
        {
            InitializeComponent();
            InitializedObjects();
        }
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddPatient(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void LoadAllTags()
        {

            List<Tag> tagsList = repTags.GetAll().ToList();
            foreach (Tag t in tagsList)
            {
                Label lblTag = new Label();
                lblTag.AutoSize = true;
                lblTag.BackColor = System.Drawing.SystemColors.Highlight;
                lblTag.ForeColor = System.Drawing.Color.White;
                lblTag.Cursor = Cursors.Hand;
                var margin = lblTag.Margin;
                margin.Top = 5;
                lblTag.Margin = margin;
                lblTag.Location = new System.Drawing.Point(333, 360);
                lblTag.MinimumSize = new System.Drawing.Size(0, 18);
                lblTag.Name = "lblTag";
                lblTag.Size = new System.Drawing.Size(41, 18);
                lblTag.TabIndex = 20;
                lblTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblTag.Text = t.TagName.ToUpper();
                lblTag.Tag = t.TagId;
                lblTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lblTag.Click += new EventHandler(lblTag_Clicked);
                pnlTags.Controls.Add(lblTag);
            }
        }
        private void lblTag_Clicked(object sender, EventArgs e)
        {
            PageNo = 1;
            PageSize = 2;
            isByTag = true;
            Label clickedLabel = (Label)sender;
            SelectedTagId = int.Parse(clickedLabel.Tag.ToString());
            GetPatientsAsync_ByTag(SelectedTagId);
        }
        private void frmPatients_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddPatient });
                LoadForm();
                SharedFunctions.SetGridStyle(grdPatients);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading patients data, please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadForm()
        {
            GetPatientsAsync();
            LoadAllTags();
            cmbSearchBy.SelectedIndex = 0;
        }
        private void GetPatientsAsync()
        {
            if (grdPatients.Columns.Count <= 0) //----------- The use of this check is to avoid the exception ----------- //
                return;
            grdPatients.Rows.Clear();
            PatientsList = repPatients.GetPatientsAsync(PageNo, PageSize);
            btnFirstPage.Enabled = btnPrevious.Enabled = PatientsList.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = PatientsList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, PatientsList.PageCount);
            foreach (Patient p in PatientsList)
            {
                grdPatients.Rows.Add(p.PatientId, p.MRNo, p.Name, p.Gender, p.Phone, p.ReferredBy, p.RegistrationDate);
            }
        }
        private void GetPatientsAsync_ByTag(int TagId)
        {
            grdPatients.Rows.Clear();
            PatientsList = repPatients.GetPatientsAsync_ByTag(TagId, PageNo, PageSize);
            btnPrevious.Enabled = PatientsList.HasPreviousPage;
            btnNext.Enabled = PatientsList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, PatientsList.PageCount);
            foreach (Patient p in PatientsList)
            {
                grdPatients.Rows.Add(p.PatientId, p.MRNo, p.Name, p.Gender, p.Phone, p.ReferredBy, p.RegistrationDate);
            }
        }
        private void GetPatientsAsync_BySearch()
        {
            grdPatients.Rows.Clear();
            PatientsList = repPatients.GetPatientsAsync_BySearch(SearchString, PageNo, PageSize);
            btnPrevious.Enabled = PatientsList.HasPreviousPage;
            btnNext.Enabled = PatientsList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, PatientsList.PageCount);
            foreach (Patient p in PatientsList)
            {
                grdPatients.Rows.Add(p.PatientId, p.MRNo, p.Name, p.Gender, p.Phone, p.ReferredBy, p.RegistrationDate);
            }
        }
        private void GetPatientsAsync_ByAdvancedSearch(string SearchString, string SearchBy)
        {
            grdPatients.Rows.Clear();
            PatientsList = repPatients.GetPatientsAsync_ByAdvancedSearch(SearchString, SearchBy, PageNo, PageSize);
            btnPrevious.Enabled = PatientsList.HasPreviousPage;
            btnNext.Enabled = PatientsList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, PatientsList.PageCount);
            foreach (Patient p in PatientsList)
            {
                grdPatients.Rows.Add(p.PatientId, p.MRNo, p.Name, p.Gender, p.Phone, p.ReferredBy, p.RegistrationDate);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlTags.Controls.Clear();
            PageNo = 1;
            isByTag = false;
            isSearch = false;
            SelectedTagId = 0;
            txtSearch.Text = "";
            SearchString = "";
            lblAdvancedSearch.Text = "Advanced Search";
            isAdvancedSearch = false;
            cmbSearchBy.Visible = false;
            cmbSearchBy.SelectedIndex = 0;
            InitializedObjects();
            LoadForm();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchString = txtSearch.Text.Trim();
                if (SearchString != "")
                {
                    if (isAdvancedSearch)
                    {
                        GetPatientsAsync_ByAdvancedSearch(SearchString, cmbSearchBy.GetItemText(cmbSearchBy.SelectedItem));
                        return;
                    }
                    isSearch = true;
                    GetPatientsAsync_BySearch();
                }
            }
        }

        private void lblAdvancedSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!isAdvancedSearch)
            {
                lblAdvancedSearch.Text = "General Search";
                isAdvancedSearch = true;
                cmbSearchBy.Visible = true;
            }
            else
            {
                lblAdvancedSearch.Text = "Advanced Search";
                isAdvancedSearch = false;
                cmbSearchBy.Visible = false;
            }
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void grdPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    long PatientId = Convert.ToInt64(grdPatients.Rows[e.RowIndex].Cells["colPatientId"].Value);
                    string mrNo = SharedFunctions.ObjectToString(grdPatients.Rows[e.RowIndex].Cells["colMRNo"].Value);
                    if (e.ColumnIndex == grdPatients.Columns["colViewPatient"].Index)
                    {
                        Form f = SharedFunctions.OpenForm(new frmPatientProfile(PatientId), this.MdiParent);
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    }
                    else if (e.ColumnIndex == grdPatients.Columns["colAddInvoice"].Index)
                    {
                        frmInvoice f = new frmInvoice(PatientId);
                        f.Show();
                    }
                    else if (e.ColumnIndex == grdPatients.Columns["colDelete"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Delete Patients") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.Description.Equals("Super Admin")))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        DialogResult rs = MessageBox.Show("Are you Sure, You Want To Delete This Patient", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            repPatients.SetInActive(PatientId);
                            using (UnitOfWork unitOfWork = new UnitOfWork())
                            {
                                AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + mrNo, AuditModule.Patient, AuditAction.deleted);
                                //SOT
                                Patient objPatient = new Patient() { PatientId = PatientId };
                                if (objPatient != null)
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, objPatient, ModuleName.Patient, ModuleAction.Delete, PatientId);
                            }
                            MessageBox.Show("Patient Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnRefresh.PerformClick();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = PatientsList.Count;
            GetPatientsAsync();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo--;
            GetPatientsAsync();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            GetPatientsAsync();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo++;
            GetPatientsAsync();
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > PatientsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    GetPatientsAsync();
                }
            }
        }
    }
}