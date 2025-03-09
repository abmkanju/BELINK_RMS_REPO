using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemsVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string PCTCode { get; set; }
        public string Barcode { get; set; }
        public string Suppliers { get; set; }
        public string ManufacturerName { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public List<Supplier> SuppliersList { get; set; }
        public int ReorderingLevel { get; set; }
        public double RetailPrice { get; set; }
        public double UnitCost { get; set; }
        public int ItemQuantity { get; set; }
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int BonusStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }
        public int StockReturn { get; set; }
        public int UnitValue { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
        public int NoOfStrips { get; set; }
        public bool IsNarcotic { get; set; }
        public string ItemType { get; set; }
        public string RackNo { get; set; }
        public string chemicalname { get; set; }
        public double Discount { get; set; }
        public int Bonus { get; set; }
        public int DiscountType { get; set; }

    }
    public class StockStaticsVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string GenericName { get; set; }
        public string Suppliers { get; set; }
        public Manufacturer Manufecturers { get; set; }
        public string Racks { get; set; }
        public int TotalStock { get; set; }
        public int TotalAdjustment { get; set; }
        public int totalstock { get; set; }
        public int TotalExpired { get; set; }
        public int TotalConsumed { get; set; }
        public int AvailableStock { get; set; }
        public int ExpiredStock { get; set; }
        public double UnitCost { get; set; }
        public double RetailPrice { get; set; }
        public int CostofAvailableQty { get; set; }
        public int RetailofAvailableQty { get; set; }
        public List<Supplier> SuppliersList { get; set; }
        public int HoldStock { get; set; }
        public int StockReturn { get; set; }
        public int BonusStock { get; set; }
        public int ExpiredConsumedStock { get; set; }


        
    }

    public class JsonItemsVM
    {
        public JsonItemsVM()
        {
            Category = new Category();
            Manufacturer = new Manufacturer();
            Suppliers = new List<Supplier>();
        }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Barcode { get; set; }
        public Rack Rack { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
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

        public Category Category { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public User User { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public bool IsActive { get; set; }

        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public string RackNo { get; set; }

    }
}