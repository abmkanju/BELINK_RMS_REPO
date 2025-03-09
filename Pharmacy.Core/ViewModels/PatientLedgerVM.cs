using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PatientLedgerVM
    {
        public string PatientName { get; set; }
        public long InvoiceRefNo { get; set; }
        public string Description { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public DateTime PaymentDate { get; set; }
        public double SalesTax { get; set; }
        public double GrandTotal { get; set; }
        public double TotalPaid { get; set; }
        public double GrossTotal { get; set; }
        public double Due { get; set; }
        public double Refund { get; set; }
        public int SaleQuantity { get; set; }
        public double ReturnedQty { get; set; }
        public double RemainingTotal { get; set; }
        public Patient Patient { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public IEnumerable<InvoiceItemVM> InvoiceItemsV { get; set; }
        
    }

    public class PatientLedgerSumVM
    {
        public double invoiceid { get; set; }
        public double GrossTotal { get; set; }
        public double TotalPaid { get; set; }
        public double TotalDue { get; set; }
        public double TotalRefund { get; set; }
    }
}
