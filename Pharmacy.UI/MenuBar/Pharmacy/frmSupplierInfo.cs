using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmSupplierInfo : Form
    {
        private UnitOfWork unitOfWork;
        long SupplierId = 0;
        public frmSupplierInfo()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public frmSupplierInfo(long SupplierId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.SupplierId = SupplierId;
        }
        private void frmSupplierInfo_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
            SharedFunctions.SetGridStyle(grdBills);
            SharedFunctions.SetGridStyle(grdItems);
            SharedFunctions.SetGridStyle(grdPayments);
            SharedFunctions.SetGridStyle(grdStocks);
        }

        private void LoadSupplierData()
        {
            SupplierDetailsVM sd = unitOfWork.SupplierRepository.GetSupplierWithDataDetails(this.SupplierId);
            lblName.Text = sd.Supplier.Name;
            lblPhone.Text = sd.Supplier.Phone;
            lblAddress.Text = sd.Supplier.Address;
            lblntn.Text = sd.Supplier.NTN;
            lblstn.Text = sd.Supplier.STN;
            lblcreditlimit.Text = sd.Supplier.CreditLImit.ToString();
            string PaymentMode = "Cash";
            grdStocks.Rows.Clear();
            grdPayments.Rows.Clear();
            grdStocks.Rows.Clear();
            grdItems.Rows.Clear();
            List<Item> ItemList = new List<Item>();
            if (sd.Stocks != null)
            {
                List<Item> StockItems =  sd.Stocks.Where(x => x.StockItems != null).SelectMany(x=>x.StockItems.Select(k=>k.Item)).Distinct().ToList();
                ItemList.AddRange(StockItems);
            }
            foreach (Item i in ItemList)
            {
                if (i.IsActive)
                {
                    grdItems.Rows.Add(i.ItemID, i.ItemName, i.IsNarcotic ? "Narcotic" : "Drug", i.CreateDate);
                }
            }
            string itemsString = "";
            double NetValue = 0;
            foreach (StockVM s in sd.Stocks)
            {
                itemsString = "";
                NetValue = 0;
                foreach (StockItemVM si in s.StockItems)
                {
                    itemsString += si.Item.ItemName + ", ";
                    NetValue += si.TotalCost;
                }
                if (!string.IsNullOrEmpty(itemsString))
                {
                    itemsString = itemsString.Trim().TrimEnd(',');
                }
                grdStocks.Rows.Add(s.StockId, Math.Round(s.DocumentNo, 0), s.CreateDate, itemsString, NetValue);
                if (s.TotalPaid < s.TotalAmount)
                {
                    grdBills.Rows.Add(s.StockId, Math.Round(s.DocumentNo, 0), s.CreateDate, s.SupplierInvoiceNo, s.SupplierIvoiceDate, s.TotalAmount - s.TotalReturnStockAmount, s.TotalAmount - s.TotalPaid - s.TotalReturnStockAmount);
                }
                foreach (StockPaymentVM p in s.StockPayments)
                {
                    if (p.PaymentType == 1) { PaymentMode = "Cash"; }
                    else
                        if (p.PaymentType == 2) { PaymentMode = "Cheque"; }
                        else
                            if (p.PaymentType == 3) { PaymentMode = "Debit/Credit Card"; }
                            else
                                if (p.PaymentType == 4) { PaymentMode = "Online Payment"; }
                    grdPayments.Rows.Add(s.StockId, Math.Round(s.DocumentNo, 0), s.CreateDate, "Stock Payment" + Environment.NewLine + s.DocumentNo.ToString(), PaymentMode, p.Payment);
                }
            }

        }

        private void grdBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdBills.Columns["colPayment"].Index)
            {
                frmStockPayments f = new frmStockPayments(this.SupplierId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
            }
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void grdPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grdItems.Rows.Clear();
            grdBills.Rows.Clear();
            grdPayments.Rows.Clear();
            LoadSupplierData();
        }
    }
}
