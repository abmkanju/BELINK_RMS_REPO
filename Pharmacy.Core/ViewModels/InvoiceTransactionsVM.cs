using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoiceTransactionsVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string InvoiceNoString { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalAmount { get; set; }
        public double Payment { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public int ServiceType { get; set; }
        public List<InvoiceItemVM> InvoiceItems { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double Due { get; set; }
        public double Advance { get; set; }
        public Patient Patient { get; set; }
        public double Services { get; set; }
        public string PatientName { get; set; }
        public string MrNo { get; set; }
        public double TotalPaid { get; set; }
        public double TotalRefund { get; set; }
        public string ReasonForRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentCreateDate { get; set; }
        public string PaymentType { get; set; }
        public DateTime RefundDate { get; set; }
        public int SaleQuantity { get; set; }
        public int ReturnedQty { get; set; }
        public string Description { get; set; }
        public List<InvoicePayment> InvoicePayments { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsProcedureInvoice { get; set; }
        public bool IsOnlineIntegrate { get; set; }
        public List<double> TotalPayments { get; set; }
        public bool IsQInvoice { get; set; }
    }
}