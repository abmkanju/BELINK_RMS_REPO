using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StocksReportVM
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public DateTime SupplierIvoiceDate { get; set; }
        public string ImagePath { get; set; }        
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }        
        public string SupplierName { get; set; }
        public double TotalAmount { get; set; }
        public double TotalPaid { get; set; }
        //----------- stock items Variables--------------//
        public long StockItemId { get; set; }        
        public int Quantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public double RetailPrice { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public double SalesTax { get; set; }
        public int SalesTaxType { get; set; }
        public double NetValue { get; set; }
        public DateTime ItemCreatedAt { get; set; }
        public DateTime ItemUpdatedAt { get; set; }    
        public string ItemName { get; set; }
        public string BatchName { get; set; }
        public DateTime BatchExpiry { get; set; }
        public string DiscoutnTypeString { get; set; }
        public string SalesTaxTypeString { get; set; }
        public int ReturnQuantity { get; set; }
        
        public string WHT { get; set; }
        public int WHTType { get; set; }
    }
}
