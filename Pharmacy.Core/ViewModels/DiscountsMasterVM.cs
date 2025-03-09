using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Pharmacy.Core.ViewModels
{
    public class DiscountsMasterVM
    {
        public double TotalDiscount { get; set; }
        public IPagedList<DiscountVM> Discounts { get; set; }
    }
}
