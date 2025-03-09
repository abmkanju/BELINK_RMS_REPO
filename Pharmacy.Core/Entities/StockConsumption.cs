using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Core.Entities
{
    public class StockConsumption
    {
        public long StockConsumptionId { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }

        public User User { get; set; }
        public List<StockConsumptionItem> StockConsumptions { get; set; }

        public long InvoiceId { get; set; } // for syncing purpose :  this was required for live
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}