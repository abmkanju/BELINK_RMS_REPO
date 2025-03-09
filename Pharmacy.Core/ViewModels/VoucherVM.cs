using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class VoucherVM
    {
        public long OrderVoucherId { get; set; }
        public string VoucherNumber { get; set; }
        public double VoucherValue { get; set; }
        public double GrandTotal { get; set; }
        public double SubTotal { get; set; }
        public double Due { get; set; }
        public bool IsVoucherPaid { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public double VoucherAmount { get; set; }
        public double WalletCredit { get; set; }
    }
}
