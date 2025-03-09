using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class OnlineOrder
    {
        [Key]
        public long OrderId { get; set; }
        public string Origin { get; set; }
        public string OrderValue { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public double GrandTotal { get; set; }
        public string PaymentMethod { get; set; }
        public double SubTotal { get; set; }
        public int DeliveryCharges { get; set; }
        public double VoucherAmount { get; set; }
        public string VoucherNumber { get; set; }
        public string CancelReason { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Pack { get; set; }
        public bool Dispatch { get; set; }
        public bool Delivered { get; set; }
        public bool Cancel { get; set; }
        public ICollection<OnlineOrderItem> OnlineOrderItem { get; set; }
        public ICollection<OnlineCustomer> OnlineCustomer { get; set; }
        public Invoice Invoice { get; set; }
        public double WalletAmount { get; set; }
    }
}