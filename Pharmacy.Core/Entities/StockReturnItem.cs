using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class StockReturnItem
    {
        public long StockReturnItemId { get; set; }
        public int Quantity { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public double UnitCost { get; set; }
        public double Discount { get; set; } //---- Discount
        public int DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public double SalesTax { get; set; } //---- Sales
        public int SalesTaxType { get; set; }
        public double SalesTaxValue { get; set; }
        public double NetValue { get; set; }
        public StockReturn StockReturn { get; set; }
    }
}
