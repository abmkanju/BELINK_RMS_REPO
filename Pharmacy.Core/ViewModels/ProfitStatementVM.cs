using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ProfitStatementVM
    {
        public string Date { get; set; }
        public double GrandTotals { get; set; }
        public double NetTotalSale { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalSales { get; set; }
        public double CostOfSales { get; set; }
        public double DiscountedCostofSales { get; set; }
        public double Discount { get; set; }
        public double ProfitMargin { get; set; }
        public double Profit { get; set; }
        public double  Expense { get; set; }
    }
}
