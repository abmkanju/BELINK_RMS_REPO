using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ProfitStatementTransactionVM
    {
        public DateTime PaymentDate { get; set; }
        public double TotalRevenue { get; set; }
        public double CostOfSales { get; set; }
        public double DiscountedCostOfSales { get; set; }
        public double Discount { get; set; }
        public double Expense { get; set; }
        public double TotalRefund { get; set; }
        public double Profit { get; set; }
        public string Date { get; set; }

    }

    
}
