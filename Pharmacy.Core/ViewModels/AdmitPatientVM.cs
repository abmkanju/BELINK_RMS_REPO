using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class AdmitPatientVM
    {
        public long AdmitPatientId { get; set; }
        public long AdmissionNo { get; set; }
        public Patient Patient { get; set; }
        public Ward Ward { get; set; }
        public Bed Bed { get; set; }
        public string ReasonForAdmission { get; set; }
        public string Diagnosis { get; set; }
        public string Consultant { get; set; }
        public ICollection<User> Doctors { get; set; }
        public string Anesthetist { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsDischarged { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }        
        public long PatientId { get; set; }
        public long WardId { get; set; }
    }
}
