using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AllItemsDiscVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public FlatDiscountVM DiscountData { get; set; }
        public double Disc { get; set; }
    }
}