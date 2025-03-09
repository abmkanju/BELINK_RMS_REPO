using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class NarcoticDrugsPrintReportVM
    {
        public int ReportId { get; set; }
        public string InvoiceId { get; set; }
        public long ItemId { get; set; }
        public string PatientName { get; set; }
        public string NarcoticDrugName { get; set; }
        public int AvailableQty { get; set; }
        public int SoldQty { get; set; }
        public int RefundQty { get; set; }
        public string SupplierName { get; set; }
        public string CreateByName { get; set; }
        public double Value { get; set; }
        public double TotalDiscount { get; set; }
        public DateTime InvoiceCreatedOn { get; set; }
        public DateTime InvoicePaynmentOn { get; set; }
        public string BatchName { get; set; }
        public string BatchExpiry { get; set; }
        public int TotalStock { get; set; }
        public double Rate { get; set; }
    }

    public class NarcoticDrugsItemwiseReportVM
    {
        public string InvoiceId { get; set; }
        public long ItemId { get; set; }
        public string NarcoticDrugName { get; set; }
        public string BatchName { get; set; }
        public string BatchExpiry { get; set; }
        public string Description { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int RefundQty { get; set; }
        public int OpeningBalance { get; set; }
        public string SupplierName { get; set; }
        public string CreateByName { get; set; }
        public string ManufacturerName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
