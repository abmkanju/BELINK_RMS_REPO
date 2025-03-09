using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockAdjustment : Form
    {
        UnitOfWork unitOfWork;
        private long AdjustmentId = 0;
        private long itemId = 0;
        List<BatchStockVM> BatchStockList;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        int SelectedItemId = 0;
        string SelectedItemName = "";
        public frmStockAdjustment()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmStockAdjustment(long AdjustmentId)
        {
            InitializeComponent();
            this.AdjustmentId = AdjustmentId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            if (this.SelectedItemId > 0)
            {
                LoadItemData(this.SelectedItemId);
                GetItemBacthes(this.SelectedItemId);
            }
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        private void LoadItemData(int ItemId)
        {
            try
            {
                cmbUnit.SelectedValueChanged -= cmbUnit_SelectedValueChanged;
                using (unitOfWork = new UnitOfWork())
                {
                    Item item = unitOfWork.ItemRspository.GetById(ItemId);
                    cmbUnit.Items.Add(item.Unit);
                    cmbUnit.Items.Add("Units");
                    cmbUnit.SelectedIndex = 0;
                    numConvUnit.Value = item.ConversionUnit;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cmbUnit.SelectedValueChanged += cmbUnit_SelectedValueChanged;
            }
        }
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.AdjustmentId > 0)
                {
                    LoadAdjustment();
                    grdItems.Columns["colEdit"].Visible = false;
                }
                cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
                LoadItems();
                cmbItems.SelectedIndex = -1;
                cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
                SharedFunctions.SetGridStyle(grdItems);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.I))
            {
                cmbItems.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
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
        private void LoadAdjustment()
        {
            Adjustment a = new Adjustment();
            using (unitOfWork = new UnitOfWork())
            {
                a = unitOfWork.AdjustmentRepository.GetAdjustmentById(this.AdjustmentId);
            }
            int count = 0;
            foreach (AdjustmentItem i in a.AdjustmentItems)
            {
                if (i.IsActive)
                {
                    i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                    //double qty = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    //double costprice = i.Unit == 0 ? i.CostPrice * i.Item.ConversionUnit : i.CostPrice;
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    i.CostPrice = i.Unit == 0 ? i.CostPrice * i.Item.ConversionUnit : i.CostPrice;
                    i.AdjustmentValue = i.Quantity * i.CostPrice;
                    grdItems.Rows.Add(0, i.AdjustmentItemId, i.Item.ItemID, i.Item.ItemName, i.Batch.BatchId, i.Batch.BatchName, i.CostPrice, i.UnitString, i.Item.ConversionUnit, i.Quantity, i.AdjustmentValue, i.Reason);
                    LoadSerial();
                    grdItems.Rows[count].DefaultCellStyle.BackColor = Color.LightGray;
                    count++;
                }
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                cmbItems.Enabled = false;
                lblLoadingItems.Visible = true;
            });

            //if (SharedVariables.POSItems == null)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            }
            //}
        }

        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            cmbItems.DataSource = Items;
            cmbItems.ValueMember = "ItemId";
            cmbItems.DisplayMember = "ItemName";
            cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            cmbItems.SelectedIndex = -1;
            cmbItems.Enabled = true;
            lblLoadingItems.Visible = false;
        }
        private void LoadItems()
        {
            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
            //Thread worker = new Thread(() => { LoadItems_Async(); });
            //worker.Name = "ItemsLoader";
            //worker.IsBackground = true;
            //worker.Start(); 
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long AdjItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colAdjustmentItemId"].Value);
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {

                if (AdjItemId > 0)
                {
                    DialogResult rs = MessageBox.Show("Are you Sure You Want to Delete This Adjustment?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == DialogResult.OK)
                    {
                        if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                        else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                        grdItems.Rows.RemoveAt(e.RowIndex);
                        //grdItems.Rows[e.RowIndex].Visible = false;
                    }
                }
                else
                {
                    if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                    else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                //StockItemsList.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {

                if (AdjItemId > 0)
                {
                    MessageBox.Show("You Can't Edit This Item", "Inavalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Clear();
                FillForm(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                cmbItems.Enabled = false;
                if (EditingRowIndex >= 0)
                {
                    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;

                //Edit(grdItems.Rows[e.RowIndex]);
                //grdItems.Rows.RemoveAt(e.RowIndex);
                // StockItemsList.RemoveAt(e.RowIndex);
            }
        }

        private void GetItemBacthes(int itemId)
        {
            if (itemId > 0)
            {
                cmbBatch.DataSource = null;
                cmbBatch.SelectedIndex = -1;

                cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
                GetItemBatches(itemId);
                cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
                if (cmbBatch.Items.Count > 0)
                {
                    cmbBatch.SelectedIndex = 0;
                    cmbSelectBatch_SelectedIndexChanged(null, null);
                }
            }

        }
        private void FillForm(DataGridViewRow r)
        {
            //cmbItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemName = r.Cells["colItem"].Value.ToString();
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = this.SelectedItemName;
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            LoadItemData(this.SelectedItemId);
            GetItemBacthes(this.SelectedItemId);
            cmbBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            //cmbConsumtionType.SelectedIndex = cmbConsumtionType.FindStringExact(r.Cells["colConsumptionType"].Value.ToString());
            txtReason.Text = r.Cells["colReason"].Value.ToString();
            numQty.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
        }
        public void Edit(DataGridViewRow r)
        {
            cmbItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            cmbBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            numQty.Value = Convert.ToDecimal(r.Cells["colQuantity"].Value);
            txtReason.Text = r.Cells["colReason"].Value.ToString();
        }


        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
            }
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    AdjustmentItem objAdjItem = new AdjustmentItem();
                    FillObject(objAdjItem);
                    if (this.EditingRowIndex >= 0)
                    {
                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Adjustment Can't be Less Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            numQty.Focus();
                            numQty.Select(0, numQty.Value.ToString().Length);
                            return;
                        }
                        UpdateRow(objAdjItem);
                    }
                    else
                    {
                        if (IsItemAlreadyAdded())
                        {
                            return;
                        }

                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Adjustment Can't be Less Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            numQty.Focus();
                            numQty.Select(0, numQty.Value.ToString().Length);
                            return;
                        }
                        grdItems.Rows.Add(0, 0, objAdjItem.ItemId, objAdjItem.ItemName, objAdjItem.BatchId, objAdjItem.BatchName, objAdjItem.CostPrice, objAdjItem.UnitString, objAdjItem.ConversionUnit, objAdjItem.Quantity, objAdjItem.AdjustmentValue, objAdjItem.Reason);
                        LoadSerial();
                    }
                    Clear();
                    int index = grdItems.Rows[grdItems.Rows.Count - 1].Index;
                    grdItems.FirstDisplayedScrollingRowIndex = index;
                    grdItems.ClearSelection();
                    grdItems.Rows[index].Selected = true;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally { this.uC_SearchItems1.SetFocus(); }
        }
        private void LoadSerial()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                grdItems.Rows[row.Index].Cells["colSrNo"].Value = string.Format("{0}  ", row.Index + 1).ToString();
            }
        }
        private void UpdateRow(AdjustmentItem objAdjustment)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objAdjustment.BatchId;
            r.Cells["colBatch"].Value = objAdjustment.BatchName;

            r.Cells["colUnit"].Value = objAdjustment.UnitString;
            r.Cells["colConvUnit"].Value = objAdjustment.ConversionUnit;

            r.Cells["colQuantity"].Value = objAdjustment.Quantity;
            r.Cells["colReason"].Value = objAdjustment.Reason;

            r.Cells["colAdjustmentPrice"].Value = objAdjustment.AdjustmentValue;
            r.Cells["colCostPrice"].Value = objAdjustment.CostPrice;
            grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        private void FillObject(AdjustmentItem objAdjItem)
        {
            objAdjItem.ItemId = this.SelectedItemId; //Convert.ToInt32(cmbItems.SelectedValue);
            objAdjItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);
            objAdjItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objAdjItem.ConversionUnit = (int)numConvUnit.Value;
            objAdjItem.BatchId = Convert.ToInt32(cmbBatch.SelectedValue);
            objAdjItem.BatchName = cmbBatch.GetItemText(cmbBatch.SelectedItem);
            objAdjItem.Quantity = Convert.ToInt32(numQty.Value);
            objAdjItem.Reason = txtReason.Text.Trim();
            objAdjItem.CostPrice = SharedFunctions.StringToDouble(txtCostPrice.Text);
            objAdjItem.AdjustmentValue = SharedFunctions.StringToDouble(txtAdjustmentPrice.Text);
        }
        private bool IsItemAlreadyAdded()
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == this.SelectedItemId// Convert.ToInt32(cmbItems.SelectedValue)
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == Convert.ToInt64(cmbBatch.SelectedValue)
                    )
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            return false;
        }
        private void Clear()
        {
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            cmbItems.Enabled = true;
            btnAddItem.Text = "Add Item";
            this.SelectedItemId = 0;
            this.SelectedItemName = "";
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = "";
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            cmbUnit.Items.Clear();
            cmbUnit.SelectedIndex = -1;
            numConvUnit.Value = 1;
            cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
            cmbBatch.SelectedIndex = -1;
            cmbBatch.DataSource = null;
            cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
            EditingRowIndex = -1;
            txtAdjustmentPrice.Text = "0";
            txtCostPrice.Text = "0";
            cmbItems.SelectedIndex = -1;
            numQty.Value = 1;
            txtReason.Text = "";
            this.uC_SearchItems1.SetFocus();
            this.uC_SearchItems1.SetFocus();
        }
        private bool IsValidInput()
        {

            bool ErrFound = false;
            if (this.SelectedItemId <= 0) // (cmbItems.SelectedIndex < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (cmbBatch.SelectedIndex < 0)
            {
                errSelectBatch.Visible = true;
                ErrFound = true;
            }
            else
            {
                errSelectBatch.Visible = false;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 1)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    ClearAll();
                }
            }
        }

        private void ClearAll()
        {
            Clear();
            grdItems.Rows.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Can't Save Empty Adjustment, Please Add Some items", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (this.AdjustmentId > 0)
                {
                    UpdateAdjustment();
                    return;
                }
                InsertAdjustment();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Errror");
            }
        }

        private void InsertAdjustment()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Adjustment objAdjustment = new Adjustment();
                objAdjustment.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                objAdjustment.CreateDate = DateTime.Now;
                objAdjustment.UpdateDate = DateTime.Now;
                objAdjustment.AdjustmentItems = new List<AdjustmentItem>();
                objAdjustment.IsNew = true;
                objAdjustment.IsActive = true;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    AdjustmentItem objItem = new AdjustmentItem();
                    objItem.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                    objItem.Batch = unitOfWork.BatchRepository.GetById(Convert.ToInt64(r.Cells["colbatchid"].Value));
                    objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                    objItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                    objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity * objItem.Item.ConversionUnit : objItem.Quantity;
                    objItem.Reason = r.Cells["colReason"].Value.ToString();
                    objItem.CostPrice = objItem.Unit == 0 ? SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value) / objItem.Item.ConversionUnit : SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value);
                    objItem.AdjustmentValue = objItem.Unit == 0 ? SharedFunctions.StringToDouble(r.Cells["colAdjustmentPrice"].Value) / objItem.Item.ConversionUnit : SharedFunctions.StringToDouble(r.Cells["colAdjustmentPrice"].Value);
                    objItem.CreateDate = DateTime.Now;
                    objItem.UpdateDate = DateTime.Now;
                    objItem.IsActive = true;
                    objItem.IsNew = true;

                    objItem.Item.IsSyncable = true;
                    unitOfWork.GetDbContext().Entry(objItem.Item).State = System.Data.Entity.EntityState.Modified;
                    objAdjustment.AdjustmentItems.Add(objItem);
                }
                unitOfWork.AdjustmentRepository.Insert(objAdjustment);
                AuditLogTask.CreateAuditLog(unitOfWork, AuditModule.Stock_Adjustment, AuditAction.created);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objAdjustment, ModuleName.StockAdjustment, ModuleAction.Create, objAdjustment.AdjustmentId);
            }
            MessageBox.Show("Adjustment Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //ClearAll();
            SharedVariables.manageStockAdjsRef = true;
            this.Close();
        }
        private void UpdateAdjustment()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Adjustment objAdjustment = unitOfWork.AdjustmentRepository.GetAdjustmentById(this.AdjustmentId);
                objAdjustment.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);

                if (objAdjustment.IsSynced)
                {
                    objAdjustment.IsNew = false;
                    objAdjustment.IsUpdate = true;
                    objAdjustment.IsSynced = false;
                }

                objAdjustment.UpdateDate = DateTime.Now;
                long AdjItemId = 0;
                int ItemId = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    AdjItemId = Convert.ToInt64(r.Cells["colAdjustmentItemId"].Value);
                    ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    if (AdjItemId == 0)
                    {
                        AdjustmentItem objItem = new AdjustmentItem();
                        objItem.Item = unitOfWork.ItemRspository.GetById(ItemId);
                        objItem.Batch = unitOfWork.BatchRepository.GetById(Convert.ToInt64(r.Cells["colbatchid"].Value));

                        objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                        objItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                        objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity * objItem.Item.ConversionUnit : objItem.Quantity;

                        objItem.Reason = r.Cells["colReason"].Value.ToString();
                        objItem.CreateDate = DateTime.Now;
                        objItem.UpdateDate = DateTime.Now;
                        objItem.IsNew = true;
                        objItem.IsActive = true;

                        //following 2 lines of code were added due to default items issue handeling.
                        objItem.Item.IsSyncable = true;
                        if (SharedFunctions.IsDoubleFormat(SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value), "Stock Adjustment Item"))
                            return;
                        objItem.CostPrice = objItem.Unit == 0 ? Math.Round(SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value), 3) * objItem.Item.ConversionUnit : Math.Round(SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value), 3);

                        objItem.AdjustmentValue = objItem.Unit == 0 ? SharedFunctions.StringToDouble(r.Cells["colAdjustmentPrice"].Value) * objItem.Item.ConversionUnit : SharedFunctions.StringToDouble(r.Cells["colAdjustmentPrice"].Value);
                        unitOfWork.GetDbContext().Entry(objItem.Item).State = System.Data.Entity.EntityState.Modified;

                        objAdjustment.AdjustmentItems.Add(objItem);
                    }
                    else
                    {
                        if (!r.Visible)
                        {
                            AdjustmentItem ai = objAdjustment.AdjustmentItems.Where(i => i.AdjustmentItemId == AdjItemId).FirstOrDefault();
                            ai.IsActive = false;
                            ai.IsUpdate = true;
                            ai.IsSynced = false;
                            ai.UpdateDate = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(ai).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    AdjItemId = 0;
                    ItemId = 0;
                }
                unitOfWork.AdjustmentRepository.Update(objAdjustment);
                AuditLogTask.CreateAuditLog(unitOfWork, AuditModule.Stock_Adjustment, AuditAction.updated);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objAdjustment, ModuleName.StockAdjustment, ModuleAction.Update, objAdjustment.AdjustmentId);
            }
            MessageBox.Show("Adjustment Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // ClearAll();
            SharedVariables.manageStockAdjsRef = true;
            this.Close();
        }
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBatch.DataSource = null;
            cmbBatch.SelectedIndex = -1;
            if (cmbItems.SelectedIndex >= 0)
            {
                cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
                GetItemBatches(Convert.ToInt32(cmbItems.SelectedValue));
                cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
            }
            if (cmbBatch.Items.Count > 0)
            {
                cmbBatch.SelectedIndex = 0;
                cmbSelectBatch_SelectedIndexChanged(null, null);
            }
        }

        private void GetItemBatches(int ItemId)
        {
            itemId = ItemId;
            ItemDetailVM itemDetail = new ItemDetailVM();
            using (unitOfWork = new UnitOfWork())
            {
                itemDetail = unitOfWork.ItemRspository.GetBatchStockByItemId_1(ItemId);
            }
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            foreach (BatchStockVM i in itemDetail.BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);
           
                //set Quanity accor to Unit
                //if (i.AvailableStock < 0) { i.AvailableStock = i.Unit == 0 ? i.AvailableStock / itemDetail.Item.ConversionUnit : i.AvailableStock * itemDetail.Item.ConversionUnit; }
                //else { 
                //i.AvailableStock = i.Unit == 0 ? i.AvailableStock / itemDetail.Item.ConversionUnit : i.AvailableStock; 
                //}
                int convUnit = Convert.ToInt32(numConvUnit.Value);
                i.BatchName = i.BatchName + " | Available Stock : " + (i.AvailableStock / (cmbUnit.Text == "Pack" || cmbUnit.Text == "pack" ? convUnit : 1));
                if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                {
                    if (i.AvailableStock > 0)
                    {
                        FinalResult.Add(i);
                    }
                }
                else
                {
                    FinalResult.Add(i);
                }
            }
            cmbBatch.DataSource = FinalResult;
            cmbBatch.ValueMember = "BatchId";
            cmbBatch.DisplayMember = "BatchName";
        }

        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBatch.SelectedIndex >= 0)
            {
                double costPrice = SharedFunctions.StringToDouble(txtCostPrice.Text);
                txtAdjustmentPrice.Text = Math.Round(((int)numQty.Value * costPrice), 2).ToString();
                //if (numQty.Value < 0)
                //{
                //if (isValidQuantityEntered())
                //{
                //    numQty.BackColor = SystemColors.Window;
                //}
                //else
                //{
                //    numQty.BackColor = Color.Coral;
                //}
                //}
            }
        }

        private bool isValidQuantityEntered()
        {
            //int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            //if (!cmbUnit.Text.ToLower().Contains("units"))
            //{
            //    if (numQty.Value < 0 || Math.Abs(numQty.Value) * numConvUnit.Value > AvailableStock)
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (numQty.Value < 0 || Math.Abs(numQty.Value) > AvailableStock)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbUnit.SelectedValueChanged -= cmbUnit_SelectedValueChanged;
                //cmbUnit.SelectedIndex = 0;
                if (cmbBatch.SelectedValue != null && SharedFunctions.StringToInt(cmbBatch.SelectedValue) > 0)
                {
                    AssignCostAndAdjustment(SharedFunctions.StringToInt(cmbBatch.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbUnit.SelectedValueChanged += cmbUnit_SelectedValueChanged;
            }
        }

        private void cmbUnit_SelectedValueChanged(object sender, EventArgs e)
        {
            //set Batch Value acc to unit/pack
            ItemDetailVM itemDetail = new ItemDetailVM();
            using (unitOfWork = new UnitOfWork())
            {
                itemDetail = unitOfWork.ItemRspository.GetBatchStockByItemId_1(Convert.ToInt32(itemId));
            }
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            int indexCopy = cmbBatch.SelectedIndex;

            foreach (BatchStockVM i in itemDetail.BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);
                //i.AvailableStock = cmbUnit.Text == "Pack" ? i.AvailableStock / itemDetail.Item.ConversionUnit : i.AvailableStock;
                //i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
                i.BatchName = i.BatchName + " | Available Stock : " + (i.AvailableStock / (cmbUnit.Text == "Pack" || cmbUnit.Text == "pack" ? numConvUnit.Value : 1));

                if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                {
                    if (i.AvailableStock > 0)
                    {
                        FinalResult.Add(i);
                    }
                }
                else
                {
                    FinalResult.Add(i);
                }
            }
            cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
            cmbBatch.DataSource = FinalResult;
            cmbBatch.ValueMember = "BatchId";
            cmbBatch.DisplayMember = "BatchName";

            cmbBatch.SelectedIndex = indexCopy;
            if (SharedFunctions.StringToInt(cmbBatch.SelectedValue) > 0)
            {
                AssignCostAndAdjustment(SharedFunctions.StringToInt(cmbBatch.SelectedValue));
            }
            cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        }
        private void AssignCostAndAdjustment(int BatchId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                double costprice = unitOfWork.ItemRspository.GetBatchCostPriceById(BatchId);
                if (!cmbUnit.Text.ToLower().Contains("units"))
                {
                    txtCostPrice.Text = (costprice * (int)numConvUnit.Value).ToString();
                    //numQty.Value = numQty.Value * (int)numConvUnit.Value;
                    txtAdjustmentPrice.Text = (costprice * (int)numConvUnit.Value * (int)numQty.Value).ToString();
                }
                else
                {
                    txtCostPrice.Text = (costprice).ToString();
                    //numQty.Value = numQty.Value;
                    txtAdjustmentPrice.Text = (costprice * (int)numQty.Value).ToString();
                }
            }
        }

        private void txtCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.IsValidDoubleNumerickey(sender, e);
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}