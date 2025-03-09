using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class XRptVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRef { get; set; }
        public DateTime TrTime { get; set; }
        public double Amount { get; set; }
    }
}
