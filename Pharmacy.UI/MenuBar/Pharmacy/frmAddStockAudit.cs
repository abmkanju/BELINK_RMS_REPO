using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddStockAudit : Form
    {
        UnitOfWork unitOfWork;
        private int StockAuditId = 0;
        private List<int> deledtedDetails = new List<int>();
        private bool isCellValuesChanging = false;
        private bool isLoadingAudit = false;

        public frmAddStockAudit()
        {
            InitializeComponent();
        }
        public frmAddStockAudit(int StockAuditId)
        {
            InitializeComponent();
            this.StockAuditId = StockAuditId;
        }

        private void frmAddStockAudit_Load(object sender, EventArgs e)
        {
            try
            {
                this.isCellValuesChanging = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                lblCurrentDate.Text = DateTime.Now.ToShortDateString();
                SharedFunctions.SetGridStyle(dataGridView1);
                if (this.StockAuditId > 0)
                {                    
                    isLoadingAudit = true;
                    LoadAudit();
                    isLoadingAudit = false;
                    foreach(DataGridViewColumn c in dataGridView1.Columns)
                    {                        
                        c.ReadOnly = true;
                    }                 
                }
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                isLoadingAudit = false;
                this.isCellValuesChanging = false;
            }
        }

        string ItemUnit = "";

        private void LoadAudit()
        {
            StockAuditVM audit = new StockAuditVM();
            using (unitOfWork = new UnitOfWork())
            {
                audit = unitOfWork.StockAuditRepository.GetAuditDetailVMById(this.StockAuditId);
            }
            this.txtNote.Text = audit.Note;
            foreach (var d in audit.StockAuditDetailVM)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    d.AvailableStock = d.TotalStock + d.AdjustedStock - d.ConsumedStock - (d.ExpiredStock - d.ExpiredConsumedStock);
                }
                else
                {
                    d.AvailableStock = d.TotalStock + d.AdjustedStock - d.ConsumedStock - (d.ExpiredStock - d.ExpiredConsumedStock) + d.HoldStock;
                }
                string unitString = "";
                ItemUnit = d.ItemUnit;
                unitString = d.AuditUnit == 0 ? d.ItemUnit : "Units";
                dataGridView1.Rows.Add(d.StockAuditDetailId, d.ItemId, d.ItemName, unitString, d.ConversionUnit, d.AvailableStock, d.SystemQuantity, d.PhysicalQuantity, d.Differnce, d.UnitCost, d.CurrentAdjustedQuantity, 0, d.RetailPrice1, d.AmountDifference);
            }
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem(0,true);
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            if (this.ItemAlreadAdded(ItemId))
            {
                MessageBox.Show("Item already added, please select some other items.", "Item Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dataGridView1.Rows.Add(0, ItemId, "", "", 1, 0, 0, 0, 0, 0, 0, 0);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private bool ItemAlreadAdded(int itemId)
        {
            int id = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                id = 0;
                id = Convert.ToInt32(r.Cells["colItemId"].Value);
                if (itemId == id)
                {
                    return true;
                }
            }
            return false;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("value changed");
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            calculateRowTotals(dataGridView1.Rows[e.RowIndex], e);
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int itmId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colItemId"].Value);
            if (itmId <= 0) return;
            if (isLoadingAudit)
            {
                loadItemData_1(dataGridView1.Rows[e.RowIndex]);
            }
            else
            {
                loadItemData(dataGridView1.Rows[e.RowIndex]);
            }
        }

        private void loadItemData(DataGridViewRow r)
        {
            // requried to unregister twice. was not gettin gun registered on single call            
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            ItemsVM vm = new ItemsVM();
            int itemId = Convert.ToInt32(r.Cells["colItemId"].Value); ;
            int phQty = 0, sysQty = 0;
            double retPr = 0;
            double costPr = 0;
            try
            {

                //int.TryParse(r.Cells["colPhyQty"].Value.ToString(), out phQty);               
                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.ItemRspository.getStockByItemId(itemId);
                }

                r.Cells["colItemName"].Value = vm.ItemName;
                r.Cells["colConvUnit"].Value = vm.ConversionUnit;
                DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                dgCmbUnit.Items.Add(vm.Unit);
                dgCmbUnit.Items.Add("Units");
                r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
                r.Cells["colConvUnit"].Value = vm.ConversionUnit;

                if (vm != null)
                {
                    if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    {
                        sysQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                    }
                    else
                    {
                        sysQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock + vm.HoldStock;
                    }
                    retPr = vm.RetailPrice;
                    costPr = vm.UnitCost;
                }


                if (vm.Unit != "Units")
                {
                    sysQty = sysQty / vm.ConversionUnit;
                    retPr = retPr * vm.ConversionUnit;
                    costPr = costPr * vm.ConversionUnit;
                }
                r.Cells["colSysQty"].Value = sysQty;
                r.Cells["colNewSysQty"].Value = sysQty;
                r.Cells["colRetPr"].Value = retPr;
                r.Cells["colCostPrice"].Value = costPr;
                int diff = phQty - sysQty;
                r.Cells["colQtyDiff"].Value = diff;
                r.Cells["colAmtDiff"].Value = diff * retPr;
                r.Cells["colCurrAdjQty"].Value = sysQty + diff;
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            }
        }
        private void loadItemData_1(DataGridViewRow r)
        {
            // requried to unregister twice. was not gettin gun registered on single call            
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            int itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            try
            {
                DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                dgCmbUnit.Items.Add(this.ItemUnit); // this variables is being re-assigned in loop while loading stock audit.
                dgCmbUnit.Items.Add("Units");
                r.Cells["colUnit"].Value = r.Cells["colUnit"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            }
        }
        private void calculateRowTotals(DataGridViewRow r, DataGridViewCellEventArgs e)
        {
            try
            {
                int phQty = 0;
                double retPr = 0;
                double costPr = 0;
                int sysQty = 0;
                int itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                int.TryParse(r.Cells["colPhyQty"].Value.ToString(), out phQty);
                
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                ItemsVM vm = new ItemsVM();

                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.ItemRspository.getStockByItemId(itemId);
                }
                if (vm != null)
                {
                    if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    {
                        sysQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                    }
                    else
                    {
                        sysQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock + vm.HoldStock;
                    }
                    retPr = vm.RetailPrice;
                    costPr = vm.UnitCost;
                }

                if (unit != "Units")
                {
                    sysQty = sysQty / vm.ConversionUnit;
                    retPr = retPr * vm.ConversionUnit;
                    costPr = costPr * vm.ConversionUnit;
                }

                r.Cells["colSysQty"].Value = sysQty;
                r.Cells["colRetPr"].Value = retPr;
                int qtyDiff = Convert.ToInt32(r.Cells["colQtyDiff"].Value);

                //change
                if (e.ColumnIndex == dataGridView1.Columns["colUnit"].Index)
                {
                    if (qtyDiff >= 0)
                    {
                        r.Cells["colPhyQty"].Value = sysQty;
                        phQty = Convert.ToInt32(r.Cells["colPhyQty"].Value);
                    }
                }
                r.Cells["colCostPrice"].Value = costPr;

                int diff = phQty - sysQty;
                r.Cells["colQtyDiff"].Value = diff;
                r.Cells["colAmtDiff"].Value = diff * retPr;
                r.Cells["colCurrAdjQty"].Value = sysQty + diff;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            }
        }

        private void loadDetailData(DataGridViewRow r, int auditDetailId)
        {
            StockAuditDetail d = new StockAuditDetail();
            using (unitOfWork = new UnitOfWork())
            {
                d = unitOfWork.StockAuditRepository.getAuditDetailById(auditDetailId);
            }
            if (d != null)
            {
                r.Cells["colSysQty"].Value = d.SystemQuantity;
                r.Cells["colPhyQty"].Value = d.PhysicalQuantity;
                r.Cells["colQtyDiff"].Value = d.Differnce;
                r.Cells["colCurrAdjQty"].Value = d.CurrentAdjustedQuantity;
                r.Cells["colCurrQty"].Value = 0;
                r.Cells["colRetPr"].Value = d.RetailPrice;
                r.Cells["colAmtDiff"].Value = d.AmountDifference;
            }
        }

        private void btnSaveAudit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.StockAuditId > 0)
                {
                    update();
                }
                else
                {
                    insert();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting stock audit, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void insert()
        {
            using (unitOfWork = new UnitOfWork())
            {
                StockAudit audit = new StockAudit();
                audit.CreatedAt = DateTime.Now;
                audit.UpdatedAt = DateTime.Now;
                audit.StockAuditDate = dtpAudDate.Value;
                audit.StockAuditDetail = new List<StockAuditDetail>();
                audit.Note = txtNote.Text.Trim();
                audit.IsNew = true;
                audit.IsActive = true;
                audit.IsUpdate = false;
                audit.IsSynced = false;
                audit.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                Adjustment objAdjustment = new Adjustment();
                objAdjustment.CreateDate = DateTime.Now;
                objAdjustment.UpdateDate = DateTime.Now;
                objAdjustment.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                objAdjustment.IsNew = true;
                objAdjustment.IsUpdate = false;
                objAdjustment.IsActive = true;
                objAdjustment.IsSynced = false;
                objAdjustment.AdjustmentItems = new List<AdjustmentItem>();
                int itemId = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    itemId = 0;
                    itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    StockAuditDetail detail = new StockAuditDetail();
                    detail.Item = unitOfWork.ItemRspository.GetById(itemId);
                    detail.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                    detail.SystemQuantity = Convert.ToInt32(r.Cells["colSysQty"].Value);
                    //detail.SystemQuantity = detail.Unit == 0 ? detail.SystemQuantity * detail.Item.ConversionUnit : detail.SystemQuantity;
                    detail.PhysicalQuantity = Convert.ToInt32(r.Cells["colPhyQty"].Value);
                    //detail.PhysicalQuantity = detail.Unit == 0 ? detail.PhysicalQuantity * detail.Item.ConversionUnit : detail.PhysicalQuantity;
                    detail.CurrentAdjustedQuantity = Convert.ToInt32(r.Cells["colCurrAdjQty"].Value);
                    //detail.CurrentAdjustedQuantity = detail.Unit == 0 ? detail.CurrentAdjustedQuantity * detail.Item.ConversionUnit : detail.CurrentAdjustedQuantity;
                    detail.Differnce = Convert.ToInt32(r.Cells["colQtyDiff"].Value);
                    //detail.Differnce = detail.Unit == 0 ? detail.CurrentAdjustedQuantity * detail.Item.ConversionUnit : detail.CurrentAdjustedQuantity;
                    detail.RetailPrice = Convert.ToInt32(r.Cells["colRetPr"].Value);
                    detail.AmountDifference = detail.Differnce * detail.RetailPrice;
                    detail.CreatedAt = DateTime.Now;
                    detail.UpdatedAt = DateTime.Now;
                    detail.IsActive = true;
                    detail.IsNew = true;
                    detail.IsSynced = false;
                    detail.IsUpdate = false;
                    audit.TotalDifference += detail.AmountDifference;
                    audit.StockAuditDetail.Add(detail);

                    AdjustmentItem ai = new AdjustmentItem();
                    ai.Item = unitOfWork.ItemRspository.GetById(itemId);
                    ai.IsNew = true;
                    ai.IsActive = true;
                    ai.IsSynced = false;
                    ai.Quantity = detail.Unit == 0 ? detail.Differnce * detail.Item.ConversionUnit : detail.Differnce;
                    ai.Reason = "Stock Audit";
                    ai.CostPrice = ai.Item.UnitCostPrice;
                    ai.AdjustmentValue = ai.Unit == 0 ? ai.CostPrice / detail.Item.ConversionUnit : ai.CostPrice;
                    //ai.AdjustmentValue = ai.CostPrice;
                    ai.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                    ai.Batch = unitOfWork.StockRepository.GetRequiredBatch(itemId);
                    ai.CreateDate = DateTime.Now;
                    ai.UpdateDate = DateTime.Now;
                    ai.IsActive = true;
                    ai.IsSynced = false;
                    ai.IsNew = true;
                    objAdjustment.AdjustmentItems.Add(ai);
                    //AdjustmentItem adjItem = new AdjustmentItem{
                    //    Item = unitOfWork.ItemRspository.GetById(detail.Item.ItemID),
                    //}
                }
                unitOfWork.AdjustmentRepository.Insert(objAdjustment);
                unitOfWork.StockAuditRepository.Insert(audit);
                unitOfWork.Save();
                MessageBox.Show("Stock audit saved successfully.", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void update()
        {
            using (unitOfWork = new UnitOfWork())
            {
                StockAudit audit = unitOfWork.StockAuditRepository.GetAuditWithDetailById(this.StockAuditId);
                if (audit == null)
                {
                    MessageBox.Show("An error occurred while updating audit, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                audit.UpdatedAt = DateTime.Now;
                audit.StockAuditDate = dtpAudDate.Value;
                audit.Note = txtNote.Text.Trim();
                if (audit.IsSynced)
                {
                    audit.IsNew = false;
                    audit.IsUpdate = true;
                    audit.IsSynced = false;
                }
                audit.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);

                // in_activate removed records
                foreach (var d in audit.StockAuditDetail)
                {
                    foreach (int id in deledtedDetails)
                    {
                        if (d.StockAuditDetailId == id)
                        {
                            d.IsActive = false;
                            unitOfWork.GetDbContext().Entry(d).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }

                double totalDiff = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    int detailId = Convert.ToInt32(r.Cells["colAuditDetailId"].Value);
                    if (detailId > 0)
                    {
                        foreach (var e in audit.StockAuditDetail)
                        {
                            if (e.StockAuditDetailId == detailId)
                            {
                                // no item update. only other attributes can be updated.
                                e.SystemQuantity = Convert.ToInt32(r.Cells["colSysQty"].Value);
                                e.PhysicalQuantity = Convert.ToInt32(r.Cells["colPhyQty"].Value);
                                e.CurrentAdjustedQuantity = Convert.ToInt32(r.Cells["colCurrAdjQty"].Value);
                                e.Differnce = Convert.ToInt32(r.Cells["colQtyDiff"].Value);
                                e.RetailPrice = Convert.ToInt32(r.Cells["colRetPr"].Value);
                                e.AmountDifference = e.Differnce * e.RetailPrice;
                                e.UpdatedAt = DateTime.Now;
                                e.IsActive = true;
                                e.IsSynced = false;
                                e.IsUpdate = false;
                                totalDiff += e.AmountDifference;
                                unitOfWork.GetDbContext().Entry(e).State = System.Data.Entity.EntityState.Modified;
                                break;
                            }
                        }
                    }
                    else
                    {
                        StockAuditDetail detail = new StockAuditDetail();
                        detail.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                        detail.SystemQuantity = Convert.ToInt32(r.Cells["colSysQty"].Value);
                        detail.PhysicalQuantity = Convert.ToInt32(r.Cells["colPhyQty"].Value);
                        detail.CurrentAdjustedQuantity = Convert.ToInt32(r.Cells["colCurrAdjQty"].Value);
                        detail.Differnce = Convert.ToInt32(r.Cells["colQtyDiff"].Value);
                        detail.RetailPrice = Convert.ToInt32(r.Cells["colRetPr"].Value);
                        detail.AmountDifference = detail.Differnce * detail.RetailPrice;
                        detail.CreatedAt = DateTime.Now;
                        detail.UpdatedAt = DateTime.Now;
                        detail.IsActive = true;
                        detail.IsNew = true;
                        detail.IsSynced = false;
                        detail.IsUpdate = false;
                        audit.StockAuditDetail.Add(detail);
                    }
                }
                audit.TotalDifference = totalDiff;
                unitOfWork.StockAuditRepository.Update(audit);
                unitOfWork.Save();
                MessageBox.Show("Stock audit updated successfully.", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnImplAudit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Cells["colSysQty"].Value = r.Cells["colPhyQty"].Value;
            }
        }

        private void btnDiffEnt_Click(object sender, EventArgs e)
        {
            int dif = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                dif = 0;
                dif = Convert.ToInt32(r.Cells["colQtyDiff"].Value);
                if (dif == 0)
                {
                    r.Visible = false;
                }
            }
            lblStatus.Text = "Showing Specfic";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Cells["colPhyQty"].Value = r.Cells["colSysQty"].Value;
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Visible = true;
            }
            lblStatus.Text = "Showing All";
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            int diff = 0;
            int sysQty = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                diff = Convert.ToInt32(r.Cells["colQtyDiff"].Value);
                sysQty = Convert.ToInt32(r.Cells["colSysQty"].Value);
                ////if (diff <= 0)
                ////{
                ////    DialogResult res = MessageBox.Show("Quantity Differnce is zero OR less, are you sure you want to adjust in system quantity.", "Please make sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                ////    if (res == System.Windows.Forms.DialogResult.Cancel || res == System.Windows.Forms.DialogResult.No)
                ////    {
                ////        return;
                ////    }
                ////}
                ////r.Cells["colCurrAdjQty"].Value = sysQty + diff;
                r.Cells["colCurrAdjQty"].Value = r.Cells["colPhyQty"].Value;
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.RowIndex < 0) return;
            // no action needed
            if(this.StockAuditId  > 0)
            {
                return;
            }
            int PhyQty = 0;
            int.TryParse(dataGridView1.Rows[e.RowIndex].Cells["colPhyQty"].Value.ToString(), out PhyQty);
            int diff = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colQtyDiff"].Value);
            int sysQty = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colSysQty"].Value);
            if (e.ColumnIndex == dataGridView1.Columns["colBtnAdjust"].Index)
            {
                //if (PhyQty <= 0)
                //{
                //    DialogResult res = MessageBox.Show("Physical quantity is zero OR less, are you sure you want to adjust in system quantity.", "Please make sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                //    if (res == System.Windows.Forms.DialogResult.Cancel || res == System.Windows.Forms.DialogResult.No)
                //    {
                //        return;
                //    }
                //}
                dataGridView1.Rows[e.RowIndex].Cells["colCurrAdjQty"].Value = PhyQty;
                return;
            }
            if (e.ColumnIndex == dataGridView1.Columns["colBtnCopy"].Index)
            {
                //if (sysQty <= 0)
                //{
                //    DialogResult res = MessageBox.Show("System quantity is zero OR less, are you sure you want to change physical quantity.", "Please make sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                //    if (res == System.Windows.Forms.DialogResult.Cancel || res == System.Windows.Forms.DialogResult.No)
                //    {
                //        return;
                //    }
                //}
                dataGridView1.Rows[e.RowIndex].Cells["colPhyQty"].Value = sysQty;
                return;
            }

            int detailId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colAuditDetailId"].Value);
            if (e.ColumnIndex == dataGridView1.Columns["colBtnRefresh"].Index)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to refresh values for selected row?.", "Please make sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    dataGridView1.Rows[e.RowIndex].Cells["colSysQty"].Value = "0";
                    dataGridView1.Rows[e.RowIndex].Cells["colPhyQty"].Value = "0";
                    dataGridView1.Rows[e.RowIndex].Cells["colQtyDiff"].Value = "0";
                    dataGridView1.Rows[e.RowIndex].Cells["colCurrAdjQty"].Value = "0";
                    dataGridView1.Rows[e.RowIndex].Cells["colRetPr"].Value = "0";
                    dataGridView1.Rows[e.RowIndex].Cells["colAmtDiff"].Value = "0";
                    int itemId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colItemId"].Value);
                    if (detailId > 0)
                    {
                        loadDetailData(dataGridView1.Rows[e.RowIndex], detailId);
                    }
                    else
                    {
                        loadItemData(dataGridView1.Rows[e.RowIndex]);
                    }
                }
                return;
            }
            if (e.ColumnIndex == dataGridView1.Columns["colBtnRemove"].Index)
            {
                if (detailId > 0)
                {
                    deledtedDetails.Add(detailId);
                }
                dataGridView1.Rows.Remove(dataGridView1.Rows[e.RowIndex]);
                return;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["colUnit"].Index)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}