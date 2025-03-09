using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class OfflineTask
    {
        public long OfflineTaskId { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }
        [Column(TypeName = "text")]
        public string ModuleJson { get; set; }
        public string ErrorResponse { get; set; }
        public int ErrorSensitivity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
