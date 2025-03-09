using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class StoreClosing
    {

        // cash summary attrs
        public int StoreClosingId { get; set; }
        public double OpeningCash { get; set; }
        public double TotalInflow { get; set; }
        public double TotalOutFlow { get; set; }
        public double SystemCash { get; set; }
        public double PhysicalCash { get; set; }
        public double CashDiff { get; set; }


        // cash submission attrs
        public double CashSubmittedToBank { get; set; }
        public double CashSubmittedToHO { get; set; }
        public double TotalCashSubmitted { get; set; }
        public double ClosingCash { get; set; }


        public DateTime ClosingDate { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }

        public User User { get; set; }        
    }
}
