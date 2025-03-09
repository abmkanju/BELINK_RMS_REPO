using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
namespace Pharmacy.Core.ViewModels
{
    public class StockAddFilterVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Manufacturer Manufecturer { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
        public double UnitCost { get; set; }
        public double RetailPrice { get; set; }
        public int AvailableStock { get; set; }
        public int TotalStock { get; set; }
        public int ConsumedStock { get; set; }
        public int AdjustedStock { get; set; }
        /// <summary>
        /// sum of total stock that has been expired
        /// </summary>
        public int ExpiredStock { get; set; }
        /// <summary>
        /// sum of consumed stock whom batches expired now
        /// </summary>
        public int ExpiredConsStock { get; set; }
        /// <summary>
        /// this contains stock consumption quantity for on hold invoices
        /// </summary>
        public int HoldStock { get; set; }
        public int Quantity { get; set; }
        public int StockReturn  { get; set; }
        public int BonusStock { get; set; }
        public List<int> BatchId { get; set; }
        public long InvoiceId { get; set; }
        public List<long> SupplierIds { get; set; }
        public int ConsumedStock1 { get; set; }
        public long InvoiceItemId { get; set; }
        public int InvoiceItemQty { get; set; }
        public double PerUnitCostPrice { get; set; }
    }
}