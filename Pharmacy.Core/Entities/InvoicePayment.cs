using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class InvoicePayment
    {
        public long InvoicePaymentId { get; set; }
        public int PaymentType { get; set; }
        //If you need to change the Payment Column then you also change Staff UI Query
        public double Payment { get; set; }
        public string RefundReason { get; set; }
        public string ChequeNumber { get; set; }
        public string BankName { get; set; }
        public string ChequeStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt{ get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public double Dues { get; set; }
        public int MethodType { get; set; }

        public Invoice Invoice { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced{ get; set; }
        public bool IsQInvoice { get; set; }

        [NotMapped]
        public long StockId { get; set; }
        [NotMapped]
        public string PaymentString { get; set; }
        public string P_InvoiceNo { get; set; }
        public string P_RiderName { get; set; }
        public string P_VendorName { get; set; }
    }
}