using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoiceItemVM
    {
        public long InvoiceItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public long BatchId { get; set; } // long was nullable()long, but removed as nullable
        public string BatchName { get; set; }
        public int ConUnit { get; set; }
        public string BatchExpiry { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public double DiscountedPrice { get; set; }
        public string Rack { get; set; }
        public double Rate { get; set; }
        public double ProfitMargin { get; set; }
        public int Quantity { get; set; }
        public int BonusQuantity { get; set; }
        public double Amount { get; set; }
        public double NetAmount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public string SaleTax { get; set; }
        public int SaleTaxType { get; set; }
        public string Description { get; set; }
        public int ReturnedQuantity { get; set; }
        public bool IsActive { get; set; }
        public Procedure Procedure { get; set; }
        public string ProcedureDescription { get; set; }
        public string DiscountTypeString { get; set; }
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public double Revenue { get; set; }
        public double UnitCost { get; set; }
        public bool AffectStock { get; set; }
        public List<string> SupplierNameList { get; set; }
        public int Unit { get; set; }
        public string UnitString { get; set; }
        public bool IsOptionalBatchNo { get; set; }
        public string DiscString { get; set; }
        public int TotalStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }
        public string ManufacturerName { get; set; }
        public long InvoiceID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string InvoiceType { get;  set; }

    }
}