using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AvailableQtyVM
    {
        public int ItemId { get; set; }
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int HoldStock { get; set; }
        public int StockReturn { get; set; }
        public int BonusStock { get; set; }
    }
}
