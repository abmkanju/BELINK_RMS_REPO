using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ServiceModel
{
    public class m_OrderItem
    {
        public int? id { get; set; }
        public int? item_id { get; set; }
        public int? order_id { get; set; }
        public int? quantity { get;set; }
        public string size_of_strip { get; set; }
        public int? conversion_unit { get; set; }
        public double? unit_cost { get;set; }
        public double? total_amount { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public double? sale_price { get; set; }
        public double? discount_amount { get; set; }
        public string unit { get; set; }
        public string pack_type { get; set; }
        public string variant { get; set; }
    }
}
