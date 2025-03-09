using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;

namespace Pharmacy.Sync.DAL
{
    public class UpdateResults
    {
        private PharmacyContext cxt = new PharmacyContext();

        public bool Categories(List<int> ids)
        {
            try
            {
                List<Category> obj = cxt.Categories.Where(c => ids.Contains((int)c.CategoryId)).ToList();
                if (obj != null)
                {
                    foreach (var i in obj)
                    {
                        i.IsNew = false;
                        i.IsUpdate = false;
                        i.IsSynced = true;
                        i.SyncedAt = DateTime.Now;
                        cxt.Entry(i).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Suppliers(List<int> ids)
        {
            try
            {
                List<Supplier> obj = cxt.Suppliers
                    .Where(s => ids.Contains((int)s.SupplierID))
                    .ToList();
                if (obj != null)
                {
                    foreach (var s in obj)
                    {

                        s.IsNew = false;
                        s.IsUpdate = false;
                        s.IsSynced = true;
                        s.SyncedAt = DateTime.Now;
                        cxt.Entry(s).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                cxt.Dispose();
            }
        }
        public bool Items(List<int> ids)
        {
            try
            {
                List<Item> obj = cxt.Items.Where(c => ids.Contains((int)c.ItemID)).ToList();
                if (obj != null)
                {
                    foreach (var i in obj)
                    {

                        i.IsNew = false;
                        i.IsUpdate = false;
                        i.IsSynced = true;
                        i.SyncedAt = DateTime.Now;
                        cxt.Entry(i).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Batches(List<int> ids)
        {
            try
            {
                List<Batch> obj = cxt.Batches.Where(c => ids.Contains((int)c.BatchId)).ToList();
                if (obj != null)
                {
                    foreach (var i in obj)
                    {

                        i.IsNew = false;
                        i.IsUpdate = false;
                        i.IsSynced = true;
                        i.SyncedAt = DateTime.Now;
                        cxt.Entry(i).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool PurchaseOrders(List<int> ids)
        {
            try
            {
                List<PurchaseOrder> obj = cxt.PurchaseOrders.Include(o => o.PurchaseOrderItems).Where(o => ids.Contains((int)o.PurchaseOrderId)).ToList();
                if (obj != null)
                {
                    foreach (PurchaseOrder o in obj)
                    {

                        o.IsNew = false;
                        o.IsUpdate = false;
                        o.IsSynced = true;
                        o.SyncedAt = DateTime.Now;
                        cxt.Entry(o).State = EntityState.Modified;
                        //foreach (var i in obj)
                        //{

                        //    i.IsNew = false;
                        //    i.IsUpdate = false;
                        //    i.IsSynced = true;
                        //    i.SyncedAt = DateTime.Now;
                        //    cxt.Entry(i).State = EntityState.Modified;
                        //}
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Patients(List<int> ids)
        {
            try
            {
                List<Patient> obj = cxt.Patients.Where(p => ids.Contains((int)p.PatientId)).ToList();
                if (obj != null)
                {
                    foreach (Patient p in obj)
                    {

                        p.IsNew = false;
                        p.IsUpdate = false;
                        p.IsSynced = true;
                        p.SyncedAt = DateTime.Now;
                        cxt.Entry(p).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Adjustments(List<int> ids)
        {
            try
            {
                List<Adjustment> obj = cxt.Adjustments.Include(a => a.AdjustmentItems).Where(a => ids.Contains((int)a.AdjustmentId)).ToList();
                if (obj != null)
                {
                    foreach (Adjustment a in obj)
                    {

                        a.IsNew = false;
                        a.IsUpdate = false;
                        a.IsSynced = true;
                        a.SyncedAt = DateTime.Now;
                        cxt.Entry(a).State = EntityState.Modified;
                        //foreach (AdjustmentItem i in a.AdjustmentItems)
                        //{
                        //    i.IsSynced = true;
                        //    i.SyncedAt = DateTime.Now;
                        //    cxt.Entry(i).State = EntityState.Modified;
                        //}
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StockConsumptionItems(List<int> ids)
        {
            try
            {
                List<StockConsumptionItem> obj = cxt.StockConsumptionItems.Where(sc => ids.Contains((int)sc.StockConsumptionItemId)).ToList();
                if (obj != null)
                {
                    foreach (var sc in obj)
                    {

                        sc.IsNew = false;
                        sc.IsUpdate = false;
                        sc.IsSynced = true;
                        sc.SyncedAt = DateTime.Now;
                        cxt.Entry(sc).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Stocks(List<int> ids)
        {
            try
            {
                List<Stock> obj = cxt.Stocks.Include(s => s.StockItems).Include(s => s.InvoicePayments).Where(s => ids.Contains((int)s.StockId)).ToList();
                if (obj != null)
                {
                    foreach (var s in obj)
                    {

                        s.IsNew = false;
                        s.IsUpdate = false;
                        s.IsSynced = true;
                        s.SyncedAt = DateTime.Now;
                        cxt.Entry(s).State = EntityState.Modified;
                        //foreach (StockItem i in s.StockItems)
                        //{
                        //    i.IsSynced = true;
                        //    i.SyncedAt = DateTime.Now;
                        //    cxt.Entry(i).State = EntityState.Modified;
                        //}
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Expenses(List<int> ids)
        {
            try
            {
                List<Expense> obj = cxt.Expenses.Where(e => ids.Contains((int)e.ExpenseId)).ToList();
                if (obj != null)
                {
                    foreach (var e in obj)
                    {

                        e.IsNew = false;
                        e.IsUpdate = false;
                        e.IsSynced = true;
                        e.SyncedAt = DateTime.Now;
                        cxt.Entry(e).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Invoices(List<int> ids)
        {
            try
            {
                List<Invoice> obj = cxt.Invoices
                        //.Include(i => i.InvoiceItems)
                        //.Include(i => i.InvoicePayments)
                        //.Include(i => i.StockConsumption)
                        //.Include(i => i.StockConsumption.StockConsumptions)
                        .Where(i => ids.Contains((int)i.InvoiceId))
                        .ToList();
                if (obj != null)
                {
                    foreach (var i in obj)
                    {
                        i.IsNew = false;
                        i.IsUpdate = false;
                        i.IsSynced = true;
                        i.SyncedAt = DateTime.Now;
                        cxt.Entry(i).State = EntityState.Modified;
                        //foreach (InvoiceItem ii in i.InvoiceItems)
                        //{
                        //    ii.IsSynced = true;
                        //    ii.SyncedAt = DateTime.Now; ;
                        //    cxt.Entry(ii).State = EntityState.Modified;
                        //}

                        //foreach (InvoicePayment p in i.InvoicePayments)
                        //{
                        //    p.IsSynced = true;
                        //    p.SyncedAt = DateTime.Now; ;
                        //    cxt.Entry(p).State = EntityState.Modified;
                        //}


                        //i.StockConsumption.IsNew = false;
                        //i.StockConsumption.IsUpdate = false;
                        //i.StockConsumption.IsSynced = true;
                        //i.StockConsumption.SyncedAt = DateTime.Now;
                        //cxt.Entry(i.StockConsumption).State = EntityState.Modified;

                        //foreach (StockConsumptionItem si in i.StockConsumption.StockConsumptions)
                        //{
                        //    si.IsSynced = true;
                        //    si.SyncedAt = DateTime.Now; ;
                        //    cxt.Entry(si).State = EntityState.Modified;
                        //}
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InvoicePayments(List<int> ids)
        {
            try
            {
                List<InvoicePayment> obj = cxt.invoicePayments
                        .Where(ip => ids.Contains((int)ip.InvoicePaymentId))
                        .ToList();
                if (obj != null)
                {
                    foreach (var ip in obj)
                    {
                        ip.IsNew = true;
                        ip.IsUpdate = true;
                        ip.IsSynced = true;
                        ip.SyncedAt = DateTime.Now;
                        cxt.Entry(ip).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Users(List<int> ids)
        {
            try
            {
                List<User> obj = cxt.Users.Where(e => ids.Contains((int)e.UserId)).ToList();
                if (obj != null)
                {
                    foreach (var e in obj)
                    {
                        e.IsNew = true;
                        e.IsUpdate = true;
                        e.IsSynced = true;
                        e.SyncedAt = DateTime.Now;
                        cxt.Entry(e).State = EntityState.Modified;
                    }
                    cxt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}