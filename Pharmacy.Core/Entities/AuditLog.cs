using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AuditLog
    {
        public long AuditLogId { get; set; }
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}
