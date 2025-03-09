using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
    }
}
