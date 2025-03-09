using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Supplier
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string primary_contact_person_name { get; set; }
        public string primary_contact_person_phone { get; set; }
        public double opening_balance { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public int practice_id { get; set; }
        public int user_id { get; set; }
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}