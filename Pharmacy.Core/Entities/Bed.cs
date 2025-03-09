using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Bed
    {
        public long BedId { get; set; }
        public string RoomNo { get; set; }
        public double ChargesPerDay { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public Ward Ward { get; set; }

        [NotMapped]
        public long WardId { get; set; }
        [NotMapped]
        public string WardString { get; set; }
    }
}
