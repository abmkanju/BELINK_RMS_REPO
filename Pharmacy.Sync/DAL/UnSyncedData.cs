using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Pharmacy.Sync.MappedEntities;
using Pharmacy.Infrastructure;
using Pharmacy.Core;

namespace Pharmacy.Sync.DAL
{
    public static class UnSyncedData
    {
        private static PharmacyContext cxt = new PharmacyContext();
        public static List<m_Category> NewCategories()
        {
            int practiseID = SharedVariables.PracticeId;
            List<m_Category> cats =
            cxt.Categories
                      .Where(c => !c.IsSynced && c.IsNew && c.IsActive)
                      .Select(c => new m_Category()
                      {
                          desktop_id = (int)c.CategoryId,
                          name = c.Name,
                          created_at = c.CreateDate,
                          updated_at = c.UpdateDate,
                          practice_id = SharedVariables.PracticeId,
                      })
                      .OrderBy(c => c.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
            return cats;
        }
        public static List<m_Category> UpdatedCategories()
        {
            return cxt.Categories
                      .Where(c => !c.IsSynced && c.IsUpdate && c.IsActive)
                      .Select(c => new m_Category()
                      {
                          id = (int)c.CategoryId,
                          desktop_id = (int)c.CategoryId,
                          name = c.Name,
                          updated_at = c.UpdateDate,
                          practice_id = SharedVariables.PracticeId
                      })
                      .OrderBy(c => c.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
        }
        public static List<m_Category> DeletedCategories()
        {
            return cxt.Categories
                      .Where(c => !c.IsSynced && !c.IsActive)
                      .Select(c => new m_Category()
                      {
                          desktop_id = (int)c.CategoryId,
                          practice_id = SharedVariables.PracticeId,
                      })
                      .OrderBy(c => c.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
        }

        public static List<m_Supplier> NewSuppliers()
        {
            return cxt.Suppliers
                .Where(s => !s.IsSynced && s.IsNew && s.IsActive)
                .Select(s => new m_Supplier()
                {
                    desktop_id = (int)s.SupplierID,
                    name = s.Name,
                    phone = s.Phone,
                    address = s.Address,
                    primary_contact_person_name = s.PrimaryContactPersonName,
                    primary_contact_person_phone = s.PrimaryContactPersonPhone,
                    opening_balance = s.OpeningBalance,
                    created_at = s.CreatedAt,
                    updated_at = s.UpdatedAt,
                    deleted_at = s.DeletedAt,
                    practice_id = SharedVariables.PracticeId,
                    user_id = s.User.UserId,
                })
                .OrderBy(s => s.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Supplier> UpdatedSuppliers()
        {
            return cxt.Suppliers
                .Where(s => !s.IsSynced && s.IsUpdate && s.IsActive)
                .Select(s => new m_Supplier()
                {
                    id = (int)s.SupplierID,
                    desktop_id = (int)s.SupplierID,
                    name = s.Name,
                    phone = s.Phone,
                    address = s.Address,
                    primary_contact_person_name = s.PrimaryContactPersonName,
                    primary_contact_person_phone = s.PrimaryContactPersonPhone,
                    opening_balance = s.OpeningBalance,
                    updated_at = s.UpdatedAt,
                    practice_id = SharedVariables.PracticeId,
                    user_id = s.User.UserId,
                })
                .OrderBy(s => s.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Supplier> DeletedSuppliers()
        {
            return cxt.Suppliers
                .Where(s => !s.IsSynced && !s.IsActive)
                .Select(s => new m_Supplier()
                {
                    desktop_id = (int)s.SupplierID,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(s => s.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }


        public static List<m_Item> NewItems()
        {
            return cxt.Items
                      .Where(i => !i.IsSynced && i.IsNew && i.IsActive)
                      .Select(i => new m_Item()
                      {
                          id = i.ItemID,
                          desktop_id = i.ItemID,
                          item_name = i.ItemName,
                          barcode = i.Barcode,
                          rack = i.RackNo,
                          stocking_unit = i.Unit,
                          re_ordering_level = i.ReOrderingLevel,
                          retail_price = (float)i.RetailPrice,
                          chemical_name = i.ChecmicalName,
                          unit_cost = (float)i.UnitCostPrice,
                          narcotic = i.IsNarcotic,
                          item_classification_id = (int?)i.Category.CategoryId,
                          user_id = null,//i.User.UserId,
                          created_at = i.CreateDate,
                          updated_at = i.UpdateDate,
                          deleted_at = i.DeletedAt,
                          practice_id = SharedVariables.PracticeId,
                          inventory_supplier_ids = i.Suppliers.Select(s => (int)s.SupplierID).ToList()
                          //inventory_suppliers = (i.Suppliers.Select(s => new m_Supplier()
                          //{
                          //    id = (int)s.SupplierID,
                          //    desktop_id = (int)s.SupplierID,
                          //    name = s.Name,
                          //    address = s.Address,
                          //    phone = s.Phone,
                          //    primary_contact_person_name = s.PrimaryContactPersonName,
                          //    primary_contact_person_phone = s.PrimaryContactPersonName,
                          //})).ToList()
                      })
                      .OrderBy(i => i.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
        }
        public static List<m_Item> UpdatedItems()
        {
            return cxt.Items
                      .Where(i => !i.IsSynced && i.IsUpdate && i.IsActive)
                      .Select(i => new m_Item()
                      {
                          id = i.ItemID,
                          desktop_id = i.ItemID,
                          item_name = i.ItemName,
                          barcode = i.Barcode,
                          rack = i.RackNo,
                          stocking_unit = i.Unit,
                          re_ordering_level = i.ReOrderingLevel,
                          retail_price = (float)i.RetailPrice,
                          chemical_name = i.ChecmicalName,
                          unit_cost = (float)i.UnitCostPrice,
                          narcotic = i.IsNarcotic,
                          item_classification_id = (int)i.Category.CategoryId,
                          user_id = null, //i.User.UserId,
                          created_at = i.CreateDate,
                          updated_at = i.UpdateDate,
                          deleted_at = i.DeletedAt,
                          practice_id = SharedVariables.PracticeId,
                          inventory_supplier_ids = i.Suppliers.Select(s => (int)s.SupplierID).ToList()
                          // = (i.Suppliers.Select(s => new m_Supplier()
                          //{
                          //    id = (int)s.SupplierID,
                          //    desktop_id = (int)s.SupplierID,
                          //    name = s.Name,
                          //    address = s.Address,
                          //    phone = s.Phone,
                          //    primary_contact_person_name = s.PrimaryContactPersonName,
                          //    primary_contact_person_phone = s.PrimaryContactPersonName,
                          //})).ToList()
                      })
                      .OrderBy(i => i.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
        }
        public static List<m_Item> DeletedItems()
        {
            return cxt.Items
                      .Where(i => !i.IsSynced && !i.IsActive)
                      .Select(i => new m_Item()
                      {
                          desktop_id = i.ItemID,
                          practice_id = SharedVariables.PracticeId,
                      })
                      .OrderBy(i => i.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                      .ToList();
        }


        public static List<m_Adjustment> NewAdjustments()
        {
            return cxt.Adjustments
                .Where(a => !a.IsSynced && a.IsNew && a.IsActive)
                .Select(a => new m_Adjustment()
                {
                    id = (int)a.AdjustmentId,
                    desktop_id = (int)a.AdjustmentId,
                    user_id = a.User.UserId,
                    created_at = a.CreateDate,
                    updated_at = a.UpdateDate,
                    category = "Pharmacy",
                    practice_id = SharedVariables.PracticeId,
                    adjustment_items_attributes =
                    (a.AdjustmentItems.Where(ai => ai.IsActive).Select(ai => new m_AdjustmentItem()
                    {
                        id = (int)ai.AdjustmentItemId,
                        desktop_id = (int)ai.AdjustmentItemId,
                        item_id = ai.Item.ItemID,
                        batch_id = ai.Batch.BatchId,
                        quantity = ai.Quantity,
                        reason = ai.Reason,
                        adjustment_id = (int)a.AdjustmentId,
                        created_at = ai.CreateDate,
                        updated_at = ai.UpdateDate,
                    }).ToList())
                })
                .OrderBy(a => a.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Adjustment> UpdatedAdjustments()
        {
            return cxt.Adjustments
                .Where(a => !a.IsSynced && a.IsUpdate && a.IsActive)
                .Select(a => new m_Adjustment()
                {
                    id = (int)a.AdjustmentId,
                    desktop_id = (int)a.AdjustmentId,
                    user_id = a.User.UserId,
                    created_at = a.CreateDate,
                    updated_at = a.UpdateDate,
                    category = "Pharmacy",
                    practice_id = SharedVariables.PracticeId,
                    adjustment_items_attributes =
                    (a.AdjustmentItems.Select(ai => new m_AdjustmentItem()
                    {
                        id = (int)ai.AdjustmentItemId,
                        desktop_id = (int)ai.AdjustmentItemId,
                        item_id = ai.Item.ItemID,
                        batch_id = ai.Batch.BatchId,
                        quantity = ai.Quantity,
                        reason = ai.Reason,
                        adjustment_id = (int)a.AdjustmentId,
                        created_at = ai.CreateDate,
                        updated_at = ai.UpdateDate,
                        _destroy = !ai.IsActive,
                    }).ToList())
                })
                .OrderBy(a => a.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Adjustment> DeletedAdjustments()
        {
            return cxt.Adjustments
                .Where(a => !a.IsSynced && !a.IsActive)
                .Select(a => new m_Adjustment()
                {
                    id = (int)a.AdjustmentId,
                    desktop_id = (int)a.AdjustmentId,
                    practice_id = SharedVariables.PracticeId
                })
                .OrderBy(a => a.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }


        public static List<m_Invoice> NewInvoices()
        {
            return cxt.Invoices
                .Where(i => !i.IsSynced && i.IsNew && i.IsActive)
                .Select(i => new m_Invoice()
                {
                    id = (int)i.InvoiceId,
                    desktop_id = (int)i.InvoiceId,
                    invoice_ref = (int)i.InvoiceRefNo,
                    total = i.SubTotal,
                    total_discount = (float)i.TotalDiscount,
                    total_discount_type = i.DiscountType - 1,
                    paid_total = (int)i.TotalPaid,
                    total_unit_cost = (int)i.TotalUnitCost,
                    created_at = i.CreateDate,
                    updated_at = i.UpdateDate,
                    patient_id = (i.Patient == null ? null : (int?)i.Patient.PatientId),
                    user_id = i.User.UserId,
                    practice_id = SharedVariables.PracticeId,
                    invoice_type = "inventory",
                    is_hospital = 0,
                    invoice_date = i.CreateDate,
                    deleted_at = i.DeletedAt,
                    total_tax = 0,
                    notes = i.Note,
                    total_balance = (int)i.Balance,
                    //created_by_id = null,//i.User.UserId,
                    //collection_center_id = null,
                    //qb_id = null,      
                    //store_id = null,
                    //bed_id = null,
                    //referred_by = null,
                    //room_type_id= null,
                    //appointment_id = null,

                    line_items_attributes = (i.InvoiceItems.Where(ii => ii.IsActive).Select(ii => new m_InvoiceItem()
                    {
                        id = (int)ii.InvoiceItemId,
                        desktop_id = (int)ii.InvoiceItemId,
                        item_id = ii.Item.ItemID,
                        treatment = ii.Item.ItemName,
                        batch_id = ii.Batch.BatchId,
                        unit_price = (float)ii.Rate,
                        quantity = ii.Quantity,
                        amount = (double)ii.Amount,
                        discount = (float)ii.Discount,
                        discount_type = ii.DiscountType - 1,
                        return_quantity = ii.ReturnedQuantity,
                        is_return = ii.ReturnedQuantity > 0 ? true : false,
                        per_unit_cost = (float)ii.PerUnitCostPrice,
                        serial_no = ii.SerialNo,
                        created_at = ii.CreateDate,
                        updated_at = ii.UpdateDate,
                        deleted_at = ii.DeletedAt,
                        is_bed_charges = false,
                        category = "Pharmacy",
                        _destroy = !ii.IsActive
                        //store_id = null,
                        //doctor_id = null,
                        //package_id = null
                    }).ToList()),
                    payments_attributes = (i.InvoicePayments.OrderByDescending(ip => ip.InvoicePaymentId).Select(ip => new m_InvoicePayment()
                    {
                        id = (int)ip.InvoicePaymentId,
                        desktop_id = (int)ip.InvoicePaymentId,
                        invoice_id = (int)i.InvoiceId,
                        payment_type = ip.PaymentType - 1,
                        amount_paid = (int)ip.Payment,
                        reason_for_refund = ip.RefundReason,
                        cheque_number = ip.ChequeNumber,
                        bank_name = ip.BankName,
                        cheque_status = ip.ChequeStatus,
                        payment_date = ip.PaymentDate,
                        created_at = ip.CreatedAt,
                        updated_at = ip.UpdatedAt,
                        deleted_at = ip.DeletedAt,
                        refund = ip.Payment < 0 ? true : false,
                        _destroy = !ip.IsActive
                        //user_id = null, // ip.User.UserId,                       
                        //qb_id = null,
                        //bank_deposit_id = null,
                    })).ToList()
                    //StockConsumptionItems = (i.StockConsumption.StockConsumptions.Select(sc => new m_StockConumptionItem()
                    //{
                    //    id = (int)sc.StockConsumptionItemId,
                    //    desktop_id = (int)sc.StockConsumptionItemId,
                    //    item_id = sc.Item.ItemID,
                    //    batch_id = sc.Batch.BatchId,
                    //    quantity = sc.Quantity,
                    //    consumption_type = sc.ConsumptionType - 1,
                    //    comment = sc.Comment,
                    //    created_at = sc.CreateDate,
                    //    updated_at = sc.UpdateDate,
                    //    deleted_at = sc.DeletedAt,
                    //    invoice_id = (int)i.InvoiceId,
                    //    user_id = sc.User.UserId,
                    //    practice_id = SharedVariables.PracticeId,
                    //    is_new = sc.IsNew,
                    //    is_update = sc.IsUpdate,
                    //})).ToList()
                })
                .OrderBy(i => i.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Invoice> UpdatedInvoices()
        {
            return cxt.Invoices
                .Where(i => !i.IsSynced && i.IsUpdate && i.IsActive)
                .Select(i => new m_Invoice()
                {
                    id = (int)i.InvoiceId,
                    desktop_id = (int)i.InvoiceId,
                    invoice_ref = (int)i.InvoiceRefNo,
                    total = i.SubTotal,
                    total_discount = (float)i.TotalDiscount,
                    total_discount_type = i.DiscountType - 1,
                    paid_total = (int)i.TotalPaid,
                    total_unit_cost = (int)i.TotalUnitCost,
                    created_at = i.CreateDate,
                    updated_at = i.UpdateDate,
                    patient_id = (i.Patient == null ? null : (int?)i.Patient.PatientId),
                    user_id = i.User.UserId,
                    practice_id = SharedVariables.PracticeId,
                    invoice_type = "inventory",
                    is_hospital = 0,
                    invoice_date = i.CreateDate,
                    deleted_at = i.DeletedAt,
                    total_tax = 0,
                    notes = i.Note,
                    total_balance = i.Balance,
                    //created_by_id = null,//i.User.UserId,
                    //collection_center_id = null,
                    //qb_id = null,      
                    //store_id = null,
                    //bed_id = null,
                    //referred_by = null,
                    //room_type_id= null,
                    //appointment_id = null,

                    line_items_attributes = (i.InvoiceItems.Select(ii => new m_InvoiceItem()
                    {
                        id = (int)ii.InvoiceItemId,
                        desktop_id = (int)ii.InvoiceItemId,
                        item_id = ii.Item.ItemID,
                        treatment = ii.Item.ItemName,
                        batch_id = ii.Batch.BatchId,
                        unit_price = (float)ii.Rate,
                        quantity = ii.Quantity,
                        amount = ii.Amount,
                        discount = (float)ii.Discount,
                        discount_type = ii.DiscountType - 1,
                        return_quantity = ii.ReturnedQuantity,
                        is_return = ii.ReturnedQuantity > 0 ? true : false,
                        per_unit_cost = (float)ii.PerUnitCostPrice,
                        serial_no = ii.SerialNo,
                        created_at = ii.CreateDate,
                        updated_at = ii.UpdateDate,
                        deleted_at = ii.DeletedAt,
                        is_bed_charges = false,
                        category = "Pharmacy",
                        _destroy = !ii.IsActive
                        //store_id = null,
                        //doctor_id = null,
                        //package_id = null

                    }).ToList()),
                    payments_attributes = (i.InvoicePayments.OrderByDescending(ip => ip.InvoicePaymentId).Select(ip => new m_InvoicePayment()
                    {
                        id = (int)ip.InvoicePaymentId,
                        desktop_id = (int)ip.InvoicePaymentId,
                        invoice_id = (int)i.InvoiceId,
                        payment_type = ip.PaymentType - 1,
                        amount_paid = (int)ip.Payment,
                        reason_for_refund = ip.RefundReason,
                        cheque_number = ip.ChequeNumber,
                        bank_name = ip.BankName,
                        cheque_status = ip.ChequeStatus,
                        payment_date = ip.PaymentDate,
                        created_at = ip.CreatedAt,
                        updated_at = ip.UpdatedAt,
                        deleted_at = ip.DeletedAt,
                        refund = ip.Payment < 0 ? true : false,
                        //user_id = null, // ip.User.UserId,                       
                        //qb_id = null,
                        //bank_deposit_id = null,
                    })).ToList()
                    //StockConsumptionItems = (i.StockConsumption.StockConsumptions.Select(sc => new m_StockConumptionItem()
                    //{
                    //    id = (int)sc.StockConsumptionItemId,
                    //    desktop_id = (int)sc.StockConsumptionItemId,
                    //    item_id = sc.Item.ItemID,
                    //    batch_id = sc.Batch.BatchId,
                    //    quantity = sc.Quantity,
                    //    consumption_type = sc.ConsumptionType - 1,
                    //    comment = sc.Comment,
                    //    created_at = sc.CreateDate,
                    //    updated_at = sc.UpdateDate,
                    //    deleted_at = sc.DeletedAt,
                    //    invoice_id = (int)i.InvoiceId,
                    //    user_id = sc.User.UserId,
                    //    practice_id = SharedVariables.PracticeId,
                    //    is_new = sc.IsNew,
                    //    is_update = sc.IsUpdate,
                    //})).ToList()
                })
                .OrderBy(i => i.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Invoice> DeletedInvoices()
        {
            return cxt.Invoices
                .Where(i => !i.IsSynced && !i.IsActive)
                .Select(i => new m_Invoice()
                {
                    desktop_id = (int)i.InvoiceId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(i => i.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }

        public static List<m_StockConumptionItem> NewStockConsumptionItem()
        {
            return cxt.StockConsumptionItems
                .Where(c => !c.IsSynced && c.IsNew && c.IsActive)
                .Select(sc => new m_StockConumptionItem()
                {
                    id = (int)sc.StockConsumptionItemId,
                    desktop_id = (int)sc.StockConsumptionItemId,
                    item_id = sc.Item.ItemID,
                    batch_id = sc.Batch.BatchId,
                    quantity = sc.Quantity,
                    consumption_type = sc.ConsumptionType - 1,
                    comment = sc.Comment,
                    created_at = sc.CreateDate,
                    updated_at = sc.UpdateDate,
                    deleted_at = sc.DeletedAt,
                    practice_id = SharedVariables.PracticeId,
                    invoice_id = sc.InvoiceId > 0 ? (int?)sc.InvoiceId : null,
                    which_type = "Pharmacy",
                    user_id = sc.StockConsumption.User.UserId
                })
                .OrderBy(s => s.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_StockConumptionItem> updatedStockConsumptionItem()
        {
            return cxt.StockConsumptionItems
                .Where(c => !c.IsSynced && c.IsUpdate && c.IsActive)
                .Select(sc => new m_StockConumptionItem()
                {
                    id = (int)sc.StockConsumptionItemId,
                    desktop_id = (int)sc.StockConsumptionItemId,
                    item_id = sc.Item.ItemID,
                    batch_id = sc.Batch.BatchId,
                    quantity = sc.Quantity,
                    consumption_type = sc.ConsumptionType - 1,
                    comment = sc.Comment,
                    created_at = sc.CreateDate,
                    updated_at = sc.UpdateDate,
                    deleted_at = sc.DeletedAt,
                    practice_id = SharedVariables.PracticeId,
                    invoice_id = sc.InvoiceId > 0 ? (int?)sc.InvoiceId : null,
                    which_type = "Pharmacy",
                    user_id = sc.StockConsumption.User.UserId
                })
                .OrderBy(s => s.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_StockConumptionItem> DeletedStockConsumptionItem()
        {
            return cxt.StockConsumptionItems
                .Where(c => !c.IsSynced && !c.IsActive)
                .Select(sc => new m_StockConumptionItem()
                {
                    desktop_id = (int)sc.StockConsumptionItemId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(s => s.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }

        public static List<m_Batch> NewBatches()
        {
            return cxt.Batches
                .Where(b => !b.IsSynced && b.IsNew && b.IsActive)
                .Select(b => new m_Batch()
                {
                    id = b.BatchId,
                    desktop_id = b.BatchId,
                    name = b.BatchName,
                    expiry = b.Expiry,
                    created_at = b.CreatedAt,
                    updated_at = b.UpdatedAt,
                    practice_id = SharedVariables.PracticeId,
                    is_new = b.IsNew,
                    is_update = b.IsUpdate,
                    _destroy = !b.IsActive,
                })
                .OrderBy(b => b.desktop_id)
                .ToList();
        }
        public static List<m_Batch> UpdatedBatches()
        {
            return cxt.Batches
                .Where(b => !b.IsSynced && b.IsUpdate && b.IsActive)
                .Select(b => new m_Batch()
                {
                    id = b.BatchId,
                    desktop_id = b.BatchId,
                    name = b.BatchName,
                    expiry = b.Expiry,
                    created_at = b.CreatedAt,
                    updated_at = b.UpdatedAt,
                    practice_id = SharedVariables.PracticeId,
                    is_new = b.IsNew,
                    is_update = b.IsUpdate,
                    _destroy = !b.IsActive,
                })
                .OrderBy(b => b.desktop_id)
                .ToList();
        }
        public static List<m_Batch> DeletedBatches()
        {
            return cxt.Batches
                .Where(b => !b.IsSynced && !b.IsActive)
                .Select(b => new m_Batch()
                {
                    desktop_id = b.BatchId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(b => b.desktop_id)
                .ToList();
        }

        public static List<m_Stock> NewStocks()
        {
            return cxt.Stocks
                .Where(s => !s.IsSynced && s.IsNew && s.IsActive)
                .Select(s => new m_Stock()
                {
                    id = (int)s.StockId,
                    desktop_id = (int)s.StockId,
                    document_number = (int)s.DocumentNo,
                    invoice_number = s.SupplierInvoiceNo,
                    supplier_invoice_date = s.SupplierIvoiceDate,
                    grand_total = (float)s.GrandTotal,
                    paid_total = (float)s.TotalPaid,
                    stock_date = s.StockDate,
                    deleted_at = s.DeletedAt,
                    order_id = (s.PurchaseOrder == null ? null : (int?)s.PurchaseOrder.PurchaseOrderId),
                    user_id = s.User.UserId,
                    practice_id = SharedVariables.PracticeId,
                    inventory_supplier_id = s.Supplier == null ? null : (int?)s.Supplier.SupplierID,
                    which_type = "Pharmacy",
                    stock_items_attributes = (s.StockItems.Where(si => si.IsActive).Select(si => new m_StockItem()
                    {
                        id = (int)si.StockItemId,
                        desktop_id = (int)si.StockItemId,
                        item_id = si.Item.ItemID,
                        quantity = si.Quantity,
                        per_unit_cost = (float)si.UnitCost,
                        total_value = (float)si.TotalCost,
                        unit_sales_value = (float)si.RetailPrice,
                        batch_id = si.Batch.BatchId,

                        discount = (float)si.Discount,
                        discount_value = 0,
                        sale_value = 0,
                        stock_discount_type = si.DiscountType,

                        sales_tax = (float)si.SalesTax,
                        sales_tax_type = si.SalesTaxType,

                        net_value = (float)si.NetValue,
                        created_at = si.CreatedAt,
                        updated_at = si.UpdatedAt,
                        deleted_at = si.DeletedAt,
                    })).ToList()
                    //InvoicePayments = (s.InvoicePayments.Select(sp => new m_Expense()
                    //{
                    //    id = (int)sp.ExpenseId,
                    //    desktop_id = (int)sp.ExpenseId,
                    //    description = sp.description,
                    //    payment_mode = sp.PaymentMode - 1,
                    //    date = sp.Date,
                    //    created_at = sp.CreatedAt,
                    //    updated_at = sp.UpdatedAt,
                    //    deleted_at = sp.DeletedAt,
                    //    practice_id = SharedVariables.PracticeId,
                    //    voucher_no = sp.VoucherNo,
                    //    auto_added = sp.AutoAdded,
                    //    amount = sp.Amount,
                    //    stock_id = (int)sp.Stock.StockId,
                    //    inventory_supplier_id = (int)sp.Supplier.SupplierID,
                    //    is_new = sp.IsNew,
                    //    is_update = sp.IsUpdate,
                    //    _destroy = !sp.IsActive,
                    //})).ToList(),
                })
                .OrderBy(s => s.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Stock> UpdatedStocks()
        {
            return cxt.Stocks
                .Where(s => !s.IsSynced && s.IsUpdate && s.IsActive)
                .Select(s => new m_Stock()
                {
                    id = (int)s.StockId,
                    desktop_id = (int)s.StockId,
                    document_number = (int)s.DocumentNo,
                    invoice_number = s.SupplierInvoiceNo,
                    supplier_invoice_date = s.SupplierIvoiceDate,
                    grand_total = (float)s.GrandTotal,
                    paid_total = (float)s.TotalPaid,
                    stock_date = s.StockDate,
                    deleted_at = s.DeletedAt,
                    order_id = (s.PurchaseOrder == null ? null : (int?)s.PurchaseOrder.PurchaseOrderId),
                    user_id = s.User.UserId,
                    practice_id = SharedVariables.PracticeId,
                    inventory_supplier_id = s.Supplier == null ? null : (int?)s.Supplier.SupplierID,
                    which_type = "Pharmacy",
                    stock_items_attributes = (s.StockItems.Select(si => new m_StockItem()
                    {
                        id = (int)si.StockItemId,
                        desktop_id = (int)si.StockItemId,
                        item_id = si.Item.ItemID,
                        quantity = si.Quantity,
                        per_unit_cost = (float)si.UnitCost,
                        total_value = (float)si.TotalCost,
                        unit_sales_value = (float)si.RetailPrice,
                        batch_id = si.Batch.BatchId,

                        discount = (float)si.Discount,
                        discount_value = 0,
                        sale_value = 0,
                        stock_discount_type = si.DiscountType,

                        sales_tax = (float)si.SalesTax,
                        sales_tax_type = si.SalesTaxType,

                        net_value = (float)si.NetValue,
                        created_at = si.CreatedAt,
                        updated_at = si.UpdatedAt,
                        deleted_at = si.DeletedAt,
                        _destroy = si.IsActive ? "false" : "1"
                    })).ToList()
                    //InvoicePayments = (s.InvoicePayments.Select(sp => new m_Expense()
                    //{
                    //    id = (int)sp.ExpenseId,
                    //    desktop_id = (int)sp.ExpenseId,
                    //    description = sp.description,
                    //    payment_mode = sp.PaymentMode - 1,
                    //    date = sp.Date,
                    //    created_at = sp.CreatedAt,
                    //    updated_at = sp.UpdatedAt,
                    //    deleted_at = sp.DeletedAt,
                    //    practice_id = SharedVariables.PracticeId,
                    //    voucher_no = sp.VoucherNo,
                    //    auto_added = sp.AutoAdded,
                    //    amount = sp.Amount,
                    //    stock_id = (int)sp.Stock.StockId,
                    //    inventory_supplier_id = (int)sp.Supplier.SupplierID,
                    //    is_new = sp.IsNew,
                    //    is_update = sp.IsUpdate,
                    //    _destroy = !sp.IsActive,
                    //})).ToList(),
                })
                .OrderBy(s => s.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Stock> DeletedStocks()
        {
            return cxt.Stocks
                .Where(s => !s.IsSynced && !s.IsActive)
                .Select(s => new m_Stock()
                {
                    desktop_id = (int)s.StockId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(s => s.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }

        public static List<m_Expense> NewExpenses()
        {
            return cxt.Expenses
                .Where(e => !e.IsSynced && e.IsNew && e.IsActive)
                .Select(e => new m_Expense()
                {
                    id = (int)e.ExpenseId,
                    desktop_id = (int)e.ExpenseId,
                    description = e.description,
                    payment_mode = e.PaymentMode - 1,
                    date = e.Date,
                    created_at = e.CreatedAt,
                    updated_at = e.UpdatedAt,
                    deleted_at = e.DeletedAt,
                    practice_id = SharedVariables.PracticeId,
                    voucher_no = e.VoucherNo,
                    auto_added = e.AutoAdded,
                    amount = e.Amount,
                    stock_id = (int)e.Stock.StockId,
                    inventory_supplier_id = (int)e.Supplier.SupplierID,
                    category_id = null,
                })
                .OrderBy(e => e.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Expense> UpdatedExpenses()
        {
            return cxt.Expenses
                .Where(e => !e.IsSynced && e.IsUpdate && e.IsActive)
                .Select(e => new m_Expense()
                {
                    id = (int)e.ExpenseId,
                    desktop_id = (int)e.ExpenseId,
                    description = e.description,
                    payment_mode = e.PaymentMode - 1,
                    date = e.Date,
                    created_at = e.CreatedAt,
                    updated_at = e.UpdatedAt,
                    deleted_at = e.DeletedAt,
                    practice_id = SharedVariables.PracticeId,
                    voucher_no = e.VoucherNo,
                    auto_added = e.AutoAdded,
                    amount = e.Amount,
                    stock_id = (int)e.Stock.StockId,
                    inventory_supplier_id = (int)e.Supplier.SupplierID,
                    category_id = null,
                })
                .OrderBy(e => e.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Expense> DeletedExpenses()
        {
            return cxt.Expenses
                .Where(e => !e.IsSynced && !e.IsActive)
                .Select(e => new m_Expense()
                {
                    desktop_id = (int)e.ExpenseId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(e => e.desktop_id)
                .Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }


        public static List<m_Patient> NewPatients()
        {
            return cxt.Patients
                .Where(p => !p.IsSynced && p.IsNew && p.IsActive)
                .Select(p => new m_Patient()
                {
                    id = (int)p.PatientId,
                    desktop_id = (int)p.PatientId,
                    patient_id = p.MRNo.ToString(),
                    name = p.Name,
                    email = p.Email,
                    phone = p.Phone,
                    gender = p.Gender,
                    birth_date = p.DateOfBirth,
                    practice_id = SharedVariables.PracticeId,
                    created_at = p.CreateDate,
                    updated_at = p.UpdateDate,
                    phone_home = p.Phone,
                    phone_office = p.Phone2,
                    age_y = p.AgeYears,
                    age_m = p.AgeMonths,
                    age_d = p.AgeDays,
                    is_admin_created = true,
                    referred_by = p.ReferredBy,
                    date = p.RegistrationDate,
                    country_code_1 = p.CountryCode1,
                    country_code_2 = p.CountryCode2,
                    country_code_3 = p.CountryCode3,
                    //user_id = p.User.UserId,
                    blood_type = p.BloodType - 1,
                    which_type = 0,
                    patient_type = 0,
                })
                .OrderBy(p => p.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Patient> UpdatedPatients()
        {
            return cxt.Patients
                .Where(p => !p.IsSynced && p.IsUpdate && p.IsActive)
                .Select(p => new m_Patient()
                {
                    id = (int)p.PatientId,
                    desktop_id = (int)p.PatientId,
                    name = p.Name,
                    email = p.Email,
                    phone = p.Phone,
                    gender = p.Gender,
                    birth_date = p.DateOfBirth,
                    practice_id = SharedVariables.PracticeId,
                    created_at = p.CreateDate,
                    updated_at = p.UpdateDate,
                    phone_home = p.Phone,
                    phone_office = p.Phone2,
                    age_y = p.AgeYears,
                    age_m = p.AgeMonths,
                    age_d = p.AgeDays,
                    is_admin_created = true,
                    referred_by = p.ReferredBy,
                    date = p.RegistrationDate,
                    country_code_1 = p.CountryCode1,
                    country_code_2 = p.CountryCode2,
                    country_code_3 = p.CountryCode3,
                    //user_id = p.User.UserId,
                    blood_type = p.BloodType - 1,
                    which_type = 0,
                    patient_type = 0,
                })
                .OrderBy(p => p.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_Patient> DeletedPatients()
        {
            return cxt.Patients
                .Where(p => !p.IsSynced && !p.IsActive)
                .Select(p => new m_Patient()
                {
                    desktop_id = (int)p.PatientId,
                    practice_id = SharedVariables.PracticeId,
                })
                .OrderBy(p => p.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }


        public static List<m_PurchaseOrder> NewPurchaseOrders()
        {
            return cxt.PurchaseOrders
                 .Where(p => !p.IsSynced && p.IsNew && p.IsActive)
                 .Select(p => new m_PurchaseOrder()
                 {
                     id = (int)p.PurchaseOrderId,
                     desktop_id = (int)p.PurchaseOrderId,
                     order_number = (int)p.PurchaseOrderNo,
                     inventory_supplier_id = (p.Supplier == null ? null : (int?)p.Supplier.SupplierID),
                     order_grand_total = (float)p.TotalAmount,
                     user_id = p.User.UserId,
                     created_at = p.CreateDate,
                     updated_at = p.UpdateDate,
                     practice_id = SharedVariables.PracticeId,
                     which_type = "Pharmacy",
                     order_items_attributes = (p.PurchaseOrderItems.Where(pi => pi.IsActive).Select(pi => new m_PurchaseOrderItem()
                     {
                         id = (int)pi.PurchaseOrderItemId,
                         desktop_id = (int)pi.PurchaseOrderItemId,
                         order_id = (int)p.PurchaseOrderId,
                         item_id = pi.Item.ItemID,
                         quantity = pi.Quantity,
                         unit_cost = (float)pi.UnitCost,
                         total_amount = (float)pi.TotalAmount,
                         created_at = pi.CreateDate,
                         updated_at = pi.UpdateDate,
                         _destroy = !pi.IsActive,
                     })).ToList()
                 })
                 .OrderBy(p => p.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                 .ToList();
        }
        public static List<m_PurchaseOrder> UpdatedPurchaseOrders()
        {
            return cxt.PurchaseOrders
                 .Where(p => !p.IsSynced && p.IsUpdate && p.IsActive)
                 .Select(p => new m_PurchaseOrder()
                 {
                     id = (int)p.PurchaseOrderId,
                     desktop_id = (int)p.PurchaseOrderId,
                     order_number = (int)p.PurchaseOrderNo,
                     inventory_supplier_id = (p.Supplier == null ? null : (int?)p.Supplier.SupplierID),
                     order_grand_total = (float)p.TotalAmount,
                     user_id = p.User.UserId,
                     created_at = p.CreateDate,
                     updated_at = p.UpdateDate,
                     practice_id = SharedVariables.PracticeId,
                     which_type = "Pharmacy",
                     order_items_attributes = (p.PurchaseOrderItems.Select(pi => new m_PurchaseOrderItem()
                     {
                         id = (int)pi.PurchaseOrderItemId,
                         desktop_id = (int)pi.PurchaseOrderItemId,
                         order_id = (int)p.PurchaseOrderId,
                         item_id = pi.Item.ItemID,
                         quantity = pi.Quantity,
                         unit_cost = (float)pi.UnitCost,
                         total_amount = (float)pi.TotalAmount,
                         created_at = pi.CreateDate,
                         updated_at = pi.UpdateDate,
                         is_new = pi.IsNew,
                         is_update = pi.IsUpdate,
                         _destroy = !pi.IsActive,
                     })).ToList()
                 })
                 .OrderBy(p => p.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                 .ToList();
        }
        public static List<m_PurchaseOrder> DeletedPurchaseOrders()
        {
            return cxt.PurchaseOrders
                 .Where(p => !p.IsSynced && !p.IsActive)
                 .Select(p => new m_PurchaseOrder()
                 {
                     id = (int)p.PurchaseOrderId,
                     desktop_id = (int)p.PurchaseOrderId,
                 })
                 .OrderBy(p => p.desktop_id)
                 .Take(SharedVariables.API_PerBatchCount)
                 .ToList();
        }


        //public static List<m_InvoicePayment> NewInvoicePayments()
        //{
        //    return cxt.invoicePayments
        //        .Where(p => !p.IsSynced && p.IsNew && p.IsActive)
        //        .Select(p => new m_InvoicePayment()
        //        {
        //            id = (int)p.InvoicePaymentId,
        //            desktop_id = (int)p.InvoicePaymentId,
        //            invoice_id = (int)p.Invoice.InvoiceId,
        //            payment_type = p.PaymentType - 1,
        //            amount_paid = (int)p.Payment,
        //            reason_for_refund = p.RefundReason,
        //            cheque_number = p.ChequeNumber,
        //            bank_name = p.BankName,
        //            cheque_status = p.ChequeStatus,
        //            payment_date = p.PaymentDate,
        //            created_at = p.CreatedAt,
        //            updated_at = p.UpdatedAt,
        //            deleted_at = p.DeletedAt,
        //            refund = p.Payment < 0 ? true : false,
        //            user_id = p.User.UserId,
        //            _destroy = !p.IsActive,
        //        })
        //        .OrderBy(e => e.desktop_id)
        //        .Take(SharedVariables.API_PerBatchCount)
        //        .ToList();
        //}
        //public static List<m_InvoicePayment> UpdatedInvoicePayments()
        //{
        //    return cxt.invoicePayments
        //        .Where(p => !p.IsSynced && p.IsUpdate && p.IsActive)
        //        .Select(p => new m_InvoicePayment()
        //        {
        //            id = (int)p.InvoicePaymentId,
        //            desktop_id = (int)p.InvoicePaymentId,
        //            invoice_id = (int)p.Invoice.InvoiceId,
        //            payment_type = p.PaymentType - 1,
        //            amount_paid = (int)p.Payment,
        //            reason_for_refund = p.RefundReason,
        //            cheque_number = p.ChequeNumber,
        //            bank_name = p.BankName,
        //            cheque_status = p.ChequeStatus,
        //            payment_date = p.PaymentDate,
        //            created_at = p.CreatedAt,
        //            updated_at = p.UpdatedAt,
        //            deleted_at = p.DeletedAt,
        //            refund = p.Payment < 0 ? true : false,
        //            user_id = p.User.UserId,
        //            _destroy = !p.IsActive,
        //        })
        //        .OrderBy(e => e.desktop_id)
        //        .Take(SharedVariables.API_PerBatchCount)
        //        .ToList();
        //}
        //public static List<m_InvoicePayment> DeletedInvoicePayments()
        //{
        //    return cxt.invoicePayments
        //        .Where(p => !p.IsSynced && !p.IsActive)
        //        .Select(p => new m_InvoicePayment()
        //        {
        //            desktop_id = (int)p.InvoicePaymentId,
        //            practice_id = SharedVariables.PracticeId
        //        })
        //        .OrderBy(e => e.desktop_id)
        //        .Take(SharedVariables.API_PerBatchCount)
        //        .ToList();
        //}


        public static List<m_User> NewUsers()
        {
            return cxt.Users
                .Where(u => !u.IsSynced && u.IsNew && u.IsActive)
                .Select(u => new m_User()
                {
                    id = (int)u.UserId,
                    desktop_id = (int)u.UserId,
                    first_name = u.UserName,
                    email = u.Email,
                    phone = u.Phone,
                    password = u.Password,
                    created_at = u.CreateDate,
                    updated_at = u.UpdateDate,
                    practice_id = SharedVariables.PracticeId,
                    user_practices = (u.UserRoles.Select(ur => new m_userRole
                    {
                        role = ur.UserRoleId,
                        practice_id = SharedVariables.PracticeId
                    })).ToList()
                })
                .OrderBy(e => e.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_User> UpdatedUsers()
        {
            return cxt.Users
                .Where(u => !u.IsSynced && u.IsUpdate && u.IsActive)
                .Select(u => new m_User()
                {
                    id = (int)u.UserId,
                    desktop_id = (int)u.UserId,
                    first_name = u.UserName,
                    email = u.Email,
                    phone = u.Phone,
                    password = u.Password,
                    practice_id = SharedVariables.PracticeId,
                    created_at = u.CreateDate,
                    updated_at = u.UpdateDate,
                    user_practices = (u.UserRoles.Select(ur => new m_userRole
                    {
                        role = ur.UserRoleId,
                        practice_id = SharedVariables.PracticeId
                    })).ToList()
                })
                .OrderBy(e => e.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
        public static List<m_User> DeletedUsers()
        {
            return cxt.Users
                .Where(u => !u.IsSynced && !u.IsActive)
                .Select(u => new m_User()
                {
                    id = (int)u.UserId,
                    desktop_id = (int)u.UserId,
                })
                .OrderBy(e => e.desktop_id)
                //.Take(SharedVariables.API_PerBatchCount)
                .ToList();
        }
    }
}