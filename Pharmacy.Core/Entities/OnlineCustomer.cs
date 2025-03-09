using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class OnlineCustomer
    {
        [Key]
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public OnlineOrder OnlineOrder { get; set; }
        public bool IsActive { get; set; }
    }
}