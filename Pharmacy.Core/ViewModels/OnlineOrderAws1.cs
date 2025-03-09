using Pharmacy.Core.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class OnlineOrderAws1
    {
        public int? id { get; set; }
        public int? inventory_supplier_id { get; set; }
        public DateTime? order_date { get; set; }
        public int? order_number { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? delivery_time { get; set; }
        public int? delivery_charges { get; set; }
        public string payment_method { get; set; }
        public string food_panda_status { get; set; }
        public int? practice_id { get; set; }
        public float? order_grand_total { get; set; }
        public float? order_product_total { get; set; }
        public float? product_discount { get; set; }
        public string voucher_name { get; set; }
        public string voucher_discount { get; set; }
        public m_Customer customer { get; set; }
        public List<m_OrderItem> order_items { get; set; }
        public string origin { get; set; }
        public double wallet_amount { get; set; }


    }
}