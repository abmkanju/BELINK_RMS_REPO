using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class ChargeInfo
    {
        public long ChargeInfoId { get; set; }
        public string P_InvoiceNo { get; set; }
        public string P_RiderName { get; set; }
        public string P_Vendor { get; set; }
    }
}
