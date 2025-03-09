using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core
{
    public static class GlobalSharing
    {
        public enum Patient_SearchBy { Name = 1, MRNo = 2, Email = 3, Phone = 4, DateOfBirth = 5, Address = 6, ReferredBy = 7 }
        public enum Gender { Male = 1, Female = 2, Other = 3}
        public static long UserId = 0;
        public enum ConsumptionType { Services = 1, Sales = 2, Damages = 3, Returned = 4 };
        public static string Salt = "hw05grewqwas21w3";
    }
}
