using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class LowStockMail
    {
        public long LowStockMailId { get; set; }
        public DateTime LastSent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}