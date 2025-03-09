using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class SalesReturnItem
    {
        [Key]
        public long SaleReturnItemId { get; set; }
        public int ReturnQuantity { get; set; }
        public double RefundAmount { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public double RetialPrice { get; set; }
        public int DeductionType { get; set; }
        public double DeductionValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public int ItemId { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public int BatchId { get; set; }
        [NotMapped]
        public int ConvUnit { get; set; }
        public SalesReturn SalesReturn { get; set; }
        public int InvoiceRefNo { get; set; }
    }
}
