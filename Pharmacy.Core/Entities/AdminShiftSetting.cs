using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminShiftSetting
    {
        public int AdminShiftSettingId { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
