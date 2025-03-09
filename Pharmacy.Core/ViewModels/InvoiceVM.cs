using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoiceVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string FBRInvoiceNo { get; set; }
        public string FBRCode { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double ModifiedDiscount { get; set; }
        public int DiscountType { get; set; }
        public int ServiceType { get; set; }
        public double ServiceCheck { get; set; }

        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double FirstPayment { get; set; }
        public double Due { get; set; }
        public double CashBack { get; set; }
        public double Balance { get; set; }
        public double Advance { get; set; }
        public PatientVM Patient { get; set; }
        public Patient ObjPatient { get; set; }
        public double TotalPaid { get; set; }
        public double TotalPaidAmount { get; set; }
        public double DiscountedPrice { get; set; }

        public int PaymentType { get; set; }
        public double TotalRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RefundDate { get; set; }
        public string PatientName { get; set; }
        public string NarcoticDrugs { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public double Services { get; set; }
        public IEnumerable<InvoiceItemVM> InvoiceItems { get; set; }
        public IEnumerable<InvoicePaymentVM> InvoicePayments { get; set; }
        public List<InvoicePayment> ObjInvoicePayments { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public string InvoiceType { get; set; }
        public string VoucherName { get; set; }
        public bool IsVoucherPaid { get; set; }
    }
}
