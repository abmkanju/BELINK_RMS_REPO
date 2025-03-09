using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class PatientFormFeilds
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MR { get; set; }
        public int Name { get; set; }
        public int Email { get; set; }
        public int Phone { get; set; }
        public int Gender { get; set; }
        public int BirthDate { get; set; }
        public int Age { get; set; }
        public int RegistrationDate { get; set; }
        public int Address { get; set; }
        public int RefferedBy { get; set; }
        public int Tags { get; set; }
        public int BloodGroup { get; set; }
        public int AddPhoto { get; set; }
    }
}