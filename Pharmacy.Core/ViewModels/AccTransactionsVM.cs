using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AccTransactionsVM
    {
        public DateTime TrDate { get; set; }
        public int Type { get; set; }
        public string TypeString { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public string Comments { get; set; }        
    }
}
