using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemWisePurchaseVM
    {
        public int ItemId { get; set; }
        public long StockId { get; set; }
        public string ItemName { get; set; }
        public int PurchaseCount { get; set; }
        public int Quantity { get; set; }        
        public long GrandPurchaseTotal { get; set; }



        // calculating available stock variables
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
