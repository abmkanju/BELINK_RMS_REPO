using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class StockConsumptionItem
    {
        public long StockConsumptionItemId { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }
        public int ConsumptionType { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public Store Store { get; set; }
        public StockConsumption StockConsumption { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public User User { get; set; }

        public long InvoiceId { get; set; } // for syncing purpose :  this was required for live
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }

        [NotMapped]
        public int ItemId { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public long BatchId { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
         [NotMapped]
        public double UnitCost { get; set; }
         [NotMapped]
        public double DiscountPrice { get; set; }
        [NotMapped]
        public string ConsumptionTypeString { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
    }
}
