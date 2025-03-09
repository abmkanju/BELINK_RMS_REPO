using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class BulkItemsVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        /// <summary>
        /// in db its saved as ChecmicalNAme
        /// </summary>

        public string Unit { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string Barcode { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
        public int TotalStock { get; set; }
        public int ExpiredStock { get; set; }
        public int AdjustedStock { get; set; }
        public int BonusStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int HoldStock { get; set; }
        public int ConsumedStock { get; set; }
        public int ConversionUnit { get; set; }
        public int AvailableStock { get; set; }
        public int StockReturn { get; set; }
        public string RackName { get; set; }
        public List<string> SupplierNames { get; set; }
    }
}
