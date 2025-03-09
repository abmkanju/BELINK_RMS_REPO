using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class StockItem
    {
        public long StockItemId { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }
        public int BonusQuantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public double RetailPrice { get; set; }
        public double BonusDiscount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; } // % : value
        public int PercDiscType { get; set; } //lumpsum  : per item
        public double SalesTax { get; set; }
        public int SalesTaxType { get; set; }
        public int PercSalesTaxType { get; set; }
        public double NetValue { get; set; }
        public double DiscountValue { get; set; }
        public double SalesTaxValue { get; set; }
        public StockConsumptionItem StockConsumptionItem { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }

        public Store Store { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public Stock Stock { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }        
        [NotMapped]
        public long StockId { get; set; }
        [NotMapped]
        public int ItemId { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public object BatchExpiry { get; set; }
        [NotMapped]
        public string DiscoutnTypeString { get; set; }
        [NotMapped]
        public string SalesTaxTypeString { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
        public int WHTType { get; set; }
        public string WHT { get; set; }
        public int AdvanceWHTType { get; set; }
        public string AdvanceWHT { get; set; }
        public double DiscountedPrice { get; set; }
    }
}
