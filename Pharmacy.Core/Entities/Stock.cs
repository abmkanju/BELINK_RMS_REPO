using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Stock
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public string ImagePath { get; set; }
        public double GrandTotal { get; set; }
        public double NetDiscount { get; set; } //-------- Discount--------//
        public int NetDiscountType { get; set; }
        public double TotalDiscount { get; set; }
        public double NetSaleTax { get; set; } //-------- Sales Tax----------//
        public int NetSaleTaxType { get; set; }
        public double TotalSaleTax { get; set; } 
        public double TotalPaid { get; set; }
        /// <summary>
        /// its value is being used for syncing purpose
        /// true : if default added stock not need to be synced
        /// false : if default added stock need to 
        /// </summary>
        public bool IsAutoInsertedStock { get; set; }
        public DateTime SupplierIvoiceDate { get; set; }
        public DateTime StockDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
        public Supplier Supplier { get; set; }
        public User User { get; set; }
        /// <summary>
        /// Here InvoicePayments denote Expenses
        /// </summary>
        public ICollection<Expense> InvoicePayments { get; set; }
        public ICollection<StockItem> StockItems { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsSynced { get; set; }
        public bool IsUpdate { get; set; }
        public bool Ishold { get; set; }

        
        [NotMapped]
        public string SupplierName { get; set; }

        public double WithholdingTax { get; set; }
        public int WithholdingTaxType { get; set; }
        public double AdvanceWithholdingTax { get; set; }
        public int AdvanceWithholdingTaxType { get; set; }
        public int IsHolding { get; set; }
    }
}