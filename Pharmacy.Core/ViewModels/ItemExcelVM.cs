using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemExcelVM
    {
        public string Barcode { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public  string Manufacturer { get; set; }
        public string GenericName { get; set; }
        public string Supplier { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
        public int ConUnit { get; set; }
        public int Quantity { get; set; }
        public bool Narcotic { get; set; }
    }
}
