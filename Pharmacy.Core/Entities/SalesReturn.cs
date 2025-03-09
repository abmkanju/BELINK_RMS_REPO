using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Core.Entities
{
    public class SalesReturn
    {
        [Key]
        public long SaleReturnId { get; set; }
        public long SaleReturnRefNo { get; set; }
        public double TotalDeductionValue { get; set; }
        public int TotalDeductionType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }
        public User User { get; set; }
        public Patient Patient { get; set; }
        public ICollection<SalesReturnItem> SalesReturnItem { get; set; }
    }
}
