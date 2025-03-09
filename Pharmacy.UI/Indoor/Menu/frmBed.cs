using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
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
    public partial class frmBed : Form
    {
        private UnitOfWork unitOfWork;
        //List<PurchaseOrderItem> PurchaseOrderItemsList = new List<PurchaseOrderItem>();
        PurchaseOrder LoadedPurchaseOrder;
        private bool IsOldOrderLoaded = false;
        long PurchaseOrderId = 0;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        public frmBed()
        {
            InitializeComponent();
        }
        public frmBed(long PurchaseOrderId)
        {
            InitializeComponent();
            this.PurchaseOrderId = PurchaseOrderId;
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddBed.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.I))
            {
                cmbWard.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdItems.Focus();
                if (grdItems.Rows.Count >= 1)
                {
                    grdItems.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClearForm.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }


            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
                return true;
            }
            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.R))
            {
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void LoadWards()
        {
            List<Ward> wards = new List<Ward>();
            using (unitOfWork = new UnitOfWork())
            {
                wards = unitOfWork.WardRepository.GetAllActiveWards();
            }
            Ward w = new Ward();
            w.WardId = 0;
            w.Name = "Select Ward";
            wards.Insert(0, w);
            cmbWard.DataSource = wards;
            cmbWard.ValueMember = "WardId";
            cmbWard.DisplayMember = "Name";
        }
        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddBed, btnClearForm });
                LoadWards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count > 0)
                {
                    DialogResult rs = MessageBox.Show("There are Some Un-Saved Items In Grid, are you Sure you Want to clear", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                cmbWard.SelectedIndex = 0;
                grdItems.Rows.Clear();
                IsOldOrderLoaded = false;
                this.PurchaseOrderId = 0;
                LoadedPurchaseOrder = null;
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear()
        {
            //cmbWard.SelectedIndex = 0;
            txtRoomNo.Text = "";
            txtCharges.Text = "";
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            btnAddBed.Text = "Add Bed";
            EditingRowIndex = -1;
            cmbWard.Focus();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    Bed objWard = new Bed();
                    FillObject(objWard);
                    if (WardAlreadyHasRoomNo(objWard))
                    {
                        ErrMessage.Text = "Alert : Bed no is already exist in same Ward.";
                        ErrMessage.Visible = true;
                        return;
                    }

                    if (EditingRowIndex >= 0)
                    {
                        UpdateRow(objWard);
                    }
                    else
                    {
                        AddItemToGrid(objWard);
                    }
                    Clear();
                }
            }
            catch (Exception ex)
            {
                if (SharedVariables.ShowActualExceptionMessages)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool WardAlreadyHasRoomNo(Bed obj)
        {
            bool Exists = false;
            foreach(DataGridViewRow r in grdItems.Rows)
            {
                if(Convert.ToInt64(r.Cells["colWardId"].Value) == obj.WardId && r.Cells["colRoomNo"].Value.ToString().ToLower().Equals(obj.RoomNo))
                {
                    Exists = true;
                }
            }
            using(unitOfWork = new UnitOfWork ())
            {
                if (unitOfWork.WardRepository.HasRoomNo(obj.WardId, obj.RoomNo) > 0)
                {
                    Exists = true;
                }
            }
            return Exists ? true : false;
        } 
        private void UpdateRow(Bed obj)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colWardId"].Value = obj.WardId;
            r.Cells["colWard"].Value = cmbWard.GetItemText(cmbWard.SelectedItem);
            r.Cells["colRoomNo"].Value = obj.RoomNo;
            r.Cells["colChargesPerDay"].Value = obj.ChargesPerDay;
            r.DefaultCellStyle.BackColor = Color.White;
        }
        private void AddItemToGrid(Bed obj)
        {
            //if (ItemAlreadyExists(objPItem.ItemID)) return;
            object[] objPurchaseItem = { 0, obj.WardId, obj.WardString, obj.RoomNo, obj.ChargesPerDay };
            grdItems.Rows.Add(objPurchaseItem);
            //PurchaseOrderItemsList.Add(objPItem);
        }
        private void AddItemToGrid(PurchaseOrderItem objItem, decimal PurchaseOrderNo)
        {
            PurchaseOrderItem objPItem = new PurchaseOrderItem();
            //FillObject(objPItem, objItem, PurchaseOrderNo);
            if (ItemAlreadyExists(objItem.Item.ItemID)) return;
            object[] objPurchaseItem = { objItem.PurchaseOrderItemId, objItem.Item.ItemID, objItem.Item.ItemName, objItem.Quantity, objItem.UnitCost, objItem.TotalAmount };
            grdItems.Rows.Add(objPurchaseItem);
            ///PurchaseOrderItemsList.Add(objPItem);
        }
        private bool ItemAlreadyExists(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    MessageBox.Show("Selected Item is Already Added, Please Laod that one", "Item Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[r.Index].Selected = true;
                    return true;
                }
            }
            return false;
        }
        private void FillObject(Bed obj)
        {
            obj.WardId = Convert.ToInt64(cmbWard.SelectedValue);
            obj.RoomNo = txtRoomNo.Text.Trim();
            obj.WardString = cmbWard.GetItemText(cmbWard.SelectedItem);
            obj.ChargesPerDay = double.Parse(txtCharges.Text);
        }
        private void FillObject(PurchaseOrderItem objPItem, PurchaseOrderItem objLoadedItem, decimal PurchaseOrderNo)
        {
            objPItem.PurchaseOrderItemId = objLoadedItem.PurchaseOrderItemId;
            objPItem.PurchaseOrderNo = PurchaseOrderNo;
            objPItem.Item = new Item();
            objPItem.ItemID = objLoadedItem.Item.ItemID;
            objPItem.ItemName = objLoadedItem.Item.ItemName;
            objPItem.Quantity = objLoadedItem.Quantity;
            objPItem.UnitCost = objLoadedItem.UnitCost;
            objPItem.TotalAmount = objLoadedItem.TotalAmount;
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (cmbWard.SelectedIndex <= 0)
            {
                errWard.Visible = true;
                ErrFound = true;
                cmbWard.Focus();
            }
            else
            {
                errWard.Visible = false;
            }
            if (txtRoomNo.Text.Trim().Equals(""))
            {
                errRoomNo.Visible = true;
                ErrFound = true;
            }
            else
            {
                errRoomNo.Visible = false;
            }

            if (txtCharges.Text.Trim().Equals(""))
            {
                errCharges.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCharges.Visible = false;
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
        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some Purchase Items", "Empty Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveBed();
        }
        private void SaveBed()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        Bed objBed = new Bed();
                        objBed.Ward = unitOfWork.WardRepository.GetById(r.Cells["colWardId"].Value);
                        objBed.RoomNo = r.Cells["colRoomNo"].Value.ToString();
                        objBed.ChargesPerDay = double.Parse(r.Cells["colChargesPerDay"].Value.ToString());
                        objBed.IsActive = true;
                        objBed.IsNew = true;
                        objBed.IsAvailable = true;
                        objBed.CreateDate = DateTime.Now;
                        objBed.UpdateDate = DateTime.Now;
                        unitOfWork.BedRepository.Insert(objBed);
                    }
                    unitOfWork.Save();
                    MessageBox.Show("Bed details saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            DataGridViewRowCancelEventArgs ev = new DataGridViewRowCancelEventArgs(grdItems.Rows[e.RowIndex]);
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {

                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                grdItems.Rows.RemoveAt(e.RowIndex);
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                FillItemInfo(grdItems.Rows[e.RowIndex]);
                EditingRowIndex = e.RowIndex;
                this.btnAddBed.Text = "Update Bed";
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                return;
            }
        }
        private void FillItemInfo(DataGridViewRow rItem)
        {
            cmbWard.SelectedValue = rItem.Cells["colWardId"].Value;
            txtRoomNo.Text = rItem.Cells["colRoomNo"].Value.ToString();
            txtCharges.Text = rItem.Cells["colChargesPerDay"].Value.ToString();
        }


        private double GetGrandTotals()
        {
            double TotalAmount = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Index != EditingRowIndex)
                {
                    TotalAmount += Convert.ToDouble(r.Cells["coltotalAmount"].Value);
                }
            }
            return TotalAmount;
        }

        private void cmbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddBed.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddBed.PerformClick();
            }
        }

        private void numUnitCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddBed.PerformClick();
            }
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            //ShortCutDialogs.frmPurchaseOrderShortCuts f = new ShortCutDialogs.frmPurchaseOrderShortCuts();
            //f.ShowDialog();
        }

        private void txtCharges_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtCharges_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnAddBed.PerformClick();
            }
        }
    }
}