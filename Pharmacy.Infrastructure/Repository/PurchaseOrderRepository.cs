using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Pharmacy.Core.ViewModels;
using PagedList;
using System.Data.SqlClient;
using Pharmacy.Core;

namespace Pharmacy.Infrastructure.Repository
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>
    {
        private PharmacyContext cxt = null;
        public PurchaseOrderRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public decimal GetNewPurchaseOrderNumber()
        {
            decimal NewOrderNum = 0;
            if (cxt.PurchaseOrders.FirstOrDefault() == null)
            {
                NewOrderNum = 1;
            }
            else
            {
                NewOrderNum = cxt.PurchaseOrders.Max(n => n.PurchaseOrderNo) + 1;
            }
            return NewOrderNum;
        }
        public List<PurchaseOrderVM> GetPo_W_DetailByDateRange(DateTime datefrom, DateTime dateTo)
        {
            List<PurchaseOrderVM> result =
                cxt.PurchaseOrders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom) && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
           .Select(order => new PurchaseOrderVM
           {
               PurchaseOrderNo = order.PurchaseOrderNo,
               SupplierName = order.Supplier.Name,
               OrderDate = order.OrderDate,
               PurchaseOrderItems = (order.PurchaseOrderItems.Select(i => new PurchaseOrderItemVM
               {
                   PurchaseOrderItemId = i.ItemID,
                   ItemName = i.Item.ItemName,
                   Quantity = i.Quantity,
                   UnitCost = i.UnitCost,
                   TotalAmount = i.TotalAmount
               }))
           }).ToList();
            return result;
        }
        public PurchaseOrderVM GetPo_W_Supp_ByPoNo(int PoNo)
        {
            PurchaseOrderVM result =
                cxt.PurchaseOrders
                .Where(o => (int)o.PurchaseOrderNo == PoNo)
           .Select(order => new PurchaseOrderVM
           {
               PurchaseOrderId = order.PurchaseOrderId,
               PurchaseOrderNo = order.PurchaseOrderNo,
               Supplier = order.Supplier,
               OrderDate = order.OrderDate,
               StockId = order.StockId,
               TotalAmount = order.TotalAmount,
               //PurchaseOrderItems = (order.PurchaseOrderItems.Select(i => new PurchaseOrderItemVM
               //{
               //    PurchaseOrderItemId = i.PurchaseOrderItemId,
               //    ItemID = i.Item.ItemID,
               //    ItemName = i.Item.ItemName,
               //    Quantity = i.Quantity,
               //    UnitCost = i.UnitCost,
               //    TotalAmount = i.TotalAmount
               //}))
           }).FirstOrDefault();
            return result;
        }
        public IPagedList<PurchaseOrder> GetPurchaseOrderSummaryByDateRange(DateTime datefrom, DateTime dateTo, long SupplierId, int PageNo, int PageSize)
        {
            // List<PurchaseOrderVM> result = cxt.PurchaseOrders
            //         .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom) && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
            //         .Where(o => o.IsDeleted == false)
            //.Select(order => new PurchaseOrderVM
            //{
            //    PurchaseOrderNo = order.PurchaseOrderNo,
            //    SupplierName = order.Supplier.Name,
            //    OrderDate = order.OrderDate,
            //    TotalAmount = order.TotalAmount
            //}).ToList();

            if (SupplierId > 0)
            {
                IPagedList<PurchaseOrder> Result =
                cxt.PurchaseOrders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom)
                    && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
                    .Where(o => o.IsActive == true)
                    .Where(o => o.Supplier != null && o.Supplier.SupplierID == SupplierId)
                    .Include(o => o.Supplier)
                    .Include(o => o.Manufecturers)
                    .Include(o => o.User)
                    //.Include(o=>o.Stock)
                    .OrderByDescending(o => o.PurchaseOrderId)
                    .ToPagedList(PageNo, PageSize);
                return Result;
            }
            else
            {
                IPagedList<PurchaseOrder> Result =
                cxt.PurchaseOrders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom)
                    && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
                    .Where(o => o.IsActive == true)
                    .Include(o => o.Supplier)
                    .Include(o => o.Manufecturers)
                    .Include(o => o.User)
                    //.Include(o=>o.Stock)
                    .OrderByDescending(o => o.PurchaseOrderId)
                    .ToPagedList(PageNo, PageSize);
                return Result;
            }

        }
        public IPagedList<PurchaseOrder> GetPurchaseOrderSummaryByDateRange(int PageNo, int PageSize)
        {
            // List<PurchaseOrderVM> result = cxt.PurchaseOrders
            //         .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom) && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
            //         .Where(o => o.IsDeleted == false)
            //.Select(order => new PurchaseOrderVM
            //{
            //    PurchaseOrderNo = order.PurchaseOrderNo,
            //    SupplierName = order.Supplier.Name,
            //    OrderDate = order.OrderDate,
            //    TotalAmount = order.TotalAmount
            //}).ToList();

            IPagedList<PurchaseOrder> Result =
                cxt.PurchaseOrders
                    .Where(o => o.IsActive == true)
                    .Include(o => o.Supplier)
                    .Include(o => o.User)
                    .Include(o => o.Manufecturers)
                //.Include(o => o.Stock)
                    .OrderByDescending(o => o.PurchaseOrderId)
                    .ToPagedList(PageNo, PageSize);
            return Result;
        }
        public IPagedList<PurchaseOrder> GetPoByPoNo(int PoNo)
        {
            // List<PurchaseOrderVM> result = cxt.PurchaseOrders
            //         .Where(o => DbFunctions.TruncateTime(o.OrderDate) >= DbFunctions.TruncateTime(datefrom) && DbFunctions.TruncateTime(o.OrderDate) <= DbFunctions.TruncateTime(dateTo))
            //         .Where(o => o.IsDeleted == false)
            //.Select(order => new PurchaseOrderVM
            //{
            //    PurchaseOrderNo = order.PurchaseOrderNo,
            //    SupplierName = order.Supplier.Name,
            //    OrderDate = order.OrderDate,
            //    TotalAmount = order.TotalAmount
            //}).ToList();

            IPagedList<PurchaseOrder> Result =
                cxt.PurchaseOrders
                    .Where(o => o.IsActive == true && o.PurchaseOrderNo == PoNo)
                    .Include(o => o.Supplier)
                    .Include(o => o.Manufecturers)
                    .Include(o => o.User)
                //.Include(o=>o.Stock)
                    .OrderByDescending(o => o.PurchaseOrderId)
                    .ToPagedList(1, 1);
            return Result;
        }
        public void SetInActive(long PurchaseOrderId)
        {
            cxt.Database.ExecuteSqlCommand("Update PurchaseOrders set IsActive = 0, Issynced = 0 where PurchaseOrderId = @PurchaseOrderId", new SqlParameter("@PurchaseOrderId", PurchaseOrderId));
        }
        public PurchaseOrder GetPODetailByOrderNo_Origional(decimal PurchaseOrderNo)
        {
            PurchaseOrder po = cxt.PurchaseOrders.Where(o => o.PurchaseOrderNo == PurchaseOrderNo).Include(p => p.PurchaseOrderItems).FirstOrDefault();
            return po;
        }

        public PurchaseOrder GetPurchaseOrderById(long PurchaseOrderId)
        {
            PurchaseOrder po = cxt.PurchaseOrders
                .Where(o => o.PurchaseOrderId == PurchaseOrderId)
                .Include(o=>o.Manufecturers)
                .Include(p => p.PurchaseOrderItems.Select(i => i.Item))
                .Include(p => p.PurchaseOrderItems.Select(i => i.Manufecturer))
                .Include(o => o.Supplier).FirstOrDefault();
            return po;
        }

      
        public AddPOStockVM GetPOById(long POId)
        {
            var res = cxt.PurchaseOrders
                .Where(o => o.PurchaseOrderId == POId)
                .Select(o => new AddPOStockVM
                {
                    SupId = o.Supplier == null ? 0 : o.Supplier.SupplierID,
                    Items = (o.PurchaseOrderItems.Where(i => i.IsActive).Select(i => new ItemsVM
                    {
                        ItemId = i.Item.ItemID,
                        ItemName = i.Item.ItemName,
                        Manufacturer = i.Item.Manufacturer,
                        Unit = i.Item.Unit,
                        UnitValue = i.Unit,
                        ConversionUnit = i.Item.ConversionUnit,
                        UnitCost = i.UnitCost,
                        ItemQuantity = i.Quantity,
                        Discount = i.Discount ,
                        DiscountType = i.DiscountType,
                        Bonus = i.Bonus,
                        RetailPrice = i.Item.RetailPrice,
                        //SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        //    (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Item.ItemID orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                        //    :
                        //    (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Item.ItemID select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.Item.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                    })).ToList()
                }).FirstOrDefault();
            //.Include(p => p.PurchaseOrderItems.Select(i => i.Item))
            //.Include(o => o.Supplier).FirstOrDefault();
            return res;
        }
        public PurchaseOrderVM GetPurchaseOrderById_print(long PurchaseOrderId)
        {
            PurchaseOrderVM po = cxt.PurchaseOrders
                .Where(o => o.PurchaseOrderId == PurchaseOrderId && o.IsActive)
                .Select(vm => new PurchaseOrderVM
                {
                    PurchaseOrderNo = vm.PurchaseOrderNo,
                    CreateDate = vm.CreateDate,
                    OrderDate = vm.OrderDate,
                    DeliveryDate = vm.DeliveryDate,
                    PurchaseOrderItems = (vm.PurchaseOrderItems.Where(i => i.IsActive).Select(i => new PurchaseOrderItemVM
                    {
                        IsActive = i.IsActive,
                        ItemID = i.Item.ItemID,
                        ConversionUnit = i.Item.ConversionUnit,
                        ItemName = i.Item.ItemName,
                        Unit = i.Unit,
                        Discount = i.Discount,
                        Manuf = i.Manufecturer.Name,
                        DiscountType = i.DiscountType,
                        GrossAmount = i.GrossAmount,
                        Quantity = i.Quantity,
                        UnitCost = i.UnitCost,
                        TotalAmount = i.TotalAmount,
                        CreateDate = i.CreateDate
                    })).ToList(),
                    Supplier = vm.Supplier,
                    Manufecturers = vm.Manufecturers
                }).FirstOrDefault();
            return po;
        }

        public PurchaseOrder GetPurchaseOrderById_Inc_Stock(long PurchaseOrderId)
        {
            PurchaseOrder po = cxt.PurchaseOrders
                .Where(o => o.PurchaseOrderId == PurchaseOrderId)
                .Include(p => p.PurchaseOrderItems.Select(i => i.Item))
                .Include(p => p.PurchaseOrderItems.Select(i => i.Item.Manufacturer))
                .Include(o => o.Manufecturers)
                .Include(p => p.Supplier)
                .FirstOrDefault();
            //.Include(o => o.Supplier).Include(o=>o.Stock).FirstOrDefault();
            return po;
        }
        public List<PurchaseOrderGraphVM> GetMonthlyPurchaseOrderGraph(DateTime DateFrom, DateTime DateTo)
        {
            List<PurchaseOrderGraphVM> Result = new List<PurchaseOrderGraphVM>();
            Result = (from i in cxt.PurchaseOrders
                      where i.IsActive
                      && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                      group i by DbFunctions.TruncateTime(i.CreateDate) into g
                      select new PurchaseOrderGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreateDate.Day,
                          GrandTotal = (double)g.Sum(p => p.TotalAmount),
                      }).ToList();
            return Result;
        }
    }
}