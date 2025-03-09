using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_StockConumptionItem
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int item_id { get; set; }
        public int batch_id { get; set; }
        public int quantity { get; set; }
        public int consumption_type { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int? invoice_id { get; set; }
        public int? user_id { get; set; }
        public int? store_id { get; set; }
        public string which_type { get; set; }
        public int practice_id { get; set; }
        public bool is_new { get; set; }
        public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}