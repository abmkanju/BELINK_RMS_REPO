using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockAuditDetailVM
    {
        public int StockAuditDetailId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public int AuditUnit { get; set; }
        public int ConversionUnit { get; set; }


        // available stock attributes.....
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }


        public int SystemQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public int Differnce { get; set; }
        public int CurrentAdjustedQuantity { get; set; }
        public int RetailPrice { get; set; }
        public double RetailPrice1 { get; set; }
        public double UnitCost { get; set; }
        public double AmountDifference { get; set; }
        public StockAudit StockAudit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}
