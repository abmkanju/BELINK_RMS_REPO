using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class HwFileDataRepository : Repository<HwFileData>
    {
        PharmacyContext cxt = null;
        public HwFileDataRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}