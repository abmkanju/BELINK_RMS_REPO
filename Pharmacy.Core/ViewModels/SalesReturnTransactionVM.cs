using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class SalesReturnTransactionVM
    {
        public long SaleReturnId { get; set; }
        public long SaleReturnRefNo { get; set; }
        public double TotalDeductionValue { get; set; }
        public int TotalDeductionType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<SalesReturnItemVM> SalesReturnItem { get; set; }
        public string PatientName { get; set; }
        public decimal? MRNo { get; set; }
        public Patient Patient { get; set; }
    }
}
