using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockItemVM
    {
        public long StockItemId { get; set; }
        public Item Item { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public string UnitString { get; set; } // string value of unit 0:specified with item, 1 : units
        public int ConversionUnit { get; set; }
        public int Quantity { get; set; }
        public int BonusQuantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public double RetailPrice { get; set; }
        public Batch Batch { get; set; }
        public double BonusDiscount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public int PercDiscType { get; set; }
        public double SalesTax { get; set; }
        public int SalesTaxType { get; set; }
        public int PercSalesTaxType { get; set; }
        public double NetValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public Stock Stock { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public int StockConsumed { get; set; }
        public double DiscountVal { get; set; }
        public double SalesTaxVal { get; set; }

        public long StockId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchName { get; set; }
        public DateTime BatchExpiry { get; set; }
        public string DiscoutnTypeString { get; set; }
        public string SalesTaxTypeString { get; set; }
        public int StockReturn { get; set; } //----------- Return Stock
        public int TotalStock { get; set; } //----------- For Available Stock
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }
        public string WHT { get; set; }
        public int WHTType { get; set; }
        public string AdWHT { get; set; }
        public string AdvanceWithholdingTax { get; set; }
        public int AdWHTType { get; set; }
        public Store Store { get; set; }
        public double DiscountedPrice { get; set; }
    }
}
