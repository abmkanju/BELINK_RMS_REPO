using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ServiceModel
{
    public class m_Order
    {
       public int id { get;set; }
       public int? inventory_supplier_id { get; set; }
       public DateTime? order_date { get; set; }
       public int? order_number { get; set; }
       public DateTime? created_at {get; set;}
       public DateTime? updated_at {get; set;}
       public int? practice_id { get; set; }
       public float? order_grand_total { get; set; }
       public float? order_product_total { get; set; }
       public float? product_discount { get; set; }
       public List<m_OrderItem> order_items { get; set; }
       public string count { get; set; }
    }
}
