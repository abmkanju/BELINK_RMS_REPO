using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Reports;
using PagedList;
using Pharmacy.Core;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddTemplate : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        UnitOfWork unitOfWork;
        private int SelectedItemId = 0;
        public string SelectedItemName = "";
        private int TemplateId = 0;
        private List<int> deleteTemplateItemIds = new List<int>();       // it only contains items which were loaded with template but not the ones which were newly added and remved.
        int EditingRowIndex = -1;

        public frmAddTemplate()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmAddTemplate(int templateId)
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.TemplateId = templateId;
        }
        private void Clear()
        {
            uC_SearchItems1.Enabled = true;
            btnAdd.Text = "Add";
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = "";
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;

            EditingRowIndex = -1;
            this.SelectedItemId = 0;
            this.numQuantity.Value = 1;
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            if (this.SelectedItemId <= 0)
            {
                btnClear.PerformClick();
                return;
            }
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAdd, btnClear });
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll });
            SharedFunctions.SetGridStyle(grdItems);
            //SharedFunctions.UpdateCommonStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetGridStyle(grdItems);
            LoadTemplate(this.TemplateId);
        }
        private void LoadTemplate(int p_templateId)
        {
            try
            {
                TemplateVM obj = new TemplateVM();
                using (unitOfWork = new UnitOfWork())
                {
                    obj = unitOfWork.TemplateRepository.getTemplateById(p_templateId);
                }
                if (obj != null)
                {
                    this.txtTemplate.Text = obj.Name;
                    foreach (var i in obj.TemplateItems)
                    {
                        grdItems.Rows.Add(i.TemplateItemId, i.Item.ItemID, i.Item.ItemName, i.Quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while loading template data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        int quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                        string item = grdItems.Rows[e.RowIndex].Cells["colItem"].Value.ToString();
                        int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                        numQuantity.Value = quantity;
                        uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                        uC_SearchItems1.SetText = item;
                        uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                        this.SelectedItemId = itemId;
                        EditingRowIndex = e.RowIndex;
                        uC_SearchItems1.Enabled = false;
                        btnAdd.Text = "Update";
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}

                        int templateItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colTemplateItemId"].Value);
                        if (templateItemId > 0)
                        {
                            DialogResult rs = MessageBox.Show("Are you sure you want to remove this template item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rs == System.Windows.Forms.DialogResult.OK)
                            {
                                deleteTemplateItemIds.Add(templateItemId);                                
                            }
                            else
                            {
                                return;
                            }
                        }
                        if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                        else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                        grdItems.Rows.RemoveAt(e.RowIndex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void frmAllItems_Activated(object sender, EventArgs e)
        {
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (EditingRowIndex >= 0)
                    {
                        grdItems.Rows[EditingRowIndex].Cells["colQuantity"].Value = Convert.ToInt32(numQuantity.Value);
                    }
                    else
                    {
                        if (isItemAlreadtAdded(this.SelectedItemId))
                        {
                            MessageBox.Show("Selected item already been added, please try another item.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        grdItems.Rows.Add(0, this.SelectedItemId, this.SelectedItemName, numQuantity.Value);
                    }
                    btnClear.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding item to template, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool isItemAlreadtAdded(int p_itemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                int itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                if (p_itemId == itemId)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (this.SelectedItemId <= 0)// (cmbSelectItems.SelectedValue == null)
            {
                errItem.Visible = true;
                ErrFound = true;
                //cmbSelectItems.Focus();
                this.uC_SearchItems1.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                errTemplate.Visible = false;
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.TemplateId > 0)
            {
                if (!TempExists_Update(txtTemplate.Text.Trim().ToLower(), this.TemplateId))
                {
                    update();
                }
                else
                {
                    MessageBox.Show("Template with Given Name Already Exists, Please Specify Another Name", "Manufacturer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (!TempExists_new(txtTemplate.Text.Trim().ToLower()))
                {
                    save();
                }
                else
                {
                    MessageBox.Show("Template with Given Name Already Exists, Please Specify Another Name", "Manufacturer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void save()
        {
            try
            {
                if (txtTemplate.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Please enter template name before proceeding.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Please add some items to template before proceeding.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    Template objTemp = new Template();
                    objTemp.Name = txtTemplate.Text;
                    objTemp.CreatedAt = DateTime.Now;
                    objTemp.UpdatedAt = DateTime.Now;
                    objTemp.IsActive = true;
                    objTemp.IsNew = true;
                    objTemp.IsUpdate = false;
                    objTemp.IsSynced = false;
                    objTemp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    objTemp.TemplateItems = new List<TemplateItem>();
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        TemplateItem tempItem = new TemplateItem();
                        int itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                        tempItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                        tempItem.Item = unitOfWork.ItemRspository.GetById(itemId);
                        tempItem.CreatedAt = DateTime.Now;
                        tempItem.UpdatedAt = DateTime.Now;
                        tempItem.IsActive = true;
                        objTemp.TemplateItems.Add(tempItem);
                    }
                    unitOfWork.TemplateRepository.Insert(objTemp);
                    unitOfWork.Save();
                    this.Close();
                }
                MessageBox.Show("Template data inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while inserting template data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void update()
        {
            try
            {

                List<TemplateItem> removedItems = new List<TemplateItem>();
                List<TemplateItem> newItems = new List<TemplateItem>();
                if (txtTemplate.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Please enter template name before proceeding.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Please add some items to template before proceeding.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Template objTemp = new Template();

                if (objTemp == null)
                {
                    MessageBox.Show("An error occurred while updating tempalate data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (unitOfWork = new UnitOfWork())
                {
                    objTemp = unitOfWork.TemplateRepository.getTemplateWithDetailsById(this.TemplateId);
                    objTemp.Name = txtTemplate.Text.Trim();
                    if (objTemp.IsSynced)
                    {
                        objTemp.IsNew = false;
                        objTemp.IsUpdate = true;
                        objTemp.IsSynced = false;
                    }
                    objTemp.UpdatedAt = DateTime.Now;
                    objTemp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);

                    foreach (var i in deleteTemplateItemIds)
                    {
                        TemplateItem tempItem = objTemp.TemplateItems.Where(ti => ti.TemplateItemId == i).FirstOrDefault();
                        if (tempItem != null)
                        {
                            tempItem.DeletedAt = DateTime.Now;
                            tempItem.IsActive = false;
                            unitOfWork.GetDbContext().Entry(tempItem).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        TemplateItem tempItem = new TemplateItem();
                        int templateItemId = Convert.ToInt32(r.Cells["colTemplateItemId"].Value);
                        int itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                        tempItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                        tempItem.Item = unitOfWork.ItemRspository.GetById(itemId);
                        bool tempItemFound = false;
                        foreach (var i in objTemp.TemplateItems)
                        {
                            if (i.TemplateItemId == templateItemId)
                            {
                                i.Quantity = tempItem.Quantity;
                                i.IsUpdate = true;
                                i.UpdatedAt = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                tempItemFound = true;
                            }
                        }
                        if (!tempItemFound)
                        {
                            tempItem.IsNew = true;
                            tempItem.IsActive = true;
                            tempItem.CreatedAt = DateTime.Now;
                            tempItem.UpdatedAt = DateTime.Now;
                            objTemp.TemplateItems.Add(tempItem);
                        }
                    }
                    unitOfWork.TemplateRepository.Update(objTemp);
                    unitOfWork.Save();
                    this.Close();
                    MessageBox.Show("Template updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while update data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool TempExists_new(string name)
        {            
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.TemplateRepository.TemplateExists_New(name);
            }
        }
        private bool TempExists_Update(string name, int TemplateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.TemplateRepository.TemplateExists_Update(name, TemplateId);
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult res = MessageBox.Show("Please make sure before proceed.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            ClearAll();
        }
        private void ClearAll()
        {
            Clear();
            txtTemplate.Text = "";
        }
    }
}