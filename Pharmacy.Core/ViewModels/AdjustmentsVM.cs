using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AdjustmentsVM
    {
        public long AdjustmentId { get; set; }
        public long UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<AdjustmentItemVM> AdjustmentItems { get; set; }
        public bool IsActive { get; set; }
    }
}
