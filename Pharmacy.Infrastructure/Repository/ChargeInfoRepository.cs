using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;

namespace Pharmacy.Infrastructure.Repository
{

    public class ChargeInfoRepository : Repository<ChargeInfo>, IChargeInfoRepository
    {
        PharmacyContext cxt = null;
        public ChargeInfoRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public long InsertChargeInfo(ChargeInfo objChargeInfo)
        {
            cxt.Chargeinfos.Add(objChargeInfo);
            cxt.SaveChanges();
            return objChargeInfo.ChargeInfoId;
        }
    }
}