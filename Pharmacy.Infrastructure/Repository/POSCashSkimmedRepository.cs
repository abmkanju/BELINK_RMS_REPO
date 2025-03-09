using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class POSCashSkimmedRepository : Repository<POSSkimmedCash>
    {
        PharmacyContext cxt;
        public POSCashSkimmedRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<POSSkimmedCash> GetCS_CurrSess(int userId)
        {
            //return cxt.POSReceivedCash
            //    .Where(c => c.IsActive
            //        && c.UserId == userId
            //        && c.CreatedAt >= c.User.Shift.StartTime
            //        && c.CreatedAt <= c.User.Shift.EndTime)                    
            //        .ToList();

            DateTime last = (from h in cxt.LoginHistory
                             where h.UserId == userId
                             orderby h.LoginHistoryId descending
                             select h.CreatedAt)
                    .Skip(1).Take(1).FirstOrDefault();
            return cxt.POSSkimmedCash.Where(c => c.IsActive && c.UserId == userId && c.CreatedAt >= last).OrderByDescending(r => r.POSSkimmedCashId).ToList();
        }

        public double GetCSSum_CurrSess(int userId)
        {
            //return cxt.POSReceivedCash
            //    .Where(c => c.IsActive
            //        && c.UserId == userId
            //        && c.CreatedAt >= c.User.Shift.StartTime
            //        && c.CreatedAt <= c.User.Shift.EndTime)                    
            //        .ToList();

            //DateTime last = (from h in cxt.LoginHistory
            //                 where h.UserId == userId &&
            //                 orderby h.LoginHistoryId descending
            //                 select h.CreatedAt)
            //        .Skip(1).Take(1).FirstOrDefault();
            return (double?)cxt.POSSkimmedCash
                .Where(c => c.IsActive
                    && c.UserId == userId
                    && c.CreatedAt >= (from p in cxt.POSClosings where p.IsActive orderby p.POSClosingId descending select p.ClosingDate)
                       .FirstOrDefault()
                    ).Sum(r => (double?)r.Cash) ?? 0;
        }
    }
}