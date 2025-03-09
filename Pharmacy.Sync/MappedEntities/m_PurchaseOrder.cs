using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_PurchaseOrder
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int order_number { get; set; }
        public int? inventory_supplier_id { get; set; }
        public DateTime order_date { get; set; }
        public float order_grand_total { get; set; }
        public int? user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int practice_id { get; set; }
        public int? purchase_requisition_id { get; set; }//?
        public string which_type { get; set; } //?
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public List<m_PurchaseOrderItem> order_items_attributes { get; set; }
    }
}
