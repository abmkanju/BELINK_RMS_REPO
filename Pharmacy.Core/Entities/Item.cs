using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Pharmacy.Core.Entities
{
    public class Item
    {
        public int ItemID { get; set; }
        [StringLength(600)]
        public string ItemName { get; set; }
        [StringLength(600)]
        public string Barcode { get; set; }
        public Rack Rack { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
        public int NoOfStrips { get; set; }
        public int ReOrderingLevel { get; set; }
        public double RetailPrice { get; set; }
        public int OpeningStock { get; set; }
        public string ChecmicalName { get; set; }
        public double UnitCostPrice { get; set; }
        public bool IsNarcotic { get; set; }
        public long MappedCategoryId { get; set; } // just for data transfre and mapping purpose
        public long MappedProductId { get; set; }  // just for data transfre and mapping purpose
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultLoaded { get; set; }
        public bool IsSyncable { get; set; }
        public bool IsFileItem { get; set; } // this flag is used for the syncing purpose
        public string PCTCode { get; set; }
        public int Unique_itemId { get; set; }
        public Category Category { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public User User { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }

        public bool IsActive { get; set; }

        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public int IsApproved { get; set; }
        [NotMapped]
        public long CategoryId { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public string SupplierIds { get; set; }
        [NotMapped]
        public string SupplierNames { get; set; }
        [NotMapped]
        public int ManufacturerId { get; set; }
        [NotMapped]
        public string ManufacturerName { get; set; }
        [NotMapped]
        public int RackId { get; set; }
        [NotMapped]
        public string RackName { get; set; }
        [NotMapped]
        public string RackNo { get; set; }
        [NotMapped]
        public long RequestById { get; set; }
        [NotMapped]
        public long DefaultItemId { get; set; }
    }
}