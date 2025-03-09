using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class TemplateItemVm
    {
        public int TemplateItemId { get; set; }

        public Item Item { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public User User { get; set; }
        public Template Template { get; set; }
    }
}
