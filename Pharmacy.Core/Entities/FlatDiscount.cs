using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class FlatDiscount
    {
        public int FlatDiscountId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }

        public bool IsAllDays { get; set; }
        public bool IsAllTimes { get; set; }
        public string SelectedDays { get; set; }
        public bool IsAllItems { get; set; }
        public string Comment { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<DiscountItem> DiscountItems { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}