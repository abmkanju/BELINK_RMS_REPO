using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.MenuBar.Pharmacy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Serilog;
using Pharmacy.Core.Utility;

namespace Pharmacy.UI.FranchiseModuels
{
    public partial class frmOnlineIntegrate : Form
    {
        enum FilterType { Default = 1, OrderNumber = 2, CustomerName = 3, Phone = 4 }
        FilterType filter = FilterType.Default;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        InvoiceRepository repInvoice = new InvoiceRepository(cxt);
        UnitOfWork unitOfWork = new UnitOfWork();
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        PatientRepository repPatients = new PatientRepository(cxt);
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int PageNo = 1;
        Service obj = new Service();
        Invoice objInvoice = new Invoice();
        DataTable dtStockConsumption = new DataTable();
        public static ManualResetEvent allCompleted = new ManualResetEvent(false);
        Response chkRes = new Response();
        public frmOnlineIntegrate()
        {
            InitializeComponent();
        }

        private void OrderChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (OrderChange.Text.Trim() != "")
                {
                    filter = FilterType.OrderNumber;
                }
                else
                {
                    filter = FilterType.Default;
                }
                CustName1.Text = "";
                txtPhone.Text = "";
                PageNo = 1;
                LoadOnlineIntegrateItems();
            }

        }
        private void LoadOnlineIntegrateItems()
        {
            string CustName = "", CustAddress = "", CustPhone = "";
            string itemId = "";
            string itemName = "";
            string Qty = "";

            IPagedList<OnlineIntegrateVM> OnlineIntegrate;
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.OrderNumber)
                {
                    OnlineIntegrate = unitOfWork.OnlineIntegrateRepository.GetOnlineIntegrateByOrder(OrderChange.Text, PageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.CustomerName)
                {
                    OnlineIntegrate = unitOfWork.OnlineIntegrateRepository.GetOnlineIntegrateByCutomer(CustName1.Text, PageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Phone)
                {
                    OnlineIntegrate = unitOfWork.OnlineIntegrateRepository.GetOnlineIntegrateByPhone(txtPhone.Text, PageNo, SharedVariables.PageSize);
                }
                else
                {
                    OnlineIntegrate = unitOfWork.OnlineIntegrateRepository.GetOnlineIntegrate(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
            }

            grdOnline.Rows.Clear();
            foreach (OnlineIntegrateVM i in OnlineIntegrate)
            {
                //forCustomer
                foreach (OnlineCustomerVM ip in i.OnlineCustomer)
                {
                    CustName = ip.CustName;
                    CustAddress = ip.CustAddress;
                    CustPhone = ip.CustPhone;
                }

                //forItems
                foreach (OnlineItemVM ic in i.OnlineItem)
                {
                    itemName += ic.ItemName + ", ";
                    Qty += ic.Quantity + ", ";
                }

                grdOnline.Rows.Add
                (
                   i.Delivered,
                   i.OrderValue,
                   i.InvoiceId,
                   i.Cancel,
                   i.Pack,
                   i.Dispatch,
                   itemId,
                   itemName,
                   Qty,
                   i.OrderId,
                   CustName,
                   CustAddress,
                   CustPhone,
                   i.Origin,
                   i.TotalDiscount,
                   i.WalletAmount,
                   i.GrandTotal,
                   i.CancelReason,
                   i.CreateDate
                );

                itemId = "";
                Qty = "";
                itemName = "";
            }

            foreach (DataGridViewRow r in grdOnline.Rows)
            {
                if (Convert.ToBoolean(r.Cells["IsDelivered"].Value) == true)
                {
                    r.Cells["colDelivered"].Value = "Delivered";
                    r.Cells["colDelivered"].Style.ForeColor = Color.Green;
                }
            }

        }

        public void SaveOnlineIntegrate(OrderOnlineAws objOnline)
        {
            try
            {
                DataTable gets = new DataTable();
                TransactionScope scope;
                using (scope = new TransactionScope())
                {
                    try
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            if (objOnline.data != null)
                            {
                                for (int i = 0; i < objOnline.data.Count; i++)
                                {
                                    if (checkOrder(objOnline.data[i].id.ToString()) == "0")
                                    {
                                        OnlineOrder objOrder = new OnlineOrder();
                                        objOrder.OrderValue = objOnline.data[i].id.ToString();
                                        objOrder.Invoice = null;
                                        objOrder.CreateDate = DateTime.Now;
                                        //objOrder.Origin = "Web";
                                        objOrder.VoucherNumber = objOnline.data[i].voucher_name;
                                        objOrder.Origin = objOnline.data[i].origin;
                                        objOrder.WalletAmount = Math.Round(objOnline.data[i].wallet_amount, 2);
                                        objOrder.VoucherAmount = Convert.ToDouble(objOnline.data[i].voucher_discount);
                                        //objOrder.DeliveryCharges = objOnline.delivery;

                                        if (objOnline.data[i].payment_method.ToLower() == "cashondelivery")
                                        {
                                            objOrder.PaymentMethod = "Cash";
                                        }
                                        else
                                        {
                                            objOrder.PaymentMethod = "Online Payment";
                                        }
                                        objOrder.Cancel = false;
                                        objOrder.IsActive = true;
                                        objOrder.Pack = false;
                                        objOrder.Dispatch = false;
                                        objOrder.DeliveryCharges = Convert.ToInt32(objOnline.data[i].delivery_charges);
                                        objOrder.Delivered = false;
                                        objOrder.OnlineCustomer = new List<OnlineCustomer>();
                                        objOrder.OnlineOrderItem = new List<OnlineOrderItem>();
                                        objOrder.GrandTotal = Convert.ToInt32(objOnline.data[i].order_grand_total);
                                        objOrder.TotalDiscount = Convert.ToInt32(objOnline.data[i].product_discount);

                                        //for customer
                                        OnlineCustomer OnlineCust = new OnlineCustomer();
                                        OnlineCust.Name = objOnline.data[i].customer.Name;

                                        if (objOnline.data[i].customer.Address != null)
                                        {
                                            OnlineCust.Address = objOnline.data[i].customer.Address.ToString();
                                        }
                                        OnlineCust.Phone = objOnline.data[i].customer.phone;
                                        OnlineCust.IsActive = true;
                                        objOrder.OnlineCustomer.Add(OnlineCust);

                                        //Online Items save
                                        for (int j = 0; j < objOnline.data[i].order_items.Count; j++)
                                        {
                                            OnlineOrderItem OnlineItem = new OnlineOrderItem();
                                            OnlineItem.ConversionUnit = Convert.ToInt32(objOnline.data[i].order_items[j].conversion_unit);
                                            OnlineItem.Rate = Convert.ToDouble(objOnline.data[i].order_items[j].sale_price);
                                            OnlineItem.PerUnitCostPrice = Convert.ToDouble(objOnline.data[i].order_items[j].unit_cost);
                                            //OnlineItem.Unit = objOnline.data[i].order_items[j].unit.ToString() == "0" ? "pack" : "unit";
                                            OnlineItem.UniqueItem_Id = Convert.ToInt64(objOnline.data[i].order_items[j].item_id);
                                            //OnlineItem.Unit = "pack";
                                            if (objOnline.data[i].order_items[j].pack_type.ToLower() == "pack")
                                            {
                                                OnlineItem.Unit = "pack";
                                            }
                                            else
                                            {
                                                OnlineItem.Unit = "unit";
                                            }

                                            //get cost/Rate frm Stock
                                            //DataTable itemId = get("select ItemID from Items where ItemName = " + objOnline[i].order_items[j].variant);
                                            //gets = get("select * from StockItems where ItemID = " + itemId.Rows[0]);

                                            //OnlineItem.PerUnitCostPrice = OnlineItem.Unit == "0" ? OnlineItem.PerUnitCostPrice * OnlineItem.ConversionUnit : OnlineItem.PerUnitCostPrice;
                                            //OnlineItem.Rate = OnlineItem.Unit == "0" ? OnlineItem.Rate * OnlineItem.ConversionUnit : OnlineItem.Rate;

                                            if (objOnline.data[i].order_items[j].size_of_strip != null)
                                            {
                                                OnlineItem.SizeOfStrip = objOnline.data[i].order_items[j].size_of_strip;
                                            }
                                            else
                                            {
                                                OnlineItem.SizeOfStrip = "0";
                                            }

                                            OnlineItem.ItemName = objOnline.data[i].order_items[j].variant;
                                            OnlineItem.Quantity = Convert.ToInt32(objOnline.data[i].order_items[j].quantity);
                                            OnlineItem.IsActive = true;
                                            OnlineItem.Amount = 0;
                                            OnlineItem.NetAmount = 0;
                                            OnlineItem.saletax = 0;
                                            OnlineItem.saletaxType = 0;
                                            OnlineItem.Discount = Convert.ToDouble(objOnline.data[i].order_items[j].discount_amount);
                                            OnlineItem.DiscountType = 1;

                                            objOrder.OnlineOrderItem.Add(OnlineItem);
                                        }

                                        unitOfWork.OnlineIntegrateRepository.Insert(objOrder);
                                        unitOfWork.Save();
                                    }

                                    else if (Convert.ToDateTime(objOnline.data[i].created_at).Date != Convert.ToDateTime(objOnline.data[i].updated_at).Date)
                                    {
                                        OnlineOrder objOrder1 = new OnlineOrder();
                                        if (objOnline.data[i].food_panda_status != "")
                                        {
                                            int m = updateDB("Update OnlineOrders set Delivered =" + 1 + " where OrderValue= " + objOnline.data[i].id);
                                        }

                                        #region update working
                                        ////getId
                                        //DataTable OrderID = get("Select OrderId from OnlineOrders where OrderValue =" + objOnline[i].id);     
                                        //objOrder1 = unitOfWork.OnlineIntegrateRepository.GetOrderByIdWithDetails(Convert.ToInt32(OrderID.Rows[0][0]));

                                        //    objOrder1.OrderValue = objOnline[i].order_number.ToString();
                                        //    objOrder1.Invoice = null;
                                        //    objOrder1.CreateDate = DateTime.Now;
                                        //    //objOrder.PaymentMethod = "Online";
                                        //    //objOrder1.Origin = "Android";

                                        //    objOrder1.Cancel = false;
                                        //    objOrder1.IsActive = true;
                                        //    objOrder1.Pack = false;
                                        //    objOrder1.Dispatch = false;
                                        //    objOrder1.Dispatch = false;
                                        //    objOrder1.Delivered = false;

                                        //    //for customer
                                        //    OnlineCustomer OnlineCust1 = new OnlineCustomer();
                                        //    OnlineCust1.Name = objOnline[i].customer.Name;
                                        //    OnlineCust1.Address = objOnline[i].customer.Address.ToString();
                                        //    OnlineCust1.Phone = objOnline[i].customer.phone;

                                        //    int m = updateDB("Update OnlineOrders set CreateDate ='" + objOrder1.CreateDate + "' where OrderId= " + objOrder1.OrderId);
                                        //    int k = updateDB("Update OnlineCustomers set Name ='" + OnlineCust1.Name + "' where OnlineOrder_OrderId= " + objOrder1.OrderId);          
                                        //    DataTable dt = get("Select OrderItemId from OnlineOrderItems where OnlineOrder_OrderId= " + objOrder1.OrderId);

                                        //    //Online Items update
                                        //    for (int j = 0; j < objOnline[i].order_items.Count; j++)
                                        //    {
                                        //        OnlineOrderItem OnlineItem1 = new OnlineOrderItem();

                                        //        OnlineItem1.ConversionUnit = Convert.ToInt32(objOnline[i].order_items[j].conversion_unit);
                                        //        OnlineItem1.Rate = Convert.ToDouble(objOnline[i].order_items[j].sale_price);
                                        //        OnlineItem1.PerUnitCostPrice = Convert.ToDouble(objOnline[i].order_items[j].unit_cost);
                                        //        OnlineItem1.Unit = objOnline[i].order_items[j].unit.ToString() == "0" ? "pack" : "unit";

                                        //        OnlineItem1.PerUnitCostPrice = OnlineItem1.Unit == "0" ? OnlineItem1.PerUnitCostPrice / OnlineItem1.ConversionUnit : OnlineItem1.PerUnitCostPrice;
                                        //        OnlineItem1.Rate = OnlineItem1.Unit == "0" ? OnlineItem1.Rate / OnlineItem1.ConversionUnit : OnlineItem1.Rate;
                                        //        OnlineItem1.ItemName = objOnline[i].order_items[j].variant;
                                        //        //OnlineItem1.ItemName = "Mark 20mg Cap 14's";
                                        //        OnlineItem1.Quantity = Convert.ToInt32(objOnline[i].order_items[j].quantity);
                                        //        OnlineItem1.ItemName = objOnline[i].order_items[j].variant;

                                        //        OnlineItem1.Amount = 0;
                                        //        OnlineItem1.NetAmount = 0;
                                        //        OnlineItem1.saletax = 0;
                                        //        OnlineItem1.saletaxType = 0;
                                        //        OnlineItem1.Discount = 0;
                                        //        OnlineItem1.DiscountType = 0;

                                        //        string orderId = dt.Rows[j]["OrderItemId"].ToString();
                                        //        int a = updateDB("Update OnlineOrderItems set ConversionUnit =" + OnlineItem1.ConversionUnit + ",Rate =" + OnlineItem1.Rate + ",PerUnitCostPrice =" + OnlineItem1.PerUnitCostPrice + ",Quantity=" + OnlineItem1.Quantity + " where OrderItemId= " + orderId);
                                        #endregion
                                    }
                                    //}
                                }
                            }
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
            catch 
            {

            }
        }

        public string checkOrder(string OrderNumber)
        {
            string count = "0";
            DataTable dt = get("Select * from OnlineOrders where OrderValue=" + Convert.ToInt32(OrderNumber));

            if (dt.Rows.Count > 0) { count = "1"; }
            else { count = "0"; }
            return count;
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdOnline.Columns["colRemove"].Index)
            {
                int itemId = Convert.ToInt32(grdOnline.Rows[e.RowIndex].Cells["colItemId"].Value);
                try
                {
                    grdOnline.Rows.RemoveAt(e.RowIndex);
                }
                catch (Exception ex)
                { }
            }
        }

        private void Clear()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int updateDB(string qry)
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
            //if (keyData == (Keys.Alt | Keys.I))
            //{
            //    this.uC_SearchItems1.Focus();
            //    return true;
            //}
            //if (keyData == (Keys.Alt | Keys.A))
            //{
            //    btnAddItem.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Alt | Keys.D))
            //{
            //    btnClear.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (grdOnline.SelectedRows != null && grdOnline.SelectedRows.Count > 0)
                {
                    int colIndex = grdOnline.Columns["colRemove"].Index;
                    int rowIndex = grdOnline.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdOnline, e);
                }
                return true;
            }
            //if (keyData == (Keys.Control | Keys.S))
            //{
            //    btnSave.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Control | Keys.D))
            //{
            //    btnClearAll.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.Control | Keys.F))
            {
                frmInvoice f = new frmInvoice();
                f.ShowDialog();
            }
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdOnline.Focus();
                if (grdOnline.Rows.Count >= 1)
                {
                    grdOnline.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (grdOnline.Rows.Count > 0)
                {
                    if (this.ActiveControl.Name == grdOnline.Name)
                    {
                        DataGridViewRow r = grdOnline.SelectedRows[0];
                        grdOnline.CurrentCell = grdOnline.SelectedRows[0].Cells["colReturnQuantity"];
                    }
                    else
                    {
                        if (grdOnline.Rows.Count > 0)
                        {
                            this.ActiveControl = grdOnline;
                            grdOnline.Rows[0].Selected = true;
                            grdOnline.CurrentCell = grdOnline.SelectedRows[0].Cells["colReturnQuantity"];
                            grdOnline.CurrentCell.Selected = true;
                        }
                    }
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.T))
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                r.Cells["colRate"].Value = rate;
            }
        }

        private void CustName1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CustName1.Text.Trim() != "")
                {
                    filter = FilterType.CustomerName;
                }
                else
                {
                    filter = FilterType.Default;
                }
                OrderChange.Text = "";
                txtPhone.Text = "";
                PageNo = 1;
                LoadOnlineIntegrateItems();
            }
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CustName1.Text.Trim() != "")
                {
                    filter = FilterType.Phone;
                }
                else
                {
                    filter = FilterType.Default;
                }
                OrderChange.Text = "";
                txtPhone.Text = "";
                PageNo = 1;
                LoadOnlineIntegrateItems();
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadOnlineIntegrateItems();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadOnlineIntegrateItems();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.GetOrders();
        }
        public void GetOrders()
        {
            this.Cursor = Cursors.WaitCursor;
            OrderOnlineAws Order = new OrderOnlineAws();
            List<OrderOnlineAws> checkItem = new List<OrderOnlineAws>();
            Order = obj.GetOrderItems();
            this.SaveOnlineIntegrate(Order);
            LoadOnlineIntegrateItems();
            this.Cursor = Cursors.Default;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void OrderChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void grdOnline_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                LoadOnlineIntegrateItems();
                if (e.RowIndex < 0) { return; }
                string ItemId = grdOnline.Rows[e.RowIndex].Cells["colItemId"].Value.ToString();
                string OrderValue = grdOnline.Rows[e.RowIndex].Cells["colOrderValue"].Value.ToString();
                string customerName = grdOnline.Rows[e.RowIndex].Cells["colCustName"].Value.ToString();
                string customerAddress = grdOnline.Rows[e.RowIndex].Cells["colAddress"].Value.ToString();
                string customerPhone = grdOnline.Rows[e.RowIndex].Cells["colPhone"].Value.ToString();
                string dispatch = grdOnline.Rows[e.RowIndex].Cells["IsDispatch"].Value.ToString();

                string ItemName = grdOnline.Rows[e.RowIndex].Cells["colItemName"].Value.ToString();
                int OrderId = Convert.ToInt32(grdOnline.Rows[e.RowIndex].Cells["colOrderId"].Value);
                string Pack = grdOnline.Rows[e.RowIndex].Cells["IsPack"].Value.ToString();
                string Cancel = grdOnline.Rows[e.RowIndex].Cells["Cancel"].Value.ToString();
                string Qty = grdOnline.Rows[e.RowIndex].Cells["colQtyDispatch"].Value.ToString();
                string InvoiceId;
                if (grdOnline.Rows[e.RowIndex].Cells["colInvoiceId"].Value == null)
                {
                    InvoiceId = "";
                }
                else
                {
                    InvoiceId = grdOnline.Rows[e.RowIndex].Cells["colInvoiceId"].Value.ToString();
                }

                if (e.ColumnIndex == grdOnline.Columns["colPack"].Index)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (Cancel == "False")
                    {
                        if (Pack == "False")
                        {
                            OnlineIntegrateVM objOnline = new OnlineIntegrateVM();
                            using (unitOfWork = new UnitOfWork())
                            {
                                objOnline = unitOfWork.OnlineIntegrateRepository.GetOnlineIntegrateById(OrderId);
                            }

                            frmInvoice f = new frmInvoice(true, Qty, ItemName, OrderId, Convert.ToInt32(OrderValue), objOnline);
                            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                            f.Show();
                            this.Cursor = Cursors.Default;
                        }
                        else
                        {
                            SharedFunctions.MessageBoxShow("Online Order is Packed Already", MessageType.Warning);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        SharedFunctions.MessageBoxShow("Online Order is Cancelled", MessageType.Warning);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                else if (e.ColumnIndex == grdOnline.Columns["colDispatch"].Index)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (Cancel == "False")
                    {
                        if (Pack == "True")
                        {
                            if (dispatch == "False")
                            {
                                var res = MessageBox.Show("Are you sure you want to dispatch this order.", "Please Make Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (res == System.Windows.Forms.DialogResult.Yes)
                                {
                                    //api call for the Msg
                                    obj.getCredential();
                                    chkRes = obj.SendDispatchMsg(customerName, customerAddress, customerPhone, OrderValue);
                                    if (chkRes.status == "success")
                                    {
                                        int j = updateDB("Update OnlineOrders set Dispatch =" + "1" + " where OrderId =" + OrderId);
                                        SharedFunctions.MessageBoxShow("Order Dispatched", MessageType.Success);
                                    }
                                    else
                                    {
                                        SharedFunctions.MessageBoxShow("Order has not Dispatched", MessageType.Warning);
                                    }
                                    this.Cursor = Cursors.Default;
                                }
                            }
                            else
                            {
                                SharedFunctions.MessageBoxShow("Order is Dispatched Already", MessageType.Warning);
                                this.Cursor = Cursors.Default;
                                return;
                            }
                        }
                        else
                        {
                            SharedFunctions.MessageBoxShow("Order must be first Packed", MessageType.Warning);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        SharedFunctions.MessageBoxShow("Order is Cancelled", MessageType.Warning);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    this.Cursor = Cursors.Default;
                }
                else if (e.ColumnIndex == grdOnline.Columns["colCancel"].Index)
                {
                    if (Pack == "True")
                    {
                        SharedFunctions.MessageBoxShow("Order is Packed", MessageType.Warning);
                        return;
                    }
                    else
                    {
                        if (Cancel == "True")
                        {
                            SharedFunctions.MessageBoxShow("Order is Cancelled Already", MessageType.Warning);
                            return;
                        }
                        else
                        {
                            frmOnlineCancel frm = new frmOnlineCancel(Convert.ToInt32(OrderValue), Convert.ToInt32(OrderId));
                            frm.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void frmOnlineIntegrate_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmOnlineIntegrate_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdOnline);
            this.WindowState = FormWindowState.Maximized;
            LoadOnlineIntegrateItems();
            SharedFunctions.SetLarggeButtonsStyle(new[] { button1});
        }

        private void frmOnlineIntegrate_VisibleChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            filter = FilterType.Default;
            OrderChange.Text = "";
            txtPhone.Text = "";
            CustName1.Text = "";
            LoadOnlineIntegrateItems();

        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            PageNo = 1;
        }
    }
}
