using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class HwDataRepository : Repository<HwData>
    {
        PharmacyContext cxt;
        public HwDataRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public string GetSystemExpiryDate()
        {
            return cxt.HwData.Select(r => r.SystemExpiry).FirstOrDefault();
        }
    }
}
