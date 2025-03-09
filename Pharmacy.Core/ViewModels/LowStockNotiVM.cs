using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class LowStockNotiVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ReorderingLevel { get; set; }
        public int TotalStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }

        /// <summary>
        /// AvailableStock = TotalStock + adjustedStock - consumedStock - expiredStock- ExpiredConsumedStock - (HoldStock) : hold stock is conditional
        /// </summary>
        public int AvailableStock { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
    }
}