using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemWiseSaleVM
    {
        public int ItemId { get; set; }
        public long InvoiceId { get; set; }
        public string ItemName { get; set; }
        public int SaleCount { get; set; }
        public int ReturnedQuantity { get; set; }
        public double SaleRevenue { get; set; }
        public long GrandSalesTotal { get; set; }


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