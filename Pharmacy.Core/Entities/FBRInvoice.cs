using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Entities
{
    public class FBRInvoice
    {
        public FBRInvoice()
        {
            Items = new List<FBRInvoiceItems>();
        }
        public string InvoiceNumber { get; set; }  // Empty
        public int POSID { get; set; } // Compulsory -- POS Registration Number given by FBR on registration
        public string USIN { get; set; } //Compulsory -- Should be unique to every supplier
        public DateTime DateTime { get; set; } //Compulsory current datetime
        public string BuyerNTN { get; set; } //Optional 
        public string BuyerCNIC { get; set; } //Optional 
        public string BuyerName { get; set; } //Optional 
        public string BuyerPhoneNumber { get; set; }  //Optional 
        public int PaymentMode { get; set; } //Compulsory  { Cash, Card, Gift Voucher, Loyalty Card, Mixed Cheque}
        public double TotalSaleValue { get; set; } //Compulsory (Sum of Item Sales Values) Exclusive of Tax and Discount
        public double TotalQuantity { get; set; } //Compulsory Count of Total Items
        public double TotalBillAmount { get; set; } //Compulsory Sum of Total Bill Amount
        public double TotalTaxCharged { get; set; } //Compulsory Sum of All the Tax
        public double Discount { get; set; } //Compulsory Optional
        public double FurtherTax { get; set; } //Compulsory Optional
        public int InvoiceType { get; set; } //Compulsory New Debit Credit
        public List<FBRInvoiceItems> Items { get; set; }
    }

    public class FBRInvoiceItems
    {
        public string ItemCode { get; set; } //Compulsory Item Id Code
        public string ItemName { get; set; } //Compulsory Item Name
        public double Quantity { get; set; } //Compulsory
        public double TotalAmount { get; set; } //Compulsory
        public double SaleValue { get; set; } //Compulsory (Actual Sale Price on which Tax Calculated) Exclusive of Tax and Discount)
        public double TaxCharged { get; set; } //Compulsory 
        public int TaxRate { get; set; } //Compulsory 
        public string PCTCode { get; set; } //Compulsory 01011000 (Should be less than or equal to 8)(Pakistan Custom Tariff Code)
        public double FurtherTax { get; set; } //Optional 
        public int InvoiceType { get; set; } //Compulsory 1. New 3. Credit 11. 3rd Schedule New 12. 3rd Schedule Credit ( If an item falls under 3rd schedule category then use 11 for new item and use 12 for 3rd schedule item return invoice. )
        public double Discount { get; set; } //Optional
    }

}
