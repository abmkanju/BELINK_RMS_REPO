using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ExpiryVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StockAdditionDate { get; set; }
        public double TotalCost { get; set; }
        public double TotalRetailValue { get; set; }
    }
}
