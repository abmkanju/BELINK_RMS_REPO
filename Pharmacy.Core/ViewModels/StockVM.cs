using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockVM
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public Supplier Supplier { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public DateTime SupplierIvoiceDate { get; set; }
        public string ImagePath { get; set; }
        public ICollection<StockItemVM> StockItems { get; set; }
        public bool IsAutoInsertedStock { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool Ishold { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public ICollection<StockPaymentVM> StockPayments { get; set; }
        public string SupplierName { get; set; }
        public string UserName { get; set; }

        public double TotalAmount { get; set; }
        public double? TotalPaid { get; set; }
        public double GrandTotal { get; set; }
        public double NetDiscount { get; set; } //-------- Discount--------//
        public int NetDiscountType { get; set; }
        public double TotalDiscount { get; set; }
        public double NetSaleTax { get; set; } //-------- Sales Tax----------//
        public int NetSaleTaxType { get; set; }
        public double TotalSaleTax { get; set; }
        public double TotalReturnStockAmount { get; set; }
        public double WithholdingTax { get; set; }
        public int WithholdingTaxType { get; set; }
        public double AdvanceWithholdingTax { get; set; }
        public int AdvanceWithholdingTaxType { get; set; }
    }
}
