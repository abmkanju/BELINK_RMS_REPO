using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Adjustment
    {        
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int? user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime deleted_at { get; set; }        
        public int practice_id { get; set; }
        public string category { get; set; }        
        public string which_type { get; set; }
        public List<m_AdjustmentItem> adjustment_items_attributes { get; set; }
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}