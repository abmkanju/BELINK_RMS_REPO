using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockReturnItemVM
    {
        public long StockReturnItemId { get; set; }
        public int Quantity { get; set; }
        public int ReturnQuantity { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public double UnitCost { get; set; }
        public double Discount { get; set; } //---- Discount
        public int DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public double SalesTax { get; set; } //---- Sales
        public int SalesTaxType { get; set; }
        public double SalesTaxValue { get; set; }
        public double NetValue { get; set; }
        public int TotalStock { get; set; } //----- Calculate Stock
        public int BonusStock { get; set; }
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }
        public int StockConsumed { get; set; }
        public int StockReturn { get; set; }
        public string BatchName { get; set; } //--- For Reporting
        public string ItemName { get; set; }
        public string UnitString { get; set; }
        public int ConversionUnit { get; set; }
        public int ReturnAvailableQty { get; set; }

    }
}
