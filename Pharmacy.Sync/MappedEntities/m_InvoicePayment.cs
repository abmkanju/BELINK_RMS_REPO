using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_InvoicePayment
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int invoice_id { get; set; }
        public int payment_type { get; set; }
        public int amount_paid { get; set; }
        public string reason_for_refund { get; set; }
        public string cheque_number { get; set; }
        public string bank_name { get; set; }
        public string cheque_status { get; set; }
        public DateTime payment_date { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        //public int practice_id { get; set; }
        public bool refund { get; set; } //?
        public int? user_id { get; set; }
        public int insurance_type { get; set; } //?
        public bool collect_payment { get; set; } //?
        public int? qb_id { get; set; } //?
        public int method { get; set; } //?
        public int? bank_deposit_id { get; set; } //?
        public int method_type { get; set; } //?
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        public bool _destroy { get; set; }
    }
}
