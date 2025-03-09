using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockAuditsScreenVM
    {
        public int StockAuditId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AuditDate { get; set; }
        public double TotalDifference { get; set; }     
    }
}
