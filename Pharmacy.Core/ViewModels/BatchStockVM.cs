using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;

namespace Pharmacy.Core.ViewModels
{
    public class BatchStockVM
    {
        public int? BatchId { get; set; }
        public string BatchName { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public int AvailableStockQty { get; set; }
        public int TotalStock { get; set; }
        public int BonusStock { get; set; }
        public int AvailableStock { get; set; }
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
        public DateTime? Expiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
        public int StockReturn { get; set; }
        public string StoreName { get; set; }
        public Supplier Suppliers { get; set; }
    }
}