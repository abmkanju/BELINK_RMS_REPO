using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_AdjustmentItem
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int item_id { get; set; }
        public int batch_id { get; set; }
        public int quantity { get; set; }
        public string reason { get; set; }
        public int adjustment_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int? store_id { get; set; } //?
        public float value { get; set; }  //?
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}
