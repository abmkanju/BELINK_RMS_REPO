using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AccountExpenseVM
    {
        public int AccountExpenseId { get; set; }
        public string Description { get; set; }
        public int VoucherNo { get; set; }
        public DateTime DepositDate { get; set; }
        public int PaymentMode { get; set; }
        public string PaymentName { get; set; }
        public double Amount { get; set; }
        public double debit { get; set; }
        public byte[] Image { get; set; }
        public bool IsSynced { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int AccountExpenseCategoryId { get; set; }
        public string AccountExpenseCategoryName { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AccountExpenseRptVM
    {
        public int AccountExpenseId { get; set; }
        public string Description { get; set; }
        public int VoucherNo { get; set; }
        public DateTime DepositDate { get; set; }
        public int PaymentMode { get; set; }
        public string PaymentName { get; set; }
        public double Amount { get; set; }
        public double debit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AccountExpenseCategoryName { get; set; }
    }
}
