using PagedList;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AdminFBRSettingRepository : Repository<AdminFBRSetting>
    {
        PharmacyContext cxt;
        public AdminFBRSettingRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public AdminFBRSetting GetFBRRecord()
        {
            return cxt.AdminFBRSettings.FirstOrDefault();
        }
    }
}
