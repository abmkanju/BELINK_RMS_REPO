using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class InvoicePayment
    {
        public long InvoicePaymentId { get; set; }
        public Invoice Invoice { get; set; }
        public int PaymentType { get; set; }
        public double Payment { get; set; }
        public ChequeInfo ChequeInfo { get; set; }
    }
}
