using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Batch
    {        
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public DateTime? Expiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? SyncedAt { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        [NotMapped]
        public string IsJsonCreated { get; set; } //Used for the json purpose (0 => Created, 1 => Updated, -1 => Not Change)
    }
}
