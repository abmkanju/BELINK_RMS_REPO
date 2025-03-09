using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AuditLogVM
    {
        public long AuditLogId { get; set; }
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
    }
}
