using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Invoice
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string FBRInvoiceNo { get; set; }
        public string FBRCode { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double ModifiedDiscount { get; set; }
        public int DiscountType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double TotalPaid { get; set; }
        public double TotalUnitCost { get; set; }
        public double Due { get; set; }
        public double Services { get; set; }
        public int ServiceType { get; set; }
        public string Note { get; set; }
        public bool IsProcedureInvoice { get; set; }
        public string InvoiceNote { get; set; }
        public bool IsHoldInvoice { get; set; }
        public Patient Patient{ get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }
        public DateTime? DeletedAt { get; set; }        

        public StockConsumption StockConsumption { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        public ICollection<InvoicePayment> InvoicePayments { get; set; }
        //public ICollection<StockConsumption> StockConsumptions { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public bool IsQInvoice { get; set; }
        public string VoucherName { get; set; }
        public bool IsVoucherPaid { get; set; }
        public bool IsOnlineIntegrate { get; set; }
    }
}