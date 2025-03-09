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
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddSupplier : Form
    {
        UnitOfWork unitOfWork;
        int PageNo = 1;
        long SupplierEditId = 0;
        public frmAddSupplier()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public frmAddSupplier(long SupplierEditId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.SupplierEditId = SupplierEditId;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSuppName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtPrimContName.Text = "";
            txtPrimaryContPh.Text = "";
            numOpBal.Value = 0;
            txtstn.Clear();
            txtntn.Clear();
            ErrSuppName.Visible = false;
            ErrPhoneNo.Visible = false;
            ErrAddress.Visible = false;
            ErrPrimContName.Visible = false;
            ErrPrimContPh.Visible = false;
            ErrOpBal.Visible = false;
            btnAddSupplier.Text = "Add";
        }    

        private void FillObject(Supplier objSupplier)
        {
            objSupplier.Name = txtSuppName.Text;
            objSupplier.Phone = txtPhone.Text;
            objSupplier.Address = txtAddress.Text;
            objSupplier.PrimaryContactPersonName = txtPrimContName.Text;
            objSupplier.PrimaryContactPersonPhone = txtPrimaryContPh.Text;
            objSupplier.NTN = txtntn.Text;
            objSupplier.STN = txtstn.Text;
            objSupplier.CreditLImit = Convert.ToInt32(numcreditlimit.Value);
            objSupplier.OpeningBalance = (double)numOpBal.Value;            
            if (this.SupplierEditId <= 0)
            {
                objSupplier.IsNew = true;
                objSupplier.CreatedAt = DateTime.Now;
                objSupplier.IsActive = true;
            }
            objSupplier.UpdatedAt = DateTime.Now;
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtSuppName.Text.Trim()))
            {
                ErrSuppName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSuppName.Visible = false;
            }
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                ErrPhoneNo.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrPhoneNo.Visible = false;
            }
            //if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            //{
            //    ErrAddress.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrAddress.Visible = false;
            //}
            
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

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if(this.SupplierEditId > 0)
                    {
                        this.btnAddSupplier.Text = "Update";
                        UpdateSupplier();
                    }
                    else
                    {
                        InsertSupplier();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                unitOfWork = new UnitOfWork();
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Supplier with Given Name Already Exists, Please Specify Another Name", "Supplier Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }                    
                }
               // MessageBox.Show(ex.InnerException.InnerException.Message, "Inner exception details");
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void InsertSupplier()
        {            
            Supplier objSupplier = new Supplier();
            FillObject(objSupplier);
            using (unitOfWork = new UnitOfWork())
            {
                objSupplier.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.SupplierRepository.Insert(objSupplier);
                AuditLogTask.CreateAuditLog(unitOfWork, objSupplier.Name, AuditModule.Supplier, AuditAction.created);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork,objSupplier, ModuleName.Supplier, ModuleAction.Create,objSupplier.SupplierID);
            }
            MessageBox.Show("Supplier Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateSupplier()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Supplier objSupplier = unitOfWork.SupplierRepository.GetById(this.SupplierEditId);
                FillObject(objSupplier);
                if (objSupplier.IsSynced)
                {
                    objSupplier.IsNew = false;
                    objSupplier.IsUpdate = true;
                    objSupplier.IsSynced = false;
                }
                objSupplier.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.SupplierRepository.Update(objSupplier);
                AuditLogTask.CreateAuditLog(unitOfWork, objSupplier.Name, AuditModule.Supplier, AuditAction.updated);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objSupplier, ModuleName.Supplier, ModuleAction.Update, objSupplier.SupplierID);
            }
            MessageBox.Show("Supplier Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        private void frmAddSupplier_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            try
            {
                if (this.SupplierEditId > 0)
                {                    
                    pnlSupplierItems.Visible = true;
                    LoadSupplier();
                    btnAddSupplier.Text = "Update";
                    this.Text = "Update Supplier";
                }
                else
                {
                    this.Size = new System.Drawing.Size(415, 450);
                    this.Text = "Add Supplier";
                }
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadSupplier()
        {
            SupplierVM vm = new SupplierVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.SupplierRepository.GetSupplierWithItems(this.SupplierEditId, PageNo, SharedVariables.PageSize);
            }
            txtSuppName.Text = vm.Name;
            txtPhone.Text = vm.Phone;
            txtntn.Text = vm.NTN;
            txtstn.Text = vm.STN;
            numcreditlimit.Value = vm.CreditLImit;
            txtAddress.Text = vm.Address;
            txtPrimContName.Text = vm.PrimaryContactPersonName;
            txtPrimaryContPh.Text = vm.PrimaryContactPersonPhone;
            numOpBal.Value = (decimal)vm.OpeningBalance;
            foreach (Item i in vm.Items)
            {
                grdSupplierItems.Rows.Add(i.ItemName, i.IsNarcotic ? "Narcotic" : "Drug");
            }
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAddSupplierShortCuts f = new ShortCutDialogs.frmAddSupplierShortCuts();
            f.ShowDialog();
        }
    }
}