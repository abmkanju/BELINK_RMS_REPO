using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AdjustmentItemVM
    {
        public long AdjustmentItemId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public double CostPrice { get; set; }
        public double AdjustmentValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public int Unit { get; set; }
        public int ConversionUnit { get; set; }
        public string ItemName { get; set; }        
        public string BatchName { get; set; }
    }
}
