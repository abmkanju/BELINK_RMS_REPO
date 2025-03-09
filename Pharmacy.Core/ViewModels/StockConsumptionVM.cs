using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class StockConsumptionVM
    {
        public long StockConsumptionId { get; set; }
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public List<StockConsumptionItemVM> StockConsumptionsList { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
    }
}