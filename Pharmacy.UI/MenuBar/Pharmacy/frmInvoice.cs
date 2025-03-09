using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageAuditLog;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmInvoice : Form
    {
        Service obj = new Service();
        int OrderId = 0;
        double TotalCost = 0;
        bool discountChange = false;
        bool isNewItemAdded = false;
        bool IsEditAllow = false;
        bool isInvoiceUpdated = false;
        bool isInvoiceUpdated1 = false;
        bool createInvoice = false;
        private bool IsRefreshFrm = false;
        IPagedList<InvoiceVM> holdingInvoices;
        public long HoldingInvoiceId = 0;
        private int InvHoldPageNo = 0;
        private UnitOfWork unitOfWork;
        OnlineIntegrateVM OnlineIntegrateList;
        bool CancelInvoice = false;
        List<BatchStockVM> BatchStockList;
        List<InvoicePayment> DeletedPayments = new List<InvoicePayment>();
        List<InvoiceItemVM> DeletedInvoiceItems;
        List<Item> POSItems = new List<Item>();
        List<ItemDetailVM> AddedItemsData = new List<ItemDetailVM>();
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        List<Item> ItemsGlobal = new List<Item>();
        DataTable dtStockConsumption = new DataTable();
        long PatientId = 0;
        long InvoiceEditId = 0;
        bool IsInvoicecopy = false;
        long InvoicecopyId = 0;
        bool IsInvoiceEdit = false;
        bool IsInvoiceRefund = false;
        bool IsDiscountByItem = false;
        bool IsOnlineIntegrate = false;
        int OrderValue = 0;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        int EditingRowIndex = -1;
        bool IsNameSearching, IsPhoneSearching = false;
        bool IsReturnedQuantityFound = false;
        private double InitialTotalRefundValue = 0;
        private double LoadedInvoice_TotalPaid = 0;
        private double TotalPaid = 0;
        List<BatchStockVM> FinalResult5 = new List<BatchStockVM>();
        bool IsOnlineCancel = false;
        int deliveryOrderCharges = 0;
        int voucherCharges = 0;
        string voucherName = "";
        double WalletAmount = 0;
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int pageNoPatients = 1;
        private int SelectedItemId = 0;
        public string SelectedItemName = "";
        bool isBalanceActive = true;
        bool isLoadingForm = false; // this will be used in different places to negate an operation while invoice data/Form data is being loaded.
        bool IsRemovedAll = false;
        public frmInvoice()
        {
            InitializeComponent();
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            CheckAttributesHide();
            createInvoice = true;
            //SharedVariables.AdminPharmacySetting.IsItemConumptionFifo = false;
            SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS = false;
            PatientSearchTimer.Enabled = true;
            PatientSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            PatientSearchTimer.AutoReset = true;
            PatientSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(PatientSearchTimer_Elapsed);
            PatientSearchTimer.Stop();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            //this.uC_SearchItems1.OnEnterKeyDown += uC_SearchItems1_OnEnterKeyDown;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            checkdiscprice();
            canceltextBox.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            lblWalletLabel.Visible = false;
            lblWalletRs.Visible = false;
            lblWalletAmount.Visible = false;
            lblDeliveryRs.Visible = false;
        }

        private void checkdiscprice()
        {
            if (SharedVariables.AdminPharmacySetting.DiscountedPrice)
            {
                grdItems.Columns["DiscountedPrice"].Visible = true;
            }
            else
            {
                grdItems.Columns["DiscountedPrice"].Visible = false;
            }

        }
        public frmInvoice(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            CheckAttributesHide();
            button3.Enabled = false;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            canceltextBox.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            lblWalletLabel.Visible = false;
            lblWalletRs.Visible = false;
            lblWalletAmount.Visible = false;
            lblDeliveryRs.Visible = false;
        }
        public frmInvoice(long PatientId , long id)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            CheckAttributesHide();
            button3.Enabled = false;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            canceltextBox.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            lblWalletLabel.Visible = false;
            lblWalletRs.Visible = false;
            lblWalletAmount.Visible = false;
            lblDeliveryRs.Visible = false;
        }
        private void FillObject(SalesReturnItem objInvoiceRetItem, DataGridViewRow r)
        {
            objInvoiceRetItem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objInvoiceRetItem.ItemName = r.Cells["colItemName"].Value.ToString();
            objInvoiceRetItem.IsActive = true;
            objInvoiceRetItem.ReturnQuantity = Convert.ToInt32(r.Cells["colReturnQuantity"].Value) - Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value);
            objInvoiceRetItem.ConvUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            objInvoiceRetItem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            objInvoiceRetItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            objInvoiceRetItem.RetialPrice = Convert.ToDouble(r.Cells["colRate"].Value);
            objInvoiceRetItem.RetialPrice = objInvoiceRetItem.Unit == 0 ? Convert.ToDouble(r.Cells["colRate"].Value) / Convert.ToInt32(r.Cells["colConvUnit"].Value) : objInvoiceRetItem.RetialPrice;
            objInvoiceRetItem.DeductionValue = Convert.ToDouble(r.Cells["colDiscount"].Value);
            objInvoiceRetItem.DeductionType = (objInvoiceRetItem.DeductionValue == 0) ? (cmbTotalDiscountType.Text == "%") ? 1 : 0 : r.Cells["colDiscountType"].Value.ToString() == "%" ? 1 : 0;
            objInvoiceRetItem.RefundAmount = (Convert.ToDouble(r.Cells["colRate"].Value) * objInvoiceRetItem.ReturnQuantity);
            if (objInvoiceRetItem.DeductionValue <= 0)
            {
                objInvoiceRetItem.DeductionValue = (objInvoiceRetItem.DeductionValue == 0) ? Convert.ToDouble(numTotalDiscount.Value) : objInvoiceRetItem.DeductionValue;
                double deduction = objInvoiceRetItem.DeductionType == 0 ? Convert.ToDouble(objInvoiceRetItem.DeductionValue) : (Convert.ToDouble(objInvoiceRetItem.DeductionValue)) / 100 * ((Convert.ToDouble(r.Cells["colRate"].Value) * Convert.ToInt32(r.Cells["colQuantity"].Value)));
                objInvoiceRetItem.RefundAmount = objInvoiceRetItem.RefundAmount - ((deduction / grdItems.RowCount) / Convert.ToInt32(r.Cells["colQuantity"].Value)) * objInvoiceRetItem.ReturnQuantity;
            }
            else
            {
                double deduction = objInvoiceRetItem.DeductionType == 0 ? Convert.ToDouble(objInvoiceRetItem.DeductionValue) : (Convert.ToDouble(objInvoiceRetItem.DeductionValue)) / 100 * ((Convert.ToDouble(r.Cells["colRate"].Value) * Convert.ToInt32(r.Cells["colQuantity"].Value)));
                objInvoiceRetItem.RefundAmount = objInvoiceRetItem.RefundAmount - (deduction / Convert.ToInt32(r.Cells["colQuantity"].Value)) * objInvoiceRetItem.ReturnQuantity;
            }
            //objInvoiceRetItem.DeductionValue = objInvoiceRetItem.Unit == 0 ? objInvoiceRetItem.DeductionValue / objInvoiceRetItem.ConvUnit : objInvoiceRetItem.DeductionValue;
            objInvoiceRetItem.RetialPrice = Math.Round(Convert.ToDouble(r.Cells["colRate"].Value), 2);
        }
        public int SaveSalesReturn(int UserId)
        {
            int QtyUpdate = 0;
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        SalesReturn objSalesRet = new SalesReturn();
                        objSalesRet.CreateDate = DateTime.Now;
                        objSalesRet.SubTotal = double.Parse(txtCurrentRefund.Text);
                        objSalesRet.GrandTotal = double.Parse(txtCurrentRefund.Text);
                        objSalesRet.TotalDeductionValue = Convert.ToDouble(0);
                        objSalesRet.TotalDeductionType = 1;
                        objSalesRet.SalesReturnItem = new List<SalesReturnItem>();
                        objSalesRet.UpdateDate = DateTime.Now;
                        objSalesRet.IsActive = true;
                        objSalesRet.User = unitOfWork.UserRepository.GetById(UserId);

                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            SalesReturnItem objSalesRetItem = new SalesReturnItem();
                            FillObject(objSalesRetItem, r);
                            objSalesRetItem.InvoiceRefNo = Convert.ToInt32(this.InvoiceEditId);
                            QtyUpdate = Convert.ToInt32(r.Cells["colReturnQuantity"].Value) - Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value);
                            //int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                            //int BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);

                            if (QtyUpdate <= 0)
                            {
                                continue;
                            }
                            else
                            {
                                int batchid = Convert.ToInt32(r.Cells["colBatchId"].Value);
                                objSalesRetItem.Batch = unitOfWork.BatchRepository.GetById(batchid);
                            }
                            int ItemID = Convert.ToInt32(r.Cells["colItemId"].Value);
                            objSalesRetItem.Item = unitOfWork.ItemRspository.GetById(ItemID);
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
                        }

                        //add Patient
                        if (txtSearchByName.Text.Trim() != "")
                        {
                            var p = PatientId;
                            Patient newP = unitOfWork.PatientRepository.GetPatientByPatientId(p);
                            objSalesRet.Patient = newP;
                        }

                        unitOfWork.SalesReturnRepository.Insert(objSalesRet);
                        objSalesRet.SaleReturnRefNo = unitOfWork.SalesReturnRepository.GetSalesReturnRefNo();
                        unitOfWork.Save();
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objSalesRet, ModuleName.SalesReturn, ModuleAction.Create, objSalesRet.SaleReturnId);

                        //update Item

                        scope.Complete();

                        //MessageBox.Show("Sales Return Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public frmInvoice(bool IsInvoiceEdit, bool IsInvoiceRefund, long InvoiceEditId)
        {
            InitializeComponent();
            CheckAttributesHide();
            createInvoice = false;
            this.InvoiceEditId = InvoiceEditId;
            this.IsInvoiceEdit = IsInvoiceEdit;
            this.IsInvoiceRefund = IsInvoiceRefund;
            button1.Visible = false;
            button3.Enabled = false;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            canceltextBox.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            lblWalletLabel.Visible = false;
            lblWalletRs.Visible = false;
            lblWalletAmount.Visible = false;
            lblDeliveryRs.Visible = false;
        }
        public frmInvoice(bool IsInvoiceCopy, long InvoicecopyId, bool isEstimateBill)
        {
            InitializeComponent();
            CheckAttributesHide();
            createInvoice = true;
            this.InvoicecopyId = InvoicecopyId;

            this.IsInvoicecopy = IsInvoiceCopy;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            this.Text = isEstimateBill ? "Create Invoice" : "Copy Invoice";
            canceltextBox.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            lblWalletLabel.Visible = false;
            lblWalletRs.Visible = false;
            lblWalletAmount.Visible = false;
            lblDeliveryRs.Visible = false;
        }

        public void CheckAttributesHide()
        {
            if (SharedVariables.AdminPharmacySetting.IsPOSUnitCost)
            {
                grdItems.Columns["colCostPrice"].Visible = false;
            }

            if (SharedVariables.AdminPharmacySetting.IsPOSProfitLoss)
            {
                label3.Visible = false;
                lblNetProfit.Visible = false;
            }
        }
        public frmInvoice(bool IsOnlineIntegrate, string Quantity , String ItemName , int OrderId , int  OrderValue , OnlineIntegrateVM objOnline)
        {
            InitializeComponent();
            
            this.IsOnlineIntegrate = IsOnlineIntegrate;
            this.OrderId = OrderId;
            this.OrderValue = OrderValue;
            OnlineIntegrateList = objOnline;

            deliveryOrderCharges = objOnline.DeliveryCharges;
            if (objOnline.VoucherNumber != "0")
            {
                voucherCharges = Convert.ToInt32(objOnline.VoucherAmount);
                WalletAmount = Convert.ToDouble(objOnline.WalletAmount);
                voucherName = objOnline.VoucherNumber;
            }
            uC_SearchItems1.Enabled = false;
            btnSearchItems.Enabled = false;
            btnAddItem.Enabled = false;
            numTotalDiscount.Enabled = false;
            cmbTotalDiscountType.Enabled = false;
            btnClear.Enabled = false;
            pnlPatientDetails.Enabled = false;
            canceltextBox.Visible = false;
            label24.Visible = false;
            grpInvoiceHolding.Enabled = false;
            btnClearAll.Enabled = false;

            dtpPmntType2.Enabled = false;
            cmbPmntType1.Enabled = false;
            numPmnt1.Enabled = false;
            chkPmnt2.Enabled = false;

            //seperate item
            //var Items = ItemId.Split(',');
            var Qty = Quantity.Split(',');
            var ItemsName = ItemName.Split(',');

            if (objOnline.PaymentMethod.ToLower() == "cash")
            {
                cmbPmntType1.SelectedIndex = 0;
            }
            else
            {
                cmbPmntType1.SelectedIndex = 3;
            }
            setItemOnlineIntegrate(OnlineIntegrateList);
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            checkdiscprice();
            CheckAttributesHide();
        }
        public void setItemOnlineIntegrate(OnlineIntegrateVM onlineOrder)
        {

            foreach (OnlineItemVM order in onlineOrder.OnlineItem)
            {
                DataTable itemId = getItemName("Select * from Items where Unique_itemId=@Unique_itemId and IsActive=" + 1, order.ItemId);

                this.SelectedItemId = Convert.ToInt32(itemId.Rows[0]["ItemID"]);

                this.SelectedItemName = order.ItemName;
                try
                {
                    this.isNewItemAdded = true;
                    if (IsValidInput())
                    {
                        //---------Item Already Exists----------//
                        if (ItemAlreadyAdded(this.SelectedItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                        {
                            MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //uC_SearchItems1.SetText = "";                       
                            uC_SearchItems1.SetFocus();
                            return;
                        }

                        #region AddSearchItem
                        //using (unitOfWork = new UnitOfWork())
                        //{
                        //    un
                        //}
                        //if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                        this.isNewItemAdded = true;
                        Double UnitCost = 0;
                        Double DiscountedCheck = 0;
                        Double DiscountedPrice = 0;
                        string batchname = "";
                        Double Rate = 0;
                        int unit = 0;
                        int btch = 0;
                        DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + SelectedItemId);
                        DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + SelectedItemId);
                        if (dt.Rows.Count > 0)
                        {

                            Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                            UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                            Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                            int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                            unit = order.Unit.ToLower() == "pack" ? 0 : 1;
                            //unit = order.Unit == "pack" ? 0 : 1;
                            btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                            DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                            batchname = batch.Rows[0][0].ToString();
                            int qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                            //Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                            Rate = order.Rate;
                            UnitCost = UnitCost;
                            if (DiscountType == 0)
                            {
                                Discount = ((Discount / 100) * UnitCost);
                                if (unit == 0)
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        //Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        //DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                                        DiscountedPrice = Math.Round(((UnitCost - Discount) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                                    }
                                    else
                                    {
                                        DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);
                                    }
                                }
                                else
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        //Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        //DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                        DiscountedPrice = Math.Round((((UnitCost)) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                                    }
                                    else
                                    {
                                        //DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                        DiscountedPrice = Math.Round((UnitCost) - Discount, 2);
                                    }
                                }
                            }
                            else
                            {
                                if (unit == 0)
                                {

                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        //Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                        }
                                    }
                                    else
                                    {
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) - Discount / qty), 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);

                                        }
                                    }
                                }
                                else
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                        }
                                    }
                                    else
                                    {
                                        DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round((UnitCost) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round((UnitCost) - Discount, 2);

                                        }
                                    }
                                }
                            }
                        }

                        double rate = 0;
                        //if (order.Unit == "unit")
                        //{
                        //     //int sizeOfStrip = (order.ConversionUnit / Convert.ToInt32(order.SizeOfStrip));
                        //     rate = order.Rate + (order.Discount / order.Quantity);
                        //}
                        //else
                        //{
                        rate = order.Rate + (order.Discount / order.Quantity);
                        //}
                        grdItems.Rows.Add(
                            0, //SrNo,
                            0,//invoiceitemid
                            this.SelectedItemId,//itemid
                            "",//itemname
                            "", //PCT Code
                            batchname,//batch
                            btch,//batchid
                            LoadItemRackName(this.SelectedItemId), //"",// rackno
                            unit,//unit
                            order.ConversionUnit,//conv unit
                            order.PerUnitCostPrice,//unitcost
                            DiscountedPrice, //DiscountedPrice
                            rate,//retail price
                            Profit(rate, order.PerUnitCostPrice), //ProfitMargin
                            0,//available qty
                            order.Quantity,//qty according to given Online Order
                            0, //bonus qty
                            order.Amount,//amount
                            order.NetAmount,//net amount
                            order.Discount,//discount acc to onlineOrder
                            order.DiscountType == 1 ? "Value" : "%", //discount type
                            order.saletax,
                            order.saletaxType == 1 ? "Value" : "%", //saletax
                            true,//is new row
                            0,//return
                            0,//initial return
                            false,// affect stock
                            false // isoptionalbatchno.
                            );

                        #endregion
                        btnClear.PerformClick();
                        uC_SearchItems1.SetFocus();
                        grdItems.BeginEdit(true);
                        int index = grdItems.CurrentRow.Index;
                        grdItems.FirstDisplayedScrollingRowIndex = index;
                        SelectedItemId = 0;
                        getprofit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            SelectedItemId = 0;
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            if (this.SelectedItemId <= 0)
            {
                btnClear.PerformClick();
                return;
            }
            btnAddItem.PerformClick();

            //LoadUnitsData();
            //GetItemBatches(this.SelectedItemId);
            //if (cmbSelectBatch.Items.Count > 1)
            //{
            //    cmbSelectBatch.Focus();
            //}
            //else
            //{
            //    btnAddItem.PerformClick();
            //}
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }

        private void LoadUnitsData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(this.SelectedItemId); ;
                string unit = objItem.Unit;
                cmbUnit.Items.Add(unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndex = 0;
                numConvUnit.Value = objItem.ConversionUnit;
            }
        }
        private void LoadItemData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(this.SelectedItemId);
                string unit = objItem.Unit;
                cmbUnit.Items.Add(unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndex = 0;
                numConvUnit.Value = objItem.ConversionUnit;
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.Invoke((MethodInvoker)delegate()
            //{
            //});

            //if (SharedVariables.POSItems == null)
            //{
            //using (unitOfWork = new UnitOfWork())
            //{
            //POSItems = unitOfWork.ItemRspository.GetActiveItems().ToList();
            //POSItems = unitOfWork.ItemRspository.GetActiveItems(20).ToList();
            //foreach(Item i in POSItems)
            //{
            //    grdSItems.Rows.Add(i.ItemID, i.item)
            //}
            //}
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //grdSItems.DataSource = POSItems;
            //cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.DataSource = POSItems;
            //cmbSelectItems.ValueMember = "ItemId";
            //cmbSelectItems.DisplayMember = "ItemName";
            //cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.SelectedIndex = -1;
            //cmbSelectItems.Enabled = true;
            //lblLoadingItems.Visible = false;
        }
        private void setNumericUD_toEmpty()
        {
            numRate.ResetText();
            numDiscount.ResetText();
        }
        private void ShowHoldingInvoices(int p_pageNo)
        {
            try
            {
                int totalHold = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    if (InvHoldPageNo == 0)
                    {
                        totalHold = unitOfWork.InvoiceRepository.GetHoldingInvoicesCount();
                        grpInvoiceHolding.Text = "Holding Invoices[" + InvHoldPageNo + " / " + totalHold + "]";
                    }
                    else
                    {
                        holdingInvoices = unitOfWork.InvoiceRepository.GetHoldingInvoices(p_pageNo, 1);
                        this.HoldingInvoiceId = holdingInvoices.FirstOrDefault().InvoiceId;
                        grpInvoiceHolding.Text = "Holding Invoices[" + InvHoldPageNo + " / " + holdingInvoices.PageCount + "]";
                        LoadHoldingInvoice(holdingInvoices.FirstOrDefault());
                    }
                }

                if (InvHoldPageNo == 0)
                {
                    if (totalHold > 0)
                    {
                        btnNext.Enabled = btnLastPage.Enabled = true;
                        btnPrevious.Enabled = btnFirstPage.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = btnLastPage.Enabled = false;
                        btnPrevious.Enabled = btnFirstPage.Enabled = false;
                    }
                }
                else
                {
                    btnNext.Enabled = btnLastPage.Enabled = holdingInvoices.HasNextPage;
                    if (InvHoldPageNo >= 1 || holdingInvoices.HasPreviousPage)
                    {
                        btnFirstPage.Enabled = btnPrevious.Enabled = true;
                    }
                    else
                    {
                        btnFirstPage.Enabled = btnPrevious.Enabled = false;
                    }
                }
                CalculateTotals();
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading next invoice, please try after reloading invoice screen.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            LoadBonusQuantity();
            LoadFormData();
            if (IsOnlineIntegrate && IsOnlineCancel)
            {
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colDiscountType"].ReadOnly = false;
                grdItems.Columns["colUnit"].ReadOnly = true;
                grdItems.Columns["colRate"].ReadOnly = true;
                grdItems.Columns["colRemove"].Visible = false;
                grdItems.Columns["colQuantity"].ReadOnly = true;
                grdItems.Columns["colReturnToggle"].Visible = false;
                //grdItems.Columns["DiscountedPrice"].Visible = false;
                grdItems.Columns["colAffectStock"].Visible = false;
                btnAddStock.Enabled = false;
                btnAddNewItem.Enabled = false;
            }
            else if (IsOnlineIntegrate)
            {
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colUnit"].ReadOnly = true;
                grdItems.Columns["colRate"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = false;
                //grdItems.Columns["DiscountedPrice"].Visible = false;
                grdItems.Columns["colQuantity"].ReadOnly = true;
                grdItems.Columns["colRemove"].Visible = false;
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;
                grdItems.Columns["colSaleTax"].ReadOnly = true;
                grdItems.Columns["colSaleTaxType"].ReadOnly = true;
                btnAddStock.Enabled = false;
                btnAddNewItem.Enabled = false;
            }
            else if (!createInvoice && !IsInvoiceEdit && !IsInvoicecopy)
            {
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;
                grdItems.Columns["colSaleTax"].ReadOnly = false;
                grdItems.Columns["colSaleTaxType"].ReadOnly = false;
            }
            else
            {
                if (!numTotalDiscount.Enabled || numTotalDiscount.Value == 0)
                {
                    grdItems.Columns["colDiscount"].ReadOnly = false;
                    grdItems.Columns["colDiscountType"].ReadOnly = false;
                    grdItems.Columns["colSaleTax"].ReadOnly = false;
                    grdItems.Columns["colSaleTaxType"].ReadOnly = false;
                }
                else if (createInvoice)
                {
                    grdItems.Columns["colDiscount"].ReadOnly = false;
                    grdItems.Columns["colDiscountType"].ReadOnly = false;
                    grdItems.Columns["colSaleTaxType"].ReadOnly = false;
                    grdItems.Columns["colSaleTax"].ReadOnly = false;
                }
                else
                {
                    grdItems.Columns["colDiscount"].ReadOnly = true;
                    grdItems.Columns["colDiscountType"].ReadOnly = true;
                    grdItems.Columns["colSaleTax"].ReadOnly = false;
                    grdItems.Columns["colSaleTaxType"].ReadOnly = false;
                }

            }

            if (IsInvoiceRefund)
            {
                cmbPosServiceType.Enabled = false;
            }
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                button2.Visible = false;
                cbouom.Visible = false;

            }
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
            if (get("select * from InvoiceSettings where UserID = " + SharedVariables.LoggedInUser.UserId).Rows.Count == 0)
            {
                savegridsetting();
            }
            
            //FillAttributesInDropdownList(InvoiceItemsToolStrip , SharedVariables.invoiceSetting);
            //fillgridsetting();
        }

        private void LoadBonusQuantity()
        {
            if (SharedVariables.AdminPharmacySetting.IsBonusQty)
            {
                grdItems.Columns["colBonusQuantity"].Visible = true;
            }
        }

        private void LoadStores()
        {
            this.uC_SearchItems1.StoreId = 0;
            if (IsInvoiceRefund)
            {
                cmbStores.Visible = false;
                lblStore.Visible = false;
            }
            if (!SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                cmbStores.Visible = false;
                lblStore.Visible = false;
                return;
            }
            try
            {
                this.uC_SearchItems1.StoreId = -1;
                using (unitOfWork = new UnitOfWork())
                {
                    List<StoreVM> list = new List<StoreVM>();
                    list.Add(new StoreVM { StoreId = 0, Name = "Select Store" });
                    List<StoreVM> stores = unitOfWork.StoreRepository.GetAllStores();
                    list.AddRange(stores);
                    cmbStores.DataSource = list;
                    cmbStores.DisplayMember = "Name";
                    cmbStores.ValueMember = "StoreId";
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void LoadFormData()
        {
            LoadForm(false);
            grdItems.Columns["colPCTCode"].Visible = SharedVariables.AdminPharmacySetting.IsFBRUnable;
            if (!SharedVariables.LoggedInUser.UserRoles.Any(r => r.UserRoleId == 1))
            {
                grdItems.Columns["colDiscount"].Visible = false;
                grdItems.Columns["colDiscountType"].Visible = false;
                numTotalDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
            }
            if (SharedVariables.LoggedInUser.UserRoles.Any(r => r.UserRoleId == 2) && SharedVariables.LoggedInUser.CanGiveDiscount)
            {
                grdItems.Columns["colDiscount"].Visible = true;
                grdItems.Columns["colDiscountType"].Visible = true;
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            if (!SharedVariables.AdminPharmacySetting.ShowRackNoInPOS)
            {
                grdItems.Columns["colRackNo"].Visible = false;
            }
            if (SharedVariables.AdminPharmacySetting.IsPharmacyTemps)
            {
                cmbTemplates.Visible = true;
                lblTemplate.Visible = true;
            }
            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                grdItems.Columns["colBatch"].Visible = false;
            }
        }
        private void LoadForm(bool isLocallyEditing)
        {
            isLoadingForm = true; // purpose of unregister/register events and isLoading form is same. but we can't unregister grdItems_RowsAdded event, so it will be used there for same purpose
            UnRegisterAllEvents();
            LoadStores();

            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                grdItems.Columns["colBatch"].Visible = false;
            }

            if (!isLocallyEditing)
            {
                ShowHoldingInvoices(1);
                //GridDesigner.CustomizeDataGridView(grdItems);
                SharedFunctions.SetGridStyle(grdItems, true);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddStock, btnAddNewItem, btnSave, btnSaveAndPrint, btnClearAll, button3 });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnAddtoAdv, btnAddToBal });
                if (!IsRefreshFrm)
                    this.WindowState = FormWindowState.Maximized;
            }

            if (!SharedFunctions.IsActionallowed("Edit Invoice Retail Price") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                numRate.Enabled = false;
                grdItems.Columns["colRate"].ReadOnly = true;
                //SharedFunctions.AccessDeniedMsg();
                //return;
            }
            //txtSearchByName.Enter -= txtSearchByName_Enter;
            if (!this.IsOnlineIntegrate)
            {
                cmbPmntType1.SelectedIndex = 0;
            }
            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;
            cmbTotalDiscountType.SelectedIndex = 0;
            cmbPosServiceType.SelectedIndex = 0;

            try
            {
                Tuple<double, double, double> invoiceTotals = null;
                if ((IsInvoiceEdit || IsInvoiceRefund || IsInvoicecopy) && (this.InvoiceEditId > 0 || this.InvoicecopyId > 0))
                {
                    grpInvoiceHolding.Enabled = false;
                    if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                    {
                        cmbStores.Enabled = false;
                        lblStore.Enabled = false;
                    }
                    isBalanceActive = false;
                    if (IsInvoiceRefund)
                    {
                        pnlDues.Visible = false;
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        grdItems.Columns["colReturnToggle"].Visible = true;
                        grdItems.Columns["colAffectStock"].Visible = true;
                        grdItems.Columns["colSaleTaxType"].Visible = true;
                        grdItems.Columns["colSaleTax"].Visible = true;
                        btnReturnAll.Visible = true;
                        pnlPatientDetails.Enabled = false;
                        radCalculatorDropDown1.Visible = false;
                        InvoiceItemsToolStrip.Visible = false;
                        this.Text = "Invoice Refund";
                        hideItemsDetails();
                        pnlContainer.Location = new Point(pnlContainer.Location.X, pnlContainer.Location.Y - 45);
                        grdItems.Columns["colReturnQuantity"].Visible = true;
                        pnlRefund.Visible = true;
                        numTotalDiscount.Enabled = false;
                        txtServices.Enabled = false;
                        cmbTotalDiscountType.Enabled = false;
                        grdItems.Columns["colDiscount"].ReadOnly = true;
                        grdItems.Columns["colDiscountType"].ReadOnly = true;

                        invoiceTotals = LoadInvoice(this.InvoiceEditId);
                        numTotalDiscount.Enabled = false;
                        cmbTotalDiscountType.Enabled = false;
                        duep = lblDue.Text;
                        grandtotal = lblGrandTotal.Text;
                    }
                    if (IsInvoicecopy)
                    {
                        pnlDues.Visible = true;
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = true;
                        grdItems.Columns["colReturnToggle"].Visible = false;
                        grdItems.Columns["colAffectStock"].Visible = false;
                        btnReturnAll.Visible = false;
                        pnlPatientDetails.Enabled = true;

                        //this.Text = "Copy Invoice";
                        showItemsDetails();
                        //pnlContainer.Location = new Point(pnlContainer.Location.X, pnlContainer.Location.Y - 45);
                        grdItems.Columns["colReturnQuantity"].Visible = false;
                        pnlRefund.Visible = false;
                        numTotalDiscount.Enabled = true;
                        txtServices.Enabled = true;
                        cmbTotalDiscountType.Enabled = true;
                        grdItems.Columns["colDiscount"].ReadOnly = false;
                        grdItems.Columns["colDiscountType"].ReadOnly = false;
                        invoiceTotals = LoadInvoiceCopy(this.InvoicecopyId);
                        pnlPmnt1.Enabled = true;
                        pnlPmnt2.Enabled = true;
                        pnlPmnt3.Enabled = true;
                        pnlPmnt4.Enabled = true;
                        pnlPmnt5.Enabled = true;
                        chkPmnt2.Visible = true;
                        chkPmnt2.Enabled = true;
                        pnlDue.Visible = true;
                        pnlAdvance.Visible = true;
                        panel2.Visible = true;
                    }

                    if (IsReturnedQuantityFound)
                    {
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        DisableItemsDetails();
                        if (!IsInvoicecopy)
                        {
                            pnlDue.Visible = false;
                            pnlDues.Visible = false;
                        }
                        else
                        {
                            pnlDue.Visible = true;
                            pnlDues.Visible = true;
                        }
                    }
                    if (IsInvoiceEdit)
                    {
                        pnlInvoiceEditControls.Enabled = true;
                        this.Text = "Invoice Edit";
                        //grdItems.Columns["colDiscount"].ReadOnly = false;
                        //grdItems.Columns["colDiscountType"].ReadOnly = false;
                        invoiceTotals = LoadInvoice(this.InvoiceEditId);
                        btnSave.Enabled = true;
                        btnSaveAndPrint.Enabled = true;
                        grandtotal = lblGrandTotal.Text;
                        radCalculatorDropDown1.Visible = false;
                        InvoiceItemsToolStrip.Visible = false;
                        //numTotalDiscount.Enabled = false;
                        //cmbTotalDiscountType.Enabled = false;
                        //grdItems.Columns["colQuantity"].ReadOnly = false;
                        foreach (DataGridViewRow check in grdItems.Rows)
                        {
                            if (Convert.ToDouble(check.Cells["colReturnQuantity"].Value) == 0)
                            {
                                check.Cells["colQuantity"].ReadOnly = false;
                            }
                        }
                    }

                    //commnected 20-12-2020 and added in above same condition
                    //if (IsInvoiceRefund)
                    //{
                    //    this.Text = "Invoice Refund";
                    //    //pnlAddItem.Visible = false;
                    //    hideItemsDetails();
                    //    pnlContainer.Location = new Point(pnlContainer.Location.X, pnlContainer.Location.Y - 80);
                    //    //grdItems.Location = new Point(grdItems.Location.X, grdItems.Location.Y - 80);
                    //    //grdItems.Height = 300;
                    //    grdItems.Columns["colReturnQuantity"].Visible = true;
                    //    pnlRefund.Visible = true;
                    //    numTotalDiscount.Enabled = false;
                    //    cmbTotalDiscountType.Enabled = false;
                    //}
                    CalculateTotals(); // in case of edit/return grdItems_CellValuChanges function is not called,
                }
                if (this.InvoiceEditId <= 0)
                {
                    if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                    {
                        txtServices.Value = 1;
                    }
                    if (SharedVariables.AdminPharmacySetting.POSDiscount > 0)
                    {
                        numTotalDiscount.Value = (decimal)SharedVariables.AdminPharmacySetting.POSDiscount;
                        cmbTotalDiscountType.SelectedIndex = SharedVariables.AdminPharmacySetting.POSDiscountType;
                    }
                }


                //if (this.PatientId > 0)// patient invoice
                //{
                //    LoadPateint();
                //}
                //if (!IsInvoiceRefund)// load items when its not refunding invoice
                //{
                //    LoadItems();
                //}

                if (invoiceTotals != null)// && !IsDiscountByItem)
                {
                    //numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;

                    //zahid commented
                    //numTotalDiscount.Value = (decimal)Math.Round(invoiceTotals.Item1, 2, MidpointRounding.AwayFromZero);
                    numTotalDiscount_ValueChanged(null, null);
                    //numTotalDiscount.ValueChanged -= numTotalDiscount_ValueChanged;
                    //lblGrandTotal.Text = invoiceTotals.Item2.ToString();
                    //cmbTotalDiscountType.SelectedIndex = 0;
                }

                // don't move below two lines up, otherwise auto-calculation on invoice-edit/refund will be disturbed.
                //cmbDiscountType.SelectedIndexChanged -= cmbDiscountType_SelectedIndexChanged;
                cmbDiscountType.SelectedIndex = 1; // for add item panel
                                                   //cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
                ///cmbSelectItems.SelectedIndex = -1; // for add item panel
                // don't below line anywhere else, this will updted numModifieddiscount values after automatic calculatoin

                if (invoiceTotals != null)
                {
                    numModifiedDiscount.Value = (decimal)invoiceTotals.Item3;
                }

                //if (!IsInvoiceEdit && !IsInvoiceRefund) // commented 2020-05-13
                //{
                //    cmbPmntType1.SelectedIndex = 0;
                //    cmbPmntType2.SelectedIndex = 0;
                //    cmbPmntType3.SelectedIndex = 0;
                //    cmbPmntType4.SelectedIndex = 0;
                //    cmbTotalDiscountType.SelectedIndex = 0;
                //}
                //SharedFunctions.GenerateStockConsumptionTable(dtStockConsumption);

                setNumericUD_toEmpty();
                txtAmount.Text = "";
                txtNetAmount.Text = "";
                pmodeindex = cmbPmntType1.SelectedIndex;

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            //txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
            //txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            //cmbSelectItems.Focus();
            if (!isLocallyEditing)
            {
                //GridDesigner.CustomizeDataGridView(grdItems);
                SharedFunctions.SetGridStyle(grdItems, true);
                loadTemplates();
            }
            RegisterAllEvents();

            isLoadingForm = false;

        }
        public int pmodeindex = -1;
        private void loadTemplates()
        {
            try
            {
                cmbTemplates.SelectedIndexChanged -= cmbTemplates_SelectedIndexChanged;
                List<Template> templates = new List<Template>();
                using (unitOfWork = new UnitOfWork())
                {
                    templates = unitOfWork.TemplateRepository.GetAllActiveTemplates();
                }
                Template temp = new Template();
                temp.TemplateId = 0;
                temp.Name = "Select Template";
                templates.Insert(0, temp);
                cmbTemplates.DataSource = templates;
                cmbTemplates.ValueMember = "TemplateId";
                cmbTemplates.DisplayMember = "Name";
                cmbTemplates.SelectedIndexChanged -= cmbTemplates_SelectedIndexChanged;
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading templates, please try again.", "Templates missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private DataTable getItemName(string qry, long Unique_itemId)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            com.Parameters.AddWithValue("@Unique_itemId", Unique_itemId);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        private DataTable getVoucher(string qry, string voucherName)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            com.Parameters.AddWithValue("@VoucherNumber", voucherName);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        private void DisableItemsDetails()
        {
            if (!IsInvoicecopy)
            {
                uC_SearchItems1.Enabled = false;
                lblTemplate.Enabled = false;
                cmbTemplates.Enabled = false;
                btnSearchItems.Enabled = false;
                lblSearchItem.Enabled = false;
            }

        }

        private void hideItemsDetails()
        {
            lblSearchItem.Visible = false;
            uC_SearchItems1.Visible = false;
            lblTemplate.Visible = false;
            cmbTemplates.Visible = false;
            btnSearchItems.Visible = false;
        }
        private void showItemsDetails()
        {
            lblSearchItem.Visible = true;
            uC_SearchItems1.Visible = true;
            lblTemplate.Visible = false;
            cmbTemplates.Visible = false;
            btnSearchItems.Visible = true;
        }
        private void LoadPateint()
        {
            Patient objPatient = new Patient();
            using (unitOfWork = new UnitOfWork())
            {
                objPatient = unitOfWork.PatientRepository.GetById(this.PatientId);
            }
            if (objPatient != null)
            {
                pnlPatientDetails.Enabled = false;
            }
            txtSearchByName.Text = objPatient.Name;
            txtSearchByPhone.Text = objPatient.Phone;
        }
        //below method "LoadHoldingInvoice" is duplicate of this "LoadInvoice" method. for any changes in any method make consideratino on both.
        private void UnRegisterAllEvents()
        {
            txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
            txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
            txtAddress.TextChanged -= txtAddress_TextChanged;
            numTotalDiscount.ValueChanged -= numTotalDiscount_ValueChanged;
            numModifiedDiscount.ValueChanged -= numTotalDiscount_ValueChanged;
            cmbTotalDiscountType.SelectedIndexChanged -= cmbTotalDiscountType_SelectedIndexChanged;
            dtpPmntType1.ValueChanged -= dtpPmntType1_ValueChanged;
            dtpPmntType2.ValueChanged -= dtpPmntType2_ValueChanged;
            dtpPmntType3.ValueChanged -= dtpPmntType3_ValueChanged;
            dtpPmntType4.ValueChanged -= dtpPmntType4_ValueChanged;
            dtpPmntType5.ValueChanged -= dtpPmntType5_ValueChanged;
            cmbPmntType1.SelectedIndexChanged -= cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged -= cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged -= cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged -= cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged -= cmbPmntType5_SelectedIndexChanged;
            numPmnt1.ValueChanged -= Payments_ValueChanged;
            numPmnt2.ValueChanged -= Payments_ValueChanged;
            numPmnt3.ValueChanged -= Payments_ValueChanged;
            numPmnt4.ValueChanged -= Payments_ValueChanged;
            numPmnt5.ValueChanged -= Payments_ValueChanged;
            txtInvoiceNote.TextChanged -= txtInvoiceNote_TextChanged;
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            txtServices.ValueChanged -= txtServices_ValueChanged;
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                cmbStores.SelectedIndexChanged -= cmbStores_SelectedIndexChanged;
        }
        private void RegisterAllEvents()
        {
            txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
            txtAddress.TextChanged += txtAddress_TextChanged;
            numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;
            cmbTotalDiscountType.SelectedIndexChanged += cmbTotalDiscountType_SelectedIndexChanged;
            dtpPmntType1.ValueChanged += dtpPmntType1_ValueChanged;
            dtpPmntType2.ValueChanged += dtpPmntType2_ValueChanged;
            dtpPmntType3.ValueChanged += dtpPmntType3_ValueChanged;
            dtpPmntType4.ValueChanged += dtpPmntType4_ValueChanged;
            dtpPmntType5.ValueChanged += dtpPmntType5_ValueChanged;
            cmbPmntType1.SelectedIndexChanged += cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged += cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged += cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged += cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged += cmbPmntType5_SelectedIndexChanged;
            numPmnt1.ValueChanged += Payments_ValueChanged;
            numPmnt2.ValueChanged += Payments_ValueChanged;
            numPmnt3.ValueChanged += Payments_ValueChanged;
            numPmnt4.ValueChanged += Payments_ValueChanged;
            numPmnt5.ValueChanged += Payments_ValueChanged;
            txtInvoiceNote.TextChanged += txtInvoiceNote_TextChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            txtServices.ValueChanged += txtServices_ValueChanged;
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                cmbStores.SelectedIndexChanged += cmbStores_SelectedIndexChanged;
        }
        private Tuple<double, double, double> LoadInvoice(long InvoiceId)
        {
            double sumofdiscount = 0;
            InvoiceVM objInvoice = new InvoiceVM();
            decimal chkBalance = 0;
            using (unitOfWork = new UnitOfWork())
            {
                objInvoice = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(InvoiceId);
            }
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                if (objInvoice.Store != null)
                {
                    this.uC_SearchItems1.StoreId = objInvoice.Store.StoreId;
                    cmbStores.SelectedValue = objInvoice.Store.StoreId;
                }
            }
            txtServices.Value = (decimal)objInvoice.Services;
            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            txtAddress.Text = objInvoice.Patient.Address;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceRefNo.ToString();
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;
            txtInvoiceNote.Text = objInvoice.Note;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0)
                {
                    IsReturnedQuantityFound = true;
                    grdItems.Columns["colEdit"].Visible = false;
                    grdItems.Columns["colRemove"].Visible = false;
                    DisableItemsDetails();
                    pnlDue.Visible = false;
                    pnlDues.Visible = false;
                }
                AddItemtoGrid(i);
                sumofdiscount += i.Discount;
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    if (IsInvoiceRefund && c.ColumnIndex == grdItems.Columns["colAffectStock"].Index)
                    {
                        continue;
                    }
                    if (c.ColumnIndex == grdItems.Columns["colRate"].Index && Convert.ToDouble(r.Cells["colReturnQuantity"].Value) == 0)
                    {
                        continue;
                    }
                    c.ReadOnly = true;
                }
                if (!SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    r.DefaultCellStyle.BackColor = Color.LightGray;
                }
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            cmbPosServiceType.SelectedIndex = objInvoice.ServiceType - 1;

            //numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;
            //numTotalDiscount.Value = (decimal)Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero);
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            var val = objInvoice.InvoicePayments.Select(x => x.Payment).Sum();
            //------- Can not assign balance while editing the invoice -------//
            lblBalance.Text = (objInvoice.TotalPaidAmount - objInvoice.InvoicePayments.Select(x => x.Payment).Sum()).ToString();
            lblAdvance.Text = objInvoice.Advance.ToString();
            int count = 1;

            //load patient details if available
            if (objInvoice.Patient.PatientId > 0)
            {
                pnlPatientDetails.Enabled = false;
                txtSearchByName.Text = objInvoice.Patient.Name;
                txtSearchByPhone.Text = objInvoice.Patient.Phone;
                txtAddress.Text = objInvoice.Patient.Address;
            }

            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {
                if (p.Payment < 0)
                {
                    InitialTotalRefundValue += Math.Abs(p.Payment);
                    continue;
                }
                if (count == 1)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Visible = false;
                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = true;
                        chkPmnt1.Enabled = true;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = true;
                        pnlPmnt2.Enabled = true;
                        pnlPmnt2.Visible = false;
                    }
                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 

                    //zahid change
                    chkBalance = Convert.ToDecimal(lblBalance.Text);
                    if (chkBalance > 0)
                    {
                        numPmnt1.Value = (decimal)p.Payment + chkBalance;
                        isBalanceActive = true;
                    }
                    else
                    {
                        numPmnt1.Value = (decimal)p.Payment;
                        isBalanceActive = false;
                    }
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId1.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId1.Tag = null;
                    }
                    if (p.P_InvoiceNo != "")
                    {
                        ChargeInfo ci = new ChargeInfo();
                        ci.P_InvoiceNo = p.P_InvoiceNo;
                        ci.P_RiderName = p.P_RiderName;
                        ci.P_Vendor = p.P_VendorName;
                        txtChargeInfoId1.Tag = ci;

                    }
                    else
                    {
                        txtChargeInfoId1.Tag = null;
                    }
                }
                if (count == 2)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;
                        chkPmnt2.Enabled = false;

                        chkPmnt3.Visible = false;
                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        pnlPmnt2.Enabled = true;
                        chkPmnt2.Enabled = true;
                        chkPmnt2.Checked = true;

                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = true;
                        pnlPmnt3.Enabled = true;
                        pnlPmnt3.Visible = false;
                    }

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    //txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId2.Tag = null;
                    }
                    if (p.P_InvoiceNo != "")
                    {
                        ChargeInfo ci = new ChargeInfo();
                        ci.P_InvoiceNo = p.P_InvoiceNo;
                        ci.P_RiderName = p.P_RiderName;
                        ci.P_Vendor = p.P_VendorName;
                        txtChargeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChargeInfoId2.Tag = null;
                    }
                }
                if (count == 3)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;
                        chkPmnt3.Enabled = false;

                        chkPmnt4.Visible = false;
                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;

                        pnlPmnt3.Enabled = true;
                        chkPmnt3.Enabled = true;
                        chkPmnt3.Checked = true;

                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = true;
                        pnlPmnt4.Enabled = true;
                        pnlPmnt4.Visible = false;
                    }
                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    //txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId3.Tag = null;
                    }
                    if (p.P_InvoiceNo != "")
                    {
                        ChargeInfo ci = new ChargeInfo();
                        ci.P_InvoiceNo = p.P_InvoiceNo;
                        ci.P_RiderName = p.P_RiderName;
                        ci.P_Vendor = p.P_VendorName;
                        txtChargeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChargeInfoId3.Tag = null;
                    }
                }
                if (count == 4)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;
                        chkPmnt4.Enabled = false;

                        chkPmnt5.Visible = false;
                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;

                        pnlPmnt4.Enabled = true;
                        chkPmnt4.Enabled = true;
                        chkPmnt4.Checked = true;

                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = true;
                        pnlPmnt5.Enabled = true;
                        pnlPmnt5.Visible = false;
                    }
                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    //txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId4.Tag = null;
                    }
                    if (p.P_InvoiceNo != "")
                    {
                        ChargeInfo ci = new ChargeInfo();
                        ci.P_InvoiceNo = p.P_InvoiceNo;
                        ci.P_RiderName = p.P_RiderName;
                        ci.P_Vendor = p.P_VendorName;
                        txtChargeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChargeInfoId4.Tag = null;
                    }
                }
                if (count == 5)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        chkPmnt5.Checked = true;
                        pnlPmnt5.Enabled = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;

                        pnlPmnt5.Enabled = true;
                        chkPmnt5.Enabled = true;
                        chkPmnt5.Checked = true; ;
                    }
                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    //txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId5.Tag = null;
                    }
                    if (p.P_InvoiceNo != "")
                    {
                        ChargeInfo ci = new ChargeInfo();
                        ci.P_InvoiceNo = p.P_InvoiceNo;
                        ci.P_RiderName = p.P_RiderName;
                        ci.P_Vendor = p.P_VendorName;
                        txtChargeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChargeInfoId5.Tag = null;
                    }
                }
                count++;
            }
            if (sumofdiscount > 0)
            {
                numTotalDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;
            }
            else
            {
                numTotalDiscount.Value = Convert.ToDecimal(objInvoice.TotalDiscount);
                cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;
            }
            //ShowLatestRefund();
            txtRefundAmount.Text = Math.Round(double.Parse(InitialTotalRefundValue.ToString()), 2, MidpointRounding.AwayFromZero).ToString();
            return new Tuple<double, double, double>(objInvoice.TotalDiscount, objInvoice.GrandTotal, objInvoice.ModifiedDiscount);
        }
        private Tuple<double, double, double> LoadInvoiceCopy(long InvoiceId)
        {
            double sumofdiscount = 0;
            InvoiceVM objInvoice = new InvoiceVM();
            using (unitOfWork = new UnitOfWork())
            {
                objInvoice = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(InvoiceId);
            }
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                if (objInvoice.Store != null)
                {
                    this.uC_SearchItems1.StoreId = objInvoice.Store.StoreId;
                    cmbStores.SelectedValue = objInvoice.Store.StoreId;
                }
            }
            txtServices.Value = (decimal)objInvoice.Services;
            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            txtAddress.Text = objInvoice.Patient.Address;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = (objInvoice.InvoiceRefNo + 1).ToString();
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;
            txtInvoiceNote.Text = objInvoice.Note;

            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0) { IsReturnedQuantityFound = true; }

                AddItemtoGrid(i);
                sumofdiscount += i.Discount;
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    if (IsInvoiceRefund && c.ColumnIndex == grdItems.Columns["colAffectStock"].Index)
                    {
                        continue;
                    }
                    //c.ReadOnly = true;
                }
                if (!SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    r.DefaultCellStyle.BackColor = Color.LightGray;
                }
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            cmbPosServiceType.SelectedIndex = objInvoice.ServiceType - 1;

            //numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;
            //numTotalDiscount.Value = (decimal)Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero);
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            var val = objInvoice.InvoicePayments.Select(x => x.Payment).Sum();
            //------- Can not assign balance while editing the invoice -------//
            //lblBalance.Text = (objInvoice.TotalPaidAmount - objInvoice.InvoicePayments.Select(x => x.Payment).Sum()).ToString();
            //lblAdvance.Text = objInvoice.Advance.ToString();
            int count = 1;

            //load patient details if available
            if (objInvoice.Patient.PatientId > 0)
            {
                pnlPatientDetails.Enabled = true;
                //txtSearchByName.Text = objInvoice.Patient.Name;
                //txtSearchByPhone.Text = objInvoice.Patient.Phone;
                //txtAddress.Text = objInvoice.Patient.Address;
            }

            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {
                if (p.Payment < 0)
                {
                    InitialTotalRefundValue += Math.Abs(p.Payment);
                    continue;
                }
                if (count == 1)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Visible = false;
                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = true;
                        chkPmnt1.Enabled = true;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = true;
                        pnlPmnt2.Enabled = true;
                        pnlPmnt2.Visible = false;
                    }
                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt1.Value = (decimal)p.Payment;
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId1.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId1.Tag = null;
                    }
                }
                if (count == 2)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;
                        chkPmnt2.Enabled = false;

                        chkPmnt3.Visible = false;
                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        pnlPmnt2.Enabled = true;
                        chkPmnt2.Enabled = true;
                        chkPmnt2.Checked = true;

                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = true;
                        pnlPmnt3.Enabled = true;
                        pnlPmnt3.Visible = false;
                    }

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    //txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId2.Tag = null;
                    }
                }
                if (count == 3)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;
                        chkPmnt3.Enabled = false;

                        chkPmnt4.Visible = false;
                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;

                        pnlPmnt3.Enabled = true;
                        chkPmnt3.Enabled = true;
                        chkPmnt3.Checked = true;

                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = true;
                        pnlPmnt4.Enabled = true;
                        pnlPmnt4.Visible = false;
                    }
                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    //txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId3.Tag = null;
                    }
                }
                if (count == 4)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;
                        chkPmnt4.Enabled = false;

                        chkPmnt5.Visible = false;
                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;

                        pnlPmnt4.Enabled = true;
                        chkPmnt4.Enabled = true;
                        chkPmnt4.Checked = true;

                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = true;
                        pnlPmnt5.Enabled = true;
                        pnlPmnt5.Visible = false;
                    }
                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    //txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId4.Tag = null;
                    }
                }
                if (count == 5)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        chkPmnt5.Checked = true;
                        pnlPmnt5.Enabled = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;

                        pnlPmnt5.Enabled = true;
                        chkPmnt5.Enabled = true;
                        chkPmnt5.Checked = true; ;
                    }
                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    //txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId5.Tag = null;
                    }
                }
                count++;
            }
            //ShowLatestRefund();
            if (sumofdiscount > 0)
            {

                numTotalDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colDiscountType"].ReadOnly = false;



            }
            else
            {
                numTotalDiscount.Value = Convert.ToDecimal(objInvoice.TotalDiscount);
                cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;

                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;

            }
            txtRefundAmount.Text = Math.Round(double.Parse(InitialTotalRefundValue.ToString()), 2, MidpointRounding.AwayFromZero).ToString();
            return new Tuple<double, double, double>(objInvoice.TotalDiscount, objInvoice.GrandTotal, objInvoice.ModifiedDiscount);
        }
        private void LoadHoldingInvoice(InvoiceVM objInvoice)
        {
            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            txtAddress.Text = objInvoice.Patient.Address;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceRefNo.ToString();
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;
            txtInvoiceNote.Text = objInvoice.Note;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0) { IsReturnedQuantityFound = true; }
                AddItemtoGrid(i);
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (!SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    r.DefaultCellStyle.BackColor = Color.LightGray;
                }
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            numTotalDiscount.Value = (decimal)Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero);
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            lblAdvance.Text = objInvoice.Advance.ToString();
            int count = 1;
            cmbPmntType1.SelectedIndexChanged -= cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged -= cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged -= cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged -= cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged -= cmbPmntType5_SelectedIndexChanged;

            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {

                if (count == 1)
                {

                    pnlPmnt1.Enabled = true;
                    chkPmnt1.Enabled = true;
                    chkPmnt1.Checked = true;

                    chkPmnt2.Checked = false;
                    chkPmnt2.Enabled = true;
                    pnlPmnt2.Enabled = true;
                    pnlPmnt2.Visible = false;

                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt1.Value = (decimal)p.Payment;
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId1.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId1.Tag = null;
                    }
                }
                if (count == 2)
                {

                    //pnlPmnt1.Enabled = false;
                    //chkPmnt1.Enabled = false;
                    chkPmnt1.Checked = true;

                    pnlPmnt2.Enabled = true;
                    chkPmnt2.Enabled = true;
                    chkPmnt2.Checked = true;

                    chkPmnt3.Checked = false;
                    chkPmnt3.Enabled = true;
                    pnlPmnt3.Enabled = true;
                    pnlPmnt3.Visible = false;

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    //txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId2.Tag = null;
                    }
                }
                if (count == 3)
                {

                    //pnlPmnt2.Enabled = false;
                    //chkPmnt2.Enabled = false;
                    chkPmnt2.Checked = true;

                    pnlPmnt3.Enabled = true;
                    chkPmnt3.Enabled = true;
                    chkPmnt3.Checked = true;

                    chkPmnt4.Checked = false;
                    chkPmnt4.Enabled = true;
                    pnlPmnt4.Enabled = true;
                    pnlPmnt4.Visible = false;

                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    //txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId3.Tag = null;
                    }
                }
                if (count == 4)
                {

                    //pnlPmnt3.Enabled = false;
                    //chkPmnt3.Enabled = false;
                    chkPmnt3.Checked = true;

                    pnlPmnt4.Enabled = true;
                    chkPmnt4.Enabled = true;
                    chkPmnt4.Checked = true;

                    chkPmnt5.Checked = false;
                    chkPmnt5.Enabled = true;
                    pnlPmnt5.Enabled = true;
                    pnlPmnt5.Visible = false;

                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    //txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId4.Tag = null;
                    }
                }
                if (count == 5)
                {


                    //pnlPmnt4.Enabled = false;
                    //chkPmnt4.Enabled = false;
                    chkPmnt4.Checked = true;

                    pnlPmnt5.Enabled = true;
                    chkPmnt5.Enabled = true;
                    chkPmnt5.Checked = true; ;

                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    //txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId5.Tag = null;
                    }
                }
                count++;
            }
            cmbPmntType1.SelectedIndexChanged += cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged += cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged += cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged += cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged += cmbPmntType5_SelectedIndexChanged;
        }
        private void ShowLatestRefund(InvoicePaymentVM p)
        {
            dtpRedundDate.Value = p.PaymentDate;
            txtRefundReason.Text = p.RefundReason;
            txtRefundAmount.Text = p.Payment.ToString();
        }
        private void SetDisabledColor()
        {
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
        //private void LoadItems_Async()
        //{
        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.Enabled = false;
        //        lblLoadingItems.Visible = true;
        //    }));
        //    List<Item> Items = new List<Item>();
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
        //    }

        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
        //        cmbSelectItems.DataSource = Items;
        //        cmbSelectItems.ValueMember = "ItemId";
        //        cmbSelectItems.DisplayMember = "ItemName";
        //        cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
        //        cmbSelectItems.SelectedIndex = -1;
        //    }));

        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.Enabled = true;
        //        lblLoadingItems.Visible = false;
        //    }));
        //}
        private Tuple<double, double, double, double> GetGrandTotals()
        {

            int ReturnedQty = 0;
            double subTotal = 0;
            double GrandTotal = 0;
            double ReturnTotal = 0;
            double Discount = 0;
            double totalCost = 0;
            //double TotalSaleTax = 0;
            double tax = 0;

            foreach (DataGridViewRow r in grdItems.Rows)
            {
                //if (r.Index != EditingRowIndex)
                //{
                double colSaleQty = Convert.ToDouble(r.Cells["colQuantity"].Value);
                double colRate = Convert.ToDouble(r.Cells["colRate"].Value);
                double colCost = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                double colAmount = Convert.ToDouble(r.Cells["colAmount"].Value);
                double colDiscount = Convert.ToDouble(r.Cells["colDiscount"].Value);
                //double colSaleTax = Convert.ToDouble(r.Cells["colSaleTax"].Value);

                string DiscountType = r.Cells["colDiscountType"].Value.ToString();
                ReturnedQty = Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value);
                subTotal += colAmount;
                if (DiscountType == "%")
                {
                    colDiscount = (colDiscount / 100) * colAmount;
                }
                ReturnTotal += (colRate * ReturnedQty) - ((colDiscount / colSaleQty) * ReturnedQty);
                Discount += colDiscount;
                if (colDiscount > 0)
                {
                    IsDiscountByItem = true;
                }
                totalCost += Math.Round(colCost * colSaleQty, 2);
                //}
            }
            double discount = 0;
            if (!IsDiscountByItem)
            {
                Discount = Convert.ToDouble(numTotalDiscount.Value);
                if (cmbTotalDiscountType.SelectedIndex == 1)
                {
                    discount = (Discount / 100) * subTotal;
                }
            }
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                try
                {
                    if (row.Cells["colSaleTaxType"].Value.ToString() == "%")
                    {
                        tax += (Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString()) / 100) * Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
                    }
                    else
                    {
                        tax += (Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString()));
                    }
                }
                catch (Exception) { }
            }
            subTotal = subTotal + tax;
            subTotal = Math.Round(subTotal, 2, MidpointRounding.AwayFromZero);
            Discount = Math.Round(Discount, 2, MidpointRounding.AwayFromZero);
            discount = Math.Round(discount, 2, MidpointRounding.AwayFromZero);
            ReturnTotal = Math.Round(ReturnTotal, 2, MidpointRounding.AwayFromZero);
            GrandTotal = subTotal - (cmbTotalDiscountType.SelectedIndex == 1 ? discount : Discount);

            return new Tuple<double, double, double, double>(subTotal, GrandTotal, Discount, totalCost);

        }
        public int chkgrid = 0;
        private double GetTotalCost()
        {
            chkgrid = 0;
            double TotalCost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                double colSaleQty = Convert.ToDouble(r.Cells["colQuantity"].Value);
                //double colUnitCost = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
                double colRate = Convert.ToDouble(r.Cells["colRate"].Value);
                double colReturn = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                double colCost = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                //TotalCost += ((colRate * colCost) - (colReturn * colCost));
                //TotalCost += (((colRate - colCost) * colReturn));
                if (IsInvoiceRefund && colReturn != 0 || chkgrid == 1)
                {
                    TotalCost += Math.Round(colCost * colReturn, 2);
                    chkgrid = 1;
                }
                else if (chkgrid == 0)
                {
                    TotalCost += Math.Round(colCost * colSaleQty, 2);

                }
                //TotalCost += Math.Round(colCost * colSaleQty, 2);
            }
            return TotalCost;
        }
        // no reffernce ti this
        //private void CalculateTotals()
        //{
        //    Tuple<double, double, double> GrandTotals = GetGrandTotals();
        //    double SubTotal = GrandTotals.Item1;
        //    double GrandTotal = GrandTotals.Item2;
        //    double TotalDiscount = GrandTotals.Item3;

        //    double rate = (double)numRate.Value;
        //    int qty = (int)numQuantity.Value;
        //    double amount = rate * qty;
        //    double discount = (double)numDiscount.Value;
        //    string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        //    double Refund = 0;
        //    Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
        //    if (discountType.ToLower() == "%")
        //    {
        //        discount = (discount / 100) * amount;
        //    }
        //    SubTotal += amount;
        //    GrandTotal += amount - discount + Refund;
        //    //if (IsDiscountChanged)
        //    // {
        //    TotalDiscount += discount;
        //    //}
        //    txtAmount.Text = amount.ToString();
        //    txtNetAmount.Text = (amount - discount).ToString();
        //    lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
        //    lblGrandTotal.Text = Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero).ToString();
        //    numTotalDiscount.Value = (decimal)Math.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero);
        //    numModifiedDiscount.Value = (decimal)TotalDiscount;
        //    if (!(this.IsInvoiceRefund))
        //    {
        //        if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
        //        {
        //            if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
        //            {
        //                numPmnt1.Value = (decimal)GrandTotal;
        //            }
        //        }
        //        //var NewPayable = GetEditablePaymentBox();
        //        //if (NewPayable != null)
        //        //{
        //        //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
        //        //    string n = PmntBox.Name;
        //        //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
        //        //    PmntBox.Value = (decimal)newPayment;
        //        //}
        //    }
        //    GetCustomerTotals();
        //}
        private void CalculateTotals()
        {
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double TotalDiscount = GrandTotals.Item3;
            double netProfit = GrandTotals.Item4;

            //Discount = ((Discount / 100) * UnitCost);


            // commented 2020-12-29
            //double rate = Convert.ToDouble(r.Cells["colRate"].Value);
            //int qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
            //double amount = rate * qty;
            //double discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            //int discountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
            //double Refund = 0;
            //Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            //if (discountType == 2)
            //{
            //    discount = (discount / 100) * amount;
            //}
            // commented 2020-12-29

            //SubTotal += amount;
            //GrandTotal += amount - discount + Refund;
            //TotalDiscount += discount;            
            lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
            lblGrandTotal.Text = (Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero) + Convert.ToDouble(txtServices.Value)).ToString();
            //lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
            lblDeliveryRs.Text = lblGrandTotal.Text;
            //lblNetProfit.Text = Math.Round((GrandTotal - netProfit)).ToString();
            lblNetProfit.Text = getprofit().ToString();

            double totalDisc = Math.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero); // this is special case.
            numTotalDiscount.Value = (decimal)totalDisc;
            numModifiedDiscount.Value = (decimal)TotalDiscount;


            decimal gt = 0;
            if (!(this.IsInvoiceRefund))
            {
                if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                    {
                        decimal.TryParse(lblGrandTotal.Text, out gt);
                        numPmnt1.Value = gt + txtServices.Value;
                    }
                }
                //else
                //{
                //    if (this.LoadedInvoice_TotalPaid == 0)
                //    {
                //        if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                //        {
                //            decimal.TryParse(lblGrandTotal.Text, out gt);
                //            numPmnt1.Value = gt + txtServices.Value;
                //        }
                //    }
                //}

                //var NewPayable = GetEditablePaymentBox();
                //if (NewPayable != null)
                //{
                //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
                //    string n = PmntBox.Name;
                //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
                //    PmntBox.Value = (decimal)newPayment;
                //}
            }
            //calculateSaleTax();
            GetCustomerTotals();
        }
        private Tuple<Control, double> GetEditablePaymentBox()
        {
            double TotalPaid = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            if (numPmnt1.Enabled && numPmnt1.Visible) return new Tuple<Control, double>(numPmnt1, TotalPaid);
            if (numPmnt2.Enabled && numPmnt2.Visible) return new Tuple<Control, double>(numPmnt2, TotalPaid);
            if (numPmnt3.Enabled && numPmnt3.Visible) return new Tuple<Control, double>(numPmnt3, TotalPaid);
            if (numPmnt4.Enabled && numPmnt4.Visible) return new Tuple<Control, double>(numPmnt4, TotalPaid);
            if (numPmnt5.Enabled && numPmnt5.Visible) return new Tuple<Control, double>(numPmnt5, TotalPaid);
            else return null;
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
        public int RowIndex123 = -1;
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.isNewItemAdded = true;
                if (IsValidInput())
                {
                    //InvoiceItem objInvoiceItem = new InvoiceItem();
                    ////FillObject(objInvoiceItem);
                    //if (EditingRowIndex >= 0)
                    //{
                    //    if (!isValidQuantityEntered())
                    //    {
                    //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //        numQuantity.Focus();
                    //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                    //        return;
                    //    }
                    //    UpdateRow(objInvoiceItem);
                    //}
                    //else if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                    //{
                    //    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    //else
                    //{
                    //    if (!isValidQuantityEntered())
                    //    {
                    //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //        numQuantity.Focus();
                    //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                    //        return;
                    //    }
                    //    AddItemtoGrid(objInvoiceItem);
                    //}
                    //Clear();
                    //cmbSelectItems.Focus();

                    foreach (DataGridViewRow row in grdItems.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["colItemId"].Value.ToString()) == uC_SearchItems1.SelectedItemId)
                        {
                            RowIndex123 = row.Index;
                            continue;
                        }
                    }
                    // adding row with default values to avoid any validatio errors
                    //grdItems.CurrentCellDirtyStateChanged -= grdItems_CurrentCellDirtyStateChanged;
                    if (!SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS && RowIndex123 != -1)
                    {
                        grdItems.Rows[RowIndex123].Cells["colQuantity"].Value = (Convert.ToInt32(grdItems.Rows[RowIndex123].Cells["colQuantity"].Value) + 1).ToString();
                        RowIndex123 = -1;
                        return;
                    }
                    else
                    {
                        RowIndex123 = -1;
                    }
                    //
                    if (this.cmbTemplates.SelectedIndex > 0)
                    {
                        #region AdddTemplateItems
                        TemplateVM res = GetTemplateItems(Convert.ToInt32(this.cmbTemplates.SelectedValue));
                        foreach (TemplateItemVm i in res.TemplateItems)
                        {
                            if (ItemAlreadyAdded(i.Item.ItemID))
                            {
                                MessageBox.Show("Selected Item (" + i.Item.ItemName + ") Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }



                            grdItems.Rows.Add(
                                0, //SrNo
                                0, //invoiceitemid
                                i.Item.ItemID, //itemid
                                "", //itemname
                                "", //PCT Code
                                "", // batch
                                0, //batchid
                                LoadItemRackName(i.Item.ItemID),//i.Item.Rack != null ? i.Item.Rack.Name : "",
                                "", //unit
                                1, //conversionunit
                                0, //DiscountedPrice
                                0, //unitcost
                                0, //rate
                                0, //ProfitMargin
                                0, //available quantity
                                i.Quantity, //quantity
                                0, //bonus quantity
                                0, //amount
                                0, //net amount
                                0, //discount
                                "%", //disocunttype
                                0, //saletax
                                "%", //saletaxtype
                                true, //isnewrow
                                0, //return
                                0, //initial return
                                false, //affect stock
                                false // isOptionalBatchNo
                                );
                        }
                        #endregion
                    }
                    else
                    {
                        //---------Item Already Exists----------//
                        if (ItemAlreadyAdded(this.SelectedItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                        {
                            MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //uC_SearchItems1.SetText = "";                       
                            uC_SearchItems1.SetFocus();
                            return;
                        }

                        #region AddSearchItem
                        //using (unitOfWork = new UnitOfWork())
                        //{
                        //    un
                        //}
                        //if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                        this.isNewItemAdded = true;
                        Double UnitCost = 0;
                        Double DiscountedCheck = 0;
                        Double DiscountedPrice = 0;
                        string batchname = "";
                        Double Rate = 0;
                        int unit = 0;
                        int btch = 0;
                        DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + SelectedItemId);
                        DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + SelectedItemId);
                        if (dt.Rows.Count > 0)
                        {
                            int qty = 0;
                            Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                            {
                                UnitCost = Convert.ToDouble(dt.Rows[0]["UnitCost"].ToString());
                            }
                            else
                            {
                                UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                            }

                            Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                            int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                            unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                            unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                            {
                                btch = Convert.ToInt32(dt.Rows[0]["Batch_BatchId"].ToString());
                            }
                            else
                            {
                                btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                            }
                            DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                            {
                                batchname = batch.Rows[0][0].ToString();
                                qty = Convert.ToInt32(dt.Rows[0]["Quantity"].ToString());
                                Rate = Convert.ToDouble(dt.Rows[0]["RetailPrice"].ToString());
                            }
                            else
                            {
                                batchname = batch.Rows[batch.Rows.Count - 1][0].ToString();
                                qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                                Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                            }
                            //qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                            //Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                            UnitCost = UnitCost;
                            if (DiscountType == 0)
                            {
                                Discount = ((Discount / 100) * UnitCost);
                                if (unit == 0)
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        //DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                                        DiscountedPrice = Math.Round(((UnitCost - Discount) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                                    }
                                    else
                                    {
                                        DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);
                                    }
                                }
                                else
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                        DiscountedPrice = Math.Round((((UnitCost)) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                                    }
                                    else
                                    {
                                        DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                        DiscountedPrice = Math.Round((UnitCost) - Discount, 2);
                                    }
                                }
                            }
                            else
                            {
                                if (unit == 0)
                                {

                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                        }
                                    }
                                    else
                                    {
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) - Discount / qty), 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);

                                        }
                                    }
                                }
                                else
                                {
                                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                                    {
                                        Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                        }
                                    }
                                    else
                                    {
                                        DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                        if (DiscountType != 0)
                                        {
                                            DiscountedPrice = Math.Round((UnitCost) - Discount / qty, 2);
                                        }
                                        else
                                        {
                                            DiscountedPrice = Math.Round((UnitCost) - Discount, 2);

                                        }
                                    }
                                }
                            }

                        }
                        grdItems.Rows.Add(
                            0, //SrNo,
                            0,//invoiceitemid
                            this.SelectedItemId,//itemid
                            "",//itemname
                            "", //PCT Code
                            batchname,//batch
                            btch,//btachid
                            LoadItemRackName(this.SelectedItemId), //"",// rackno
                            unit,//unit
                            1,//conv unit
                            0,//DiscountedPrice
                            DiscountedPrice,//unitcot
                            0,//retail price
                            Profit(Rate, DiscountedPrice), //ProfitMargin
                            0,//available qty
                            1,//qty
                            0, //bonus qty
                            0,//amount
                            0,//net amount
                            0,//discount
                            "%", //discount type
                            0,
                            "%",
                            true,//is new row
                            0,//return
                            0,//initial return
                            false,// affect stock
                            false // isoptionalbatchno.
                            );
                        #endregion
                    }
                    btnClear.PerformClick();
                    uC_SearchItems1.SetFocus();
                    grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];
                    grdItems.BeginEdit(true);
                    int index = grdItems.CurrentRow.Index;
                    grdItems.FirstDisplayedScrollingRowIndex = index;
                    grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    SelectedItemId = 0;
                    getprofit();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void copyLastInvoice()
        {
            try
            {

                this.isNewItemAdded = true;
                DataTable Invoices = get("select InvoiceId from Invoices");
                int InvoiceId = Convert.ToInt32(Invoices.Rows[Invoices.Rows.Count - 1][0].ToString());
                DataTable InvoiceItems = get("Select Item_ItemId , Quantity , Batch_BatchId ,  Unit from InvoiceItems where Invoice_InvoiceId=" + InvoiceId);

                for (int i = 0; i < InvoiceItems.Rows.Count; i++)
                {
                    //this.SelectedItemId = Convert.ToInt32(InvoiceItems.Rows[i][0].ToString());

                    this.SelectedItemId = Convert.ToInt32(InvoiceItems.Rows[i][0].ToString());
                    int invqty = Convert.ToInt32(InvoiceItems.Rows[i][1].ToString());
                    int invbtch = Convert.ToInt32(InvoiceItems.Rows[i][2].ToString());
                    int invUnit = Convert.ToInt32(InvoiceItems.Rows[i][3].ToString());
                    string unitstring = invUnit == 0 ? "Pack" : "Units";

                    if (IsValidInput())
                    {
                        //InvoiceItem objInvoiceItem = new InvoiceItem();
                        ////FillObject(objInvoiceItem);
                        //if (EditingRowIndex >= 0)
                        //{
                        //    if (!isValidQuantityEntered())
                        //    {
                        //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //        numQuantity.Focus();
                        //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        //        return;
                        //    }
                        //    UpdateRow(objInvoiceItem);
                        //}
                        //else if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                        //{
                        //    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        //else
                        //{
                        //    if (!isValidQuantityEntered())
                        //    {
                        //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //        numQuantity.Focus();
                        //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        //        return;
                        //    }
                        //    AddItemtoGrid(objInvoiceItem);
                        //}
                        //Clear();
                        //cmbSelectItems.Focus();

                        // adding row with default values to avoid any validatio errors
                        //grdItems.CurrentCellDirtyStateChanged -= grdItems_CurrentCellDirtyStateChanged;

                        //


                        //---------Item Already Exists----------//
                        if (ItemAlreadyAdded(this.SelectedItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                        {
                            MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //uC_SearchItems1.SetText = "";                       
                            uC_SearchItems1.SetFocus();
                            return;
                        }

                        #region AddSearchItem
                        //using (unitOfWork = new UnitOfWork())
                        //{
                        //    un
                        //}
                        //if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                        this.isNewItemAdded = true;
                        Double UnitCost = 0;
                        Double DiscountedCheck = 0;
                        Double DiscountedPrice = 0;
                        string batchname = "";
                        Double Rate = 0;
                        int unit = 0;
                        int btch = 0;
                        int qty = 1;
                        DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + SelectedItemId);
                        DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + SelectedItemId);
                        if (dt.Rows.Count > 0)
                        {

                            Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                            UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                            Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                            int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                            unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                            unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                            btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                            DataTable batch = get("Select BatchName from Batches where BatchId=" + invbtch);
                            batchname = batch.Rows[0][0].ToString();
                            qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                            Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                            if (invUnit == 0 && SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                invqty = invqty / Convert.ToInt32(dt1.Rows[0][0].ToString());
                            }
                            if (DiscountType == 0)
                            {
                                Discount = ((Discount / 100) * subTotal);
                                if (unit == 0)
                                {
                                    DiscountedPrice = Math.Round(((subTotal - Discount) / qty), 2);
                                }
                                else
                                {
                                    DiscountedCheck = Math.Round(((subTotal - Discount) / qty), 2);
                                    DiscountedPrice = Math.Round(((subTotal - Discount) / qty) * Convert.ToInt32(dt1.Rows[0][0].ToString()), 2);
                                }
                            }
                            else
                            {
                                if (unit == 0)
                                {
                                    DiscountedPrice = Math.Round(((subTotal - Discount) / qty), 2);
                                }
                                else
                                {
                                    DiscountedCheck = Math.Round(((subTotal - Discount) / qty), 2);
                                    DiscountedPrice = Math.Round(((subTotal - Discount) / qty) * Convert.ToInt32(dt1.Rows[0][0].ToString()), 2);
                                }
                            }

                        }
                        grdItems.Rows.Add(
                            0, //SrNo,
                            0,//invoiceitemid
                            this.SelectedItemId,//itemid
                            "",//itemname
                            "", //PCT Code
                            batchname,//batch
                            btch,//btachid
                            LoadItemRackName(this.SelectedItemId), //"",// rackno
                            unit,//unit
                            1,//conv unit
                            0,//DiscountedPrice
                            DiscountedPrice,//unitcot
                            0,//retail price
                            Profit(Rate, DiscountedPrice), //ProfitMargin
                            0,//available qty
                            invqty,//qty
                            0, //bonus qty
                            0,//amount
                            0,//net amount
                            0,//discount
                            "%", //discount type
                            0,
                            "%",
                            true,//is new row
                            0,//return
                            0,//initial return
                            false,// affect stock
                            false // isoptionalbatchno.
                            );

                        #endregion
                    }
                    btnClear.PerformClick();
                    //uC_SearchItems1.SetFocus();
                    grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];

                    grdItems.BeginEdit(true);
                    int index = grdItems.CurrentRow.Index;
                    grdItems.FirstDisplayedScrollingRowIndex = index;



                }
                //SelectedItemId = 0;

            }
            catch (Exception)
            {

            }



        }

        private TemplateVM GetTemplateItems(int templateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.TemplateRepository.getTemplateById(templateId);
            }
        }
        private void UpdateRow(InvoiceItem objInvoiceItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objInvoiceItem.BatchId;
            r.Cells["colBatch"].Value = objInvoiceItem.BatchName;
            r.Cells["colUnit"].Value = objInvoiceItem.UnitString;
            r.Cells["colConvUnit"].Value = objInvoiceItem.ConvUnit;
            r.Cells["colRate"].Value = objInvoiceItem.Rate;
            r.Cells["colQuantity"].Value = objInvoiceItem.Quantity;
            r.Cells["colAmount"].Value = objInvoiceItem.Amount;
            r.Cells["colNetAmount"].Value = objInvoiceItem.NetAmount;
            r.Cells["colDiscount"].Value = objInvoiceItem.Discount;
            r.Cells["colDiscountType"].Value = objInvoiceItem.DiscountTypeString;
            r.Cells["colDiscountTypeId"].Value = objInvoiceItem.DiscountType;
            DataGridViewRowsAddedEventArgs e = new DataGridViewRowsAddedEventArgs(EditingRowIndex, 1);
            grdItems_RowsAdded(null, e);
            //r.Cells["colIsNewRow"].Value = true;
        }
        private void AddItemtoGrid(InvoiceItemVM objInvoiceItem)
        {
            double DiscountPrice = 0;
            objInvoiceItem.UnitString = objInvoiceItem.Unit == 0 ? objInvoiceItem.Item.Unit : "Units";
            double RateSet = objInvoiceItem.Rate;
            objInvoiceItem.Rate = objInvoiceItem.Unit == 0 ? objInvoiceItem.Rate * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.Rate;
            //objInvoiceItem.DiscountedPrice = objInvoiceItem.Unit == 1 ? objInvoiceItem.DiscountedPrice / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.DiscountedPrice;
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                DiscountPrice = objInvoiceItem.Unit == 0 ? objInvoiceItem.DiscountedPrice * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.DiscountedPrice;
            }
            else
            {
                DiscountPrice = GetDiscountedPrice(objInvoiceItem);
            }
            if (IsOnlineIntegrate && IsOnlineCancel)
            {
                grdItems.Rows.Add
                (
                    0,
                    objInvoiceItem.InvoiceItemId,
                    objInvoiceItem.ItemId,
                    objInvoiceItem.ItemName,
                    objInvoiceItem.Item.PCTCode,
                    objInvoiceItem.BatchName,
                    objInvoiceItem.BatchId,
                    objInvoiceItem.Rack,
                    objInvoiceItem.UnitString,
                    objInvoiceItem.Item.ConversionUnit,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.UnitCost * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.UnitCost,
                    objInvoiceItem.DiscountedPrice = DiscountPrice,
                    objInvoiceItem.Rate,
                    objInvoiceItem.ProfitMargin = Convert.ToDouble(Profit(RateSet, objInvoiceItem.UnitCost)),
                    0,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.Quantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.Quantity,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.BonusQuantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.BonusQuantity,
                    objInvoiceItem.Amount,
                    objInvoiceItem.NetAmount,
                    objInvoiceItem.Discount,
                    objInvoiceItem.DiscountType == 1 ? "%" : "Value",
                    objInvoiceItem.SaleTax,
                    objInvoiceItem.SaleTaxType == 0 ? "%" : "Value",
                    false,
                    objInvoiceItem.Quantity,
                    objInvoiceItem.Quantity,
                    objInvoiceItem.AffectStock,
                    objInvoiceItem.IsOptionalBatchNo
                );
            }
            else
            {
                grdItems.Rows.Add
                (
                    0,
                    objInvoiceItem.InvoiceItemId,
                    objInvoiceItem.ItemId,
                    objInvoiceItem.ItemName,
                    objInvoiceItem.Item.PCTCode,
                    objInvoiceItem.BatchName,
                    objInvoiceItem.BatchId,
                    objInvoiceItem.Rack,
                    objInvoiceItem.UnitString,
                    objInvoiceItem.Item.ConversionUnit,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.UnitCost * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.UnitCost,
                    objInvoiceItem.DiscountedPrice = DiscountPrice,
                    objInvoiceItem.Rate,
                    objInvoiceItem.ProfitMargin = Convert.ToDouble(Profit(RateSet, objInvoiceItem.UnitCost)),
                    0,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.Quantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.Quantity,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.BonusQuantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.BonusQuantity,
                    objInvoiceItem.Amount,
                    objInvoiceItem.NetAmount,
                    objInvoiceItem.Discount,
                    objInvoiceItem.DiscountType == 1 ? "%" : "Value",
                    objInvoiceItem.SaleTax,
                    objInvoiceItem.SaleTaxType == 0 ? "%" : "Value",
                    false,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.ReturnedQuantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.ReturnedQuantity,
                    objInvoiceItem.Unit == 0 ? objInvoiceItem.ReturnedQuantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.ReturnedQuantity,
                    objInvoiceItem.AffectStock,
                    objInvoiceItem.IsOptionalBatchNo
                );
            }
        }
        public double GetDiscountedPrice(InvoiceItemVM objInvoiceItem)
        {
            if (objInvoiceItem.Unit == 0)
            {
                return objInvoiceItem.UnitCost * objInvoiceItem.Item.ConversionUnit;
            }
            else
            {
                return objInvoiceItem.UnitCost;
            }
        }
        private void FillObject(InvoiceItem objInvoiceItem)
        {
            objInvoiceItem.ItemId = this.SelectedItemId;//Convert.ToInt32(cmbSelectItems.SelectedValue);
            objInvoiceItem.ItemName = this.SelectedItemName; //cmbSelectItems.GetItemText(cmbSelectItems.SelectedItem);
            objInvoiceItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objInvoiceItem.ConvUnit = (int)numConvUnit.Value;
            objInvoiceItem.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            objInvoiceItem.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objInvoiceItem.Rate = (double)numRate.Value;
            objInvoiceItem.Quantity = (int)numQuantity.Value;
            objInvoiceItem.Amount = double.Parse(txtAmount.Text);
            objInvoiceItem.NetAmount = Math.Round(double.Parse(txtNetAmount.Text), 2, MidpointRounding.AwayFromZero);
            objInvoiceItem.Discount = (double)numDiscount.Value;
            objInvoiceItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
            objInvoiceItem.DiscountTypeString = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        }
        private void FillObject(InvoiceItem objInvoiceItem, DataGridViewRow r)
        {
            objInvoiceItem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objInvoiceItem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            objInvoiceItem.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objInvoiceItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            objInvoiceItem.DiscountedPrice = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
            objInvoiceItem.ConvUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            objInvoiceItem.Rate = Convert.ToDouble(r.Cells["colRate"].Value);
            objInvoiceItem.Rate = objInvoiceItem.Unit == 0 ? objInvoiceItem.Rate / objInvoiceItem.ConvUnit : objInvoiceItem.Rate; // to save quantity always in broken form.  
            objInvoiceItem.PerUnitCostPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
            if (!IsInvoiceEdit && !IsInvoiceRefund)
            {
                objInvoiceItem.DiscountPrice = objInvoiceItem.Unit == 0 ? Convert.ToDouble(r.Cells["DiscountedPrice"].Value) / objInvoiceItem.ConvUnit : Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
            }
            else
            {
                objInvoiceItem.DiscountPrice = objInvoiceItem.Unit == 0 ? Convert.ToDouble(r.Cells["DiscountedPrice"].Value) / objInvoiceItem.ConvUnit : Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
            }
            objInvoiceItem.PerUnitCostPrice = objInvoiceItem.Unit == 0 ? objInvoiceItem.PerUnitCostPrice / objInvoiceItem.ConvUnit : objInvoiceItem.PerUnitCostPrice; // to save quantity always in broken form.
            if (IsInvoiceEdit && Convert.ToInt32(r.Cells["colReturnQuantity"].Value) > 0)
            {
                objInvoiceItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value) + Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
            }
            else
            {
                objInvoiceItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
            }
            objInvoiceItem.Quantity = objInvoiceItem.Unit == 0 ? objInvoiceItem.Quantity * objInvoiceItem.ConvUnit : objInvoiceItem.Quantity; // to save quantity always in broken form.                        
            objInvoiceItem.BonusQuantity = SharedFunctions.StringToInt(r.Cells["colBonusQuantity"].Value);
            objInvoiceItem.BonusQuantity = objInvoiceItem.Unit == 0 ? objInvoiceItem.BonusQuantity * objInvoiceItem.ConvUnit : objInvoiceItem.BonusQuantity; // to save quantity always in broken form.                        
            objInvoiceItem.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            objInvoiceItem.Discount = objInvoiceItem.Unit == 0 ? objInvoiceItem.Discount / objInvoiceItem.ConvUnit : objInvoiceItem.Discount; // to save quantity always in broken form.
            objInvoiceItem.TotalDiscPrice = (Convert.ToDouble(r.Cells["DiscountedPrice"].Value) * Convert.ToInt32(r.Cells["colQuantity"].Value));
            objInvoiceItem.Amount = Convert.ToDouble(r.Cells["colAmount"].Value);
            objInvoiceItem.NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);
            objInvoiceItem.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            objInvoiceItem.DiscountType = r.Cells["colDiscountType"].Value.ToString() == "%" ? 1 : 0;//   Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
            objInvoiceItem.PerUnitCostPrice = objInvoiceItem.PerUnitCostPrice;
            objInvoiceItem.Rate = objInvoiceItem.Rate;
            objInvoiceItem.saletaxType = r.Cells["colSaleTaxType"].Value.ToString() == "%" ? 0 : 1;
            objInvoiceItem.saletax = Convert.ToDouble(r.Cells["colSaleTax"].Value.ToString());
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (this.SelectedItemId <= 0 && cmbTemplates.SelectedIndex <= 0)// (cmbSelectItems.SelectedValue == null)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                //cmbSelectItems.Focus();
                this.uC_SearchItems1.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                ErrSelectItem.Visible = false;
            }

            //if (cmbSelectBatch.SelectedValue == null)
            //{
            //    ErrSelectBatch.Visible = true;
            //    ErrFound = true;
            //    cmbSelectBatch.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrSelectBatch.Visible = false;
            //}
            //if (Convert.ToDouble(numRate.Value) <= 0)
            //{
            //    errRate.Visible = true;
            //    ErrFound = true;
            //    numRate.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    errRate.Visible = false;
            //}
            //if (Convert.ToInt32(numQuantity.Value) <= 0)
            //{
            //    ErrQuantity.Visible = true;
            //    ErrFound = true;
            //    numQuantity.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrQuantity.Visible = false;
            //}

            //if (Convert.ToInt32(numQuantity.Value) > int.Parse(txtAvlQty.Text))
            //{
            //    ErrQuantity.Visible = true;
            //    ErrFound = true;
            //    numQuantity.Focus();
            //    MessageBox.Show("Please enter valid quantity", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrQuantity.Visible = false;
            //}

            //if (!IsCorrectItemDiscount())
            //{
            //    Errdisc.Visible = true;
            //    ErrMessage.Text = "Incorrect Discount Values.";
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    Errdisc.Visible = false;
            //}

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
        private void AddToDeletedItems(DataGridViewRow r)
        {
            InvoiceItemVM DeletedITem = new InvoiceItemVM();
            DeletedITem.InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);
            DeletedITem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            DeletedITem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            DeletedInvoiceItems.Add(DeletedITem);
        }
        private void RegisterEvents(bool Register)
        {
            if (Register)
            {
                numRate.ValueChanged += numRate_ValueChanged;
                numQuantity.ValueChanged += numQuantity_ValueChanged;
                numDiscount.ValueChanged += numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
            }
            else
            {
                numRate.ValueChanged -= numRate_ValueChanged;
                numQuantity.ValueChanged -= numQuantity_ValueChanged;
                numDiscount.ValueChanged -= numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged -= cmbDiscountType_SelectedIndexChanged;
            }
        }
        //private void FillForm(DataGridViewRow r)
        //{

        //    this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
        //        cmbUnit.Items.Add(unit);
        //        cmbUnit.Items.Add("Units");
        //    }
        //    cmbUnit.SelectedIndex = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
        //    numConvUnit.Value = Convert.ToInt32(r.Cells["colConvUnit"].Value);
        //    this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
        //    this.uC_SearchItems1.SetText = r.Cells["colItemName"].Value.ToString();
        //    this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        //    GetItemBatches(this.SelectedItemId);
        //    //.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);

        //    cmbSelectBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
        //    numRate.Value = Convert.ToDecimal(r.Cells["colRate"].Value);
        //    numQuantity.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
        //    numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
        //    cmbDiscountType.SelectedItem = r.Cells["colDiscounttype"].Value.ToString();
        //    txtAmount.Text = r.Cells["colAmount"].Value.ToString();
        //    txtNetAmount.Text = r.Cells["colNetAmount"].Value.ToString();
        //}
        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    //&& Convert.ToInt32(r.Cells["colBatchId"].Value) == BatchId)
                    )
                {
                    return true;
                }
            }
            return false;
            //return false;
        }
        //private void LoadItemBatches(int ItemId)
        //{
        //    cmbSelectBatch.DataSource = null;
        //    cmbSelectBatch.SelectedIndex = -1;
        //    if (ItemId >= 0)
        //    {
        //        cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //        GetItemBatches(ItemId);
        //        cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    }
        //    if (cmbSelectBatch.Items.Count > 0)
        //    {sender, EventArgs e)
        //{
        //    cmbSelectBatch.DataSource = null;
        //    cmbSelectBatch.SelectedIndex = -1;
        //    if (cmbSelectItems.SelectedIndex >= 0)
        //    {
        //        cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //        GetItemBatches(Convert.ToInt32(cmbSelectItems.SelectedValue));
        //        cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    }
        //    if (cmbSelectBatch.Items.Count > 0)
        //    {
        //        cmbSelectBatch.SelectedIndex = 0;
        //        cmbSelectBatch_SelectedIndexChanged(null, null);
        //    }
        //}
        //private void GetItemBatches(int ItemId)
        //{
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        BatchStockList = unitOfWork.ItemRspository.GetBatchStockByItemId(ItemId);
        //    }
        //    List<BatchStockVM> FinalResult = new List<BatchStockVM>();
        //    foreach (BatchStockVM i in BatchStockList)
        //    {
        //        i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
        //        i.BatchName = i.BatchName;// +" | Available Stock : " + i.AvailableStock;
        //        if (i.AvailableStock > 0)
        //        {
        //            FinalResult.Add(i);
        //        }
        //    }
        //    cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //    cmbSelectBatch.DataSource = FinalResult;
        //    cmbSelectBatch.ValueMember = "BatchId";
        //    cmbSelectBatch.DisplayMember = "BatchName";
        //    cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    if (this.cmbSelectBatch.Items.Count > 0)
        //    {
        //        cmbSelectBatch.SelectedIndex = -1; // to trigger in case there is single item in combobox
        //        cmbSelectBatch.SelectedIndex = 0;
        //    }
        //}
        private FlatDiscount loadFlatDiscount(int itemId, DataGridViewRow r)
        {
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            return SharedVariables.TodayDiscounts
                .Where(
                            d =>
                            (
                                d.IsAllItems
                                || d.DiscountItems.Any(i => i.Item.ItemID == itemId)
                            )
                            &&
                            (
                                d.IsAllTimes
                                ||
                                (
                                    DateTime.Now.TimeOfDay >= d.StartTime.TimeOfDay
                                    && DateTime.Now.TimeOfDay <= d.EndTime.TimeOfDay
                                )
                            )
                            &&
                            (
                                d.IsAllDays
                                ||
                                d.SelectedDays.Contains(dayOfWeek.ToString())
                            )
                        )
                        .LastOrDefault();
        }

        private void GetSelectedItemData(int ItemId, DataGridViewRow r)
        {
            int qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
            int avQty = 0;
            double Rate = 0;
            double unitCost = 0;

            //setQuantity Initial
            if (IsInvoiceEdit || IsInvoicecopy)
            {
                IsEditAllow = true;
                if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) == 0)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
                else
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    double reQty = 0;
                    double setQty = 0;
                    double qty1 = 0;
                    reQty = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                    qty1 = Convert.ToDouble(r.Cells["colQuantity"].Value);
                    setQty = qty1 - reQty;
                    r.Cells["colQuantity"].Value = setQty;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
            }

            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            LoadSerial();
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();
            using (unitOfWork = new UnitOfWork())
            {
                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStoreWise(ItemId, SharedFunctions.StringToInt(cmbStores.SelectedValue), SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                else
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                AddedItemsData.Add(itemDetail); // this list will be used  at time of saving invoice to reduce db cost.
                BatchStockList = itemDetail.BatchStockList.ToList();
            }
            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            r.Cells["colPCTCode"].Value = itemDetail.Item.PCTCode;
            //r.Cells["colRackNo"].Value = itemDetail.Item.Rack == null ? "" : itemDetail.Item.Rack.Name;
            //if (!isLoadingform)
            //{
            //    r.Cells["colRate"].Value = itemDetail.RetailPrice * itemDetail.Item.ConversionUnit;
            //}
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();

            foreach (BatchStockVM i in BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);

                //i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                //{
                //    i.AvailableStock += i.HoldStock;
                //}
                //double avlStock = i.AvailableStock;
                if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                {
                    i.AvailableStock = i.AvailableStock / itemDetail.Item.ConversionUnit;
                }

                i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";

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



            //FinalResult = FinalResult.OrderBy(x => x.BatchId).ToList();
            //r.Cells["colBatch"].Value = FinalResult[FinalResult.Count -1].BatchName;
            //r.Cells["colBatchId"].Value = FinalResult[FinalResult.Count - 1].BatchId;
            //WCH
            if (SharedVariables.AdminPharmacySetting.AllowNegCons)
            {
                if (BatchStockList != null && BatchStockList.Count() > 0 && FinalResult.Count() <= 0)
                {
                    FinalResult.Add(BatchStockList.OrderByDescending(x => x.BatchId).FirstOrDefault());
                }
            }
            DataTable dt;
            if (FinalResult[0].BatchName.Contains("Opening Stock"))
            {
                dt = get("Select Batch_BatchId , Stock_StockId  from StockItems where Item_ItemID=" + SelectedItemId + " and (Batch_BatchId=" + r.Cells["colBatchId"].Value + " or Stock_StockId = Batch_BatchId)");
            }
            else
            {
                dt = get("Select Batch_BatchId from StockItems where Item_ItemID=" + SelectedItemId + " and Batch_BatchId=" + r.Cells["colBatchId"].Value);
            }

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
                        r.Cells["colBatchId"].Value = dt.Rows[0][0].ToString();
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(dt.Rows[0][0].ToString()))
                            {
                                r.Cells["colBatch"].Value = i.BatchName;
                            }
                        }
                        string contanBrack = r.Cells["colBatch"].Value.ToString();
                        if (!contanBrack.Contains("["))
                        {
                            r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                            r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
                        }
                    }
                    else
                    {
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                r.Cells["colBatch"].Value = i.BatchName;
                            }
                        }
                        string contanBrack = r.Cells["colBatch"].Value.ToString();
                        if (!contanBrack.Contains("["))
                        {
                            r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                            r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
                        }
                    }

                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                    {
                        //avQty = (int)Math.Floor((double)(FinalResult[0].AvailableStock / itemDetail.Item.ConversionUnit));
                        //avQty = (int)FinalResult[FinalResult.Count-1].AvailableStock;
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                avQty = i.AvailableStock;
                                unitCost = i.CostPrice;
                                Rate = i.RetailPrice * itemDetail.Item.ConversionUnit;
                            }
                        }
                        //if (avQty == 0)
                        //{
                        //    avQty = (int)FinalResult[0].AvailableStock;
                        //    unitCost = FinalResult[0].CostPrice;
                        //    Rate = FinalResult[0].RetailPrice * itemDetail.Item.ConversionUnit;
                        //}
                        //Rate = itemDetail.Item.RetailPrice * itemDetail.Item.ConversionUnit;
                        //unitCost = FinalResult[0].CostPrice * itemDetail.Item.ConversionUnit;
                        //unitCost = itemDetail.Item.UnitCostPrice;

                    }
                    else
                    {
                        //Rate = itemDetail.Item.RetailPrice;
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                avQty = i.AvailableStock;
                                unitCost = i.CostPrice;
                                Rate = i.RetailPrice;
                            }
                        }
                        if (avQty == 0)
                        {
                            avQty = FinalResult[0].AvailableStock;
                            unitCost = FinalResult[0].CostPrice;
                            Rate = FinalResult[0].RetailPrice;
                        }
                        //avQty = (int)FinalResult[FinalResult.Count-1].AvailableStock;
                        //unitCost = itemDetail.Item.UnitCostPrice;
                    }
                }
            }
            else
            {
                int aq = FinalResult.Sum(f => f.AvailableStock);
                if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                {
                    avQty = (int)Math.Floor((double)(aq / itemDetail.Item.ConversionUnit));
                }
                else
                {
                    avQty = aq;
                }
            }

            r.Cells["colAvailableQty"].Value = avQty;
            if (avQty < qty)
            {
                if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                {
                    MessageBox.Show("Availabe quantuity is less than enterd quantity. quantity will be set to available quantity automatically", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    r.Cells["colQuantity"].Value = avQty;
                }
                else
                {
                    r.Cells["colQuantity"].Value = qty;
                    r.DefaultCellStyle.SelectionBackColor = Color.Red;
                    r.DefaultCellStyle.BackColor = Color.Red;
                }
            }
            else
            {
                r.Cells["colQuantity"].Value = qty;
            }
            r.Cells["colRate"].Value = Rate;

            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;

            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");

            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            }
            else
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            }

            //Double ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value) * itemDetail.Item.ConversionUnit;
            //r.Cells["colCostPrice"].Value = unitCost * itemDetail.Item.ConversionUnit;
            Double DiscountedPrice = 0;
            DataTable dt1 = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit  from StockItems where Item_ItemID=" + ItemId + " and  Batch_BatchId=" + r.Cells["colBatchId"].Value);
            DataTable dt11 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
            if (dt1.Rows.Count > 0)
            {
                Double Discount = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["Discount"].ToString());
                int stockunit = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["Unit"].ToString());
                //if (stockunit == 0)
                //{
                int stockqty = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["Quantity"].ToString());
                //}
                //stockqty = stockqty / Convert.ToInt32(dt11.Rows[0][0].ToString());
                Double UnitCost = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["UnitCost"].ToString());
                Double subTotal = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["TotalCost"].ToString());
                Double RetailPrice = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["RetailPrice"].ToString());
                int DiscountType = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["PercDiscType"].ToString());
                if (DiscountType == 0)
                {
                    if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                    {
                        Discount = ((Discount / 100) * (unitCost) * Convert.ToInt32(dt11.Rows[0][0].ToString()));
                    }
                    else
                    {
                        Discount = ((Discount / 100) * (unitCost));

                    }
                    DiscountedPrice = Math.Round(((unitCost * Convert.ToInt32(dt11.Rows[0][0].ToString())) - Discount), 2);
                }
                else
                {
                    DiscountedPrice = Math.Round(((unitCost - Discount / stockqty) * Convert.ToInt32(dt11.Rows[0][0].ToString())), 2);
                }

                if (r.Cells["colUnit"].Value.ToString() == "Pack")
                {
                    UnitCost = UnitCost * Convert.ToInt32(dt11.Rows[0][0].ToString());

                }
                if (r.Cells["colUnit"].Value.ToString() == "Units")
                {
                    if (DiscountType != 0)
                    {
                        DiscountedPrice = Math.Round((UnitCost - Discount / stockqty), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round((UnitCost - Discount), 2);

                    }
                }
            }
            r.Cells["ProfitMargin"].Value = Profit(Rate, DiscountedPrice);
            //Double ds = 0;
            r.Cells["DiscountedPrice"].Value = DiscountedPrice;
            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            {
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                r.Cells["colCostPrice"].Value = unitCost * itemDetail.Item.ConversionUnit;
            }
            else
            {
                //if ((r.Cells["colUnit"].Value).ToString() == "1")
                //{
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value) / itemDetail.Item.ConversionUnit;
                //}
                //else
                //{
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                //}
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                r.Cells["colCostPrice"].Value = unitCost;
            }

            //if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            //{
            //    r.Cells["DiscountedPrice"].Value = ds * itemDetail.Item.ConversionUnit;
            //}
            //else
            //{
            //r.Cells["DiscountedPrice"].Value = ds;
            //}
            //r.Cells["colCostPrice"].Value = unitCost;


            FlatDiscount fd = loadFlatDiscount(ItemId, r);
            if (fd != null)
            {
                r.Cells["colDiscount"].Value = fd.Discount;
                r.Cells["colDiscountType"].Value = fd.DiscountType == 0 ? "%" : "Value";
            }

            //DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            //string unit = itemDetail.Item.Unit;

            //dgCmbUnit.Items.Add(unit);
            //dgCmbUnit.Items.Add("Units");

            //if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            //{
            //    r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            //}
            //else
            //{
            //    r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            //}

            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            CalculateItemTotals(r);
            CalculateTotals();
            int BatchId = SharedFunctions.StringToInt(r.Cells["colBatchId"].Value);
            if (BatchId > 0)
            {
                if (CheckExpItem(ItemId, BatchId))
                {
                    r.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                    r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                }
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void GetSelectedItemDataOnline(int ItemId, DataGridViewRow r)
        {
            int qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
            int avQty = 0;
            double Rate = 0;
            double unitCost = 0;

            //setQuantity Initial
            if (IsInvoiceEdit || IsInvoicecopy)
            {
                IsEditAllow = true;
                if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) == 0)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
                else
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    double reQty = 0;
                    double setQty = 0;
                    double qty1 = 0;
                    reQty = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                    qty1 = Convert.ToDouble(r.Cells["colQuantity"].Value);
                    setQty = qty1 - reQty;
                    r.Cells["colQuantity"].Value = setQty;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
            }

            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            LoadSerial();
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();
            using (unitOfWork = new UnitOfWork())
            {
                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStoreWise(ItemId, SharedFunctions.StringToInt(cmbStores.SelectedValue), SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                else
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                AddedItemsData.Add(itemDetail); // this list will be used  at time of saving invoice to reduce db cost.
                BatchStockList = itemDetail.BatchStockList.ToList();
            }
            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            r.Cells["colPCTCode"].Value = itemDetail.Item.PCTCode;
            //r.Cells["colRackNo"].Value = itemDetail.Item.Rack == null ? "" : itemDetail.Item.Rack.Name;
            //if (!isLoadingform)
            //{
            //    r.Cells["colRate"].Value = itemDetail.RetailPrice * itemDetail.Item.ConversionUnit;
            //}
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();

            foreach (BatchStockVM i in BatchStockList)
            {
                SharedFunctions.CalculateInvoiceItemQty(i);

                //i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                //{
                //    i.AvailableStock += i.HoldStock;
                //}
                //double avlStock = i.AvailableStock;
                if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                {
                    i.AvailableStock = i.AvailableStock / itemDetail.Item.ConversionUnit;
                }

                i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";

                if (!SharedVariables.AdminPharmacySetting.AllowNegCons || this.IsOnlineIntegrate)
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



            //FinalResult = FinalResult.OrderBy(x => x.BatchId).ToList();
            //r.Cells["colBatch"].Value = FinalResult[FinalResult.Count -1].BatchName;
            //r.Cells["colBatchId"].Value = FinalResult[FinalResult.Count - 1].BatchId;
            //WCH
            if (SharedVariables.AdminPharmacySetting.AllowNegCons)
            {
                if (BatchStockList != null && BatchStockList.Count() > 0 && FinalResult.Count() <= 0)
                {
                    FinalResult.Add(BatchStockList.OrderByDescending(x => x.BatchId).FirstOrDefault());
                }
            }
            DataTable dt;
            if (FinalResult[0].BatchName.Contains("Opening Stock"))
            {
                dt = get("Select Batch_BatchId , Stock_StockId  from StockItems where Item_ItemID=" + SelectedItemId + " and (Batch_BatchId=" + r.Cells["colBatchId"].Value + " or Stock_StockId = Batch_BatchId)");
            }
            else
            {
                dt = get("Select Batch_BatchId from StockItems where Item_ItemID=" + SelectedItemId + " and Batch_BatchId=" + r.Cells["colBatchId"].Value);
            }

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
                        r.Cells["colBatchId"].Value = dt.Rows[0][0].ToString();
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(dt.Rows[0][0].ToString()))
                            {
                                r.Cells["colBatch"].Value = i.BatchName;
                            }
                        }
                        string contanBrack = r.Cells["colBatch"].Value.ToString();
                        if (!contanBrack.Contains("["))
                        {
                            r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                            r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
                        }
                    }
                    else
                    {
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                r.Cells["colBatch"].Value = i.BatchName;
                            }
                        }
                        string contanBrack = r.Cells["colBatch"].Value.ToString();
                        if (!contanBrack.Contains("["))
                        {
                            r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                            r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
                        }
                    }

                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                    {
                        //avQty = (int)Math.Floor((double)(FinalResult[0].AvailableStock / itemDetail.Item.ConversionUnit));
                        //avQty = (int)FinalResult[FinalResult.Count-1].AvailableStock;
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                avQty = i.AvailableStock;
                                unitCost = i.CostPrice;
                                Rate = i.RetailPrice * itemDetail.Item.ConversionUnit;
                            }
                        }
                        //if (avQty == 0)
                        //{
                        //    avQty = (int)FinalResult[0].AvailableStock;
                        //    unitCost = FinalResult[0].CostPrice;
                        //    Rate = FinalResult[0].RetailPrice * itemDetail.Item.ConversionUnit;
                        //}
                        //Rate = itemDetail.Item.RetailPrice * itemDetail.Item.ConversionUnit;
                        //unitCost = FinalResult[0].CostPrice * itemDetail.Item.ConversionUnit;
                        //unitCost = itemDetail.Item.UnitCostPrice;

                    }
                    else
                    {
                        //Rate = itemDetail.Item.RetailPrice;
                        foreach (var i in FinalResult.ToList())
                        {
                            if (i.BatchId == Convert.ToInt32(r.Cells["colBatchId"].Value.ToString()))
                            {
                                avQty = i.AvailableStock;
                                unitCost = i.CostPrice;
                                Rate = i.RetailPrice;
                            }
                        }
                        if (avQty == 0)
                        {
                            avQty = FinalResult[0].AvailableStock;
                            unitCost = FinalResult[0].CostPrice;
                            Rate = FinalResult[0].RetailPrice;
                        }
                        //avQty = (int)FinalResult[FinalResult.Count-1].AvailableStock;
                        //unitCost = itemDetail.Item.UnitCostPrice;
                    }
                }
            }
            else
            {
                int aq = FinalResult.Sum(f => f.AvailableStock);
                if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                {
                    avQty = (int)Math.Floor((double)(aq / itemDetail.Item.ConversionUnit));
                }
                else
                {
                    avQty = aq;
                }
            }

            r.Cells["colAvailableQty"].Value = avQty;
            if (avQty < qty)
            {
                if (!SharedVariables.AdminPharmacySetting.AllowNegCons || this.IsOnlineIntegrate)
                {
                    MessageBox.Show("Availabe quantuity is less than enterd quantity. quantity will be set to available quantity automatically", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    r.Cells["colQuantity"].Value = avQty;
                }
                else
                {
                    r.Cells["colQuantity"].Value = qty;
                    r.DefaultCellStyle.SelectionBackColor = Color.Red;
                    r.DefaultCellStyle.BackColor = Color.Red;
                }
            }
            else
            {
                r.Cells["colQuantity"].Value = qty;
            }
                            r.Cells["colRate"].Value = Rate;
           

            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;

            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");
            if (!IsOnlineIntegrate)
            {
                if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                {
                    r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
                }
                else
                {
                    r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
                }
            }
            else
            {
                foreach (var i in OnlineIntegrateList.OnlineItem) 
                {
                    if (i.ItemName.ToString() == r.Cells["colItemName"].Value.ToString())
                    {
                        r.Cells["colUnit"].Value = i.Unit.ToString();
                    }
                }
            }
            if (IsOnlineIntegrate)
            {
                if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                {
                    r.Cells["colRate"].Value = Rate* itemDetail.Item.ConversionUnit;
                }
                else
                {
                    r.Cells["colRate"].Value = Rate;
                }
            }
            else
            {
                r.Cells["colRate"].Value = Rate;

            }
            //Double ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value) * itemDetail.Item.ConversionUnit;
            //r.Cells["colCostPrice"].Value = unitCost * itemDetail.Item.ConversionUnit;
            Double DiscountedPrice = 0;
            DataTable dt1 = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit  from StockItems where Item_ItemID=" + ItemId + " and  Batch_BatchId=" + r.Cells["colBatchId"].Value);
            DataTable dt11 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
            if (dt1.Rows.Count > 0)
            {
                Double Discount = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["Discount"].ToString());
                int stockunit = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["Unit"].ToString());
                //if (stockunit == 0)
                //{
                int stockqty = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["Quantity"].ToString());
                //}
                //stockqty = stockqty / Convert.ToInt32(dt11.Rows[0][0].ToString());
                Double UnitCost = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["UnitCost"].ToString());
                Double subTotal = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["TotalCost"].ToString());
                Double RetailPrice = Convert.ToDouble(dt1.Rows[dt1.Rows.Count - 1]["RetailPrice"].ToString());
                int DiscountType = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["PercDiscType"].ToString());
                if (DiscountType == 0)
                {
                    if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                    {
                        Discount = ((Discount / 100) * (unitCost) * Convert.ToInt32(dt11.Rows[0][0].ToString()));
                    }
                    else
                    {
                        Discount = ((Discount / 100) * (unitCost));

                    }
                    if (IsOnlineIntegrate) 
                    {
                        if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                        {
                            DiscountedPrice = Math.Round(((unitCost * Convert.ToInt32(dt11.Rows[0][0].ToString())) - Discount), 2);
                        }
                        else
                        {
                            DiscountedPrice = Math.Round((unitCost - Discount), 2);

                        }

                    }
                    else
                    {
                        DiscountedPrice = Math.Round(((unitCost - Discount / stockqty) * Convert.ToInt32(dt11.Rows[0][0].ToString())), 2);

                    }
                }
                else
                {
                    DiscountedPrice = Math.Round(((unitCost - Discount / stockqty) * Convert.ToInt32(dt11.Rows[0][0].ToString())), 2);
                }

                if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                {
                    UnitCost = UnitCost * Convert.ToInt32(dt11.Rows[0][0].ToString());

                }
                if (r.Cells["colUnit"].Value.ToString().ToLower() == "units")
                {
                    if (DiscountType != 0)
                    {
                        DiscountedPrice = Math.Round((UnitCost - Discount / stockqty), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round((UnitCost - Discount), 2);

                    }
                }
            }
            if (IsOnlineIntegrate)
            {
                if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                {
                    r.Cells["ProfitMargin"].Value = Profit(Rate* itemDetail.Item.ConversionUnit, DiscountedPrice);
                }
                else
                {
                    r.Cells["ProfitMargin"].Value = Profit(Rate, DiscountedPrice);

                }
            }
            else
            {
                r.Cells["ProfitMargin"].Value = Profit(Rate, DiscountedPrice);

            }
            //Double ds = 0;
            r.Cells["DiscountedPrice"].Value = DiscountedPrice;
            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            {
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                r.Cells["colCostPrice"].Value = unitCost * itemDetail.Item.ConversionUnit;
            }
            else
            {
                //if ((r.Cells["colUnit"].Value).ToString() == "1")
                //{
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value) / itemDetail.Item.ConversionUnit;
                //}
                //else
                //{
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                //}
                //ds = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                if (IsOnlineIntegrate)
                {

                    if (r.Cells["colUnit"].Value.ToString().ToLower() == "pack")
                    {
                        r.Cells["colCostPrice"].Value = unitCost * itemDetail.Item.ConversionUnit;
                    }
                    else
                    {
                        r.Cells["colCostPrice"].Value = unitCost;
                    }
                }
                else
                {
                    r.Cells["colCostPrice"].Value = unitCost;

                }
            }

            //if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            //{
            //    r.Cells["DiscountedPrice"].Value = ds * itemDetail.Item.ConversionUnit;
            //}
            //else
            //{
            //r.Cells["DiscountedPrice"].Value = ds;
            //}
            //r.Cells["colCostPrice"].Value = unitCost;


            FlatDiscount fd = loadFlatDiscount(ItemId, r);
            if (fd != null)
            {
                r.Cells["colDiscount"].Value = fd.Discount;
                r.Cells["colDiscountType"].Value = fd.DiscountType == 0 ? "%" : "Value";
            }

            //DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            //string unit = itemDetail.Item.Unit;

            //dgCmbUnit.Items.Add(unit);
            //dgCmbUnit.Items.Add("Units");

            //if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
            //{
            //    r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            //}
            //else
            //{
            //    r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            //}

            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            CalculateItemTotals(r);
            CalculateTotals();
            int BatchId = SharedFunctions.StringToInt(r.Cells["colBatchId"].Value);
            if (BatchId > 0)
            {
                if (CheckExpItem(ItemId, BatchId))
                {
                    r.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                    r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                }
            }
            
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void GetSelectedItemData_1(int ItemId, DataGridViewRow r)
        {
            int avQty = 0, convUnit = 1;
            double rate = 0, cost = 0;

            //setQuantity Initial
            if (IsInvoiceEdit || IsInvoicecopy)
            {
                IsEditAllow = true;
                if (Convert.ToInt32(r.Cells["colReturnQuantity"].Value) == 0)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
                else
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    double reQty, qty, setQty = 0;
                    reQty = Convert.ToDouble(r.Cells["colReturnQuantity"].Value);
                    qty = Convert.ToDouble(r.Cells["colQuantity"].Value);
                    setQty = qty - reQty;
                    r.Cells["colQuantity"].Value = setQty;
                    r.Cells["colQuantityFirst"].Value = r.Cells["colQuantity"].Value;
                }
            }
            convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            string sellUnit = r.Cells["colUnit"].Value.ToString();
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            LoadSerial();
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();

            using (unitOfWork = new UnitOfWork())
            {
                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStoreWise(ItemId, SharedFunctions.StringToInt(cmbStores.SelectedValue), SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                else
                {
                    itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                }
                AddedItemsData.Add(itemDetail); // this list will be used  at time of saving invoice to reduce db cost.
                BatchStockList = itemDetail.BatchStockList.ToList();
            }

            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            DataTable dtt = get("select SaleTax , SaleTaxType from InvoiceItems where Invoice_InvoiceId=" + InvoiceEditId + " and Item_ItemId=" + itemDetail.Item.ItemID);
            if (dtt.Rows.Count > 0)
            {
                double saletax = Convert.ToDouble(dtt.Rows[0][0].ToString());
                int saletaxtype = Convert.ToInt32(dtt.Rows[0][1].ToString());
                r.Cells["colSaleTax"].Value = saletax;
                r.Cells["colSaleTaxType"].Value = saletaxtype == 0 ? "%" : "Value";
            }


            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            r.Cells["colPCTCode"].Value = itemDetail.Item.PCTCode;
            //r.Cells["colRackNo"].Value = itemDetail.Item.Rack == null ? "" : itemDetail.Item.Rack.Name;
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();

            if (this.HoldingInvoiceId > 0)
            {
                foreach (BatchStockVM i in BatchStockList)
                {
                    SharedFunctions.CalculateInvoiceItemQty(i);
                    //i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                    //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                    //{
                    //    i.AvailableStock += i.HoldStock;
                    //}
                    //i.AvailableStock = i.Unit == 0 ? i.AvailableStock * itemDetail.Item.ConversionUnit : i.AvailableStock;
                    i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
                    if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                    {
                        if (i.AvailableStock > 0) // for holding invoices its mandatory to show only those batches which have some stock;
                        {
                            FinalResult.Add(i);
                        }
                    }
                    else
                    {
                        FinalResult.Add(i);
                    }
                }
            }
            else // this will be called to populate in case user opens an old invoice for view and update, so here we will load all batches even if there is not stock in them, because its for diaplay purpose and no more consumptions will be done in edit made for old items.
            {
                foreach (BatchStockVM i in BatchStockList)
                {
                    // no need to check available stock in case of form loading, as it may have become zero now.
                    SharedFunctions.CalculateInvoiceItemQty_1(i);
                    //i.AvailableStock = i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                    //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                    //{
                    //    i.AvailableStock += i.HoldStock;
                    //}
                    //i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
                    //FinalResult.Add(i);
                    //double avlStock = i.AvailableStock;
                    if (Convert.ToString(r.Cells["colUnit"].Value).ToLower() == "pack")
                    {
                        i.AvailableStock = i.AvailableStock / itemDetail.Item.ConversionUnit;
                    }

                    i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
                    //if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                    //{
                    //  if (avlStock > 0)
                    //{
                    //FinalResult.Add(i);
                    //}
                    //}
                    //else
                    //{
                    FinalResult.Add(i);
                    //}
                }
            }


            if (!SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colBatch"];
                dgCmbBatch.DataSource = FinalResult;
                dgCmbBatch.ValueMember = "BatchId";
                dgCmbBatch.DisplayMember = "BatchName";

                if (FinalResult.Count > 0)
                {
                    int selectedBatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
                    if (this.HoldingInvoiceId > 0)
                    {
                        var batch = FinalResult.Where(b => b.BatchId == selectedBatchId).FirstOrDefault();
                        if (batch != null)
                        {
                            r.Cells["colBatch"].Value = batch.BatchName;
                            r.Cells["colBatchId"].Value = batch.BatchId;
                            if (sellUnit != "Units")
                            {
                                avQty = batch.AvailableStock / convUnit;
                                rate = batch.RetailPrice * convUnit;
                                cost = batch.CostPrice * convUnit;
                            }
                            else
                            {
                                avQty = batch.AvailableStock;
                                rate = batch.RetailPrice;
                                cost = batch.CostPrice;
                            }
                        }
                        else
                        {
                            r.Cells["colBatch"].Value = FinalResult[0].BatchName;
                            r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
                            if (sellUnit != "Units")
                            {
                                avQty = FinalResult[0].AvailableStock / convUnit;
                                rate = FinalResult[0].RetailPrice * convUnit;
                                cost = itemDetail.Item.UnitCostPrice * convUnit;
                            }
                            else
                            {
                                avQty = FinalResult[0].AvailableStock;
                                rate = FinalResult[0].RetailPrice;
                                cost = itemDetail.Item.UnitCostPrice;
                            }
                        }
                    }
                    else
                    {

                        var batch = FinalResult.Where(b => b.BatchId == selectedBatchId).FirstOrDefault();
                        if (batch != null)
                        {
                            r.Cells["colBatch"].Value = batch.BatchName;
                            r.Cells["colBatchId"].Value = batch.BatchId;
                            if (sellUnit != "Units")
                            {
                                avQty = batch.AvailableStock;
                                //WCH 
                                //---------- Comment this code It take new stock batch value that is wrong --------//
                                //rate = batch.RetailPrice * convUnit;
                                //cost = batch.CostPrice * convUnit;
                                rate = SharedFunctions.StringToDouble(r.Cells["colRate"].Value);  //* convUnit;
                                cost = SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value) * convUnit;// *convUnit;                                
                                //cost = itemDetail.Item.UnitCostPrice * convUnit;// *convUnit;
                            }
                            else
                            {
                                avQty = batch.AvailableStock;
                                rate = SharedFunctions.StringToDouble(r.Cells["colRate"].Value);
                                cost = SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value);
                                //cost = itemDetail.Item.UnitCostPrice;
                                //rate = batch.RetailPrice;
                                //cost = batch.CostPrice;
                            }
                        }
                        else
                        {
                            r.Cells["colBatch"].Value = r.Cells["colBatch"].Value;
                            r.Cells["colBatchId"].Value = r.Cells["colBatchId"].Value;
                            if (sellUnit != "Units")
                            {
                                avQty = FinalResult[0].AvailableStock / convUnit;
                                rate = FinalResult[0].RetailPrice * convUnit;
                                cost = SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value) * convUnit;
                                //cost = itemDetail.Item.UnitCostPrice * convUnit;
                            }
                            else
                            {
                                avQty = FinalResult[0].AvailableStock;
                                rate = FinalResult[0].RetailPrice;
                                cost = SharedFunctions.StringToDouble(r.Cells["colCostPrice"].Value);
                                //cost = itemDetail.Item.UnitCostPrice;
                            }
                        }
                    }
                }
            }
            else
            {
                int aq = FinalResult.Sum(f => f.AvailableStock);
                avQty = (int)Math.Floor((double)(aq / itemDetail.Item.ConversionUnit));
            }
            r.Cells["colRate"].Value = rate;
            int units = 0;
            DataTable csprice = get("Select UnitCost, RetailPrice, Discount, PercDiscType, Unit , Quantity from StockItems where Item_ItemId=" + r.Cells["colItemId"].Value + " and Batch_BatchId=" + r.Cells["colBatchId"].Value);
            units = Convert.ToInt32(csprice.Rows[csprice.Rows.Count - 1]["Unit"].ToString());
            Double Discount = Convert.ToDouble(csprice.Rows[csprice.Rows.Count - 1]["Discount"].ToString());
            int DiscountType = Convert.ToInt32(csprice.Rows[csprice.Rows.Count - 1]["PercDiscType"].ToString());
            int stockqty = Convert.ToInt32(csprice.Rows[csprice.Rows.Count - 1]["Quantity"].ToString());
            //stockqty = stockqty / Convert.ToInt32(r.Cells["colConvUnit"].Value);
            Double DiscountedPrice = 0;
            if (IsInvoicecopy || this.HoldingInvoiceId > 0)
            {
                r.Cells["colCostPrice"].Value = Convert.ToString(r.Cells["colUnit"].Value).ToLower() == "pack" ? Convert.ToDouble(csprice.Rows[csprice.Rows.Count - 1][0].ToString()) * Convert.ToInt32(r.Cells["colConvUnit"].Value) : Convert.ToDouble(csprice.Rows[csprice.Rows.Count - 1][0].ToString());
                double unitCost = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                r.Cells["colRate"].Value = Convert.ToString(r.Cells["colUnit"].Value).ToLower() == "pack" ? Convert.ToDouble(csprice.Rows[csprice.Rows.Count - 1][1].ToString()) * Convert.ToInt32(r.Cells["colConvUnit"].Value) : Convert.ToDouble(csprice.Rows[csprice.Rows.Count - 1][1].ToString());
                if (DiscountType == 0)
                {
                    Discount = ((Discount / 100) * unitCost);
                    DiscountedPrice = Math.Round(((unitCost) - Discount), 2);
                }
                else
                {
                    if (Convert.ToString(r.Cells["colUnit"].Value).ToLower() == "pack")
                    {
                        DiscountedPrice = Math.Round((unitCost - (Discount / stockqty) * Convert.ToInt32(r.Cells["colConvUnit"].Value)), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round((unitCost - (Discount / stockqty)), 2);
                    }
                }
                r.Cells["DiscountedPrice"].Value = DiscountedPrice.ToString();
            }
            if (IsInvoiceRefund || IsInvoiceEdit)
            {
                double costPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                if (Convert.ToInt32(r.Cells["DiscountedPrice"].Value) == 0)
                {
                    r.Cells["DiscountedPrice"].Value = costPrice;
                }

            }
            //r.Cells["colCostPrice"].Value = cost;
            r.Cells["colAvailableQty"].Value = avQty;
            r.Cells["ProfitMargin"].Value = Profit(rate, DiscountedPrice);
            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;
            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");
            r.Cells["colUnit"].Value = r.Cells["colUnit"].Value;
            CalculateItemTotals(r);
            CalculateTotals();
            grdItems.CellValueChanged += grdItems_CellValueChanged;

        }
        private bool isValidQuantityEntered()
        {
            int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQuantity.Value > AvailableStock)
            {
                //ToolTip tt = new ToolTip();
                //numQuantity.Value = 1;
                //ErrMessage.Text = "Alert : Sold Quantity Can't be Greater Than Available Quantity.";
                //ErrMessage.Visible = true;
                //tt.Show("Please Enter Valid Quantity", numQuantity, 800);
                //numQuantity.Select(0, 1);
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
            }
            return true;
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();
            ////IsDiscountChanged = false;
            //if (cmbSelectBatch.SelectedIndex >= 0)
            //{
            //    //isValidQuantityEntered();
            //    CalculateTotals();
            //}
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            try
            {
                this.isNewItemAdded = false;
                //RegisterEvents(false);
                //cmbSelectItems.SelectedIndex = -1;
                //cmbSelectBatch.SelectedIndex = -1;
                //cmbDiscountType.SelectedIndex = 1;
                //cmbSelectItems.Enabled = true;
                //uC_SearchItems1.Enabled = true;
                //numQuantity.Value = 1;
                //numRate.Value = 0;
                //numDiscount.Value = 0;
                //this.btnAddItem.Text = "Add Item";
                //if (EditingRowIndex > -1)
                //{
                //    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                //}
                //EditingRowIndex = -1;
                //setNumericUD_toEmpty();
                //txtAmount.Text = "";
                //txtNetAmount.Text = "";
                //txtAvlQty.Text = "";
                //if (grdItems.Rows.Count == 0)
                //{
                //    numTotalDiscount.Enabled = true;
                //    numModifiedDiscount.Value = 0;
                //    numTotalDiscount.Value = 0;
                //}
                this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                this.uC_SearchItems1.SetText = "";
                this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                this.SelectedItemId = 0;

                cmbTemplates.SelectedIndex = 0;
                //cmbSelectBatch.DataSource = null;
                //cmbUnit.SelectedIndex = -1;
                //cmbUnit.Items.Clear();
                //numConvUnit.Value = 0;

                // this line must be called at end of clear method.
                this.uC_SearchItems1.Focus();
                //RegisterEvents(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                //Total Discount Enable
                discountChange = false;
                numTotalDiscount.Controls[0].Visible = true;
                grdItems.Columns["colDiscount"].ReadOnly = false;

                ClearAll(false);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Clearing Screen, Please Try Again OR ReOpen This Screen.", ex.Message, "Error");
            }
        }

        public bool checkTabforMultipleItemBatch()
        {
            int newVal = 1;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                for (var i = newVal; i < grdItems.Rows.Count; i++)
                {
                    if (Convert.ToString(grdItems.Rows[i].Cells["colItemId"].Value) == Convert.ToString(r.Cells["colItemId"].Value)
                        && Convert.ToString(grdItems.Rows[i].Cells["colBatchId"].Value) == Convert.ToString(r.Cells["colBatchId"].Value))
                    {
                        MessageBox.Show("Please select different batch of same item added", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                newVal++;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveInvoice(true, false, this.HoldingInvoiceId);
            servicecheck = 0;
            notifyIcon1.Text = "Save";
            notifyIcon1.ShowBalloonTip(3, "New Invoice", "Saved", ToolTipIcon.Info);
        }

        private User loginUser()
        {
            frmInvoiceLogin login = new frmInvoiceLogin();
            login.BringToFront();
            login.StartPosition = FormStartPosition.CenterParent;
            login.ShowDialog();
            return login.VerfiedUser;
        }
        private long SaveInvoice(bool ShowSuccessDialog, bool isHoldInvoice, long HoldingInvoiceId)
        {
            int userId = 0;
            //if (SharedVariables.AdminInvoiceSetting.IsAskLoginOnInvSave && !isHoldInvoice)
            //{
            //    workingUser = loginUser();
            //    if (workingUser == null)
            //    {
            //        MessageBox.Show("Invoice save cancelled, You are not allowed to save invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return 0;
            //    }
            //    userId = workingUser.UserId;
            //}
            userId = SharedVariables.LoggedInUser.UserId;
            if (SharedVariables.AdminPharmacySetting.AskForPIN && !isHoldInvoice)
            {
                try
                {
                    int pin = Convert.ToInt32(Interaction.InputBox("Enter PIN of User to Save the Invoice"));
                    User user = new User();
                    using (unitOfWork = new UnitOfWork())
                    {
                        user = unitOfWork.UserRepository.IsInvalidPIN(pin);
                    }
                    if (user == null)
                    {
                        MessageBox.Show("Invoice save cancelled, You are not allowed to save invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return 0;
                    }
                    userId = user.UserId;
                }
                catch (Exception)
                {
                    MessageBox.Show("Invoice save cancelled, You are not allowed to save invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return 0;
                }
            }

            long InvoiceId = 0;
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Can't Save Empty Invoice, ", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            //if (EditingRowIndex > -1) // this commented, this functionality has been removed from invoice screen, but others
            //{
            //    MessageBox.Show("Can't Save Invoice While Editing an Item, ", "Editing Invoice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return 0;
            //}

            if (isHoldInvoice) // this will be only true when "Hold" button is pressed.
            {
                if (this.HoldingInvoiceId > 0)
                {
                    bool val = checkTabforMultipleItemBatch();
                    if (val == true)
                    {

                        InvoiceId = EditInvoice(this.HoldingInvoiceId, ShowSuccessDialog, false, userId);
                    }
                }
                else
                {
                    bool val = checkTabforMultipleItemBatch();
                    if (val == true)
                    {
                        InvoiceId = InsertInvoice(ShowSuccessDialog, isHoldInvoice, userId);
                    }
                }
                if (InvoiceId > 0)
                {
                    InvHoldPageNo = 0;
                }
            }
            else
            {
                if (HoldingInvoiceId > 0) // this will be true if user is editing a holding invoice
                {
                    InvoiceId = EditInvoice(HoldingInvoiceId, ShowSuccessDialog, true, userId);
                    if (InvoiceId > 0)
                    {
                        InvHoldPageNo = 0;
                        ShowHoldingInvoices(InvHoldPageNo);
                    }
                }
                else
                {
                    if (this.InvoiceEditId > 0)
                    {
                        if (IsInvoiceEdit)
                        {
                            bool val = checkTabforMultipleItemBatch();
                            if (val == true)
                            {
                                //getsums();
                                InvoiceId = EditInvoice((int)this.InvoiceEditId, ShowSuccessDialog, false, userId);
                            }
                        }
                        else if (IsInvoiceRefund)
                        {
                            if(IsOnlineIntegrate && IsOnlineCancel)
                            {
                                if(canceltextBox.Text == "")
                                {
                                    MessageBox.Show("Cancel Reason Must be added before Refund");
                                    return 0;
                                }
                            }
                            SaveSalesReturn(userId);
                            InvoiceId = RefundInvoice(ShowSuccessDialog, userId);

                        }
                    }
                    else
                    {
                        bool val = checkTabforMultipleItemBatch();
                        if (val == true)
                        {
                            InvoiceId = InsertInvoice(ShowSuccessDialog, isHoldInvoice, userId);
                        }
                    }
                }
            }
            if (InvoiceId > 0)
            {
                ClearAll(true);
            }
            return InvoiceId;
        }

        int st = 0;
        private long InsertInvoice(bool ShowSuccessMsg, bool isHoldInvoice, int UserId)
        {
            TransactionScope scope;
            double due = 0;
            double balance = 0;
            double advance = 0;
            if (this.IsOnlineIntegrate)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            //------------Patitent Required For Narcotic items -------------//
            List<long> ItemIds = new List<long>();
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                ItemIds.Add(Convert.ToInt32(r.Cells["colItemId"].Value));
                if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                {
                    if (string.IsNullOrEmpty(SharedFunctions.ObjectToString(r.Cells["colPCTCode"].Value)))
                    {
                        SharedFunctions.MessageBoxShow("PCT code is required on each item", MessageType.Info);
                        return 0;
                    }
                }
            }


            if (SharedVariables.AdminPharmacySetting.IsPatientSelectionMandatory && this.PatientId <= 0 && txtSearchByName.Text.Trim() == "")
            {
                SharedFunctions.MessageBoxShow("Patient selection is mandatory", MessageType.Info);
                return 0;
            }
            else
            {
                using (unitOfWork = new UnitOfWork())
                {
                    bool IsNarcoticItem = unitOfWork.ItemRspository.AnyNarcoticItem(ItemIds);
                    if (IsNarcoticItem && this.PatientId <= 0 && txtSearchByName.Text.Trim() == "")
                    {
                        SharedFunctions.MessageBoxShow("Patient is required for narcotic item", MessageType.Info);
                        return 0;
                    }
                }
            }
            //if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "" && string.IsNullOrEmpty(txtSearchByPhone.Text))
            //{
            //    SharedFunctions.MessageBoxShow("Patient phone number is required.", MessageType.Info);
            //    return 0;
            //}

            DateTime myDate;
            if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "" && !string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) && txtSearchByPhone.Text.Trim().Length < 10)
            {
                SharedFunctions.MessageBoxShow("Patient phone should be 10 characters.", MessageType.Info);
                return 0;
            }
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        double sumofPayments = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
                        List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                        Invoice objInvoice = new Invoice();
                        if (!CheckAndAssingStore(objInvoice))
                            return 0;
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        objInvoice.Services = Convert.ToDouble(txtServices.Value);
                        objInvoice.ServiceType = cmbPosServiceType.GetItemText(cmbPosServiceType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        due = double.Parse(lblDue.Text);
                        balance = double.Parse(lblBalance.Text);
                        advance = double.Parse(lblAdvance.Text);

                        objInvoice.IsOnlineIntegrate = this.IsOnlineIntegrate;

                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.IsNew = true;
                        objInvoice.IsActive = true;
                        objInvoice.InvoiceItems = new List<InvoiceItem>();
                        objInvoice.User = unitOfWork.UserRepository.GetById(UserId);
                        objInvoice.IsProcedureInvoice = false;
                        objInvoice.IsHoldInvoice = isHoldInvoice;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        StockConsumption ObjCons = new StockConsumption();
                        ObjCons.CreateDate = DateTime.Now;
                        myDate = ObjCons.CreateDate;
                        ObjCons.UpdateDate = DateTime.Now;
                        ObjCons.IsNew = true;
                        ObjCons.IsActive = true;

                        ObjCons.User = unitOfWork.UserRepository.GetById(UserId);
                        ObjCons.StockConsumptions = new List<StockConsumptionItem>();
                        int SrNo = 0;
                        ProfitStatementTransactions Transaction1 = new ProfitStatementTransactions();
                        double TCost = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            //int val = Convert.ToInt32(r.Cells["colBatch"].Value);
                            InvoiceItem objInvoiceItem = new InvoiceItem();
                            FillObject(objInvoiceItem, r);
                            ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                            if (SharedFunctions.IsDoubleFormat(objInvoiceItem.Rate, "Item Rate Price"))
                                return 0;
                            if (SharedFunctions.IsDoubleFormat(objInvoiceItem.PerUnitCostPrice, "Item Unit Cost Price"))
                                return 0;
                            objInvoiceItem.SerialNo = SrNo += 1;

                            objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                            if (objInvoiceItem.Quantity <= 0)
                            {
                                var indx = r.Cells["colQuantity"].RowIndex;
                                grdItems.CurrentCell = grdItems.Rows[Convert.ToInt32(indx)].Cells["colQuantity"];
                                grdItems.BeginEdit(true);
                                int index = grdItems.CurrentRow.Index;
                                grdItems.FirstDisplayedScrollingRowIndex = index;
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            var Rate = Convert.ToDouble(r.Cells["colRate"].Value);
                            var CostPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                            if (Rate <= CostPrice && !SharedVariables.AdminInvoiceSetting.AllowBelowCostSale)
                            {
                                var indx = r.Cells["colRate"].RowIndex;
                                grdItems.CurrentCell = grdItems.Rows[Convert.ToInt32(indx)].Cells["colRate"];
                                grdItems.BeginEdit(true);
                                int index = grdItems.CurrentRow.Index;
                                grdItems.FirstDisplayedScrollingRowIndex = index;
                                MessageBox.Show("Some items have Retail Price Less to the Cost Price , please enter Valid Retail Price for all items.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo) // this check is not in used
                            {
                                objInvoiceItem.Batch = null;
                                objInvoiceItem.IsOptionalBatch = true;
                                int avQty = Convert.ToInt32(r.Cells["colAvailableQty"].Value);
                                if (avQty == 0)
                                {
                                    MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                                else if (objInvoiceItem.Quantity > avQty)
                                {
                                    MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                            }
                            else
                            {
                                objInvoiceItem.IsOptionalBatch = false;
                                if (objInvoiceItem.BatchId <= 0)
                                {
                                    MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                                else
                                {
                                    objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                }

                                //SCMB
                                //if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.Item.ItemID) && !SharedVariables.AdminPharmacySetting.AllowNegCons && !SharedVariables.AdminPharmacySetting.IsConsumptionFromMultipleBatches)
                                //{
                                //    MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    return 0;
                                //}

                                if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.Item.ItemID) && !SharedVariables.AdminPharmacySetting.AllowNegCons)
                                {
                                    MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                            }
                            objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                            // botton lines code has been shifted in "InsertStockConsumption()" method
                            //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                            //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                            //following 2 lines of code were added due to default items issue handeling.
                            objInvoiceItem.Item.IsSyncable = true;
                            unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Modified;
                            objInvoiceItem.IsNew = true;
                            objInvoiceItem.IsActive = true;
                            objInvoiceItem.saletax = Convert.ToDouble(r.Cells["colSaleTax"].Value);
                            objInvoiceItem.saletaxType = r.Cells["colSaleTaxType"].Value == "%" ? 0 : 1;
                            objInvoiceItem.CreateDate = DateTime.Now;
                            objInvoiceItem.UpdateDate = DateTime.Now;
                            objInvoiceItem.Procedure = unitOfWork.ProcedureRepository.GetDefaultProcedures();
                            TCost += InsertStockConsumption(ObjCons, ref objInvoiceItem);
                            objInvoice.TotalUnitCost = TCost;
                            objInvoice.InvoiceItems.Add(objInvoiceItem);
                            if (r.Visible)
                            {

                                //if ((Math.Round(Convert.ToDouble(lblGrandTotal.Text), 0) <= Math.Round(sumofPayments, 0) || (Math.Round(Convert.ToDouble(lblGrandTotal.Text), 2) - Math.Round(sumofPayments, 0)) < 1) && !isHoldInvoice)
                                {
                                    #region Record Profit Statement Transaction
                                    if (TotalDiscountFound())
                                    {

                                        Transaction.CreatedAt = DateTime.Now;
                                        Transaction.TransactionDate = DateTime.Now;
                                        Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.Unit == 1 ? (objInvoiceItem.DiscountedPrice * objInvoiceItem.Quantity) : (objInvoiceItem.DiscountedPrice / objInvoiceItem.ConvUnit) * objInvoiceItem.Quantity;
                                        Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                        Transaction.TransactionDiscount = objInvoiceItem.DiscountType == 1 ? objInvoiceItem.Discount / 100 * objInvoiceItem.Amount : objInvoiceItem.Discount;
                                        Transaction.TransactionRevenue = objInvoiceItem.NetAmount;
                                        Transaction.User = SharedVariables.LoggedInUser.UserId;
                                        Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                        Transaction.IsDeleted = false;
                                        Transaction.InvoiceId = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                                        objTransaction.Add(Transaction);
                                    }
                                    else
                                    {
                                        int SumofQty = getSumofQty();
                                        if (cmbTotalDiscountType.Text == "%")
                                        {
                                            double Discount = Convert.ToDouble(numTotalDiscount.Value);
                                            double PerQtyDisc = (Discount / 100) * Convert.ToDouble(r.Cells["colRate"].Value);
                                            int qty = objInvoiceItem.Unit == 1 ? objInvoiceItem.Quantity : objInvoiceItem.Quantity / objInvoiceItem.ConvUnit;
                                            Transaction.CreatedAt = DateTime.Now;
                                            Transaction.TransactionDate = DateTime.Now;
                                            Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.Unit == 1 ? (objInvoiceItem.DiscountedPrice * objInvoiceItem.Quantity) : (objInvoiceItem.DiscountedPrice / objInvoiceItem.ConvUnit) * objInvoiceItem.Quantity;
                                            Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                            Transaction.TransactionDiscount = PerQtyDisc * qty;
                                            Transaction.TransactionRevenue = objInvoiceItem.NetAmount - (PerQtyDisc * qty);
                                            Transaction.User = SharedVariables.LoggedInUser.UserId;
                                            Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                            Transaction.IsDeleted = false;
                                            Transaction.InvoiceId = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                                            objTransaction.Add(Transaction);

                                        }
                                        else
                                        {
                                            double Discount = ConvertPercToValue(Convert.ToDouble(numTotalDiscount.Value), Convert.ToDouble(lblSubTotal.Text), cmbTotalDiscountType.SelectedIndex);
                                            double PerQtyDisc = Discount / SumofQty;
                                            int qty = objInvoiceItem.Unit == 1 ? objInvoiceItem.Quantity : objInvoiceItem.Quantity / objInvoiceItem.ConvUnit;
                                            Transaction.CreatedAt = DateTime.Now;
                                            Transaction.TransactionDate = DateTime.Now;
                                            Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.Unit == 1 ? (objInvoiceItem.DiscountedPrice * objInvoiceItem.Quantity) : (objInvoiceItem.DiscountedPrice / objInvoiceItem.ConvUnit) * objInvoiceItem.Quantity;
                                            Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                            Transaction.TransactionDiscount = PerQtyDisc * qty;
                                            Transaction.TransactionRevenue = objInvoiceItem.NetAmount - (PerQtyDisc * qty);
                                            Transaction.User = SharedVariables.LoggedInUser.UserId;
                                            Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                            Transaction.IsDeleted = false;
                                            Transaction.InvoiceId = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                                            objTransaction.Add(Transaction);

                                        }
                                    }
                                    #endregion


                                }
                            }


                        }
                        Tuple<double, double, double> Values = BalanceGrossProfit();
                        double sum = objTransaction.Sum(i => i.TransactionRevenue);

                        if (Values != null && Values.Item3 != sum)
                        {
                            ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                            Transaction.TransactionRevenue = (-1) * Values.Item1;
                            Transaction.InvoiceId = objTransaction.Select(i => i.InvoiceId).First();
                            Transaction.CreatedAt = DateTime.Now;
                            Transaction.UpdatedAt = DateTime.Now;
                            Transaction.User = SharedVariables.LoggedInUser.UserId;
                            Transaction.ItemId = -1;
                            Transaction.TransactionDate = DateTime.Now;
                            Transaction.IsDeleted = false;
                            objTransaction.Add(Transaction);
                        }
                        unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                        unitOfWork.Save();
                        if (!AddInvoicePayments(objInvoice))
                        {
                            return 0;
                        }
                        if (balance > 0)
                        {
                            objInvoice.InvoicePayments.Last().Payment -= balance;
                        }
                        if (this.IsOnlineIntegrate)
                        {
                            objInvoice.TotalPaid = this.TotalPaid - this.voucherCharges;
                        }
                        else
                        {
                            objInvoice.TotalPaid = this.TotalPaid;
                        }
                        objInvoice.TotalPaid = this.TotalPaid;

                        // conmmented on 2021-02-04
                        // dues are in positive.
                        // advance is in negatve.
                        //if (this.PatientId > 0)
                        //{
                        //    objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        //    //objInvoice.Patient.Dues += due > 0 ? due : advance;
                        //    if (due > 0)
                        //    {
                        //        objInvoice.Patient.Dues -= due;
                        //    }
                        //    else
                        //    {
                        //        objInvoice.Patient.Dues += advance;
                        //    }
                        //    if (objInvoice.Patient.IsSynced)
                        //    {
                        //        objInvoice.Patient.IsNew = false;
                        //        objInvoice.Patient.IsUpdate = true;
                        //        objInvoice.Patient.IsSynced = false;
                        //    }
                        //    unitOfWork.GetDbContext().Entry(objInvoice.Patient).State = System.Data.Entity.EntityState.Modified;
                        //}


                        if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "")
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
                            if (due > 0)
                            {
                                newP.Dues -= due;
                            }
                            else if (advance > 0)
                            {
                                newP.Dues += advance;
                            }

                            objInvoice.Patient = newP;
                            AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + newP.MRNo, AuditModule.Patient, AuditAction.created);
                        }
                        if (this.PatientId > 0)
                        {
                            double patientBal = unitOfWork.InvoiceRepository.GetInvoicesBalanceByPatientId(this.PatientId);
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(this.PatientId);
                            if (due > 0)
                            {
                                objInvoice.Patient.Dues = -patientBal - due;
                            }
                            else if (advance > 0)
                            {
                                objInvoice.Patient.Dues = -patientBal + advance;
                            }
                            if (objInvoice.Patient.IsSynced)
                            {
                                objInvoice.Patient.IsNew = false;
                                objInvoice.Patient.IsUpdate = true;
                                objInvoice.Patient.IsSynced = false;
                            }
                        }

                        if (due > 0)
                        {
                            objInvoice.Due = due;
                        }
                        else if (advance > 0)
                        {
                            objInvoice.Due = -advance;
                        }
                        objInvoice.StockConsumption = ObjCons;
                        if (this.IsOnlineIntegrate)
                        {
                            Response check = new Response();
                            obj.getCredential();
                            check = obj.hitPandaGo(this.OrderValue);
                            this.Cursor = Cursors.Default;
                            if (check.status == "Success")
                            {
                            }
                            else if (check.status == "Failure")
                            {
                                SharedFunctions.MessageBoxShow("Order does'nt place", MessageType.Warning);
                                return 0;
                            }
                            else if (check.status == "Expired")
                            {
                                SharedFunctions.MessageBoxShow("Order is Expired", MessageType.Warning);
                                return 0;
                            }
                            else if (check.status == "Cancelled")
                            {
                                int cancel = updateOrderStatus("update OnlineOrders set Cancel= " + 1 + " where OrderId= " + this.OrderId);
                                SharedFunctions.MessageBoxShow("Order is Cancelled by the Customer", MessageType.Warning);
                                this.Close();
                                scope.Complete();
                                return 0;
                            }
                        }
                        //unitOfWork.StockConsumptionRepository.Insert(ObjCons);
                        unitOfWork.InvoiceRepository.Insert(objInvoice);
                        objInvoice.InvoiceRefNo = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                        AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + objInvoice.InvoiceRefNo, AuditModule.Invoice, AuditAction.created);
                        unitOfWork.Save();
                        //SOT
                        if (!isHoldInvoice)
                        {
                            if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "")
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                            if (this.PatientId > 0)
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Update, objInvoice.Patient.PatientId);
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Create, objInvoice.InvoiceId);
                        }
                        // for sake of saving invoiceid to stockConsumption :  its not relation but required for syncing purpose with web
                        StockConsumption con = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(objInvoice.StockConsumption.StockConsumptionId);
                        con.InvoiceId = objInvoice.InvoiceId;
                        List<StockConsumptionItem> StockConsumpList = new List<StockConsumptionItem>();
                        foreach (StockConsumptionItem sci in con.StockConsumptions)
                        {
                            sci.InvoiceId = objInvoice.InvoiceId;
                            foreach (DataGridViewRow rr in grdItems.Rows)
                            {
                                if (Convert.ToInt32(rr.Cells["colItemId"].Value) == sci.Item.ItemID && Convert.ToInt32(rr.Cells["colBatchId"].Value) == sci.Batch.BatchId)
                                {
                                    sci.Unit = rr.Cells["colUnit"].Value.ToString() == "Pack" || rr.Cells["colUnit"].Value.ToString() == "pack" ? 0 : 1;
                                }
                            }
                            StockConsumpList.Add(sci);
                            unitOfWork.GetDbContext().Entry<StockConsumptionItem>(sci).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.StockConsumptionRepository.Update(con);
                        unitOfWork.Save();

                        //SOT
                        if (!isHoldInvoice)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumpList, ModuleName.BulkStockConsumptionItems, ModuleAction.Create, 0);
                        }

                        if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                        {
                            FBRResponseVM fbrModel = SendInvoiceToFBR(objInvoice);
                            if (!string.IsNullOrEmpty(fbrModel.Error))
                            {
                                SharedFunctions.MessageBoxShow(fbrModel.Error, MessageType.Error);
                                return 0;
                            }
                            objInvoice.FBRCode = fbrModel.Code;
                            objInvoice.FBRInvoiceNo = fbrModel.InvoiceNumber;
                        }
                        unitOfWork.Save();
                        if (this.IsOnlineIntegrate)
                        {
                            int j = updateOrderStatus("Update OnlineOrders set Invoice_InvoiceId=" + objInvoice.InvoiceId + ",Pack =" + "1" + " where OrderId =" + this.OrderId);

                            //add voucher in the table and get voucherId
                            if (this.voucherCharges != 0 || this.WalletAmount != 0)
                            {
                                InsertVoucherOrder(objInvoice);
                            }
                        }

                        scope.Complete();
                        //checkStock();
                        if (ShowSuccessMsg)
                        {
                            //Total Discount Enable
                            discountChange = false;
                            numTotalDiscount.Controls[0].Visible = true;
                            grdItems.Columns["colDiscount"].ReadOnly = false;
                            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable && objInvoice.Patient != null)
                            {
                                if (objInvoice.Patient.Phone.ToString() != "")
                                {
                                    Task.Delay(new TimeSpan(0, 0, 10)).ContinueWith(o =>
                                    {
                                        obj.getCredential();
                                    });
                                    Task.Delay(new TimeSpan(0, 0, 30)).ContinueWith(o =>
                                    {
                                        obj.PostDataInvoice(objInvoice.Patient.Phone.ToString(), objInvoice.Patient.Name.ToString());
                                    });
                                }
                            }

                            if (this.IsOnlineIntegrate)
                            {
                                MessageBox.Show("Invoice Saved Successfully and Order placed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        return objInvoice.InvoiceId;

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
                    scope.Dispose();
                    return 0;
                }
            }


        }
        public void InsertVoucherOrder(Invoice objInvoice)
        {
            try
            {
                TransactionScope scope;
                VoucherVM voucher = new VoucherVM();
                OrderVoucher voucher1 = new OrderVoucher();

                DataTable voucherName = getVoucher("Select * from OrderVouchers where VoucherNumber=@VoucherNumber", objInvoice.VoucherName == null ? "Wallet Credit" : objInvoice.VoucherName);
                if (voucherName.Rows.Count > 0)
                {
                    //update voucher
                    int updateVouch = updateOrderStatus("Update OrderVouchers set VoucherValue=VoucherValue +" + this.voucherCharges + ",WalletCredit=WalletCredit +" + this.WalletAmount + ",GrandTotal=GrandTotal +" + objInvoice.GrandTotal + ",SubTotal=SubTotal +" + objInvoice.SubTotal + ",Due=Due +" + objInvoice.Due + ",IsVoucherPaid=" + 0 + ",UpdateDate= GETDATE()" + " where OrderVoucherId=" + voucherName.Rows[0]["OrderVoucherId"]);
                }
                else
                {
                    using (scope = new TransactionScope())
                    {
                        try
                        {
                            using (unitOfWork = new UnitOfWork())
                            {
                                voucher1.GrandTotal = objInvoice.GrandTotal;
                                voucher1.SubTotal = objInvoice.SubTotal;
                                voucher1.CreateDate = DateTime.Now;
                                voucher1.UpdateDate = DateTime.Now;
                                voucher1.IsActive = true;
                                voucher1.Due = objInvoice.Due;
                                voucher1.IsVoucherPaid = false;
                                voucher1.VoucherValue = this.voucherCharges;
                                voucher1.VoucherNumber = this.voucherName != null ? this.voucherName : "Wallet Credit";
                                voucher1.WalletCredit = this.WalletAmount;
                                unitOfWork.VoucherRepository.Insert(voucher1);
                                unitOfWork.Save();
                            }

                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public int updateOrderStatus(string qry)
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
        private long InsertQInvoice(bool ShowSuccessMsg, bool isHoldInvoice, int UserId, bool IsQInvoice)
        {
            TransactionScope scope;
            double due = 0;
            double balance = 0;
            double advance = 0;
            //------------Patitent Required For Narcotic items -------------//
            List<long> ItemIds = new List<long>();
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                ItemIds.Add(Convert.ToInt32(r.Cells["colItemId"].Value));
                if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                {
                    if (string.IsNullOrEmpty(SharedFunctions.ObjectToString(r.Cells["colPCTCode"].Value)))
                    {
                        SharedFunctions.MessageBoxShow("PCT code is required on each item", MessageType.Info);
                        return 0;
                    }
                }
            }


            if (SharedVariables.AdminPharmacySetting.IsPatientSelectionMandatory && this.PatientId <= 0 && txtSearchByName.Text.Trim() == "")
            {
                SharedFunctions.MessageBoxShow("Patient selection is mandatory", MessageType.Info);
                return 0;
            }
            else
            {
                using (unitOfWork = new UnitOfWork())
                {
                    bool IsNarcoticItem = unitOfWork.ItemRspository.AnyNarcoticItem(ItemIds);
                    if (IsNarcoticItem && this.PatientId <= 0 && txtSearchByName.Text.Trim() == "")
                    {
                        SharedFunctions.MessageBoxShow("Patient is required for narcotic item", MessageType.Info);
                        return 0;
                    }
                }
            }
            if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "" && !string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) && txtSearchByPhone.Text.Trim().Length < 10)
            {
                SharedFunctions.MessageBoxShow("Patient phone should be 10 characters.", MessageType.Info);
                return 0;
            }
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        if (!CheckAndAssingStore(objInvoice))
                            return 0;
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        objInvoice.Services = Convert.ToDouble(txtServices.Value);
                        objInvoice.ServiceType = cmbPosServiceType.GetItemText(cmbPosServiceType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        due = double.Parse(lblDue.Text);
                        balance = double.Parse(lblBalance.Text);
                        advance = double.Parse(lblAdvance.Text);
                        objInvoice.IsQInvoice = true;
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.IsNew = true;
                        objInvoice.IsActive = true;
                        objInvoice.InvoiceItems = new List<InvoiceItem>();
                        objInvoice.User = unitOfWork.UserRepository.GetById(UserId);
                        objInvoice.IsProcedureInvoice = false;
                        objInvoice.IsHoldInvoice = isHoldInvoice;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        //StockConsumption ObjCons = new StockConsumption();
                        //ObjCons.CreateDate = DateTime.Now;
                        //myDate = ObjCons.CreateDate;
                        //ObjCons.UpdateDate = DateTime.Now;
                        //ObjCons.IsNew = true;
                        //ObjCons.IsActive = false;

                        //ObjCons.User = unitOfWork.UserRepository.GetById(UserId);
                        //ObjCons.StockConsumptions = new List<StockConsumptionItem>();
                        int SrNo = 0;
                        double TCost = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            //int val = Convert.ToInt32(r.Cells["colBatch"].Value);
                            InvoiceItem objInvoiceItem = new InvoiceItem();
                            FillObject(objInvoiceItem, r);
                            objInvoiceItem.IsQInvoice = true;
                            if (SharedFunctions.IsDoubleFormat(objInvoiceItem.Rate, "Item Rate Price"))
                                return 0;
                            if (SharedFunctions.IsDoubleFormat(objInvoiceItem.PerUnitCostPrice, "Item Unit Cost Price"))
                                return 0;
                            objInvoiceItem.SerialNo = SrNo += 1;

                            objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                            if (objInvoiceItem.Quantity <= 0)
                            {
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo) // this check is not in used
                            {
                                objInvoiceItem.Batch = null;
                                objInvoiceItem.IsOptionalBatch = true;
                                int avQty = Convert.ToInt32(r.Cells["colAvailableQty"].Value);
                                if (avQty == 0)
                                {
                                    MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                                else if (objInvoiceItem.Quantity > avQty)
                                {
                                    MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                            }
                            else
                            {
                                objInvoiceItem.IsOptionalBatch = false;
                                if (objInvoiceItem.BatchId <= 0)
                                {
                                    MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                                else
                                {
                                    objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                }

                                //SCMB
                                //if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.Item.ItemID) && !SharedVariables.AdminPharmacySetting.AllowNegCons && !SharedVariables.AdminPharmacySetting.IsConsumptionFromMultipleBatches)
                                //{
                                //    MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    return 0;
                                //}

                                if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.Item.ItemID) && !SharedVariables.AdminPharmacySetting.AllowNegCons)
                                {
                                    MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return 0;
                                }
                            }
                            objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                            // botton lines code has been shifted in "InsertStockConsumption()" method
                            //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                            //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                            //following 2 lines of code were added due to default items issue handeling.
                            objInvoiceItem.Item.IsSyncable = true;
                            unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Modified;
                            objInvoiceItem.IsNew = true;
                            objInvoiceItem.IsActive = true;
                            objInvoiceItem.saletax = Convert.ToDouble(r.Cells["colSaleTax"].Value);
                            objInvoiceItem.saletaxType = r.Cells["colSaleTaxType"].Value == "%" ? 0 : 1;
                            objInvoiceItem.CreateDate = DateTime.Now;
                            objInvoiceItem.UpdateDate = DateTime.Now;
                            objInvoiceItem.Procedure = unitOfWork.ProcedureRepository.GetDefaultProcedures();
                            //TCost += InsertStockConsumption(ObjCons, ref objInvoiceItem);
                            objInvoice.TotalUnitCost = TCost;
                            objInvoice.InvoiceItems.Add(objInvoiceItem);
                        }

                        if (!AddInvoicePayments(objInvoice))
                        {
                            return 0;
                        }
                        if (balance > 0)
                        {
                            objInvoice.InvoicePayments.Last().Payment -= balance;
                        }
                        objInvoice.TotalPaid = this.TotalPaid;

                        // conmmented on 2021-02-04
                        // dues are in positive.
                        // advance is in negatve.
                        //if (this.PatientId > 0)
                        //{
                        //    objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        //    //objInvoice.Patient.Dues += due > 0 ? due : advance;
                        //    if (due > 0)
                        //    {
                        //        objInvoice.Patient.Dues -= due;
                        //    }
                        //    else
                        //    {
                        //        objInvoice.Patient.Dues += advance;
                        //    }
                        //    if (objInvoice.Patient.IsSynced)
                        //    {
                        //        objInvoice.Patient.IsNew = false;
                        //        objInvoice.Patient.IsUpdate = true;
                        //        objInvoice.Patient.IsSynced = false;
                        //    }
                        //    unitOfWork.GetDbContext().Entry(objInvoice.Patient).State = System.Data.Entity.EntityState.Modified;
                        //}


                        if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "")
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
                            if (due > 0)
                            {
                                newP.Dues -= due;
                            }
                            else if (advance > 0)
                            {
                                newP.Dues += advance;
                            }

                            objInvoice.Patient = newP;
                            AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + newP.MRNo, AuditModule.Patient, AuditAction.created);
                        }
                        if (this.PatientId > 0)
                        {
                            double patientBal = unitOfWork.InvoiceRepository.GetInvoicesBalanceByPatientId(this.PatientId);
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(this.PatientId);
                            if (due > 0)
                            {
                                objInvoice.Patient.Dues = -patientBal - due;
                            }
                            else if (advance > 0)
                            {
                                objInvoice.Patient.Dues = -patientBal + advance;
                            }
                            if (objInvoice.Patient.IsSynced)
                            {
                                objInvoice.Patient.IsNew = false;
                                objInvoice.Patient.IsUpdate = true;
                                objInvoice.Patient.IsSynced = false;
                            }
                        }

                        if (due > 0)
                        {
                            objInvoice.Due = due;
                        }
                        else if (advance > 0)
                        {
                            objInvoice.Due = -advance;
                        }
                        //objInvoice.StockConsumption = ObjCons;
                        //unitOfWork.StockConsumptionRepository.Insert(ObjCons);
                        unitOfWork.InvoiceRepository.Insert(objInvoice);
                        objInvoice.InvoiceRefNo = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                        //AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + objInvoice.InvoiceRefNo, AuditModule.Invoice, AuditAction.created);
                        unitOfWork.Save();
                        //SOT
                        //if (!isHoldInvoice)
                        //{
                        //if (this.PatientId <= 0 && txtSearchByName.Text.Trim() != "")
                        //OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                        //if (this.PatientId > 0)
                        //OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Update, objInvoice.Patient.PatientId);
                        //OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Create, objInvoice.InvoiceId);
                        //}
                        // for sake of saving invoiceid to stockConsumption :  its not relation but required for syncing purpose with web
                        //StockConsumption con = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(objInvoice.StockConsumption.StockConsumptionId);
                        //con.InvoiceId = objInvoice.InvoiceId;
                        //List<StockConsumptionItem> StockConsumpList = new List<StockConsumptionItem>();
                        //int stt = 0;
                        //foreach (StockConsumptionItem sci in con.StockConsumptions)
                        //{
                        //    sci.InvoiceId = objInvoice.InvoiceId;
                        //    foreach (DataGridViewRow r in grdItems.Rows)
                        //    {
                        //        if (Convert.ToInt32(r.Cells["colItemId"].Value) == sci.Item.ItemID && Convert.ToInt32(r.Cells["colBatchId"].Value) == sci.Batch.BatchId)
                        //        {
                        //            sci.Unit = r.Cells["colUnit"].Value.ToString() == "Pack" || r.Cells["colUnit"].Value.ToString() == "pack" ? 0 : 1;
                        //        }
                        //    }
                        //    StockConsumpList.Add(sci);
                        //    unitOfWork.GetDbContext().Entry<StockConsumptionItem>(sci).State = System.Data.Entity.EntityState.Modified;
                        //}
                        //unitOfWork.StockConsumptionRepository.Update(con);
                        //unitOfWork.Save();

                        //SOT
                        //if (!isHoldInvoice)
                        //{
                        //    //OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumpList, ModuleName.BulkStockConsumptionItems, ModuleAction.Create, 0);
                        //}

                        //if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                        //{
                        //    FBRResponseVM fbrModel = SendInvoiceToFBR(objInvoice);
                        //    if (!string.IsNullOrEmpty(fbrModel.Error))
                        //    {
                        //        SharedFunctions.MessageBoxShow(fbrModel.Error, MessageType.Error);
                        //        return 0;
                        //    }
                        //    objInvoice.FBRCode = fbrModel.Code;
                        //    objInvoice.FBRInvoiceNo = fbrModel.InvoiceNumber;
                        //}
                        //unitOfWork.Save();
                        scope.Complete();
                        //checkStock();
                        if (ShowSuccessMsg)
                        {
                            //Total Discount Enable
                            discountChange = false;
                            numTotalDiscount.Controls[0].Visible = true;
                            grdItems.Columns["colDiscount"].ReadOnly = false;

                            MessageBox.Show("Estimate Bill Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearAll(true);
                        }
                        return objInvoice.InvoiceId;
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
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private bool CheckAndAssingStore(Invoice objInvoice)
        {
            bool IsError = true;
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                if (SharedFunctions.StringToInt(cmbStores.SelectedValue) <= 0)
                {
                    SharedFunctions.MessageBoxShow("Select store", MessageType.Info);
                    return IsError = false;
                }
                Store store = unitOfWork.StoreRepository.GetById(SharedFunctions.StringToInt(cmbStores.SelectedValue));
                if (store == null)
                {
                    SharedFunctions.MessageBoxShow("Unable to get store details", MessageType.Info);
                    return IsError = false;
                }
                objInvoice.Store = store;
            }
            return IsError;
        }
        private Tuple<double, double, double> BalanceGrossProfit()
        {
            Tuple<double, double, double> Values = null;
            double SumofPayments = Convert.ToDouble(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            double SumofNetAmount = GetSumofNetAmount();

            if (SumofNetAmount == SumofPayments)
            {
                return null;
            }
            else
            {
                double SumofDiscountValue = ConvertPercToValue(Convert.ToDouble(numTotalDiscount.Value), Convert.ToDouble(lblSubTotal.Text), cmbTotalDiscountType.SelectedIndex);
                int SumofQty = getSumofQty();
                double SumofSaleTax = GetSumofSaleTax();
                Values = new Tuple<double, double, double>(SumofNetAmount - SumofPayments, SumofDiscountValue, SumofPayments);
                return Values;
            }
        }
        private double GetSumofSaleTax()
        {
            double sumofSaletax = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                if (row.Cells["colSaleTaxType"].Value.ToString() == "%")
                {
                    sumofSaletax += Convert.ToInt32(row.Cells["colSaleTax"].Value.ToString());
                }
                else
                {
                    sumofSaletax += (Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString()) / 100) * Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
                }
            }
            return sumofSaletax;
        }
        private double GetSumofNetAmount()
        {
            double sumofnetamount = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                sumofnetamount += Convert.ToDouble(row.Cells["colNetAmount"].Value.ToString());
            }
            return sumofnetamount;
        }
        private int getSumofQty()
        {
            int qty = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                qty += Convert.ToInt32(row.Cells["colQuantity"].Value.ToString());
            }
            return qty;
        }
        private double ConvertPercToValue(double TotalDiscount, double TotalAmount, int type)
        {
            double Discount = 0;
            if (type == 1)
            {
                Discount = TotalDiscount / 100 * TotalAmount;
            }
            else
            {
                Discount = TotalDiscount;
            }

            return Discount;
        }

        private bool TotalDiscountFound()
        {
            bool Found = false;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                if (Convert.ToInt32(row.Cells["colDiscount"].Value) > 0)
                {
                    Found = true;
                }
            }
            return Found;
        }
        private bool IsStockAvailable(DataGridViewRow r, int batchId, int itemId)
        {
            BatchStockVM vm = new BatchStockVM();
            int saleQty = Convert.ToInt32(r.Cells["colQuantity"].Value);

            vm = unitOfWork.ItemRspository.GetBatchStock(batchId,
                itemId,
                SharedVariables.AdminPharmacySetting.IsEnableStores ? SharedFunctions.StringToInt(cmbStores.SelectedValue) : 0,
                SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);

            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                SharedFunctions.CalculateInvoiceItemQty(vm);
                int availableQty = vm.AvailableStock;
                //int availableQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                //{
                //    availableQty += vm.HoldStock;
                //}
                double rate = vm.RetailPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                    availableQty = (int)Math.Floor((double)(availableQty / convUnit));
                }
                if (saleQty <= availableQty)
                {
                    return true;
                }
            }
            return false;
        }
        private double InsertStockConsumption(StockConsumption objConsumption, ref InvoiceItem objInvoiceItem)
        {
            double itemTotalCost = 0;
            double itemDiscCost = 0;
            int remainingQty = objInvoiceItem.Quantity;
            int ItemId = objInvoiceItem.Item.ItemID;
            //if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo) // this function is not in used 
            //{
            //    foreach (ItemDetailVM i in this.AddedItemsData)
            //    {
            //        if (i.Item.ItemID == objInvoiceItem.Item.ItemID)
            //        {
            //            foreach (BatchStockVM b in i.BatchStockList)
            //            {
            //                if (remainingQty <= b.AvailableStock)
            //                {
            //                    objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, b.BatchId);
            //                    itemTotalCost += objInvoiceItem.PerUnitCostPrice * remainingQty;
            //                    this.ConsumeStock(ref objConsumption, objInvoiceItem.Item, (int)b.BatchId, remainingQty);
            //                    break;
            //                }
            //                else
            //                {
            //                    int consumeQty = b.AvailableStock;
            //                    remainingQty -= consumeQty;
            //                    objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, b.BatchId);
            //                    itemTotalCost += objInvoiceItem.PerUnitCostPrice * consumeQty;
            //                    this.ConsumeStock(ref objConsumption, objInvoiceItem.Item, (int)b.BatchId, consumeQty);
            //                }
            //            }
            //            break;
            //        }
            //    }
            //    objInvoiceItem.TotalCostPrice = itemTotalCost;
            //}
            int batchId = objInvoiceItem.Batch.BatchId;

            if (SharedVariables.AdminPharmacySetting.IsConsumptionFromMultipleBatches)
            {
                foreach (ItemDetailVM i in this.AddedItemsData.Where(x => x.Item.ItemID == ItemId))
                {
                    List<BatchStockVM> batchList = new List<BatchStockVM>();
                    batchList.AddRange(i.BatchStockList);
                    BatchStockVM batch = i.BatchStockList.Where(x => x.BatchId == batchId).FirstOrDefault();
                    if (batch != null)
                    {
                        batchList.Remove(batch);
                        batchList.Insert(0, batch);
                    }
                    foreach (BatchStockVM b in batchList)
                    {
                        if (b.AvailableStock > 0)
                        {
                            if (remainingQty <= b.AvailableStock)
                            {
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, b.BatchId);
                                itemTotalCost += objInvoiceItem.PerUnitCostPrice * remainingQty;
                                itemDiscCost += objInvoiceItem.DiscountPrice * remainingQty;
                                this.ConsumeStock(ref objConsumption, objInvoiceItem.Item, (int)b.BatchId, remainingQty);
                                break;
                            }
                            else
                            {
                                int consumeQty = b.AvailableStock;
                                remainingQty -= consumeQty;
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, b.BatchId);
                                itemTotalCost += objInvoiceItem.PerUnitCostPrice * consumeQty;
                                itemDiscCost += objInvoiceItem.DiscountPrice * consumeQty;
                                this.ConsumeStock(ref objConsumption, objInvoiceItem.Item, (int)b.BatchId, consumeQty);
                            }
                        }
                    }
                }
                objInvoiceItem.TotalCostPrice = itemTotalCost;
                objInvoiceItem.TotalDiscPrice = itemDiscCost;
            }
            else
            {
                objInvoiceItem.PerUnitCostPrice = objInvoiceItem.PerUnitCostPrice;
                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                itemTotalCost += objInvoiceItem.TotalCostPrice;
                itemDiscCost += objInvoiceItem.DiscountPrice * objInvoiceItem.Quantity;
                this.ConsumeStock(ref objConsumption, objInvoiceItem.Item, objInvoiceItem.Batch.BatchId, objInvoiceItem.Quantity);
            }
            return itemTotalCost;
        }

        private void ConsumeStock(ref StockConsumption objConsumption, Item item, int batchId, int quantity)
        {
            StockConsumptionItem sc = new StockConsumptionItem();
            sc.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            sc.Quantity = quantity;
            sc.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
            sc.Comment = "Sale Transaction";
            sc.Item = item;
            sc.Batch = unitOfWork.BatchRepository.GetById(batchId);
            sc.CreateDate = DateTime.Now;
            sc.UpdateDate = DateTime.Now;
            sc.IsNew = true;
            sc.IsActive = true;
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                sc.Store = unitOfWork.StoreRepository.GetById(SharedFunctions.StringToInt(cmbStores.SelectedValue));
            objConsumption.StockConsumptions.Add(sc);
        }
        private long EditInvoice(long invoiceId, bool ShowSuccessMsg, bool isSavingHoldInvoice, int userId) // param "isSavingHoldInvoice" : will true only when user is going to perform "Save" action on an invoice that was on hold
        {
            double loadedDueAmt = 0;
            double loadedPmtsSum = 0;

            double due = 0;
            double advance = 0;
            double balance = 0;
            //if (txtSearchByName.Text.Trim() != "" && string.IsNullOrEmpty(txtSearchByPhone.Text))
            //{
            //    SharedFunctions.MessageBoxShow("Patient phone number is required.", MessageType.Info);
            //    return 0;
            //}
            //if (SharedVariables.AdminPharmacySetting.AskForPIN)
            //{
            //    try
            //    {
            //        int pin = Convert.ToInt32(Interaction.InputBox("Enter PIN for User '" + SharedVariables.LoggedInUser.UserName + "' to Save the Invoice"));
            //        User user = new User();
            //        using (unitOfWork = new UnitOfWork())
            //        {
            //            user = unitOfWork.UserRepository.IsInvalidPIN(SharedVariables.LoggedInUser.Email, pin);
            //        }
            //        if (user == null)
            //        {
            //            MessageBox.Show("Invoice save cancelled, You are not allowed to update invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //            return 0;
            //        }
            //        userId = user.UserId;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Invoice save cancelled, You are not allowed to update invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return 0;
            //    }
            //}
            if (txtSearchByName.Text.Trim() != "" && !string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) && txtSearchByPhone.Text.Trim().Length < 10)
            {
                SharedFunctions.MessageBoxShow("Patient phone should be 10 characters.", MessageType.Info);
                return 0;
            }
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        if (!CheckAndAssingStore(objInvoice))
                            return 0;
                        double sumofPayments = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoicceByIdWithDetails(invoiceId);
                        loadedPmtsSum = objInvoice.InvoicePayments.Sum(ip => ip.Payment);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.Services = Convert.ToDouble(txtServices.Value);
                        objInvoice.ServiceType = cmbPosServiceType.GetItemText(cmbPosServiceType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        if (!IsReturnedQuantityFound)
                        {
                            objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        }
                        objInvoice.InvoiceNote = txtInvoiceNote.Text.Trim();
                        objInvoice.UpdateDate = DateTime.Now;
                        loadedDueAmt = objInvoice.Due;
                        objInvoice.User = unitOfWork.UserRepository.GetById(userId);
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        objInvoice.TotalUnitCost = 0;

                        if (isSavingHoldInvoice)
                        {
                            objInvoice.IsHoldInvoice = false;
                        }

                        if (!IsReturnedQuantityFound)
                        {
                            due = double.Parse(lblDue.Text);
                            balance = double.Parse(lblBalance.Text);
                            advance = double.Parse(lblAdvance.Text);
                        }

                        if (objInvoice.IsSynced)
                        {
                            objInvoice.IsNew = false;
                            objInvoice.IsUpdate = true;
                            objInvoice.IsSynced = false;
                        }

                        long InvoiceItemId = 0;
                        foreach (InvoiceItemVM DeletedItem in DeletedInvoiceItems)
                        {
                            InvoiceItem _DeletedItem = objInvoice.InvoiceItems.Where(i => i.Item.ItemID == DeletedItem.ItemId && i.Batch.BatchId == DeletedItem.BatchId && i.IsActive).FirstOrDefault();
                            _DeletedItem.IsUpdate = true;
                            _DeletedItem.IsActive = false;
                            _DeletedItem.IsSynced = false;
                            _DeletedItem.DeletedAt = DateTime.Now;
                            _DeletedItem.UpdateDate = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(_DeletedItem).State = System.Data.Entity.EntityState.Modified;
                            // commected for consume stock change
                            List<StockConsumptionItem> DeletedCons = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).ToList();

                            //StockConsumptionItem DeletedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).FirstOrDefault();DeletedCon.IsSynced = false;
                            foreach (StockConsumptionItem dltd in DeletedCons)
                            {
                                dltd.IsActive = false;
                                dltd.IsUpdate = true;
                                dltd.UpdateDate = DateTime.Now;
                            }
                            //unitOfWork.GetDbContext().Entry(DeletedCon).State = System.Data.Entity.EntityState.Modified;
                        }
                        int srNo = 0;
                        List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);

                            if (Convert.ToInt32(r.Cells["colQuantity"].Value) <= 0)
                            {
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }


                            if (InvoiceItemId == 0) // new items
                            {
                                InvoiceItem objInvoiceItem = new InvoiceItem();
                                FillObject(objInvoiceItem, r);
                                if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                                {
                                    objInvoiceItem.Batch = null;
                                    objInvoiceItem.IsOptionalBatch = true;
                                    int avQty = Convert.ToInt32(r.Cells["colAvailableQty"].Value);
                                    if (avQty == 0)
                                    {
                                        MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else if (objInvoiceItem.Quantity > avQty)
                                    {
                                        MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    objInvoiceItem.IsOptionalBatch = false;
                                    if (objInvoiceItem.BatchId <= 0)
                                    {
                                        MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else
                                    {
                                        objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                    }

                                    if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.ItemId))
                                    {
                                        MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                }
                                objInvoiceItem.SerialNo = srNo += 1;
                                objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                //objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                                //commented due change in loginc of calculcating PerUnitCost and total unit cost for invoice and invoice items.
                                //objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId);
                                //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                                //following 2 lines of code were added due to default items issue handeling.
                                objInvoiceItem.Item.IsSyncable = true;
                                unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Modified;

                                objInvoiceItem.IsNew = true;
                                objInvoiceItem.IsActive = true;
                                objInvoiceItem.CreateDate = DateTime.Now;
                                objInvoiceItem.UpdateDate = DateTime.Now;

                                objInvoice.InvoiceItems.Add(objInvoiceItem);
                                this.InsertStockConsumption(objInvoice.StockConsumption, ref objInvoiceItem);
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId);
                                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                objInvoiceItem.TotalDiscPrice = objInvoiceItem.DiscountedPrice * (objInvoiceItem.Unit == 0 ? (objInvoiceItem.Quantity / objInvoiceItem.ConvUnit) : objInvoiceItem.Quantity);
                                //StockConsumptionItem scItem = new StockConsumptionItem();
                                //scItem.User = unitOfWork.UserRepository.GetById(userId);
                                //scItem.Quantity = objInvoiceItem.Quantity;
                                //scItem.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //scItem.Comment = "Sale Transaction";
                                //scItem.Item = objInvoiceItem.Item;
                                //scItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //scItem.CreateDate = DateTime.Now;
                                //scItem.UpdateDate = DateTime.Now;
                                //scItem.IsNew = true;
                                //scItem.IsActive = true;
                                //objInvoice.StockConsumption.StockConsumptions.Add(scItem);
                                //if (Math.Round(Convert.ToDouble(lblGrandTotal.Text), 0) <= Math.Round(sumofPayments, 0) || (Math.Round(Convert.ToDouble(lblGrandTotal.Text), 0) - Math.Round(sumofPayments, 0)) < 1)
                                //{
                                //    ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                                //    #region Record Profit Statement Transaction
                                //    Transaction.CreatedAt = DateTime.Now;
                                //    Transaction.TransactionDate = DateTime.Now;
                                //    Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.Unit == 1 ? (objInvoiceItem.DiscountedPrice * objInvoiceItem.Quantity) : (objInvoiceItem.DiscountedPrice / objInvoiceItem.ConvUnit) * objInvoiceItem.Quantity;
                                //    Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //    Transaction.TransactionDiscount = objInvoice.TotalDiscount;
                                //    Transaction.TransactionRevenue = objInvoiceItem.NetAmount;
                                //    Transaction.User = SharedVariables.LoggedInUser.UserId;
                                //    Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                //    Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                //    Transaction.IsDeleted = false;
                                //    objTransaction.Add(Transaction);
                                //    #endregion
                                //}

                            }
                            // editing of an item is not allowed currently. simply uncomment below else part to enable editing of an item.
                            // also if you gonna enable editing of an item then manage stock consumption for optional batch case as in case of new invoice items insertion.
                            else
                            {

                                InvoiceItem objInvoiceItem = objInvoice.InvoiceItems.Where(item => item.InvoiceItemId == InvoiceItemId).FirstOrDefault();
                                double Revenue = 0;
                                double CostofSales = 0;
                                double DiscountedCostOfSales = 0;
                                double discount = 0;
                                if (r.Cells["colQuantityFirst"].Value != null)
                                {
                                    Revenue = Convert.ToDouble(r.Cells["colNetAmount"].Value.ToString()) - objInvoiceItem.NetAmount;
                                    CostofSales = ((double)r.Cells["colCostPrice"].Value * Convert.ToInt32(r.Cells["colQuantity"].Value.ToString())) - ((double)r.Cells["colCostPrice"].Value * Convert.ToInt32(r.Cells["colQuantityFirst"].Value.ToString()));
                                    DiscountedCostOfSales = (Convert.ToDouble(r.Cells["DiscountedPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colQuantity"].Value.ToString())) - (objInvoiceItem.DiscountedPrice * Convert.ToInt32(r.Cells["colQuantityFirst"].Value.ToString()));
                                    discount = objInvoiceItem.Discount - Convert.ToDouble(r.Cells["colDiscount"].Value.ToString());
                                }
                                else
                                {
                                    Revenue = Convert.ToDouble(r.Cells["colNetAmount"].Value.ToString()) - objInvoiceItem.NetAmount;
                                    CostofSales = ((double)r.Cells["colCostPrice"].Value * Convert.ToInt32(r.Cells["colQuantity"].Value.ToString()));
                                    DiscountedCostOfSales = (Convert.ToDouble(r.Cells["DiscountedPrice"].Value.ToString()) * Convert.ToInt32(r.Cells["colQuantity"].Value.ToString()));
                                    discount = objInvoiceItem.Discount - Convert.ToDouble(r.Cells["colDiscount"].Value.ToString());
                                }
                                FillObject(objInvoiceItem, r);
                                objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                objInvoiceItem.SerialNo = srNo += 1;
                                objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                                objInvoiceItem.TotalCostPrice = (unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId) * objInvoiceItem.Quantity);
                                objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;
                                objInvoiceItem.IsUpdate = true;
                                objInvoiceItem.IsSynced = false;
                                objInvoiceItem.UpdateDate = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(objInvoiceItem).State = System.Data.Entity.EntityState.Modified;
                                StockConsumptionItem ModifiedCon = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                                ModifiedCon.User = unitOfWork.UserRepository.GetById(userId);
                                ModifiedCon.Quantity = objInvoiceItem.Quantity;
                                ModifiedCon.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                ModifiedCon.Comment = "Sale Transaction";
                                ModifiedCon.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                ModifiedCon.IsUpdate = true;
                                ModifiedCon.UpdateDate = DateTime.Now;
                                ModifiedCon.IsSynced = false;
                                unitOfWork.GetDbContext().Entry(ModifiedCon).State = System.Data.Entity.EntityState.Modified;
                                //ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                                //if (r.Cells["colQuantityFirst"].Value != null && Convert.ToInt32(r.Cells["colQuantity"].Value.ToString()) != Convert.ToInt32(r.Cells["colQuantityFirst"].Value.ToString()) && objInvoice.Due == 0)
                                //{
                                //    if (!(due > 0))
                                //    {


                                //        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                                //        #region Record Profit Statement Transaction
                                //        Transaction.CreatedAt = DateTime.Now;
                                //        Transaction.TransactionDate = DateTime.Now;
                                //        Transaction.TransactionDiscountedCostOfSale = DiscountedCostOfSales;
                                //        Transaction.TransactionCostOfSales = CostofSales;
                                //        Transaction.TransactionDiscount = discount;
                                //        Transaction.TransactionRevenue = Revenue;
                                //        Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                //        Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                //        Transaction.IsDeleted = false;
                                //        Transaction.User = SharedVariables.LoggedInUser.UserId;
                                //        objTransaction.Add(Transaction);
                                //    }
                                //    #endregion
                                //}
                                //else if (r.Cells["colQuantityFirst"].Value != null && Convert.ToInt32(r.Cells["colQuantity"].Value.ToString()) == Convert.ToInt32(r.Cells["colQuantityFirst"].Value.ToString()) && Convert.ToDouble(lblPaymentPrice.Text) != objInvoice.TotalPaid && objInvoiceItem.NetAmount != Convert.ToDouble(r.Cells["colNetAmount"].Value.ToString()))
                                //{
                                //    ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                                //    #region Record Profit Statement Transaction
                                //    Transaction.CreatedAt = DateTime.Now;
                                //    Transaction.TransactionDate = DateTime.Now;
                                //    Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.DiscountedPrice * (objInvoiceItem.Unit == 1 ? objInvoiceItem.Quantity : objInvoiceItem.Quantity / objInvoiceItem.Item.ConversionUnit);
                                //    Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //    Transaction.TransactionDiscount = objInvoiceItem.DiscountType == 1 ? objInvoiceItem.Discount : objInvoiceItem.Discount / 100 * objInvoiceItem.NetAmount;
                                //    Transaction.TransactionRevenue = objInvoiceItem.NetAmount;
                                //    Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                //    Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                //    Transaction.IsDeleted = false;
                                //    Transaction.User = SharedVariables.LoggedInUser.UserId;
                                //    objTransaction.Add(Transaction);
                                //    #endregion
                                //}
                                //else if (objInvoice.Due > 0 && objInvoice.TotalPaid != Convert.ToDouble(lblPaymentPrice.Text))
                                //{
                                //    ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                                //    #region Record Profit Statement Transaction
                                //    Transaction.CreatedAt = DateTime.Now;
                                //    Transaction.TransactionDate = DateTime.Now;
                                //    Transaction.TransactionDiscountedCostOfSale = objInvoiceItem.DiscountedPrice * (objInvoiceItem.Unit == 1 ? objInvoiceItem.Quantity : objInvoiceItem.Quantity / objInvoiceItem.Item.ConversionUnit);
                                //    Transaction.TransactionCostOfSales = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //    Transaction.TransactionDiscount = objInvoiceItem.DiscountType == 1 ? objInvoiceItem.Discount : objInvoiceItem.Discount / 100 * objInvoiceItem.NetAmount;
                                //    Transaction.TransactionRevenue = objInvoiceItem.NetAmount;
                                //    Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                //    Transaction.ItemId = objInvoiceItem.Item.ItemID;
                                //    Transaction.IsDeleted = false;
                                //    Transaction.User = SharedVariables.LoggedInUser.UserId;
                                //    objTransaction.Add(Transaction);
                                //    #endregion
                                //}


                            }

                        }
                        CalculateGrossProfitStatement(objInvoice.InvoiceRefNo);
                        DataTable dt = get("select TransactionDiscount , TransactionRevenue from ProfitStatementTransactions where InvoiceId=" + objInvoice.InvoiceRefNo);
                        double sum = 0;
                        double sumofdisc = 0;
                        double sumofdisc786 = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            sum += Convert.ToDouble(row[1].ToString());
                            sumofdisc += Convert.ToDouble(row[0].ToString());
                        }
                        foreach (DataGridViewRow row in grdItems.Rows)
                        {
                            if (row.Cells["colDiscountType"].Value.ToString() == "%")
                            {
                                sumofdisc786 += Convert.ToDouble(row.Cells["colDiscount"].Value.ToString()) / 100 * Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
                            }
                            else
                            {
                                sumofdisc786 += Convert.ToDouble(row.Cells["colDiscount"].Value.ToString());
                            }
                        }
                        Tuple<double, double, double> Values = BalanceGrossProfit();
                        double sumofDiscoff = sumofdisc786 - sumofdisc;
                        double DiscDiff = ConvertPercToValue(Convert.ToDouble(numTotalDiscount.Value), Convert.ToDouble(lblSubTotal.Text), cmbTotalDiscountType.SelectedIndex) - sumofdisc;
                        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                        //Transaction.TransactionDiscount = DiscDiff;
                        double DueAmount = Convert.ToDouble(lblDue.Text);
                        double AdvanceAmount = Convert.ToDouble(lblAdvance.Text);

                        Transaction.TransactionRevenue = diffofreve;
                        Transaction.TransactionCostOfSales = diffofcost;
                        Transaction.TransactionDiscount = diffofDisc;
                        Transaction.TransactionDiscountedCostOfSale = diffofDisccost;
                        Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                        Transaction.CreatedAt = DateTime.Now;
                        Transaction.UpdatedAt = DateTime.Now;
                        Transaction.User = SharedVariables.LoggedInUser.UserId;
                        Transaction.ItemId = -1;
                        Transaction.TransactionDate = DateTime.Now;
                        Transaction.IsDeleted = false;
                        objTransaction.Add(Transaction);

                        if (DueAmount > 0)
                        {
                            ProfitStatementTransactions Transaction1 = new ProfitStatementTransactions();
                            Transaction1.TransactionRevenue = -1 * DueAmount;
                            Transaction1.InvoiceId = objInvoice.InvoiceRefNo;
                            Transaction1.CreatedAt = DateTime.Now;
                            Transaction1.UpdatedAt = DateTime.Now;
                            Transaction1.User = SharedVariables.LoggedInUser.UserId;
                            Transaction1.ItemId = -1;
                            Transaction1.TransactionDate = DateTime.Now;
                            Transaction1.IsDeleted = false;
                            objTransaction.Add(Transaction1);
                        }
                        if (AdvanceAmount > 0)
                        {
                            ProfitStatementTransactions Transaction2 = new ProfitStatementTransactions();
                            Transaction2.TransactionRevenue = AdvanceAmount;
                            Transaction2.InvoiceId = objInvoice.InvoiceRefNo;
                            Transaction2.CreatedAt = DateTime.Now;
                            Transaction2.UpdatedAt = DateTime.Now;
                            Transaction2.User = SharedVariables.LoggedInUser.UserId;
                            Transaction2.ItemId = -1;
                            Transaction2.TransactionDate = DateTime.Now;
                            Transaction2.IsDeleted = false;
                            objTransaction.Add(Transaction2);
                        }
                        //if (Values != null && Values.Item3 != sum)
                        //{
                        //    if (Convert.ToInt32(sum - Values.Item3) == Convert.ToInt32(DiscDiff))
                        //    {
                        //        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                        //        //Transaction.TransactionDiscount = DiscDiff;
                        //        Transaction.TransactionRevenue = -1 * Convert.ToDouble(sum - Values.Item3);
                        //        Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                        //        Transaction.CreatedAt = DateTime.Now;
                        //        Transaction.UpdatedAt = DateTime.Now;
                        //        Transaction.User = SharedVariables.LoggedInUser.UserId;
                        //        Transaction.ItemId = -1;
                        //        Transaction.TransactionDate = DateTime.Now;
                        //        Transaction.IsDeleted = false;
                        //        objTransaction.Add(Transaction);
                        //    }
                        //    else
                        //    {
                        //        ProfitStatementTransactions Transaction = new ProfitStatementTransactions();
                        //        Transaction.TransactionRevenue = (-1) * (sum - Values.Item3);
                        //        Transaction.TransactionDiscount = sumofDiscoff;
                        //        Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                        //        Transaction.CreatedAt = DateTime.Now;
                        //        Transaction.UpdatedAt = DateTime.Now;
                        //        Transaction.User = SharedVariables.LoggedInUser.UserId;
                        //        Transaction.ItemId = -1;
                        //        Transaction.TransactionDate = DateTime.Now;
                        //        Transaction.IsDeleted = false;
                        //        objTransaction.Add(Transaction);
                        //    }
                        //}
                        unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                        unitOfWork.Save();
                        objInvoice.TotalUnitCost = objInvoice.InvoiceItems.Where(ii => ii.IsActive).Sum(r => r.TotalCostPrice);
                        if (!IsReturnedQuantityFound)
                        {
                            List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                            if (!AddInvoicePayments(NewPaymentsMade))
                            {
                                return 0;
                            }

                            objInvoice.TotalPaid = this.TotalPaid;
                            bool found = false;

                            foreach (InvoicePayment dp in DeletedPayments)
                            {
                                InvoicePayment DeletedPayment = objInvoice.InvoicePayments.Where(p => p.InvoicePaymentId == dp.InvoicePaymentId).FirstOrDefault();
                                DeletedPayment.IsUpdate = true;
                                DeletedPayment.IsSynced = false;
                                DeletedPayment.UpdatedAt = DateTime.Now;
                                DeletedPayment.IsActive = false;
                                DeletedPayment.DeletedAt = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(DeletedPayment).State = System.Data.Entity.EntityState.Modified;
                            }
                            foreach (InvoicePayment p in NewPaymentsMade)
                            {
                                found = false;
                                foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                                {
                                    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId != 0)
                                    {
                                        found = true;
                                        p2.Payment = p.Payment;
                                        p2.ChequeNumber = p.ChequeNumber;
                                        p2.BankName = p.BankName;
                                        p2.ChequeStatus = p.ChequeStatus;
                                        p2.PaymentDate = p.PaymentDate;
                                        p2.IsUpdate = true;
                                        p2.IsSynced = false;
                                        p2.UpdatedAt = DateTime.Now;
                                        p2.Dues = objInvoice.Due;
                                        p2.P_InvoiceNo = p.P_InvoiceNo;
                                        p2.P_RiderName = p.P_RiderName;
                                        p2.P_VendorName = p.P_VendorName;
                                        unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                                if (!found)
                                {
                                    objInvoice.InvoicePayments.Add(p);
                                }
                            }
                            if (balance > 0)
                            {
                                objInvoice.InvoicePayments.Last().Payment -= balance;
                            }
                        }
                        Patient newP = null;
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }

                        else
                        {
                            if (txtSearchByName.Text.Trim() != "")
                            {
                                newP = new Patient();
                                newP.MRNo = unitOfWork.PatientRepository.GetNewMrNo();
                                newP.Name = txtSearchByName.Text.Trim();
                                newP.Phone = string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) ? "0000000000" : txtSearchByPhone.Text.Trim();
                                newP.Address = txtAddress.Text.Trim();
                                newP.CreateDate = DateTime.Now;
                                newP.UpdateDate = DateTime.Now;
                                newP.RegistrationDate = DateTime.Now;
                                newP.IsActive = true;
                                newP.IsNew = true;
                                newP.Gender = "other";
                                //newP.Dues = -due;
                                if (due > 0)
                                {
                                    newP.Dues -= due;
                                }
                                else if (advance > 0)
                                {
                                    newP.Dues += advance;
                                }
                                objInvoice.Patient = newP;
                                AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + newP.MRNo, AuditModule.Patient, AuditAction.created);
                            }
                        }
                        if (!IsReturnedQuantityFound) // in else invoice due value will remain the same
                        {
                            if (due >= 0)
                            {
                                objInvoice.Due = due;
                            }
                            if (advance > 0)
                            {
                                objInvoice.Due = -advance;
                            }
                            if (balance > 0)
                            {
                                objInvoice.Due = 0;
                            }
                        }

                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + objInvoice.InvoiceRefNo, AuditModule.Invoice, AuditAction.updated);
                        unitOfWork.Save();
                        //SOT
                        if (!objInvoice.IsHoldInvoice)
                        {
                            if (newP == null && objInvoice.Patient != null && objInvoice.Patient.IsSynced)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Update, objInvoice.Patient.PatientId);
                            }
                            else if (newP != null && !string.IsNullOrEmpty(newP.Name) && isSavingHoldInvoice)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                            }
                            else if (isSavingHoldInvoice && objInvoice.Patient != null)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                            }

                            if (!isSavingHoldInvoice)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Update, objInvoice.InvoiceId);
                            }
                            else
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Create, objInvoice.InvoiceId);
                            }
                            List<StockConsumptionItem> StockConsumptionsList = objInvoice.StockConsumption.StockConsumptions;
                            StockConsumptionsList.ForEach(x => x.User = objInvoice.User);
                            if (!isSavingHoldInvoice)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Update, 0);
                            }
                            else
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Create, 0);
                            }
                        }
                        if (PatientId > 0 && !IsReturnedQuantityFound)
                        {
                            double patientBal = unitOfWork.InvoiceRepository.GetInvoicesBalanceByPatientId(this.PatientId);
                            objInvoice.Patient.Dues = -patientBal;
                            if (objInvoice.Patient.IsSynced)
                            {
                                objInvoice.Patient.IsNew = false;
                                objInvoice.Patient.IsUpdate = true;
                                objInvoice.Patient.IsSynced = false;
                            }
                            unitOfWork.GetDbContext().Entry(objInvoice.Patient).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            if (!isSavingHoldInvoice)
                            {
                                MessageBox.Show("Invoice Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        return invoiceId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }

        private long EditQInvoice(long invoiceId, bool ShowSuccessMsg, bool isSavingHoldInvoice, int userId, bool IsQInvoice) // param "isSavingHoldInvoice" : will true only when user is going to perform "Save" action on an invoice that was on hold
        {
            double loadedDueAmt = 0;
            double loadedPmtsSum = 0;

            double due = 0;
            double advance = 0;
            double balance = 0;
            //if (txtSearchByName.Text.Trim() != "" && string.IsNullOrEmpty(txtSearchByPhone.Text))
            //{
            //    SharedFunctions.MessageBoxShow("Patient phone number is required.", MessageType.Info);
            //    return 0;
            //}
            //if (SharedVariables.AdminPharmacySetting.AskForPIN)
            //{
            //    try
            //    {
            //        int pin = Convert.ToInt32(Interaction.InputBox("Enter PIN for User '" + SharedVariables.LoggedInUser.UserName + "' to Save the Invoice"));
            //        User user = new User();
            //        using (unitOfWork = new UnitOfWork())
            //        {
            //            user = unitOfWork.UserRepository.IsInvalidPIN(SharedVariables.LoggedInUser.Email, pin);
            //        }
            //        if (user == null)
            //        {
            //            MessageBox.Show("Invoice save cancelled, You are not allowed to update invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //            return 0;
            //        }
            //        userId = user.UserId;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Invoice save cancelled, You are not allowed to update invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return 0;
            //    }
            //}
            if (txtSearchByName.Text.Trim() != "" && !string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) && txtSearchByPhone.Text.Trim().Length < 10)
            {
                SharedFunctions.MessageBoxShow("Patient phone should be 10 characters.", MessageType.Info);
                return 0;
            }
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        if (!CheckAndAssingStore(objInvoice))
                            return 0;
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoicceByIdWithDetails(invoiceId);
                        loadedPmtsSum = objInvoice.InvoicePayments.Sum(ip => ip.Payment);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.Services = Convert.ToDouble(txtServices.Value);
                        objInvoice.ServiceType = cmbPosServiceType.GetItemText(cmbPosServiceType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        if (!IsReturnedQuantityFound)
                        {
                            objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        }
                        objInvoice.InvoiceNote = txtInvoiceNote.Text.Trim();
                        objInvoice.UpdateDate = DateTime.Now;
                        loadedDueAmt = objInvoice.Due;
                        objInvoice.User = unitOfWork.UserRepository.GetById(userId);
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        objInvoice.TotalUnitCost = 0;
                        objInvoice.IsQInvoice = true;
                        if (isSavingHoldInvoice)
                        {
                            objInvoice.IsHoldInvoice = false;
                        }

                        if (!IsReturnedQuantityFound)
                        {
                            due = double.Parse(lblDue.Text);
                            balance = double.Parse(lblBalance.Text);
                            advance = double.Parse(lblAdvance.Text);
                        }

                        if (objInvoice.IsSynced)
                        {
                            objInvoice.IsNew = false;
                            objInvoice.IsUpdate = true;
                            objInvoice.IsSynced = false;
                        }

                        long InvoiceItemId = 0;
                        foreach (InvoiceItemVM DeletedItem in DeletedInvoiceItems)
                        {
                            InvoiceItem _DeletedItem = objInvoice.InvoiceItems.Where(i => i.Item.ItemID == DeletedItem.ItemId && i.Batch.BatchId == DeletedItem.BatchId && i.IsActive).FirstOrDefault();
                            _DeletedItem.IsUpdate = true;
                            _DeletedItem.IsActive = false;
                            _DeletedItem.IsSynced = false;
                            _DeletedItem.DeletedAt = DateTime.Now;
                            _DeletedItem.UpdateDate = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(_DeletedItem).State = System.Data.Entity.EntityState.Modified;
                            // commected for consume stock change
                            List<StockConsumptionItem> DeletedCons = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).ToList();

                            //StockConsumptionItem DeletedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).FirstOrDefault();DeletedCon.IsSynced = false;
                            //foreach (StockConsumptionItem dltd in DeletedCons)
                            //{
                            //    dltd.IsActive = false;
                            //    dltd.IsUpdate = true;
                            //    dltd.UpdateDate = DateTime.Now;
                            //}
                            //unitOfWork.GetDbContext().Entry(DeletedCon).State = System.Data.Entity.EntityState.Modified;
                        }
                        int srNo = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);

                            if (Convert.ToInt32(r.Cells["colQuantity"].Value) <= 0)
                            {
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }


                            if (InvoiceItemId == 0) // new items
                            {
                                InvoiceItem objInvoiceItem = new InvoiceItem();
                                FillObject(objInvoiceItem, r);
                                if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                                {
                                    objInvoiceItem.Batch = null;
                                    objInvoiceItem.IsOptionalBatch = true;
                                    int avQty = Convert.ToInt32(r.Cells["colAvailableQty"].Value);
                                    if (avQty == 0)
                                    {
                                        MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else if (objInvoiceItem.Quantity > avQty)
                                    {
                                        MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    objInvoiceItem.IsOptionalBatch = false;
                                    if (objInvoiceItem.BatchId <= 0)
                                    {
                                        MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else
                                    {
                                        objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                    }

                                    if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.ItemId))
                                    {
                                        MessageBox.Show("Stock not available for item <" + objInvoiceItem.Item.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                }
                                objInvoiceItem.SerialNo = srNo += 1;
                                objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                //objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                                //commented due change in loginc of calculcating PerUnitCost and total unit cost for invoice and invoice items.
                                //objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId);
                                //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                                //following 2 lines of code were added due to default items issue handeling.
                                objInvoiceItem.Item.IsSyncable = true;
                                unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Modified;

                                objInvoiceItem.IsNew = true;
                                objInvoiceItem.IsActive = true;
                                objInvoiceItem.CreateDate = DateTime.Now;
                                objInvoiceItem.UpdateDate = DateTime.Now;

                                objInvoice.InvoiceItems.Add(objInvoiceItem);
                                //this.InsertStockConsumption(objInvoice.StockConsumption, ref objInvoiceItem);
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId);
                                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                objInvoiceItem.TotalDiscPrice = objInvoiceItem.DiscountedPrice * (objInvoiceItem.Unit == 0 ? (objInvoiceItem.Quantity / objInvoiceItem.ConvUnit) : objInvoiceItem.Quantity);
                                //StockConsumptionItem scItem = new StockConsumptionItem();
                                //scItem.User = unitOfWork.UserRepository.GetById(userId);
                                //scItem.Quantity = objInvoiceItem.Quantity;
                                //scItem.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //scItem.Comment = "Sale Transaction";
                                //scItem.Item = objInvoiceItem.Item;
                                //scItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //scItem.CreateDate = DateTime.Now;
                                //scItem.UpdateDate = DateTime.Now;
                                //scItem.IsNew = true;
                                //scItem.IsActive = true;
                                //objInvoice.StockConsumption.StockConsumptions.Add(scItem);                                
                            }
                            // editing of an item is not allowed currently. simply uncomment below else part to enable editing of an item.
                            // also if you gonna enable editing of an item then manage stock consumption for optional batch case as in case of new invoice items insertion.
                            else
                            {
                                InvoiceItem objInvoiceItem = objInvoice.InvoiceItems.Where(item => item.InvoiceItemId == InvoiceItemId).FirstOrDefault();
                                FillObject(objInvoiceItem, r);
                                ////UpdateRow(objInvoiceItem);
                                //// item is not being updated in this case
                                objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                objInvoiceItem.SerialNo = srNo += 1;
                                objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                                objInvoiceItem.TotalCostPrice = (unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId) * objInvoiceItem.Quantity);
                                //objInvoiceItem.TotalDiscPrice = (unitOfWork.ItemRspository.GetDiscCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId) * (objInvoiceItem.Unit == 0 ? (objInvoiceItem.Quantity / objInvoiceItem.ConvUnit) : objInvoiceItem.Quantity));
                                objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;
                                objInvoiceItem.IsUpdate = true;
                                objInvoiceItem.IsSynced = false;
                                objInvoiceItem.UpdateDate = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(objInvoiceItem).State = System.Data.Entity.EntityState.Modified;

                                ////// commecnted for consume stock change..
                                //StockConsumptionItem ModifiedCon = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                                //////StockConsumptionItem ModifiedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                                //ModifiedCon.User = unitOfWork.UserRepository.GetById(userId);
                                //ModifiedCon.Quantity = objInvoiceItem.Quantity;
                                //ModifiedCon.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //ModifiedCon.Comment = "Sale Transaction";
                                ////sc.Item = objInvoiceItem.Item;
                                //ModifiedCon.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //ModifiedCon.IsUpdate = true;
                                //ModifiedCon.UpdateDate = DateTime.Now;
                                //ModifiedCon.IsSynced = false;
                                //unitOfWork.GetDbContext().Entry(ModifiedCon).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        objInvoice.TotalUnitCost = objInvoice.InvoiceItems.Where(ii => ii.IsActive).Sum(r => r.TotalCostPrice);
                        if (!IsReturnedQuantityFound)
                        {
                            List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                            if (!AddInvoicePayments(NewPaymentsMade))
                            {
                                return 0;
                            }

                            objInvoice.TotalPaid = this.TotalPaid;
                            bool found = false;

                            foreach (InvoicePayment dp in DeletedPayments)
                            {
                                InvoicePayment DeletedPayment = objInvoice.InvoicePayments.Where(p => p.InvoicePaymentId == dp.InvoicePaymentId).FirstOrDefault();
                                DeletedPayment.IsUpdate = true;
                                DeletedPayment.IsSynced = false;
                                DeletedPayment.UpdatedAt = DateTime.Now;
                                DeletedPayment.IsActive = false;
                                DeletedPayment.DeletedAt = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(DeletedPayment).State = System.Data.Entity.EntityState.Modified;
                            }
                            foreach (InvoicePayment p in NewPaymentsMade)
                            {
                                found = false;
                                foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                                {
                                    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId != 0)
                                    {
                                        found = true;
                                        p2.Payment = p.Payment;
                                        p2.ChequeNumber = p.ChequeNumber;
                                        p2.BankName = p.BankName;
                                        p2.ChequeStatus = p.ChequeStatus;
                                        p2.PaymentDate = p.PaymentDate;
                                        p2.IsUpdate = true;
                                        p2.IsSynced = false;
                                        p2.UpdatedAt = DateTime.Now;
                                        p2.Dues = objInvoice.Due;
                                        p2.P_InvoiceNo = p.P_InvoiceNo;
                                        p2.P_RiderName = p.P_RiderName;
                                        p2.P_VendorName = p.P_VendorName;
                                        unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                                if (!found)
                                {
                                    objInvoice.InvoicePayments.Add(p);
                                }
                            }
                            if (balance > 0)
                            {
                                objInvoice.InvoicePayments.Last().Payment -= balance;
                            }
                        }
                        Patient newP = null;
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }

                        else
                        {
                            if (txtSearchByName.Text.Trim() != "")
                            {
                                newP = new Patient();
                                newP.MRNo = unitOfWork.PatientRepository.GetNewMrNo();
                                newP.Name = txtSearchByName.Text.Trim();
                                newP.Phone = string.IsNullOrEmpty(txtSearchByPhone.Text.Trim()) ? "0000000000" : txtSearchByPhone.Text.Trim();
                                newP.Address = txtAddress.Text.Trim();
                                newP.CreateDate = DateTime.Now;
                                newP.UpdateDate = DateTime.Now;
                                newP.RegistrationDate = DateTime.Now;
                                newP.IsActive = true;
                                newP.IsNew = true;
                                newP.Gender = "other";
                                //newP.Dues = -due;
                                if (due > 0)
                                {
                                    newP.Dues -= due;
                                }
                                else if (advance > 0)
                                {
                                    newP.Dues += advance;
                                }
                                objInvoice.Patient = newP;
                                AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + newP.MRNo, AuditModule.Patient, AuditAction.created);
                            }
                        }
                        if (!IsReturnedQuantityFound) // in else invoice due value will remain the same
                        {
                            if (due >= 0)
                            {
                                objInvoice.Due = due;
                            }
                            if (advance > 0)
                            {
                                objInvoice.Due = -advance;
                            }
                            if (balance > 0)
                            {
                                objInvoice.Due = 0;
                            }
                        }

                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        //AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + objInvoice.InvoiceRefNo, AuditModule.Invoice, AuditAction.updated);
                        unitOfWork.Save();
                        //SOT
                        //if (!objInvoice.IsHoldInvoice)
                        //{
                        //    if (newP != null && isSavingHoldInvoice)
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                        //    }
                        //    else if (newP != null && !string.IsNullOrEmpty(newP.Name) && isSavingHoldInvoice)
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice.Patient, ModuleName.Patient, ModuleAction.Create, objInvoice.Patient.PatientId);
                        //    }

                        //    if (!isSavingHoldInvoice)
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Update, objInvoice.InvoiceId);
                        //    }
                        //    else
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Create, objInvoice.InvoiceId);
                        //    }
                        //    List<StockConsumptionItem> StockConsumptionsList = objInvoice.StockConsumption.StockConsumptions;
                        //    StockConsumptionsList.ForEach(x => x.User = objInvoice.User);
                        //    if (!isSavingHoldInvoice)
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Update, 0);
                        //    }
                        //    else
                        //    {
                        //        OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Create, 0);
                        //    }
                        //}
                        if (PatientId > 0 && !IsReturnedQuantityFound)
                        {
                            double patientBal = unitOfWork.InvoiceRepository.GetInvoicesBalanceByPatientId(this.PatientId);
                            objInvoice.Patient.Dues = -patientBal;
                            if (objInvoice.Patient.IsSynced)
                            {
                                objInvoice.Patient.IsNew = false;
                                objInvoice.Patient.IsUpdate = true;
                                objInvoice.Patient.IsSynced = false;
                            }
                            unitOfWork.GetDbContext().Entry(objInvoice.Patient).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            if (!isSavingHoldInvoice)
                            {
                                MessageBox.Show("Estimate Bill Invoice Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Estimate Bill Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        return invoiceId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        public double sumofcost = 0;
        public double sumofdisc = 0;
        public double sumofdisccost = 0;
        public double sumofReve = 0;
        public double sumofreturn = 0;
        private void getsums()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                sumofcost += Convert.ToDouble(row.Cells["colCostPrice"].Value.ToString()) * Convert.ToInt32(row.Cells["colQuantity"].Value.ToString());
                sumofdisc += row.Cells["colDiscountType"].Value.ToString() != "%" ? Convert.ToDouble(row.Cells["colDiscount"].Value.ToString()) : (Convert.ToDouble(row.Cells["colDiscount"].Value.ToString()) / 100) * Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
                sumofdisccost += Convert.ToDouble(row.Cells["DiscountedPrice"].Value.ToString()) * Convert.ToInt32(row.Cells["colQuantity"].Value.ToString());
                sumofReve += Convert.ToDouble(row.Cells["colNetAmount"].Value.ToString());
            }
            if (sumofdisc == 0 && numTotalDiscount.Value > 0)
            {
                if (cmbTotalDiscountType.SelectedIndex == 0)
                {
                    sumofdisc = Convert.ToDouble(numTotalDiscount.Value);
                    sumofReve = sumofReve - sumofdisc;
                }
                else
                {
                    sumofdisc = (Convert.ToDouble(numTotalDiscount.Value) / 100) * sumofReve;
                    sumofReve = sumofReve - sumofdisc;
                }
            }
        }
        public double diffofcost = 0;
        public double diffofDisccost = 0;
        public double diffofreve = 0;
        public double diffofDisc = 0;

        private void CalculateGrossProfitStatement(long invoiceid)
        {
            getsums();
            DataTable dt = get("select sum(TransactionDiscountedCostOfSale) , sum(TransactionRevenue) ,sum(TransactionCostOfSales),sum(TransactionDiscount)  from ProfitStatementTransactions where InvoiceId=" + invoiceid);
            if (dt.Rows.Count == 1)
            {
                double re = Convert.ToDouble(dt.Rows[0][1].ToString());
                diffofcost = sumofcost - Convert.ToDouble(dt.Rows[0][2].ToString());
                diffofDisccost = sumofdisccost - Convert.ToDouble(dt.Rows[0][0].ToString());
                diffofreve = sumofReve - Convert.ToDouble(dt.Rows[0][1].ToString());
                diffofDisc = sumofdisc - Convert.ToDouble(dt.Rows[0][3].ToString());
            }

        }
        private long RefundInvoice(bool ShowSuccessMsg, int userId)
        {
            TransactionScope scope;
            //if (SharedVariables.AdminPharmacySetting.AskForPIN)
            //{
            //    try
            //    {
            //        int pin = Convert.ToInt32(Interaction.InputBox("Enter PIN for User '" + SharedVariables.LoggedInUser.UserName + "' to Save the Invoice"));
            //        User user = new User();
            //        using (unitOfWork = new UnitOfWork())
            //        {
            //            user = unitOfWork.UserRepository.IsInvalidPIN(SharedVariables.LoggedInUser.Email, pin);
            //        }
            //        if (user == null)
            //        {
            //            MessageBox.Show("Invoice save cancelled, You are not allowed to Refund invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //            return 0;
            //        }
            //        userId = user.UserId;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Invoice save cancelled, You are not allowed to Refund invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return 0;
            //    }
            //}
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        if (!CheckAndAssingStore(objInvoice))
                            return 0;
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(this.InvoiceEditId);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.ModifiedDiscount = Convert.ToDouble(numModifiedDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.Services = Convert.ToDouble(txtServices.Value);
                        objInvoice.User = unitOfWork.UserRepository.GetById(userId);//  SharedVariables.LoggedInUser;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();

                        if (objInvoice.IsSynced)
                        {
                            objInvoice.IsNew = false;
                            objInvoice.IsUpdate = true;
                            objInvoice.IsSynced = false;
                        }
                        int ItemId = 0;
                        int BatchId = 0;
                        int RtrndQty = 0;
                        int Rate = 0;
                        bool AffectStock = true;
                        int intialReturn = 0;
                        int quantity = 0;
                        double returnCost = 0;
                        string itemName = "";
                        bool affectStock = false;
                        List<ProfitStatementTransactions> objTransaction = new List<ProfitStatementTransactions>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            returnCost = 0;
                            affectStock = false;
                            ProfitStatementTransactions Transaction = new ProfitStatementTransactions();

                            intialReturn = Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value);
                            int newVal = 0;

                            itemName = r.Cells["colItemName"].Value.ToString();
                            AffectStock = Convert.ToBoolean(r.Cells["colAffectStock"].Value);
                            ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                            BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);

                            RtrndQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                            Rate = Convert.ToInt32(r.Cells["colRate"].Value);
                            quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
                            int conUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                            int unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                            int QuntiyVal = 0;

                            if (unit == 1)
                            {
                                QuntiyVal = quantity;
                            }
                            else
                            {
                                QuntiyVal = quantity * conUnit;
                            }

                            //WWW
                            if (intialReturn > 0)
                            {
                                if (RtrndQty > intialReturn) { }
                                else if (RtrndQty == intialReturn) { }
                                else
                                {
                                    newVal = RtrndQty + intialReturn;
                                    RtrndQty = newVal;
                                }
                            }

                            if (RtrndQty > quantity)
                            {
                                MessageBox.Show("Returned quantity can't be greater than sold quantity. please check for item " + itemName, "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }

                            if (newVal > quantity)
                            {
                                MessageBox.Show("Returned quantity can't be greater than sold quantity. please check for item " + itemName, "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }

                            if (RtrndQty > 0)
                            {
                                foreach (InvoiceItem i in objInvoice.InvoiceItems)
                                {
                                    if (i.Item.ItemID == ItemId && Convert.ToInt32(i.Batch.BatchId) == BatchId && i.IsActive)
                                    {
                                        i.IsUpdate = true;
                                        i.UpdateDate = DateTime.Now;
                                        i.IsSynced = false;
                                        i.ReturnedQuantity = i.Unit == 0 ? RtrndQty * i.Item.ConversionUnit : RtrndQty;
                                        //i.PerUnitCostPrice = i.TotalCostPrice / i.Quantity; //unitOfWork.ItemRspository.GetUnitCost(i.Item.ItemID, i.Batch.BatchId);
                                        //i.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(i.Item.ItemID, i.Batch.BatchId);
                                        i.PerUnitCostPrice = r.Cells["colUnit"].Value.ToString() == "Units" ? Convert.ToDouble(r.Cells["colCostPrice"].Value) : Convert.ToDouble(r.Cells["colCostPrice"].Value) / Convert.ToInt32(r.Cells["colConvUnit"].Value);
                                        returnCost = i.PerUnitCostPrice * i.ReturnedQuantity;
                                        i.TotalCostPrice = (i.PerUnitCostPrice * i.Quantity) - returnCost;
                                        i.TotalDiscPrice = ((i.Unit == 0 ? i.DiscountedPrice / i.Item.ConversionUnit : i.DiscountedPrice) * i.Quantity) - ((i.Unit == 0 ? i.DiscountedPrice / i.Item.ConversionUnit : i.DiscountedPrice) * i.ReturnedQuantity);
                                        objInvoice.TotalUnitCost -= returnCost;
                                        i.AffectStock = affectStock;
                                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                        if (RtrndQty > intialReturn)
                                        {
                                            #region Record Profit Statement Transaction
                                            if (TotalDiscountFound())
                                            {
                                                int myqty = RtrndQty - intialReturn;
                                                double discount = i.DiscountType == 1 ? i.Discount / 100 * i.Amount : i.Discount;
                                                #region Recording Profit Statement
                                                Transaction.CreatedAt = DateTime.Now;
                                                Transaction.TransactionDate = dtpRedundDate.Value;


                                                Transaction.TransactionDiscountedCostOfSale = -1 * i.DiscountedPrice * myqty;
                                                Transaction.TransactionCostOfSales = i.Unit == 1 ? (-1 * (i.PerUnitCostPrice * myqty)) : (-1 * (i.PerUnitCostPrice * i.Item.ConversionUnit * myqty));
                                                Transaction.TransactionDiscount = i.Unit == 1 ? (-1) * ((discount / i.Quantity) * myqty) : (-1) * ((discount / i.Quantity) * (myqty * i.Item.ConversionUnit));
                                                Transaction.TransactionRevenue = i.Unit == 1 ? (-1) * (myqty * i.Rate) - Transaction.TransactionDiscount : (-1) * (myqty * (i.Rate * i.Item.ConversionUnit)) - Transaction.TransactionDiscount;
                                                Transaction.TransactionReturn = i.Unit == 1 ? (myqty * i.Rate) + Transaction.TransactionDiscount : (myqty * (i.Rate * i.Item.ConversionUnit)) + Transaction.TransactionDiscount;
                                                Transaction.User = SharedVariables.LoggedInUser.UserId;
                                                Transaction.ItemId = i.Item.ItemID;
                                                Transaction.IsDeleted = false;
                                                Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                                objTransaction.Add(Transaction);
                                            }
                                            else
                                            {
                                                int SumofQty = getSumofQty();
                                                double Discount = ConvertPercToValue(Convert.ToDouble(numTotalDiscount.Value), Convert.ToDouble(lblSubTotal.Text), cmbTotalDiscountType.SelectedIndex);
                                                double PerQtyDisc = 0;

                                                int qty = RtrndQty - intialReturn;
                                                double rate = i.Unit == 1 ? i.Rate : i.Rate * i.Item.ConversionUnit;
                                                if (cmbTotalDiscountType.SelectedIndex == 1)
                                                {
                                                    PerQtyDisc = (Convert.ToDouble(numTotalDiscount.Value) / 100) * rate;
                                                }
                                                else
                                                {
                                                    PerQtyDisc = Discount / SumofQty;
                                                }
                                                Transaction.CreatedAt = DateTime.Now;
                                                Transaction.TransactionDate = dtpRedundDate.Value;
                                                Transaction.TransactionDiscountedCostOfSale = (-1) * (i.DiscountedPrice * qty);
                                                Transaction.TransactionCostOfSales = i.Unit == 1 ? (-1) * i.PerUnitCostPrice * qty : (-1) * (i.PerUnitCostPrice * i.Item.ConversionUnit) * qty;
                                                Transaction.TransactionDiscount = -1 * PerQtyDisc * qty;
                                                Transaction.TransactionRevenue = (-1) * rate * qty + (PerQtyDisc * qty);
                                                Transaction.TransactionReturn = rate * qty - (PerQtyDisc * qty);
                                                Transaction.User = SharedVariables.LoggedInUser.UserId;
                                                Transaction.ItemId = i.Item.ItemID;
                                                Transaction.IsDeleted = false;
                                                Transaction.InvoiceId = objInvoice.InvoiceRefNo;
                                                objTransaction.Add(Transaction);
                                            }
                                            #endregion

                                        }
                                        #endregion
                                    }
                                }
                                //commented for consume stock change
                                if (AffectStock) // only revert/update stock consumption if affectStock checkbox is checked..
                                {
                                    foreach (StockConsumptionItem s in objInvoice.StockConsumption.StockConsumptions)
                                    {
                                        if (s.Item.ItemID == ItemId && Convert.ToInt32(s.Batch.BatchId) == BatchId && s.IsActive)
                                        {
                                            RtrndQty = unit == 0 ? RtrndQty * s.Item.ConversionUnit : RtrndQty;
                                            int NewQty = QuntiyVal - RtrndQty; //RtrndQty - quantity; 
                                            s.Quantity = NewQty;
                                            s.UpdateDate = DateTime.Now;
                                            if (s.IsSynced)
                                            {
                                                s.IsNew = false;
                                                s.IsUpdate = true;
                                                s.IsSynced = false;
                                            }
                                        }
                                        unitOfWork.GetDbContext().Entry(s).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                            }


                        }
                        unitOfWork.ProfitStatementTransactionsRepository.InsertRange(objTransaction);
                        unitOfWork.Save();
                        InvoicePayment ExistingRefund = objInvoice.InvoicePayments.Where(p => p.Payment < 0).Select(p => p).FirstOrDefault();
                        double refund = 0;
                        double.TryParse(txtCurrentRefund.Text, out refund);
                        if (ExistingRefund != null)
                        {
                            if (refund > 0)
                            {
                                InvoicePayment p = new InvoicePayment();
                                p.PaymentType = cmbPmntType1.SelectedIndex + 1;
                                p.PaymentDate = dtpRedundDate.Value;
                                p.CreatedAt = DateTime.Now;
                                p.UpdatedAt = dtpRedundDate.Value;
                                p.IsActive = true;
                                p.IsNew = true;
                                p.Payment = -1 * refund;
                                p.MethodType = 1; // in case of return method type : 1 else : 0 =>  required for syncing purpose. not used locally
                                p.User = unitOfWork.UserRepository.GetById(userId);
                                objInvoice.InvoicePayments.Add(p);
                            }
                        }
                        else
                        {
                            double currentRefund = 0;
                            double.TryParse(txtCurrentRefund.Text, out currentRefund);
                            if (currentRefund > 0)
                            {
                                InvoicePayment p = new InvoicePayment();
                                p.PaymentType = cmbPmntType1.SelectedIndex + 1;
                                p.PaymentDate = dtpRedundDate.Value;
                                p.CreatedAt = DateTime.Now;
                                p.UpdatedAt = dtpRedundDate.Value;
                                p.IsActive = true;
                                p.IsNew = true;
                                p.Payment = -1 * currentRefund;
                                p.MethodType = 1; // in case of return method type : 1 else : 0 =>  required for syncing purpose. not used locally
                                p.User = unitOfWork.UserRepository.GetById(userId);
                                objInvoice.InvoicePayments.Add(p);

                                //ExistingRefund.MethodType = 1;
                                //ExistingRefund.Dues = objInvoice.Due;
                                //ExistingRefund.IsUpdate = true;
                                //ExistingRefund.IsSynced = false;
                                //ExistingRefund.UpdatedAt = DateTime.Now;
                                //ExistingRefund.PaymentDate = dtpRedundDate.Value;
                                //ExistingRefund.User = unitOfWork.UserRepository.GetById(userId);
                                //ExistingRefund.Payment = -1 * refund;
                            }
                        }

                        double totalRefundAmount = 0;
                        double.TryParse(txtRefundAmount.Text, out totalRefundAmount);
                        objInvoice.TotalPaid = objInvoice.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment);
                        //if (objInvoice.InvoicePayments.Where(i => i.Payment > 0).Sum(i => i.Payment) == totalRefundAmount)
                        if (objInvoice.InvoiceItems.Sum(i => i.Quantity) == objInvoice.InvoiceItems.Sum(i => i.ReturnedQuantity))
                        {
                            if (objInvoice.Patient != null)
                            {
                                objInvoice.Patient.Dues += objInvoice.Due;
                                if (objInvoice.Patient.IsSynced)
                                {
                                    objInvoice.Patient.IsNew = false;
                                    objInvoice.Patient.IsUpdate = true;
                                    objInvoice.Patient.IsSynced = false;
                                }
                            }
                            objInvoice.Due = 0;
                        }

                        //List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                        //if (!AddInvoicePayments(NewPaymentsMade))
                        //{
                        //    return 0;
                        //}
                        //bool found = false;
                        //foreach (InvoicePayment p in NewPaymentsMade)
                        //{
                        //    //found = false;
                        //    //foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                        //    //{
                        //    //    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId > 0)
                        //    //    {
                        //    //        found = true;
                        //    //        p2.Payment = p.Payment;
                        //    //        p.IsUpdate = true;
                        //    //        p.UpdateDate = DateTime.Now;
                        //    //        unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                        //    //    }
                        //    //}
                        //    //if (!found)
                        //    //{
                        //    //    objInvoice.InvoicePayments.Add(p);
                        //    //}
                        //    if (p.Payment < 0)
                        //    {
                        //        objInvoice.InvoicePayments.Add(p);
                        //    }
                        //}
                        //if (double.Parse(lblDue.Text) > 0)
                        //{
                        //    MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}

                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }
                        //foreach (DataRow r in dtStockConsumption.Rows)
                        //{
                        //    r["Invoice_InvoiceId"] = this.InvoiceEditId;
                        //}
                        //unitOfWork.StockConsumptionRespository.BulkInsert(dtStockConsumption);
                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + objInvoice.InvoiceRefNo, AuditModule.Invoice, AuditAction.refunded);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, objInvoice, ModuleName.Invoice, ModuleAction.Update, objInvoice.InvoiceId);
                        List<StockConsumptionItem> StockConsumptionsList = objInvoice.StockConsumption.StockConsumptions;
                        StockConsumptionsList.ForEach(x => x.User = objInvoice.User);
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Update, 0);
                        if (this.IsOnlineIntegrate && this.IsOnlineCancel)
                        {
                            string reason = canceltextBox.Text.ToString();
                            int j = updateOrderStatus("Update OnlineOrders set CancelReason='" + reason + "', Cancel =" + "1" + " where OrderId =" + this.OrderId);
                        }
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Refund Saved Successfully", "Refund Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtRefundAmount.Text = "0";
                            lblGrandTotal.Text = "0";
                            lblDeliveryRs.Text = "0";
                            txtCurrentRefund.Text = "0";
                            numPmnt1.Value = 0;
                            numPmnt2.Value = 0;
                            numPmnt3.Value = 0;
                            numPmnt4.Value = 0;
                            numPmnt5.Value = 0;


                        }
                        return this.InvoiceEditId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private bool AddInvoicePayments(Invoice objInvoice)
        {
            User u = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            objInvoice.InvoicePayments = new List<InvoicePayment>();

            //if (numPmnt1.Value <= 0)
            //{
            //    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            objIp.Payment = (double)numPmnt1.Value;
            objIp.PaymentDate = isInvoiceUpdated1 == true ? dtpPmntType1.Value : DateTime.Now;
            isInvoiceUpdated1 = false;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.IsQInvoice = objInvoice.IsQInvoice == true ? true : false;
            objIp.CreatedAt = DateTime.Now;
            objIp.UpdatedAt = DateTime.Now;
            objIp.User = u;
            this.TotalPaid += objIp.Payment;
            if (objIp.PaymentType == 5)//cheque selected
            {
                if (txtChequeInfoId1.Tag != null)
                {
                    ChequeInfo ci = (ChequeInfo)txtChequeInfoId1.Tag;
                    objIp.ChequeNumber = ci.ChequeNo;
                    objIp.BankName = ci.BankName;
                    objIp.ChequeStatus = ci.Status;
                }
                //if (txtChequeInfoId1.Text != "")
                //{
                //    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                //    if (chequeId > 0)
                //    {                        
                //        objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                //    }
                //}
                //else
                //{
                //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (rs != DialogResult.OK)
                //    {
                //        return false;
                //    }
                //}
            }
            else if (objIp.PaymentType == 2)
            {
                if (txtChargeInfoId1.Tag != null)
                {
                    ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                    objIp.P_InvoiceNo = ci.P_InvoiceNo;
                    objIp.P_RiderName = ci.P_RiderName;
                    objIp.P_VendorName = ci.P_Vendor;
                }
            }
            objInvoice.InvoicePayments.Add(objIp);

            if (chkPmnt2.Checked)
            {
                if (numPmnt2.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp2 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId2.Text);
                objIp2.PaymentType = cmbPmntType2.SelectedIndex + 1;
                objIp2.Payment = (double)numPmnt2.Value;
                objIp2.CreatedAt = DateTime.Now;
                objIp2.PaymentDate = dtpPmntType2.Value;
                objIp2.IsNew = true;
                objIp2.IsActive = true;
                objIp2.IsQInvoice = objInvoice.IsQInvoice == true ? true : false;
                objIp2.UpdatedAt = DateTime.Now;
                objIp2.User = u;
                this.TotalPaid += objIp2.Payment;
                if (objIp2.PaymentType == 5)//cheque selected
                {
                    if (txtChequeInfoId2.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId2.Tag;
                        objIp.ChequeNumber = ci.ChequeNo;
                        objIp.BankName = ci.BankName;
                        objIp.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId2.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId2.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp2.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId2.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                objInvoice.InvoicePayments.Add(objIp2);
            }
            if (chkPmnt3.Checked)
            {
                if (numPmnt3.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp3 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId3.Text);
                objIp3.PaymentType = cmbPmntType3.SelectedIndex + 1;
                objIp3.Payment = (double)numPmnt3.Value;
                objIp3.CreatedAt = DateTime.Now;
                objIp3.PaymentDate = dtpPmntType3.Value;
                objIp3.IsNew = true;
                objIp3.IsActive = true;
                objIp3.IsQInvoice = objInvoice.IsQInvoice == true ? true : false;
                objIp3.UpdatedAt = DateTime.Now;
                objIp3.User = u;
                this.TotalPaid += objIp3.Payment;
                if (objIp3.PaymentType == 5)//cheque selected
                {
                    if (txtChequeInfoId3.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId3.Tag;
                        objIp.ChequeNumber = ci.ChequeNo;
                        objIp.BankName = ci.BankName;
                        objIp.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId3.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId3.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp3.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId3.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                objInvoice.InvoicePayments.Add(objIp3);
            }

            if (chkPmnt4.Checked)
            {
                if (numPmnt4.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp4 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId4.Text);
                objIp4.PaymentType = cmbPmntType4.SelectedIndex + 1;
                objIp4.Payment = (double)numPmnt4.Value;
                objIp4.CreatedAt = DateTime.Now;
                objIp4.PaymentDate = dtpPmntType4.Value;
                objIp4.IsNew = true;
                objIp4.IsQInvoice = objInvoice.IsQInvoice == true ? true : false;
                objIp4.IsActive = true;
                objIp4.UpdatedAt = DateTime.Now;
                objIp4.User = u;
                this.TotalPaid += objIp4.Payment;
                if (objIp4.PaymentType == 5)//cheque selected
                {
                    if (txtChequeInfoId4.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId4.Tag;
                        objIp.ChequeNumber = ci.ChequeNo;
                        objIp.BankName = ci.BankName;
                        objIp.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId4.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId4.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp4.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId4.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                objInvoice.InvoicePayments.Add(objIp4);
            }
            if (chkPmnt5.Checked)
            {
                if (numPmnt5.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp5 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId5.Text);
                objIp5.PaymentType = cmbPmntType5.SelectedIndex + 1;
                objIp5.Payment = (double)numPmnt5.Value;
                objIp5.CreatedAt = DateTime.Now;
                objIp5.PaymentDate = dtpPmntType5.Value;
                objIp5.IsNew = true;
                objIp5.IsActive = true;
                objIp5.IsQInvoice = objInvoice.IsQInvoice == true ? true : false;
                objIp5.UpdatedAt = DateTime.Now;
                objIp5.User = u;
                this.TotalPaid += objIp5.Payment;
                if (objIp5.PaymentType == 5)//cheque selected
                {
                    if (txtChequeInfoId5.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId5.Tag;
                        objIp.ChequeNumber = ci.ChequeNo;
                        objIp.BankName = ci.BankName;
                        objIp.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId5.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId5.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp5.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId5.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                objInvoice.InvoicePayments.Add(objIp5);
            }
            return true;
        }
        private bool AddInvoicePayments(List<InvoicePayment> PaymentsList)
        {
            User u = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            //if (numPmnt1.Value <= 0)
            //{
            //    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}

            if (IsInvoiceRefund)// this is set true if invoice is loaded from return button
            {
                InvoicePayment objIp6 = new InvoicePayment();
                objIp6.PaymentType = 1;
                objIp6.RefundReason = txtRefundReason.Text.Trim();
                objIp6.Payment = txtCurrentRefund.Text == "" ? 0 : -double.Parse(txtCurrentRefund.Text);
                objIp6.PaymentDate = dtpRedundDate.Value;
                objIp6.IsNew = true;
                objIp6.IsActive = true;
                objIp6.CreatedAt = DateTime.Now;
                objIp6.UpdatedAt = DateTime.Now;
                objIp6.User = u;
                PaymentsList.Add(objIp6);
                return true;
            }
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            objIp.Payment = (double)numPmnt1.Value;
            objIp.PaymentDate = dtpPmntType1.Value;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.CreatedAt = DateTime.Now;
            objIp.UpdatedAt = DateTime.Now;
            objIp.User = u;
            this.TotalPaid += objIp.Payment;
            if (objIp.PaymentType == 5)//cheque selected
            {
                if (txtChequeInfoId1.Tag != null)
                {
                    ChequeInfo ci = (ChequeInfo)txtChequeInfoId1.Tag;
                    objIp.ChequeNumber = ci.ChequeNo;
                    objIp.BankName = ci.BankName;
                    objIp.ChequeStatus = ci.Status;
                }
                //if (txtChequeInfoId1.Text != "")
                //{
                //    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                //    if (chequeId > 0)
                //    {
                //        objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                //    }
                //}
                //else
                //{
                //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (rs != DialogResult.OK)
                //    {
                //        return false;
                //    }
                //}
            }
            else if (objIp.PaymentType == 2)
            {
                if (txtChargeInfoId1.Tag != null)
                {
                    ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                    objIp.P_InvoiceNo = ci.P_InvoiceNo;
                    objIp.P_RiderName = ci.P_RiderName;
                    objIp.P_VendorName = ci.P_Vendor;
                }
            }
            PaymentsList.Add(objIp);

            if (chkPmnt2.Checked)
            {
                if (numPmnt2.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp2 = new InvoicePayment();
                objIp2.InvoicePaymentId = int.Parse(txtInvPmntId2.Text);
                objIp2.PaymentType = cmbPmntType2.SelectedIndex + 1;
                objIp2.Payment = (double)numPmnt2.Value;
                objIp2.PaymentDate = dtpPmntType2.Value;
                objIp2.IsNew = true;
                objIp2.IsActive = true;
                objIp2.CreatedAt = DateTime.Now;
                objIp2.UpdatedAt = DateTime.Now;
                objIp2.User = u;
                this.TotalPaid += objIp2.Payment;
                if (objIp2.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId2.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId2.Tag;
                        objIp2.ChequeNumber = ci.ChequeNo;
                        objIp2.BankName = ci.BankName;
                        objIp2.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId2.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId2.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp2.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId2.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                PaymentsList.Add(objIp2);
            }

            if (chkPmnt3.Checked)
            {
                if (numPmnt3.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp3 = new InvoicePayment();
                objIp3.InvoicePaymentId = int.Parse(txtInvPmntId3.Text);
                objIp3.PaymentType = cmbPmntType3.SelectedIndex + 1;
                objIp3.Payment = (double)numPmnt3.Value;
                objIp3.PaymentDate = dtpPmntType3.Value;
                objIp3.IsNew = true;
                objIp3.IsActive = true;
                objIp3.CreatedAt = DateTime.Now;
                objIp3.UpdatedAt = DateTime.Now;
                objIp3.User = u;
                this.TotalPaid += objIp3.Payment;
                if (objIp3.PaymentType == 5)//cheque selected
                {
                    if (txtChequeInfoId3.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId3.Tag;
                        objIp3.ChequeNumber = ci.ChequeNo;
                        objIp3.BankName = ci.BankName;
                        objIp3.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId3.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId3.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp3.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId3.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                PaymentsList.Add(objIp3);
            }

            if (chkPmnt4.Checked)
            {
                if (numPmnt4.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp4 = new InvoicePayment();
                objIp4.InvoicePaymentId = int.Parse(txtInvPmntId4.Text);
                objIp4.PaymentType = cmbPmntType4.SelectedIndex + 1;
                objIp4.Payment = (double)numPmnt4.Value;
                objIp4.PaymentDate = dtpPmntType4.Value;
                objIp4.IsNew = true;
                objIp4.IsActive = true;
                objIp4.CreatedAt = DateTime.Now;
                objIp4.UpdatedAt = DateTime.Now;
                objIp4.User = u;
                this.TotalPaid += objIp4.Payment;
                if (objIp4.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId4.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId4.Tag;
                        objIp4.ChequeNumber = ci.ChequeNo;
                        objIp4.BankName = ci.BankName;
                        objIp4.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId4.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId4.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp4.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId4.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                PaymentsList.Add(objIp4);
            }
            if (chkPmnt5.Checked)
            {
                if (numPmnt5.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp5 = new InvoicePayment();
                objIp5.InvoicePaymentId = int.Parse(txtInvPmntId5.Text);
                objIp5.PaymentType = cmbPmntType5.SelectedIndex + 1;
                objIp5.Payment = (double)numPmnt5.Value;
                objIp5.PaymentDate = dtpPmntType5.Value;
                objIp5.IsNew = true;
                objIp5.IsActive = true;
                objIp5.CreatedAt = DateTime.Now;
                objIp5.UpdatedAt = DateTime.Now;
                objIp5.User = u;
                this.TotalPaid += objIp5.Payment;
                if (objIp5.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId5.Tag != null)
                    {
                        ChequeInfo ci = (ChequeInfo)txtChequeInfoId5.Tag;
                        objIp5.ChequeNumber = ci.ChequeNo;
                        objIp5.BankName = ci.BankName;
                        objIp5.ChequeStatus = ci.Status;
                    }
                    //if (txtChequeInfoId5.Text != "")
                    //{
                    //    long chequeId = Convert.ToInt64(txtChequeInfoId5.Text);
                    //    if (chequeId > 0)
                    //    {
                    //        objIp5.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    //    }
                    //}
                    //else
                    //{
                    //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (rs != DialogResult.OK)
                    //    {
                    //        return false;
                    //    }
                    //}
                }
                else if (objIp.PaymentType == 2)
                {
                    if (txtChargeInfoId5.Tag != null)
                    {
                        ChargeInfo ci = (ChargeInfo)txtChargeInfoId1.Tag;
                        objIp.P_InvoiceNo = ci.P_InvoiceNo;
                        objIp.P_RiderName = ci.P_RiderName;
                        objIp.P_VendorName = ci.P_Vendor;
                    }
                }
                PaymentsList.Add(objIp5);

            }
            return true;
        }
        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            pnlPmnt2.Visible = true;
        }
        private void chkPmnt2_CheckedChanged(object sender, EventArgs e)
        {
            //pnlPmnt2.Enabled = chkPmnt2.Checked;
            if (chkPmnt3.Checked)
            {
                chkPmnt2.Checked = true;
                return;
            }
            if (chkPmnt2.Checked)
            {
                if (numPmnt1.Value <= 0)
                {
                    if (!pnlPmnt1.Enabled)
                    {

                    }
                    else
                    {
                        chkPmnt2.Checked = false;
                        ToolTip tt = new ToolTip();
                        tt.Show("Enter Payment Here.", numPmnt1, 1000);
                        return;
                    }
                }
                chkPmnt3.Visible = true;
                pnlPmnt2.Visible = true;
            }
            else
            {
                pnlPmnt2.Visible = false;

                chkPmnt3.Checked = false;
                chkPmnt3.Visible = false;
                pnlPmnt3.Visible = false;

                chkPmnt4.Checked = false;
                chkPmnt4.Visible = false;
                pnlPmnt4.Visible = false;

                long pmntId = 0;
                long.TryParse(txtInvPmntId2.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt2.Value = 0;
                dtpPmntType2.Value = DateTime.Now;
                txtChequeInfoId2.Text = "";
                txtInvPmntId2.Text = "0";

            }
        }
        private void chkPmnt3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPmnt4.Checked)
            {
                chkPmnt3.Checked = true;
                return;
            }
            if (numPmnt2.Value <= 0)
            {
                if (!pnlPmnt2.Enabled) { }
                else
                {
                    chkPmnt3.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt3.Checked)
            {
                chkPmnt4.Visible = true;
                pnlPmnt3.Visible = true;
            }
            else
            {
                pnlPmnt3.Visible = false;
                chkPmnt4.Checked = false;
                chkPmnt4.Visible = false;
                pnlPmnt4.Visible = false;


                long pmntId = 0;
                long.TryParse(txtInvPmntId3.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt3.Value = 0;
                dtpPmntType3.Value = DateTime.Now;
                txtChequeInfoId3.Text = "";
                txtInvPmntId3.Text = "0";
            }
        }
        private void chkPmnt4_CheckedChanged(object sender, EventArgs e)
        {

            if (chkPmnt5.Checked)
            {
                chkPmnt4.Checked = true;
                return;
            }
            if (numPmnt3.Value <= 0)
            {
                if (!pnlPmnt3.Enabled) { }
                else
                {
                    chkPmnt4.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt4.Checked)
            {
                chkPmnt5.Visible = true;
                pnlPmnt4.Visible = true;
            }
            else
            {
                pnlPmnt4.Visible = false;
                chkPmnt5.Checked = false;
                chkPmnt5.Visible = false;
                pnlPmnt5.Visible = false;

                long pmntId = 0;
                long.TryParse(txtInvPmntId4.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt4.Value = 0;
                dtpPmntType4.Value = DateTime.Now;
                txtChequeInfoId4.Text = "";
                txtInvPmntId4.Text = "0";
            }
            ////////////////////////////////

        }
        private void chkPmnt5_CheckedChanged(object sender, EventArgs e)
        {
            if (numPmnt4.Value <= 0)
            {
                if (!pnlPmnt4.Enabled) { }
                else
                {
                    chkPmnt5.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt3, 1000);
                    return;
                }
            }
            pnlPmnt5.Visible = chkPmnt5.Checked;
            if (!chkPmnt5.Checked)
            {
                long pmntId = 0;
                long.TryParse(txtInvPmntId5.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt5.Value = 0;
                dtpPmntType5.Value = DateTime.Now;
                txtChequeInfoId5.Text = "";
                txtInvPmntId5.Text = "0";
            }
        }
        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            numDiscount.Value = Math.Round(numDiscount.Value, 2, MidpointRounding.AwayFromZero);
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // numDiscount.Value = 100;
                //cmbDiscountType.SelectedIndex = 1;
            }
        }
        private bool IsCorrectItemDiscount()
        {
            int discountType = cmbDiscountType.SelectedIndex + 1;
            double Disc = (double)numDiscount.Value;
            double Amount = 0;
            double.TryParse(txtAmount.Text.Trim(), out Amount);
            if (discountType == 2)
            {
                Disc = (Disc / 100) * Amount;
            }

            if (Disc > Amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void CalculateItemTotals()
        {
            double rate = (double)numRate.Value;
            int qty = (int)numQuantity.Value;
            double amount = rate * qty;
            double discount = (double)numDiscount.Value;
            string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            double Refund = 0;
            Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }
            txtAmount.Text = amount.ToString();
            txtNetAmount.Text = (amount - discount).ToString();
            //if (IsDiscountChanged)
            // {            
            //}            
            //numTotalDiscount.Value = 0;
            //numModifiedDiscount.Value = 0;
            //CalculateTotals();
        }
        private void CalculateItemTotals(DataGridViewRow r)
        {
            try
            {
                double rate = Convert.ToDouble(r.Cells["colRate"].Value);
                double SaleTax = Convert.ToDouble(r.Cells["colSaleTax"].Value);
                int qty = 0;
                if (IsInvoiceRefund)
                {
                    qty = Convert.ToInt32(r.Cells["colQuantity"].Value) - Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                }
                else
                {
                    qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                }
                double amount = rate * qty;

                double discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
                string discountType = r.Cells["colDiscountType"].Value.ToString();

                if (discountType.ToLower() == "%")
                {
                    discount = (discount / 100) * amount;
                }

                ////////////////////////// validate user allowed discount here /////////////////////////
                double AllowedDiscount = this.getUserAllowedDiscount(amount);
                if (discount > AllowedDiscount)
                {
                    if (discountType.ToLower() == "%")
                    {
                        MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + SharedVariables.LoggedInUser.DiscLimit + " % discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + AllowedDiscount + " rupees discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    r.Cells["colDiscount"].Value = 0;
                    return;
                }
                if (r.Cells["colSaleTaxType"].Value.ToString() == "%")
                {
                    SaleTax = (Convert.ToDouble(r.Cells["colSaleTax"].Value) / 100) * Convert.ToDouble(r.Cells["colAmount"].Value);
                }

                ////////////////////////////

                double NetAmount = Math.Round((amount - discount), 2, MidpointRounding.AwayFromZero);
                r.Cells["colAmount"].Value = amount;
                r.Cells["colNetAmount"].Value = (NetAmount + SaleTax).ToString();

            }
            catch (Exception)
            {
                MessageBox.Show("Entered Quantity Is Not Valid ..... ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                grdItems.SelectedRows[0].Cells["colQuantity"].Value = "1";
            }
        }

        private double getUserAllowedDiscount(double amount)
        {
            double allowedDisc = 0;
            if (SharedVariables.LoggedInUser.UserRoles.Any(rl => rl.UserRoleId == 1)) // restriction for admin
            {
                allowedDisc = 100;
            }
            else if (SharedVariables.LoggedInUser.CanGiveDiscount) //only pharmacists can give discount currently
            {
                allowedDisc = SharedVariables.LoggedInUser.DiscLimit;
            }
            allowedDisc = (allowedDisc / 100) * amount;
            return allowedDisc;
        }


        private void numRate_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();
        }
        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //numDiscount.Value = 100;
                //cmbDiscountType.SelectedIndex = 1;
            }
        }

        private ChargeInfo showChargeInfoDialogForEdit(TextBox txt)
        {
            ChargeInfo info = (ChargeInfo)txt.Tag;
            frmChargeDetails f = new frmChargeDetails(info);
            f.ShowDialog();
            ChargeInfo ChargeInfo = f.chargeinfo;
            return ChargeInfo;
        }
        private ChargeInfo showChargeInfoDialog()
        {
            frmChargeDetails f = new frmChargeDetails();
            f.ShowDialog();
            ChargeInfo ChargeInfo = f.chargeinfo;
            return ChargeInfo;
        }
        private ChequeInfo showChequeInfoDialog()
        {
            frmChequeInfo f = new frmChequeInfo();
            f.ShowDialog();
            ChequeInfo ChequeInfo = f.ChequeInfo;
            return ChequeInfo;
        }
        private void cmbPmntType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType1.SelectedIndex == 4)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId1.Tag = ChequeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1 && IsInvoiceEdit)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialogForEdit(txtChargeInfoId1);
                txtChargeInfoId1.Tag = ChargeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1)
            {

                ChargeInfo ChargeInfo = showChargeInfoDialog();
                txtChargeInfoId1.Tag = ChargeInfo;
            }
            if (pmodeindex != cmbPmntType1.SelectedIndex)
            {
                btnSave.Enabled = true;
                btnSaveAndPrint.Enabled = true;
            }
            else
            {
                btnSave.Enabled = true;

                btnSaveAndPrint.Enabled = true;
            }
        }
        private void cmbPmntType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType2.SelectedIndex == 4)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId2.Tag = ChequeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1 && IsInvoiceEdit)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialogForEdit(txtChargeInfoId2);
                txtChargeInfoId2.Tag = ChargeInfo;
            }
            else if (cmbPmntType2.SelectedIndex == 1)
            {

                ChargeInfo ChargeInfo = showChargeInfoDialog();
                txtChargeInfoId2.Tag = ChargeInfo;
            }
        }
        private void cmbPmntType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType3.SelectedIndex == 4)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId3.Tag = ChequeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1 && IsInvoiceEdit)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialogForEdit(txtChargeInfoId3);
                txtChargeInfoId3.Tag = ChargeInfo;
            }
            else if (cmbPmntType3.SelectedIndex == 1)
            {

                ChargeInfo ChargeInfo = showChargeInfoDialog();
                txtChargeInfoId3.Tag = ChargeInfo;
            }
        }
        private void cmbPmntType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType4.SelectedIndex == 4)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId4.Tag = ChequeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1 && IsInvoiceEdit)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialogForEdit(txtChargeInfoId4);
                txtChargeInfoId4.Tag = ChargeInfo;
            }
            else if (cmbPmntType4.SelectedIndex == 1)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialog();
                txtChargeInfoId4.Tag = ChargeInfo;
            }
        }
        private void cmbPmntType5_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType5.SelectedIndex == 4)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId5.Tag = ChequeInfo;
            }
            else if (cmbPmntType1.SelectedIndex == 1 && IsInvoiceEdit)
            {
                ChargeInfo ChargeInfo = showChargeInfoDialogForEdit(txtChargeInfoId5);
                txtChargeInfoId5.Tag = ChargeInfo;
            }
            else if (cmbPmntType5.SelectedIndex == 1)
            {

                ChargeInfo ChargeInfo = showChargeInfoDialog();
                txtChargeInfoId5.Tag = ChargeInfo;
            }
        }


        private void RemoveSelectedRow()
        {
            DataGridViewRow r = grdItems.SelectedRows[0];
            int rIndex = r.Index;
            long InvId = Convert.ToInt64(r.Cells["colInvoiceItemID"].Value);
            //if (InvId > 0 && !IsInvoicecopy)
            //{
            //    MessageBox.Show("This item can't be deleted.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            grdItems.Rows.RemoveAt(rIndex);
        }

        private Control LastActiveControl = null;
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            
                if (keyData == Keys.F5)
                {
                    RefreshFrm();
                    return true;
                }

                if (keyData == (Keys.Alt | Keys.I) || keyData == (Keys.Shift | Keys.OemQuestion))
                {
                    this.uC_SearchItems1.Focus();
                    //cmbSelectItems.Focus();
                    return true;
                }

                // remove selected row from datagridview
                if (keyData == (Keys.Alt | Keys.R))
                {
                    if (grdItems.Rows.Count > 0)
                    {
                        if (this.ActiveControl.Name == grdItems.Name)
                        {
                            RemoveSelectedRow();
                        }
                        else
                        {
                            this.LastActiveControl = this.ActiveControl;
                            var res = MessageBox.Show("Are you sure you want remove selected row from grid.", "Please Make Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res == System.Windows.Forms.DialogResult.Yes)
                            {
                                this.ActiveControl = grdItems;
                                RemoveSelectedRow();

                                if (grdItems.Rows.Count != 0)
                                {
                                    int index = grdItems.Rows[grdItems.Rows.Count - 1].Index;
                                    grdItems.CurrentCell = grdItems.Rows[index].Cells["colQuantity"];
                                    grdItems.FirstDisplayedScrollingRowIndex = index;
                                }
                                //this.ActiveControl = this.LastActiveControl;
                            }
                        }
                    }
                    return true;
                }

                // remove selected row from datagridview
                if (keyData == (Keys.Alt | Keys.Q))
                {
                    if (grdItems.Rows.Count > 0)
                    {
                        if (this.ActiveControl.Name == grdItems.Name)
                        {
                            DataGridViewRow r = grdItems.SelectedRows[0];
                            grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                        }
                        else
                        {
                            if (grdItems.Rows.Count > 0)
                            {
                                this.ActiveControl = grdItems;
                                grdItems.Rows[0].Selected = true;

                                grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                                grdItems.CurrentCell.Selected = true;
                            }
                        }
                    }
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
                    btnClear.PerformClick();
                    return true;
                }

                if (keyData == (Keys.Alt | Keys.X))
                {
                    numTotalDiscount.Focus();
                    return true;
                }

                if (keyData == (Keys.Alt | Keys.P))
                {
                    txtSearchByName.Focus();
                    return true;
                }

                if (keyData == (Keys.Control | Keys.Shift | Keys.A))
                {
                    btnAddNewItem.PerformClick();
                    return true;
                }

                if (keyData == (Keys.Control | Keys.Shift | Keys.T))
                {
                    btnAddStock.PerformClick();
                    return true;
                }


                if (keyData == (Keys.Control | Keys.S))
                {
                    if (btnSave.Enabled)
                    {
                        btnSave.PerformClick();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Invoice can't be saved ... ");
                        return true;
                    }

                }
                if (keyData == (Keys.Control | Keys.Q))
                {
                    if (btnSave.Enabled)
                    {
                        button3.PerformClick();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Estimate Bill Invoice can't be saved ... ");
                        return true;
                    }

                }

                if (keyData == (Keys.Control | Keys.P))
                {
                    btnSaveAndPrint.PerformClick();
                    return true;
                }
                if (keyData == (Keys.Control | Keys.D))
                {
                    btnClearAll.PerformClick();
                    return true;
                }

                if (keyData == (Keys.Control | Keys.N))
                {
                    txtSearchByName.Focus();
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
                        cmbSelectBatch.Focus();
                    }
                    return true;
                }
                if (keyData == Keys.Enter)
                {
                    if (grdItems.CurrentCell != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                            {
                                uC_SearchItems1.SetFocus();
                                grdItems.CurrentCell = this.grdItems[0, 0];


                            }
                        }));
                    }
                }

                //if (keyData == Keys.RButton)
                //{
                //    if (grdItems.CurrentCell != null)
                //    {
                //        this.Invoke(new Action(() =>
                //        {
                //            int count = grdItems.ColumnCount;
                //            for (int i = grdItems.CurrentCell.ColumnIndex; i <= count; i++)
                //            {
                //                var value = grdItems.Columns[i];
                //                if (grdItems.Columns[i].ReadOnly == false && grdItems.Columns[i].Visible == true)
                //                {
                //                    grdItems.CurrentCell = this.grdItems[i, grdItems.CurrentRow.Index];
                //                    break;
                //                }
                //            }
                //        }));
                //    }
                //}


                //if (keyData == (Keys.Alt | Keys.R))
                //{
                //    {
                //        int colIndex = grdItems.Columns["colRemove"].Index;
                //        int rowIndex = grdItems.SelectedRows[0].Index;
                //        DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                //        grdItems_CellContentClick(grdItems, e);
                //        cmbSelectBatch.Focus();
                //    }
                //    return true;
                //}
                #endregion grid short cuts
                return base.ProcessCmdKey(ref msg, keyData);
            
        }

        private void Payments_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            GetCustomerTotals();
        }
        private void GetCustomerTotals()
        {
            double Receiveable = double.Parse(lblGrandTotal.Text);
            double payments = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            double due = 0;

            //if (Receiveable - payments >= 0)
            //{
            //    due = Receiveable - payments;
            //    Balance = 0;
            //}
            //else if (payments - Receiveable >= 0)
            //{
            //    Balance = payments - Receiveable;
            //    due = 0;
            //}
            if (this.IsOnlineIntegrate)
            {
                int voucher = Convert.ToInt32(label29.Text);
                double wallet = Convert.ToDouble(lblWalletAmount.Text);
                lblPaymentPrice.Text = Math.Round((Receiveable - (voucher + wallet))).ToString();
                numPmnt1.Text = lblPaymentPrice.Text;
                payments = Convert.ToDouble(numPmnt1.Value);
            }
            else
            {
                lblPaymentPrice.Text = Convert.ToString(payments);
            }
            due = Receiveable - payments;
            if (due > 0)
            {
                lblDue.Text = Math.Round(due, 2).ToString();// due.ToString();
                lblBalance.Text = "0";
                lblAdvance.Text = "0";
            }
            else// due less than zero becomes advance
            {
                lblDue.Text = "0";
                if (isBalanceActive)
                {
                    lblBalance.Text = Math.Round(-1 * due, 2, MidpointRounding.AwayFromZero).ToString();//Balance.ToString();
                }
                else
                {
                    lblAdvance.Text = Math.Round(-1 * due, 2, MidpointRounding.AwayFromZero).ToString();//Balance.ToString();
                }
            }

            if (double.Parse(lblBalance.Text) > 0)
            {
                btnAddtoAdv.Enabled = true;
            }
            else
            {
                btnAddtoAdv.Enabled = false;
            }

            if (double.Parse(lblAdvance.Text) > 0)
            {
                btnAddToBal.Enabled = true;
            }
            else
            {
                btnAddToBal.Enabled = false;
            }
            if (this.IsInvoiceRefund)
            {
                btnAddToBal.Enabled = false;
                btnAddtoAdv.Enabled = false;
            }
        }
        private void txtServices_ValueChanged(object sender, EventArgs e)
        {
            //Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            //double SubTotal = GrandTotals.Item1;
            //double GrandTotal = GrandTotals.Item2;
            //double TotalDiscount = GrandTotals.Item3;
            //double netProfit = GrandTotals.Item4;
            //GrandTotal = Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero);
            //lblGrandTotal.Text = (GrandTotal + Convert.ToDouble(txtServices.Value)).ToString();
            //GetCustomerTotals();
            getprofit();
            GetTotalsByTotalDiscountChanges();
        }

        private void txtServices_Validated(object sender, EventArgs e)
        {
            if (txtServices.Text == "")
            {
                txtServices.Text = "0.00";
            }
        }
        private void numTotalDiscount_Validated(object sender, EventArgs e)
        {
            if (numTotalDiscount.Text == "")
            {
                numTotalDiscount.Text = "0.00";
            }
        }
        private void numTotalDiscount_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            numTotalDiscount.Value = Math.Round(numTotalDiscount.Value, 2, MidpointRounding.AwayFromZero);

            //Discount disabled
            if (discountChange == false && numTotalDiscount.Value > 0 && !IsInvoicecopy)
            {
                grdItems.Columns["colDiscount"].ReadOnly = true;
                grdItems.Columns["colDiscountType"].ReadOnly = true;
            }
            else if (Convert.ToDouble(numTotalDiscount.Value) == 0.00)
            {
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colDiscountType"].ReadOnly = false;
            }
            else if (!IsInvoicecopy)
            {
                grdItems.Columns["colDiscount"].ReadOnly = false;
                grdItems.Columns["colDiscountType"].ReadOnly = false;
                numTotalDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
            }

            if (grdItems.Rows.Count <= 0)
            { return; }

            double SubTotal = 0;
            double.TryParse(lblSubTotal.Text.Trim(), out SubTotal);
            double AllowedDiscount = this.getUserAllowedDiscount(SubTotal);

            if ((double)numTotalDiscount.Value > AllowedDiscount)
            {
                if (cmbTotalDiscountType.SelectedIndex == 1)
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + SharedVariables.LoggedInUser.DiscLimit + " % discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + AllowedDiscount + " rupees discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                numTotalDiscount.Value = 0;
                return;
            }
            else
            {
                GetTotalsByTotalDiscountChanges();
            }

            //if (isCorrectTotalDiscount())
            //{
            //    //if (numTotalDiscount.Enabled)
            //    //{
            //    //}
            //    GetTotalsByTotalDiscountChanges();
            //}
        }

        private bool isCorrectTotalDiscount()
        {
            double SubTotal = double.Parse(lblSubTotal.Text);
            int discType = cmbTotalDiscountType.SelectedIndex + 1;
            double TotalDisc = (double)numTotalDiscount.Value;
            if (discType == 2) // percent
            {
                double val = (TotalDisc / 100);
                TotalDisc = val * SubTotal;
            }
            if (TotalDisc > SubTotal)
            {
                ToolTip tt = new ToolTip();
                MessageBox.Show("Incorrect total discount value", "Incorrect discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numTotalDiscount.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void cmbTotalDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (grdItems.Rows.Count <= 0)
            { return; }
            if (isCorrectTotalDiscount())
            {
                if (numTotalDiscount.Enabled)
                {
                    GetTotalsByTotalDiscountChanges();
                }
            }
        }
        private void GetTotalsByTotalDiscountChanges()
        {
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double Posservice = (double)txtServices.Value;
            double discount = (double)numTotalDiscount.Value;

            //check Overall Table
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToString(r.Cells["colDiscount"].Value) == "0.0" || Convert.ToString(r.Cells["colDiscount"].Value) == "0.00"
                            || Convert.ToString(r.Cells["colDiscount"].Value) == "0" || Convert.ToString(r.Cells["colDiscount"].Value) == "")
                {
                    discountChange = false;
                }
                else
                {
                    discountChange = true;
                    break;
                }
            }
            //foreach (DataGridViewRow row in grdItems.Rows)
            //{
            //    if (row.Cells["colSaleTaxType"].Value.ToString() == "%")
            //    {
            //        tax += (Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString()) / 100) * Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
            //    }
            //    else
            //    {
            //        tax +=(Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString()));
            //    }
            //}
            //SubTotal = SubTotal + tax;
            if (cmbTotalDiscountType.SelectedIndex == 1 && discountChange == false)
            {
                discount = (discount / 100) * SubTotal;
            }

            if (cmbPosServiceType.SelectedIndex == 1)
            {
                Posservice = (Posservice / 100) * SubTotal;
            }
            lblSubTotal.Text = (Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero)).ToString();
            if (SubTotal == 0)
            {
                lblGrandTotal.Text = (0).ToString();
                if (!this.IsInvoiceRefund && chkPmnt1.Enabled)
                {
                    numPmnt1.Value = 0;
                }
            }
            else
            {
                double services;
                if (cmbPosServiceType.SelectedIndex == 0) { services = Convert.ToDouble(txtServices.Value); }
                else { services = Convert.ToDouble(Posservice); }

                lblGrandTotal.Text = Math.Round((SubTotal - discount + services), 2, MidpointRounding.AwayFromZero).ToString();
                if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                    {
                        numPmnt1.Value = (decimal)Math.Round((SubTotal - discount + services), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            //if (discountChange) { }
            //else { lblNetProfit.Text = Math.Round((SubTotal - discount) - this.GetTotalCost(), 2).ToString(); }

            double GrandSet = 0;
            GrandSet = Convert.ToDouble(lblGrandTotal.Text);
            GrandSet = Math.Round(GrandSet, 0);
            //if (!IsInvoiceEdit && !IsInvoiceRefund)
            //{
            //    lblNetProfit.Text = Math.Round((GrandSet) - this.GetTotalCost(), 2).ToString();
            //}
            //else
            //{
            lblNetProfit.Text = getprofit().ToString();
            //}
            //lblNetProfit.Text = Math.Round((GrandTotal - discount) - this.GetTotalCost(), 2).ToString();

            TotalCost = Convert.ToDouble(lblNetProfit.Text);
            GetCustomerTotals();
        }
        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            getItemQuantityByBatches();
        }

        private void getItemQuantityByBatches()
        {
            if (cmbSelectBatch.SelectedIndex >= 0)
            {
                BatchStockVM r = BatchStockList.Where(b => b.BatchId == Convert.ToInt64(cmbSelectBatch.SelectedValue)).FirstOrDefault();
                if (r != null)
                {
                    double rate = 0;
                    int itemId = this.SelectedItemId; // Convert.ToInt32(cmbSelectItems.SelectedValue);
                    int? batchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
                    using (unitOfWork = new UnitOfWork())
                    {
                        rate = unitOfWork.ItemRspository.GetItemRate(itemId, batchId > 0 ? batchId : null);
                    }

                    int totalQty = r.TotalStock + r.AdjustedStock - r.ConsumedStock - r.ExpiredStock;
                    int conUnit = (int)numConvUnit.Value;
                    if (cmbUnit.SelectedIndex >= 0)
                    {
                        if (cmbUnit.SelectedIndex == 0)
                        {
                            totalQty = (int)Math.Floor(totalQty / (double)conUnit);
                            rate = rate * conUnit;
                        }
                    }
                    txtAvlQty.Text = totalQty.ToString();
                    numRate.Value = (decimal)rate;

                }
            }
        }

        private void getAvailableStock(DataGridViewRow r, int batchId, int itemId)
        {
            BatchStockVM vm = new BatchStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.ItemRspository.GetBatchStock(batchId,
                    itemId,
                    SharedVariables.AdminPharmacySetting.IsEnableStores ? SharedFunctions.StringToInt(cmbStores.SelectedValue) : 0,
                    SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
            }
            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                SharedFunctions.CalculateInvoiceItemQty(vm);
                int availableQty = vm.AvailableStock;
                //int availableQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                //if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                //{
                //    availableQty += vm.HoldStock;
                //}
                double rate = vm.RetailPrice;
                double unitCost = vm.CostPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                    unitCost = vm.CostPrice * convUnit;
                    availableQty = (int)Math.Floor((double)(availableQty / convUnit));
                }
                r.Cells["colRate"].Value = rate;
                r.Cells["colCostPrice"].Value = unitCost;
                r.Cells["colAvailableQty"].Value = availableQty;
            }
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
            if (f.PatientId > 0)
            {
                PatientId = f.PatientId;
                //txtSearchPatient.Text = f.PatientName;
            }
        }
        private void ResetPayments()
        {
            pnlPmnt1.Enabled = true;
            pnlPmnt2.Enabled = true;
            pnlPmnt3.Enabled = true;
            pnlPmnt4.Enabled = true;
            pnlPmnt5.Enabled = true;

            chkPmnt2.Enabled = true;
            chkPmnt3.Enabled = true;
            chkPmnt4.Enabled = true;
            chkPmnt5.Enabled = true;

            pnlPmnt2.Visible = false;
            pnlPmnt3.Visible = false;
            pnlPmnt4.Visible = false;
            pnlPmnt5.Visible = false;

            chkPmnt3.Visible = false;
            chkPmnt4.Visible = false;
            chkPmnt5.Visible = false;

            dtpPmntType1.Value = DateTime.Now;
            dtpPmntType2.Value = DateTime.Now;
            dtpPmntType3.Value = DateTime.Now;
            dtpPmntType4.Value = DateTime.Now;
            dtpPmntType5.Value = DateTime.Now;

            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;

            numPmnt1.Value = 0;
            numPmnt2.Value = 0;
            numPmnt3.Value = 0;
            numPmnt4.Value = 0;
            numPmnt5.Value = 0;

            txtChequeInfoId1.Text = "";
            txtChequeInfoId2.Text = "";
            txtChequeInfoId3.Text = "";
            txtChequeInfoId4.Text = "";
            txtChequeInfoId5.Text = "";

            txtInvPmntId1.Text = "0";
            txtInvPmntId2.Text = "0";
            txtInvPmntId3.Text = "0";
            txtInvPmntId4.Text = "0";
            txtInvPmntId5.Text = "0";

            chkPmnt5.Checked = false;
            chkPmnt4.Checked = false;
            chkPmnt3.Checked = false;
            chkPmnt2.Checked = false;
        }
        private void ClearAll(bool isAfterSave)
        {
            UnRegisterAllEvents();
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.OK;
                }
                else
                {
                    rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK)
                {
                    Clear();
                    this.AddedItemsData = new List<ItemDetailVM>();
                    isInvoiceUpdated = false;
                    isLoadingForm = false;
                    this.lblInvoiceNo.Text = "";
                    // Start:   variables relevant to invoice Hold feature
                    this.HoldingInvoiceId = 0;
                    //InvHoldPageNo = 0;
                    //ShowHoldingInvoices(this.InvHoldPageNo);
                    // End:     variables relevant to invoice Hold feature
                    IsRemovedAll = true;
                    numTotalDiscount.Value = 0;
                    grdItems.Rows.Clear();
                    IsRemovedAll = false;
                    PatientId = 0;
                    lblGrandTotal.Text = "0";
                    lblBalance.Text = "0";
                    lblAdvance.Text = "0";
                    lblDue.Text = "0";
                    lblSubTotal.Text = "0";
                    dtStockConsumption.Rows.Clear();
                    dtStockConsumption.AcceptChanges();
                    //txtSearchPatient.Text = "";
                    InvoiceEditId = 0;
                    IsInvoiceEdit = false;
                    IsInvoiceRefund = false;
                    IsDiscountByItem = false;
                    EditingRowIndex = -1;
                    IsNameSearching = false;
                    IsPhoneSearching = false;
                    if (SharedVariables.AdminPharmacySetting.POSDiscount > 0)
                    {
                        numTotalDiscount.Value = (decimal)SharedVariables.AdminPharmacySetting.POSDiscount;
                        cmbTotalDiscountType.SelectedIndex = SharedVariables.AdminPharmacySetting.POSDiscountType;
                    }
                    else
                    {
                        numTotalDiscount.Value = 0;
                        cmbTotalDiscountType.SelectedIndex = 0;
                    }
                    numDiscount.Value = 0;
                    //numModifiedDiscount.Value = 0;
                    //patient details controls clear
                    txtSearchByName.Text = "";
                    txtSearchByPhone.Text = "";
                    txtAddress.Text = "";
                    lblNetProfit.Text = "";
                    txtServices.Value = SharedVariables.AdminPharmacySetting.IsFBRUnable ? 1 : 0;

                    txtServices.Enabled = true;
                    ResetPayments();
                    if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                        cmbStores.SelectedIndex = 0;
                    pnlPatientDetails.Enabled = true;

                    //grdItems.Columns["colEdit"].Visible = true;
                    grdItems.Columns["colRemove"].Visible = true;
                    grdItems.Columns["colReturnToggle"].Visible = false;

                    IsReturnedQuantityFound = false;
                    DeletedPayments = new List<InvoicePayment>();
                    DeletedInvoiceItems = new List<InvoiceItemVM>();
                    txtInvoiceNote.Text = "";
                    LoadedInvoice_TotalPaid = 0;
                    this.TotalPaid = 0;
                }
            }
            RegisterAllEvents();
        }
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this.MdiParent);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LoadItems();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Refershing Items, Please Try Reopen Sale Form.", ex.Message, "Error");
            }
        }

        private void CalculateRefundAmount()
        {
            double totalRefundAmount = 0;
            double GrandTotal = double.Parse(lblGrandTotal.Text);
            double SubTotal = double.Parse(lblSubTotal.Text);

            double Rate = 0;
            double ItemTotal = 0;
            double ItemReturnTotal = 0;
            double ItemReturnGrandTotal = 0;
            double DiscAmt = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                int rtQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                int SaleQty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                if (rtQty <= 0)
                {
                    continue;
                }
                Rate = Convert.ToInt32(r.Cells["colRate"].Value);
                ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);
                ItemReturnTotal = rtQty * Rate;
                ItemReturnGrandTotal += ItemReturnTotal;
                DiscAmt = 0;
                if (IsDiscountByItem)
                {
                    int DiscountType = Convert.ToInt32(r.Cells["colDiscountType"].Value);
                    DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                    if (DiscountType == 2)
                    {
                        DiscAmt = (DiscAmt / 100) * ItemTotal;
                    }
                    DiscAmt = (DiscAmt / SaleQty) * rtQty;
                }
                else
                {
                    DiscAmt = GetGrandDiscount();
                    double Ratio = ItemTotal / SubTotal;
                    DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                }
                double RefundAmount = ItemReturnTotal - DiscAmt;
                totalRefundAmount += RefundAmount;
            }
            double GDisc = 0;
            GDisc = GetGrandDiscount();
            GrandTotal = SubTotal - GDisc - totalRefundAmount;
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            double CurrentRefund = totalRefundAmount - InitialTotalRefundValue;
            txtCurrentRefund.Text = Math.Round(CurrentRefund, 2, MidpointRounding.AwayFromZero).ToString();
            txtRefundAmount.Text = Math.Round(totalRefundAmount, 2, MidpointRounding.AwayFromZero).ToString();
            double due = GrandTotal - GetPaymentsSum();

            if (due < 0)
            {
                lblBalance.Text = Math.Round((-1 * due), 2, MidpointRounding.AwayFromZero).ToString();
                lblDue.Text = "0";
            }
            else
            {
                lblBalance.Text = "0";
                lblAdvance.Text = "0";
                lblDue.Text = Math.Round(due, 2, MidpointRounding.AwayFromZero).ToString();
            }
            if (double.Parse(lblBalance.Text) > 0)
            {
                btnAddtoAdv.Enabled = true;
            }
            else
            {
                btnAddtoAdv.Enabled = false;
            }
        }
        public int indexes = -1;
        private void CalculateRefundAmount2()
        {
            try
            {
                //_WOD : Without Deductions
                double GrandTotal_WOD = double.Parse(lblSubTotal.Text.ToString()) - GetGrandDiscount();

                double TotalPaid = GetPaymentsSum(); //- Convert.ToDouble(txtServices.Value);
                double totalRefundAmount = 0;
                double totalRefundAmountWP = 0;
                double GrandTotal = double.Parse(lblGrandTotal.Text);
                double SubTotal = double.Parse(lblSubTotal.Text);

                double totalCost = 0;

                double RetQtydiscount = 0;
                double prft = 0;
                double Rate = 0;
                double Cost = 0;
                double ItemTotal = 0;
                double ItemReturnTotal = 0;
                double ItemReturnTotalWP = 0; // WP : for when customer has paid some amount prior to returning
                double ItemReturnGrandTotalWP = 0; // WP : for when customer has paid some amount prior to returning
                double ItemReturnGrandTotal = 0;
                double DiscAmt = 0;
                var intialrtQty = 0;
                double totQty = getSumofQty();
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    int rtQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                    int SaleQty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                    if (rtQty <= 0)
                    {
                        continue;
                    }

                    Rate = Convert.ToDouble(r.Cells["colRate"].Value);
                    Cost = Convert.ToDouble(r.Cells["DiscountedPrice"].Value);
                    ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);
                    intialrtQty = Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value.ToString());
                    DiscAmt = 0;

                    if (IsDiscountByItem)
                    {
                        rtQty = rtQty - intialrtQty;
                        ItemReturnTotal = (rtQty) * Rate;
                        ItemReturnGrandTotal += ItemReturnTotal;
                        string DiscountType = r.Cells["colDiscountType"].Value.ToString();
                        DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                        double NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);

                        if (DiscountType == "%")
                        {
                            DiscAmt = ItemReturnTotal * (DiscAmt / 100);
                        }
                        else
                        {
                            var peritemDisc = DiscAmt / SaleQty;
                            DiscAmt = peritemDisc * (rtQty + intialrtQty);
                        }


                        //if (DiscountType == "%")
                        //{
                        //    DiscAmt = (DiscAmt / 100) * ItemTotal;
                        //}
                        if (TotalPaid > 0) // only show refund if customer have paid some amount
                        {
                            // 4 rows commecnted at 2020-07-17 1626
                            //double Ratio = ((ItemTotal - DiscAmt) / GrandTotal_WOD);
                            //double Share = (Ratio * TotalPaid) / SaleQty;
                            //ItemReturnTotalWP = Share * rtQty;
                            //ItemReturnGrandTotalWP += ItemReturnTotalWP;
                            // 2 rows added at 2020-07-17 1626          
                            double Ratio = ((NetAmount) / GrandTotal_WOD);
                            double Share = (Ratio * TotalPaid) / SaleQty;
                            ItemReturnTotalWP = Share * rtQty;
                            ItemReturnGrandTotalWP += ItemReturnTotalWP;
                        }
                        //DiscAmt = (DiscAmt / SaleQty) * rtQty;
                    }
                    else
                    {
                        ItemReturnTotal = (rtQty) * Rate;
                        ItemReturnGrandTotal += ItemReturnTotal;

                        //DiscAmt = GetGrandDiscount();
                        int DiscountType = cmbTotalDiscountType.SelectedIndex + 1;
                        DiscAmt = (double)numTotalDiscount.Value;
                        if (DiscountType == 2)
                        {
                            var peritemDiscAmount = Convert.ToInt32(r.Cells["colRate"].Value) * (DiscAmt / 100);
                            DiscAmt = peritemDiscAmount * rtQty;

                            //DiscAmt = (DiscAmt / 100) * SubTotal;
                            //double Ratio = 0;
                            //if (SubTotal != 0)
                            //{
                            //    Ratio = ItemTotal / SubTotal;
                            //}
                            double DiscByLineItem = DiscAmt;
                            //DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                            if (TotalPaid > 0) // only show refund if customer have paid some amount
                            {
                                double Ratio2 = ((ItemTotal - DiscByLineItem) / GrandTotal_WOD);
                                double Share = (Ratio2 * TotalPaid) / SaleQty;
                                ItemReturnTotalWP = Share * rtQty;
                                ItemReturnGrandTotalWP += ItemReturnTotalWP;
                            }

                        }
                        else
                        {
                            double Ratio = 0;
                            if (SubTotal != 0)
                            {
                                Ratio = ItemTotal / SubTotal;
                            }
                            double DiscByLineItem = (DiscAmt * Ratio);
                            //DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                            DiscAmt = (DiscAmt / totQty) * rtQty;
                            if (TotalPaid > 0) // only show refund if customer have paid some amount
                            {
                                double Ratio2 = ((ItemTotal - DiscByLineItem) / GrandTotal_WOD);
                                double Share = (Ratio2 * TotalPaid) / SaleQty;
                                ItemReturnTotalWP = Share * rtQty;
                                ItemReturnGrandTotalWP += ItemReturnTotalWP;
                            }

                        }
                    }
                    //double RefundAmountWP = ItemReturnTotalWP + DiscAmt;
                    double RefundAmountWP = ItemReturnTotalWP;
                    double RefundAmount = ItemReturnTotal - DiscAmt;
                    totalRefundAmount += RefundAmount;
                    totalRefundAmountWP += RefundAmountWP;
                    RetQtydiscount += DiscAmt; // this value contains value for discount share for returned item quantity.
                    //totalCost += ((SaleQty * Cost) - (rtQty * Cost));
                    totalCost += (((Rate - Cost) * rtQty) - DiscAmt);
                    //totalCost1 = (((Rate - Cost) * rtQty) - DiscAmt);

                }


                double GDisc = 0;
                GDisc = GetGrandDiscount();
                Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
                double netProfit = GrandTotals.Item4;
                double grand2 = GrandTotals.Item2;
                prft = (Math.Round((netProfit - grand2), 2));
                GrandTotal = SubTotal - GDisc - totalRefundAmount;
                lblGrandTotal.Text = (Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero) + Convert.ToDouble(txtServices.Value)).ToString();
                //lblSubTotal.Text = Convert.ToString(Math.Abs(ItemReturnTotal - SubTotal));

                double CurrentRefund = 0;
                if (totalRefundAmountWP > 0)
                {
                    totalRefundAmountWP = totalRefundAmountWP;
                    CurrentRefund = totalRefundAmountWP - InitialTotalRefundValue;
                }
                else
                {
                    CurrentRefund = totalRefundAmountWP - InitialTotalRefundValue;
                }
                txtCurrentRefund.Text = Math.Round(CurrentRefund, 2, MidpointRounding.AwayFromZero).ToString();
                txtRefundAmount.Text = Math.Round(totalRefundAmountWP, 2, MidpointRounding.AwayFromZero).ToString();
                double a = getprofit();
                lblNetProfit.Text = a.ToString();
                //lblNetProfit.Text = Math.Round((GrandTotal - totalCost1), 2).ToString();

                //lblNetProfit.Text = Math.Round((TotalCost - totalCost), 2).ToString();
                //dont calculate due in case of refund
                //double due = GrandTotal - GetPaymentsSum();

                if (a != 0)
                {
                    numModifiedDiscount.Value = (decimal)(GDisc - RetQtydiscount);
                }
                else
                {
                    numModifiedDiscount.Value = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while performing refund calculations, please try again.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private double calculateprofit1()
        {
            double prufit = 0;
            int rqty = 0;
            int inrqty = 0;
            double rate = 0;
            double cost = 0;
            double Disc = 0;
            string DiscType = "%";
            double SaleTax = 0;
            string SaleTaxType = "%";
            double Service = 0;
            string ServiceType = "%";
            double totalrate = 0;
            double GrandTotal = 0;
            int iqty = 0;
            double totalcost = 0;
            int totalqty = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                rqty = Convert.ToInt32(row.Cells["colReturnQuantity"].Value.ToString());
                inrqty = Convert.ToInt32(row.Cells["colReturnQtyInitial"].Value.ToString());
                Disc = Convert.ToDouble(row.Cells["colDiscount"].Value.ToString());
                cost = Convert.ToDouble(row.Cells["colCostPrice"].Value.ToString());
                rate = Convert.ToDouble(row.Cells["colRate"].Value.ToString());
                SaleTax = Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString());
                DiscType = row.Cells["colDiscountType"].Value.ToString();
                ServiceType = row.Cells["colSaleTaxType"].Value.ToString();
                totalqty = totalqty - rqty;
                totalrate = totalqty * rate;
                totalcost = totalqty * cost;
                iqty += inrqty;

                if (DiscType == "%")
                {
                    Disc = ((Disc / 100) * totalrate);
                }
                if (SaleTaxType == "%")
                {
                    SaleTax = ((SaleTax / 100) * totalrate);
                }
                if ((rqty - inrqty) != 0)
                {
                    GrandTotal += totalrate - Disc + SaleTax;
                    prufit = prufit + ((totalrate + SaleTax) - (totalcost + Disc));
                }
                else
                {
                    GrandTotal += 0;
                    prufit = prufit + 0;
                }
            }
            double disc2 = 0;
            Service = Convert.ToDouble(txtServices.Text);
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                disc2 += Convert.ToDouble(row.Cells["colDiscount"].Value.ToString());
            }
            if (disc2 == 0)
            {
                if ((rqty - inrqty) != 0)
                {
                    Disc = Convert.ToDouble(numTotalDiscount.Value);
                    GrandTotal = GrandTotal - Disc + SaleTax;
                    prufit = prufit + SaleTax - Disc;
                }
                else
                {
                    GrandTotal += 0;
                    prufit = prufit + 0;
                }
            }
            if (Service > 0)
            {
                GrandTotal = GrandTotal + Service;
                prufit = prufit + Service;
            }
            return prufit;
        }
        private double calculateprofit()
        {
            try
            {
                double rate = 0;
                double cost = 0;
                double discount = 0;
                double discount1 = 0;
                double discount2 = 0;
                double saletax = 0;
                double saletax1 = 0;
                double service = 0;
                double sumofnetamount = 0;
                double sumofamount = 0;
                string discounttype = "%";
                string saletaxtype = "%";
                string servicetype = "%";
                int totalqty = 0;
                int initialreturnqty = 0;
                int sumofinitialreturnqty = 0;
                int returnqty = 0;
                int remainingqty = 0;
                int sumofqty = 0;
                int sumofremainingqty = 0;
                double refundamount = 0;
                double totalrefundamount = 0;
                double grandtotal = 0;
                double subtotal = 0;
                double profit = 0;
                double amount = 0;
                service = Convert.ToInt32(txtServices.Value);
                servicetype = cmbPosServiceType.Text;
                foreach (DataGridViewRow row in grdItems.Rows)
                {

                    if (row.Cells["colQuantity"].Value == null) { row.Cells["colQuantity"].Value = 0; }
                    rate = Convert.ToDouble(row.Cells["colRate"].Value.ToString());
                    discount1 = Convert.ToDouble(row.Cells["colDiscount"].Value.ToString());
                    saletax1 = Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString());
                    totalqty = Convert.ToInt32(row.Cells["colQuantity"].Value.ToString());
                    initialreturnqty = Convert.ToInt32(row.Cells["colReturnQtyInitial"].Value.ToString());
                    amount = Convert.ToDouble(row.Cells["colAmount"].Value.ToString());
                    returnqty = Convert.ToInt32(row.Cells["colReturnQuantity"].Value.ToString());

                    if (SharedVariables.AdminPharmacySetting.DiscountedPrice)
                    {
                        cost = Convert.ToDouble(row.Cells["DiscountedPrice"].Value);
                    }
                    else
                    {
                        cost = Convert.ToDouble(row.Cells["colCostPrice"].Value);

                    }
                    discounttype = row.Cells["colDiscountType"].Value.ToString();
                    saletaxtype = row.Cells["colSaleTaxType"].Value.ToString();
                    sumofamount += (totalqty) * rate;
                    if (discounttype == "%")
                    {
                        discount1 = (discount1 / 100) * (rate * totalqty);
                        discount += discount1;
                    }
                    else
                    {
                        discount += discount1;
                    }
                    if (saletaxtype == "%")
                    {
                        saletax1 = (saletax1 / 100) * (rate * totalqty);
                        saletax += saletax1;
                    }
                    else
                    {
                        saletax += saletax1;
                    }
                    sumofnetamount += (totalqty) * rate + (saletax1) - (discount1);
                    if (discount1 > 0 || (cmbTotalDiscountType.Text != "%"))
                    {
                        refundamount += (returnqty - initialreturnqty) * rate + ((saletax1 / totalqty) * (returnqty - initialreturnqty)) - ((discount1 / totalqty) * (returnqty - initialreturnqty));
                        totalrefundamount += (returnqty) * rate + ((saletax1 / totalqty) * (returnqty)) - ((discount1 / totalqty) * (returnqty));

                    }
                    if (IsInvoiceEdit || IsInvoicecopy)
                    {
                        remainingqty = totalqty;
                    }
                    else
                    {
                        remainingqty = totalqty - returnqty;
                    }
                    sumofqty += totalqty;
                    sumofremainingqty += remainingqty;
                    sumofinitialreturnqty += initialreturnqty;




                    if (cmbTotalDiscountType.Text == "%" && returnqty > 0)
                    {
                        var peritemDiscount = rate * (Convert.ToDouble(numTotalDiscount.Value) / 100);
                        refundamount += ((returnqty - initialreturnqty) * rate) - ((returnqty - initialreturnqty) * peritemDiscount);
                        totalrefundamount += (returnqty * rate) - (returnqty * peritemDiscount);
                        //profit += (remainingqty * (rate - cost)) + (totalqty != 0 ? ((saletax1 / totalqty) * remainingqty) - (((Convert.ToDouble(numTotalDiscount.Value) / 100) / totalqty) * remainingqty) : 0);
                        profit += (remainingqty * (rate - cost)) + (totalqty != 0 ? ((saletax1 / totalqty) * remainingqty) - (peritemDiscount * remainingqty) : 0);
                    }
                    else if (cmbTotalDiscountType.Text == "%")
                    {
                        var peritemDiscount = rate * (Convert.ToDouble(numTotalDiscount.Value) / 100);

                        //if (IsInvoiceRefund)
                        //{
                        //    var disc = Convert.ToDouble(numTotalDiscount.Value);
                        //    if(cmbTotalDiscountType.Text == "%")
                        //    {
                        //        disc = disc / 100;
                        //        disc = disc * rate;
                        //    }

                        //    profit += (remainingqty * (rate - cost)) + (totalqty != 0 ? ((saletax1 / totalqty) * remainingqty) - ((disc *rate ) * remainingqty) : 0);
                        //    IsProfitcompcalculate = true;
                        //}
                        //else
                        //{
                        profit += (remainingqty * (rate - cost)) + (totalqty != 0 ? ((saletax1 / totalqty) * remainingqty) - (peritemDiscount * remainingqty) : 0);
                        //}
                    }
                    else
                    {
                        profit += (remainingqty * (rate - cost)) + (totalqty != 0 ? ((saletax1 / totalqty) * remainingqty) - ((discount1 / totalqty) * remainingqty) : 0);
                    }

                }
                if (servicetype == "%")
                {
                    service = (service / 100) * sumofamount;
                }
                if (((discount == 0.0 || discount == 0 || discount == 0.00) && (saletax == 0.0 || saletax == 0 || saletax == 0.00)) && numTotalDiscount.Value > 0)
                {
                    discount = Convert.ToDouble(numTotalDiscount.Value);
                    discount2 = Convert.ToDouble(numTotalDiscount.Value);
                    if (cmbTotalDiscountType.Text == "%")
                    {
                        if (!IsInvoiceRefund)
                        {
                            discount = (discount / 100) * sumofamount;
                        }
                        //else
                        //{
                        //    discount = (discount / 100) * (sumofamount - totalrefundamount);
                        //    discount2 = (discount2 / 100) * sumofamount;

                        //}
                    }
                    if (cmbTotalDiscountType.Text != "%")
                    {
                        if (!IsInvoiceRefund)
                        {
                            profit = profit - (discount / sumofqty) * sumofremainingqty;
                        }
                        else
                        {

                            profit = profit - (discount2 / sumofqty) * sumofremainingqty;
                        }

                    }
                    if ((sumofqty - sumofremainingqty) > 0)
                    {
                        if (sumofremainingqty != 0 && refundamount > 0)
                        {
                            if (cmbTotalDiscountType.Text != "%" || returnqty < 0)
                            {
                                refundamount = refundamount - (discount2 / sumofqty) * ((sumofqty - sumofremainingqty) - sumofinitialreturnqty);
                                totalrefundamount -= (discount2 / sumofqty) * (sumofqty - sumofremainingqty);
                            }
                        }
                        else if (refundamount > 0)
                        {
                            if (sumofremainingqty == 0)
                            {
                                refundamount = refundamount - discount2;
                            }
                            else
                            {
                                //refundamount = refundamount - (discount2 / sumofqty) * sumofremainingqty;
                            }
                            totalrefundamount -= discount2;

                        }
                        else
                        {
                            if (totalrefundamount > 0 && cmbTotalDiscountType.Text != "%")
                            {
                                //refundamount = refundamount - (discount2 / sumofqty) * sumofremainingqty;
                                totalrefundamount -= (discount2 / sumofqty) * (sumofqty - sumofremainingqty);
                            }
                        }

                    }
                    if (sumofremainingqty != 0)
                    {
                        if (!IsInvoiceRefund)
                        {
                            sumofnetamount = sumofnetamount - (discount / sumofqty) * sumofremainingqty;
                        }
                        else
                        {

                            sumofnetamount = sumofnetamount - (discount2 / 100) * sumofamount; ;

                        }

                    }
                    else
                    {
                        if (!IsInvoiceRefund)
                        {
                            sumofnetamount = sumofnetamount - discount;
                        }
                        else
                        {
                            sumofnetamount = sumofnetamount - discount2;
                        }

                    }
                }
                if (this.IsOnlineIntegrate)
                {
                    //voucher charges
                    label29.Text = voucherCharges.ToString();
                    lblWalletAmount.Text = WalletAmount.ToString();
                    if (voucherName == null)
                    {
                        label30.Text = "";
                    }
                    else
                    {
                        label30.Text = voucherName.ToString();
                    }
                    //delivery charges
                    grandtotal = sumofnetamount + service + deliveryOrderCharges;
                    lblDeliveryRs.Text = deliveryOrderCharges.ToString();
                }
                else
                {
                    grandtotal = sumofnetamount + service;
                }
                //grandtotal = sumofnetamount + service;
                subtotal = sumofamount;
                lblNetProfit.Text = profit.ToString();
                lblGrandTotal.Text = grandtotal.ToString();
                lblSubTotal.Text = subtotal.ToString();
                if (((!IsInvoiceEdit && !IsInvoiceRefund) || changeofdiscount == 1) && SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                {
                    numPmnt1.Value = Convert.ToDecimal(grandtotal);
                    changeofdiscount = 0;
                }

                txtCurrentRefund.Text = refundamount.ToString();
                txtRefundAmount.Text = totalrefundamount.ToString();
                return profit;
            }
            catch (Exception) { return 0; }

        }
        private double getprofit()
        {
            double prufit = calculateprofit();
            //int rqty = 0;
            //int inrqty = 0;
            //double rate = 0;
            //double cost = 0;
            //double Disc = 0;
            //string DiscType = "%";
            //double SaleTax = 0;
            //string SaleTaxType = "%";
            //double Service = 0;
            //string ServiceType = "%";
            //double totalrate = 0;
            //double GrandTotal = 0;
            //int iqty = 0;
            //double RefundAmount = 0;
            //double RefundTotalAmount = 0;
            //double totalcost = 0;
            //int totalqty = 0;
            //foreach (DataGridViewRow row in grdItems.Rows)
            //{

            //    rqty = Convert.ToInt32(row.Cells["colReturnQuantity"].Value);
            //    rate = Convert.ToDouble(row.Cells["colRate"].Value);
            //    Disc = Convert.ToInt32(row.Cells["colDiscount"].Value);
            //    if (SharedVariables.AdminPharmacySetting.DiscountedPrice)
            //    {
            //        cost = Convert.ToDouble(row.Cells["DiscountedPrice"].Value);
            //    }
            //    else
            //    {
            //        cost = Convert.ToDouble(row.Cells["colCostPrice"].Value);
            //    }
            //    totalqty = Convert.ToInt32(row.Cells["colQuantity"].Value);
            //    inrqty = Convert.ToInt32(row.Cells["colReturnQtyInitial"].Value);
            //    DiscType = row.Cells["colDiscountType"].Value.ToString();
            //    SaleTaxType = row.Cells["colSaleTaxType"].Value.ToString();
            //    if (DiscType == "%")
            //    {
            //        Disc = (Disc / 100) * Convert.ToDouble(row.Cells["colAmount"].Value);
            //    }
            //    else
            //    {
            //        Disc = Disc;
            //    }
            //    SaleTax = Convert.ToDouble(row.Cells["colSaleTax"].Value.ToString());
            //    if (SaleTaxType == "%")
            //    {
            //        SaleTax = (SaleTax / 100) * Convert.ToDouble(row.Cells["colAmount"].Value);
            //    }
            //    else
            //    {
            //        SaleTax = SaleTax;
            //    }
            //    if (!IsInvoiceEdit && !IsInvoicecopy)
            //    {
            //        totalqty = totalqty - rqty;
            //    }
            //    iqty += inrqty;
            //    totalrate = rate * totalqty;
            //    totalcost = cost * totalqty;
            //    if (totalrate > 0)
            //    {
            //        if (!IsInvoiceRefund)
            //        {
            //            GrandTotal += (totalrate + SaleTax) - Disc;
            //            prufit = prufit + ((totalrate + SaleTax) - (totalcost + Disc));
            //        }
            //        else if(rqty >0)
            //        {
            //            GrandTotal += (totalrate + SaleTax) - ((rqty == totalqty) ? Disc/2 : Disc/(rqty+inrqty+totalqty));
            //            prufit = prufit + ((totalrate + SaleTax) - (totalcost)) - ((rqty == totalqty) ? Disc/2 : Disc/(rqty+inrqty+totalqty));
            //        }
            //        else
            //        {
            //            GrandTotal += (totalrate + SaleTax) - Disc;
            //            prufit = prufit + ((totalrate + SaleTax) - (totalcost + Disc));
            //        }
            //    }
            //    if (IsInvoiceRefund)
            //    {
            //        if (rqty > 0)
            //        {
            //            //if ((rqty - inrqty) != 0)
            //            {
            //                RefundAmount += ((rate * (rqty - inrqty)) - Disc) + SaleTax;
            //                RefundTotalAmount += ((rate * rqty) - Disc) + SaleTax;
            //            }
            //            //else
            //            //{
            //            //  RefundAmount += ((rate * (rqty - inrqty))) + SaleTax;
            //            //RefundTotalAmount += ((rate * rqty)) + SaleTax;
            //            //}


            //        }
            //        else
            //        {
            //            RefundTotalAmount += 0;
            //            RefundAmount += 0;
            //        }
            //    }


            //}
            //double dicsou = Disc;
            //foreach (DataGridViewRow row in grdItems.Rows)
            //{
            //    dicsou += Convert.ToDouble(row.Cells["colDiscount"].Value.ToString());
            //}
            //if (GrandTotal < 0)
            //{
            //    GrandTotal = 0;

            //    prufit = 0;
            //}
            //if (dicsou == 0)
            //{
            //    if (cmbTotalDiscountType.SelectedIndex == 1)
            //    {
            //        Disc = Convert.ToDouble(numTotalDiscount.Value);
            //        Disc = (Disc / 100) * Convert.ToDouble(GrandTotal);
            //    }
            //    else
            //    {
            //        Disc = Convert.ToDouble(numTotalDiscount.Value);
            //    }

            //    GrandTotal = GrandTotal - Disc;
            //    prufit = prufit - Disc;
            //}
            //else
            //{
            //    numTotalDiscount.Enabled = false;
            //    cmbDiscountType.Enabled = false;
            //}

            ////if (txtServices.Value > 0)
            //{
            //    if (cmbPosServiceType.SelectedIndex == 1)
            //    {
            //        Service = Convert.ToDouble(txtServices.Value);
            //        Service = (Service / 100) * Convert.ToDouble(GrandTotal);
            //    }
            //    else
            //    {
            //        Service = Convert.ToDouble(txtServices.Value);
            //    }
            //    GrandTotal = GrandTotal + Service;
            //    prufit = prufit + Service;
            //    servicecheck = 1;
            //}

            //lblGrandTotal.Text = GrandTotal.ToString();
            //txtCurrentRefund.Text = RefundAmount.ToString();
            //txtRefundAmount.Text = RefundTotalAmount.ToString();
            //numPmnt1.Value = Convert.ToDecimal(GrandTotal);
            return prufit;


        }
        private double GetPaymentsSum()
        {
            double TotalPaid = 0;
            TotalPaid = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            return TotalPaid;
        }
        public int servicecheck = 0;
        private double GetGrandDiscount()
        {
            double DiscAmt = 0;
            double SubTotal = double.Parse(lblSubTotal.Text);
            int DiscountType = cmbTotalDiscountType.SelectedIndex + 1;
            DiscAmt = (double)numTotalDiscount.Value;
            if (DiscountType == 2)
            {
                DiscAmt = (DiscAmt / 100) * SubTotal;
            }
            return DiscAmt;
        }
        private void numPmnt5_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            GetCustomerTotals();
        }
        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            //Total Discount Enable
            discountChange = false;
            numTotalDiscount.Controls[0].Visible = true;
            grdItems.Columns["colDiscount"].ReadOnly = false;

            double Balance = 0;
            double Advance = 0;
            if (!string.IsNullOrEmpty(lblBalance.Text))
                double.TryParse(lblBalance.Text, out Balance);
            if (!string.IsNullOrEmpty(lblAdvance.Text))
                double.TryParse(lblAdvance.Text, out Advance);

            long invoiceid = SaveInvoice(false, false, 0);
            if (invoiceid > 0)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(invoiceid);
                //v.Show();
                Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(invoiceid, true, false, Balance, Advance);
                v.Print();
            }
        }

        private void cmbSelectItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        string duep = "0";
        string cashback = "0";
        string advance = "0";
        private void lblDue_TextChanged(object sender, EventArgs e)
        {
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                if ((lblDue.Text != duep && !(Convert.ToDouble(txtCurrentRefund.Text) > 0) && lblDue.Text != "0") || (btnSave.Enabled && lblDue.Text != duep) || lblGrandTotal.Text != grandtotal)
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            //if(double.Parse(lblDue.Text) == 0)
            //{
            //    pnlDue.Visible = false;
            //}
            //else
            //{
            //    pnlDue.Visible = true;
            //}
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Stock"))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void numQuantity_Enter(object sender, EventArgs e)
        {
            numQuantity.Select(0, numQuantity.Value.ToString().Length);
        }

        private void numRate_Enter(object sender, EventArgs e)
        {
            numRate.Select(0, numRate.Value.ToString().Length);
        }

        private void numDiscount_Enter(object sender, EventArgs e)
        {
            numDiscount.Select(0, numDiscount.Value.ToString().Length);
        }

        private void numTotalDiscount_Enter(object sender, EventArgs e)
        {
            numTotalDiscount.Select(0, numTotalDiscount.Value.ToString().Length);
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmInvoiceShortCusts f = new ShortCutDialogs.frmInvoiceShortCusts();
            f.ShowDialog();
        }

        private void numTotalDiscount_Leave(object sender, EventArgs e)
        {
            // stop user from leaving if total discount amount goes greater than sub-total.
            //if (!isCorrectTotalDiscount())
            //{
            //    numTotalDiscount.Focus();
            //}
        }

        private void pnlLoadedInvoiceData_Paint(object sender, PaintEventArgs e)
        {
        }

        private void numRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //btnAddItem.PerformClick();
            }
        }

        private void numDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbDiscountType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbSelectItems_TextUpdate(object sender, EventArgs e)
        {
            //PageNoProds = 1;
            //prodsTimer.Start();
            //ItemsGlobal = new List<Item>();
            //SearchItems();
        }

        #region search patients
        #region search patient phone
        private void txtSearchByPhone_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
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
        private void txtSearchByPhone_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, txtSearchByPhone.Location.Y + txtSearchByPhone.Height);
        }
        private void txtSearchByPhone_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
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
        #endregion
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
                    //grdSuggestion.Rows[0].Selected = false;
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
        long pid = 0;
        private void LoadSearcherPatient()
        {
            try
            {
                if (IsInvoiceEdit)
                {
                    if (pid == this.PatientId)
                    {
                        btnSave.Enabled = true;
                        btnSaveAndPrint.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSaveAndPrint.Enabled = true;
                    }
                }

                PatientSearchTimer.Stop();
                if (grdSuggestion.SelectedRows.Count > 0)
                {
                    txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                    txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
                    txtAddress.TextChanged -= txtAddress_TextChanged;
                    this.PatientId = SharedFunctions.StringToInt32(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                    pid = this.PatientId;
                    this.txtSearchByName.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colPatient"].Value);
                    this.txtSearchByPhone.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colPhone"].Value);
                    this.txtAddress.Text = SharedFunctions.ObjectToString(grdSuggestion.SelectedRows[0].Cells["colAddress"].Value);
                    grdSuggestion.Visible = false;
                    txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                    txtSearchByName.TextChanged += txtSearchByName_TextChanged;
                    txtAddress.TextChanged += txtAddress_TextChanged;
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }
        private void PatientSearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pageNoPatients += 1;
            string searchString = txtSearchByName.Text.ToLower();
            searchPatient();
        }
        private void txtSearchByName_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByPhone.Location.Y + txtSearchByName.Height);
        }
        private void txtSearchByName_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
        }
        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
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
        private void grdSuggestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearcherPatient();
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
                TargetText.Text += e.KeyChar;
                TargetText.SelectionStart = txtSearchByName.Text.Length;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                TargetText.Focus();
                TargetText.SelectionStart = txtSearchByName.Text.Length;
                //txtSearchByName.Text = txtSearchByName.Text.Substring(txtSearchByName.Text.Length, 1);
                //txtSearchByName.SelectionStart = 0;
                //txtSearchByName.SelectionLength = 1;
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
        #endregion search patients
        private void frmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.PatientSearchTimer.Stop();
        }

        private void uC_SearchItems1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                cmbSelectBatch.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            getItemQuantityByBatches();
        }

        private void btnAddtoAdv_Click(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            lblAdvance.Text = lblBalance.Text;
            lblBalance.Text = "0";
            btnAddtoAdv.Enabled = false;
            btnAddToBal.Enabled = true;
            isBalanceActive = false;
        }

        private void btnAddToBal_Click(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            lblBalance.Text = lblAdvance.Text;
            lblAdvance.Text = "0";
            btnAddToBal.Enabled = false;
            btnAddtoAdv.Enabled = true;
            isBalanceActive = true;
        }

        private void cmbTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbTemplates.SelectedValue) > 0)
            {

            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            long invId = 0;
            if (this.HoldingInvoiceId > 0)
            {
                invId = SaveInvoice(false, true, this.HoldingInvoiceId);
            }
            else
            {
                invId = SaveInvoice(false, true, 0);
            }
            if (invId > 0)
            {
                this.InvHoldPageNo = 0;
                ShowHoldingInvoices(this.InvHoldPageNo);
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo = 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo -= 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo += 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            if (this.holdingInvoices == null)
            {
                int totalHold = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    totalHold = unitOfWork.InvoiceRepository.GetHoldingInvoicesCount();
                }
                this.InvHoldPageNo = totalHold;
            }
            else
            {
                this.InvHoldPageNo = holdingInvoices.PageCount;
            }
            ShowHoldingInvoices(this.InvHoldPageNo);
        }
        private void clearall()
        {
            try
            {
                if (this.HoldingInvoiceId > 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice obj = unitOfWork.InvoiceRepository.GetInvoiceToDelete(this.HoldingInvoiceId);
                        obj.IsActive = false;
                        foreach (InvoiceItem i in obj.InvoiceItems)
                        {
                            i.IsActive = false;
                            unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                        }
                        obj.StockConsumption.IsActive = false;
                        unitOfWork.GetDbContext().Entry(obj.StockConsumption).State = System.Data.Entity.EntityState.Modified;
                        foreach (StockConsumptionItem si in obj.StockConsumption.StockConsumptions)
                        {
                            si.IsActive = false;
                            unitOfWork.GetDbContext().Entry(si).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (InvoicePayment ip in obj.InvoicePayments)
                        {
                            ip.IsActive = false;
                            unitOfWork.GetDbContext().Entry(ip).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.InvoiceRepository.Update(obj);
                        unitOfWork.Save();
                    }
                    MessageBox.Show("Invoice deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAll(true);
                    this.InvHoldPageNo = 0;
                    ShowHoldingInvoices(this.InvHoldPageNo);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while cancelling holding invoice.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelHold_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.HoldingInvoiceId > 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice obj = unitOfWork.InvoiceRepository.GetInvoiceToDelete(this.HoldingInvoiceId);
                        obj.IsActive = false;
                        foreach (InvoiceItem i in obj.InvoiceItems)
                        {
                            i.IsActive = false;
                            unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                        }
                        obj.StockConsumption.IsActive = false;
                        unitOfWork.GetDbContext().Entry(obj.StockConsumption).State = System.Data.Entity.EntityState.Modified;
                        foreach (StockConsumptionItem si in obj.StockConsumption.StockConsumptions)
                        {
                            si.IsActive = false;
                            unitOfWork.GetDbContext().Entry(si).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (InvoicePayment ip in obj.InvoicePayments)
                        {
                            ip.IsActive = false;
                            unitOfWork.GetDbContext().Entry(ip).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.InvoiceRepository.Update(obj);
                        unitOfWork.Save();
                    }
                    MessageBox.Show("Invoice deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAll(true);
                    this.InvHoldPageNo = 0;
                    ShowHoldingInvoices(this.InvHoldPageNo);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while cancelling holding invoice.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAvlQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNextInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            int invoiceNo = 0;
            int.TryParse(lblInvoiceNo.Text, out invoiceNo);
            if (invoiceNo >= 0)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (invoiceNo == 0)
                    {
                        id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(100001); // sttart from first invoice number.. 100001 is programmatically generated first invoice by default when first invoice is created..
                    }
                    else
                    {
                        id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(invoiceNo + 1);
                    }
                }
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);
            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnPreviousInvoice.Enabled = true;
        }

        private void btnLastInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            using (unitOfWork = new UnitOfWork())
            {
                id = unitOfWork.InvoiceRepository.getMaxInvoiceId();
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);

            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnNextInvoice.Enabled = false;
            btnPreviousInvoice.Enabled = true;
        }

        private void brnPreviousInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            int invoiceNo = 0;
            int.TryParse(lblInvoiceNo.Text, out invoiceNo);
            if (invoiceNo > 0)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(invoiceNo - 1);
                }
            }
            if (this.InvoiceEditId > 0)
            {
                if (this.isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);
            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnNextInvoice.Enabled = true;
        }

        private void btn1stInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;

            int id = 0;
            using (unitOfWork = new UnitOfWork())
            {
                id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(100001);
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);

            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnPreviousInvoice.Enabled = false;
            btnNext.Enabled = true;
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {


            frmSearchItem f = new frmSearchItem
            (
                SharedVariables.AdminPharmacySetting.IsEnableStores == true
                && SharedFunctions.StringToInt(cmbStores.SelectedValue) > 0 ?
                SharedFunctions.StringToInt(cmbStores.SelectedValue) : -1
               , false, true
            );
            getprofit();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();

        }
        public string Profit(double RetailPrice, double UnitPrice)
        {
            string profit = "";
            try
            {
                double result = (((RetailPrice - UnitPrice) / RetailPrice) * 100);
                result = Math.Round(result, 2);
                if (double.IsInfinity(result))
                    return profit;
                profit = double.IsNaN(result) ? "" : result.ToString();
            }
            catch (Exception)
            {
            }
            return profit;
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            try
            {


                if (ItemAlreadyAdded(ItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                {
                    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.isNewItemAdded = true;
                Double UnitCost = 0;
                Double DiscountedCheck = 0;
                Double DiscountedPrice = 0;
                string batchname = "";
                Double Rate = 0;
                int unit = 0;
                int btch = 0;
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + ItemId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
                if (dt.Rows.Count > 0)
                {
                    Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                    UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                    Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                    int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                    unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                    unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                    if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                    {
                        btch = Convert.ToInt32(dt.Rows[0]["Batch_BatchId"].ToString());
                    }
                    else
                    {
                        btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Batch_BatchId"].ToString());
                    }
                    DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                    batchname = batch.Rows[0][0].ToString();
                    int qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                    Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                    if (DiscountType == 0)
                    {
                        Discount = ((Discount / 100) * UnitCost);
                        if (unit == 0)
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedPrice = Math.Round(((UnitCost - Discount) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                                //DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                            }
                            else
                            {
                                DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);
                            }
                        }
                        else
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                DiscountedPrice = Math.Round((((UnitCost)) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                            }
                            else
                            {
                                DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                DiscountedPrice = Math.Round((UnitCost) - Discount, 2);
                            }
                        }
                    }
                    else
                    {
                        if (unit == 0)
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                }
                            }
                            else
                            {
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) - Discount / qty), 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);

                                }
                            }
                        }
                        else
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                }
                            }
                            else
                            {
                                DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round((UnitCost) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round((UnitCost) - Discount, 2);

                                }
                            }
                        }
                    }

                }
                grdItems.Rows.Add(
                            0, //SrNo,
                            0,//invoiceitemid
                            ItemId,//itemid
                            "",//itemname
                            "", //PCT Code
                            batchname,//batch
                            btch,//btachid
                            LoadItemRackName(this.SelectedItemId), //"",// rackno
                            unit,//unit
                            1,//conv unit
                            0,//DiscountedPrice
                            DiscountedPrice,//unitcot
                            0,//retail price
                            Profit(Rate, DiscountedPrice), //ProfitMargin
                            0,//available qty
                            Quantity,
                            0, //bonus qty
                            0,//amount
                            0,//net amount
                            0,//discount
                            "%", //discount type
                            0,
                            "%",
                            true,//is new row
                            0,//return
                            0,//initial return
                            false,// affect stock
                            false // isoptionalbatchno.
                            );

                int index = grdItems.Rows.Count - 1;
                grdItems.FirstDisplayedScrollingRowIndex = index;
                btnClear.PerformClick();

                grdItems.ClearSelection();
                grdItems.Rows[index].Selected = true;
                getprofit();
                grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //MessageBox.Show("item loaded : " + ItemId);
            }
            catch (Exception)
            {

            }
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity, int Batchid)
        {
            try
            {


                if (ItemAlreadyAdded(ItemId) && !SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                {
                    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.isNewItemAdded = true;
                Double UnitCost = 0;
                Double DiscountedCheck = 0;
                Double DiscountedPrice = 0;
                string batchname = "";
                Double Rate = 0;
                int unit = 0;
                int btch = 0;
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice,Unit,Batch_BatchId  from StockItems where Item_ItemID=" + ItemId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
                if (dt.Rows.Count > 0)
                {
                    Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                    UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                    Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                    int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                    unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                    unit = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Unit"].ToString());
                    if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                    {

                        btch = Convert.ToInt32(dt.Rows[1]["Batch_BatchId"].ToString());
                    }
                    else
                    {
                        btch = Convert.ToInt32(dt.Rows[dt.Rows.Count - 2]["Batch_BatchId"].ToString());
                    }
                    if (newbatchid != 0)
                    {
                        btch = newbatchid;
                    }
                    DataTable batch = get("Select BatchName from Batches where BatchId=" + btch);
                    batchname = batch.Rows[0][0].ToString();
                    int qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                    Rate = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                    if (DiscountType == 0)
                    {
                        Discount = ((Discount / 100) * UnitCost);
                        if (unit == 0)
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedPrice = Math.Round(((UnitCost - Discount) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                                //DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                            }
                            else
                            {
                                DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);
                            }
                        }
                        else
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                DiscountedPrice = Math.Round((((UnitCost)) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);
                            }
                            else
                            {
                                DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                DiscountedPrice = Math.Round((UnitCost) - Discount, 2);
                            }
                        }
                    }
                    else
                    {
                        if (unit == 0)
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                }
                            }
                            else
                            {
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) - Discount / qty), 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) - Discount), 2);

                                }
                            }
                        }
                        else
                        {
                            if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                            {
                                Rate = Rate * Convert.ToInt32(dt1.Rows[0][0].ToString());
                                DiscountedCheck = Math.Round(((UnitCost - Discount)), 2);
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round(((UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount, 2);

                                }
                            }
                            else
                            {
                                DiscountedCheck = Math.Round((((UnitCost) / Convert.ToInt32(dt1.Rows[0][0].ToString()))) - Discount, 2);
                                if (DiscountType != 0)
                                {
                                    DiscountedPrice = Math.Round((UnitCost) - Discount / qty, 2);
                                }
                                else
                                {
                                    DiscountedPrice = Math.Round((UnitCost) - Discount, 2);

                                }
                            }
                        }
                    }

                }
                grdItems.Rows.Add(
                            0, //SrNo,
                            0,//invoiceitemid
                            ItemId,//itemid
                            "",//itemname
                            "", //PCT Code
                            batchname,//batch
                            btch,//btachid
                            LoadItemRackName(this.SelectedItemId), //"",// rackno
                            unit,//unit
                            1,//conv unit
                            0,//DiscountedPrice
                            DiscountedPrice,//unitcot
                            0,//retail price
                            Profit(Rate, DiscountedPrice), //ProfitMargin
                            0,//available qty
                            Quantity,
                            0, //bonus qty
                            0,//amount
                            0,//net amount
                            0,//discount
                            "%", //discount type
                            0,
                            "%",
                            true,//is new row
                            0,//return
                            0,//initial return
                            false,// affect stock
                            false // isoptionalbatchno.
                            );

                int index = grdItems.Rows.Count - 1;
                grdItems.FirstDisplayedScrollingRowIndex = index;
                btnClear.PerformClick();

                grdItems.ClearSelection();
                grdItems.Rows[index].Selected = true;
                getprofit();
                grdItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //MessageBox.Show("item loaded : " + ItemId);
            }
            catch (Exception)
            {

            }
        }

        private void dtpPmntType1_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
            isInvoiceUpdated1 = true;
        }

        private void dtpPmntType2_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
            isInvoiceUpdated1 = true;
        }

        private void dtpPmntType3_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
            isInvoiceUpdated1 = true;
        }

        private void dtpPmntType4_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
            isInvoiceUpdated1 = true;
        }

        private void dtpPmntType5_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
            isInvoiceUpdated1 = true;
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
        }

        private void txtInvoiceNote_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
        }



        #region ReLoadFrm
        private void RefreshFrm()
        {
            try
            {
                using (new CursorWait())
                {
                    if (this.PatientId > 0)
                    {
                        long PatientIdTemp = this.PatientId;
                        ClearAll(true);
                        this.PatientId = PatientIdTemp;
                        DeletedInvoiceItems = new List<InvoiceItemVM>();
                        //this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                        //grdItems.CellValueChanged += grdItems_CellValueChanged;
                        LoadFormData();
                    }
                    else if (this.InvoiceEditId > 0)
                    {
                        long EditInvoiceIdTemp = this.InvoiceEditId;
                        bool IsInvoiceEditTemp = this.IsInvoiceEdit;
                        bool IsInvoiceRefundTemp = this.IsInvoiceRefund;
                        ClearAll(true);
                        IsRefreshFrm = true;
                        this.InvoiceEditId = EditInvoiceIdTemp;
                        this.IsInvoiceEdit = IsInvoiceEditTemp;
                        this.IsInvoiceRefund = IsInvoiceRefundTemp;
                        DeletedInvoiceItems = new List<InvoiceItemVM>();
                        LoadFormData();
                        //this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                        //grdItems.CellValueChanged += grdItems_CellValueChanged;
                    }
                    else
                    {
                        ClearAll(true);
                        DeletedInvoiceItems = new List<InvoiceItemVM>();
                        PatientSearchTimer.Enabled = true;
                        PatientSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
                        PatientSearchTimer.AutoReset = true;
                        PatientSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(PatientSearchTimer_Elapsed);
                        PatientSearchTimer.Stop();
                        LoadFormData();
                        //this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                        //grdItems.CellValueChanged += grdItems_CellValueChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                IsRefreshFrm = false;
            }

        }
        #endregion

        private void cmbStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll(true);
            this.uC_SearchItems1.StoreId =
                SharedFunctions.StringToInt(cmbStores.SelectedValue) > 0 ?
                SharedFunctions.StringToInt(cmbStores.SelectedValue) : -1;
        }
        public int newbatchid = 0;

        public string statusofdiscount = "";
        double prfit = 0;
        public int changeofdiscount = 0;
        #region DataGridEvents
        bool DubItemAdded = false;
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            int returnQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value);
            if (e.RowIndex < 0) return;
            int batchId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
            int ItemId = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);

            if (e.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                int Qty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                int initRet = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colReturnQtyInitial"].Value);

                var r = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value;
                int RQty = r == null || r.ToString() == "" ? 0 : Convert.ToInt32(r);
                grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = RQty; // this to avoid funrther checks on calculateFormuale

                if (RQty < initRet)
                {
                    MessageBox.Show("Return Quantity Can't be less Than the Initial Return", "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = initRet;
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    return;
                }
                if (RQty > Qty)
                {
                    MessageBox.Show("Return Quantity Can't be Greater Than Sale Quantity", "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Qty;
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    return;
                }
                CalculateRefundAmount2();
            }
            int avQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
            int sQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colRate"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscountType"].Index || e.ColumnIndex == grdItems.Columns["colSaleTaxType"].Index)
            {
                if (e.ColumnIndex == grdItems.Columns["colRate"].Index)
                {
                    if (IsInvoiceEdit)
                    {
                        changeofdiscount = 1;
                    }
                    double cst = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value);
                    double ret = SharedFunctions.StringToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value);
                    if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale)
                    {
                        if (ret < cst && !SharedVariables.AdminInvoiceSetting.AllowBelowCostSale)
                        {
                            MessageBox.Show("Retail can't be less than cost price.", "Ivalid Retail price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value;
                        }
                    }
                }

                //Total Discount visibilty
                if (e.ColumnIndex == grdItems.Columns["colDiscount"].Index)
                {
                    int disValue = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value);
                    if (IsInvoiceEdit)
                    {
                        changeofdiscount = 1;
                    }
                    //check Overall Table
                    if (createInvoice || IsInvoiceEdit)
                    {
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            if (Convert.ToString(r.Cells["colDiscount"].Value) == "0.0" || Convert.ToString(r.Cells["colDiscount"].Value) == "0.00"
                                || Convert.ToString(r.Cells["colDiscount"].Value) == "0" || Convert.ToString(r.Cells["colDiscount"].Value) == "")
                            {
                                numTotalDiscount.Enabled = true;
                                numTotalDiscount.Controls[0].Visible = true;
                                cmbTotalDiscountType.Enabled = true;
                                discountChange = false;
                            }
                            else
                            {
                                numTotalDiscount.Enabled = false;
                                cmbTotalDiscountType.Enabled = false;
                                numTotalDiscount.Controls[0].Visible = false;
                                discountChange = true;

                                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                                CalculateTotals();
                                grdItems.CellValueChanged += grdItems_CellValueChanged;
                                return;
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == grdItems.Columns["colSaleTaxType"].Index)
                {
                    if (IsInvoiceEdit)
                    {
                        changeofdiscount = 1;
                    }
                    //grdItems.BeginEdit(true);
                    //calculateSaleTax();
                    //grdItems.EndEdit();
                    //grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
            }
            else if (e.ColumnIndex == grdItems.Columns["colSaleTax"].Index)
            {
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
                GetTotalsByTotalDiscountChanges();
                CalculateItemTotals();
                GetCustomerTotals();
                if (IsInvoiceEdit)
                {
                    changeofdiscount = 1;
                }

            }

            else if (e.ColumnIndex == grdItems.Columns["colBatch"].Index || e.ColumnIndex == grdItems.Columns["colBatchId"].Index)
            {

                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                if (e.ColumnIndex == grdItems.Columns["colBatchId"].Index) // in case of edit
                {
                    batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                }
                else
                {
                    //if (IsInvoicecopy)
                    //{
                    //    batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                    //}
                    //else
                    //{
                    //grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = batchId;
                    string check = (grdItems.Rows[e.RowIndex].Cells["colBatch"].Value).ToString();
                    if (check.Contains("["))
                    {
                        batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                    }
                    else
                    {
                        batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatch"].Value);
                    }
                    //}
                    grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId;
                }

                getAvailableStock(grdItems.Rows[e.RowIndex], batchId, itemId);
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
                Double DiscountedPrice = 0;
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice  from StockItems where Item_ItemID=" + ItemId + " and  Batch_BatchId=" + batchId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
                if (dt.Rows.Count > 0)
                {
                    string Profitmargin;

                    Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                    Double UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                    Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                    Double RetailPrice = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                    //Profitmargin = Profit(RetailPrice, UnitCost).ToString();
                    int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                    int qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                    int qty1 = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString()); /// Convert.ToInt32(dt1.Rows[0][0].ToString());

                    if (DiscountType == 0)
                    {
                        if (grdItems.SelectedRows[0].Cells["colUnit"].Value.ToString().ToLower() == "pack")
                        {
                            Discount = ((Discount / 100) * (UnitCost) * Convert.ToInt32(dt1.Rows[0][0].ToString()));
                        }
                        else
                        {
                            Discount = ((Discount / 100) * (UnitCost));

                        }
                        DiscountedPrice = Math.Round(((UnitCost * Convert.ToInt32(dt1.Rows[0][0].ToString())) - Discount), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount / qty1) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                    }

                    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Pack")
                    {
                        UnitCost = UnitCost * Convert.ToInt32(dt1.Rows[0][0].ToString());

                    }
                    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Units")
                    {
                        if (DiscountType != 0)
                        {
                            DiscountedPrice = Math.Round((UnitCost - Discount / qty), 2);
                        }
                        else
                        {
                            DiscountedPrice = Math.Round((UnitCost - Discount), 2);

                        }

                    }
                    grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = UnitCost;
                    grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = DiscountedPrice;
                    grdItems.Rows[e.RowIndex].Cells["ProfitMargin"].Value = Profit(RetailPrice, DiscountedPrice);
                    Profitmargin = Profit(Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value), Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value)).ToString();
                    grdItems.Rows[e.RowIndex].Cells["ProfitMargin"].Value = Profitmargin;
                    CalculateItemTotals();
                    Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
                    double SubTotal = GrandTotals.Item1;
                    double GrandTotal = GrandTotals.Item2;
                    double TotalDiscount = GrandTotals.Item3;
                    double netProfit = GrandTotals.Item4;
                    lblNetProfit.Text = Math.Round((GrandTotal - netProfit), 2).ToString();
                }

                //Code for Batch Here
            }

            else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                string unitSet = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                ItemDetailVM itemDetail = new ItemDetailVM();
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);

                //set select BatchList value
                using (unitOfWork = new UnitOfWork())
                {
                    if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                    {
                        itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStoreWise(ItemId, SharedFunctions.StringToInt(cmbStores.SelectedValue), SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                    }
                    else
                    {
                        itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                    }
                    BatchStockList = itemDetail.BatchStockList.ToList();
                }
                List<BatchStockVM> FinalResult = new List<BatchStockVM>();
                foreach (BatchStockVM i in BatchStockList)
                {
                    SharedFunctions.CalculateInvoiceItemQty(i);
                    //double avlStock = i.AvailableStock;
                    i.AvailableStock = unitSet.ToLower() == "pack" ? i.AvailableStock / itemDetail.Item.ConversionUnit : i.AvailableStock;
                    i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
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

                DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colBatch"];
                dgCmbBatch.DataSource = FinalResult;
                dgCmbBatch.ValueMember = "BatchId";
                dgCmbBatch.DisplayMember = "BatchName";

                int batchId1 = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                string batchName = grdItems.Rows[e.RowIndex].Cells["colBatch"].Value.ToString();
                //string batchName = FinalResult[0].BatchName;

                if (!IsInvoicecopy)
                {
                    foreach (var chk in FinalResult)
                    {
                        if (batchId1 == chk.BatchId)
                        {
                            grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId1;
                            grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = chk.BatchName;
                        }
                    }
                }

                if (IsInvoicecopy)
                {
                    foreach (var chk in FinalResult)
                    {
                        if (batchId1 == chk.BatchId)
                        {
                            grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId1;
                            grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = chk.BatchName;
                        }
                    }
                }

                //else
                //{
                //    grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = FinalResult[0].BatchName;
                //    grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = FinalResult[0].BatchId;
                //}
                int cu = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ColConvUnit"].Value);

                //int qt = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                BatchStockVM vm = this.AddedItemsData.Where(i => i.Item.ItemID == itemId).FirstOrDefault().BatchStockList.Where(b => b.BatchId == batchId).FirstOrDefault();
                vm.AvailableStock = (vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock) - vm.StockReturn - vm.BonusStock;

                if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString().Equals("Units"))
                {
                    if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = vm.AvailableStock;
                    }
                    else
                    {
                        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = vm.AvailableStock;
                    }
                    grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = vm.CostPrice;
                    grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value.ToString()) / Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value)).ToString();
                    grdItems.Rows[e.RowIndex].Cells["colRate"].Value = vm.RetailPrice;
                    statusofdiscount = "Changed";
                }
                else
                {
                    statusofdiscount = "Changed";
                    if (vm == null)
                    {
                        grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = Math.Floor((double)(0 / cu));
                        grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = 0 * cu;
                        grdItems.Rows[e.RowIndex].Cells["colRate"].Value = 0 * cu;
                        if (statusofdiscount == "Changed")
                        {
                            grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value.ToString())).ToString();
                        }
                    }
                    else
                    {
                        //vm.AvailableStock = (vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock) - vm.StockReturn - vm.BonusStock;
                        if (SharedVariables.AdminPharmacySetting.IsItemDefPackOnPOS)
                        {
                            grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = Math.Floor((double)(vm.AvailableStock / cu));
                        }
                        else
                        {
                            grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = Math.Floor((double)(vm.AvailableStock / cu));
                        }
                        grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = vm.CostPrice * cu;
                        grdItems.Rows[e.RowIndex].Cells["colRate"].Value = vm.RetailPrice * cu;
                        if (statusofdiscount == "Changed")
                        {
                            grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value.ToString())).ToString();
                        }
                    }
                }

                Double DiscountedPrice = 0;
                DataTable dt = get("Select Item_ItemID , Discount ,UnitCost,PercDiscType,TotalCost,Quantity,RetailPrice  from StockItems where Item_ItemID=" + ItemId + " and  Batch_BatchId=" + vm.BatchId);
                DataTable dt1 = get("Select ConversionUnit from Items where ItemID=" + ItemId);
                if (dt.Rows.Count > 0)
                {
                    string Profitmargin;
                    Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                    Double UnitCost = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["UnitCost"].ToString());
                    Double subTotal = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["TotalCost"].ToString());
                    Double RetailPrice = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["RetailPrice"].ToString());
                    int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                    int qty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                    if (DiscountType == 0)
                    {
                        Discount = ((Discount / 100) * UnitCost);
                        DiscountedPrice = Math.Round(((UnitCost - Discount) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount / qty) * Convert.ToInt32(dt1.Rows[0][0].ToString())), 2);
                    }

                    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Pack")
                    {
                        UnitCost = UnitCost * Convert.ToInt32(dt1.Rows[0][0].ToString());

                    }
                    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString() == "Units")
                    {
                        if (DiscountType != 0)
                        {
                            DiscountedPrice = Math.Round((UnitCost - Discount / qty), 2);
                        }
                        else
                        {
                            DiscountedPrice = Math.Round((UnitCost - Discount), 2);

                        }

                    }


                    //Profitmargin = Profit(RetailPrice, DiscountedPrice1).ToString();
                    grdItems.Rows[e.RowIndex].Cells["ProfitMargin"].Value = Profit(RetailPrice, DiscountedPrice);

                    grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = UnitCost;
                    grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value = DiscountedPrice;
                    Profitmargin = Profit(Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value), Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["DiscountedPrice"].Value)).ToString();
                    grdItems.Rows[e.RowIndex].Cells["ProfitMargin"].Value = Profitmargin;
                }

                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
                Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
                double SubTotal = GrandTotals.Item1;
                double GrandTotal = GrandTotals.Item2;
                double TotalDiscount = GrandTotals.Item3;
                double netProfit = GrandTotals.Item4;

                lblNetProfit.Text = Math.Round((GrandTotal - netProfit), 2).ToString();
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            if (e.ColumnIndex != grdItems.Columns["colReturnQuantity"].Index && e.ColumnIndex != grdItems.Columns["colReturnToggle"].Index && e.ColumnIndex != grdItems.Columns["colAffectStock"].Index)
            {
                avQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                int bQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value);
                sQty = SharedFunctions.StringToInt(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                if (sQty + bQty > avQty)
                {
                    if (!SharedVariables.AdminPharmacySetting.AllowNegCons && SharedVariables.AdminInvoiceSetting.ShowMultipleBatchPOS)
                    {
                        //SCMB
                        //if (SharedVariables.AdminPharmacySetting.IsConsumptionFromMultipleBatches)
                        //{
                        //    MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                        //    grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;

                        //int StoreId = 0;
                        //int cu = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ColConvUnit"].Value);
                        //StoreId = SharedFunctions.StringToInt(cmbStores.SelectedValue);
                        //DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colBatch"];
                        //List<BatchStockVM> batchList = (List<BatchStockVM>)dgCmbBatch.DataSource;
                        //if (batchList != null && batchList.Count() > 0)
                        //{
                        //    int totalAvailableStock = batchList.Sum(x => x.AvailableStock);
                        //    //For Unit and Pack
                        //    if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString().Equals("Units"))
                        //    {
                        //        if (sQty + bQty > totalAvailableStock)
                        //        {
                        //            MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //            grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                        //            grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (sQty + bQty > totalAvailableStock / cu)
                        //        {
                        //            MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //            grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                        //            grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                        //    grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                        //}
                        //}
                        //SCMB
                        //else
                        //{
                        //for Edit Change
                        if (IsInvoiceEdit && IsEditAllow)
                        {
                            int qty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                            int Initialqty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantityFirst"].Value);
                            int newValue = Math.Abs(qty - Initialqty);
                            if (avQty == 0)
                            {
                                if (qty > Initialqty)
                                {
                                    MessageBox.Show("Quantity should not be Greater than Available Quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = Initialqty;
                                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                                    return;
                                }
                            }
                            else
                            {
                                if (newValue > avQty)
                                {
                                    MessageBox.Show("Quantity should not be Greater than Available Quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = Initialqty;
                                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                                    return;
                                }
                            }

                            return;
                        }

                        if (avQty != 0)
                        {
                            DialogResult dr;
                            int oldBatchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value.ToString());
                            newbatchid = getbatchid(ItemId, oldBatchId);
                            if (newbatchid == 0)
                            {
                                MessageBox.Show("Quantity should not be Greater than Available Quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                                grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                                return;
                            }
                            if (DubItemAdded)
                            {
                                DubItemAdded = false;
                                return;
                            }
                            dr = MessageBox.Show("Entered Sale Quantity is Greater Than Available quantity in Current Batch , Do you want to add same Item with Different Batch Again?", "Invalid Sale Quantity", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                //MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                                grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                                int remqty = sQty - avQty;
                                DubItemAdded = true;

                                frmSearchItem_OnItemSelected(ItemId, remqty, newbatchid);
                                grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

                                return;
                            }
                            else
                            {

                                MessageBox.Show("Quantity should not be Greater than Available Quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                                grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                                return;

                            }
                            newbatchid = 0;


                        }
                        grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                        grdItems.Rows[e.RowIndex].Cells["colBonusQuantity"].Value = 0;
                    }
                    //}
                    else
                    {
                        if (avQty < Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value.ToString()) && !SharedVariables.AdminPharmacySetting.AllowNegCons)
                        {
                            MessageBox.Show("Quantity should not be Greater than Available Quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;

                        }

                        grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Red;
                        grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                    }
                }
                else if (!SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SharedVariables.RowSelectionBackColor;
                    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
            if (CheckExpItem(ItemId, batchId) && !SharedVariables.AdminPharmacySetting.DarkMode)
            {
                grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            }
            else if (!SharedVariables.AdminPharmacySetting.DarkMode)
            {
                grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SharedVariables.RowSelectionBackColor;
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            //avQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
            //sQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
            //if (sQty > avQty)
            //{
            //    MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
            //}

            getprofit();
        }
        private int getbatchid(int ItemId, int oldBatchId1)
        {
            int BatchId = 0;
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                int oldBatchId = Convert.ToInt32(row.Cells["colBatchId"].Value.ToString());
                DataTable dt = get("select Batch_BatchId from StockItems where Batch_BatchId !=" + oldBatchId + " and Batch_BatchId !=" + oldBatchId1 + " and Item_ItemID =" + ItemId + " and IsActive=1");
                if (dt.Rows.Count != 0)
                {

                    int i = 0;
                    l:
                    if (dt.Rows.Count <= i)
                    {
                        return 0;
                    }
                    BatchId = Convert.ToInt32(dt.Rows[i][0].ToString());
                    int a = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        a = unitOfWork.ItemRspository.GetItemAvailableQty1(ItemId, BatchId);

                    }
                    if (a > 0)
                    {
                        return BatchId;
                    }
                    else
                    {
                        i = i + 1;
                        goto l;
                    }
                }
                else
                {
                    BatchId = 0;
                }
            }
            return BatchId;
        }
        private bool CheckExpItem(int ItemId, int BatchId)
        {
            bool result = false;
            try
            {
                int ExpiryPeriodCount = SharedVariables.AdminPharmacySetting.ExpiryPeriod;
                if (ExpiryPeriodCount <= 0)
                    return result;
                DateTime ExpNotiPeriod = DateTime.Now;
                if (SharedVariables.AdminPharmacySetting.ExpiryPeriodUnit == (int)SharedVariables.ExpiryPeriodUnit.Days)
                {
                    ExpNotiPeriod = DateTime.Now.AddDays(ExpiryPeriodCount);
                }
                else
                {
                    ExpNotiPeriod = DateTime.Now.AddMonths(ExpiryPeriodCount);
                }
                using (unitOfWork = new UnitOfWork())
                {
                    result = unitOfWork.ItemRspository.GetExpiryPOSNotis(ExpNotiPeriod, ItemId, BatchId);
                }
            }
            catch (Exception)
            {

            }
            return result;
        }
        private void ApplySetting(InvoiceSetting setting)
        {
            if(setting == null)
            {
                try
                {
                    InvoiceSetting settings = new InvoiceSetting();
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Amount").FirstOrDefault().Index].Checked = settings.Amount;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "AvailableQty").FirstOrDefault().Index].Checked = settings.AvailableQty;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Batch").FirstOrDefault().Index].Checked = settings.Batch;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "BonusQuantity").FirstOrDefault().Index].Checked = settings.BonusQuantity;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ConvUnit").FirstOrDefault().Index].Checked = settings.ConvUnit;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "CostPrice").FirstOrDefault().Index].Checked = settings.CostPrice;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscCost").FirstOrDefault().Index].Checked = settings.DiscCost;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Discount").FirstOrDefault().Index].Checked = settings.Discount;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscountType").FirstOrDefault().Index].Checked = settings.DiscountType;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ItemName").FirstOrDefault().Index].Checked = settings.ItemName;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "NetAmount").FirstOrDefault().Index].Checked = settings.NetAmount;
                    if (InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").ToList().Count == 1)
                    {
                        InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").FirstOrDefault().Index].Checked = settings.PCTCode;
                    }
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ProfitMargin").FirstOrDefault().Index].Checked = settings.ProfitMargin;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Quantity").FirstOrDefault().Index].Checked = settings.Quantity;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "RackNo").FirstOrDefault().Index].Checked = settings.RackNo;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Rate").FirstOrDefault().Index].Checked = settings.Rate;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTax").FirstOrDefault().Index].Checked = settings.SaleTax;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTaxType").FirstOrDefault().Index].Checked = settings.SaleTaxType;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Sr").FirstOrDefault().Index].Checked = true;
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Unit").FirstOrDefault().Index].Checked = settings.Unit;

                    ApplySetting(settings);
                    setting = settings;
                }
                catch (Exception ex)
                {
                    return;
                }
                
            }
            if (!setting.Sr)
            {
                grdItems.Columns["SrNo"].Visible = false;
            }
            else
            {
                grdItems.Columns["SrNo"].Visible = true;
            }
            if (!setting.ItemName)
            {
                grdItems.Columns["colItemName"].Visible = false;
            }
            else
            {
                grdItems.Columns["colItemName"].Visible = true;
            }
            if (!setting.PCTCode)
            {
                grdItems.Columns["colPCTCode"].Visible = false;
            }
            else
            {
                grdItems.Columns["colPCTCode"].Visible = true;
            }
            if (!setting.Batch)
            {
                grdItems.Columns["colBatch"].Visible = false;
            }
            else
            {
                grdItems.Columns["colBatch"].Visible = true;
            }
            if (!setting.RackNo)
            {
                grdItems.Columns["colRackNo"].Visible = false;
            }
            else
            {
                grdItems.Columns["colRackNo"].Visible = true;
            }
            if (!setting.Unit)
            {
                grdItems.Columns["colUnit"].Visible = false;
            }
            else
            {
                grdItems.Columns["colUnit"].Visible = true;
            }
            if (!setting.ConvUnit)
            {
                grdItems.Columns["colConvUnit"].Visible = false;
            }
            else
            {
                grdItems.Columns["colConvUnit"].Visible = true;
            }
            if (!setting.CostPrice)
            {
                grdItems.Columns["colCostPrice"].Visible = false;
            }
            else
            {
                grdItems.Columns["colCostPrice"].Visible = true;
            }
            if (!setting.DiscCost)
            {
                grdItems.Columns["DiscountedPrice"].Visible = false;
            }
            else
            {
                grdItems.Columns["DiscountedPrice"].Visible = true;
            }
            if (!setting.Rate)
            {
                grdItems.Columns["colRate"].Visible = false;
            }
            else
            {
                grdItems.Columns["colRate"].Visible = true;
            }
            if (!setting.ProfitMargin)
            {
                grdItems.Columns["ProfitMargin"].Visible = false;
            }
            else
            {
                grdItems.Columns["ProfitMargin"].Visible = true;
            }
            if (!setting.AvailableQty)
            {
                grdItems.Columns["colAvailableQty"].Visible = false;
            }
            else
            {
                grdItems.Columns["colAvailableQty"].Visible = true;
            }
            if (!setting.Quantity)
            {
                grdItems.Columns["colQuantity"].Visible = false;
            }
            else
            {
                grdItems.Columns["colQuantity"].Visible = true;
            }
            if (!setting.BonusQuantity)
            {
                grdItems.Columns["colBonusQuantity"].Visible = false;
            }
            else
            {
                grdItems.Columns["colBonusQuantity"].Visible = true;
            }
            if (!setting.Amount)
            {
                grdItems.Columns["colAmount"].Visible = false;
            }
            else
            {
                grdItems.Columns["colAmount"].Visible = true;
            }
            if (!setting.NetAmount)
            {
                grdItems.Columns["colNetAmount"].Visible = false;
            }
            else
            {
                grdItems.Columns["colNetAmount"].Visible = true;
            }
            if (!setting.Discount)
            {
                grdItems.Columns["colDiscount"].Visible = false;
            }
            else
            {
                grdItems.Columns["colDiscount"].Visible = true;
            }
            if (!setting.DiscountType)
            {
                grdItems.Columns["colDiscountType"].Visible = false;
            }
            else
            {
                grdItems.Columns["colDiscountType"].Visible = true;
            }
            if (!setting.SaleTax)
            {
                grdItems.Columns["colSaleTax"].Visible = false;
            }
            else
            {
                grdItems.Columns["colSaleTax"].Visible = true;
            }
            if (!setting.SaleTaxType)
            {
                grdItems.Columns["colSaleTaxType"].Visible = false;
            }
            else
            {
                grdItems.Columns["colSaleTaxType"].Visible = true;
            }
            grdItems.Update();
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index
                //|| e.ColumnIndex == grdItems.Columns["colRemove"].Index
                )
            {
                if ((IsInvoiceEdit || IsInvoiceRefund) && !Convert.ToBoolean(grdItems.Rows[e.RowIndex].Cells["colIsNewRow"].Value))
                {
                    MessageBox.Show("Can't Edit This Row", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            //if (e.ColumnIndex == grdItems.Columns["colEdit"].Index) // this column later removed and edit functionality has been shifted to datagridview
            //{
            //    Clear();
            //    RegisterEvents(false);
            //    //cmbSelectItems.Enabled = false;
            //    this.uC_SearchItems1.Enabled = false;
            //    FillForm(grdItems.Rows[e.RowIndex]);
            //    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
            //    EditingRowIndex = e.RowIndex;
            //    this.btnAddItem.Text = "Update Item";
            //    RegisterEvents(true);
            //    //grdItems.Rows.RemoveAt(e.RowIndex);
            //}
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {

                long InvoiceItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceItemId"].Value);
                if (InvoiceItemId > 0)
                {
                    DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete Selected Item From Invoice?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    AddToDeletedItems(grdItems.Rows[e.RowIndex]);

                }

                //if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                //else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                ItemDetailVM foundItem = this.AddedItemsData.Where(i => i.Item.ItemID == itemId).FirstOrDefault();
                if (foundItem != null)
                {
                    try
                    {
                        if (this.AddedItemsData.Count == 1)
                        {
                            numTotalDiscount.Value = 0;
                            numTotalDiscount.Controls[0].Visible = true;
                        }


                        this.AddedItemsData.Remove(foundItem);

                        grdItems.Rows.RemoveAt(e.RowIndex);

                        //check for Discount
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            if (Convert.ToDouble(r.Cells["colDiscount"].Value) > 0)
                            {
                                numTotalDiscount.Controls[0].Visible = true;
                                numTotalDiscount.ReadOnly = true;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    { }
                }
                else
                {
                    MessageBox.Show("Some error occurred while removing selected item, please try after reloading current invoice.", "Unexpected Issue Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (e.ColumnIndex == grdItems.Columns["colReturnToggle"].Index)
            {
                int RQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colReturnQtyInitial"].Value);
                int Qty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                //if (RQty == Qty)
                //{
                //    grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].ToolTipText = "Can't Return";
                //    return;
                //}
                //WWW
                //if (RQty > 0)
                //{
                //    grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].ToolTipText = "Can't Return";
                //    return;
                //}
                if (grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].Value.ToString() == "Return")
                {
                    DataGridViewCheckBoxCell chkAffectStock = (DataGridViewCheckBoxCell)grdItems.Rows[e.RowIndex].Cells["colAffectStock"];
                    chkAffectStock.Value = "True";
                    DataGridViewButtonCell BtnCell = (DataGridViewButtonCell)grdItems.Rows[e.RowIndex].Cells["colReturnToggle"];
                    BtnCell.UseColumnTextForButtonValue = false;
                    BtnCell.Value = "Cancel";
                    grdItems.CurrentCell = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"];
                    grdItems.CurrentCell.Selected = true;
                    grdItems.BeginEdit(false);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].ReadOnly = false;
                }
                else if (grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].Value.ToString() == "Cancel")
                {
                    DataGridViewCheckBoxCell chkAffectStock = (DataGridViewCheckBoxCell)grdItems.Rows[e.RowIndex].Cells["colAffectStock"];
                    chkAffectStock.Value = "False";
                    var BtnCell = (DataGridViewButtonCell)grdItems.Rows[e.RowIndex].Cells["colReturnToggle"];
                    BtnCell.Value = "Return";
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].ReadOnly = true;
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                }
            }
        }
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSaleTaxType"].Index)
            {
                grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colSaleTax"];
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscountType"].Index
             || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBatch"].Index
             || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSaleTaxType"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscountType"].Index)
            {



            }

        }
        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if(e.Exception.Message == "DataGridViewComboBoxCell value is not valid")
            //{
            //    object value = grdItems.Rows[e.RowIndex].Cells["colCmbBatch"].Value;

            //}
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidIntegerKey);
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colRate"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler((arg, ers) => IsValidDoubleNumerickeys(sender, ers, tb));
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler((arg, ers) => IsValidDoubleNumerickeys(sender, ers, tb));
                //e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidIntegerKey);
            }
            ComboBox cmb = e.Control as ComboBox;
        }

        private void IsValidDoubleNumerickeys(object sender, KeyPressEventArgs e, TextBox tb)
        {
            try
            {
                SharedFunctions.IsValidDoubleNumericDatagridview(sender, e, tb);
            }
            catch (Exception) { }
        }
        private void grdItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //this.grdItems.Rows[e.RowIndex].Cells["SrNo"].Value = (e.RowIndex + 1).ToString();
            //var grid = sender as DataGridView;
            //var rowIdx = (e.RowIndex + 1).ToString();

            //var centerFormat = new StringFormat()
            //{
            //    // right alignment might actually make more sense for numbers
            //    Alignment = StringAlignment.Center,
            //    LineAlignment = StringAlignment.Center
            //};

            //var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            //e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                int itemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                if (!isLoadingForm)
                {
                    isInvoiceUpdated = true;
                }
                //if (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value) > 0)
                //{
                //    IsDiscountByItem = true;
                //    numTotalDiscount.Enabled = false;
                //    cmbTotalDiscountType.Enabled = false;
                //}      
                if (isLoadingForm || this.HoldingInvoiceId > 0 && !isNewItemAdded)
                {
                    this.GetSelectedItemData_1(itemId, grdItems.Rows[e.RowIndex]);
                }
                else if (this.IsOnlineIntegrate)
                {
                    this.GetSelectedItemDataOnline(itemId, grdItems.Rows[e.RowIndex]);
                }
                else
                {
                    this.GetSelectedItemData(itemId, grdItems.Rows[e.RowIndex]);


                }

                //CalculateTotals(grdItems.Rows[e.RowIndex]);
                //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colBatch"];
                //if (cell.Items.Count > 0)
                //{
                //    grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = ((BatchStockVM)cell.Items[0]).BatchName;
                //}


            }
            catch (Exception ex)
            {
                if (ex.Message == "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index")
                {
                    SharedFunctions.MessageBoxShow("Item have Zero Available Quantity, Please Add Stock", MessageType.Warning);
                    //SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");

                    grdItems.Rows.RemoveAt(grdItems.Rows.Count - 1);

                }
            }
        }
        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            bool discountFound = false;

            if (grdItems.Rows.Count == 1)
            {
                grdItems.Columns["colDiscount"].ReadOnly = false;
                numTotalDiscount.ReadOnly = false;
            }

            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToDouble(r.Cells["colDiscount"].Value) > 0)
                {
                    discountFound = true;
                }
            }
            if (!discountFound)
            {
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            lblSubTotal.Text = Math.Round(GrandTotals.Item1, 2, MidpointRounding.AwayFromZero).ToString();
            lblGrandTotal.Text = (Math.Round(GrandTotals.Item2, 2, MidpointRounding.AwayFromZero) + Convert.ToDouble(txtServices.Value)).ToString();

            double netProfit = GrandTotals.Item4;
            lblNetProfit.Text = Math.Round((GrandTotals.Item2 - netProfit)).ToString();

            if (!this.IsInvoiceRefund && numPmnt1.Enabled)
            {
                if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                {
                    numPmnt1.Value = (decimal)Math.Round(GrandTotals.Item2, 2, MidpointRounding.AwayFromZero);
                }
            }
            numTotalDiscount.Value = (decimal)Math.Round(GrandTotals.Item3, 2, MidpointRounding.AwayFromZero);
            numModifiedDiscount.Value = (decimal)GrandTotals.Item3;
            GetCustomerTotals();
            getprofit();
            if (!IsRemovedAll)
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                LoadSerial();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }
        private void grdItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                {
                    SelectedItemId = 0;
                    uC_SearchItems1.SetFocus();
                }

            }
        }
        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell == null)
                    return;
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                {
                    SelectedItemId = 0;
                    uC_SearchItems1.SetFocus();
                }

            }
        }
        private void LoadSerial()
        {
            foreach (DataGridViewRow row in grdItems.Rows)
            {
                grdItems.Rows[row.Index].Cells["SrNo"].Value = string.Format("{0}  ", row.Index + 1).ToString();
            }
        }
        #endregion

        private void txtSearchByPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        public string LoadItemRackName(int ItemId)
        {
            string rackname = "";
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    rackname = unitOfWork.InvoiceRepository.RackName(ItemId);
                }
            }
            catch (Exception)
            {

            }
            return rackname;
        }

        private void txtSearchByName_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidAlphabaticKey(sender, e);
        }

        #region FBRInvoiceIntigration
        private FBRResponseVM SendInvoiceToFBR(Invoice objInvoice)
        {
            FBRResponseVM result = new FBRResponseVM();
            try
            {
                FBRInvoice objInv = new FBRInvoice();
                objInv.InvoiceNumber = string.Empty;
                objInv.POSID = SharedVariables.AdminFBRSettings.POSIDEnc;
                objInv.USIN = SharedVariables.AdminPractiseSetting.FBRUsinNo;
                objInv.DateTime = DateTime.Now;
                objInv.BuyerNTN = "";
                objInv.BuyerCNIC = "";
                objInv.BuyerName = "";
                objInv.BuyerPhoneNumber = "";
                objInv.PaymentMode = 1;
                objInv.TotalSaleValue = objInvoice.SubTotal;
                objInv.TotalQuantity = objInvoice.InvoiceItems.Sum(x => x.Quantity);
                objInv.TotalTaxCharged = 0;
                objInv.TotalBillAmount = objInvoice.GrandTotal;
                objInv.Discount = objInvoice.TotalDiscount;
                objInv.FurtherTax = 0;
                objInv.InvoiceType = 1;
                objInv.Items = objInvoice.InvoiceItems.Select(x => new FBRInvoiceItems
                {
                    ItemCode = x.Item.ItemID.ToString(),
                    ItemName = x.Item.ItemName,
                    Quantity = x.Quantity,
                    TotalAmount = x.Amount,
                    SaleValue = x.NetAmount,
                    TaxCharged = 0,
                    TaxRate = 0,
                    PCTCode = x.Item.PCTCode,
                    FurtherTax = 0,
                    InvoiceType = 1,
                    Discount = x.Discount,
                }).ToList();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SharedVariables.AdminFBRSettings.TockenEnc);
                    string json = JsonConvert.SerializeObject(objInv);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    HttpResponseMessage response = client.PostAsync("https://gw.fbr.gov.pk/imsp/v1/api/Live/PostData", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string model = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<FBRResponseVM>(model);
                    }
                    else
                    {
                        result.Error = "Api response error.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in grdItems.Rows)
                {
                    DataGridViewCheckBoxCell chkAffectStock = (DataGridViewCheckBoxCell)item.Cells["colAffectStock"];
                    chkAffectStock.Value = "True";
                    DataGridViewButtonCell BtnCell = (DataGridViewButtonCell)item.Cells["colReturnToggle"];
                    BtnCell.UseColumnTextForButtonValue = false;
                    BtnCell.Value = "Cancel";
                    grdItems.CurrentCell = item.Cells["colReturnQuantity"];
                    grdItems.CurrentCell.Selected = true;
                    grdItems.BeginEdit(false);
                    item.Cells["colReturnQuantity"].ReadOnly = false;
                    item.Cells["colReturnQuantity"].Value = SharedFunctions.StringToInt(item.Cells["colQuantity"].Value);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                grdItems.CurrentCell = this.grdItems[0, 0];
                this.grdItems.CurrentCell.Selected = false;
            }
        }

        private void lblNetProfit_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void cmbPosServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (grdItems.Rows.Count <= 0)
            { return; }
            if (txtServices.Enabled)
            {
                GetTotalsByTotalDiscountChanges();
            }
        }

        private void uC_SearchItems1_Load(object sender, EventArgs e)
        {
            SelectedItemId = 0;
        }

        private void uC_SearchItems1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            copyLastInvoice();
            string a = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colUnit"].Value.ToString();
            grdItems.Rows[grdItems.Rows.Count - 1].Cells["colUnit"].Value = "";
            grdItems.Rows[grdItems.Rows.Count - 1].Cells["colUnit"].Value = a;


        }

        private void uC_SearchItems1_Leave(object sender, EventArgs e)
        {
            //uC_SearchItems1.SetFocus();

            uC_SearchItems1.SetText = "";

            SelectedItemId = 0;

        }

        private void grdItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DubItemAdded = false;
            if (!IsInvoiceRefund && !IsInvoiceEdit)
            {
                CalculateItemTotals();
                CalculateTotals();
                //calculateSaleTax1();
                //prfit = Convert.ToDouble(lblNetProfit.Text);
                getprofit();
            }
        }
        private double calculateSaleTax()
        {
            double subtotal = Convert.ToDouble(lblSubTotal.Text);
            double gtotal = Convert.ToDouble(lblGrandTotal.Text);
            double saletax = 0;
            double saletax1 = 0;
            if (!IsInvoiceEdit)
            {


                //Discount = ((Discount / 100) * UnitCost);
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (r.Cells["colSaleTaxType"].Value.ToString() != "%")
                    {
                        saletax += Convert.ToDouble(r.Cells["colSaleTax"].Value);
                        saletax1 = Convert.ToDouble(r.Cells["colSaleTax"].Value);
                    }
                    else
                    {
                        saletax += ((Convert.ToDouble(r.Cells["colSaleTax"].Value)) / 100) * Convert.ToDouble(r.Cells["colAmount"].Value);
                        saletax1 = ((Convert.ToDouble(r.Cells["colSaleTax"].Value)) / 100) * Convert.ToDouble(r.Cells["colAmount"].Value);
                    }
                    //r.Cells["colNetAmount"].Value = Convert.ToDouble(r.Cells["colNetAmount"].Value) + saletax1;
                }
                //lblSubTotal.Text = (subtotal + saletax).ToString();
                //lblGrandTotal.Text = (gtotal + saletax).ToString();
            }
            return saletax;
            //CalculateTotals();
            //isInvoiceUpdated = true;
            //GetCustomerTotals();

        }
        private void calculateSaleTax1()
        {
            if (!IsInvoiceEdit)
            {
                double subtotal = Convert.ToDouble(lblSubTotal.Text);
                double gtotal = Convert.ToDouble(lblGrandTotal.Text);
                double saletax = 0;
                double saletax1 = 0;

                //Discount = ((Discount / 100) * UnitCost);
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    if (r.Cells["colSaleTaxType"].Value.ToString() != "%")
                    {
                        saletax += Convert.ToDouble(r.Cells["colSaleTax"].Value);
                        saletax1 = Convert.ToDouble(r.Cells["colSaleTax"].Value);
                    }
                    else
                    {
                        saletax += (Convert.ToDouble(r.Cells["colSaleTax"].Value) / 100) * Convert.ToDouble(r.Cells["colRate"].Value);
                        saletax1 = (Convert.ToDouble(r.Cells["colSaleTax"].Value) / 100) * Convert.ToDouble(r.Cells["colRate"].Value);
                    }
                    //r.Cells["colNetAmount"].Value = Convert.ToDouble(r.Cells["colNetAmount"].Value) - saletax1; 
                }
                lblSubTotal.Text = (subtotal - saletax).ToString();
                lblGrandTotal.Text = (gtotal - saletax).ToString();
            }
        }
        private void grdItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsInvoiceRefund && !IsInvoiceEdit)
            {

                CalculateItemTotals();
                CalculateTotals();
                getprofit();
                duep = lblDue.Text;
                cashback = lblBalance.Text;
                advance = lblAdvance.Text;
                if (Properties.Settings.Default.IsUsingBarcode)
                {
                    uC_SearchItems1.SetFocus();
                }
                //int index = grdItems.CurrentRow.Index;
                //grdItems.FirstDisplayedScrollingRowIndex = index;
                //calculateSaleTax();        
            }
        }

        private void grdItems_EditModeChanged(object sender, EventArgs e)
        {

        }

        private void uC_SearchItems1_Enter(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
        }
        string grandtotal = "";
        private void lblGrandTotal_TextChanged(object sender, EventArgs e)
        {
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                if ((lblGrandTotal.Text != grandtotal && !(Convert.ToDouble(txtCurrentRefund.Text) > 0)))
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else if (Convert.ToDouble(txtCurrentRefund.Text) > 0 && IsInvoiceRefund)
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void txtCurrentRefund_TextChanged(object sender, EventArgs e)
        {
            if (IsInvoiceRefund)
            {
                if ((Convert.ToDouble(txtCurrentRefund.Text) > 0))
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
            }

        }

        private void lblPaymentPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblBalance_TextChanged(object sender, EventArgs e)
        {
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                if (lblBalance.Text != cashback && !(Convert.ToDouble(txtCurrentRefund.Text) > 0) && lblBalance.Text != "0")
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (cbouom.SelectedIndex == 0)
            {
                //grdItems.CellValueChanged -= grdItems_CellValueChanged;

                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    row.Cells["colUnit"].Value = "Pack";
                }
                //grdItems.CellValueChanged += grdItems_CellValueChanged;

                CalculateItemTotals();
                CalculateTotals();
                getprofit();
                duep = lblDue.Text;
                cashback = lblBalance.Text;
                advance = lblAdvance.Text;

            }
            else if (cbouom.SelectedIndex == 1)
            {
                //grdItems.CellValueChanged -= grdItems_CellValueChanged;

                foreach (DataGridViewRow row in grdItems.Rows)
                {
                    row.Cells["colUnit"].Value = "Units";
                }
                //grdItems.CellValueChanged += grdItems_CellValueChanged;

                CalculateItemTotals();
                CalculateTotals();
                getprofit();
                duep = lblDue.Text;
                cashback = lblBalance.Text;
                advance = lblAdvance.Text;
            }
            else
            {
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Can't Save Empty Invoice, ", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.InvoiceEditId > 0)
            {
                bool val = checkTabforMultipleItemBatch();
                if (val)
                {
                    EditQInvoice(this.InvoiceEditId, true, false, SharedVariables.LoggedInUser.UserId, true);
                    servicecheck = 0;
                    notifyIcon1.Text = "Save";
                    notifyIcon1.ShowBalloonTip(3, "Estimate Bill Invoice Update", "Saved", ToolTipIcon.Info);
                }
            }
            else
            {
                bool val = checkTabforMultipleItemBatch();
                if (val)
                {
                    InsertQInvoice(true, false, SharedVariables.LoggedInUser.UserId, true);
                    servicecheck = 0;
                    notifyIcon1.Text = "Save";
                    notifyIcon1.ShowBalloonTip(3, "New Estimate Bill Invoice", "Saved", ToolTipIcon.Info);

                }
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        public void FillAttributesInDropdownList(RadCheckedDropDownList dropdownList, InvoiceSetting invoiceSetting)
        {
            dropdownList.Items.Clear();

            // Get all properties of the InvoiceSetting class
            PropertyInfo[] properties = typeof(InvoiceSetting).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if(property.Name == "SettingId" || property.Name == "UserID")
                {
                    continue;
                }
                if(!SharedVariables.AdminPharmacySetting.IsFBRUnable)
                {
                    if(property.Name == "PCTCode")
                    {
                        continue;
                    }
                }
                dropdownList.Items.Add(property.Name);
                
            }
            ApplySetting(invoiceSetting);
        }
        private void fillgridsetting()
        {
            try
            {
                InvoiceSetting settings = new InvoiceSetting();
                using(unitOfWork = new UnitOfWork())
                {
                    settings = unitOfWork.InvoiceSettingRepository.GetAll().Where(u=>u.UserID==SharedVariables.LoggedInUser.UserId).FirstOrDefault();
                }
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Amount").FirstOrDefault().Index].Checked = settings.Amount;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "AvailableQty").FirstOrDefault().Index].Checked = settings.AvailableQty;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Batch").FirstOrDefault().Index].Checked = true;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "BonusQuantity").FirstOrDefault().Index].Checked = settings.BonusQuantity;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ConvUnit").FirstOrDefault().Index].Checked = settings.ConvUnit;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "CostPrice").FirstOrDefault().Index].Checked = settings.CostPrice;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscCost").FirstOrDefault().Index].Checked = settings.DiscCost;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Discount").FirstOrDefault().Index].Checked = settings.Discount;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscountType").FirstOrDefault().Index].Checked = settings.DiscountType;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ItemName").FirstOrDefault().Index].Checked = true;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "NetAmount").FirstOrDefault().Index].Checked = settings.NetAmount;
                if (InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").ToList().Count == 1)
                {
                    InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").FirstOrDefault().Index].Checked = settings.PCTCode;
                }
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ProfitMargin").FirstOrDefault().Index].Checked = settings.ProfitMargin;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Quantity").FirstOrDefault().Index].Checked = true;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "RackNo").FirstOrDefault().Index].Checked = settings.RackNo;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Rate").FirstOrDefault().Index].Checked = true;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTax").FirstOrDefault().Index].Checked = settings.SaleTax;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTaxType").FirstOrDefault().Index].Checked = settings.SaleTaxType;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Sr").FirstOrDefault().Index].Checked = true;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Unit").FirstOrDefault().Index].Checked = true;
                SharedVariables.invoiceSetting = settings;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Batch").FirstOrDefault().Index].Enabled = false;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Rate").FirstOrDefault().Index].Enabled = false;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Quantity").FirstOrDefault().Index].Enabled = false;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ItemName").FirstOrDefault().Index].Enabled = false;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Sr").FirstOrDefault().Index].Enabled = false;
                InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Unit").FirstOrDefault().Index].Enabled = false;
                ApplySetting(settings);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void radCheckedDropDownList1_ItemCheckedChanged(object sender, Telerik.WinControls.UI.RadCheckedListDataItemEventArgs e)
        { 
            if(e.Item.Text == "Sr" || e.Item.Text == "Batch" || e.Item.Text == "Quantity" || e.Item.Text == "Rate" || e.Item.Text == "Unit" || e.Item.Text == "ItemName")
            {
                //MessageBox.Show("Changing Visibility of This  Item is not Allowed ... !");
                e.Item.Checked = true;
                return;
            }
            if(get("select * from InvoiceSettings where UserID = " + SharedVariables.LoggedInUser.UserId).Rows.Count == 0)
            {
                savegridsetting();
            }
            else
            {
                Updategridsetting();
            }
        }

        private void InvoiceItemsToolStrip_PopupClosed(object sender, RadPopupClosedEventArgs args)
        {
           
        }
        private void savegridsetting()
        {
            try
            {
                InvoiceSetting settings = new InvoiceSetting();

                settings.Amount = true;
                settings.AvailableQty = true;
                settings.Batch = true;
                settings.BonusQuantity = SharedVariables.AdminPharmacySetting.IsBonusQty;
                settings.ConvUnit = true;
                settings.CostPrice = true;
                settings.DiscCost = SharedVariables.AdminPharmacySetting.DiscountedPrice;
                settings.Discount = true;
                settings.DiscountType = true;
                settings.ItemName = true;
                settings.NetAmount = true;
                settings.PCTCode = false;
                settings.ProfitMargin = true;
                settings.Quantity = true;
                settings.RackNo = SharedVariables.AdminPharmacySetting.ShowRackNoInPOS;
                settings.Rate = true;
                settings.SaleTax = true;
                settings.SaleTaxType = true;
                settings.Sr = true;
                settings.Unit = true;
                settings.UserID = SharedVariables.LoggedInUser.UserId;
/*

                settings.Amount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Amount").FirstOrDefault().Index].Checked;
                settings.AvailableQty = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "AvailableQty").FirstOrDefault().Index].Checked;
                settings.Batch = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Batch").FirstOrDefault().Index].Checked;
                settings.BonusQuantity = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "BonusQuantity").FirstOrDefault().Index].Checked;
                settings.ConvUnit = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ConvUnit").FirstOrDefault().Index].Checked;
                settings.CostPrice = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "CostPrice").FirstOrDefault().Index].Checked;
                settings.DiscCost = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscCost").FirstOrDefault().Index].Checked;
                settings.Discount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Discount").FirstOrDefault().Index].Checked;
                settings.DiscountType = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscountType").FirstOrDefault().Index].Checked;
                settings.ItemName = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ItemName").FirstOrDefault().Index].Checked;
                settings.NetAmount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "NetAmount").FirstOrDefault().Index].Checked;
                settings.PCTCode = InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").ToList().Count == 1 ? InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").FirstOrDefault().Index].Checked : false;
                settings.ProfitMargin = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ProfitMargin").FirstOrDefault().Index].Checked;
                settings.Quantity = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Quantity").FirstOrDefault().Index].Checked;
                settings.RackNo = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "RackNo").FirstOrDefault().Index].Checked;
                settings.Rate = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Rate").FirstOrDefault().Index].Checked;
                settings.SaleTax = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTax").FirstOrDefault().Index].Checked;
                settings.SaleTaxType = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTaxType").FirstOrDefault().Index].Checked;
                settings.Sr = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Sr").FirstOrDefault().Index].Checked;
                settings.Unit = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Unit").FirstOrDefault().Index].Checked;
*/
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.InvoiceSettingRepository.Insert(settings);
                    unitOfWork.Save();
                }
                SharedVariables.invoiceSetting = settings;
                ApplySetting(settings);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void Updategridsetting()
        {
            try
            {
                InvoiceSetting settings = new InvoiceSetting();
                using(unitOfWork = new UnitOfWork())
                {
                    settings = unitOfWork.InvoiceSettingRepository.GetAll().Where(u => u.UserID == SharedVariables.LoggedInUser.UserId).FirstOrDefault();
                }
                settings.Amount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Amount").FirstOrDefault().Index].Checked;
                settings.AvailableQty = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "AvailableQty").FirstOrDefault().Index].Checked;
                settings.Batch = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Batch").FirstOrDefault().Index].Checked;
                settings.BonusQuantity = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "BonusQuantity").FirstOrDefault().Index].Checked;
                settings.ConvUnit = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ConvUnit").FirstOrDefault().Index].Checked;
                settings.CostPrice = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "CostPrice").FirstOrDefault().Index].Checked;
                settings.DiscCost = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscCost").FirstOrDefault().Index].Checked;
                settings.Discount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Discount").FirstOrDefault().Index].Checked;
                settings.DiscountType = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "DiscountType").FirstOrDefault().Index].Checked;
                settings.ItemName = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ItemName").FirstOrDefault().Index].Checked;
                settings.NetAmount = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "NetAmount").FirstOrDefault().Index].Checked;
                settings.PCTCode = InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").ToList().Count == 1 ? InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "PCTCode").FirstOrDefault().Index].Checked : false;
                settings.ProfitMargin = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "ProfitMargin").FirstOrDefault().Index].Checked;
                settings.Quantity = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Quantity").FirstOrDefault().Index].Checked;
                settings.RackNo = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "RackNo").FirstOrDefault().Index].Checked;
                settings.Rate = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Rate").FirstOrDefault().Index].Checked;
                settings.SaleTax = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTax").FirstOrDefault().Index].Checked;
                settings.SaleTaxType = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "SaleTaxType").FirstOrDefault().Index].Checked;
                settings.Sr = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Sr").FirstOrDefault().Index].Checked;
                settings.Unit = InvoiceItemsToolStrip.Items[InvoiceItemsToolStrip.Items.Where(i => i.Text.ToString() == "Unit").FirstOrDefault().Index].Checked;
                settings.UserID = SharedVariables.LoggedInUser.UserId;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.InvoiceSettingRepository.Update(settings);
                    unitOfWork.Save();
                }
                SharedVariables.invoiceSetting = settings;
                ApplySetting(settings);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void InvoiceItemsToolStrip_CheckAllItemCheckedChanged(object sender, RadCheckedListDataItemEventArgs e)
        {
            
        }

        private void lblAdvance_TextChanged(object sender, EventArgs e)
        {
            if (IsInvoiceEdit || IsInvoiceRefund)
            {
                if (lblAdvance.Text != advance && !(Convert.ToDouble(txtCurrentRefund.Text) > 0) && lblAdvance.Text != "0")
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSaveAndPrint.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }
    }


    //Stock Consumption from Multiple Batches SCMB
    class MyDGV : DataGridView
    {
        public bool SelectNextCell(string Key)
        {
            try
            {
                if (CurrentCell == null)
                    return false;
                int row = CurrentCell.RowIndex;
                int column = CurrentCell.ColumnIndex;
                DataGridViewCell startingCell = CurrentCell;
                if (Key == "right")
                {
                    for (int i = CurrentCell.ColumnIndex + 1; i <= ColumnCount; i++)
                    {
                        if (Columns[i].ReadOnly == false && Columns[i].Visible == true)
                        {
                            CurrentCell = this[i, row];
                            return true;
                        }
                    }
                }
                else if (Key == "left")
                {
                    for (int i = CurrentCell.ColumnIndex - 1; i >= 0; i--)
                    {
                        if (Columns[i].ReadOnly == false && Columns[i].Visible == true)
                        {
                            CurrentCell = this[i, row];
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                return SelectNextCell("right");
            if (e.KeyCode == Keys.Left)
                return SelectNextCell("left");
            return base.ProcessDataGridViewKey(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Right)
                return SelectNextCell("right");
            if ((keyData & Keys.KeyCode) == Keys.Left)
                return SelectNextCell("left");
            return base.ProcessDialogKey(keyData);
        }
    }
}