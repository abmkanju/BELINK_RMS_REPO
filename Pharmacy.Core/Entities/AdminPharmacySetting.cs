using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class AdminPharmacySetting
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int AdminPharmacySettingId { get; set; }
        public bool IsItemConumptionFifo { get; set; }
        /// <summary>
        /// Allow Negative Stock Consumption While Selling on Invoice Screeen.
        /// </summary>
        public bool AllowNegCons { get; set; }
        /// <summary>
        /// is pharmacy templates visible
        /// </summary>
        public bool IsPharmacyTemps { get; set; }
        public bool IsHolsStockOnInvoiceHold { get; set; }
        public bool IsUseNewestStockPrice { get; set; }
        public bool IsItemDefUnitOnPOS { get; set; }

        public bool IsItemDefPackOnPOS { get; set; }
        /// <summary>
        /// Represent days/Months counts
        /// </summary>
        public int ExpiryPeriod { get; set; }
        /// <summary>
        /// This Represents Period int Days/Months</br>
        /// 0: Days
        /// 1:Months
        /// </summary>
        public int ExpiryPeriodUnit { get; set; }
        public bool ShowRackNoInPOS { get; set; }
        public bool IsStockEditableUnitRetailPrice { get; set; }
        public bool IsPatientSelectionMandatory { get; set; }
        public bool IsEnableStores { get; set; }
        public bool IsStockFieldMandatory { get; set; }
        public bool IsConsumptionFromMultipleBatches { get; set; }
        public bool IsBonusQty { get; set; }
        public double WithholdingTax { get; set; }
        public int WithholdingTaxType { get; set; }
        public double AdvanceTax { get; set; }
        public int AdvanceTaxType { get; set; }
        public bool IsItemPrintOnStock { get; set; }
        public bool IsPOSUnitCost { get; set; }
        public bool IsPOSProfitLoss { get; set; }
        public bool IsSameItemDiffBatches { get; set; }
        public bool IsFBRUnable { get; set; }
        public double POSDiscount { get; set; }
        public int POSDiscountType { get; set; }
        public bool DiscountedPrice { get; set; }
        public bool EnableAutoBackup { get; set; }
        public string LastBackupDate { get; set; }
        public string Lcoation { get; set; }
        public string FileName { get; set; }
        public bool ReplaceBackUp { get; set; }
        public string Duration { get; set; }
        public string OnlineWebUsername { get; set; }
        public string AutoUpdateTime { get; set; }
        public bool devQueryCheck { get; set; }
        public string OnlineWebPassword { get; set; }
        public bool AskForPIN { get; set; }
        public bool NegativeSaleReturn { get; set; }
        public bool AllowAddingSameItemwithDifferentIvoice { get; set; }
        public bool LoginbyPINInsteadoffPassword { get; set; }
        public bool ShowStockItems { get; set; }
        public bool DarkMode { get; set; }
        public bool IsOnlineIntegEnable { get; set; }
        public int PracticeId { get; set; }
        public int OnlineGlobalCount { get; set; }
    }
}