using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class SupplierItemsVM
    {
        public long SupplierId { get; set; }
        public List<ItemsComboVM> Items { get; set; }
    }
}
