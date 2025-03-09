using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Supplier
    {
        public long SupplierID { get; set; }
        [StringLength(600)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PrimaryContactPersonName { get; set; }
        public string PrimaryContactPersonPhone { get; set; }
        public double OpeningBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public User User { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<Item> Items { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public string NTN { get; set; }
        public string STN { get; set; }
        public int CreditLImit { get; set; }
    }
}