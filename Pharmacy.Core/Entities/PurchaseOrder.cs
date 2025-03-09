using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class PurchaseOrder
    {
        public long PurchaseOrderId { get; set; }
        public decimal PurchaseOrderNo { get; set; }
        public decimal TotalAmount { get; set; }
        public long StockId { get; set; } // this for easness to know if stock exists against given purchase order or not
        public DateTime OrderDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<Manufacturer> Manufecturers { get; set; }
        public User User { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        [NotMapped]
        public string SupplierName { get; set; }
        [NotMapped]
        public long SupplierId { get; set; }
    }
}