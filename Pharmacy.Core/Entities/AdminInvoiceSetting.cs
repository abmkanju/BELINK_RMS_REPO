using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminInvoiceSetting
    {
        public int AdminInvoiceSettingId { get; set; }
        public int PrintPageSize { get; set; }
        public int MarginTop { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int FontSize { get; set; }
        public int PageType { get; set; }
        public int PageOrientation { get; set; }
        public bool ShowUserName { get; set; }
        public bool ShowLogoWaterMark { get; set; }
        public bool HideRateAndQuantityInPrintFormat { get; set; }
        public bool ShowGrandtotalsInWords { get; set; }
        public bool GrandTotalsOfInvoiceAsPaymentByDefault { get; set; }
        public bool HideDuesAndAdvanceFromInvoiceAndPmntReceipts { get; set; }

        public bool PrintPractiseName { get; set; }
        public bool ShowPharmaPhone { get; set; }
        public bool ShowPharmaAddress { get; set; }

        public string InvoiceNote { get; set; }
        public bool IsOptionalBatchNo { get; set; } //We can not add invoice line item without batch 
        public bool AllowBelowCostSale { get; set; }
        public bool IsAskLoginOnInvSave { get; set; }
        public bool IsPurchaseOderA8Format { get; set; }
        public string PrinterName { get; set; }
        public string BarcodePrinterName { get; set; }
        public bool ShowPOSGrandTotal { get; set; }
        public bool ShowPOSSubTotal { get; set; }
        public bool ShowPOSService { get; set; }
        public bool ShowMultipleBatchPOS { get; set; }
        // patient details
        public bool ShowGender { get; set; }
        public bool ShowPhone { get; set; }
        public bool ShowMR { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowDOB { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowTPInsteadOfRate { get; set; }
        public bool HideLineItemDiscount { get; set; }
    }
}