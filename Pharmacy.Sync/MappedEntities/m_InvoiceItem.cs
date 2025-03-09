using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_InvoiceItem
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int item_id { get; set; }
        //public int template_id { get; set; } //procedure? nill
        //public int? laboratory_template_id { get; set; } //? nill
        public int batch_id { get; set; }
        public float unit_price { get; set; } //retailPrice?
        public int quantity { get; set; }
        public double amount { get; set; } //retailPrice * quantity
        public string treatment { get; set; } //? procedureDescription : nill
        public float discount { get; set; }
        public int discount_type { get; set; }
        public bool is_return { get; set; }
        public int return_quantity { get; set; }
        public float per_unit_cost { get; set; } // per unit cost
        public int serial_no { get; set; } // item serail no in table
        public int invoice_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public bool is_bed_charges { get; set; } //? false
        public string category { get; set; } //? categoryId
        //public int? doctor_id { get; set; } //? nill
        //public int? store_id { get; set; } //? nill
        public string collection_time { get; set; } //? nill
        public double? tax_value { get; set; } //? nill
        public int tax_percentage { get; set; } //? nill
        public int share_percentage { get; set; } //? nill
        public double? deduct_discount { get; set; } //? nill
        public int share_type { get; set; } //? nill
        //public int? package_id { get; set; } //? nill
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}