using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Pharmacy.Core.ViewModels
{
    public class SummationsVM
    {
        public double? TotalRevenue { get; set; }
        public double? TotalRefund { get; set; }
        public double? RetailValueOfAvailableStock { get; set; }
        public double? CostOfAvailableStock { get; set; }        
        public double? MaxMonthRevenue { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentType { get; set; }
        public string PaymentTypeString { get; set; }
        public double? Payment { get; set; }
        public double? TotalDues { get; set; }
        /// <summary>
        /// Retail Value of Total stock
        /// </summary>
        public double RV_T_Stock { get; set; }
        /// <summary>
        /// Retail Value of total Adjusted
        /// </summary>
        public double RV_T_A_Stock { get; set; }
        /// <summary>
        /// Retail value of Total consumed stock
        /// </summary>
        public double RV_T_C_Stock { get; set; }
        /// <summary>
        /// retail value of total expired stock
        /// </summary>
        public double RV_T_E_Stock { get; set; }
        /// <summary>
        /// retail value of total consumed expired stock
        /// </summary>
        public double RV_T_C_E_Stock { get; set; }
        /// <summary>
        /// consumed value of total stock
        /// </summary>
        public double CV_T_Stock { get; set; }
        /// <summary>
        /// consumed value of total adjusted stock
        /// </summary>
        public double CV_T_A_Stock { get; set; }
        /// <summary>
        /// consumed value of total consumed stock
        /// </summary>
        public double CV_T_C_Stock { get; set; }
        /// <summary>
        /// consumed value of total expired stock
        /// </summary>
        public double CV_T_E_Stock { get; set; }
        /// <summary>
        /// cost value of total consumed expired stock
        /// </summary>
        public double CV_T_C_E_Stock { get; set; }
        public double? GrossSale { get; set; }

    }
}