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
    public partial class frmAddDepartment : Form
    {
        UnitOfWork unitOfWork;
        int EditingRowIndex = -1;
        public long DepartmentId = 0;
        private List<long> DeletedSubDepsIds = new List<long>();
        public frmAddDepartment()
        {
            InitializeComponent();
        }

        public frmAddDepartment(long DepartmentId)
        {
            InitializeComponent();
            this.DepartmentId = DepartmentId;
        }
        private void frmAddDepartment_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[]{this.btnSave});
                SharedFunctions.SetSmallButtonsStyle(new[] { this.btnAdd});
                LoadDepartments();
                if (this.DepartmentId > 0)
                {
                    cmbDepartments.SelectedValue = this.DepartmentId;
                    this.LoadDepartmentData();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while loading form data, please try again", ex.Message, "Load Failed");
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (keyData == (Keys.Control| Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }
            //if (keyData == (Keys.Alt | Keys.G))
            //{
            //    grdItems.Focus();
            //    if (grdItems.Rows.Count >= 1)
            //    {
            //        grdItems.Rows[0].Selected = true;
            //    }
            //    return true;
            //}
            //if (keyData == (Keys.Alt | Keys.D))
            //{
            //    btnClear.PerformClick();
            //    return true;
            //}

            //if (keyData == (Keys.Alt | Keys.X))
            //{
            //    numTotalDiscount.Focus();
            //    return true;
            //}

            //if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            //{
            //    btnAddNewItem.PerformClick();
            //    return true;
            //}

            //if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            //{
            //    btnAddStock.PerformClick();
            //    return true;
            //}


            //if (keyData == (Keys.Control | Keys.S))
            //{
            //    btnSave.PerformClick();
            //    return true;
            //}

            //if (keyData == (Keys.Control | Keys.P))
            //{
            //    btnSaveAndPrint.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Control | Keys.D))
            //{
            //    btnClearAll.PerformClick();
            //    return true;
            //}

            //if (keyData == (Keys.Control | Keys.N))
            //{
            //    txtSearchByName.Focus();
            //    return true;
            //}
            //#region grid short cuts
            //if (keyData == (Keys.Alt | Keys.E))
            //{
            //    if (grdItems.SelectedRows.Count > 0)
            //    {
            //        int colIndex = grdItems.Columns["colEdit"].Index;
            //        int rowIndex = grdItems.SelectedRows[0].Index;
            //        DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
            //        grdItems_CellContentClick(grdItems, e);
            //        cmbSelectBatch.Focus();
            //    }
            //    return true;
            //}

            //if (keyData == (Keys.Alt | Keys.R))
            //{
            //    {
            //        int colIndex = grdItems.Columns["colRemove"].Index;
            //        int rowIndex = grdItems.SelectedRows[0].Index;
            //        DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
            //        grdItems_CellContentClick(grdItems, e);
            //        cmbSelectBatch.Focus();
            //    }
            //    return true;
            //}
            //#endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadDepartmentData()
        {
            DepartmentVM dep = new DepartmentVM();
            using (unitOfWork = new UnitOfWork())
            {
                dep = unitOfWork.DepratmentRepository.GetDepWithActiveSubDeps(this.DepartmentId);
            }
            foreach (SubDepartment sd in dep.SubDepartments)
            {
                grdsp.Rows.Add(sd.SubDepartmentId, sd.Name);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<Department> deps = unitOfWork.DepratmentRepository.GetAll().ToList();
                    Department defaultDep = new Department { Name = "Select Department" };
                    deps.Insert(0, defaultDep);
                    cmbDepartments.DataSource = deps;
                    cmbDepartments.ValueMember = "DepartmentId";
                    cmbDepartments.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (IsValidInput())
            {
                if (EditingRowIndex >= 0)
                {
                    UpdateRow();
                }
                else
                {
                    grdsp.Rows.Add(0, txtSubDep.Text.Trim());
                }
                Clear();
            }
        }
        private void ClearForm()
        {
            Clear();
            this.DepartmentId = 0;
            this.DeletedSubDepsIds = new List<long>();
        }
        private void UpdateRow()
        {
            DataGridViewRow r = grdsp.Rows[EditingRowIndex];
            r.Cells["colDepartmentName"].Value = txtSubDep.Text.Trim();
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if(cmbDepartments.SelectedIndex == 0)
            {
                errSelectDep.Visible = true;
                ErrFound = true;
            }
            else
            {
                errSelectDep.Visible = false;
            }
            if (string.IsNullOrEmpty(txtSubDep.Text.Trim()))
            {
                errSelectDep.Visible = false;
                ErrFound = true;
            }
            else
            {
                ErrItemName.Visible = false;
            }
            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }


        private void Clear()
        {
            btnAdd.Text = "Add";
            txtSubDep.Text = "";
            if (EditingRowIndex >= 0)
            {
                grdsp.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }
        private void grdsp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long subDepId = Convert.ToInt64(grdsp.Rows[e.RowIndex].Cells["colSubDepartmentId"].Value);
            if (e.ColumnIndex == grdsp.Columns["colRemove"].Index)
            {
                if (subDepId > 0)
                {
                    DialogResult res = MessageBox.Show("Are you sure you want to deleted selected sub department", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        DeletedSubDepsIds.Add(subDepId);
                    }
                }
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                grdsp.Rows.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdsp.Columns["colEdit"].Index)
            {
                Clear();
                EditSubDepartment(grdsp.Rows[e.RowIndex]);
                btnAdd.Text = "Update";
                grdsp.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;
            }
        }

        private void EditSubDepartment(DataGridViewRow r)
        {
            txtSubDep.Text = r.Cells["colDepartmentName"].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DepartmentId > 0)
                {
                    Update();
                }
                else
                {
                    Add();
                }
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while saving/updating departments.", ex.Message, "Error");
            }
        }

        private void Add()
        {
            try
            {
                Department dep;
                using (unitOfWork = new UnitOfWork())
                {
                    dep = unitOfWork.DepratmentRepository.GetById(this.cmbDepartments.SelectedValue);

                    if (dep != null)
                    {
                        dep.UpdateDate = DateTime.Now;
                        dep.IsUpdate = true;
                        dep.IsSynced = false;
                        dep.SubDepartments = new List<SubDepartment>();
                        foreach (DataGridViewRow r in grdsp.Rows)
                        {
                            SubDepartment sd = new SubDepartment();
                            FillObjec(ref sd, r);
                            dep.SubDepartments.Add(sd);
                        }
                        unitOfWork.DepratmentRepository.Update(dep);
                        unitOfWork.Save();
                        MessageBox.Show("Department Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while updating Departments.", ex.Message, "Error");
            }
        }
        private void Update()
        {
            try
            {
                Department dep;
                long subDepId = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    dep = unitOfWork.DepratmentRepository.GetDepWithAllSubDeps(Convert.ToInt64(this.cmbDepartments.SelectedValue));
                    if (dep != null)
                    {
                        dep.UpdateDate = DateTime.Now;
                        dep.IsUpdate = true;
                        dep.IsSynced = false;
                        //dep.SubDepartments = new List<SubDepartment>();
                        foreach (long id in DeletedSubDepsIds)
                        {
                            foreach (SubDepartment sd in dep.SubDepartments)
                            {
                                if (id == sd.SubDepartmentId)
                                {
                                    sd.IsActive = false;
                                    sd.IsUpdate = true;
                                    sd.IsSynced = false;
                                    unitOfWork.GetDbContext().Entry(sd).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }

                        foreach (DataGridViewRow r in grdsp.Rows)
                        {
                            subDepId = 0;
                            subDepId = Convert.ToInt64(r.Cells["colSubDepartmentId"].Value);
                            if (subDepId > 0)
                            {
                                SubDepartment sdp = dep.SubDepartments.Where(sd => sd.SubDepartmentId == subDepId).FirstOrDefault(); ;
                                sdp.Name = r.Cells["colDepartmentName"].Value.ToString();
                                sdp.UpdateDate = DateTime.Now;
                                sdp.IsUpdate = true;
                                sdp.IsSynced = false;
                                unitOfWork.GetDbContext().Entry(sdp).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                SubDepartment sd = new SubDepartment();
                                FillObjec(ref sd, r);
                                dep.SubDepartments.Add(sd);
                            }
                        }
                        unitOfWork.DepratmentRepository.Update(dep);
                        unitOfWork.Save();
                        MessageBox.Show("Department updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while updating Departments.", ex.Message, "Error");
            }
        }
        private void FillObjec(ref SubDepartment sd, DataGridViewRow r)
        {

            sd.CreateDate = sd.UpdateDate = DateTime.Now;
            sd.IsActive = true;
            sd.Name = r.Cells["colDepartmentName"].Value.ToString();
            sd.User = SharedVariables.LoggedInUser;
        }

        private void FillUpdateObjec(ref SubDepartment sd, DataGridViewRow r)
        {

        }

        private void txtSubDep_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnAdd.PerformClick();
            }
        }
    }
}