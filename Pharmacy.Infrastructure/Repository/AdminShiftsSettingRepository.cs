using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{

    public class AdminShiftsSettingRepository : Repository<AdminShiftSetting>
    {
        PharmacyContext cxt;
        public AdminShiftsSettingRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<AdminShiftSetting> GetActiveShifts()
        {
            return cxt.Shifts.Where(s => s.IsActive).ToList();
        }
        public List<AdminShiftSetting> GetShifts()
        {
            return cxt.Shifts.ToList();
        }
    }
}