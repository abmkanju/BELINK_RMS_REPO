using PagedList;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AccountExpenseCategoryRepository : Repository<AccountExpenseCategory>
    {
        PharmacyContext cxt;
        public AccountExpenseCategoryRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public bool ExpenseCategoryExistsNew(string name)
        {
            return cxt.AccountExpenseCategory.Any(m => m.Name.ToLower() == name && m.IsActive);
        }
        public List<AccountExpenseCategory> GetAllActiveDetails()
        {
            return cxt.AccountExpenseCategory.Where(x=>x.IsActive).ToList();
        }
        public bool ExpenseCategoryExistsUpdate(string name, int ExpenseCateId)
        {
            return cxt.AccountExpenseCategory.Any(m => m.Name.ToLower() == name && m.AccountExpenseCategoryId != ExpenseCateId && m.IsActive);
        }
        public void SetExpenseCategoryInActive(int ExpenseCategoryId)
        {
             cxt.Database.ExecuteSqlCommand(
                "Update AccountExpenseCategories Set IsActive = 0,  DeletedAt = @DeletedAt WHERE AccountExpenseCategoryId = @AccountExpenseCategoryId",
                new SqlParameter("@AccountExpenseCategoryId", ExpenseCategoryId), new SqlParameter("@DeletedAt", DateTime.Now));
        }
        public IPagedList<AccountExpenseCategory> GenericSearchForExpenseCategory(DateTime? dtpFromDate, DateTime? dtpToDate, int PageNo, int PageSize)
        {
            IQueryable<AccountExpenseCategory> query = cxt.AccountExpenseCategory.Where(x => x.IsActive == true).AsQueryable();
            if (dtpFromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= DbFunctions.TruncateTime(dtpFromDate));
            if (dtpToDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) <= DbFunctions.TruncateTime(dtpToDate));
            return query.OrderByDescending(x => x.AccountExpenseCategoryId).ToPagedList(PageNo, PageSize);
        }

    }
}
