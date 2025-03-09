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

    public class ChequeInfoRepository : Repository<ChequeInfo>, IChequeInfoRepository
    {
        PharmacyContext cxt = null;
        public ChequeInfoRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public long InsertChequeInfo(ChequeInfo objChequInfo)
        {
            cxt.Chequeinfos.Add(objChequInfo);
            cxt.SaveChanges();
            return objChequInfo.ChequeInfoId;
        }
    }
}