using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AccountExpenseRepository : Repository<AccountExpense>
    {
        PharmacyContext cxt;
        public AccountExpenseRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public AccountExpenseVM GetExpenseById(int ExpenseId)
        {
            AccountExpenseVM model = cxt.AccountExpenses.Where(x => x.AccountExpenseId == ExpenseId && x.IsActive).Select(e => new AccountExpenseVM
            {
                AccountExpenseId = e.AccountExpenseId,
                AccountExpenseCategoryId = e.AccountExpenseCategory.AccountExpenseCategoryId,
                Description = e.Description,
                DepositDate = e.DepositDate,
                PaymentMode = e.PaymentMode,
                Amount = e.Amount,
                debit = e.debit,
                Image = e.Image,

            }).FirstOrDefault();
            return model;
        }

        public int GetVoucherNo()
        {
            int VoucherNo = 0;
            if (cxt.AccountExpenses.FirstOrDefault() == null)
                VoucherNo = 100000;
            else
                VoucherNo = cxt.AccountExpenses.Max(n => n.VoucherNo);
            return VoucherNo;
        }

        public IPagedList<AccountExpenseVM> GenericSearchForAccountUI(DateTime? fromDate, DateTime? toDate, int ExpenseCategoryId, int PageNo, int PageSize)
        {
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            if (ExpenseCategoryId > 0)
                query = query.Where(x => x.AccountExpenseCategory.AccountExpenseCategoryId == ExpenseCategoryId && x.AccountExpenseCategory.IsActive);
            IPagedList<AccountExpenseVM> objList = query.Select(e => new AccountExpenseVM
            {
                AccountExpenseId = e.AccountExpenseId,
                VoucherNo = e.VoucherNo,
                Description = e.Description,
                DepositDate = e.DepositDate,
                PaymentMode = e.PaymentMode,
                Amount = e.Amount,
                debit=e.debit,
                AccountExpenseCategoryName = e.AccountExpenseCategory.Name,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.User.UserName,
            }).OrderByDescending(s => s.AccountExpenseId)
              .ToPagedList(PageNo, PageSize);
            return objList;
        }

        public List<AccountExpenseRptVM> GenericSearchForAccountUIRpt(DateTime? fromDate, DateTime? toDate, int ExpenseCategoryId)
        {
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            if (ExpenseCategoryId > 0)
                query = query.Where(x => x.AccountExpenseCategory.AccountExpenseCategoryId == ExpenseCategoryId && x.AccountExpenseCategory.IsActive);
            List<AccountExpenseRptVM> objList = query.Select(e => new AccountExpenseRptVM
            {
                AccountExpenseId = e.AccountExpenseId,
                VoucherNo = e.VoucherNo,
                Description = e.Description,
                DepositDate = e.DepositDate,
                PaymentMode = e.PaymentMode,
                Amount = e.Amount,
                debit = e.debit,
                AccountExpenseCategoryName = e.AccountExpenseCategory.Name,
                CreatedAt = e.CreatedAt,
            }).OrderByDescending(s => s.AccountExpenseId).ToList();
            return objList;
        }
        public double GenericSearchForExpensesSum(DateTime? fromDate, DateTime? toDate, int ExpenseCategoryId)
        {
            double TotalExpense = 0;
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            if (ExpenseCategoryId > 0)
                query = query.Where(x => x.AccountExpenseCategory.AccountExpenseCategoryId == ExpenseCategoryId && x.AccountExpenseCategory.IsActive);
            TotalExpense = query.Sum(x => (double?)x.Amount) ?? 0;
            return TotalExpense;
        }
        public double GenericSearchForExpensesSum_forStatment(DateTime? fromDate, DateTime? toDate)
        {
            double TotalExpense = 0;
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            
            TotalExpense = query.Sum(x => (double?)x.Amount) ?? 0;
            return TotalExpense;
        }
        public double GenericSearchForExpensesSum_debit(DateTime? fromDate, DateTime? toDate, int ExpenseCategoryId)
        {
            double TotalExpense = 0;
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            if (ExpenseCategoryId > 0)
                query = query.Where(x => x.AccountExpenseCategory.AccountExpenseCategoryId == ExpenseCategoryId && x.AccountExpenseCategory.IsActive);
            TotalExpense = query.Sum(x => (double?)x.debit) ?? 0;
            
            return TotalExpense;
        }
        public double GenericSearchForExpensesSum_debit_ForStatment(DateTime? fromDate, DateTime? toDate)
        {
            double TotalExpense = 0;
            IQueryable<AccountExpense> query = cxt.AccountExpenses.Where(x => x.IsActive).AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) >= DbFunctions.TruncateTime(fromDate.Value));
            if (toDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.UpdatedAt) <= DbFunctions.TruncateTime(toDate.Value));
            
            TotalExpense = query.Sum(x => (double?)x.debit) ?? 0;
            return TotalExpense;
        }


        public void SetExpenseInActive(int ExpenseId)
        {
            cxt.Database.ExecuteSqlCommand("Update AccountExpenses set IsActive = 0, DeletedAt = @DeletedAt WHERE AccountExpenseId = @AccountExpenseId", new SqlParameter("@AccountExpenseId", ExpenseId), new SqlParameter("@DeletedAt", DateTime.Now));
        }
    }
}
