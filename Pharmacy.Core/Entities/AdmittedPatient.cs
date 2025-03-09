using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdmittedPatient
    {
        public long AdmittedPatientId { get; set; }
        public long AdmissionNo { get; set; }
        public Patient Patient { get; set; }
        public Ward Ward { get; set; }
        public Bed Bed { get; set; }
        public string ReasonForAdmission { get; set; }
        public string Diagnosis { get; set; }
        public string Consultant { get; set; }
        public string Anesthetist { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsDischarged { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        //public ICollection<Vital> Vitals { get; set; }
        [InverseProperty("AdmittedPatients")]
        public ICollection<User> Doctors { get; set; }
        //[Required]
        //[InverseProperty("AdmittedPatient")]
        //public User User { get; set; }
        public User User { get; set; }

        [NotMapped]
        public long PatientId { get; set; }
        [NotMapped]
        public long WardId { get; set; }
    }
}
