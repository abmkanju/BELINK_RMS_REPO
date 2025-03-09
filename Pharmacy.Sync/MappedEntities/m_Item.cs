using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Item
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public string item_name { get; set; }
        public string barcode { get; set; }
        public string rack { get; set; }
        public string stocking_unit { get; set; }
        public int re_ordering_level { get; set; }
        public float retail_price { get; set; }
        public int initial_stock { get; set; }
        public string chemical_name { get; set; }
        public float unit_cost { get; set; }
        public bool narcotic { get; set; }
        public string manufacturer { get; set; } // not required
        public int item_type { get; set; } // default 0
        public string item_code { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int practice_id { get; set; }
        //public int inventory_supplier_id { get; set; }// null :  excelude from object
        public int which_drug { get; set; }// default 0
        public bool is_pharmaguide { get; set; }// default false
        public int? user_id { get; set; }
        public int? item_classification_id { get; set; }// categoryID
        public List<int> inventory_supplier_ids { get; set; }
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        //public List<m_Supplier> inventory_suppliers { get; set; }
        //public bool _destroy { get; set; }
    }
}