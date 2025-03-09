using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class Item_SuppliersVM
    {
        public int ItemId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
