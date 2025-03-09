using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class TodaysReportVM
    {
        public double? SalesTotal { get; set; }
        public double? StockReceivedTotal { get; set; }
        public double? PurchaseOrdersTotal { get; set; }
        public double? CashReceivedTotal { get; set; }                     
        public double? DebitCreditTotal { get; set; }
        public double? OnlinePaymentsTotal { get; set; }
        public double? ChecquePaymentsTotal { get; set; }
        public double? TotalRefund { get; set; }
        public double? TotalChargePayment { get; set; }


    }
}
