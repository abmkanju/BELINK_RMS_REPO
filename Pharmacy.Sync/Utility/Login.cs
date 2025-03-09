using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.Utility
{
    public class Login
    {
        public bool success { get; set; }
        public string status { get; set; }
        public string auth_token { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        public string fname { get; set; }
        public object lname { get; set; }
        public int practice_id { get; set; }
        public string user_role { get; set; }
    }
}
