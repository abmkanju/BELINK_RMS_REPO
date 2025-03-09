using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;

namespace Pharmacy.Core.ViewModels
{
    public class SupplierDetailsVM
    {
        public Supplier Supplier { get; set; }
        public List<Item> Items { get; set; }
        public List<StockVM> Stocks { get; set; }
        public List<StockBillVM> StockBills { get; set; }
        public List<StockPaymentVM> StockPayments{ get; set; }
    }
}
