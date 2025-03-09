using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class ProfitStatementTransactions
    {
        [Key]
        public int TransactionId { get; set; }
        public double TransactionRevenue { get; set; }
        public double TransactionCostOfSales { get; set; }
        public double TransactionDiscountedCostOfSale { get; set; }
        public double TransactionDiscount { get; set; }
        public double TransactionReturn { get; set; }
        public double TransactionExpense { get; set; }
        public DateTime TransactionDate { get; set; }
        public int User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long InvoiceId { get; set; }
        public long ItemId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
