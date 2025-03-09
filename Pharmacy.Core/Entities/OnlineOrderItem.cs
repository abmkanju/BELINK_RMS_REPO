using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class OnlineOrderItem
    {
        [Key]
        public long OrderItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }// 0/1 value for unit
        public string SizeOfStrip { get; set; }
        public double Rate { get; set; }
        public int ConversionUnit { get; set; }
        public double PerUnitCostPrice { get; set; }
        public double saletax { get; set; }
        public int saletaxType { get; set; }
        public double Amount { get; set; }
        public double NetAmount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
        public long UniqueItem_Id { get; set; }
        public OnlineOrder OnlineOrder { get; set; }
    }
}
