using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class PurchaseOrderVM
    {
        public string SupplierName { get; set; }
        public long PurchaseOrderId { get; set; }
        public decimal? PurchaseOrderNo { get; set; }
        public long SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public long StockId { get; set; }
        public IEnumerable<PurchaseOrderItemVM> PurchaseOrderItems { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<Manufacturer> Manufecturers { get; set; }
    }
}