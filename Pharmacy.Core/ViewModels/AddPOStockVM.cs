using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AddPOStockVM
    {
        public long SupId { get; set; }
        public List<ItemsVM> Items { get; set; }
    }
}
