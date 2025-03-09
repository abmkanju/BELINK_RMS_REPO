using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AccountExpense
    {
        public int AccountExpenseId { get; set; }
        public int VoucherNo { get; set; }
        public string Description { get; set; }
        public DateTime DepositDate { get; set; }
        public int PaymentMode { get; set; }
        public double Amount { get; set; }
        public double debit { get; set; }
        public bool IsActive { get; set; }
        public byte[] Image { get; set; }
        public bool IsSynced { get; set; }
        public AccountExpenseCategory AccountExpenseCategory { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
