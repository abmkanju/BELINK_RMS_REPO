using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminPractiseSetting
    {
        public long AdminPractiseSettingId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LogoPath { get; set; }
        public int PracticeId { get; set; }
        public bool UserDeletedDefaultData { get; set; }
        public string FBRUsinNo { get; set; }
        public int slotlimits { get; set; }
        public string networkip { get; set; }
        public bool IsNetworkSystem { get; set; }
        public bool IsFranchiseMapped { get; set; }
    }
}
