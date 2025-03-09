using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemWiseRevenueMasterVM
    {
        public double TotalRevenue { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int PageCount { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public IPagedList<ItemWiseRevenueVM> ItemWiseRevenueList { get; set; }
        public ICollection<ItemWiseRevenueVM> ItemWiseRevenue { get; set; }
    }
}