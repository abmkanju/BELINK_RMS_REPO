using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class StockReturnRepository : Repository<StockReturn>
    {
        PharmacyContext cxt;
        public StockReturnRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public long GetNewDocumentNumber()
        {
            long NewDocNo = 0;
            NewDocNo = cxt.StockReturns.FirstOrDefault() == null ? 100001 : cxt.StockReturns.Max(n => n.DocumentNo) + 1;
            return NewDocNo;
        }
        public Dictionary<long, string> GetStockDocumentNo()
        {
            Dictionary<long, string> list = new Dictionary<long, string>();
            list = cxt.Stocks.Where(l1 => l1.IsActive && !cxt.StockReturns.Where(x => x.IsActive).Any(l2 => l1.StockId == l2.Stock.StockId)).ToDictionary(t => t.StockId, t => Math.Round(t.DocumentNo).ToString());
            return list;
        }

        public StockReturnVM GetStockReturnById(long StockId)
        {
            StockReturnVM result = cxt.Stocks
              .Where(s => s.StockId == StockId)
              .Select(s => new StockReturnVM
              {
                  StockReturnId = s.StockId,
                  Stock = s,
                  SupplierName = s.Supplier != null ? s.Supplier.Name : "N/A",
                  StockReturnItems = (s.StockItems.Where(si => si.IsActive).Select(si => new StockReturnItemVM
                  {
                      Item = si.Item,
                      Batch = si.Batch,
                      UnitCost = si.UnitCost,
                      Unit = si.Unit,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      ConversionUnit = si.Item.ConversionUnit,
                      NetValue = si.NetValue,
                      DiscountValue = si.DiscountValue,
                      DiscountType = si.PercDiscType,
                      SalesTaxType = si.PercSalesTaxType,
                      TotalStock = (from ts in cxt.StockItems
                                    where ts.IsActive
                                        && ts.Item.ItemID == si.Item.ItemID && ts.Batch.BatchId == si.Batch.BatchId
                                    select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                      ConsumedStock = (from cs in cxt.StockConsumptionItems
                                       where cs.IsActive
                                           && cs.Item.ItemID == si.Item.ItemID && cs.Batch.BatchId == si.Batch.BatchId
                                       select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == si.Item.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()),
                      //--StockReturnQuantity--//
                      StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == si.Item.ItemID && sr.IsActive && sri.Batch.BatchId == si.Batch.BatchId select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      //--BonusQunatity--//
                      BonusStock =
                      (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == si.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                  })).ToList(),
              }).FirstOrDefault();
            return result;
        }
        public StockReturn GetStockReturnByIdEdit(long StockReturnId)
        {
            StockReturn result = cxt.StockReturns
                .Where(s => s.StockReturnId == StockReturnId)
                .Include(s => s.User)
                .Include(x => x.Stock)
                .Include(s => s.StockReturnItems).FirstOrDefault();
            return result;
        }

        public StockReturnVM GetStockReturnItemById(long StockReturnId)
        {
            StockReturnVM result = cxt.StockReturns
                .Where(s => s.StockReturnId == StockReturnId)
              .Select(s => new StockReturnVM
              {
                  StockReturnId = s.StockReturnId,
                  DocumentNo = s.DocumentNo,
                  GrandTotal = s.GrandTotal,
                  NetDiscount = s.NetDiscount,
                  NetSalesTax = s.NetSalesTax,
                  User = s.User,
                  Stock = s.Stock,
                  SupplierName = s.Stock.Supplier != null ? s.Stock.Supplier.Name : "N/A",
                  StockReturnItems = (s.StockReturnItems.Select(si => new StockReturnItemVM
                  {
                      Item = si.Item,
                      StockReturnItemId = si.StockReturnItemId,
                      Batch = si.Batch,
                      BatchName = si.Batch.BatchName,
                      ItemName = si.Item.ItemName,
                      ConversionUnit = si.Item.ConversionUnit,
                      Quantity = (from sri in cxt.StockReturnItems
                                  join sr in cxt.StockReturns on sri.StockReturn.StockReturnId
                                  equals sr.StockReturnId
                                  where sr.IsActive
                                      &&
                                      (sr.DocumentNo == s.DocumentNo && sri.Item.ItemID == si.Item.ItemID
                                      && sri.Batch.BatchId == si.Batch.BatchId)
                                  select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      UnitCost = si.UnitCost,
                      Unit = si.Unit,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountValue = si.DiscountValue,
                      TotalStock = (from ts in cxt.StockItems
                                    where ts.IsActive && ts.Item.ItemID == si.Item.ItemID
                                        && ts.Batch.BatchId == si.Batch.BatchId
                                    select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                      ConsumedStock = (from cs in cxt.StockConsumptionItems
                                       where cs.IsActive
                                           &&
                                           (cs.Item.ItemID == si.Item.ItemID
                                           && cs.Batch.BatchId == si.Batch.BatchId)
                                       select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == si.Item.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()),
                      //--StockReturnQuantity--//
                      StockReturn =
                        (from sri in cxt.StockReturnItems
                         join sr in cxt.StockReturns
                             on sri.StockReturn.StockReturnId equals sr.StockReturnId
                         where
                             sri.Item.ItemID == si.Item.ItemID && sr.IsActive && sri.Batch.BatchId == si.Batch.BatchId
                         select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      //--BonusQunatity--//
                      BonusStock =
                      (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == si.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                  })).ToList(),
              }).FirstOrDefault();
            return result;
        }

        public IPagedList<StockReturnVM> GetStockReturn(DateTime? FromDate, DateTime? ToDate, string ItemName, string StockDocumentNo, string StockRetDocumentNo, int pageNo, int PageSize)
        {
            IPagedList<StockReturnVM> r = null;
            IQueryable<StockReturn> result = cxt.StockReturns
              .Where(s => s.IsActive).AsQueryable();
            if (FromDate.HasValue)
                result = result.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate));
            if (ToDate.HasValue)
                result = result.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate));
            if (!string.IsNullOrEmpty(ItemName))
                result = result.Where(s => s.StockReturnItems.Any(i => i.Item.ItemName.ToLower().Contains(ItemName)));
            if (!string.IsNullOrEmpty(StockDocumentNo))
                result = result.Where(x => x.Stock.DocumentNo.ToString() == StockDocumentNo);
            if (!string.IsNullOrEmpty(StockRetDocumentNo))
                result = result.Where(x => x.DocumentNo.ToString() == StockRetDocumentNo);

            r = result.Select(s => new StockReturnVM
              {
                  StockReturnId = s.StockReturnId,
                  DocumentNo = s.DocumentNo,
                  GrandTotal = s.GrandTotal,
                  NetDiscount = s.NetDiscount,
                  NetSalesTax = s.NetSalesTax,
                  User = s.User,
                  Stock = s.Stock,
                  CreateDate = s.CreateDate,
                  StockReturnItems = (s.StockReturnItems.Select(si => new StockReturnItemVM
                  {
                      Item = si.Item,
                      StockReturnItemId = si.StockReturnItemId,
                      Batch = si.Batch,
                      UnitCost = si.UnitCost,
                      Quantity = si.Quantity,
                      Unit = si.Unit,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountValue = si.DiscountValue,
                  })).ToList(),
              })
              .OrderByDescending(s => s.StockReturnId)
              .ToPagedList(pageNo, PageSize);
            return r;
        }

        public void SetInactive(long StockId)
        {
            cxt.Database.ExecuteSqlCommand("Update StockReturns Set IsActive = 0, IsSynced = 0, DeletedAt = @DeletedAt WHERE StockReturnId = @StockId ", new SqlParameter("@StockId", StockId), new SqlParameter("@DeletedAt", DateTime.Now));
        }
    }
}
