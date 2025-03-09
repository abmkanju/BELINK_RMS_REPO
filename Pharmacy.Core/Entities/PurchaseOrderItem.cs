using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class PurchaseOrderItem
    {
        public long PurchaseOrderItemId { get; set; }//primary key
        public int Unit { get; set; }
        
        public decimal PurchaseOrderNo { get; set; }//just for refernce to avoi join
        public int Quantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }

        public Item Item { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        
        [NotMapped]
        public int ItemID { get; set; }      
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
        public double Discount { get; set; }

        public int DiscountType { get; set; }

        public double GrossAmount { get; set; }
        public int Bonus { get; set; }
        public Manufacturer Manufecturer { get; set; }



    }
}