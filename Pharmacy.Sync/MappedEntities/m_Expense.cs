using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Expense
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public string description { get; set; }
        public int payment_mode { get; set; }
        public int? category_id { get; set; }
        public DateTime date { get; set; }        
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int practice_id { get; set; }
        public int voucher_no { get; set; }
        public bool AutoAdded { get; set; }
        public double amount { get; set; }
        public int stock_id { get; set; }
        public int inventory_supplier_id { get; set; }
        //public int line_item_id { get; set; }
        //public int bank_account_id { get; set; }
        //public int payment_id { get; set; }
        public bool auto_added { get; set; } // default : true 
        //public bool employee_id { get; set; }      
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        //public bool _destroy { get; set; }
    }
}