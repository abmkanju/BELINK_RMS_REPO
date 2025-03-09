using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Pharmacy.Core.ViewModels
{
    public class ProfitStatementMasterVM
    {
        public double TotalSales { get; set; }
        public double NetTotalSales { get; set; }
        public double TotalCostOfSales { get; set; }
        public double DiscountedCostofSales { get; set; }
        public double TotalDiscount { get; set; }
        public double ProfitMargin { get; set; }
        public double TotalRevenue { get; set; }
        public double Profit { get; set; }
        public IPagedList<ProfitStatementVM> ProfitStatementPaged { get; set; }
    }
}