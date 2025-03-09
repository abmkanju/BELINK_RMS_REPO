using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class HwData
    {
        public int HwDataId { get; set; }
        public string SystemExpiry { get; set; }
        public bool IsSyncManual { get; set; }
    }
}