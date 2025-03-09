using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        public string Phone { get; set; }
        [StringLength(600)]
        public string Email { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }
        public int? ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public AdminShiftSetting Shift { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Right> Rights { get; set; }
        public ICollection<AdmittedPatient> AdmittedPatients { get; set; }

        public bool CanGiveDiscount { get; set; }

        /// <summary>
        /// this is the number that user can give max dicount in percentage on POS screen.
        /// </summary>        
        public double  DiscLimit { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public int Pin { get; set; }
    }
}