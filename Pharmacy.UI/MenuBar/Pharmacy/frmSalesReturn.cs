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
using System.Transactions;
using Pharmacy.Core;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using PagedList;
using Pharmacy.UI.ManageOfflineTask;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmSalesReturn : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        bool deductChange = false;
        ItemsRepository repItems = new ItemsRepository(cxt);
        InvoiceRepository repInvoice = new InvoiceRepository(cxt);        
        UnitOfWork unitOfWork = new UnitOfWork();
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        PatientRepository repPatients = new PatientRepository(cxt);
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int pageNoPatients = 1;
        bool IsNameSearching, IsPhoneSearching, lineitemdisc = false;
        Invoice objInvoice = new Invoice();
        DataTable dtStockConsumption = new DataTable();
        long PatientId = 0;
        long InvoiceId = 0;
        private int SelectedItemId = 0;

        public frmSalesReturn()
        {
            InitializeComponent();
            objInvoice.InvoiceItems = new List<InvoiceItem>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            //this.uC_SearchItems1.Select();
            //this.uC_SearchItems1.Focus();
        }
        public frmSalesReturn(long InvoiceId)
        {
            InitializeComponent();
            objInvoice.InvoiceItems = new List<InvoiceItem>();
            this.InvoiceId = InvoiceId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.Focus();
        }

        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(this.grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[]{ btnRefreshForm});
            LoadItems();
            fillInvoiceNumbers();
            cmbTotalDeductType.SelectedIndex = 0;
            SharedFunctions.GenerateStockConsumptionTable(dtStockConsumption);
            this.uC_SearchItems1.Focus();
        }              
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            if (this.SelectedItemId <= 0)
            {
                btnClear.PerformClick();
                return;
            }
            btnAddItem.PerformClick();
        }
        private void fillInvoiceNumbers()
        {
            DataTable dty = get("SELECT DISTINCT dbo.Invoices.InvoiceRefNo FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 and dbo.Invoices.IsQInvoice = 0 group by dbo.Invoices.InvoiceRefNo order by dbo.Invoices.InvoiceRefNo desc");
            if (dty.Rows.Count > 0)
            {
                cmbInvoices.DataSource = dty;
                cmbInvoices.ValueMember = "InvoiceRefNo";
                cmbInvoices.DisplayMember = "InvoiceRefNo";
            }
        }
        private void LoadInvoice(long InvoiceId)
        {
            InvoiceVM objInvoice = repInvoice.GetInvoiceByInvoiceId(InvoiceId);
            this.PatientId = objInvoice.Patient.PatientId;
            foreach(InvoiceItemVM i in objInvoice.InvoiceItems)
            {

            }
        }
        private void LoadItems()
        {
            List<Item> items = repItems.GetAll().ToList();
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (this.SelectedItemId <= 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                this.uC_SearchItems1.Focus();
                return false;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (!ErrFound)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                int btch = 0;
                string batchname = "";
                if (!SharedVariables.AdminPharmacySetting.AllowAddingSameItemwithDifferentIvoice)
                {
                    //---------Item Already Exists----------//
                    if (ItemAlreadyAdded(this.SelectedItemId))
                    {
                        MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        uC_SearchItems1.SetFocus();
                        return;
                    }
                }
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + SelectedItemId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + SelectedItemId);
                if (dt.Rows.Count > 0)
                {
                    btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                    DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                    batchname = batch.Rows[0][0].ToString();
                }
                grdItems.Rows.Add(
                                0,
                                0,
                               this.SelectedItemId,//itemid
                               "",//itemname
                               "",//btachid
                               "", //batch
                               "",//unit
                               0, //InvoiceIDs
                               "",//coUnit
                               0,//Rate
                               0, //qty
                               0,//return qty
                               0,//refund qty
                               0,//deduction
                               "%", //deduction type
                               true,//is new row
                               0,
                               0,
                               ""
                               );
                Clear();
                this.uC_SearchItems1.Focus();
                this.uC_SearchItems1.SetText = "";
                grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colReturnQuantity"];
                grdItems.BeginEdit(true);
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                try
                {
                    grdItems.Rows.RemoveAt(e.RowIndex);
                    numTotalDeduct.Enabled = true;
                    cmbTotalDeductType.Enabled = true;
                }
                catch (Exception ex)
                { }
            }
        }
        private void checknullReturnQty()
        {
            DataGridViewRowCollection dgv = grdItems.Rows;
            foreach (DataGridViewRow ch in dgv)
            {
                if (Convert.ToInt32(ch.Cells["colQuanitycheck"].Value.ToString()) < 1)
                {
                    grdItems.Rows.RemoveAt(ch.Index);
                }
            }
        }
        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            try
            {
                this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                this.uC_SearchItems1.SetText = "";
                this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                this.SelectedItemId = 0;
                // this line must be called at end of clear method.
                this.uC_SearchItems1.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int userId = SharedVariables.LoggedInUser.UserId;
            int ret = SaveSalesReturn(userId);

            if (ret == 1)
            {
                ClearAll(true);
            }
        }
        public bool checkTabforMultipleItems()
        {
            int newVal = 1;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                for (var i = newVal; i < grdItems.Rows.Count; i++)
                {
                    if (Convert.ToString(grdItems.Rows[i].Cells["colItemId"].Value) == Convert.ToString(r.Cells["colItemId"].Value)
                        && Convert.ToString(grdItems.Rows[i].Cells["colInvoiceId"].Value) == Convert.ToString(r.Cells["colInvoiceId"].Value))
                    {
                        MessageBox.Show("Please select different Invoice Number of same item added", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                newVal++;
            }

            return true;
        }
        public int SaveSalesReturn(int UserId)
        {
            double Totaldeduction = 0;
            double Totaldeduction1 = 0;
            int QtyUpdate = 0;
            //validation
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Can't Save Empty Sale Return, ", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            if (!checkTabforMultipleItems())
            {
                return 0;
            }

            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                        SalesReturn objSalesRet = new SalesReturn();
                        objSalesRet.CreateDate = DateTime.Now;
                        objSalesRet.SubTotal = double.Parse(lblSubTotal.Text);
                        objSalesRet.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objSalesRet.TotalDeductionValue = Convert.ToDouble(numTotalDeduct.Value);
                        objSalesRet.TotalDeductionType = cmbTotalDeductType.GetItemText(cmbTotalDeductType.SelectedItem).ToLower() == "value" ? 1 : 0;
                        objSalesRet.SalesReturnItem = new List<SalesReturnItem>();
                        objSalesRet.UpdateDate = DateTime.Now;
                        objSalesRet.IsActive = true;
                        objSalesRet.User = unitOfWork.UserRepository.GetById(UserId);
                        Totaldeduction = objSalesRet.TotalDeductionType == 0 ? (objSalesRet.TotalDeductionValue/100)* objSalesRet.SubTotal : objSalesRet.TotalDeductionValue;
                        foreach (DataGridViewRow row in grdItems.Rows)
                        {
                            Totaldeduction1 += Convert.ToDouble(row.Cells["colDeduction"].Value.ToString());
                        }
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            SalesReturnItem objSalesRetItem = new SalesReturnItem();
                            ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                            if (Totaldeduction1 == 0 && numTotalDeduct.Value > 0)
                            {
                                grdItems.CellValueChanged -= grdItems_CellValueChanged_1;
                                Totaldeduction = Math.Round((Totaldeduction / grdItems.Rows.Count), 2);
                                r.Cells["colRefundAmount"].Value = (Convert.ToDouble(r.Cells["colRefundAmount"].Value.ToString()) - Totaldeduction).ToString();
                                Totaldeduction = objSalesRet.TotalDeductionType == 0 ? (objSalesRet.TotalDeductionValue / 100) * objSalesRet.SubTotal : objSalesRet.TotalDeductionValue;
                            }
                            FillObject(objSalesRetItem, r);
                            int InvoiceId = 0;
                            
                                InvoiceId = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(r.Cells["colInvoiceId"].Value));
                            
                            if(InvoiceId == 0)
                            {
                                MessageBox.Show("Invoice for The Current Sale Return Item is Not Found?", "Invoice Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return 0;
                            }
                            objSalesRetItem.InvoiceRefNo = InvoiceId;
                            QtyUpdate = objSalesRetItem.ReturnQuantity;
                            //int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                            //int BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);

                            if (objSalesRetItem.ReturnQuantity == 0)
                            {
                                MessageBox.Show("Some items have 0 Return Quantity.", "Return Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }

                            if (objSalesRetItem.BatchId <= 0)
                            {
                                MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            else
                            {
                                    objSalesRetItem.Batch = unitOfWork.BatchRepository.GetById(objSalesRetItem.BatchId);
                                
                            }
                               objSalesRetItem.Item = unitOfWork.ItemRspository.GetById(objSalesRetItem.ItemId);

                                unitOfWork.GetDbContext().Entry(objSalesRetItem.Item).State = System.Data.Entity.EntityState.Modified;
                            
                            objSalesRetItem.CreateDate = DateTime.Now;
                            objSalesRetItem.UpdateDate = DateTime.Now;

                            objSalesRet.SalesReturnItem.Add(objSalesRetItem);

                            //foreach (SalesReturnItem s in objSalesRet.SalesReturnItem)
                            //{
                            //    if (s.Item.ItemID == ItemId && Convert.ToInt32(s.Batch.BatchId) == BatchId && s.IsActive)
                            //    {
                            //        s.Quantity = NewQty;
                            //        s.UpdateDate = DateTime.Now;
                            //        if (s.IsSynced)
                            //        {
                            //            s.IsNew = false;
                            //            s.IsUpdate = true;
                            //            s.IsSynced = false;
                            //        }
                            //    }
                            //    unitOfWork.GetDbContext().Entry(s).State = System.Data.Entity.EntityState.Modified;
                            //}
                            #region Record Profit Statement Transaction
                            if (!IsTotalDeductionNotFound())
                            {
                                int unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                                Transaction.CreatedAt = DateTime.Now;
                                Transaction.TransactionDate = DateTime.Now;
                                Transaction.TransactionDiscountedCostOfSale = (-1) * Convert.ToDouble(r.Cells["colDiscCost"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                Transaction.TransactionCostOfSales = unit == 1 ? (-1) * Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString()) : (-1) * (Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colConversionUnit"].Value)) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                Transaction.TransactionDiscount = 0;
                                Transaction.TransactionRevenue = (-1 * objSalesRetItem.RefundAmount);
                                Transaction.TransactionReturn = (objSalesRetItem.RefundAmount);
                                Transaction.User = SharedVariables.LoggedInUser.UserId;
                                Transaction.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value.ToString());
                                Transaction.IsDeleted = false;
                                Transaction.InvoiceId = Convert.ToInt64(r.Cells["colInvoiceId"].Value.ToString());
                                objTransaction.Add(Transaction);
                            }
                            else
                            {
                                double Deduction = ConvertPercToValue(Convert.ToDouble(numTotalDeduct.Value), Convert.ToDouble(lblSubTotal.Text) , cmbTotalDeductType.SelectedIndex);
                                int sumofQty = getSumofQty();
                                double deductionvalue = Deduction / sumofQty;
                                int unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                                Transaction.CreatedAt = DateTime.Now;
                                Transaction.TransactionDate = DateTime.Now;
                                Transaction.TransactionDiscountedCostOfSale = (-1) * Convert.ToDouble(r.Cells["colDiscCost"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                Transaction.TransactionCostOfSales = unit == 1 ? (-1) * Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString()) : (-1) * (Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colConversionUnit"].Value)) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                Transaction.TransactionRevenue = (-1) * (Convert.ToDouble(r.Cells["colRefundAmount"].Value.ToString()) - deductionvalue* Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString()));
                                Transaction.TransactionReturn = (objSalesRetItem.RefundAmount);
                                Transaction.IsDeleted = false;
                                Transaction.User = SharedVariables.LoggedInUser.UserId;
                                Transaction.ItemId =
                                Transaction.InvoiceId = Convert.ToInt64(r.Cells["colInvoiceId"].Value.ToString());
                                int invoiceunit = Convert.ToInt32(r.Cells["colInvoiceUnit"].Value.ToString());
                                int currentunit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                                double returnqty = 0;
                                if (invoiceunit == currentunit)
                                {
                                    returnqty = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                                    if (currentunit == 0)
                                    {
                                        returnqty = returnqty * Convert.ToInt32(r.Cells["colConversionUnit"].Value);
                                    }
                                }
                                else
                                {
                                    if (invoiceunit == 0)
                                    {
                                        returnqty = Math.Round(Convert.ToDouble(r.Cells["colReturnQuantity"].Value), 1);
                                    }
                                    else
                                    {
                                        returnqty = Math.Round(Convert.ToDouble(r.Cells["colReturnQuantity"].Value) * Convert.ToInt32(r.Cells["colConversionUnit"].Value), 1);
                                    }
                                }
                                var DiscType = r.Cells["colDeductionType"].Value.ToString();
                                var DeductedDisc = (r.Cells["colDeductedDiscount"].Value == null && r.Cells["colDeductedDiscount"].Value == "") ? 0 : Convert.ToDouble(r.Cells["colDeductedDiscount"].Value);
                                double ModifiedDiscount = 0;
                                double Disc = 0;
                                var Rate = (r.Cells["colRate"].Value.ToString() == "") ? 0 : Convert.ToDouble(r.Cells["colRate"].Value);
                                if (DiscType == "%")
                                {
                                    if (currentunit == 0)
                                    {
                                        Disc = DeductedDisc;
                                        ModifiedDiscount = (DeductedDisc / 100) * (returnqty / Convert.ToInt32(r.Cells["colConversionUnit"].Value)) * Rate;

                                    }
                                    else
                                    {
                                        Disc = DeductedDisc;
                                        ModifiedDiscount = (DeductedDisc / 100) * returnqty * Rate;

                                    }

                                }
                                else if (DiscType == "Value")
                                {
                                    //if (currentunit == 0)
                                    //{

                                    //}
                                    //else
                                    //{
                                    var orignalDisc = (r.Cells["colDiscount"].Value.ToString() == "") ? 0 : Convert.ToDouble(r.Cells["colDiscount"].Value);
                                    Disc = orignalDisc - DeductedDisc;
                                    ModifiedDiscount = DeductedDisc;

                                    //}
                                }
                                Transaction.TransactionDiscount = (-1)* ModifiedDiscount;

                                objTransaction.Add(Transaction);
                            }
                            #endregion
                        }
                        unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                        unitOfWork.Save();
                        //add Patient
                        if (txtSearchByName.Text.Trim() != "")
                        {
                            Patient newP = new Patient();
                            newP.User = unitOfWork.UserRepository.GetById(UserId);
                            newP.MRNo = unitOfWork.PatientRepository.GetNewMrNo();
                            newP.Name = txtSearchByName.Text.Trim();
                            newP.Phone = string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) ? "0000000000" : txtSearchByPhone.Text.Trim();
                            newP.Address = txtAddress.Text.Trim();
                            newP.Gender = "other";
                            newP.Email = "";
                            newP.Phone2 = "";
                            newP.AgeYears = 0;
                            newP.AgeMonths = 0;
                            newP.AgeDays = 0;
                            newP.CreateDate = DateTime.Now;
                            newP.UpdateDate = DateTime.Now;
                            newP.RegistrationDate = DateTime.Now;
                            newP.IsActive = true;
                            newP.IsNew = true;
                            objSalesRet.Patient = newP;
                        }

                        unitOfWork.SalesReturnRepository.Insert(objSalesRet);
                        objSalesRet.SaleReturnRefNo = unitOfWork.SalesReturnRepository.GetSalesReturnRefNo();
                        unitOfWork.Save();
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objSalesRet, ModuleName.SalesReturn, ModuleAction.Create, objSalesRet.SaleReturnId);
                        
                        //update Item
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            int a = 0;
                            string unit = r.Cells["colUnit"].Value.ToString();
                            using (unitOfWork = new UnitOfWork())
                            {
                                a = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(r.Cells["colInvoiceId"].Value));
                            }
                            double refund = Convert.ToDouble(r.Cells["colRefundAmount"].Value.ToString());
                            string DeductionnType = r.Cells["colDeductionType"].Value.ToString();
                            if (DeductionnType == "%")
                            {
                                refund = refund - ((Convert.ToDouble(r.Cells["colDeduction"].Value.ToString()) / 100) * refund);
                            }
                            else if (DeductionnType == "Value")
                            {
                                refund = refund - Convert.ToDouble(r.Cells["colDeduction"].Value.ToString());
                            }

                            double TotalCost = 0;
                            double TotalDiscCost = 0;
                            if (unit.ToLower() == "pack")
                            {
                                TotalCost = Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                TotalDiscCost = Convert.ToDouble(r.Cells["colDiscCost"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                TotalCost = TotalCost * Convert.ToInt32(r.Cells["colConversionUnit"].Value.ToString());
                            }
                            else
                            {
                                TotalCost = Convert.ToDouble(r.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                TotalDiscCost = Convert.ToDouble(r.Cells["colDiscCost"].Value.ToString()) * Convert.ToInt32(r.Cells["colReturnQuantity"].Value.ToString());
                                //TotalDiscCost = TotalDiscCost / Convert.ToInt32(r.Cells["colConversionUnit"].Value.ToString());
                            }
                            int invoiceunit = Convert.ToInt32(r.Cells["colInvoiceUnit"].Value.ToString());
                            int currentunit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                            double returnqty = 0;
                            if (invoiceunit == currentunit)
                            {
                                returnqty = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                                if (currentunit == 0)
                                {
                                    returnqty = returnqty * Convert.ToInt32(r.Cells["colConversionUnit"].Value);
                                }
                            }
                            else
                            {
                                if (invoiceunit == 0)
                                {
                                    returnqty = Math.Round(Convert.ToDouble(r.Cells["colReturnQuantity"].Value), 1);
                                }
                                else
                                {
                                    returnqty = Math.Round(Convert.ToDouble(r.Cells["colReturnQuantity"].Value) * Convert.ToInt32(r.Cells["colConversionUnit"].Value), 1);
                                }
                            }
                            //if(Totaldeduction1 == 0 && numTotalDeduct.Value > 0)
                            //{
                            //    Totaldeduction = Math.Round((Totaldeduction / grdItems.Rows.Count) , 2);
                            //    refund = refund - Totaldeduction;
                            //    Totaldeduction = objSalesRet.TotalDeductionType == 0 ? (objSalesRet.TotalDeductionValue / 100) * objSalesRet.SubTotal : objSalesRet.TotalDeductionValue;
                            //}
                            var DiscType = r.Cells["colDeductionType"].Value.ToString();
                            var DeductedDisc = (r.Cells["colDeductedDiscount"].Value == null && r.Cells["colDeductedDiscount"].Value == "") ? 0 : Convert.ToDouble(r.Cells["colDeductedDiscount"].Value);
                            double ModifiedDiscount = 0;
                            double Disc = 0;
                             var Rate = (r.Cells["colRate"].Value.ToString() == "")? 0 :Convert.ToDouble(r.Cells["colRate"].Value);
                            if (DiscType == "%")
                            {
                                if (currentunit == 0)
                                {
                                    Disc = DeductedDisc;
                                    ModifiedDiscount = (DeductedDisc / 100) * (returnqty/ Convert.ToInt32(r.Cells["colConversionUnit"].Value)) * Rate;

                                }
                                else
                                {
                                    Disc = DeductedDisc;
                                    ModifiedDiscount = (DeductedDisc / 100) * returnqty * Rate;

                                }

                            }
                            else if (DiscType == "Value")
                            {
                                //if (currentunit == 0)
                                //{

                                //}
                                //else
                                //{
                                    var orignalDisc = (r.Cells["colDiscount"].Value.ToString() == "") ? 0 : Convert.ToDouble(r.Cells["colDiscount"].Value);
                                    Disc = orignalDisc;
                                    ModifiedDiscount = DeductedDisc;

                                //}
                            }
                            UpdateStock(Convert.ToInt32(r.Cells["colItemId"].Value), Convert.ToInt32(r.Cells["colBatchId"].Value), returnqty, Convert.ToInt32(r.Cells["colConversionUnit"].Value), unit, a);
                            UpdateInvoice1(a, returnqty, unit, Convert.ToInt32(r.Cells["colConversionUnit"].Value), Convert.ToInt32(r.Cells["colItemId"].Value), refund, TotalCost, TotalDiscCost, Convert.ToString(r.Cells["colPaymentType"].Value), Disc, ModifiedDiscount, r.Cells["colDeductionType"].Value.ToString());

                        }
                        scope.Complete();

                        MessageBox.Show("Sales Return Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                    }
                    else
                    {
                        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    }
                    //scope.Dispose();
                    return 0;
                }
                return 1;
            }
        }
        private bool IsTotalDeductionNotFound()
        {
            bool Found = false;
            foreach(DataGridViewRow row in grdItems.Rows)
            {
                if(Convert.ToInt32(row.Cells["colDeduction"].Value) > 0)
                {
                    Found = true;
                }
            }

            return Found;
        }
        private int getSumofQty()
        {
            int qty = 0;
            foreach(DataGridViewRow row in grdItems.Rows)
            {
                qty += Convert.ToInt32(row.Cells["colReturnQuantity"].Value);
            }
            return qty;
        }
        public void UpdateInvoice1(int InvoiceItemId, double RetQty, string unit, int ConvUnit , int itemid , double refund , double TotalCost , double TotalDisCost, string PaymentType, double Discount = 0.0, double ModifiedDiscount = 0.0,  string DiscType = "")
        {
            
                int i = updateDB("Insert Into InvoicePayments(PaymentType, Payment , RefundReason , PaymentDate , CreatedAt , UpdatedAt , IsActive , Invoice_InvoiceId , User_UserId , IsNew , IsUpdate , IsSynced , MethodType , Dues , IsQInvoice) Values(" + PaymentType + "," + -1 * refund + ",'" + " " + "' , '" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "'," + 1 + "," + InvoiceItemId + "," + SharedVariables.LoggedInUser.UserId + "," + 1 + "," + 0 + "," + 0 + "," + 1 + "," + 0  +  "," +  "0" + ")");

            if (lineitemdisc)
            {
                int k = updateDB("Update Invoices set TotalPaid = TotalPaid-" + refund + " , UpdateDate='" + DateTime.Now.ToString() + "' , TotalUnitCost=TotalUnitCost-" + TotalCost + ", ModifiedDiscount=ModifiedDiscount-" + ModifiedDiscount + ", TotalDiscount=TotalDiscount-" + ModifiedDiscount + " where InvoiceId =" + InvoiceItemId);
                int j = updateDB("Update InvoiceItems set ReturnedQuantity= ReturnedQuantity + " + RetQty + ", TotalCostPrice=TotalCostPrice-" + TotalCost + " , TotalDiscPrice=TotalDiscPrice-" + TotalDisCost + " ,Discount=" + Discount + " , UpdateDate='" + DateTime.Now.ToString() + "' where Invoice_InvoiceId =" + InvoiceItemId + " and Item_ItemID=" + itemid);
            }
            else
            {
                if(DiscType == "Value")
                {
                    int k = updateDB("Update Invoices set TotalPaid = TotalPaid-" + refund + " , UpdateDate='" + DateTime.Now.ToString() + "' , TotalUnitCost=TotalUnitCost-" + TotalCost + ", ModifiedDiscount=ModifiedDiscount-" + ModifiedDiscount + " where InvoiceId =" + InvoiceItemId);
                    int j = updateDB("Update InvoiceItems set ReturnedQuantity= ReturnedQuantity + " + RetQty + ", TotalCostPrice=TotalCostPrice-" + TotalCost + " , TotalDiscPrice=TotalDiscPrice-" + TotalDisCost + ", UpdateDate='" + DateTime.Now.ToString() + "' where Invoice_InvoiceId =" + InvoiceItemId + " and Item_ItemID=" + itemid);
                }
                else
                {
                    int k = updateDB("Update Invoices set TotalPaid = TotalPaid-" + refund + " , UpdateDate='" + DateTime.Now.ToString() + "' , TotalUnitCost=TotalUnitCost-" + TotalCost + ", ModifiedDiscount=ModifiedDiscount-" + ModifiedDiscount + " where InvoiceId =" + InvoiceItemId);
                    int j = updateDB("Update InvoiceItems set ReturnedQuantity= ReturnedQuantity + " + RetQty + ", TotalCostPrice=TotalCostPrice-" + TotalCost + " , TotalDiscPrice=TotalDiscPrice-" + TotalDisCost + ", UpdateDate='" + DateTime.Now.ToString() + "' where Invoice_InvoiceId =" + InvoiceItemId + " and Item_ItemID=" + itemid);
                }

            }

        }
        public void UpdateStock(int ItemID, int BatchID, double RetQty, int ConvUnit, string unit , int invId)
        {
            double Quantity = 0;
            DataTable dt = get("Select Quantity from StockItems where Item_ItemID=" + ItemID + "And Batch_BatchId=" + BatchID);
            if (dt.Rows.Count > 0)
            {
                Quantity = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][0].ToString());
                //RetQty = unit.ToLower() == "pack" ? RetQty * ConvUnit : RetQty;
                Quantity = Quantity + RetQty;
            }
            //int i = updateDB("Update StockItems set Quantity = " + Quantity + " where Item_ItemID=" + ItemID + " and Batch_BatchId=" + BatchID);
            int j = updateDB("Update TOP(1) StockConsumptionItems set Quantity =Quantity - " + RetQty + " where Item_ItemID=" + ItemID + " and InvoiceId=" + invId);
        }
        private int updateDB(string qry )
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand com = new SqlCommand(qry, con);
            int i = com.ExecuteNonQuery();
            con.Close();
            return i;
        }
        private double ConvertPercToValue(double TotalDeduction , double TotalRefundAmount , int type)
        {
            double deduction = 0;
            if (type == 1)
            {
                deduction = TotalDeduction / 100 * TotalRefundAmount;
            }
            else
            {
                deduction = TotalDeduction;
            }
            
            return deduction;
        }

        private void FillObject(SalesReturnItem objInvoiceRetItem, DataGridViewRow r)
        {
            objInvoiceRetItem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objInvoiceRetItem.ItemName = r.Cells["colItemName"].Value.ToString();
            objInvoiceRetItem.IsActive = true;
            objInvoiceRetItem.ReturnQuantity = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
            objInvoiceRetItem.ConvUnit = Convert.ToInt32(r.Cells["colConversionUnit"].Value);
            objInvoiceRetItem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            objInvoiceRetItem.RefundAmount = Convert.ToDouble(r.Cells["colRefundAmount"].Value.ToString());
            objInvoiceRetItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            objInvoiceRetItem.RetialPrice = Convert.ToDouble(r.Cells["colRate"].Value);
            objInvoiceRetItem.RetialPrice = objInvoiceRetItem.Unit == 0 ? objInvoiceRetItem.RetialPrice / objInvoiceRetItem.ConvUnit : objInvoiceRetItem.RetialPrice;
            objInvoiceRetItem.DeductionValue = Convert.ToDouble(r.Cells["colDeduction"].Value);
            objInvoiceRetItem.DeductionType = r.Cells["colDeductionType"].Value.ToString() == "%" ? 1 : 0;
            if (objInvoiceRetItem.DeductionType == 1)
            {
                objInvoiceRetItem.RefundAmount = objInvoiceRetItem.RefundAmount - ((objInvoiceRetItem.DeductionValue / 100) * objInvoiceRetItem.RefundAmount);
            }
            else
            {
                objInvoiceRetItem.RefundAmount = objInvoiceRetItem.RefundAmount - objInvoiceRetItem.DeductionValue;
            }
            objInvoiceRetItem.DeductionValue = objInvoiceRetItem.Unit == 0 ? objInvoiceRetItem.DeductionValue / objInvoiceRetItem.ConvUnit : objInvoiceRetItem.DeductionValue;
            objInvoiceRetItem.RetialPrice = Math.Round(objInvoiceRetItem.RetialPrice, 2);

        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
            this.GetSelectedItemData(itemId, grdItems.Rows[e.RowIndex]);
           
        }
        private void GetSelectedItemData(int ItemId, DataGridViewRow r)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged_1;
            //var Batch = r.Cells["colBatch"].Value;
            //int BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value)
            int getQty = 0;
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            List<StockAddFilterVM> checkItem = new List<StockAddFilterVM>();
            InvoiceVM Inv = new InvoiceVM();
            using (unitOfWork = new UnitOfWork())
            {
                itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                BatchStockList = itemDetail.BatchStockList.ToList();
            }
            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            DataGridViewComboBoxCell dgCmbInvoiceId = (DataGridViewComboBoxCell)r.Cells["colInvoiceId"];
            DataTable dty = get("SELECT dbo.Invoices.InvoiceRefNo, dbo.Invoices.IsActive FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 and dbo.Invoices.IsQInvoice =0  where Item_ItemID=" + itemDetail.Item.ItemID);
            if (dty.Rows.Count > 0)
            {
                dgCmbInvoiceId.DataSource = dty;
                dgCmbInvoiceId.ValueMember = "InvoiceRefNo";
                dgCmbInvoiceId.DisplayMember = "InvoiceRefNo";
            }
            r.Cells["colInvoiceId"].Value = dty.Rows[dty.Rows.Count-1][0].ToString();
            using (unitOfWork = new UnitOfWork())
            {
                Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(r.Cells["colInvoiceId"].Value)));
            }
            foreach (InvoiceItemVM m in Inv.InvoiceItems)
            {
                if (m.Item.ItemID == itemDetail.Item.ItemID)
                {
                        r.Cells["colBatch"].Value = m.BatchName.ToString();
                        r.Cells["colBatchId"].Value = m.BatchId.ToString();
                        r.Cells["InvoiceItemId"].Value = m.InvoiceItemId.ToString();
                        r.Cells["colRate"].Value = m.Rate.ToString();
                        r.Cells["colQuanitycheck"].Value = (m.Quantity - m.ReturnedQuantity).ToString();
                        r.Cells["colTotQty"].Value = (m.Quantity).ToString();
                        r.Cells["colCostPrice"].Value = m.UnitCost.ToString();
                        r.Cells["colDiscCost"].Value = m.DiscountedPrice.ToString();
                        r.Cells["colInvoiceUnit"].Value = m.Unit.ToString();   
                        r.Cells["colPaymentType"].Value = Inv.PaymentType.ToString();   
                }
            }
            foreach (BatchStockVM i in BatchStockList)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    checkItem = unitOfWork.InvoiceRepository.LoadSaleReturn(ItemId, Convert.ToInt32(i.BatchId), Convert.ToInt64(r.Cells["colInvoiceId"].Value));
                    Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(r.Cells["colInvoiceId"].Value)));
                }
                foreach (StockAddFilterVM j in checkItem)
                {
                    r.Cells["InvoiceItemId"].Value = j.InvoiceItemId;
                    j.BatchId = j.BatchId.Distinct().ToList();
                    foreach (var m in j.BatchId)
                    {
                        if (Convert.ToInt32(m) == i.BatchId)
                        {
                            getQty = j.ConsumedStock;
                            i.BatchName = i.BatchName + " [ " + getQty + " ]";
                            if (getQty > 0 || SharedVariables.AdminPharmacySetting.NegativeSaleReturn)
                            {
                                FinalResult.Add(i);
                                if (FinalResult.Count == 1)
                                {
                                    //r.Cells["colQuanitycheck"].Value = getQty;
                                }
                            }
                        }
                    }
                    
                }
            }
            if (FinalResult.Count > 0 || SharedVariables.AdminPharmacySetting.NegativeSaleReturn) { 
                //r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                //r.Cells["colBatchId"].Value = FinalResult[0].BatchId; 
            }
            else
            {
                return;
            }
            DataTable dt = get("Select Batch_BatchId  from StockItems where Item_ItemID=" + itemDetail.Item.ItemID);
            if (!SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colBatch"];
                dgCmbBatch.DataSource = FinalResult;
                dgCmbBatch.ValueMember = "BatchId";
                dgCmbBatch.DisplayMember = "BatchName";
                if (FinalResult.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //r.Cells["colBatchId"].Value = dt.Rows[dt.Rows.Count - 1][0].ToString();
                    }
                }
            }
            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;
            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");
            if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            }
            else
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            }

            r.Cells["colConversionUnit"].Value = itemDetail.Item.ConversionUnit;
            string setBatch = FinalResult[0].BatchId.ToString();
            getAvailableStock(r, Convert.ToInt32(setBatch), ItemId);
            
            //r.Cells["colRate"].Value = itemDetail.Item.RetailPrice;
            grdItems.CellValueChanged += grdItems_CellValueChanged_1;
            
        }

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
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I))
            {
                this.uC_SearchItems1.Focus();
                return true;
            }
            if (keyData == (Keys.F1))
            {
                btnSearchItems.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (grdItems.SelectedRows != null && grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
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
            if (keyData == (Keys.Control | Keys.F))
            {
                frmInvoice f = new frmInvoice();
                f.ShowDialog();
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
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (grdItems.Rows.Count > 0)
                {
                    if (this.ActiveControl.Name == grdItems.Name)
                    {
                        DataGridViewRow r = grdItems.SelectedRows[0];
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colReturnQuantity"];
                    }
                    else
                    {
                        if (grdItems.Rows.Count > 0)
                        {
                            this.ActiveControl = grdItems;
                            grdItems.Rows[0].Selected = true;
                            grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colReturnQuantity"];
                            grdItems.CurrentCell.Selected = true;
                        }
                    }
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.T))
            {
                numTotalDeduct.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void numTotalDeduct_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                calculateNumTotalDeduct();
            }
            catch (Exception) { }
        }

        public void calculateNumTotalDeduct()
        {
            double deduct = 0;
            if (numTotalDeduct.Value > 0)
            {
                if (cmbTotalDeductType.SelectedItem == "Value")
                {
                    lblGrandTotal.Text = (Convert.ToDouble(lblSubTotal.Text) - Convert.ToDouble(numTotalDeduct.Value)).ToString();
                }
                else
                {
                    deduct = Convert.ToDouble(numTotalDeduct.Value);
                    deduct = (deduct / 100) * Convert.ToDouble(lblSubTotal.Text);
                    lblGrandTotal.Text = (Convert.ToDouble(lblSubTotal.Text) - deduct).ToString();
                }

                if (deductChange == false) 
                { 
                    grdItems.Columns["colDeduction"].ReadOnly = true;
                    grdItems.Columns["colDeductionType"].ReadOnly = true;    
                }
            }
            else
            {
                grdItems.Columns["colDeduction"].ReadOnly = false;
                grdItems.Columns["colDeductionType"].ReadOnly = false;   
                lblGrandTotal.Text = lblGrandTotal.Text = (Convert.ToDouble(lblSubTotal.Text) - Convert.ToDouble(numTotalDeduct.Value)).ToString();
                numTotalDeduct.Enabled = true;
                cmbTotalDeductType.Enabled = true;
            }
        }

        private void cmbTotalDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateNumTotalDeduct();
        }

        private void txtSearchPatient_KeyDown(object sender, KeyEventArgs e)
        {
            ShowPatientData();            
        }
        
        private void txtSearchPatient_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowPatientData();
        }

        private void ShowPatientData()
        {
            frmSearchPatient f = new frmSearchPatient();
            f.ShowDialog();
            if(f.PatientId > 0)
            {
                PatientId = f.PatientId;
            }
        }

        private void ClearAll(bool isAfterSave)
        {            
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.Yes;
                }
                else
                {
                      rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK || rs == DialogResult.Yes)
                {
                    grdItems.Rows.Clear();
                    Clear();
                    lblGrandTotal.Text = "0";
                    lblSubTotal.Text = "0";
                    numTotalDeduct.Value = 0;
                }
            }            
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void uC_SearchItems1_Load(object sender, EventArgs e)
        {

        }

        private void uC_SearchItems1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void uC_SearchItems1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid")
            {
                object value = grdItems.Rows[e.RowIndex].Cells["colBatch"].Value;

            }
        }
        private void grdItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }
        private void getAvailableStock(DataGridViewRow r, int batchId, int itemId)
        {
            BatchStockVM vm = new BatchStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.ItemRspository.GetBatchStockSaleRet(batchId,
                    itemId,
                    0,
                    SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
            }
            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConversionUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                double rate = vm.RetailPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                }
                //r.Cells["colRate"].Value = rate;
            }
        }

        private void grdItems_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged_1;
            InvoiceVM Inv = new InvoiceVM();
            List<StockAddFilterVM> checkItem = new List<StockAddFilterVM>();
            if (e.RowIndex < 0) return;
            int batchId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
            int ItemId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);

            if (e.ColumnIndex == grdItems.Columns["colBatch"].Index)
            {
                //DataTable dty = get("SELECT dbo.Invoices.InvoiceRefNo, dbo.Invoices.IsActive FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 AND dbo.Invoices.TotalPaid > 0 where Item_ItemID=" + ItemId + " and Batch_BatchId=" + batchId);

                ////grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value = dty.Rows[dty.Rows.Count - 1][0].ToString();
                //using (unitOfWork = new UnitOfWork())
                //{
                //    Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value)));
                //}
                //foreach (InvoiceItemVM vm in Inv.InvoiceItems)
                //{
                //    if (vm.Item.ItemID == ItemId)
                //    {
                //        grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value = Inv.InvoiceId.ToString();
                //    }
                //}
                //CalculateItemTotals();
                
                //string unitSet = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                //int ConvUnit = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                //batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatch"].Value);
                //grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId;
                //getAvailableStock(grdItems.Rows[e.RowIndex], batchId, ItemId);

                //using (unitOfWork = new UnitOfWork())
                //{
                //    checkItem = unitOfWork.InvoiceRepository.LoadSaleReturn(ItemId, Convert.ToInt32(batchId), Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value));
                //}


                //DataTable dty = get("SELECT dbo.Invoices.InvoiceRefNo, dbo.Invoices.IsActive FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 AND dbo.Invoices.TotalPaid > 0 where Item_ItemID=" + ItemId);

                //grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value = dty.Rows[dty.Rows.Count - 1][0].ToString();
                //using (unitOfWork = new UnitOfWork())
                //{
                //    Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value)));
                //}

                //foreach (InvoiceItemVM j in Inv.InvoiceItems)
                //{


                //    if (Convert.ToInt32(j.BatchId) == batchId)
                //    {
                //        int getQty = j.ConsumedStock;
                //        getQty = unitSet.ToLower() == "pack" ? getQty / ConvUnit : getQty;
                //        grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = getQty;
                //    }

                //}
                //foreach (StockAddFilterVM j in checkItem)
                //{
                //    j.BatchId = j.BatchId.Distinct().ToList();
                //    foreach (var m in j.BatchId)
                //    {
                //        if (Convert.ToInt32(m) == batchId)
                //        {
                //            int getQty = j.ConsumedStock;
                //            getQty = unitSet.ToLower() == "pack" ? getQty / ConvUnit : getQty;
                //            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = getQty;
                //        }
                //    }
                //}

                ////for Rate
                //int retQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value);
                //double rate = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value);
                ////grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                //grdItems.Rows[e.RowIndex].Cells["colRefundAmount"].Value = 0;
                //double refAmt = retQty * rate;
            }
            else if (e.ColumnIndex == grdItems.Columns["colDeduction"].Index ||
                e.ColumnIndex == grdItems.Columns["colDeductionType"].Index)
            {
                if (e.ColumnIndex == grdItems.Columns["colDeduction"].Index)
                {
                    int inde = cmbTotalDeductType.SelectedIndex;
                    //int value = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colDeduction"].Value);
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        if (Convert.ToString(r.Cells["colDeduction"].Value) == "0.0" || Convert.ToString(r.Cells["colDeduction"].Value) == "0.00"
                                || Convert.ToString(r.Cells["colDeduction"].Value) == "0" || Convert.ToString(r.Cells["colDeduction"].Value) == "")
                        {
                            numTotalDeduct.Enabled = true;
                            cmbTotalDeductType.Enabled = true;
                            cmbTotalDeductType.SelectedIndex = inde;
                            deductChange = false;
                        }
                        else
                        {
                            cmbTotalDeductType.SelectedIndex = 0;
                            numTotalDeduct.Enabled = false;
                            cmbTotalDeductType.Enabled = false;
                            
                            deductChange = true;
                            CalculateItemTotals();
                            grdItems.CellValueChanged += grdItems_CellValueChanged_1;
                            return;
                        }
                    }
                }
                CalculateItemTotals();
            }
            else if (e.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                //using (unitOfWork = new UnitOfWork())
                //{
                //    checkItem = unitOfWork.InvoiceRepository.LoadSaleReturn(ItemId, Convert.ToInt32(batchId));
                //}

                //foreach (StockAddFilterVM j in checkItem)
                //{
                //    j.BatchId = j.BatchId.Distinct().ToList();
                //    foreach (var m in j.BatchId)
                //    {
                //        if (Convert.ToInt32(m) == batchId)
                //        {
                //            int getQty = j.ConsumedStock;
                //            if (getQty < 0) { getQty = 0; }
                //                grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = getQty;
                //        }
                //    }
                //}

                string Qty = Convert.ToString(grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value);
                var r = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value;
                double RQty = r == null || r.ToString() == "" ? 0 : Convert.ToDouble(r);
                grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = RQty;

                if (RQty > Convert.ToDouble(Qty) && !SharedVariables.AdminPharmacySetting.NegativeSaleReturn)
                {
                    MessageBox.Show("Return Quantity Can't be greater than Batch Available Stock", "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Convert.ToDouble(Qty);
                    grdItems.CellValueChanged -= grdItems_CellValueChanged_1;
                }
                CalculateDeductionfromDic(e);
                CalculateItemTotals();
            }
            else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                string unitSet = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                ItemDetailVM itemDetail = new ItemDetailVM();
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                int cu = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                
                using (unitOfWork = new UnitOfWork())
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                    BatchStockList = itemDetail.BatchStockList.ToList();                                        
                }

                    int avlQty = 0;
                    int st = 0;
                    List<BatchStockVM> FinalResult = new List<BatchStockVM>();
                    foreach (BatchStockVM i in BatchStockList)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            checkItem = unitOfWork.InvoiceRepository.LoadSaleReturn(ItemId, Convert.ToInt32(i.BatchId), Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value));
                        }
                        DataTable dty = get("SELECT dbo.Invoices.InvoiceRefNo, dbo.Invoices.IsActive FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 AND dbo.Invoices.TotalPaid > 0 where Item_ItemID=" + itemDetail.Item.ItemID);
                        
                        //grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value = dty.Rows[dty.Rows.Count - 1][0].ToString();
                        using (unitOfWork = new UnitOfWork())
                        {
                            Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value)));
                        }
                        
                            //set availble Quanity 
                            foreach (InvoiceItemVM j in Inv.InvoiceItems)
                            {
                                
                                
                                    if (Convert.ToInt32(j.BatchId) == batchId)
                                    {
                                        avlQty = j.ConsumedStock;
                                        if (avlQty > 0)
                                        {
                                            avlQty = unitSet.ToLower() == "pack" ? avlQty / itemDetail.Item.ConversionUnit : avlQty;
                                            i.BatchName = i.BatchName + " [ " + avlQty + " ]";
                                            FinalResult.Add(i);
                                        }
                                    }
                                
                            }

                            //foreach (StockAddFilterVM j in checkItem)
                            //{
                            //    j.BatchId = j.BatchId.Distinct().ToList();
                            //    foreach (var m in j.BatchId)
                            //    {
                            //        if (Convert.ToInt32(m) == batchId)
                            //        {
                            //            int getQty = j.ConsumedStock;
                            //            getQty = unitSet.ToLower() == "pack" ? getQty / cu : getQty;
                            //            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = getQty;
                            //        }
                            //    }
                            //}
                    }
               
                    

                    

                        if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString().Equals("Units"))
                        {
                            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value) * Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value) * Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            //grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                        }
                        else
                        {
                            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value) * Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                            grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                          //grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value) * Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);

                            grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value) * Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);

                        }
                CalculateDeductionfromDic(e);
                CalculateItemTotals();

                //grdItems.CellValueChanged += grdItems_CellValueChanged_1;
            }
            else if (e.ColumnIndex == grdItems.Columns["colInvoiceId"].Index)
            {
                DataTable dty = get("SELECT dbo.Invoices.InvoiceRefNo, dbo.Invoices.IsActive FROM dbo.InvoiceItems INNER JOIN dbo.Invoices ON dbo.InvoiceItems.Invoice_InvoiceId = dbo.Invoices.InvoiceId AND dbo.Invoices.IsActive = 1 and dbo.Invoices.IsQInvoice = 0 AND dbo.Invoices.TotalPaid > 0 where Item_ItemID=" + ItemId);

                //grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value = dty.Rows[dty.Rows.Count - 1][0].ToString();
                using (unitOfWork = new UnitOfWork())
                {
                    Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value)));
                }
                foreach (InvoiceItemVM vm in Inv.InvoiceItems)
                {
                    if (vm.Item.ItemID == ItemId)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = vm.BatchName.ToString();
                        grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = vm.BatchId.ToString();
                        if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString().ToLower() == "pack")
                        {
                            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = ((vm.Quantity-vm.ReturnedQuantity) / vm.Item.ConversionUnit).ToString();
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = (vm.Rate * vm.Item.ConversionUnit).ToString();
                            grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = (vm.UnitCost * vm.Item.ConversionUnit).ToString();
                            grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value = (vm.DiscountedPrice * vm.Item.ConversionUnit).ToString();
                        }
                        else
                        {
                            grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value = (vm.Quantity - vm.ReturnedQuantity).ToString();
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = vm.Rate.ToString();
                            grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = (vm.UnitCost.ToString());
                            grdItems.Rows[e.RowIndex].Cells["colDiscCost"].Value = (vm.DiscountedPrice.ToString());
                        }
                    }
                }
                if(Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value) < Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value))
                {
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value;

                }
                CalculateDeductionfromDic(e);

                CalculateItemTotals();
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged_1;
        }
        
        public void CalculateItemTotals()
        {
            double TotalDiscount = 0;
            double totalnum = 0;
            int retQty = 0;
            double rate = 0;
            double refAmt = 0;
            double Subtotal = 0;
            double deductChange = 0;

            //calculate RetAmount
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                //for Rate
                retQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                rate = Convert.ToDouble(r.Cells["colRate"].Value);

                refAmt = retQty * rate;
                r.Cells["colRefundAmount"].Value = refAmt;

                //for deduction
                double deduct = Convert.ToDouble(r.Cells["colDeduction"].Value);
                string deductType = r.Cells["colDeductionType"].Value.ToString();

                if (deductType.ToLower() == "%")
                {
                    //deduct += (deduct / 100) * refAmt;
                    deductChange += (deduct / 100) * refAmt;
                }
                else
                {
                    deductChange += deduct;
                }
                //Total dis
                TotalDiscount = deductChange;
                double totalDedc = Math.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero); // this is special case.

                if (Convert.ToInt32(totalDedc) != 0) { totalnum = totalDedc; }
                else 
                {
                  totalnum = 0;
                  foreach (DataGridViewRow r1 in grdItems.Rows)
                  {
                      if (deductType == "Value")
                      {
                          totalnum += Convert.ToDouble(r1.Cells["colDeduction"].Value);
                      }
                      else if (deductType == "%")
                      {
                          totalnum += (Convert.ToDouble(r1.Cells["colDeduction"].Value) / 100) * refAmt; ;
                      }
                  }
                }
                if (numTotalDeduct.Value > 0) 
                { 
                    numTotalDeduct.Enabled = false;
                    cmbTotalDeductType.Enabled = false;
                }
          
                //Subtotal
                if (refAmt != 0) { Subtotal += refAmt; }
                //else
                //{
                //    Subtotal = 0;
                //    foreach (DataGridViewRow r1 in grdItems.Rows)
                //    {
                //        if (Convert.ToInt32(r1.Cells["colRefundAmount"].Value) != 0)
                //        {
                //           Subtotal += Convert.ToInt32(r1.Cells["colRefundAmount"].Value);
                //        }
                //    }
                //}
            }

            lblSubTotal.Text = Subtotal.ToString();
            numTotalDeduct.Value = Convert.ToDecimal(totalnum);
            lblGrandTotal.Text = (Subtotal - Convert.ToDouble(numTotalDeduct.Value)).ToString();
        }
        private void CalculateDeductionfromDic(DataGridViewCellEventArgs e)
        {
            InvoiceVM Inv = new InvoiceVM();

            var invoiceRefno = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            using (unitOfWork = new UnitOfWork())
            {
                Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(invoiceRefno));
            }
            var invoiceitemid = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
            string Qty = Convert.ToString(grdItems.Rows[e.RowIndex].Cells["colQuanitycheck"].Value);
            var r = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value;
            double RQty = r == null || r.ToString() == "" ? 0 : Convert.ToDouble(r);
            lineitemdisc = false;
            if (Inv.InvoiceItems != null)
            {
                foreach (var inv in Inv.InvoiceItems)
                {
                    if (inv.ItemId == invoiceitemid)
                    {
                        if (Inv.TotalDiscount > 0)
                        {
                            if (inv.Discount > 0)
                            {
                                lineitemdisc = true;
                                if (inv.DiscountType == 1)
                                {
                                    cmbTotalDeductType.SelectedIndex = 0;
                                    numTotalDeduct.Enabled = false;
                                    cmbTotalDeductType.Enabled = false;

                                    deductChange = true;
                                    grdItems.Rows[e.RowIndex].Cells["colDeduction"].Value = Convert.ToDouble(inv.Discount);
                                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = Convert.ToDouble(inv.Discount);
                                    grdItems.Rows[e.RowIndex].Cells["colDeductedDiscount"].Value = Convert.ToDouble(inv.Discount);
                                    grdItems.Rows[e.RowIndex].Cells["colDeductionType"].Value = "%";


                                }
                                else
                                {
                                    cmbTotalDeductType.SelectedIndex = 0;
                                    numTotalDeduct.Enabled = false;
                                    cmbTotalDeductType.Enabled = false;

                                    deductChange = true;
                                    double Qtyaboutunit = 0;
                                    if(grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Pack")
                                    {
                                        Qtyaboutunit = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colTotQty"].Value) / Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colConversionUnit"].Value);
                                    }
                                    else
                                    {
                                        Qtyaboutunit = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colTotQty"].Value) ;
                                    }

                                    var disc = (inv.Discount / Qtyaboutunit)* Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value);
                                    var deduct = disc ;
                                    grdItems.Rows[e.RowIndex].Cells["colDeduction"].Value = Convert.ToDouble(deduct);
                                    grdItems.Rows[e.RowIndex].Cells["colDeductedDiscount"].Value = Convert.ToDouble(deduct);
                                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = Convert.ToDouble(inv.Discount);
                                    grdItems.Rows[e.RowIndex].Cells["colDeductionType"].Value = "Value";
                                }
                            }
                            else
                            {
                                var otheeritemDiscount = false;
                                var count = 0;
                                foreach (var check in Inv.InvoiceItems)
                                {
                                    count++;
                                    if (check.Discount > 0)
                                    {
                                        otheeritemDiscount = true;
                                    }
                                }
                                if (!otheeritemDiscount)
                                {
                                    if (Inv.DiscountType == 1)
                                    {
                                        cmbTotalDeductType.SelectedIndex = 0;
                                        numTotalDeduct.Enabled = false;
                                        cmbTotalDeductType.Enabled = false;

                                        deductChange = true;

                                        var Disc = Inv.TotalDiscount / Convert.ToDouble(count);
                                        var deduct = (Disc / Convert.ToDouble(Qty)) * RQty;
                                        grdItems.Rows[e.RowIndex].Cells["colDeduction"].Value = Convert.ToDouble(deduct);
                                        grdItems.Rows[e.RowIndex].Cells["colDeductedDiscount"].Value = Convert.ToDouble(deduct);
                                        grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = Convert.ToDouble(Disc);
                                        grdItems.Rows[e.RowIndex].Cells["colDeductionType"].Value = "Value";

                                    }
                                    else
                                    {
                                        
                                        cmbTotalDeductType.SelectedIndex = 0;
                                        numTotalDeduct.Enabled = false;
                                        cmbTotalDeductType.Enabled = false;

                                        deductChange = true;
                                        grdItems.Rows[e.RowIndex].Cells["colDeduction"].Value = Convert.ToDouble(Inv.TotalDiscount);
                                        grdItems.Rows[e.RowIndex].Cells["colDeductedDiscount"].Value = Convert.ToDouble(Inv.TotalDiscount);
                                        grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = Convert.ToDouble(Inv.TotalDiscount);
                                        grdItems.Rows[e.RowIndex].Cells["colDeductionType"].Value = "%";

                                    }
                                }

                            }
                        }
                    }
                }
            }

        }
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBatch"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBatchId"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDeductionType"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index ||
                grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colInvoiceId"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void lblSubTotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblGrandTotal.Text = (Convert.ToDouble(lblSubTotal.Text) - Convert.ToDouble(numTotalDeduct.Value)).ToString();
            }
            catch (Exception) { }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            Clear();
        }
        private void searchPatient()
        {
            string searchString = IsNameSearching ? txtSearchByName.Text.Trim().ToLower() : txtSearchByPhone.Text.Trim().ToLower();
            if (searchString == "")
            {
                PatientSearchTimer.Stop();
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
                return;
            }

            IPagedList<PatientSearchVM> PatientsLocal = null;
            using (unitOfWork = new UnitOfWork())
            {
                PatientsLocal = unitOfWork.PatientRepository.SearchPatient(IsNameSearching, IsPhoneSearching, false, searchString, pageNoPatients);
                if (PatientsLocal.Count <= 0) PatientSearchTimer.Stop();
                PatientsGlobal.AddRange(PatientsLocal);
            }
            foreach (PatientSearchVM p in PatientsLocal)
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone, p.Address);
                }));
            }
            if (PatientsGlobal != null && PatientsGlobal.Count > 0)
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = true;
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
            }
        }
        private void txtSearchByName_TextChanged_1(object sender, EventArgs e)
        {
            if (txtSearchByName.Text.Trim() == "")
            {
                this.PatientId = 0;
                txtSearchByPhone.Text = "";
                txtAddress.Text = "";
                return;
            }
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X + 15, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            IsNameSearching = true;
            IsPhoneSearching = false;
            searchPatient();
            PatientSearchTimer.Start();
        }

        private void txtSearchByName_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByPhone.Location.Y + txtSearchByName.Height);
        }

        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
        }

        private void txtSearchByName_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidAlphabaticKey(sender, e);
        }
        private void ShiftCtrlToSuggGrid()
        {
            if (grdSuggestion.Rows.Count > 0)
            {
                grdSuggestion.BringToFront();
                grdSuggestion.Focus();
                if (grdSuggestion.Rows.Count > 1)
                {
                    grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Rows[1].Selected = true;
                }
                else
                {
                    grdSuggestion.Rows[0].Selected = true;
                }
            }
        }

        private void grdSuggestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // CHECKING FOR ALPHABET 
            TextBox TargetText = new TextBox();
            if (IsNameSearching) { TargetText = txtSearchByName; }
            else if (IsPhoneSearching) { TargetText = txtSearchByPhone; }
            if ((e.KeyChar >= 65 && e.KeyChar <= 90)
                || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                TargetText.Focus();
                TargetText.Text += (char)e.KeyChar;
                TargetText.SelectionStart = txtSearchByName.Text.Length;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                TargetText.Focus();
                TargetText.SelectionStart = txtSearchByName.Text.Length;
            }
        }

        private void grdSuggestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSearcherPatient();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                grdSuggestion.Visible = false;
            }
        }

        private void grdSuggestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearcherPatient();
        }
        private void LoadSearcherPatient()
        {
            try
            {
                PatientSearchTimer.Stop();
                if (grdSuggestion.SelectedRows.Count > 0)
                {
                    txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                    txtSearchByName.TextChanged -= txtSearchByName_TextChanged_1;
                    txtAddress.TextChanged -= txtAddress_TextChanged;
                    this.PatientId = SharedFunctions.StringToInt32(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                    this.txtSearchByName.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colPatient"].Value);
                    this.txtSearchByPhone.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colPhone"].Value);
                    this.txtAddress.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colAddress"].Value);
                    grdSuggestion.Visible = false;
                    txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                    txtSearchByName.TextChanged += txtSearchByName_TextChanged_1;
                    txtAddress.TextChanged += txtAddress_TextChanged;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }

        private void txtSearchByPhone_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, txtSearchByPhone.Location.Y + txtSearchByPhone.Height);
        }

        private void txtSearchByPhone_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
            //if (e.KeyCode == Keys.Enter)
            //{
            //    grdSuggestion.Rows.Clear();
            //    string searchString = txtSearchByName.Text.ToLower();
            //    if (searchString != "")
            //    {
            //        List<PatientSuggVM> Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString);
            //        foreach (PatientSuggVM p in Patients)
            //        {
            //            grdSuggestion.Rows.Add(p.PatientId, p.Result);
            //        }
            //        if (Patients.Count > 0)
            //        {
            //            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, grdSuggestion.Location.Y);
            //            grdSuggestion.Visible = true;
            //            grdSuggestion.Focus();
            //            grdSuggestion.Rows[0].Selected = true;
            //        }
            //    }
            //}
        }

        private void txtSearchByPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtSearchByPhone_TextChanged(object sender, EventArgs e)
        {
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X + 15, txtSearchByPhone.Location.Y + txtSearchByPhone.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            //if (txtSearchByPhone.Text != "") { txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged; txtSearchByPhone.Text = ""; txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged; }
            IsNameSearching = false;
            IsPhoneSearching = true;
            string searchString = txtSearchByPhone.Text.ToLower();
            PatientSearchTimer.Start();
            searchPatient();
            ///////////////////

            //grdSuggestion.Location = new Point(txtSearchByPhone.Location.X + 15, txtSearchByPhone.Location.Y + txtSearchByPhone.Height + 4);
            //grdSuggestion.BringToFront();
            //IsPhoneSearching = true;
            //// if (txtSearchByName.Text != "") { txtSearchByName.TextChanged -= txtSearchByName_TextChanged; txtSearchByName.Text = ""; txtSearchByName.TextChanged += txtSearchByName_TextChanged; }
            //grdSuggestion.Rows.Clear();
            //string searchString = txtSearchByPhone.Text.ToLower();
            //if (searchString != "")
            //{
            //    IPagedList<PatientSearchVM> Patients;// new List<PatientSearchVM>();
            //    using (unitOfWork = new UnitOfWork())
            //    {
            //        Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString, pageNoPatients);
            //    }
            //    foreach (PatientSearchVM p in Patients)
            //    {
            //        grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
            //    }
            //    if (Patients.Count > 0)
            //    {
            //        grdSuggestion.Rows[0].Selected = false;
            //        grdSuggestion.Visible = true;
            //    }
            //    else
            //    {
            //        grdSuggestion.Visible = false;
            //    }
            //}
            //else
            //{
            //    grdSuggestion.Visible = false;
            //}
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            //{
                //int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                //try
                //{
                    //grdItems.Rows.RemoveAt(e.RowIndex);
                    CalculateItemTotals();
                    if (grdItems.Rows.Count == 0)
                    {
                        lblSubTotal.Text = "0";
                        numTotalDeduct.Value = 0;
                        lblGrandTotal.Text = "0";
                    }
                //}
                //catch (Exception ex)
                //{ }
            //}
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            new frmShortCutSalesReturn().ShowDialog();
        }

        private void numTotalDeduct_Enter(object sender, EventArgs e)
        {
            numTotalDeduct.Select(0, numTotalDeduct.Value.ToString().Length);
        }

        private void numTotalDeduct_Validated(object sender, EventArgs e)
        {
            if (numTotalDeduct.Text == "")
            {
                numTotalDeduct.Text = "0";
            }
        }

        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
            }
        }
        private void IsValidDoubleNumerickeys(object sender, KeyPressEventArgs e, TextBox tb)
        {
            try
            {
                SharedFunctions.IsValidDoubleNumericDatagridview(sender, e, tb);
            }
            catch (Exception ex) { }
        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler((arg, ers) => IsValidDoubleNumerickeys(sender, ers, tb));
            }
            ComboBox cmb = e.Control as ComboBox;
        }

        private void frmSalesReturn_VisibleChanged(object sender, EventArgs e)
        {
            this.uC_SearchItems1.Select();
            this.uC_SearchItems1.Focus();
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            if (ItemAlreadyAdded(ItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
            {
                MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsValidInput(ItemId))
            {
                int btch = 0;
                string batchname = "";
                if (!SharedVariables.AdminPharmacySetting.AllowAddingSameItemwithDifferentIvoice)
                {
                    //---------Item Already Exists----------//
                    if (ItemAlreadyAdded(this.SelectedItemId))
                    {
                        MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        uC_SearchItems1.SetFocus();
                        return;
                    }
                }
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + SelectedItemId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + SelectedItemId);
                if (dt.Rows.Count > 0)
                {
                    btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                    DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                    batchname = batch.Rows[0][0].ToString();
                }
                grdItems.Rows.Add(
                                0,
                                0,
                               this.SelectedItemId,//itemid
                               "",//itemname
                               "",//btachid
                               "", //batch
                               "",//unit
                               0, //InvoiceIDs
                               "",//coUnit
                               0,//Rate
                               0, //qty
                               0,//return qty
                               0,//refund qty
                               0,//deduction
                               "%", //deduction type
                               true,//is new row
                               0,
                               0,
                               ""
                               );
                Clear();
                this.uC_SearchItems1.Focus();
                this.uC_SearchItems1.SetText = "";
                grdItems.Rows[grdItems.Rows.Count - 1].Cells["colReturnQuantity"].Value = Convert.ToString(Quantity);
                grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colReturnQuantity"];
                grdItems.BeginEdit(true);
                int index = grdItems.CurrentRow.Index;
                grdItems.FirstDisplayedScrollingRowIndex = index;
            }
        }
        private bool IsValidInput(int itemid = 0)
        {
            this.SelectedItemId = (itemid != 0) ? itemid : this.SelectedItemId;
            bool ErrFound = false;
            if (this.SelectedItemId <= 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                this.uC_SearchItems1.Focus();
                return false;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (!ErrFound)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            uC_SearchItems1.SetText = "";
            LoadItems();
            fillInvoiceNumbers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InvoiceVM Inv = new InvoiceVM();
            using (unitOfWork = new UnitOfWork())
            {
                Inv = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(Convert.ToInt32(cmbInvoices.Text)));
            }
            foreach (InvoiceItemVM m in Inv.InvoiceItems)
            { if((m.Quantity - m.ReturnedQuantity) > 0)
                {
                    this.SelectedItemId = m.Item.ItemID;
                    btnAddItem.PerformClick();

                }
            }
            if (grdItems.Rows == null || grdItems.Rows.Count < 1)
            {
                if(Inv.InvoiceItems != null)
                {
                    MessageBox.Show("Selected Invoice Already Returned", "Already Returned", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //checknullReturnQty();
        }
    }
}