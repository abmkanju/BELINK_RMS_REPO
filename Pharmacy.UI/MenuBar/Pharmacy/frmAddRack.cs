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
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    //rf!@
    public partial class frmAddRack : Form
    {
        UnitOfWork unitOfWork;
        private int RackId = 0;
        int SelectedItemId = 0;
        string SelectedItemName = "";

        #region LoadRack
        public frmAddRack()
        {
            InitializeComponent();
            this.uC_SearchRackItems.OnItemSelectionChanged += uC_SearchRackItems_OnItemSelectionChanged;
            this.uC_SearchRackItems.BringToFront();
        }
        public frmAddRack(int RackId)
        {
            InitializeComponent();
            this.RackId = RackId;
            this.uC_SearchRackItems.OnItemSelectionChanged += uC_SearchRackItems_OnItemSelectionChanged;
            this.uC_SearchRackItems.BringToFront();
        }
        private void frmAddRack_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetGridStyle(this.grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave });
            LoadDetails();
        }
        private void LoadDetails()
        {
            try
            {
                if (this.RackId > 0)
                {
                    LoadEditRack();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void LoadEditRack()
        {
            RackVM r = new RackVM();
            using (unitOfWork = new UnitOfWork())
            {
                r = unitOfWork.RackRepository.GetRackVMDetailsById(this.RackId);
            }
            if (r != null)
            {
                this.txtRackName.Text = r.Name;
                if (r.RackItems != null && r.RackItems.Count() > 0)
                {
                    foreach (var item in r.RackItems)
                    {
                        
                        this.SelectedItemId = item.ItemId;
                        this.SelectedItemName = item.ItemName;
                        AddItemInGrid();
                    }
                }
            }
        }
        #endregion

        #region ItemSearch
        private void uC_SearchRackItems_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchRackItems.SelectedItemId;
            this.SelectedItemName = this.uC_SearchRackItems.SelectedItemName;
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.RackRepository.CheckItemInRack(SelectedItemId, this.RackId))
                {
                    SharedFunctions.MessageBoxShow("Item already exists in other rack", MessageType.Warning);
                    return;
                }
            }
            AddItemInGrid();
        }
        private void uC_SearchRackItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddItemInGrid();
            }
        }
        private void AddItemInGrid()
        {
            if (this.SelectedItemId > 0)
            {
                if (!IsItemAlreadyAdded(this.SelectedItemId))
                {
                    grdItems.Rows.Add(this.SelectedItemId, this.SelectedItemName);
                    uC_SearchRackItems.SetText = "";
                }
            }

            this.uC_SearchRackItems.SetFocus();
        }

        private bool IsItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (SharedFunctions.StringToInt(r.Cells["colItemId"].Value) == ItemId)
                {
                    SharedFunctions.MessageBoxShow("Selected Item Already Exists", MessageType.Info);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region DatagridEvent
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                {
                    if (this.RackId > 0) //In Edit Case
                    {
                        int ItemId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                        if (ItemId <= 0)
                        {
                            SharedFunctions.MessageBoxShow("Unable to get item details", MessageType.Success);
                            return;
                        }
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            using (unitOfWork = new UnitOfWork())
                            {
                                RackItem rackItem = unitOfWork.RackRepository.GetRackItemById(RackId, ItemId);
                                if (rackItem != null)
                                {
                                    unitOfWork.RackRepository.SetRackItemInactive(RackId, ItemId);
                                    //SOT
                                }
                                //grdItems.Rows.RemoveAt(e.RowIndex);
                                grdItems.Rows[e.RowIndex].Visible = false;
                            }
                        }
                    }
                    else //In Add Case
                    {
                        grdItems.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region SaveRackDetails
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateRack())
                    return;

                if (this.RackId > 0)
                {
                    UpdateData();
                }
                else
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void Save()
        {
            String message = "The following Item(s) in Rack  Items have no stock , Add Stock of These Item(s) first ... \n\n";
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Rack r = new Rack();
                    r.Name = txtRackName.Text.Trim();
                    r.IsNew = true;
                    r.IsActive = true;
                    r.CreatedAt = DateTime.Now;
                    r.UpdatedAt = DateTime.Now;
                    r.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    r.RackItem = new List<RackItem>();
                    foreach (DataGridViewRow row in grdItems.Rows)
                    {
                        RackItem rackItem = new RackItem();
                        rackItem.Item = unitOfWork.ItemRspository.GetById(SharedFunctions.StringToLong(row.Cells["colItemId"].Value));
                        rackItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (row.Visible)
                        {
                            rackItem.IsActive = true;
                        }
                        else
                        {
                            rackItem.IsActive = false;
                            rackItem.DeletedAt = DateTime.Now;
                        }
                        rackItem.CreatedAt = DateTime.Now;
                        rackItem.UpdatedAt = DateTime.Now;
                        rackItem.Rack = r;
                        if (!rackItem.Item.IsSyncable) 
                        {
                            message += rackItem.Item.ItemName.ToString() + "\n";
                            continue;
                        }
                        r.RackItem.Add(rackItem);
                       
                    }
                    if (message != "The following Item(s) in Rack  Items have no stock , Add Stock of These Item(s) first ... \n\n")
                    {
                        MessageBox.Show(message , "Rack Item Syncing Error" , MessageBoxButtons.OK , MessageBoxIcon.Warning);
                        return ;
                    }
                    unitOfWork.RackRepository.Insert(r);
                    AuditLogTask.CreateAuditLog(unitOfWork, r.Name, AuditModule.Rack, AuditAction.created);
                    unitOfWork.Save();
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, r, ModuleName.Rack, ModuleAction.Create, r.RackId);
                    MessageBox.Show("Rack data inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateData()
        {
            try
            {
                String message = "The following Item(s) in Rack  Items have no stock , Add Stock of These Item(s) first ... \n\n";
                using (unitOfWork = new UnitOfWork())
                {
                    Rack r = unitOfWork.RackRepository.GetRackDetailsById(this.RackId);
                    r.RackItem = r.RackItem.Where(x => x.IsActive).ToList();
                    r.Name = txtRackName.Text.Trim();
                    if (r.IsSynced)
                    {
                        r.IsNew = false;
                        r.IsUpdate = true;
                        r.IsSynced = false;
                    }
                    r.UpdatedAt = DateTime.Now;
                    r.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    foreach (DataGridViewRow row in grdItems.Rows)
                    {
                        RackItem rackItem = new RackItem();
                        int ItemID = SharedFunctions.StringToInt(row.Cells["colItemId"].Value);
                        if (!r.RackItem.Any(cus => cus.Item.ItemID == ItemID))
                        {
                            rackItem.Item = unitOfWork.ItemRspository.GetById(SharedFunctions.StringToLong(row.Cells["colItemId"].Value));
                            rackItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            if (row.Visible)
                            {
                                rackItem.IsActive = true;
                            }
                            else
                            {
                                rackItem.IsActive = false;
                                rackItem.DeletedAt = DateTime.Now;
                            }
                            rackItem.CreatedAt = DateTime.Now;
                            rackItem.UpdatedAt = DateTime.Now;
                            rackItem.Rack = r;
                            if (!rackItem.Item.IsSyncable && row.Visible)
                            {
                                message += rackItem.Item.ItemName.ToString() + "\n";
                                continue;
                            }
                            if (row.Visible || rackItem.Item.IsSyncable)
                            {
                                r.RackItem.Add(rackItem);
                            }
                        }
                    }
                    if (message != "The following Item(s) in Rack  Items have no stock , Add Stock of These Item(s) first ... \n\n")
                    {
                        MessageBox.Show(message, "Rack Item Syncing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    AuditLogTask.CreateAuditLog(unitOfWork, r.Name, AuditModule.Rack, AuditAction.updated);
                    unitOfWork.RackRepository.Update(r);
                    unitOfWork.Save();
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, r, ModuleName.Rack, ModuleAction.Update, r.RackId);
                    MessageBox.Show("Rack data Updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ValidateRack
        private bool ValidateRack()
        {
            bool IsValidate = false;
            if (txtRackName.Text.Trim().Equals(""))
            {
                return IsValidate;
            }
            if (this.RackId > 0)
            {
                if (RackExists_Update(this.txtRackName.Text.Trim().ToLower(), this.RackId))
                {
                    MessageBox.Show("Rack with Given Name Already Exists, Please Specify Another Name", "Rack Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return IsValidate;
                }
            }
            else
            {
                if (RackExists_new(txtRackName.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Rack with Given Name Already Exists, Please Specify Another Name", "Rack Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return IsValidate;
                }
            }
            return IsValidate = true;
        }
        private bool RackExists_new(string name)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RackRepository.RackExists_New(name);
            }
        }
        private bool RackExists_Update(string name, int TemplateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RackRepository.RackExists_Update(name, this.RackId);
            }
        }
        #endregion

        private void uC_SearchRackItems_Load(object sender, EventArgs e)
        {

        }



    }
}
