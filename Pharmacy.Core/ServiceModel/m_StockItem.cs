using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ServiceModel
{
    public class m_StockItem
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; }
        public float per_unit_cost { get; set; } //unitCost
        public float total_value { get; set; } //? totalCost
        public float unit_sales_value { get; set; }//? retailPrice
        public int batch_id { get; set; }
        public float discount { get; set; }
        public int stock_discount_type { get; set; } //?DicountType
        public float sales_tax { get; set; }
        public int unit { get; set; }
        public int sales_tax_type { get; set; }
        public float net_value { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int discount_value { get; set; } // calculated discount value : currently working on it
        public int? sale_value { get; set; }// calculated sale tax value : currently working on it
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        public string _destroy { get; set; }
        public int? store_id { get; set; }

    }
}
