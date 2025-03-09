using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Infrastructure;

namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmWard : Form
    {
        UnitOfWork unitOfWork;
        long WardId = 0;
        public frmWard()
        {
            InitializeComponent();
        }

        public frmWard(long WardId)
        {
            InitializeComponent();
            this.WardId = WardId;
        }

        private void frmWard_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDepartments();
                if (this.WardId > 0)
                {
                    LoadWard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadWard()
        {
            try
            {
                Ward objWard = new Ward();
                using (unitOfWork = new UnitOfWork())
                {
                    objWard = unitOfWork.WardRepository.GetWardByIdWithSubDepartment(this.WardId);
                }
                if (objWard != null)
                {
                    txtWardName.Text = objWard.Name.ToString();
                    cmbDepartments.SelectedValue = objWard.SubDepratment.SubDepartmentId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading ward data for editing.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            using (unitOfWork = new UnitOfWork())
            {
                List<SubDepartment> Deps = unitOfWork.DepratmentRepository.GetActiveIPDSubDepartments();
                SubDepartment sd = new SubDepartment();
                sd.SubDepartmentId = 0;
                sd.Name = "Select Department";
                Deps.Insert(0, sd);
                cmbDepartments.DataSource = Deps;
                cmbDepartments.ValueMember = "SubDepartmentId";
                cmbDepartments.DisplayMember = "Name";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                if (this.WardId > 0)
                {
                    UpdateWard();
                }
                else
                {
                    AddWard();
                }
            }
        }

        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (txtWardName.Text.Trim() == "")
            {
                errName.Visible = true;
                ErrFound = true;
                txtWardName.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                errName.Visible = false;
            }

            if (Convert.ToInt64(cmbDepartments.SelectedValue) == 0)
            {
                errDep.Visible = true;
                ErrFound = true;
                cmbDepartments.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                errDep.Visible = false;
            }



            if (!ErrFound)
            {
                //ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }
        private void AddWard()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Ward objWard = new Ward();
                    objWard.Name = txtWardName.Text.Trim();
                    objWard.SubDepratment = unitOfWork.SubDepartmentRepository.GetById(cmbDepartments.SelectedValue);
                    objWard.IsActive = true;
                    objWard.IsNew = true;
                    objWard.CreateDate = DateTime.Now;
                    objWard.UpdateDate = DateTime.Now;
                    unitOfWork.WardRepository.Insert(objWard);
                    unitOfWork.Save();
                    MessageBox.Show("Ward saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        SqlException ex2 = ex.InnerException.InnerException as SqlException;
                        if (ex2.Number == 2601)
                        {
                            MessageBox.Show("Ward with given name already exists, please try another name.", "Ward Already Exists.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                MessageBox.Show("Error occurred while saving ward.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateWard()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Ward objWard = unitOfWork.WardRepository.GetById(this.WardId);
                    objWard.Name = txtWardName.Text.Trim();
                    objWard.SubDepratment = unitOfWork.SubDepartmentRepository.GetById(cmbDepartments.SelectedValue);
                    objWard.IsUpdate = true;
                    objWard.IsSynced = false;
                    objWard.UpdateDate = DateTime.Now;
                    unitOfWork.WardRepository.Update(objWard);
                    unitOfWork.Save();
                    MessageBox.Show("Ward updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while updating ward.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}