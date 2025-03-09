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
    public class ProfitStatementTransactionsRepository : Repository<ProfitStatementTransactions>
    {
        PharmacyContext cxt;
        public ProfitStatementTransactionsRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<ProfitStatementTransactions> GetAllActiveDetails()
        {
            return cxt.ProfitStatementTransactions.ToList();
        }
        public List<ProfitStatementTransactions> GetBetweenDates(DateTime From, DateTime to)
        {
            return cxt.ProfitStatementTransactions.Where(m => m.TransactionDate >= From && m.TransactionDate <= to).ToList();
        }
        public List<ProfitStatementTransactionVM> GetGrossProfitStatements(DateTime DateFrom, DateTime DateTo)
        {
            List<ProfitStatementTransactionVM> vm = new List<ProfitStatementTransactionVM>();
            vm = (from i in cxt.ProfitStatementTransactions
                                       

                                       where DbFunctions.TruncateTime(i.TransactionDate) >= DbFunctions.TruncateTime(DateFrom)
                                       where DbFunctions.TruncateTime(i.TransactionDate) <= DbFunctions.TruncateTime(DateTo)
                                        where !i.IsDeleted
                                       group i by DbFunctions.TruncateTime(i.TransactionDate) into g
                                       select new ProfitStatementTransactionVM
                                       {
                                           PaymentDate = (DateTime)g.Key,
                                           TotalRevenue = g.Sum(r => (double?)r.TransactionRevenue) ?? 0,
                                           CostOfSales = (double?)g.Sum(r => r.TransactionCostOfSales) ?? 0,
                                           DiscountedCostOfSales = (double?)g.Sum(r => r.TransactionDiscountedCostOfSale) ?? 0,
                                           Discount = g.Sum(r => (double?)r.TransactionDiscount) ?? 0,
                                           Expense = g.Sum(r => (double?)r.TransactionExpense) ?? 0,
                                           TotalRefund = g.Sum(r => (double?)r.TransactionReturn) ?? 0,
                                           
                                           //Expense = cxt.AccountExpenses.Where(x => x.IsActive && DbFunctions.TruncateTime(x.CreatedAt) == g.Key).Sum(x => (double?)x.Amount) ?? 0,
                                           //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                                       }).OrderByDescending(r => r.PaymentDate).ToList();
            
            //Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal)) ?? 0;

            return vm;
        }

    }
}
