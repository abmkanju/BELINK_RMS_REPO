using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;

namespace Pharmacy.Core.ViewModels
{
    public class ItemDetailVM
    {
        public Item Item { get; set; }
        public int AvailableStock { get; set; }
        public double RetailPrice { get; set; }
        public ICollection<BatchStockVM> BatchStockList { get; set; }
    }
}
