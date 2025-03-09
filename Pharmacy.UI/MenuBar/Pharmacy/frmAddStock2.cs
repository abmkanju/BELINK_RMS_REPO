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
using Pharmacy.UI.CustomCode;
using Pharmacy.Core;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddStock2 : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        SupplierRepository repSupplier = new SupplierRepository(cxt);
        PurchaseOrderRepository repPO = new PurchaseOrderRepository(cxt);
        StockRepository repStock = new StockRepository(cxt);
        ItemsRepository repItems = new ItemsRepository(cxt);
        List<StockItem> StockItemsList = new List<StockItem>();
        UnitOfWork unitOfWork = new UnitOfWork();
        long PurchaseOrderId = 0;
        int DiscountType = 1; //Lumpsum
        public frmAddStock2()
        {
            InitializeComponent();
        }

        public frmAddStock2(long PurchaseOrderId)
        {
            InitializeComponent();
            this.PurchaseOrderId = PurchaseOrderId;
        }

        private void frmAddStock_Load(object sender, EventArgs e)
        {
            try
            {

                CalendarColumn c = new CalendarColumn();
                c.Name = "colExpiry";
                c.HeaderText = "Expiry";
                grdItems.Columns.Insert(7,c);
                LoadSuppliers();
                ShowNewDocumentNumber();
                if(this.PurchaseOrderId > 0)
                {
                    LoadPurchaseOrder();
                }
                cmbDiscountType.SelectedIndex = 0;
                cmbSalesTaxType.SelectedIndex = 0;
                //grdItems.Rows[grdItems.NewRowIndex].Cells["colDiscountType"].Value = "Value";
                //grdItems.Rows[grdItems.NewRowIndex].Cells["colSalesTaxType"].Value = "Value";

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }          
        }

        private void LoadPurchaseOrder()
        {
            PurchaseOrder Po = repPO.GetPurchaseOrderById(this.PurchaseOrderId);
            cmbSuppliers.SelectedValue = Po.Supplier.SupplierID;
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            foreach (PurchaseOrderItem i in Po.PurchaseOrderItems)
            {
                if (i.IsActive)
                {
                    object[] gr = { i.Item.ItemID, i.Item.ItemName, i.Quantity, i.UnitCost, i.TotalAmount, null, null, null, "Value", null, "Value", i.TotalAmount};
                    grdItems.Rows.Add(gr);
                }
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void LoadSuppliers()
        {
            List<Supplier> suppliers = repSupplier.GetAll().ToList();
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
        }
        private void ShowNewDocumentNumber()
        {
            txtDocumentNo.Text = repStock.GetNewDocumentNumber().ToString();
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                grdItems.Rows.RemoveAt(e.RowIndex);
                //StockItemsList.RemoveAt(e.RowIndex);
            }
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            frmAddSupplier f = new frmAddSupplier();
            f.Show();
            f.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
        }

        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            LoadSuppliers();
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
                lblImagePath.Text = selectedFile;
            }
        }

        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblImagePath.Text = "No File Choosen";
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (IsItemAlreadyAdded())
                    {
                        return;
                    }
                    StockItem objStockItem = new StockItem();
                    FillObject(objStockItem);
                    StockItemsList.Add(objStockItem);
                    grdItems.Rows.Add(objStockItem.ItemId, objStockItem.ItemName, objStockItem.Quantity, objStockItem.UnitCost, objStockItem.TotalCost, objStockItem.RetailPrice, objStockItem.BatchName, objStockItem.BatchExpiry, objStockItem.Discount, objStockItem.DiscountType, objStockItem.SalesTax, objStockItem.SalesTaxType, objStockItem.NetValue); ;
                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private bool IsItemAlreadyAdded()
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == Convert.ToInt32(cmbItems.SelectedValue))
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            return false;
        }
        private void Clear()
        {
            cmbItems.SelectedIndex = -1;
            numQty.Value = 1;
            numUnitCost.Value = 0;
            txtTotalCost.Text = "0";
            numRetailPrice.Value = 0;
            txtBatch.Text = "";
            dtpExpiry.Value = DateTime.Now;
            numDiscount.Value = 0;
            cmbDiscountType.SelectedIndex = 0;
            numSalesTax.Value = 0;
            cmbSalesTaxType.SelectedIndex = 0;
            txtNetValue.Text = "0";
        }
        private bool IsValidInput()
        {

            bool ErrFound = false;
            if (cmbItems.SelectedIndex < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
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

        private void FillObject(StockItem objStockItem)
        {
            objStockItem.ItemId = int.Parse(cmbItems.SelectedValue.ToString());
            objStockItem.ItemName = cmbItems.GetItemText(cmbItems.SelectedItem);
            objStockItem.Quantity = (int)numQty.Value;
            objStockItem.UnitCost = (double)numUnitCost.Value;
            objStockItem.TotalCost = double.Parse(txtTotalCost.Text);
            objStockItem.RetailPrice = (double)numRetailPrice.Value;
            objStockItem.BatchName = txtBatch.Text.Trim() == "" ? null : txtBatch.Text.Trim();            
            objStockItem.BatchExpiry = dtpExpiry.Value;
            objStockItem.Discount = (double)numDiscount.Value;
            //objStockItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            objStockItem.SalesTax = (double)numSalesTax.Value;
            //objStockItem.SalesTaxType = cmbSalesTaxType.GetItemText(cmbSalesTaxType.SelectedItem);
            objStockItem.NetValue = double.Parse(txtNetValue.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void ApplyDiscount()
        {
            double totalCost = double.Parse(txtTotalCost.Text);
            double discount = (double)numDiscount.Value;
            double unitCost = (double)numUnitCost.Value;
            int qty = (int)numQty.Value;
            if (DiscountType == 1)
            {
                txtTotalCost.Text = ((int)numQty.Value * (double)numUnitCost.Value).ToString();
                totalCost = double.Parse(txtTotalCost.Text);
                txtNetValue.Text = (totalCost - discount).ToString();
            }
            else if (DiscountType == 2)
            {
                txtTotalCost.Text = ((unitCost - discount) * qty).ToString();
                txtNetValue.Text = 0.ToString();
            }
        }

        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDiscountType.SelectedIndex == 0)
            {
                DiscountType = 1; //lumpsum
                ApplyDiscount();
            }
            else if (cmbDiscountType.SelectedIndex == 1)
            {
                DiscountType = 2; //per item
                ApplyDiscount();
            }
        }


        private void numUnitCost_ValueChanged(object sender, EventArgs e)
        {
            txtTotalCost.Text = ((int)numQty.Value * (double)numUnitCost.Value).ToString();
        }

        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            txtTotalCost.Text = ((int)numQty.Value * (double)numUnitCost.Value).ToString();
        }

        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            ApplyDiscount();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 1)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    StockItemsList.Clear();
                    ShowNewDocumentNumber();
                    dtpDate.Value = DateTime.Now;
                    dtpSupplierInvoiceDate.Value = DateTime.Now;
                    cmbSuppliers.SelectedIndex = -1;
                    txtSupplierInvoiceNo.Text = "";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var tr = unitOfWork.GetDbContext().Database.BeginTransaction())
                {
                    Stock objStock = new Stock();
                    objStock.DocumentNo = decimal.Parse(txtDocumentNo.Text);
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        objStock.Supplier = unitOfWork.SupplierRepository.GetById(Convert.ToInt64(cmbSuppliers.SelectedValue));
                    }
                    objStock.ImagePath = lblImagePath.Text.ToLower().Equals("no file choosen") ? null : lblImagePath.Text;
                    objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                    objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                    objStock.CreateDate = dtpDate.Value;
                    objStock.UpdateDate = DateTime.Now;
                    objStock.IsNew = true;
                    objStock.IsActive = true;
                    objStock.StockItems = new List<StockItem>();
                    string ErrorString = "";
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        StockItem objStockItem = new StockItem();
                        FillStockItemObject(r, objStockItem, ErrorString);
                        objStock.StockItems.Add(objStockItem);
                    }
                    ErrorString = ErrorString.TrimEnd().Trim(',');
                    unitOfWork.StockRepository.InsertStock(objStock);
                    PurchaseOrder po = unitOfWork.PurchaseOrderRepository.GetById(this.PurchaseOrderId);
                    //po.Stock = objStock;
                    unitOfWork.PurchaseOrderRepository.Update(po);
                    unitOfWork.Save();
                    tr.Commit();
                }
                MessageBox.Show("Stock For Selected Purchase Order Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void FillStockItemObject(DataGridViewRow r, StockItem objStockItem, string ErrorString)
        {
            int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objStockItem.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
            objStockItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
            objStockItem.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
            objStockItem.TotalCost = Convert.ToDouble(r.Cells["colTotalCost"].Value);
            objStockItem.RetailPrice = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
            objStockItem.BatchName= r.Cells["colBatch"].Value == null ? "No Batch" : r.Cells["colBatch"].Value.ToString();
            DateTime? batchExp = null;
            if (r.Cells["colExpiry"].Value != null)
            {
                batchExp = Convert.ToDateTime(r.Cells["colExpiry"].Value);
            }
            objStockItem.BatchExpiry = batchExp;
            Batch objBatch = unitOfWork.BatchRepository.FindItemBatch(objStockItem.BatchName.ToLower(), ItemId);
            if (objBatch == null)
            {
                objBatch = new Batch();
                objBatch.BatchName = objStockItem.BatchName;
                if (objStockItem.BatchExpiry != null)
                {
                    objBatch.Expiry = (DateTime)objStockItem.BatchExpiry;
                }
                else
                {
                    objBatch.Expiry = null;
                }
                objBatch.CreatedAt = DateTime.Now;
                objBatch = unitOfWork.BatchRepository.InsertBatch(objBatch);
            }
            else
            {
                if (objStockItem.BatchExpiry != null)
                {
                    objBatch.Expiry = (DateTime)objStockItem.BatchExpiry;
                    unitOfWork.GetDbContext().Entry(objBatch).State = System.Data.Entity.EntityState.Modified;
                }
            }
            objStockItem.Batch = objBatch;
            objStockItem.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            objStockItem.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            string DiscountType = r.Cells["colDiscountType"].Value.ToString();
            string SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString();
            objStockItem.DiscountType = DiscountType.ToLower() == "value" ? 1 : 2;
            objStockItem.SalesTaxType = SalesTaxType.ToLower() == "value" ? 1 : 2;
            objStockItem.NetValue = Convert.ToDouble(r.Cells["colNetValue"].Value);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStockItem.IsNew = true;
        }

        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <0)
            {
                return;
            }
            double TotalCost = 0;
            double NetValue = 0;
            double UnitCost = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
            int DiscountType = grdItems.Rows[e.RowIndex].Cells["colDiscountType"].Value.ToString() == "Value" ? 1 : 2;
            double Discount = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value);
            int SalesTaxtype = grdItems.Rows[e.RowIndex].Cells["colSalesTaxType"].Value.ToString() == "Value" ? 1 : 2;
            double SalesTax = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colSalesTax"].Value);
            int Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);

            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscounttype"].Index
                || e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                )
            {
                TotalCost = Quantity * UnitCost;
                if(DiscountType == 2)
                {
                    Discount = Quantity * Discount;
                }
                NetValue = TotalCost - Discount;
                grdItems.Rows[e.RowIndex].Cells["colTotalCost"].Value = TotalCost;
                grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value = NetValue;
            }
        }

        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            grdItems.Rows[e.RowIndex].Cells["colDiscountType"].Value = "Value";
            grdItems.Rows[e.RowIndex].Cells["colSalesTaxType"].Value = "Value";            
        }
    }
}