using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class CategoryVM
    {
        public long CategoryId { get; set; }
        public List<ItemsVM> Items { get; set; }
    }
}
