using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class DeleteInvoiceVM
    {
        public long InvoiceId { get; set; }
        public string InvoiceRefNo { get; set; }
        public string PatientName { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public double GrandTotal { get; set; }
        public double Due { get; set; }
        public double Advance { get; set; }
        public double TotalPaid { get; set; }
        public List<string> ItemName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DeletedAt { get; set; }

    }
}
