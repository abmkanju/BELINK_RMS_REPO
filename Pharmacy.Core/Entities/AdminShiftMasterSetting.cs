using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminShiftMasterSetting
    {
        public int AdminShiftMasterSettingId { get; set; }
        public bool ShiftsEnabled { get; set; }
        public bool EnforceLogout { get; set; }        
    }
}
