using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class OnlineIntegrateVM
    {
        public long OrderId { get; set; }
        public string OrderValue { get; set; }
        public string Origin { get; set; }
        public double GrandTotal { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int DeliveryCharges { get; set; }
        public int DiscountType { get; set; }
        public string PaymentMethod { get; set; }
        public double SubTotal { get; set; }
        public int? InvoiceId { get; set; }
        public double VoucherAmount { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Pack { get; set; }
        public bool Dispatch { get; set; }
        public bool Delivered { get; set; }
        public bool Cancel { get; set; }
        public string CancelReason { get; set; }
        public IEnumerable<OnlineCustomerVM> OnlineCustomer { get; set; }
        public IEnumerable<OnlineItemVM> OnlineItem { get; set; }
        public string origin { get; set; }
        public double WalletAmount { get; set; }

    }
}
