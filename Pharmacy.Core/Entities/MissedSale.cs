using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class MissedSale
    {
        public int MissedSaleId { get; set; }
        public Item Item { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}