using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockPaymentVM
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public string Description { get; set; }
        public int PaymentType { get; set; }
        public double Payment { get; set; }
        public double TotalPaid { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
