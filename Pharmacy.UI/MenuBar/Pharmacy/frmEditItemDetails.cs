using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
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
    public partial class frmEditItemDetails : Form
    {
        UnitOfWork unitOfWork;
        private List<int> ItemIds;
        Int32 catId = 0;
        List<long> supplierList;
        public frmEditItemDetails()
        {
            InitializeComponent();
        }

        public frmEditItemDetails(List<int> ItemIds)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            pbLoading.Visible = false;
            supplierList = new List<long>();
            this.ItemIds = ItemIds;
        }

        #region LoadItems
        private void frmEditItemDetails_Load(object sender, EventArgs e)
        {
            LoadDetails();
        }

        private void LoadDetails()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    LoadSuppliers(unitOfWork);
                    LoadCateogories(unitOfWork);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void LoadCateogories(UnitOfWork unitOfWork)
        {
            List<Category> cts = unitOfWork.CategoryRepository.GetAllActiveCategories();
            Category c = new Category();
            c.CategoryId = 0;
            c.Name = "Select Category";
            cts.Insert(0, c);
            cmbCategory.DataSource = cts;
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DisplayMember = "Name";
        }
        private void LoadSuppliers(UnitOfWork unitOfWork)
        {
            List<Supplier> suppliers = new List<Supplier>();
            suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            lstSuppliers.DataSource = suppliers;
            lstSuppliers.ValueMember = "SupplierID";
            lstSuppliers.DisplayMember = "Name";
        }

        #endregion

        #region FormButtons
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void btnApplyAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidated())
                    return;
                List<Item> items = new List<Item>();
                catId = SharedFunctions.StringToInt32(cmbCategory.SelectedValue);
                foreach (Supplier supp in lstSuppliers.SelectedItems)
                {
                    supplierList.Add(supp.SupplierID);
                }
                this.Invoke(new Action(() =>
                {
                    pbLoading.Visible = true;
                }));
                await Task.Run(() =>
                {
                    ApplyMultipleInsertion(catId, supplierList);
                });
                this.Invoke(new Action(() =>
                {
                    pbLoading.Visible = false;
                    SharedFunctions.MessageBoxShow("Items updated successfully.", MessageType.Info);
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    pbLoading.Visible = false;
                    SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
                }));
            }
        }


        private void ApplyMultipleInsertion(Int32 catId, List<long> supplierId)
        {
            unitOfWork = new UnitOfWork();
            List<Item> updateItems = new List<Item>();
            foreach (var Id in ItemIds)
            {
                Item item = unitOfWork.ItemRspository.getItemWithSupplierById(Id);
                //-----Update Rack
                if (catId > 0)
                    item.Category = unitOfWork.CategoryRepository.GetById(catId);
                //------Update Supplier
                if (supplierId != null && supplierId.Count() > 0)
                {
                    foreach (Supplier s in item.Suppliers.ToList())
                    {
                        item.Suppliers.Remove(s);
                    }
                    item.Suppliers = new List<Supplier>();
                    foreach (long suppId in supplierId)
                    {
                        Supplier objSupplier = unitOfWork.SupplierRepository.GetById(suppId);
                        item.Suppliers.Add(objSupplier);
                    }
                }
                if (item.IsSyncable) //For Update
                {
                    item.IsNew = false;
                    item.IsUpdate = true;
                    item.IsSynced = false;
                }
                else //For Add
                {
                    item.IsSyncable = true;
                    item.IsNew = true;
                    item.IsUpdate = false;
                    item.IsSynced = false;
                }
                if (!item.IsFileItem)
                    updateItems.Add(item);
                AuditLogTask.CreateAuditLog(unitOfWork, item.ItemName, AuditModule.Item, AuditAction.updated);
            }
            unitOfWork.Save();
            //SOT
            var itemBatches = SharedFunctions.ChunkBy(updateItems, SharedVariables.ItemBatch);
            foreach (var item in itemBatches)
            {
                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkItems, ModuleAction.Update, 0);
            }
        }

        private bool IsValidated()
        {
            bool result = false;
            try
            {
                if (lstSuppliers.SelectedItem == null && cmbCategory.SelectedIndex <= 0)
                {
                    SharedFunctions.MessageBoxShow("Select atleast one item.", MessageType.Info);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
                result = true;
            }
            return result;
        }
        #endregion

    }
}