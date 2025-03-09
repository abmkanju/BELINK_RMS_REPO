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
using System.Transactions;
using PagedList;
using Pharmacy.UI.ShortCutDialogs;
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Data.SqlClient;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockReturnItem : Form
    {
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        private long StockReturnId = 0;
        private int SelectedItemId = 0;
        private string SelectedItemName = "";
        private int AvailableQty = 0;
        private int Unit;

        UnitOfWork unitOfWork;
        public frmStockReturnItem()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmStockReturnItem(long StockReturnId)
        {
            InitializeComponent();
            this.StockReturnId = StockReturnId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
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
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (this.ActiveControl.Name == grdItems.Name)
                {
                    if (grdItems.Rows != null && grdItems.Rows.Count > 0)
                    {
                        DataGridViewRow r = grdItems.SelectedRows[0];
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colReturnQuantity"];
                    }
                }
                else
                {
                    if (grdItems.Rows != null && grdItems.Rows.Count > 0)
                    {
                        this.ActiveControl = grdItems;
                        grdItems.Rows[0].Selected = true;
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colReturnQuantity"];
                        grdItems.CurrentCell.Selected = true;
                    }
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void frmStockReturnItem_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                SharedFunctions.SetGridStyle(this.grdItems);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave });
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                LoadDetails();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void LoadDetails()
        {
            if (this.StockReturnId > 0)
            {
                LoadEditDetail();
            }
            else
            {
                LoadDocument();
                LoadStockDocumentNo();
            }
        }

        private void LoadStockDocumentNo()
        {
            try
            {
                cmbStockNo.SelectedIndexChanged -= cmbStockNo_SelectedIndexChanged;
                Dictionary<long, string> stockList = new Dictionary<long, string>();
                using (unitOfWork = new UnitOfWork())
                {
                    stockList = unitOfWork.StockReturnRepository.GetStockDocumentNo();
                    stockList.Add(0, "Select Stock");
                    cmbStockNo.DataSource = new BindingSource(stockList.OrderByDescending(key => key.Value), null);
                    cmbStockNo.ValueMember = "Key";
                    cmbStockNo.DisplayMember = "Value";
                }
                lblInvoiceNo.Text = "N/A";
                lblSupplierName.Text = "N/A";
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                cmbStockNo.SelectedIndexChanged += cmbStockNo_SelectedIndexChanged;
            }
        }

        private void LoadEditDetail()
        {
            try
            {
                lblInvoiceNo.Text = "N/A";
                lblSupplierName.Text = "N/A";
                grdItems.Columns["colReturnAvailableQty"].Visible = true;
                cmbStockNo.SelectedIndexChanged -= cmbStockNo_SelectedIndexChanged;
                Dictionary<long, string> stockList = new Dictionary<long, string>();
                StockReturnVM s;
                using (unitOfWork = new UnitOfWork())
                {
                    s = unitOfWork.StockReturnRepository.GetStockReturnItemById(this.StockReturnId);
                    //lblInvoiceNo.Text = s.Stock.SupplierInvoiceNo;
                    lblSupplierName.Text = s.SupplierName;
                    if (s.Stock != null)
                    {
                        stockList.Add(s.Stock.StockId, s.DocumentNo.ToString());
                    }
                    else
                    {
                        stockList.Add(s.DocumentNo, s.DocumentNo.ToString());
                    }
                    cmbStockNo.DataSource = new BindingSource(stockList.OrderByDescending(key => key.Value), null);
                    cmbStockNo.ValueMember = "Key";
                    cmbStockNo.DisplayMember = "Value";
                    cmbStockNo.Enabled = false;
                    txtDocumentNo.Text = s.DocumentNo.ToString();
                    txtTotalDiscount.Text = s.NetDiscount.ToString();
                    txtTotalSaleTax.Text = s.NetSalesTax.ToString();

                    foreach (StockReturnItemVM i in s.StockReturnItems)
                    {
                        i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity; // convert quantity back and forth depending on unit.
                        i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                        //i.ReturnQuantity = i.Quantity;
                        SharedFunctions.CalculateStockBatchAvailableQty(i);
                        i.AvailableStock = i.Unit == 0 ? (i.AvailableStock + i.StockReturn) / i.Item.ConversionUnit : i.AvailableStock + i.StockReturn;

                        if (i.AvailableStock < 0)
                        {
                            //int PositveInt = System.Math.Abs(i.AvailableStock);
                            i.AvailableStock = 0;
                        }
                        i.StockReturn = i.Quantity;
                        i.ReturnAvailableQty = i.Quantity;
                        AddDataGridRow(i);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                cmbStockNo.SelectedIndexChanged += cmbStockNo_SelectedIndexChanged;
            }
        }

        private void LoadDocument()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    txtDocumentNo.Text = unitOfWork.StockReturnRepository.GetNewDocumentNumber().ToString();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void ShowNewDocumentNumber()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtDocumentNo.Text = unitOfWork.StockRepository.GetNewDocumentNumber().ToString();
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clear()
        {
            txtTotalSaleTax.Text = "";
            txtTotalDiscount.Text = "";
        }

        private void CalculateValues(DataGridViewRow r)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            double TotalCost = 0;
            int qty = SharedFunctions.StringToInt(r.Cells["colReturnQuantity"].Value);
            double UnitCost = SharedFunctions.StringToDouble(r.Cells["colUnitCost"].Value);
            double Disc = SharedFunctions.StringToDouble(r.Cells["colDiscount"].Value);
            double SalesTax = SharedFunctions.StringToDouble(r.Cells["colSalesTax"].Value);
            TotalCost = qty * UnitCost;
            string PercDiscType = r.Cells["colPercDiscType"].Value.ToString();
            string PercStType = r.Cells["colPercSaleStaxType"].Value.ToString();

            if (PercDiscType == "%")
            {
                Disc = (Disc / 100) * (qty * UnitCost);
            }
            if (PercStType == "%")
            {
                SalesTax = (SalesTax / 100) * (qty * UnitCost);
            }
            double netValue = TotalCost + (qty > 0 ? (-Disc + SalesTax) : 0);
            r.Cells["colNetValue"].Value = netValue;
            r.Cells["colDiscountValue"].Value = Disc;
            r.Cells["colSalesTaxValue"].Value = SalesTax;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    ///StockItemsList.Clear();
                    this.ShowNewDocumentNumber();
                    this.txtGrandTotal.Text = "0";
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some Items Before Saving", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) > Convert.ToInt32(r.Cells["colAvailableQty"].Value))
                    {
                        MessageBox.Show("Return quantity can not be greater then available quantity.", "Invaliud Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    //if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) < (this.StockReturnId > 0 ? Convert.ToInt32(r.Cells["colReturnAvailableQty"].Value) : -1))
                    //{
                    //    MessageBox.Show("Return quantity can not be less then to the Previous Returned quantity.", "Invaliud Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                }
                if (this.StockReturnId > 0)
                {
                    UpdateStock();
                }
                else
                {
                    InsertStock();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private double GetGrandTotal()
        {
            double gt = 0, dt = 0, st = 0;
            //----------Total Discount From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                dt += SharedFunctions.StringToDouble(r.Cells["colDiscountValue"].Value);
            }

            //----------Total Sales Tax  From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                st += SharedFunctions.StringToDouble(r.Cells["colSalesTaxValue"].Value);
            }
            //----------Grand Total From DataGrid-------------//
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                gt += Convert.ToDouble(r.Cells["colNetValue"].Value);
            }
            txtTotalDiscount.Text = dt.ToString();
            txtTotalSaleTax.Text = st.ToString();
            return gt;
        }


        private void grdItems_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void grdItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }

        private void cmbStockNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSupplierName.Text = "N/A";
            lblInvoiceNo.Text = "N/A";
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            grdItems.Rows.Clear();
            txtGrandTotal.Text = "0";
            txtTotalDiscount.Text = "0";
            txtTotalSaleTax.Text = "0";
            StockReturnVM s;
            if (SharedFunctions.StringToLong(cmbStockNo.SelectedValue) <= 0)
                return;
            using (unitOfWork = new UnitOfWork())
            {
                s = unitOfWork.StockReturnRepository.GetStockReturnById(SharedFunctions.StringToLong(cmbStockNo.SelectedValue));
            }
            if (s != null && s.Stock != null)
            {
                lblSupplierName.Text = s.SupplierName;
                lblInvoiceNo.Text = s.Stock.SupplierInvoiceNo;
            }
            foreach (StockReturnItemVM i in s.StockReturnItems)
            {
                i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity; // convert quantity back and forth depending on unit.
                i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                SharedFunctions.CalculateStockBatchAvailableQty(i);
                i.AvailableStock = i.Unit == 0 ? i.AvailableStock / i.Item.ConversionUnit : i.AvailableStock;

                if (i.AvailableStock < 0)
                {
                    i.AvailableStock = 0;
                }
                i.StockReturnItemId = 0;
                i.StockReturn = 0;
                i.NetValue = 0;
                i.ReturnAvailableQty = 0;
                i.DiscountValue = 0;
                i.SalesTaxValue = 0;
                AddDataGridRow(i);
            }
        }

        private void AddDataGridRow(StockReturnItemVM i)
        {
            grdItems.Rows.Add(
               0, //SrNo
               i.StockReturnItemId,
               i.Item.ItemID,
               i.Batch.BatchId,
               i.Item.ItemName,
               i.Unit == 0 ? "Pack" : "Units",
               i.Item.ConversionUnit,
               i.AvailableStock,
               i.StockReturn,
               i.ReturnAvailableQty,
               i.Batch.BatchName,
               i.UnitCost,
               i.Discount,
               i.DiscountType == 0 ? "%" : "Value",
               i.SalesTax,
               i.SalesTaxType == 0 ? "%" : "Value",
               i.NetValue,
               i.DiscountValue,
               i.SalesTaxValue);
        }

        #region DataGrid
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridViewRow r = grdItems.Rows[e.RowIndex];
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                LoadSerial();
                string value = grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value.ToString();
                double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) + NetValue).ToString();
                string value1 = grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value.ToString();
                grdItems.CellValueChanged += grdItems_CellValueChanged;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || e.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                )
            {
                int avlQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                int retQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value);
                int retAvaQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colReturnAvailableQty"].Value);
                if (this.StockReturnId > 0)
                {
                    if (retQty > avlQty)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                        MessageBox.Show("Return Quantity Can't be Greater than Available Quantity", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) < (this.StockReturnId > 0 ? Convert.ToInt32(r.Cells["colReturnAvailableQty"].Value) : -1))
                    //{
                    //    MessageBox.Show("Return quantity can not be less then to the Previous Returned quantity.", "Invaliud Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    if (retQty < 0)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                        MessageBox.Show("Return Quantity Can't be Less than the 0", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
                else
                {
                    if (retQty > avlQty)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                        MessageBox.Show("Return Quantity Can't be Greater than Available Quantity", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (retQty < 0)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                        MessageBox.Show("Return Quantity Can't be Less than the 0", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                CalculateValues(grdItems.Rows[e.RowIndex]);
                txtGrandTotal.Text = GetGrandTotal().ToString();
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                )
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                )
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }
        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        #endregion
        private DataTable get(string qry)
        {

            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;

        }
        private void InsertStock()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Random rr = new Random();
                        
                        StockReturn objStockReturn = new StockReturn();
                        if (txtDocumentNo.Text != string.Empty)
                        {
                            objStockReturn.DocumentNo = SharedFunctions.StringToLong(txtDocumentNo.Text);
                        }
                        else
                        {
                            objStockReturn.DocumentNo = Convert.ToInt64(rr.Next(9999,99999));
                        }

                        DataTable dt = get("Select Stock_StockId from StockItems where Batch_BatchId=" + SharedFunctions.StringToInt(grdItems.Rows[0].Cells["colBatchId"].Value) + " and Item_ItemID=" + SharedFunctions.StringToLong(grdItems.Rows[0].Cells["colItemId"].Value));
                        long stockid =0;
                        if (dt.Rows.Count != 0)
                        {
                            stockid = Convert.ToInt64(dt.Rows[0][0].ToString());
                        }
                        
                        objStockReturn.GrandTotal = SharedFunctions.StringToDouble(txtGrandTotal.Text);
                        objStockReturn.NetDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                        objStockReturn.NetSalesTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                        objStockReturn.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (cmbStockNo.SelectedIndex != 0 && cmbStockNo.SelectedIndex != -1)
                        {
                            objStockReturn.Stock = unitOfWork.StockRepository.GetById(SharedFunctions.StringToLong(cmbStockNo.SelectedValue));
                        }
                        else
                        {
                            objStockReturn.Stock = unitOfWork.StockRepository.GetById(SharedFunctions.StringToLong(stockid));
                        }
                        objStockReturn.IsNew = true;
                        objStockReturn.IsActive = true;
                        objStockReturn.CreateDate = DateTime.Now;
                        objStockReturn.UpdateDate = DateTime.Now;
                        objStockReturn.StockReturnItems = new List<StockReturnItem>();
                        List<int> itemIds = new List<int>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            StockReturnItem objStockReturnItem = new StockReturnItem();
                            FillObject(r, objStockReturnItem);
                            //-------- Add Check
                            Item objItem = unitOfWork.ItemRspository.GetById(objStockReturnItem.Item.ItemID);
                            if (!objItem.IsSyncable)
                            {
                                objItem.IsSyncable = true;
                            }
                            else if (objItem.IsSynced)
                            {
                                objItem.IsNew = false;
                                objItem.IsUpdate = true;
                                objItem.IsSynced = false;
                            }

                            objItem.UpdateDate = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(objItem).State = System.Data.Entity.EntityState.Modified;
                            objStockReturn.StockReturnItems.Add(objStockReturnItem);
                        }
                        unitOfWork.StockReturnRepository.Insert(objStockReturn);
                        AuditLogTask.CreateAuditLog(unitOfWork, objStockReturn.DocumentNo.ToString(), AuditModule.Stock_Return, AuditAction.created);
                        unitOfWork.Save();

                        //SOT
                        //OfflineJsonTask.SaveOfflineTask(unitOfWork, objStockReturn, ModuleName.StockReturn, ModuleAction.Create, objStockReturn.StockReturnId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception("Error occurred while inserting stock, please try again.");
                }
            }
            MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SharedVariables.manageStockReturnRef = true;
            this.Close();
        }

        private void UpdateStock()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Random rr = new Random();
                StockReturn objStockReturn = unitOfWork.StockReturnRepository.GetStockReturnByIdEdit(this.StockReturnId);
                objStockReturn.UpdateDate = DateTime.Now;
                if (objStockReturn.IsSynced)
                {
                    objStockReturn.IsNew = false;
                    objStockReturn.IsUpdate = true;
                    objStockReturn.IsSynced = false;
                }
                if (txtDocumentNo.Text != string.Empty)
                {
                    objStockReturn.DocumentNo = SharedFunctions.StringToLong(txtDocumentNo.Text);
                }
                else
                {
                    objStockReturn.DocumentNo = Convert.ToInt64(rr.Next(9999, 99999));
                }
                objStockReturn.GrandTotal = SharedFunctions.StringToDouble(txtGrandTotal.Text);
                objStockReturn.NetDiscount = SharedFunctions.StringToDouble(txtTotalDiscount.Text);
                objStockReturn.NetSalesTax = SharedFunctions.StringToDouble(txtTotalSaleTax.Text);
                objStockReturn.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                if (cmbStockNo.SelectedIndex != -1)
                {
                    objStockReturn.Stock = unitOfWork.StockRepository.GetById(SharedFunctions.StringToLong(cmbStockNo.SelectedValue));
                }
                else
                {
                    objStockReturn.Stock = unitOfWork.StockRepository.GetById(SharedFunctions.StringToLong(txtDocumentNo.Text));
                }
                objStockReturn.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    StockReturnItem objStockReturnItem = new StockReturnItem();
                    objStockReturnItem.StockReturnItemId = SharedFunctions.StringToLong(r.Cells["colStockIReturnId"].Value);
                    string checkStockReturn = (r.Cells["colStockIReturnId"].Value).ToString();

                    FillObject(r, objStockReturnItem);
                    foreach (StockReturnItem model in objStockReturn.StockReturnItems)
                    {
                        if (model.StockReturnItemId == objStockReturnItem.StockReturnItemId)
                        {
                            model.StockReturnItemId = objStockReturnItem.StockReturnItemId;
                            model.Unit = objStockReturnItem.Unit;
                            model.Item = objStockReturnItem.Item;
                            model.Batch = objStockReturnItem.Batch;
                            model.NetValue = objStockReturnItem.NetValue;
                            model.Quantity = objStockReturnItem.Quantity;
                            model.UnitCost = objStockReturnItem.UnitCost;
                            model.Discount = objStockReturnItem.Discount;
                            model.DiscountType = objStockReturnItem.DiscountType;
                            model.DiscountValue = objStockReturnItem.DiscountValue;
                            model.SalesTax = objStockReturnItem.SalesTax;
                            model.SalesTaxType = objStockReturnItem.SalesTaxType;
                            model.SalesTaxValue = objStockReturnItem.SalesTaxValue;
                            unitOfWork.GetDbContext().Entry(model).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    //if items added more
                    if (checkStockReturn == "newAdd")
                    {
                        objStockReturnItem.StockReturnItemId = -1;
                        objStockReturn.StockReturnItems.Add(objStockReturnItem);
                    }
                }

                unitOfWork.StockReturnRepository.Update(objStockReturn);
                AuditLogTask.CreateAuditLog(unitOfWork, objStockReturn.DocumentNo.ToString(), AuditModule.Stock_Return, AuditAction.updated);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objStockReturn, ModuleName.StockReturn, ModuleAction.Update, objStockReturn.StockReturnId);
            }
            MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SharedVariables.manageStockReturnRef = true;
            this.Close();
        }
        private void FillObject(DataGridViewRow r, StockReturnItem obj)
        {
            long ItemId = SharedFunctions.StringToLong(r.Cells["colItemId"].Value);
            int BatchId = SharedFunctions.StringToInt(r.Cells["colBatchId"].Value);

            int ConversionUnit = SharedFunctions.StringToInt(r.Cells["colConvUnit"].Value);
            obj.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            Batch objBatch = unitOfWork.BatchRepository.FindItemBatch(BatchId, ItemId);
            obj.Item = unitOfWork.ItemRspository.GetById(ItemId);
            obj.Batch = objBatch;

            //if (obj.StockItemId <= 0) // don't get item from db when stock is being updated, because item updation is not allowed
            obj.Quantity = SharedFunctions.StringToInt(r.Cells["colReturnQuantity"].Value);
            obj.Quantity = obj.Unit == 0 ? obj.Quantity * ConversionUnit : obj.Quantity; // to save quantity always in broken form.

            obj.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
            obj.UnitCost = obj.Unit == 0 ? obj.UnitCost / ConversionUnit : obj.UnitCost; // to save quantity always in broken form.

            obj.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            obj.DiscountType = r.Cells["colPercDiscType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.DiscountValue = Convert.ToDouble(r.Cells["colDiscountValue"].Value);

            obj.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            obj.SalesTaxType = r.Cells["colPercSalesTaxType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.SalesTaxValue = Convert.ToDouble(r.Cells["colSalesTaxValue"].Value);

            obj.NetValue = Convert.ToDouble(r.Cells["colNetValue"].Value);
        }

        private bool IsItemAlreadyAdded(int ItemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsSameItemDiffBatches)
                return false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            return false;
        }

        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            comboSelectBatch.SelectedIndex = -1;
            if (this.SelectedItemId > 0)
            {
                GetItemBatches(this.SelectedItemId);
            }
        }

        private void GetItemBatches(int ItemId)
        {
            comboSelectBatch.SelectedIndexChanged -= comboSelectBatch_SelectedIndexChanged;
            using (unitOfWork = new UnitOfWork())
            {
                BatchStockList = unitOfWork.ItemRspository.GetBatchStockReturnByItemId(ItemId);
            }
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            foreach (BatchStockVM i in BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);
                i.Name = i.BatchName;
                i.AvailableStockQty = i.AvailableStock;
                i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
                if (i.AvailableStock > 0)
                {
                    FinalResult.Add(i);
                }
            }
            comboSelectBatch.DataSource = FinalResult;
            comboSelectBatch.ValueMember = "BatchId";
            comboSelectBatch.DisplayMember = "BatchName";
            comboSelectBatch.SelectedIndexChanged += comboSelectBatch_SelectedIndexChanged;
            comboSelectBatch.SelectedIndex = -1;
            if (this.comboSelectBatch.Items.Count > 0)
            {
                comboSelectBatch.SelectedIndex = 0;
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (uC_SearchItems1.SelectedItemId == 0)
                {
                    MessageBox.Show("Item must be Selected", "Must select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbStockNo.SelectedIndex == 0)
                {
                    MessageBox.Show("Stock No must be Selected", "Must select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //else if (comboSelectBatch.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Item Batch must be Selected", "Must select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                //else if (cmbStockNo.Text == "Select Stock" || cmbStockNo.Text == "")
                //{
                //    MessageBox.Show("Stock must be selected First", "Must select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                //get BatchInfomation
                System.Reflection.PropertyInfo getId = comboSelectBatch.SelectedItem.GetType().GetProperty("BatchId");
                string BatchId = getId.GetValue(comboSelectBatch.SelectedItem, null).ToString();

                System.Reflection.PropertyInfo getName = comboSelectBatch.SelectedItem.GetType().GetProperty("Name");
                string BatchName = getName.GetValue(comboSelectBatch.SelectedItem, null).ToString();

                System.Reflection.PropertyInfo getStock = comboSelectBatch.SelectedItem.GetType().GetProperty("AvailableStock");
                string AvailableQty = getStock.GetValue(comboSelectBatch.SelectedItem, null).ToString();

                if (this.SelectedItemId > 0)
                {
                    if (!IsItemAlreadyAdded(this.SelectedItemId))
                    {
                        ItemsVM vm = new ItemsVM();

                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQty(this.SelectedItemId);
                        }

                        //getValues
                        //vm.UnitCost = Unit == 0 ? vm.UnitCost * vm.ConversionUnit : vm.UnitCost;
                        vm.AvailableStock = Convert.ToInt32(AvailableQty);
                        vm.AvailableStock = Unit == 0 ? vm.AvailableStock / vm.ConversionUnit : vm.AvailableStock;
                        if (Unit == 0) { vm.Unit = "Pack"; }
                        else { vm.Unit = "Units"; }

                        grdItems.Rows.Add(
                            0,//SrNo
                           "newAdd",
                            vm.ItemId,
                            BatchId,
                            vm.ItemName, //objStockReturnItem.ItemName,
                            vm.Unit, //objStockReturnItem.UnitString,
                            vm.ConversionUnit, //objStockReturnItem.ConversionUnit,
                            vm.AvailableStock, // available quantity
                            0, //Return quantity
                            0,
                            BatchName,
                            vm.UnitCost * vm.ConversionUnit, //objStockReturnItem.UnitCost,
                            0, //objStockReturnItem.Discount,
                            "%",//Percent discout type
                            0, //objStockReturnItem.SalesTax,
                            "%", // percent Sales Tax Type
                            0,
                            vm.UnitCost * vm.ConversionUnit * 0, //0, //objStockReturnItem.NetValue,
                            0, //objStockReturnItem.DiscountValue,
                            0 //objStockReturnItem.SalesTaxValue
                            );
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
            finally
            {
                txtGrandTotal.Text = GetGrandTotal().ToString();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void LoadSerial()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                grdItems.Rows[row.Index].Cells["colSrNo"].Value = string.Format("{0}  ", row.Index + 1).ToString();
            }
        }

        private void btn_Clear(object sender, EventArgs e)
        {
            comboSelectBatch.SelectedIndex = -1;
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = "";
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void comboSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboSelectBatch.SelectedIndex >= 0)
                {
                    BatchStockVM r = BatchStockList.Where(b => b.BatchId == Convert.ToInt64(comboSelectBatch.SelectedValue)).FirstOrDefault();
                    if (r != null)
                    {
                        int avlQty = (r.TotalStock + r.AdjustedStock - r.ConsumedStock - r.ExpiredStock);
                        AvailableQty = avlQty;
                        Unit = r.Unit;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}