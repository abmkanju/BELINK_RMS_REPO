using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AccTransactionsRepository:Repository<AccTransaction>
    {
        PharmacyContext cxt;
        public AccTransactionsRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<AccTransaction> GetAllActiveTransactions()
        {
            return cxt.AccTransactions.Where(t => t.IsActive).ToList();
        }
    }
}