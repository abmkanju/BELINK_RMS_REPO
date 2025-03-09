using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PatientInvoiceVM
    {
        public long InvoiceId { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double Due { get; set; }
        public double Advance { get; set; }
        public Patient Patient { get; set; }
        public double TotalPaid { get; set; }
        public double TotalRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RefundDate { get; set; }

        public IEnumerable<InvoiceItemVM> InvoiceItems { get; set; }
        public IEnumerable<InvoicePayment> InvoicePayments { get; set; }
    }
}
