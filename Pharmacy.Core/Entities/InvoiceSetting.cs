using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class InvoiceSetting
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }
        public bool Sr { get; set; }
        public bool ItemName { get; set; }

        public bool PCTCode { get; set; }

        public bool Batch { get; set; }

        public bool RackNo { get; set; }

        public bool Unit { get; set; }

        public bool ConvUnit { get; set; }
        public bool CostPrice { get; set; }
        public bool DiscCost { get; set; }
        public bool Rate { get; set; }
        public bool ProfitMargin { get; set; }
        public bool Amount { get; set; }
        public bool NetAmount { get; set; }
        public bool Discount { get; set; }
        public bool DiscountType { get; set; }
        public bool SaleTax { get; set; }
        public bool SaleTaxType { get; set; }
        public bool AvailableQty { get; set; }
        public bool Quantity { get; set; }
        public bool BonusQuantity { get; set; }
        public int UserID { get; set; }
    }
}
