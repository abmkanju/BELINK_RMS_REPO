using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Right
    {
        public int RightId { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
