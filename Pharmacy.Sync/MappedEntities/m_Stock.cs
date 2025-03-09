using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.MappedEntities
{
    public class m_Stock
    {
        public int id { get; set; }
        public int desktop_id { get; set; }
        public int document_number { get; set; }
        public string invoice_number { get; set; } // supplierInvoiceNumer
        public DateTime supplier_invoice_date { get; set; }
        public float grand_total { get; set; } 
        public float paid_total { get; set; }
        public DateTime stock_date { get; set; }//add stock date  : nto same as createAT
        public DateTime? deleted_at { get; set; }
        public int? order_id { get; set; }
        public int? inventory_supplier_id { get; set; }// supplier ID
        public int? user_id { get; set; }
        public int practice_id { get; set; }
        public List<m_StockItem> stock_items_attributes { get; set; }
        //relevant payment
        public ICollection<m_Expense> InvoicePayments { get; set; }
        public string which_type { get; set; }// default 0
        //public bool is_new { get; set; }
        //public bool is_update { get; set; }
        //public bool _destroy { get; set; }
    }
}