using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Pharmacy.Core.ViewModels
{
    public class ItemWiseSalesMasterVM
    {
        public long? GrandSalesTotalQuantity { get; set; }
        public double GrandSaleRevenue { get; set; }
        public IPagedList<ItemWiseSaleVM> ItemWiseSales { get; set; }
    }
}