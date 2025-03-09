using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class LowStockMailRepository : Repository<LowStockMail>
    {
        PharmacyContext cxt;
        public LowStockMailRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public DateTime GetLastMailSentDate()
        {
            return cxt.LowStockMails.OrderByDescending(r=>r.CreatedAt).FirstOrDefault().LastSent;
        }

        public void AddLastMailSentDate(LowStockMail objLowStockMail)
        {
            cxt.LowStockMails.Add(objLowStockMail);
        }
    }
}