using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Data.SqlClient;
using Pharmacy.Core;
using System.Data.Entity.SqlServer;

namespace Pharmacy.Infrastructure.Repository
{
    public class StockRepository : Repository<Stock>, IStock
    {
        PharmacyContext cxt;
        public StockRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public decimal GetNewDocumentNumber()
        {
            decimal NewDocNo = 0;
            if (cxt.Stocks.FirstOrDefault() == null)
            {
                NewDocNo = 100001;
            }
            else
            {
                NewDocNo = cxt.Stocks.Max(n => n.DocumentNo) + 1;
            }
            return Math.Round(NewDocNo);
        }

        public void AddStockItem()
        {

        }

        public void InsertStock(Stock objStock)
        {
            objStock = cxt.Stocks.Add(objStock);
            cxt.SaveChanges();
        }

        //public IPagedList<StockVM> GetStocks(int pageNo, int PageSize)
        //{
        //    //IPagedList<Stock> result = cxt.Stocks
        //    //    .Where(s => s.IsActive == true)
        //    //    .Include(s => s.StockItems.Select(i => i.Item))
        //    //    .Include(s => s.StockItems.Select(i => i.Batch))
        //    //    .Include(s => s.Supplier)
        //    //    .OrderByDescending(s => s.StockId)
        //    //    .ToPagedList<Stock>(pageNo, PageSize);
        //    //return result;

        //    IPagedList<StockVM> result = cxt.Stocks
        //        .Where(s => s.IsActive)
        //        .Select(s => new StockVM
        //        {
        //            StockId = s.StockId,
        //            DocumentNo = s.DocumentNo,
        //            SupplierInvoiceNo = s.SupplierInvoiceNo,
        //            ImagePath = s.ImagePath,
        //            CreateDate = s.CreateDate,
        //            UpdateDate = s.UpdateDate,
        //            IsActive = s.IsActive,
        //            SupplierIvoiceDate = s.SupplierIvoiceDate,
        //            StockItems = (s.StockItems.Select(si => new StockItemVM
        //            {
        //                IsActive = si.IsActive,
        //                Item = si.Item,
        //                Batch = si.Batch,
        //                Quantity = si.Quantity,
        //                UnitCost = si.UnitCost,
        //                RetailPrice = si.RetailPrice,
        //                TotalCost = si.TotalCost,
        //                Discount = si.Discount,
        //                SalesTax = si.SalesTax,
        //                NetValue = si.NetValue,
        //                DiscountType = si.DiscountType,
        //                SalesTaxType = si.SalesTaxType,
        //                CreatedAt = si.CreatedAt,
        //                StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
        //            })).ToList(),
        //            Supplier = s.Supplier
        //        })
        //        .OrderByDescending(s => s.StockId)
        //        .ToPagedList(pageNo, PageSize);
        //    return result;
        //}
        public StockVM GetStockDetailById(long StockId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            StockVM result = cxt.Stocks
              .Where(s => s.StockId == StockId)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  TotalDiscount = s.TotalDiscount,
                  TotalSaleTax = s.TotalSaleTax,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  Ishold= s.Ishold,
                  GrandTotal = s.GrandTotal,
                  WithholdingTax = s.WithholdingTax,
                  WithholdingTaxType = s.WithholdingTaxType,
                  AdvanceWithholdingTax = s.AdvanceWithholdingTax,
                  AdvanceWithholdingTaxType = s.AdvanceWithholdingTaxType,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  SupplierName = s.Supplier != null ? s.Supplier.Name : "",
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Unit = si.Unit,
                      ItemName = si.Item.ItemName,
                      BatchName = si.Batch.BatchName,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      WHT = si.WHT,
                      WHTType = si.WHTType,
                      AdvanceWithholdingTax = si.AdvanceWHT,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId).FirstOrDefault();

            return result;
        }
        public StockVM GetStockById_Show(long StockId)
        {
            //Stock result = cxt.Stocks
            //    .Where(s => s.StockId == StockId)
            //    .Include(s => s.StockItems.Select(i => i.Item))
            //    .Include(s => s.StockItems.Select(i => i.Batch))
            //    .Include(s => s.Supplier)
            //    .OrderByDescending(s => s.StockId).FirstOrDefault();
            //return result;

            StockVM result = cxt.Stocks
                .Where(s => s.StockId == StockId)
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  Ishold = s.Ishold,
                  NetDiscount = s.NetDiscount,
                  NetSaleTax = s.NetSaleTax,
                  NetSaleTaxType = s.NetSaleTaxType,
                  NetDiscountType = s.NetDiscountType,
                  TotalDiscount = s.TotalDiscount,
                  TotalSaleTax = s.TotalSaleTax,
                  UserName = s.User.UserName,
                  GrandTotal =s.GrandTotal,
                  WithholdingTax = s.WithholdingTax,
                  WithholdingTaxType = s.WithholdingTaxType,
                  AdvanceWithholdingTax = s.AdvanceWithholdingTax,
                  AdvanceWithholdingTaxType = s.AdvanceWithholdingTaxType,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  StockItems = (s.StockItems.Where(si => si.IsActive == true).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      StockItemId = si.StockItemId,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      BonusDiscount = si.BonusDiscount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountVal = si.DiscountValue,
                      PercDiscType = si.PercDiscType,
                      SalesTaxType = si.SalesTaxType,
                      SalesTaxVal = si.SalesTaxValue,
                      PercSalesTaxType = si.PercSalesTaxType,
                      CreatedAt = si.CreatedAt,
                      
                      Store = si.Store,
                      WHT = si.WHT,
                      WHTType = si.WHTType,
                      AdWHT = si.AdvanceWHT,
                      AdWHTType = si.AdvanceWHTType,
                      TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == si.Item.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                      ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == si.Item.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()),
                      //--StockReturnQuantity--//
                      StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == si.Item.ItemID && sr.IsActive && sri.Batch.BatchId == si.Batch.BatchId select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                  })).ToList(),
                  Supplier = s.Supplier
              }).FirstOrDefault();

            return result;
        }
        public StockVM GetStockById_ShowforHold(long StockId)
        {
            //Stock result = cxt.Stocks
            //    .Where(s => s.StockId == StockId)
            //    .Include(s => s.StockItems.Select(i => i.Item))
            //    .Include(s => s.StockItems.Select(i => i.Batch))
            //    .Include(s => s.Supplier)
            //    .OrderByDescending(s => s.StockId).FirstOrDefault();
            //return result;

            StockVM result = cxt.Stocks
                .Where(s => s.StockId == StockId)
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  Ishold = s.Ishold,
                  NetDiscount = s.NetDiscount,
                  NetSaleTax = s.NetSaleTax,
                  NetSaleTaxType = s.NetSaleTaxType,
                  NetDiscountType = s.NetDiscountType,
                  TotalDiscount = s.TotalDiscount,
                  TotalSaleTax = s.TotalSaleTax,
                  WithholdingTax = s.WithholdingTax,
                  WithholdingTaxType = s.WithholdingTaxType,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  StockItems = (s.StockItems.Where(si => si.IsActive == false).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      StockItemId = si.StockItemId,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      BonusDiscount = si.BonusDiscount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountVal = si.DiscountValue,
                      PercDiscType = si.PercDiscType,
                      SalesTaxType = si.SalesTaxType,
                      SalesTaxVal = si.SalesTaxValue,
                      PercSalesTaxType = si.PercSalesTaxType,
                      CreatedAt = si.CreatedAt,
                      Store = si.Store,
                      WHT = si.WHT,
                      WHTType = si.WHTType,
                      TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == si.Item.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                      ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == si.Item.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == si.Item.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()),
                      //--StockReturnQuantity--//
                      StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == si.Item.ItemID && sr.IsActive && sri.Batch.BatchId == si.Batch.BatchId select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                  })).ToList(),
                  Supplier = s.Supplier
              }).FirstOrDefault();

            return result;
        }
        
        public StockVM GetReturnStockById_Show(long StockId)
        {
            StockVM result = cxt.Stocks
                .Where(s => s.StockId == StockId)
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  NetDiscount = s.NetDiscount,
                  NetSaleTax = s.NetSaleTax,
                  NetSaleTaxType = s.NetSaleTaxType,
                  NetDiscountType = s.NetDiscountType,
                  TotalDiscount = s.TotalDiscount,
                  TotalSaleTax = s.TotalSaleTax,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  StockItems = (s.StockItems.Where(si => si.IsActive).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      StockItemId = si.StockItemId,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      BonusDiscount = si.BonusDiscount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountVal = si.DiscountValue,
                      PercDiscType = si.PercDiscType,
                      SalesTaxType = si.SalesTaxType,
                      SalesTaxVal = si.SalesTaxValue,
                      PercSalesTaxType = si.PercSalesTaxType,
                      CreatedAt = si.CreatedAt,

                      TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == si.Item.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                      ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == si.Item.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                      HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == si.Item.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                     -
                                     (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == si.Item.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == si.Item.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                      //--StockReturnQuantity--//
                      StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == si.Item.ItemID && sr.IsActive && sri.Batch.BatchId == si.Batch.BatchId select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              }).FirstOrDefault();

            return result;
        }
        public Stock GetStockById_Edit(long StockId)
        {
            Stock result = cxt.Stocks
                .Where(s => s.StockId == StockId)
                .Include(s => s.StockItems.Select(i => i.Item))
                .Include(s => s.StockItems.Select(i => i.Batch))
                .Include(s => s.StockItems.Select(i => i.Store))
                .Include(s => s.Supplier)
                .OrderByDescending(s => s.StockId).FirstOrDefault();
            return result;
        }

        public List<Stock> GetActiveStocksParentOnly()
        {
            return cxt.Stocks.Where(s => s.IsActive == true).Include(s => s.Supplier).ToList();
        }
        public List<StockItemVM> GetActiveStockItems()
        {
            //return cxt.StockItems.Where(i => i.IsActive == true).Include(i=>i.Stock).Include(i=>i.Item).ToList();

            return cxt.StockItems.Where(i => i.IsActive == true)
                  .Select(item => new StockItemVM
                  {
                      ItemName = item.Item.ItemName,
                      Quantity = item.Quantity,
                      UnitCost = item.UnitCost,
                      TotalCost = item.TotalCost,
                      RetailPrice = item.RetailPrice,
                      BatchName = item.Batch.BatchName,
                      Discount = item.Discount,
                      SalesTax = item.SalesTax,
                      NetValue = item.NetValue,
                      CreatedAt = item.CreatedAt,
                      StockId = item.Stock.StockId
                  }).ToList();

        }

        public bool StockHasReturnStock(long StockId)
        {
            return cxt.StockReturns.Any(x => x.Stock.StockId == StockId);
        }

        public void SetInactive(long StockId)
        {
            cxt.Database.ExecuteSqlCommand("Update Stocks Set IsActive = 0, issynced = 0, DeletedAt = @DeletedAt WHERE StockId = @StockId Update StockItems Set IsActive = 0 WHERE Stock_StockId = @StockId", new SqlParameter("@StockId", StockId), new SqlParameter("@DeletedAt", DateTime.Now));
            cxt.Database.ExecuteSqlCommand("Update StockReturns Set IsActive = 0, IsSynced = 0, DeletedAt = @DeletedAt WHERE Stock_StockId = @StockId ", new SqlParameter("@StockId", StockId), new SqlParameter("@DeletedAt", DateTime.Now));
        }

        public List<StockItem> GetItemsByStockId(long StockId)
        {
            Stock result = cxt.Stocks
              .Where(s => s.StockId == StockId && s.IsActive)
              .Include(s => s.StockItems.Select(i => i.Item)).FirstOrDefault();
            return result.StockItems.ToList();
        }
        public void InsertStockPayments(List<Expense> PaymentsList)
        {
            //            string qry = "";                
            //            foreach (InvoicePayment pmnt in PaymentsList)
            //            {
            //                qry = @"insert into  Invoicepayments (paymentType, payment, Date, RefundReason,  IsActive, IsNew, IsUpdate, IsSynced, Updatedate, Stock_StockId) 
            //                        values(" + pmnt.PaymentType + ", " + pmnt.Payment + ", '" + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt")  + "', " + "''" + ", " + 1 + ", " + 1 + ", " + 0 + ", " + 0 + ",'" + pmnt.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss tt") + "', " + pmnt.StockId + ")";
            //            }
            //            cxt.Database.ExecuteSqlCommand(qry);

            string qry = "";
            foreach (Expense pmnt in PaymentsList)
            {
                qry = @"insert into  Expenses (Description, PaymentMode, Date, CreatedAt,  UpdatedAt, PracticeId, VoucherNo, AutoAdded, Amount, Stock_StockId, Supplier_SupplierId, IsActive, IsNew, IsUpdate, IsSynced) 
                        values('"
                                  + pmnt.description + "', "
                                  + pmnt.PaymentMode + ", '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', "
                                  + pmnt.PracticeId + ", "
                                  + pmnt.VoucherNo + ", ";
                qry += pmnt.AutoAdded ? "1," : "0,";
                qry += pmnt.Amount + ", "
                + pmnt.StockId + ", "
                + pmnt.SupplierId + ", ";
                qry += pmnt.IsActive ? "1," : "0,";
                qry += pmnt.IsNew ? "1," : "0,";
                qry += pmnt.IsUpdate ? "1," : "0,";
                qry += pmnt.IsSynced ? "1" : "0";
                qry += ")";
            }
            cxt.Database.ExecuteSqlCommand(qry);
        }

        public PurchaseReportsMasterVM GetAllPurchases(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks 
            .Where(s => s.IsActive
                    && DbFunctions.TruncateTime(s.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(s.UpdateDate) <= DbFunctions.TruncateTime(DateTo)
                    && s.Supplier != null
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = (from s in cxt.Stocks
                                                       join e in cxt.Expenses on s.StockId equals e.Stock.StockId
                                                       where DbFunctions.TruncateTime(e.Date) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(e.Date) <= DbFunctions.TruncateTime(DateTo)
                                                            group s by s.DocumentNo into g
                                                       select new PurchaseReportVM
                                                       {
                                                           StockId = g.FirstOrDefault().StockId,
                                                           DocumentNo = g.Key,
                                                           Supplier = g.FirstOrDefault().Supplier,
                                                           StockDate = g.FirstOrDefault().CreateDate,
                                                           SupplierInvoiceNo = g.FirstOrDefault().SupplierInvoiceNo,
                                                           SupplierInvoiceDate = g.FirstOrDefault().SupplierIvoiceDate,
                                                           SaleTax = g.FirstOrDefault().TotalSaleTax,
                                                           advancetax = g.FirstOrDefault().AdvanceWithholdingTax,
                                                           wht = g.FirstOrDefault().WithholdingTax,
                                                           Amount = g.FirstOrDefault().GrandTotal,
                                                           Paid = g.FirstOrDefault().InvoicePayments.Where(p => p.IsActive && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo)).Sum(p => (double?)p.Amount) ?? 0,
                                                           LastPayment = g.FirstOrDefault().InvoicePayments.Where(p => p.IsActive && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo)).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                                                           PaymentMode = g.FirstOrDefault().InvoicePayments.Where(p => p.IsActive && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo)).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                                                       })
                                                       .OrderByDescending(r => r.StockId)
                                                       .ToPagedList(PageNo , PageSize);

            List <PurchaseReportVM> PurchaseReports1 = cxt.Stocks
                .Where(s => s.IsActive
                        && DbFunctions.TruncateTime(s.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(s.UpdateDate) <= DbFunctions.TruncateTime(DateTo)
                        && s.Supplier != null

                        )
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            SaleTax = s.TotalSaleTax,
                            advancetax = s.AdvanceWithholdingTax,
                            wht = s.WithholdingTax,
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.StockId).ToList();
            return new PurchaseReportsMasterVM
            {
                TotalPaid = PurchaseReports.Sum(x => x.Paid),
                TotalWHT = PurchaseReports.Sum(x => x.wht),
                TotalAdvanceTax = PurchaseReports.Sum(x => x.advancetax),
                TotalAmount = PurchaseReports.Sum(x => x.Amount),
                pageno = PurchaseReports.Count,
                TotalPurchaseAmount = TotalPurchaseAMount,
                PurchaseReports = PurchaseReports
            };
        }
        public PurchaseReportsMasterVM GetAllPurchases(long SupplierId, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
                    && s.Supplier.SupplierID == SupplierId
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.Supplier.SupplierID == SupplierId
                         && s.Supplier != null
                        
                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.DocumentNo).ToPagedList(PageNo, PageSize);
            List<PurchaseReportVM> PurchaseReports1 = cxt.Stocks
                .Where(s => s.IsActive
                        && s.Supplier.SupplierID == SupplierId
                        && s.Supplier != null

                        )
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SaleTax = s.NetSaleTax,
                            advancetax = s.AdvanceWithholdingTax,
                            wht = s.WithholdingTax,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.DocumentNo).ToList();
            return new PurchaseReportsMasterVM
            {
                TotalPaid = PurchaseReports1.Sum(x => x.Paid),
                TotalWHT = PurchaseReports1.Sum(x => x.wht),
                TotalAdvanceTax = PurchaseReports1.Sum(x => x.advancetax),
                TotalAmount = PurchaseReports1.Sum(x => x.Amount),
                pageno = PurchaseReports1.Count,
                TotalPurchaseAmount = TotalPurchaseAMount,
                PurchaseReports = PurchaseReports
            };
        }

        public PurchaseReportsMasterVM GetAllPurchases(int paymentmode, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
                    && s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString() == paymentmode.ToString()
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString() == paymentmode.ToString()
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToPagedList(PageNo, PageSize);
            List<PurchaseReportVM> PurchaseReports1 = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString() == paymentmode.ToString()
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToList();
            return new PurchaseReportsMasterVM
            {
                TotalPaid = PurchaseReports1.Sum(x => x.Paid),
                TotalWHT = PurchaseReports1.Sum(x => x.wht),
                TotalAdvanceTax = PurchaseReports1.Sum(x => x.advancetax),
                TotalAmount = PurchaseReports1.Sum(x => x.Amount),
                pageno = PurchaseReports1.Count,
                TotalPurchaseAmount = TotalPurchaseAMount,
                PurchaseReports = PurchaseReports
            };
        }
        public PurchaseReportsMasterVM GetAllPurchases(Decimal DocumentNo, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
                    && s.DocumentNo == DocumentNo
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.DocumentNo == DocumentNo
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToPagedList(PageNo, PageSize);
            List<PurchaseReportVM> PurchaseReports1 = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.DocumentNo == DocumentNo
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToList();
            return new PurchaseReportsMasterVM
            {
                TotalPaid = PurchaseReports1.Sum(x => x.Paid),
                TotalWHT = PurchaseReports1.Sum(x => x.wht),
                TotalAdvanceTax = PurchaseReports1.Sum(x => x.advancetax),
                TotalAmount = PurchaseReports1.Sum(x => x.Amount),
                pageno = PurchaseReports1.Count,
                TotalPurchaseAmount = TotalPurchaseAMount,
                PurchaseReports = PurchaseReports
            };
        }
        public PurchaseReportsMasterVM GetAllPurchases(int PageNo, int PageSize, string SupplierInvoiceNo)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
               && s.SupplierInvoiceNo == SupplierInvoiceNo
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                     && s.SupplierInvoiceNo == SupplierInvoiceNo
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToPagedList(PageNo, PageSize);
            List<PurchaseReportVM> PurchaseReports1 = cxt.Stocks
                 .Where(s => s.IsActive
                     && s.SupplierInvoiceNo == SupplierInvoiceNo
                         && s.Supplier != null

                         )
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SaleTax = s.NetSaleTax,
                             advancetax = s.AdvanceWithholdingTax,
                             wht = s.WithholdingTax,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToList();
            return new PurchaseReportsMasterVM
            {
                TotalPaid = PurchaseReports1.Sum(x => x.Paid),
                TotalWHT = PurchaseReports1.Sum(x => x.wht),
                TotalAdvanceTax = PurchaseReports1.Sum(x => x.advancetax),
                TotalAmount = PurchaseReports1.Sum(x => x.Amount),
                pageno = PurchaseReports1.Count,
                TotalPurchaseAmount = TotalPurchaseAMount,
                PurchaseReports = PurchaseReports
            };
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                 .Where(s => s.IsActive
                         && DbFunctions.TruncateTime(s.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                         && DbFunctions.TruncateTime(s.UpdateDate) <= DbFunctions.TruncateTime(DateTo)
                         && s.Supplier != null)
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             SupplierName = s.Supplier.Name,
                             StockDate = s.CreateDate,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             PaymentDate = s.UpdateDate,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                             PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                         }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(long SupplierId)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                .Where(s => s.IsActive
                        && s.Supplier.SupplierID == SupplierId
                        && s.Supplier != null)
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            PaymentDate = s.UpdateDate,
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(int PaymentMode)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                .Where(s => s.IsActive
                        && s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString() == PaymentMode.ToString()
                        && s.Supplier != null)
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt1(long DocumentNo)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                .Where(s => s.IsActive
                        && s.DocumentNo == DocumentNo
                        && s.Supplier != null)
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(string SupplierInvoiceNo)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                .Where(s => s.IsActive
                        && s.SupplierInvoiceNo == SupplierInvoiceNo
                        && s.Supplier != null)
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreateDate,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                            LastPayment = s.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault(),
                            PaymentMode = s.InvoicePayments.Where(p => p.IsActive).Select(p => p.PaymentMode).FirstOrDefault().ToString()
                        }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public StockStatisticsParentVM GetStockStatistics(DateTime DateFrom, DateTime DateTo)
        {
            StockStatisticsParentVM Result = new StockStatisticsParentVM();
            //Result.StockStatisticsList = (from s in cxt.Stocks
            //                              join si in cxt.StockItems on s equals si.Stock
            //                              where s.IsActive
            //                              && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //                              && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo)
            //                              group s by DbFunctions.TruncateTime(s.CreateDate) into g
            //                              select new StockStatisticsVM
            //                              {
            //                                  MonthDay = g.FirstOrDefault().CreateDate.Day,
            //                                  TotalRetailValue = g.Sum(r => r.StockItems.Sum(sit => sit.RetailPrice * sit.Quantity)),
            //                                  TotalCostValue = g.Sum(r => r.StockItems.Sum(si => si.TotalCost)),

            //                              }).ToList();

            SummationsVM res = new SummationsVM
            {
                //RV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.RetailPrice) ?? 0),
                RV_T_Stock = (from s in cxt.Stocks join si in cxt.StockItems on s.StockId equals si.Stock.StockId where DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo) && si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemID == si.Item.ItemID && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where DbFunctions.TruncateTime(sc.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(sc.CreateDate) <= DbFunctions.TruncateTime(DateTo) && sc.IsActive select new { Quantity = sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == sc.Item.ItemID && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.RetailPrice) ?? 0,
                RV_T_A_Stock = (from a in cxt.Adjustments join ai in cxt.AdjustmentItems on a.AdjustmentId equals ai.Adjustment.AdjustmentId where DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(DateTo) && ai.IsActive select new { Quantity = ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == ai.Item.ItemID && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.RetailPrice) ?? 0,
                RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                CV_T_Stock = (from s in cxt.Stocks join si in cxt.StockItems on s.StockId equals si.Stock.StockId where DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo) && si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemID == si.Item.ItemID && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where DbFunctions.TruncateTime(sc.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(sc.CreateDate) <= DbFunctions.TruncateTime(DateTo) && sc.IsActive select new { Quantity = sc.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemID == sc.Item.ItemID && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.CostPrice) ?? 0,
                CV_T_A_Stock = (from a in cxt.Adjustments join ai in cxt.AdjustmentItems on a.AdjustmentId equals ai.Adjustment.AdjustmentId where DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(DateTo) && ai.IsActive select new { Quantity = ai.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemID == ai.Item.ItemID && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.CostPrice) ?? 0,
                CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                CV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
            };
            if (res != null)
            {
                Result.TotalRetValOfAvailableStock = res.RV_T_Stock - res.RV_T_C_Stock + res.RV_T_A_Stock - (res.RV_T_E_Stock - res.RV_T_C_E_Stock);
                Result.TotalCostValOfAvailableStock = res.CV_T_Stock - res.CV_T_C_Stock + res.CV_T_A_Stock - (res.CV_T_E_Stock - res.CV_T_C_E_Stock);
                
            }
            return Result;
        }
        public void TestMethod(DateTime DateFrom, DateTime DateTo)
        {
            var Result = (from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                          && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                          group s by DbFunctions.TruncateTime(s.CreateDate) into g
                          select g.ToList());
        }
        public List<SaleStatisticsVM> GetSalesStatistics(DateTime? startDate, DateTime endDate)
        {
            return (from i in cxt.Invoices
                    where i.IsActive
                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(startDate)
                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(endDate)
                    group i by DbFunctions.TruncateTime(i.CreateDate) into g
                    select new SaleStatisticsVM
                    {
                        MonthDay = g.FirstOrDefault().CreateDate.Day,
                        TotalSales = g.Sum(i => i.GrandTotal)
                    }).ToList();
        }
        public List<SaleStatisticsVM> GetRevenuStatistics(DateTime? startDate, DateTime endDate)
        {
            //return (from i in cxt.Invoices
            //        where i.IsActive
            //        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(startDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(endDate)
            //        group i by DbFunctions.TruncateTime(i.CreateDate) into g
            //        select new SaleStatisticsVM
            //        {
            //            MonthDay = g.FirstOrDefault().CreateDate.Day,
            //            TotalSales = g.Sum(i => i.InvoicePayments.Sum())
            //        }).ToList();
            return null;
        }
        public List<StockGraphVM> GetRetailValueOfAvailableStock(DateTime DateFrom, DateTime DateTo)
        {
            List<StockGraphVM> Result = new List<StockGraphVM>();
            Result = (from s in cxt.Stocks
                      join si in cxt.StockItems on s equals si.Stock
                      where s.IsActive
                      && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                      group s by DbFunctions.TruncateTime(s.CreateDate) into g
                      select new StockGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreateDate.Day,
                          GrandTotal = g.Sum(r => r.StockItems.Sum(sit => sit.RetailPrice * sit.Quantity)),
                      }).ToList();
            return Result;
        }
        public List<StockGraphVM> GetCostValueOfAvailableStock(DateTime DateFrom, DateTime DateTo)
        {
            List<StockGraphVM> Result = new List<StockGraphVM>();
            Result = (from s in cxt.Stocks
                      join si in cxt.StockItems on s equals si.Stock
                      where s.IsActive
                      && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                      group s by DbFunctions.TruncateTime(s.CreateDate) into g
                      select new StockGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreateDate.Day,
                          GrandTotal = g.Sum(r => r.StockItems.Sum(sit => sit.TotalCost)),
                      }).ToList();
            return Result;
        }
        public List<StockItem> GetStockItemsFromStock(long StockId)
        {
            return cxt.StockItems.Where(x => x.Stock.StockId == StockId).ToList();
        }
        public StockItem GetStockItem(long StockItemId)
        {
            return cxt.StockItems.Where(x => x.StockItemId == StockItemId).Include(x => x.Batch).FirstOrDefault();
        }
        public Batch GetRequiredBatch(int itemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                return cxt.StockItems.Where(i => i.Item.ItemID == itemId && i.IsActive).OrderBy(i => i.StockItemId).Select(r => r.Batch).FirstOrDefault();
            }
            else
            {
                return cxt.StockItems.Where(i => i.Item.ItemID == itemId && i.IsActive).OrderByDescending(i => i.StockItemId).Select(r => r.Batch).FirstOrDefault();
            }
        }
        public bool anyActiveStockExists()
        {
            return cxt.Stocks.Any(s => s.IsActive);
        }
        public void deleteStocks()
        {
        }

        #region frmManageStocks
        public bool IsStoreItem(long StockId)
        {
            return cxt.StockItems.Where(x => x.Stock.StockId == StockId).Any(s => s.Store != null);
        }

        public IPagedList<StockVM> GenericSearchForManageStocks(DateTime? FromDate, DateTime? ToDate, string ItemNameFilter, string SupplierInvoiceNo, string StockDocNo, long SupplierId, int StoreId, int pageNo, int PageSize)
        {
             decimal stockDoc;
             Decimal.TryParse(StockDocNo, out stockDoc);
            IQueryable<Stock> query = cxt.Stocks.Where(s => s.IsActive).AsQueryable();
            if (FromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= DbFunctions.TruncateTime(FromDate.Value));
            if (ToDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) <= DbFunctions.TruncateTime(ToDate.Value));
            if (StoreId > 0)
                query = query.Where(x => x.StockItems.Any(i => i.Store != null && i.Store.StoreId == StoreId));
            if (SupplierId > 0)
                query = query.Where(x => x.Supplier.SupplierID == SupplierId);
            if (!string.IsNullOrEmpty(ItemNameFilter))
                query = query.Where(s => s.StockItems.Any(i => i.IsActive && i.Quantity > 0 && i.Item.ItemName.ToLower().Contains(ItemNameFilter)));
            if (!string.IsNullOrEmpty(SupplierInvoiceNo))
                query = query.Where(s => s.IsActive && s.SupplierInvoiceNo.Contains(SupplierInvoiceNo));
            if (!string.IsNullOrEmpty(StockDocNo))
                query = query.Where(s => s.IsActive && s.DocumentNo == stockDoc);

            IPagedList<StockVM> result = query.Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  Ishold= s.Ishold,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  GrandTotal = s.GrandTotal,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Stock.Ishold == false && si.Stock.IsHolding == 0 && si.Quantity > 0 ).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      Store = si.Store,
                      DiscountedPrice =si.DiscountedPrice,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      WHT = si.WHT,
                      WHTType = si.WHTType,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item.ItemID == si.Item.ItemID && c.Batch.BatchId == si.Batch.BatchId && c.IsActive  && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public List<double> GenericSumOfManageStocks(DateTime? FromDate, DateTime? ToDate, string ItemNameFilter, string SupplierInvoiceNo, string StockDocNo, long SupplierId, int StoreId)
        {
            decimal stockDoc;
            Decimal.TryParse(StockDocNo, out stockDoc);
            IQueryable<Stock> query = cxt.Stocks.Where(s => s.IsActive).AsQueryable();
            if (FromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= DbFunctions.TruncateTime(FromDate.Value));
            if (ToDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) <= DbFunctions.TruncateTime(ToDate.Value));
            if (StoreId > 0)
                query = query.Where(x => x.StockItems.Any(i => i.Store != null && i.Store.StoreId == StoreId));
            if (SupplierId > 0)
                query = query.Where(x => x.Supplier.SupplierID == SupplierId);
            if (!string.IsNullOrEmpty(ItemNameFilter))
                query = query.Where(s => s.StockItems.Any(i => i.IsActive && i.Quantity > 0 && i.Item.ItemName.ToLower().Contains(ItemNameFilter)));
            if (!string.IsNullOrEmpty(SupplierInvoiceNo))
                query = query.Where(s => s.IsActive && s.SupplierInvoiceNo.Contains(SupplierInvoiceNo));
            if (!string.IsNullOrEmpty(StockDocNo))
                query = query.Where(s => s.IsActive && s.DocumentNo == stockDoc);
            return query.Select(x => x.StockItems.Where(si => si.IsActive && si.Quantity > 0).Sum(s => (double?)s.NetValue) ?? 0).ToList();
            //return query.Select(x => x.StockItems.Where(si => si.IsActive && si.Quantity > 0).Sum(n => n.NetValue)).ToList();
        }
        public IPagedList<StockVM> GetStocks(long SupplierId, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => s.Supplier.SupplierID == SupplierId)
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => s.Supplier.SupplierID == SupplierId)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(DateTime FromDate, DateTime ToDate, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(DateTime FromDate, DateTime ToDate, string NameFilter, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
              .Where(s => s.StockItems.Any(i => i.IsActive && i.Quantity > 0 && i.Item.ItemName.ToLower().Contains(NameFilter)))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocksBySupplierInvoice(DateTime FromDate, DateTime ToDate, string SupplierInvoiceNo, int pageNo, int PageSize)
        {

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive && s.SupplierInvoiceNo.Contains(SupplierInvoiceNo))
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
              .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
              .Where(s => s.StockItems.Any(i => i.IsActive && i.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreateDate = s.CreateDate,
                  UpdateDate = s.UpdateDate,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = (s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      //--StockReturnQuantity--//
                      //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                      StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
                  })).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }

        //--------- frmManageStocksReports

        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, long SupplierId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = (from s in cxt.Stocks
                                           join si in cxt.StockItems on s equals si.Stock
                                           where s.IsActive
                                           && s.Supplier.SupplierID == SupplierId
                                           && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                                           && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate)
                                           && si.IsActive
                                           && si.Quantity > 0
                                           select new StocksReportVM
                                           {
                                               StockId = s.StockId,
                                               DocumentNo = s.DocumentNo,
                                               SupplierIvoiceDate = s.SupplierIvoiceDate,
                                               CreateDate = s.CreateDate,
                                               ItemName = si.Item.ItemName,
                                               Quantity = si.Quantity,
                                               UnitCost = si.UnitCost,
                                               TotalCost = si.TotalCost,
                                               RetailPrice = si.RetailPrice,
                                               BatchName = si.Batch.BatchName,
                                               Discount = si.Discount,
                                               DiscountType = si.DiscountType,
                                               SalesTax = si.SalesTax,
                                               NetValue = si.NetValue,
                                               WHT = si.WHT,
                                               WHTType = si.WHTType,
                                               //--StockReturnQuantity--//
                                               //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                                               ItemCreatedAt = si.CreatedAt
                                           }).OrderByDescending(s => s.StockId).ToList();
            return Result;
        }
        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, bool ApplyDateFilter)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = new List<StocksReportVM>();
            if (ApplyDateFilter)
            {
                Result = (from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                          && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate)
                          && si.IsActive
                          && si.Quantity > 0
                          select new StocksReportVM
                          {
                              StockId = s.StockId,
                              DocumentNo = s.DocumentNo,
                              SupplierIvoiceDate = s.SupplierIvoiceDate,
                              CreateDate = s.CreateDate,
                              ItemName = si.Item.ItemName,
                              Quantity = si.Quantity,
                              UnitCost = si.UnitCost,
                              TotalCost = si.TotalCost,
                              RetailPrice = si.RetailPrice,
                              BatchName = si.Batch.BatchName,
                              Discount = si.Discount,
                              DiscountType = si.DiscountType,
                              SalesTax = si.SalesTax,
                              NetValue = si.NetValue,
                              WHT = si.WHT,
                              WHTType = si.WHTType,
                              //--StockReturnQuantity--//
                              //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                              ItemCreatedAt = si.CreatedAt
                          }).OrderByDescending(s => s.StockId).ToList();
            }
            else
            {
                Result = (from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && si.IsActive
                          && si.Quantity > 0
                          select new StocksReportVM
                          {
                              StockId = s.StockId,
                              DocumentNo = s.DocumentNo,
                              SupplierIvoiceDate = s.SupplierIvoiceDate,
                              CreateDate = s.CreateDate,
                              ItemName = si.Item.ItemName,
                              Quantity = si.Quantity,
                              UnitCost = si.UnitCost,
                              TotalCost = si.TotalCost,
                              RetailPrice = si.RetailPrice,
                              BatchName = si.Batch.BatchName,
                              Discount = si.Discount,
                              DiscountType = si.DiscountType,
                              SalesTax = si.SalesTax,
                              NetValue = si.NetValue,
                              WHT = si.WHT,
                              WHTType = si.WHTType,
                              //--StockReturnQuantity--//
                              //ReturnQuantity = (from r in cxt.ReturnStockItems where  r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                              ItemCreatedAt = si.CreatedAt
                          }).OrderByDescending(s => s.StockId).ToList();
            }
            return Result;
        }
        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, string ItemName)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate))
            //.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = (from s in cxt.Stocks
                                           join si in cxt.StockItems on s equals si.Stock
                                           where s.IsActive
                                           && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                                           && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate)
                                           && si.IsActive
                                           && si.Quantity > 0
                                           && si.Item.ItemName.ToLower().Contains(ItemName)
                                           select new StocksReportVM
                                           {
                                               StockId = s.StockId,
                                               DocumentNo = s.DocumentNo,
                                               SupplierIvoiceDate = s.SupplierIvoiceDate,
                                               CreateDate = s.CreateDate,
                                               ItemName = si.Item.ItemName,
                                               Quantity = si.Quantity,
                                               UnitCost = si.UnitCost,
                                               TotalCost = si.TotalCost,
                                               RetailPrice = si.RetailPrice,
                                               BatchName = si.Batch.BatchName,
                                               Discount = si.Discount,
                                               DiscountType = si.DiscountType,
                                               SalesTax = si.SalesTax,
                                               NetValue = si.NetValue,
                                               //--StockReturnQuantity--//
                                               //ReturnQuantity = (from r in cxt.ReturnStockItems where r.Item.ItemID == si.Item.ItemID && si.Item.IsActive && r.IsActive && r.Batch == si.Batch orderby r.ReturnStockItemId descending select (int?)r.Quantity ?? 0).FirstOrDefault(),
                                               ItemCreatedAt = si.CreatedAt
                                           }).OrderByDescending(s => s.StockId).ToList();
            return Result;
        }
        #endregion
    }
}