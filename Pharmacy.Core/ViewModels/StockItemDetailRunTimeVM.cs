using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockItemDetailRunTimeVM
    {
        public double ItemId { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public int ConversionUnit { get; set; }
        public int TotalStock  { get; set; } 
        public int ConsumedStock  { get; set; } 
        public int ExpiredStock { get; set; }
        public int AdjustedStock  { get; set; }                                           
        public int AvailableQty { get; set; }            
    }
}