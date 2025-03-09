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

namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmAddVital : Form
    {
        UnitOfWork unitOfWork;
        private long VitalId = 0;
        private long AdmittedPatientId = 0;
        public frmAddVital()
        {
            InitializeComponent();
        }

        public frmAddVital(long VitalId, long AdmittedPatientId)
        {
            InitializeComponent();
            this.VitalId = VitalId;
            this.AdmittedPatientId = AdmittedPatientId;
        }    

        private void frmAddVital_Load(object sender, EventArgs e)
        {
            LoadVitalData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.VitalId > 0)
                {
                    UpdateVital();
                }
                else
                {
                    SaveVital();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing required action, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadVitalData()
        {
            try
            {
                Vital obj = new Vital();
                using (unitOfWork = new UnitOfWork())
                {
                    obj = unitOfWork.VitalRepository.GetById(this.VitalId);
                    if (obj != null)
                    {
                        numWeight.Value = (decimal)obj.Weight;
                        txtTemprature.Text = obj.Temprature.ToString();
                        txtBloodPressure.Text = obj.BloodPressure.ToString();
                        txtBloodPressure.Text = obj.BloodPressure.ToString();
                        txtBloodSugar.Text = obj.BloodSugar.ToString();
                        numTest.Value = (decimal)obj.Test;
                        txtRespiratoryRate.Text = obj.RespiratoryRate.ToString();
                        txtOxygenSaturation.Text = obj.OxygenSaturation.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading vital data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveVital()
        {
            try
            {
                Vital obj = new Vital();
                obj.Weight = (double)numWeight.Value;
                obj.Temprature = double.Parse(txtTemprature.Text);
                obj.BloodPressure = double.Parse(txtBloodPressure.Text);
                obj.BloodSugar = double.Parse(txtBloodSugar.Text);
                obj.Test = (int)numTest.Value;
                obj.RespiratoryRate = double.Parse(txtRespiratoryRate.Text);
                obj.OxygenSaturation = double.Parse(txtOxygenSaturation.Text);
                obj.CreateDate = DateTime.Now;
                obj.UpdateDate = DateTime.Now;
                obj.IsActive = true;
                obj.IsNew = true;
                using (unitOfWork = new UnitOfWork())
                {
                    obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    obj.AdmittedPatient = unitOfWork.AdmitPatientsRepository.GetById(this.AdmittedPatientId);
                    unitOfWork.VitalRepository.Insert(obj);
                    unitOfWork.Save();
                }
                MessageBox.Show("Vital saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving vatils, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateVital()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Vital obj = unitOfWork.VitalRepository.GetById(this.VitalId);
                    obj.Weight = (double)numWeight.Value;
                    obj.Temprature = double.Parse(txtTemprature.Text);
                    obj.BloodPressure = double.Parse(txtBloodPressure.Text);
                    obj.BloodSugar = double.Parse(txtBloodSugar.Text);
                    obj.Test = (int)numTest.Value;
                    obj.RespiratoryRate = double.Parse(txtRespiratoryRate.Text);
                    obj.OxygenSaturation = double.Parse(txtOxygenSaturation.Text);
                    obj.UpdateDate = DateTime.Now;
                    obj.IsUpdate = true;
                    obj.IsSynced = false;
                    obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    unitOfWork.VitalRepository.Update(obj);
                    unitOfWork.Save();
                }
                MessageBox.Show("Vital updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating vatils, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (numWeight.Value < 0)
            {
                errWeight.Visible = true;
                ErrFound = true;
            }
            else
            {
                errWeight.Visible = false;
            }
            if (txtTemprature.Text.Trim() == "")
            {
                txtTemprature.Text = "0";
            }

            if (txtBloodPressure.Text.Trim() == "")
            {
                txtBloodPressure.Text = "0";
            }

            if (txtBloodSugar.Text.Trim() == "")
            {
                txtBloodSugar.Text = "0";
            }

            if (txtRespiratoryRate.Text.Trim() == "")
            {
                txtRespiratoryRate.Text = "0";
            }

            if (txtOxygenSaturation.Text.Trim() == "")
            {
                txtOxygenSaturation.Text = "0";
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

        private void txtTemprature_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtBloodPressure_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtBloodSugar_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtRespiratoryRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtOxygenSaturation_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void Clear()
        {
            numWeight.Value = 0;
            txtTemprature.Text = "";
            txtBloodPressure.Text = "";
            txtBloodSugar.Text = "";
            numTest.Value = 0;
            txtRespiratoryRate.Text = "";
            txtOxygenSaturation.Text = "";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}