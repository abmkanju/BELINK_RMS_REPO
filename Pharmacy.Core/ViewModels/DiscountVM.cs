using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class DiscountVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double TotalPaid { get; set; }
        public double Discount { get; set; }
        public double ModifiedDiscount { get; set; }
        public int DiscountType { get; set; }
        public Patient Patient { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PatientName { get; set; }
        public decimal MrNo { get; set; }
        public string MrNoString { get; set; }
        public List<InvoiceItemVM> InvoiceItems { get; set; }
        public string Description { get; set; }
    }
}
