using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
   public class SalesReturnItemVM
    {
       public int SaleRetItemId { get; set; }
        public int SaleReturnItemId { get; set; }
        public int ReturnQuantity { get; set; }
        public bool isActive { get; set; }
        public string ItemName { get; set; }
        public string BatchName { get; set; }
        public double RefundAmount { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public double RetialPrice { get; set; }
        public int DeductionType { get; set; }
        public double DeductionValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
