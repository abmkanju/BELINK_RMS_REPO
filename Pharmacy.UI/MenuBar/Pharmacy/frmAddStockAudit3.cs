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
    public partial class frmAddStockAudit3 : Form
    {
        UnitOfWork unitOfWork;
        private int StockAuditId = 0;
        private List<int> deledtedDetails = new List<int>();
        public frmAddStockAudit3()
        {
            InitializeComponent();
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
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
            dataGridView1.Rows.Add(0, ItemId, "", "", 1, 0, 0, 0, 0, 0, 0);
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

        }
        private void calculateRowTotals(DataGridViewRow r)
        {
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            try
            {
                int phQty = 0;
                double retPr = Convert.ToInt32(r.Cells["colRetPr"].Value);
                int sysQty = Convert.ToInt32(r.Cells["colSysQty"].Value);
                int.TryParse(r.Cells["colPhyQty"].Value.ToString(), out phQty);
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();


                if (unit == "Units")
                {
                    r.Cells["colSysQty"].Value = sysQty = sysQty * convUnit;
                    r.Cells["colRetPr"].Value = retPr = retPr / convUnit;
                }
                else
                {
                    r.Cells["colSysQty"].Value = sysQty = sysQty / convUnit;
                    r.Cells["colRetPr"].Value = retPr = retPr * convUnit;
                }

                int diff = phQty - sysQty;
                r.Cells["colQtyDiff"].Value = diff;
                r.Cells["colAmtDiff"].Value = diff * retPr;
                r.Cells["colCurrAdjQty"].Value = sysQty + diff;

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

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int itmId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colItemId"].Value);
            if (itmId <= 0) return;

            loadItemData(dataGridView1.Rows[e.RowIndex], itmId);
        }
        private void loadItemData(DataGridViewRow r, int itemId)
        {
            // requried to unregister twice. was not gettin gun registered on single call            
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            try
            {
                ItemsVM vm = new ItemsVM();
                int avQty = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.ItemRspository.getStockByItemId(itemId);
                }
                if (vm != null)
                {
                    if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    {
                        avQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                    }
                    else
                    {
                        avQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock + vm.HoldStock;
                    }
                    //avQty = avQty / vm.ConversionUnit;
                    r.Cells["colItemName"].Value = vm.ItemName;

                    DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    dgCmbUnit.Items.Add(vm.Unit);
                    dgCmbUnit.Items.Add("Units");
                    r.Cells["colUnit"].Value = dgCmbUnit.Items[0];

                    r.Cells["colConvUnit"].Value = vm.ConversionUnit;

                    r.Cells["colSysQty"].Value = avQty;
                    r.Cells["colRetPr"].Value = vm.RetailPrice;
                    r.Cells["colCurrAdjQty"].Value = 0;
                    r.Cells["colCurrQty"].Value = avQty;
                    calculateRowTotals(r);
                }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int PhyQty = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["colPhyQty"].Value);
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
                dataGridView1.Rows[e.RowIndex].Cells["colCurrAdjQty"].Value = sysQty;
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
                        loadItemData(dataGridView1.Rows[e.RowIndex], itemId);
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
    }
}
