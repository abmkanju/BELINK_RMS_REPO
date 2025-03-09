using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockReturnVM
    {
        public long StockReturnId { get; set; }
        public long DocumentNo { get; set; }
        public double GrandTotal { get; set; }
        public double NetDiscount { get; set; }
        public double NetSalesTax { get; set; }
        public string SupplierName { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsSynced { get; set; }
        public bool IsUpdate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public User User { get; set; }
        public Stock Stock { get; set; }
        public ICollection<StockReturnItemVM> StockReturnItems { get; set; }
    }
}
