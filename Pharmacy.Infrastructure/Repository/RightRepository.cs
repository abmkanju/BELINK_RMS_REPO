using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class RightRepository : Repository<Right>
    {
        PharmacyContext cxt;
        public RightRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}
