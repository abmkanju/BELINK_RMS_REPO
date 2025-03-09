using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class LoginHistoryRepository:Repository<LoginHistory>
    {
        PharmacyContext cxt;
        public LoginHistoryRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}