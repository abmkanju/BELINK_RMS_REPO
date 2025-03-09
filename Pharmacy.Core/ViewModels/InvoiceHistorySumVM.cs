using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoiceHistorySumVM
    {
        public long TotalInvoice { get; set; }
        public double TotalPaid { get; set; }
        public double TotalDue { get; set; }
        public double TotalAdvance { get; set; }
        public double TotalRefund { get; set; }

        public int TotalQty { get; set; }
        public int TotalReturnQty { get; set; }
    }
}
