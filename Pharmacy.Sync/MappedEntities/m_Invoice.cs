using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Invoice
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int invoice_ref { get; set; } // 100001 
        public double total { get; set; }
        public float total_discount { get; set; }
        public int total_discount_type { get; set; }
        public int paid_total { get; set; } // total paid : sum of amount paid - refund
        public int total_unit_cost { get; set; } // save calculated value
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int? patient_id { get; set; }
        public int? user_id { get; set; }
        public int practice_id { get; set; }
        public int? appointment_id { get; set; } // nil
        public string invoice_type { get; set; } //default 0
        public bool bed_price { get; set; } //? for indoor
        public DateTime bed_pay_date { get; set; } //? for indoor
        public int is_hospital { get; set; } //?  default 0
        public int? room_type_id { get; set; } // nill
        public int balance_status { get; set; } //?
        public DateTime invoice_date { get; set; } // created at
        public int? referred_by { get; set; } // doctorid
        public int? bed_id { get; set; } //nill
        public DateTime? deleted_at { get; set; }
        public int insurance_type { get; set; } // enum
        public int? store_id { get; set; } //nill
        public int? qb_id { get; set; } // nill
        public double? total_tax { get; set; } // 0
        public string notes { get; set; }
        public int? created_by_id { get; set; } // user id
        public int priority { get; set; } // enum :  almost releatd to lab
        public int? collection_center_id { get; set; } // nill
        public double? total_balance { get; set; } // change :  not implemented in desktop so sebd ut as 0 value
        public List<m_InvoiceItem> line_items_attributes { get; set; }
        public List<m_StockConumptionItem> StockConsumptionItems { get; set; }
        public List<m_InvoicePayment> payments_attributes { get; set; }
        //    public bool is_new { get; set; }
        //    public bool is_update { get; set; }
        //    public bool _destroy { get; set; }
        //}
    }
}