using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PurchaseReportVM
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime StockDate { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public DateTime SupplierInvoiceDate { get; set; }
        public double Amount { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }
        public string SupplierName { get; set; }
        public Expense LastPayment{ get; set; }
        public DateTime PaymentDate { get; set; }
        public double SaleTax { get; set; }
        public double advancetax { get; set; }
        public double wht { get; set; }
        public string PaymentMode { get; set; }
    }
}
