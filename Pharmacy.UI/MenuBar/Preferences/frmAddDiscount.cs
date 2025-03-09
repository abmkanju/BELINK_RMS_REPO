using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.MenuBar.Pharmacy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmAddDiscount : Form
    {
        UnitOfWork unitOfWork;
        List<int> selectedItems = new List<int>(); // list of added items
        List<int> deletedItems = new List<int>(); // list of added items
        public List<int> DeletedItemsList = new List<int>();
        string SelectedDays = "";
        private int FlatdiscountEditId = 0;
        public frmAddDiscount()
        {
            InitializeComponent();
        }
        public frmAddDiscount(int FlatdiscountEditId)
        {
            InitializeComponent();
            this.FlatdiscountEditId = FlatdiscountEditId;
        }

        private void frmAddDiscount_Load(object sender, EventArgs e)
        {
            try
            {

                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnCancel });
                SharedFunctions.SetGridStyle(grdItems);
                cmbDiscType.SelectedIndex = 0;
                if (this.FlatdiscountEditId > 0)
                {
                    loadFlatDiscountData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadFlatDiscountData()
        {
            try
            {
                FlatDiscountVM objDisc = new FlatDiscountVM();
                using (unitOfWork = new UnitOfWork())
                {
                    objDisc = unitOfWork.FlatDiscountRepository.GetDiscountDet_VM(this.FlatdiscountEditId);
                    if (objDisc == null)
                    {
                        throw new Exception("Flat discount object not found");
                    }
                    txtDiscName.Text = objDisc.Name;
                    txtDiscCode.Text = objDisc.Code;
                    dtpStartDate.Value = objDisc.StartDate;
                    dtpEndDate.Value = objDisc.EndDate;
                    cmbDiscType.SelectedIndex = objDisc.DiscountType;
                    numDisc.Value = (decimal)objDisc.Discount;
                    this.SelectedDays = objDisc.SelectedDays;
                    if (objDisc.IsAllTimes)
                    {
                        chkAllTime.Checked = true;
                    }
                    else
                    {
                        chkAllTime.Checked = false;
                        dtpStartTime.Value = objDisc.StartTime;
                        dtpEndTime.Value = objDisc.EndTime;
                    }

                    if (objDisc.IsAllDays)
                    {
                        rbAllDays.Checked = true;
                        rbSelectedDays.Checked = false;
                    }
                    else
                    {
                        rbAllDays.Checked = false;
                        rbSelectedDays.Checked = true;
                    }

                    if (objDisc.IsAllItems)
                    {
                        rbAllItems.Checked = true;
                    }
                    else
                    {
                        rbAllItems.Checked = false;
                        rbSelectedItems.Checked = true;
                        btnSelectItems.Enabled = true;
                    }
                    double discountedVal = 0;
                    foreach (var i in objDisc.DiscountItems)
                    {
                        if (objDisc.DiscountType == 0)
                        {
                            discountedVal = i.Item.RetailPrice - ((objDisc.Discount / 100) * i.Item.RetailPrice);
                        }
                        else
                        {
                            discountedVal = i.Item.RetailPrice - objDisc.Discount;
                        }
                        grdItems.Rows.Add(i.DiscountItemId, i.Item.ItemID, i.Item.ItemName, objDisc.Discount, objDisc.DiscountType == 0 ? "%" : "Value", discountedVal);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAllTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllTime.Checked)
            {
                pnlTime.Enabled = false;
                dtpStartTime.Value = new DateTime(2021, 01, 01, 12, 0, 0);
                dtpEndTime.Value = new DateTime(2021, 01, 01, 23, 59, 0);

            }
            else { pnlTime.Enabled = true; }
        }

        private void btnSelectItems_Click(object sender, EventArgs e)
        {
            //if (numDisc.Value <= 0)
            //{
            //    MessageBox.Show("Please enter discount value before proceeding to next screen", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //frmDiscountItems f = new frmDiscountItems();
            //if (cmbDiscType.SelectedIndex == 0) { f.DiscountType = 0; }
            //else { f.DiscountType = 1; }
            //f.Discount = (double)this.numDisc.Value;
            //f.FormClosed += new FormClosedEventHandler(frmDiacountItems_Closed);
            //f.Show();

            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }



        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            try
            {
                ItemsDataVM d = this.getItemData(ItemId);
                double discountedVal = 0;
                if (this.cmbDiscType.SelectedIndex == 0) // percent
                {
                    discountedVal = d.RetailPrice - ((double)numDisc.Value / 100) * d.RetailPrice;

                }
                else
                {
                    discountedVal = d.RetailPrice - (double)numDisc.Value;
                }
                grdItems.Rows.Add(0, ItemId, d.ItemName, d.RetailPrice, (double)numDisc.Value, this.cmbDiscType.SelectedIndex == 0 ? "%" : "Value", discountedVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("an error occurred while loading item, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private ItemsDataVM getItemData(int itemId)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.ItemRspository.getItemData(itemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void frmDiacountItems_Closed(object sender, FormClosedEventArgs e)
        {
            frmDiscountItems f = (frmDiscountItems)sender;
            this.selectedItems = f.DiscountedItemsList;
            this.deletedItems = f.DeletedItemsList;
        }

        private void cmbDiscType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDiscType.SelectedIndex == 0) { lblDiscVal.Text = "Discount Percent"; }
            else { lblDiscVal.Text = "discount Value"; }
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllItems.Checked)
            {
                btnSelectItems.Enabled = false;
                //LoadAllItems();
            }
            else
            {
                btnSelectItems.Enabled = true;
            }
            //    toggleRowsVisibility(false);
            //}
            //else
            //{
            //    btnSelectItems.Enabled = true;
            //    toggleRowsVisibility(true);
            //}
        }

        private void rbSelectedItems_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedItems.Checked)
            {
                grdItems.Rows.Clear();
            }
        }

        private void LoadAllItems()
        {
            try
            {
                List<Item> items = new List<Item>();
                using (unitOfWork = new UnitOfWork())
                {
                    items = unitOfWork.ItemRspository.GetActiveItems();
                }
                double discountedVal = 0;
                double discVal = (double)numDisc.Value;
                foreach (var i in items)
                {
                    discountedVal = 0;
                    if (this.cmbDiscType.SelectedIndex == 0) // percent
                    {
                        discountedVal = i.RetailPrice - (discVal / 100) * i.RetailPrice;

                    }
                    else
                    {
                        discountedVal = i.RetailPrice - discVal;
                    }
                    grdItems.Rows.Add(0, i.ItemID, i.ItemName, i.RetailPrice, discVal, this.cmbDiscType.SelectedIndex == 0 ? "%" : "Value", discountedVal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("an error occurred while loading item, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toggleRowsVisibility(bool show)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                r.Visible = show;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count <= 0 && !rbAllItems.Checked)
                {
                    MessageBox.Show("Please add some items before saving.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (IsValidInput())
                {
                    if (this.FlatdiscountEditId > 0)
                    {
                        update();
                    }
                    else
                    {
                        insert();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing required action. please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshTodayDiscoutns(UnitOfWork _unitOfWork)
        {
            SharedVariables.TodayDiscounts = _unitOfWork.FlatDiscountRepository.getTodayDiscounts();
        }

        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (this.txtDiscName.Text.Trim().Equals(""))// (cmbSelectItems.SelectedValue == null)
            {
                ErrDiscName.Visible = true;
                ErrFound = true;
                txtDiscName.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrDiscName.Visible = false;
                ErrMessage.Visible = false;
            }

            if (this.txtDiscCode.Text.Trim().Equals(""))// (cmbSelectItems.SelectedValue == null)
            {
                errCode.Visible = true;
                ErrFound = true;
                txtDiscCode.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                errCode.Visible = false;
                ErrMessage.Visible = false;
            }

            if ((double)numDisc.Value <= 0)
            {
                errDisc.Visible = true;
                ErrFound = true;
                numDisc.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                errDisc.Visible = false;
                ErrMessage.Visible = false;
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
        private void insert()
        {
            try
            {
                if (DiscNameExists_new(txtDiscName.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Flat dicount with given name already exists, please try any other name", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DiscCodeExists_new(txtDiscCode.Text.Trim().ToLower()))
                {
                    MessageBox.Show("Flat dicount with given code already exists, please try any other name", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (unitOfWork = new UnitOfWork())
                {

                    FlatDiscount objDisc = new FlatDiscount();
                    objDisc.Name = txtDiscName.Text.Trim();
                    objDisc.Code = txtDiscCode.Text.Trim();
                    objDisc.StartDate = dtpStartDate.Value.Date;
                    objDisc.EndDate = dtpEndDate.Value.Date;
                    objDisc.IsAllTimes = chkAllTime.Checked;
                    objDisc.DiscountType = cmbDiscType.SelectedIndex == 0 ? 0 : 1;
                    objDisc.Discount = (double)numDisc.Value;
                    objDisc.IsAllTimes = chkAllTime.Checked;
                    objDisc.StartTime = dtpStartTime.Value;
                    objDisc.EndTime = dtpEndTime.Value;
                    objDisc.IsAllDays = rbAllDays.Checked;
                    objDisc.SelectedDays = "";
                    objDisc.IsAllItems = rbAllItems.Checked;
                    objDisc.Comment = txtComment.Text;
                    // commmon propertiese
                    objDisc.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId); objDisc.CreatedAt = DateTime.Now; objDisc.UpdatedAt = DateTime.Now; objDisc.IsActive = true; objDisc.IsSynced = false; objDisc.IsNew = true; objDisc.IsUpdate = false;
                    if (!objDisc.IsAllDays) { objDisc.SelectedDays = this.SelectedDays; }
                    //if (!objDisc.IsAllItems)
                    //{
                    objDisc.DiscountItems = new List<DiscountItem>();
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        DiscountItem item = new DiscountItem();
                        item.CreatedAt = DateTime.Now;
                        item.UpdatedAt = DateTime.Now;
                        item.IsActive = true;
                        item.IsSynced = false;
                        item.IsUpdate = false;
                        item.IsNew = true;
                        item.Item = unitOfWork.ItemRspository.GetById(r.Cells["colItemId"].Value);
                        objDisc.DiscountItems.Add(item);
                    }
                    //}
                    unitOfWork.FlatDiscountRepository.Insert(objDisc);
                    unitOfWork.Save();
                    this.Close();
                    MessageBox.Show("Flat discount created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (objDisc.StartDate.Date <= DateTime.Now.Date)
                    {
                        RefreshTodayDiscoutns(unitOfWork);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating flat discount data. please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void update()
        {
            try
            {
                if (DiscCodeExists_Update(txtDiscName.Text.Trim().ToLower(), this.FlatdiscountEditId))
                {
                    MessageBox.Show("Flat dicount with given name already exists, please try any other name", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DiscCodeExists_Update(txtDiscCode.Text.Trim().ToLower(), this.FlatdiscountEditId))
                {
                    MessageBox.Show("Flat dicount with given code already exists, please try any other name", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    FlatDiscount objDisc = unitOfWork.FlatDiscountRepository.GetDiscountDet(this.FlatdiscountEditId);
                    if (objDisc == null)
                    {
                        throw new Exception("Record not found");
                    }
                    objDisc.Name = txtDiscName.Text.Trim();
                    objDisc.Code = txtDiscCode.Text.Trim();
                    objDisc.StartDate = dtpStartDate.Value.Date;
                    objDisc.EndDate = dtpEndDate.Value.Date;
                    objDisc.IsAllTimes = chkAllTime.Checked;
                    objDisc.DiscountType = cmbDiscType.SelectedIndex == 0 ? 0 : 1;
                    objDisc.Discount = (double)numDisc.Value;
                    objDisc.IsAllTimes = chkAllTime.Checked;
                    objDisc.StartTime = dtpStartTime.Value;
                    objDisc.EndTime = dtpEndTime.Value;
                    objDisc.IsAllDays = rbAllDays.Checked;
                    objDisc.IsAllItems = rbAllItems.Checked;
                    objDisc.Comment = txtComment.Text;

                    objDisc.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    objDisc.UpdatedAt = DateTime.Now;
                    if (objDisc.IsSynced)
                    {
                        objDisc.IsNew = false;
                        objDisc.IsUpdate = true;
                        objDisc.IsSynced = false;
                    }
                    if (!objDisc.IsAllDays) { objDisc.SelectedDays = this.SelectedDays; }
                    else { objDisc.SelectedDays = ""; } // else clear in case of update
                    if (objDisc.IsAllItems)
                    {
                        foreach (var di in objDisc.DiscountItems)
                        {
                            di.IsActive = false;
                            di.IsUpdate = true;
                            di.UpdatedAt = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(di).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    if (!objDisc.IsAllItems)
                    {
                        bool found = false;
                        int discItemId = 0;
                        foreach (DataGridViewRow r in this.grdItems.Rows)
                        {
                            found = false;
                            foreach (var di in objDisc.DiscountItems)
                            {
                                discItemId = Convert.ToInt32(r.Cells["colDiscountItemId"].Value);
                                if (discItemId != 0 && di.DiscountItemId == discItemId)
                                {
                                    found = true;
                                    di.UpdatedAt = DateTime.Now;
                                    di.IsUpdate = true;
                                    unitOfWork.GetDbContext().Entry(di).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                DiscountItem item = new DiscountItem();
                                item.CreatedAt = DateTime.Now;
                                item.UpdatedAt = DateTime.Now;
                                item.IsActive = true;
                                item.IsSynced = false;
                                item.IsUpdate = false;
                                item.IsNew = true;
                                item.Item = unitOfWork.ItemRspository.GetById(r.Cells["colItemId"].Value);
                                objDisc.DiscountItems.Add(item);
                            }
                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("Please add some items to this discount type before saving.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    unitOfWork.FlatDiscountRepository.Update(objDisc);
                    unitOfWork.Save();
                    this.Close();
                    MessageBox.Show("Flat discount updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (objDisc.StartDate.Date <= DateTime.Now.Date)
                    {
                        RefreshTodayDiscoutns(unitOfWork);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting flat discount data. please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSelectDays_Click(object sender, EventArgs e)
        {

            frmSelectDays f = new frmSelectDays(this.SelectedDays);
            f.StartPosition = FormStartPosition.Manual;
            int x = btnSelectDays.Location.X;
            int y = btnSelectDays.Location.Y + btnSelectDays.Height;

            int x2 = pnlDays.Location.X;
            int y2 = pnlDays.Location.Y;
            f.Location = new Point(x + x2 + SharedVariables.SideBarWidth, y + y2 + SharedVariables.TopBarHeight + SharedVariables.TopMenueBarHeight);
            f.FormClosed += new FormClosedEventHandler(frmSelectDays_Closed);
            f.ShowDialog();
        }

        private void frmSelectDays_Closed(object sender, FormClosedEventArgs e)
        {
            frmSelectDays f = (frmSelectDays)sender;
            this.SelectedDays = f.SelectedDays;
        }

        private void rbAllDays_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllDays.Checked) { btnSelectDays.Enabled = false; }
            else { btnSelectDays.Enabled = true; }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                {
                    int DiscItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colDiscountItemId"].Value);
                    if (DiscItemId > 0)
                    {
                        DialogResult r = MessageBox.Show("Are you sure you want to remove existing item?", "Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (r == System.Windows.Forms.DialogResult.Yes)
                        {
                            DeletedItemsList.Add(DiscItemId);
                            grdItems.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operation failed, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numDisc_ValueChanged(object sender, EventArgs e)
        {
            double retailPrice = 0;
            double discountedValue = 0;
            double discount = (double)numDisc.Value;
            int DisType = cmbDiscType.SelectedIndex;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                retailPrice = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
                if (DisType == 0)
                {
                    discountedValue = retailPrice - ((discount / 100) * retailPrice);
                }
                else
                {
                    discountedValue = retailPrice - discount;
                }
                r.Cells["colNewValue"].Value = discountedValue;
                r.Cells["colDiscount"].Value = discount;
            }
        }

        private bool DiscNameExists_new(string name)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.FlatDiscountRepository.DiscNameExists_new(name);
            }
        }
        private bool DiscCodeExists_new(string code)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.FlatDiscountRepository.DiscCodeExists_new(code);
            }
        }
        private bool DiscNameExists_Update(string name, int DiscountId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.FlatDiscountRepository.DiscNameExists_Update(name, DiscountId);
            }
        }
        private bool DiscCodeExists_Update(string code, int DiscountId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.FlatDiscountRepository.DiscCodeExists_Update(code, DiscountId);
            }
        }
    }
}