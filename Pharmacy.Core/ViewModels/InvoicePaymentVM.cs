using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoicePaymentVM
    {
        public long InvoicePaymentId { get; set; }
        public InvoiceVM Invoice { get; set; }
        public int PaymentType { get; set; }
        public double Payment { get; set; }
        public DateTime PaymentDate { get; set; }
        public long ChequeInfoId { get; set; }
        public string RefundReason { get; set; }
        public string PaymentString { get; set; }
        public string PaymentTypeString { get; set; }
        public string ChequeNumber { get; set; }
        public string BankName { get; set; }
        public string ChequeStatus { get; set; }
        public string P_InvoiceNo { get; set; }
        public string P_RiderName { get; set; }
        public string P_VendorName { get; set; }
        //public ChequeInfo ChequeInfo { get; set; }
    }
}
