using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class POSSkimmedCash
    {
        public int POSSkimmedCashId { get; set; }
        public double Cash { get; set; }
        public int POSCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
    }
}
