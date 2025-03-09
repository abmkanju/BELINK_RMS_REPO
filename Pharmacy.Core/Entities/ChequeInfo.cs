using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class ChequeInfo
    {
        public long ChequeInfoId { get; set; }
        public string ChequeNo{ get; set; }
        public string BankName { get; set; }
        public string Status { get; set; }
    }
}
