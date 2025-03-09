using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class FlatDiscountVM
    {
        public int FlatDiscountId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public string DiscTypeString { get; set; }
        public double DiscountedPrice { get; set; }

        public bool IsAllDays { get; set; }
        public bool IsAllTimes { get; set; }
        public string SelectedDays { get; set; }
        public bool IsAllItems { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<DiscountItemVM> DiscountItems { get; set; }
        public User User { get; set; }
    }
}
