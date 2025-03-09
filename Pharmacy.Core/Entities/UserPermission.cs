using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class UserPermission
    {
        public int UserPermissionId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// this property will be populated from C# code, this does not belong to any Table in database
        /// </summary>
        public int RightId { get; set; }
    }
}