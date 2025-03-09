using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddManufacturer : Form
    {
        UnitOfWork unitOfWork;
        private int ManufacturerId = 0;
        public frmAddManufacturer()
        {
            InitializeComponent();
        }
        public frmAddManufacturer(int ManufacturerId)
        {
            InitializeComponent();
            this.ManufacturerId = ManufacturerId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Manufacturer obj = new Manufacturer();
            if (IsValidInput())
            {
                fillObject(obj);

                if (this.ManufacturerId > 0)
                {
                    if (!ManufExists_Update(txtName.Text.Trim().ToLower(), this.ManufacturerId))
                    {
                        Update(obj);
                    }
                    else
                    {
                        MessageBox.Show("Manufacturer with Given Name Already Exists, Please Specify Another Name", "Manufacturer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    if (!ManufExists_new(txtName.Text.Trim().ToLower()))
                    {
                        Save(obj);
                    }
                    else
                    {
                        MessageBox.Show("Manufacturer with Given Name Already Exists, Please Specify Another Name", "Manufacurer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private bool ManufExists_new(string name)
        {
            bool val  = false;
            using(unitOfWork = new UnitOfWork ())
            {
                val = unitOfWork.ManufacturerRepository.ManufacturerExists_New(name);
            }
            return val;
        }

        private bool ManufExists_Update(string name, int manufacturerId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.ManufacturerRepository.ManufacturerExists_Update(name, manufacturerId);
            }
        }
        private void Save(Manufacturer obj)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.ManufacturerRepository.Insert(obj);
                    AuditLogTask.CreateAuditLog(unitOfWork, obj.Name, AuditModule.Manufacturer, AuditAction.created);
                    unitOfWork.Save();
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, obj, ModuleName.Manufacturer, ModuleAction.Create, obj.ManufacturerId);
                    MessageBox.Show("Manufacturer data inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Manufacturer with Given Name Already Exists, Please Specify Another Name", "Supplier Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("An error occurred while inserting manufacturer data, please try again.", "Failer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Update(Manufacturer obj)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Manufacturer objM = unitOfWork.ManufacturerRepository.GetById(this.ManufacturerId);
                    if (objM.IsSynced)
                    {
                        objM.IsNew = false;
                        objM.IsUpdate = true;
                        objM.IsSynced = false;
                    }
                    objM.Name = obj.Name;
                    AuditLogTask.CreateAuditLog(unitOfWork, obj.Name, AuditModule.Manufacturer, AuditAction.updated);
                    unitOfWork.ManufacturerRepository.Update(objM);
                    unitOfWork.Save();
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, objM, ModuleName.Manufacturer, ModuleAction.Update, objM.ManufacturerId);
                    MessageBox.Show("Manufacturer data updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Manufacturer with Given Name Already Exists, Please Specify Another Name", "Supplier Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("An error occurred while updating manufacturer data, please try again.", "Failer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fillObject(Manufacturer obj)
        {
            obj.Name = txtName.Text.Trim();
            obj.CreatedAt = DateTime.Now;
            obj.UpdatedAt = DateTime.Now;
            obj.IsSynced = false;
            obj.IsActive = true;
            obj.IsNew = true;
            obj.IsUpdate = false;
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrName.Visible = false;
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

        private void frmAddManufacturer_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave });
            if (this.ManufacturerId > 0)
            {
                LoadData(this.ManufacturerId);
            }
        }

        private void LoadData(int p_manufacturerId)
        {
            Manufacturer obj = new Manufacturer();
            using (unitOfWork = new UnitOfWork())
            {
                obj = unitOfWork.ManufacturerRepository.GetById(p_manufacturerId);
            }
            this.txtName.Text = obj.Name;
        }
    }
}