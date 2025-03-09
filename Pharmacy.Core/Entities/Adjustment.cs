using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Adjustment
    {
        public long AdjustmentId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }      
        public User User { get; set; }
        public ICollection<AdjustmentItem> AdjustmentItems { get; set; }

        public bool IsActive { get; set; }

        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced{ get; set; }
    }
}
