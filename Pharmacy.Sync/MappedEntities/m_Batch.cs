using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Batch
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public string name { get; set; }
        public DateTime? expiry { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int practice_id { get; set; }
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}