using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class DiscountedItemVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public string DiscountName { get; set; }
        public int DiscountType { get; set; }
        public string DiscTypeString { get; set; }
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public FlatDiscountVM DiscountData { get; set; }
    }
}