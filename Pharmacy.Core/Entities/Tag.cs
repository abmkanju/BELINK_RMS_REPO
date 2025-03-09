using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class Tag
    {
        public long TagId { get; set; }
        public string TagName { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}