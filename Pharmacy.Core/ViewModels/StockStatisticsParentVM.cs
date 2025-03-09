using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockStatisticsParentVM
    {
        public List<StockStatisticsVM> StockStatisticsList { get; set; }
        public double TotalRetValOfAvailableStock { get; set; }
        public double TotalCostValOfAvailableStock { get; set; }
    }
}
