using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class StoreClosingRepository : Repository<StoreClosing>
    {
        PharmacyContext cxt;
        public StoreClosingRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public DateTime LastClosingDate()
        {
            return cxt.StoreClosings.Where(sc => sc.IsActive).Select(sc => sc.ClosingDate).FirstOrDefault();
        }

        public CashSummaryVM getLastCashSummary()
        {
            StoreClosing lastClosing = cxt.StoreClosings.Where(sc => sc.IsActive).OrderByDescending(sc => sc.StoreClosingId).FirstOrDefault();
            CashSummaryVM vm = new CashSummaryVM();
            //DateTime dt = lastClosing.ClosingDate; //Comment this code bez create exception and not in used           
            if (lastClosing == null)
            {
                vm = new CashSummaryVM
                       {
                           IsFirstClosing = true,
                           LastClosingDate = lastClosing == null ? new DateTime() : lastClosing.ClosingDate,
                           OpeningCash = 0,
                           SkimmedCash = cxt.POSSkimmedCash.Count() > 0 ? (double?)(from sk in cxt.POSSkimmedCash where sk.IsActive select sk.Cash).Sum() ?? 0 : 0,
                           SubmittedCash = cxt.POSClosings.Count() > 0 ? (double?)(from pc in cxt.POSClosings where pc.IsActive select pc.CashSubmitted).Sum() ?? 0 : 0,
                           ReceivedCash = cxt.POSReceivedCash.Count() > 0 ? (double?)(from rc in cxt.POSReceivedCash where rc.IsActive select rc.Cash).Sum() ?? 0 : 0,
                           SupplierPayments = cxt.Expenses.Count() > 0 ? (double?)(from ex in cxt.Expenses where ex.IsActive && ex.PaymentMode == 1 select ex.Amount).Sum() ?? 0 : 0,
                       };
            }
            else
            {
                vm = (from sc in cxt.StoreClosings
                      where sc.IsActive
                      orderby sc.StoreClosingId descending
                      select new CashSummaryVM
                      {
                          IsFirstClosing = false,
                          LastClosingDate = lastClosing.ClosingDate,
                          OpeningCash = (double?)(from x in cxt.StoreClosings where x.IsActive && x.StoreClosingId == lastClosing.StoreClosingId select x.ClosingCash).FirstOrDefault() ?? 0,
                          SkimmedCash = (double?)(from sk in cxt.POSSkimmedCash where sk.IsActive && sk.CreatedAt > lastClosing.ClosingDate select sk.Cash).Sum() ?? 0,
                          SubmittedCash = (double?)(from pc in cxt.POSClosings where pc.IsActive && pc.CreatedAt > lastClosing.ClosingDate select pc.CashSubmitted).Sum() ?? 0,
                          ReceivedCash = (double?)(from rc in cxt.POSReceivedCash where rc.IsActive && rc.CreatedAt > lastClosing.ClosingDate select rc.Cash).Sum() ?? 0,
                          SupplierPayments = (double?)(from ex in cxt.Expenses where ex.IsActive && ex.PaymentMode == 1 && ex.CreatedAt > lastClosing.ClosingDate select ex.Amount).Sum() ?? 0,

                      }).FirstOrDefault();
            }
            if (vm != null)
            {
                vm.TotalInflow = vm.SkimmedCash + vm.SubmittedCash;
                vm.TotalOutFlow = vm.ReceivedCash + vm.SupplierPayments;
            }
            return vm;
        }

        public CashSummaryVM GetLastCashSummary(DateTime dt)
        {
            StoreClosing lastClosing = cxt.StoreClosings.Where(sc => sc.IsActive).OrderByDescending(sc => sc.StoreClosingId).FirstOrDefault();
            CashSummaryVM vm = new CashSummaryVM();
            if (lastClosing == null)
            {
                vm = new CashSummaryVM
                {
                    IsFirstClosing = true,
                    LastClosingDate = lastClosing == null ? new DateTime() : lastClosing.ClosingDate,
                    OpeningCash = 0,
                    TotalInflow = 
                     cxt.invoicePayments.Where(p => p.IsActive && !p.IsQInvoice && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(dt)).Sum(r => (double?)r.Payment) ?? 0 + 
                     cxt.Stocks.Where(p => p.IsActive && DbFunctions.TruncateTime(p.CreateDate) <= DbFunctions.TruncateTime(dt)).Sum(r => (double?)r.GrandTotal) ?? 0,
                    TotalOutFlow = cxt.Expenses.Count() > 0 ? (double?)(from ex in cxt.Expenses where ex.IsActive && ex.PaymentMode == 1 select ex.Amount).Sum() ?? 0 : 0,
                };
            }
            else
            {
                vm = (from sc in cxt.StoreClosings
                      where sc.IsActive
                      orderby sc.StoreClosingId descending
                      select new CashSummaryVM
                      {
                          IsFirstClosing = false,
                          LastClosingDate = lastClosing.ClosingDate,
                          OpeningCash = (double?)(from x in cxt.StoreClosings where x.IsActive && x.StoreClosingId == lastClosing.StoreClosingId select x.ClosingCash).FirstOrDefault() ?? 0,
                          TotalInflow =
                           cxt.invoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentDate > lastClosing.ClosingDate && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(dt)).Sum(r => (double?)r.Payment) ?? 0 +
                           cxt.Stocks.Where(p => p.IsActive && p.CreateDate > lastClosing.ClosingDate && DbFunctions.TruncateTime(p.CreateDate) <= DbFunctions.TruncateTime(dt)).Sum(r => (double?)r.GrandTotal) ?? 0,

                          TotalOutFlow = (double?)(from ex in cxt.Expenses where ex.IsActive && ex.PaymentMode == 1 && ex.CreatedAt > lastClosing.ClosingDate select ex.Amount).Sum() ?? 0,

                      }).FirstOrDefault();
            }
            return vm;
        }
    }
}