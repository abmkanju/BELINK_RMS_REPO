using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Tag
    {
        // send only patient and tags list
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int tag_id { get; set; }
        public int taggable_id { get; set; } // pa
        public string taggable_type { get; set; }// "patient"
        public int tagger_id { get; set; }
        public string tagger_type { get; set; }
        public string context { get; set; }
        public DateTime created_at { get; set; }
        public int practice_id { get; set; }
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}
