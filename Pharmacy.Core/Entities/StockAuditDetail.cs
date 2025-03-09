using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class StockAuditDetail
    {
        public int StockAuditDetailId { get; set; }
        public Item Item { get; set; }

        public int Unit { get; set; }
        public int SystemQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public int Differnce { get; set; }
        public int CurrentAdjustedQuantity { get; set; }
        public int RetailPrice { get; set; }
        public double AmountDifference { get; set; }
        public StockAudit StockAudit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        

    }
}