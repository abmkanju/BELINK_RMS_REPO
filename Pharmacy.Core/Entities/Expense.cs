using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Expense
    {
        public long ExpenseId { get; set; }
        public string description { get; set; }
        public int PaymentMode { get; set; }
        public int PracticeId { get; set; }
        public int VoucherNo { get; set; }
        public bool AutoAdded { get; set; }
        public double Amount { get; set; }        
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? SyncedAt { get; set; }

        public Stock Stock { get; set; }
        public Supplier Supplier { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }

        [NotMapped]
        public long StockId { get; set; }
        [NotMapped]
        public long SupplierId { get; set; }
    }
}