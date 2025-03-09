using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class CashSummaryVM
    {
        public bool IsFirstClosing { get; set; }

        public double OpeningCash { get; set; }

        //Total inflow
        public double SkimmedCash { get; set; }
        public double SubmittedCash { get; set; }

        //total outflow
        /// <summary>
        /// Cash recevied by cashier and given by manager
        /// </summary>
        public double ReceivedCash { get; set; }
        public double SupplierPayments { get; set; }


        /// <summary>
        /// Total Inflow = SkimmedCash + SubmittedCash
        /// </summary>
        public double TotalInflow { get; set; }

        /// <summary>
        /// TotalOutFlow = ReceivedCash + SupplierPayments
        /// </summary>
        public double TotalOutFlow { get; set; }

        public DateTime LastClosingDate { get; set; }
    }
}