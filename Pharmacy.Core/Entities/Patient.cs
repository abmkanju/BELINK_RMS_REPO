using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Patient
    {
        public long PatientId{ get; set; }
        public decimal? MRNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public int? AgeYears { get; set; }
        public int? AgeMonths { get; set; }
        public int? AgeDays { get; set; }
        public string CountryCode1 { get; set; }
        public string CountryCode2 { get; set; }
        public string CountryCode3 { get; set; }
        public int BloodType { get; set; }
        public string Address { get; set; }
        public string ReferredBy { get; set; }
        public string ImagePath { get; set; }
        public string Gender { get; set; }
        public int Status { get; set; }
        public int SMSPreferrence { get; set; }
        public bool IsDeceased { get; set; }
        public double Dues { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SyncedAt { get; set; }

        public User User { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Tag> Tags { get; set; }

        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }     
    }
}