using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Pharmacy.Core.Entities;
namespace Pharmacy.Core.ViewModels
{
    public class PatientVM
    {
        public long PatientId { get; set; }
        public decimal? MRNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? AgeYears { get; set; }
        public int? AgeMonths { get; set; }
        public int? AgeDays { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Address { get; set; }
        public string ReferredBy { get; set; }
        public string ImagePath { get; set; }
        public string Gender { get; set; }
        public int Status { get; set; }
        public int SMSPreferrence { get; set; }
        public bool IsDeceased { get; set; }
        public List<Tag> Tags { get; set; }
        public IEnumerable<InvoiceVM> Invoices { get; set; }
        public DateTime UpdateDate { get; set;}
        public DateTime CreateDate { get; set; }
    }
}
