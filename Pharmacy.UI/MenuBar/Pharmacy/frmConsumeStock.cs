using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Infrastructure;
using Pharmacy.Core;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmConsumeStock : Form
    {
        UnitOfWork unitOfWork;
        //List<StockConsumption> StockConsumptionList = new List<StockConsumption>();
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        int EditingRowIndex = -1;

        public frmConsumeStock()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
            LoadItems();
            cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
            cmbSelectItems_SelectedIndexChanged(null, null);
            cmbConsumtionType.SelectedIndex = 0;
            SharedFunctions.SetGridStyle(grdItems);
        }
        private void LoadItems()
        {
            List<Item> items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            cmbSelectItems.DataSource = items;
            cmbSelectItems.ValueMember = "ItemId";
            cmbSelectItems.DisplayMember = "ItemName";
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                StockConsumptionItem objConsumption = new StockConsumptionItem();
                FillObject(objConsumption);
                if (this.EditingRowIndex >= 0)
                {
                    if (!isValidQuantityEntered())
                    {
                        MessageBox.Show("Consumed Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        numQuantity.Focus();
                        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        return;
                    }
                    UpdateRow(objConsumption);
                }
                else
                {
                    if (ItemAlreadyAdded(Convert.ToInt32(cmbSelectItems.SelectedValue)))
                    {
                        MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!isValidQuantityEntered())
                    {
                        MessageBox.Show("Consumed Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        numQuantity.Focus();
                        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        return;
                    }
                    AddItemtoGrid(objConsumption);
                    
                }
                Clear();
                
               
            }
        }
        private void UpdateRow(StockConsumptionItem objConsumption)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objConsumption.BatchId;
            r.Cells["colBatch"].Value = objConsumption.BatchName;
            r.Cells["colQuantity"].Value = objConsumption.Quantity;
            r.Cells["colConsumptiontypeId"].Value = objConsumption.ConsumptionType;
            r.Cells["colConsumptionTypeString"].Value = objConsumption.ConsumptionTypeString;
            r.Cells["colcomment"].Value = objConsumption.Comment;
            grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        private void AddItemtoGrid(StockConsumptionItem objConsumption)
        {
            //dtStockConsumtption.Rows.Add(objConsumption.Quantity, objConsumption.ConsumptionType, objConsumption.Comment, objConsumption.BatchId == 0 ? null : objConsumption.BatchId,objConsumption.ItemId, null, objConsumption.Date);            
            
            grdItems.Rows.Add(0, objConsumption.ItemId, objConsumption.ItemName, objConsumption.BatchId, objConsumption.BatchName, objConsumption.Quantity, objConsumption.ConsumptionType, objConsumption.ConsumptionTypeString, objConsumption.Comment);

        }

        private void FillObject(StockConsumptionItem objConsumption)
        {
            objConsumption.ItemId = Convert.ToInt32(cmbSelectItems.SelectedValue);
            objConsumption.ItemName = cmbSelectItems.GetItemText(cmbSelectItems.SelectedItem);
            objConsumption.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            objConsumption.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objConsumption.Quantity = (int)numQuantity.Value;
            SharedVariables.ConsumptionType enumConType;
            Enum.TryParse(cmbConsumtionType.GetItemText(cmbConsumtionType.SelectedItem), out enumConType);
            objConsumption.ConsumptionType = (int)enumConType;
            objConsumption.ConsumptionTypeString = enumConType.ToString();
            objConsumption.Comment = txtComment.Text.ToString();
            objConsumption.CreateDate = DateTime.Now;
        }

        private void FillObject(DataGridViewRow r, StockConsumptionItem objConsumption)
        {
            objConsumption.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objConsumption.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            objConsumption.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
            objConsumption.ConsumptionType = Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value);
            objConsumption.Comment = r.Cells["colComment"].Value.ToString();
            objConsumption.CreateDate = DateTime.Now;
            objConsumption.UpdateDate = DateTime.Now;
            objConsumption.IsActive = true;
            objConsumption.IsNew = true;
        }

        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (Convert.ToInt32(cmbSelectItems.SelectedValue) <= 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (Convert.ToInt32(cmbSelectBatch.SelectedValue) <= 0)
            {
                ErrSelectBatch.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectBatch.Visible = false;
            }
            if (Convert.ToUInt32(numQuantity.Value) <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
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

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                FillForm(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                cmbSelectItems.Enabled = false;
                if (EditingRowIndex >= 0)
                {
                    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;
                //StockConsumptionList.RemoveAt(e.RowIndex);
                //dtStockConsumtption.Rows.RemoveAt(e.RowIndex);
                //grdItems.Rows.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                //dtStockConsumtption.Rows.RemoveAt(e.RowIndex);
                //StockConsumptionList.RemoveAt(e.RowIndex);
                grdItems.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void FillForm(DataGridViewRow r)
        {
            cmbSelectItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            cmbSelectBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            //cmbConsumtionType.SelectedIndex = cmbConsumtionType.FindStringExact(r.Cells["colConsumptionType"].Value.ToString());
            cmbConsumtionType.SelectedItem = r.Cells["colConsumptionTypeString"].Value.ToString();
            numQuantity.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
            txtComment.Text = r.Cells["colComment"].Value == null ? "" : r.Cells["colComment"].Value.ToString();
        }

        private bool ItemAlreadyAdded(int ItemId, long BatchId, int ConsumptionType)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == BatchId
                    && Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value) == ConsumptionType)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == Convert.ToInt64(cmbSelectBatch.SelectedValue)
                    && Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value) == Convert.ToInt32(cmbConsumtionType.SelectedIndex + 1)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        private void cmbSelectItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectBatch.SelectedIndex = -1;
            if (cmbSelectItems.SelectedIndex >= 0)
            {
                GetItemBatches(Convert.ToInt32(cmbSelectItems.SelectedValue));
            }
        }

        private void GetItemBatches(int ItemId)
        {
            cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
            BatchStockList = unitOfWork.ItemRspository.GetBatchStockByItemId(ItemId);
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            foreach (BatchStockVM i in BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);
                //i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
                if (i.AvailableStock > 0)
                {
                    FinalResult.Add(i);
                }
            }
            cmbSelectBatch.DataSource = FinalResult;
            cmbSelectBatch.ValueMember = "BatchId";
            cmbSelectBatch.DisplayMember = "BatchName";
            cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        }

        //private bool isValidQuantityEntered()
        //{
        //    int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
        //    if (numQuantity.Value > AvailableStock)
        //    {
        //        ToolTip tt = new ToolTip();
        //        numQuantity.Value = 1;
        //        tt.Show("Please Enter Valid Quantity", numQuantity, 800);
        //        numQuantity.Select(0,1);
        //        return false;
        //    }
        //    return true;
        //}
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            isValidQuantityEntered();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            numQuantity.ValueChanged -= numQuantity_ValueChanged;
            cmbSelectItems.Enabled = true;
            this.btnAddItem.Text = "Add Item";
            cmbSelectItems.SelectedIndex = -1;
            cmbSelectBatch.SelectedIndex = -1;
            numQuantity.Value = 1;
            cmbConsumtionType.SelectedIndex = 0;
            txtComment.Text = "";
            numQuantity.ValueChanged += numQuantity_ValueChanged;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    grdItems.Rows.Clear();
                    Clear();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Add Some Items to Grid.", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }                
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    //StockConsumptionItem s = new StockConsumptionItem ();
                    //FillObject(r, s);
                    //s.Item = unitOfWork.ItemRspository.GetById(s.ItemId);
                    //s.Batch = unitOfWork.BatchRepository.GetById(s.BatchId);
                    //unitOfWork.StockConsumptionRepository.Insert(s);
                }
                unitOfWork.Save();
                MessageBox.Show("Stock Consumption Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelectBatch.SelectedIndex >= 0)
                {
                    BatchStockVM r = BatchStockList.Where(b => b.BatchId == Convert.ToInt64(cmbSelectBatch.SelectedValue)).FirstOrDefault();
                    if (r != null)
                    {
                        txtAvlQty.Text = (r.TotalStock + r.AdjustedStock - r.ConsumedStock - r.ExpiredStock).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private bool isValidQuantityEntered()
        {
            int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQuantity.Value > AvailableStock)
            {
                //ToolTip tt = new ToolTip();
                //numQuantity.Value = 1;
                //ErrMessage.Text = "Alert : Consumed Quantity Can't be Greater Than Available Quantity.";
                //ErrMessage.Visible = true;
                ////tt.Show("Please Enter Valid Quantity", numQuantity, 800);
                //numQuantity.Select(0, 1);
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
            }
            return true;
        }

        private void cmbSelectItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }
    }
}