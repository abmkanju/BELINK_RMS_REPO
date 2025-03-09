using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PatientRptVM
    {
        public string InvoiceRefNo { get; set; }
        public string Description { get; set; }
        public string Paid { get; set; }
        public string Dues { get; set; }
        public string Advance { get; set; }
        public string Total { get; set; }
        public string PaymentDate { get; set; }
    }
}
