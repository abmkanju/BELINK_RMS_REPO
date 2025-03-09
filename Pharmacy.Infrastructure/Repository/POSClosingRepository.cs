using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class POSClosingRepository:Repository<POSClosing>
    {
        PharmacyContext cxt;
        public POSClosingRepository(PharmacyContext cxt) :base(cxt)
        {
            this.cxt = cxt;
        }

        public DateTime GetLastPOSClosingDate()
        {
            return cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(r => r.POSClosingId).Select(r=>r.ClosingDate).FirstOrDefault();
        }
        public double GetLastCashClosingVal()
        {
            return (double?)cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(c => c.POSClosingId).Select(c => (double?)c.CashTillIn).FirstOrDefault() ?? 0;
        }

        public int GetNewClosingCode()
        {
            return cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(c => c.POSClosingId).Select(c => c.POSCode).FirstOrDefault() + 1;
        }
    }
}
