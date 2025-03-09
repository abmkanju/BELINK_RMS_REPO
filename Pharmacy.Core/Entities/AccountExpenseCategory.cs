using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AccountExpenseCategory
    {
        public int AccountExpenseCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsSynced { get; set; }
        public User User { get; set; }
        public ICollection<AccountExpense> AccountExpenses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
