using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PurchaseOrderItemVM
    {
        public long PurchaseOrderItemId { get; set; }
        public decimal PurchaseOrderNo { get; set; }
        public int ItemID { get; set; }       
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalAmount { get; set; }
        public int Unit { get; set; }
        public int ConversionUnit { get; set; }
        public long SupplierId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public double Discount { get; set; }

        public int DiscountType { get; set; }

        public double GrossAmount { get; set; }
        public int Bonus { get; set; }
        public string Manuf { get; set; }
    }
}