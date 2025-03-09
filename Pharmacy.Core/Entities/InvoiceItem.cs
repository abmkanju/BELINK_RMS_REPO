using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class InvoiceItem
    {
        public long InvoiceItemId { get; set; }
        public string ProcedureDescription { get; set; }
        public int Unit { get; set; }// 0/1 value for unit
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public int BonusQuantity { get; set; }
        public double Amount { get; set; }
        public double NetAmount { get; set; }
        public double Discount { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int DiscountType { get; set; }

        public int ReturnedQuantity { get; set; }
        public double TotalCostPrice { get; set; }
        public double TotalDiscPrice { get; set; }
        public double PerUnitCostPrice { get; set; }
        public int SerialNo { get; set; }
        public bool AffectStock { get; set; } // this will bes used in case of item return on invoice screen.
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }

        public Invoice Invoice { get; set; }
        public Item Item { get; set; }
        public Category Category { get; set; }
        public Procedure Procedure { get; set; }
        public Batch Batch { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public bool IsOptionalBatch { get; set; }
        [NotMapped]
        public long ProcedureId { get; set; }
        [NotMapped]
        public string ProcedureName { get; set; }
        [NotMapped]        
        public int ItemId { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public int BatchId { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public string DiscountTypeString { get; set; }
        [NotMapped]
        public string UnitString { get; set; } // string value for unit : item unit 0 /broken(units) 1
        [NotMapped]
        public int ConvUnit { get; set; }
        public double saletax { get; set; }
        public int saletaxType { get; set; }
        public bool IsQInvoice { get; set; }
    }
}