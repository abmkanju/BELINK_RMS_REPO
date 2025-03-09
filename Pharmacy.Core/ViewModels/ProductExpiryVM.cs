using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ProductExpiryVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public string LastTrDate { get; set; }
        public string BatchExp { get; set; }
        public int Quantity { get; set; }
        public int TotalStock { get; set; }
        public int ConsumedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int HoldStock { get; set; }
        public int AdjustedStock { get; set; }
    }
}
