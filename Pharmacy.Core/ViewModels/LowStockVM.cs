using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class LowStockVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int  ReOrderingLevel { get; set; }
        public int ItemsLeft { get; set; }
        public int TotalStock { get; set; }
        public int ConsumedStock { get; set; }
        public string Category { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public DateTime? LastStockAddedDate { get; set; }
        public string LastStockAdded { get; set; }    
    }

    public class LowStockRptVM
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string LastStockAdded { get; set; }
        public int ReOrderingLevel { get; set; }
        public int ItemsLeft { get; set; }
    }
}