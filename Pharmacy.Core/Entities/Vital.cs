using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Vital
    {
        public long VitalId { get; set; }
        public double Weight { get; set; }
        public double Temprature { get; set; }
        public double BloodPressure { get; set; }
        public double BloodSugar { get; set; }
        public int Test { get; set; }
        public double RespiratoryRate { get; set; }
        public double OxygenSaturation { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public User User { get; set; }
        public AdmittedPatient AdmittedPatient { get; set; }
        //public AdmittedPatient AdmittedPatient { get; set; }
    }
}
