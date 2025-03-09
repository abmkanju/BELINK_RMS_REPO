using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AdminPractiseSettingRepository : Repository<AdminPractiseSetting>
    {
        PharmacyContext cxt = null;
        public AdminPractiseSettingRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public int GetPracticeId()
        {
            return cxt.AdminPractiseSetting.FirstOrDefault().PracticeId;
        }

        public bool IsUserDeletedDefaultData()
        {
            return cxt.AdminPractiseSetting.FirstOrDefault().UserDeletedDefaultData;
        }

        public AdminPractiseSetting getAdminPracStting()
        {
            return cxt.AdminPractiseSetting.FirstOrDefault();
        }
    }
}