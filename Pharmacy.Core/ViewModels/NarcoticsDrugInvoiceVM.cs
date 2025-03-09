using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class NarcoticsDrugInvoiceVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string InvoiceNoString { get; set; }
        public DateTime CreateDate { get; set; }
        public Patient Patient { get; set; }
        public double SubTotal { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public double GrandTotal { get; set; }
        public double Payment { get; set; }
        public double TotalPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateString { get; set; }
        public int ReturnedQuantity { get; set; }
        public double TotalRefund { get; set; }
        public DateTime RefundDate { get; set; }
        public string PatientName { get; set; }
        public int Quantity { get; set; }
        public double Value { get; set; }
        public IEnumerable<InvoiceItemVM> InvoiceItems { get; set; }
        public IEnumerable<InvoicePayment> InvoicePayments { get; set; }
        public string SupplierName { get; set; }
        public string NarcoticDrugs { get; set; }
        public string CreateBy { get; set; }

        
    }
}
