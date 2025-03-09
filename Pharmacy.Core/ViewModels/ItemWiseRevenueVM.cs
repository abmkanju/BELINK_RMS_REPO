using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemWiseRevenueVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double Revenue { get; set; }
        public int SoldQuantity { get; set; }
        // calculating available stock variables
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int HoldStock { get; set; }

    }
}
