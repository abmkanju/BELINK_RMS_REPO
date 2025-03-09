using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class FinancialTransactionsVM
    {
        public long InvoiceId { get; set; }
        public List<InvoiceItemVM> InvoiceItems { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double Due { get; set; }
        public double Advance { get; set; }
        public Patient Patient { get; set; }
        public string PatientName { get; set; }
        public double TotalPaid { get; set; }
        public double TotalRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RefundDate { get; set; }
        public int SaleQuantity { get; set; }
        public int ReturnedQty { get; set; }
        public string Description { get; set; }
    }
}
