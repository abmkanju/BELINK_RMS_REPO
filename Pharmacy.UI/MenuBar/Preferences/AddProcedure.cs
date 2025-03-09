using Pharmacy.Core;
using Pharmacy.Core.Entities;
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
    public partial class AddProcedure : Form
    {
        UnitOfWork unitOfWork;
        long ProcedureId = 0;
        public AddProcedure()
        {
            InitializeComponent();
        }
        public AddProcedure(long ProcedureId)
        {
            InitializeComponent();
            this.ProcedureId = ProcedureId;
        }

        private void AddProcedure_Load(object sender, EventArgs e)
        {
            if (this.ProcedureId > 0)
            {
                this.Text = "Edit Procedure";
                LoadProcedure();
            }
        }

        private void LoadProcedure()
        {
            Procedure objProc = new Procedure();
            using (unitOfWork = new UnitOfWork())
            {
                objProc = unitOfWork.ProcedureRepository.GetById(this.ProcedureId);
            }
            txtProcName.Text = objProc.ProcedureName;
            numProcPrice.Value = (decimal)objProc.Price;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.ProcedureId > 0)
                    {
                        UpdateProc();
                    }
                    else
                    {
                        InsertProc();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error Occurred While Saving Procedre", ex.Message, "Error");
            }
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtProcName.Text.Trim()))
            {
                ErrProcName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrProcName.Visible = false;
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
        private void InsertProc()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Procedure objProc = new Procedure();
                    objProc.ProcedureName = txtProcName.Text.Trim();
                    objProc.Price = (double)numProcPrice.Value;
                    objProc.UpdatedAt = objProc.CreatedAt = DateTime.Now;
                    objProc.IsNew = true;
                    objProc.IsSynced = false;
                    objProc.IsActive = true;
                    objProc.IsDefault = false;
                    unitOfWork.ProcedureRepository.Insert(objProc);
                    unitOfWork.Save();
                    MessageBox.Show("Procedure Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error Occurred While Updating Procedure", ex.Message, "Error");
            }
        }

        private void UpdateProc()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Procedure objProc = unitOfWork.ProcedureRepository.GetById(this.ProcedureId);
                    objProc.ProcedureName = txtProcName.Text.Trim();
                    objProc.Price = (double)numProcPrice.Value;
                    objProc.UpdatedAt = DateTime.Now;
                    objProc.IsUpdate = true;
                    objProc.IsSynced = false;
                    unitOfWork.ProcedureRepository.Update(objProc);
                    unitOfWork.Save();
                    MessageBox.Show("Procedure Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error Occurred While Updating Procedure", ex.Message, "Error");
            }
        }
    }
}