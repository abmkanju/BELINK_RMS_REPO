using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class POSClosing
    {
        public int POSClosingId { get; set; }
        public int POSCode { get; set; }
        public DateTime ClosingDate { get; set; }
        public double CashCounted { get; set; }
        public double CashSubmitted { get; set; }
        public double CashTillIn { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
    }
}