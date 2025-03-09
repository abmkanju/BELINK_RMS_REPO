using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class SupplierVM
    {
        public long SupplierID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PrimaryContactPersonName { get; set; }
        public string PrimaryContactPersonPhone { get; set; }
        public double OpeningBalance { get; set; }
        public ICollection<Item> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public string NTN { get; set; }
        public string STN { get; set; }
        public int CreditLImit { get; set; }
    }
}
