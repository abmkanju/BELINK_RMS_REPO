using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class StockAuditRepository : Repository<StockAudit>
    {
        PharmacyContext cxt;
        public StockAuditRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<StockAuditsScreenVM> getActiveAudits(int pageNo, int PageSize)
        {
            return cxt.StockAudits
                 .Where(sa => sa.IsActive)
                 .Select(sa => new StockAuditsScreenVM
                 {
                     StockAuditId = sa.StockAuditId,
                     TotalDifference = sa.TotalDifference,
                     AuditDate = sa.StockAuditDate,
                     CreatedAt = sa.CreatedAt
                 }).OrderByDescending(sa => sa.StockAuditId)
                 .ToPagedList(pageNo, PageSize);
        }
        public IPagedList<StockAuditVM> getAudits(DateTime from, DateTime to, int pageNo, int PageSize)
        {
            cxt.StockAudits
                .Where(sa => DbFunctions.TruncateTime(sa.StockAuditDate) >= DbFunctions.TruncateTime(from)
                    && DbFunctions.TruncateTime(sa.StockAuditDate) <= DbFunctions.TruncateTime(to))
                .Select(sa => new StockAuditVM
                {
                    StockAuditId = sa.StockAuditId,
                    TotalDifference = sa.TotalDifference,
                    CreatedAt = sa.CreatedAt
                }).OrderByDescending(sa => sa.StockAuditId)
                .ToPagedList(pageNo, PageSize);
            return null;
        }
        public StockAudit GetAuditWithDetailById(int StockAuditId)
        {

            return cxt.StockAudits
                .Where(sa => sa.StockAuditId == StockAuditId)
                .Include(sa => sa.StockAuditDetail)
                .Include(sa => sa.StockAuditDetail.Select(sd => sd.Item))
                .FirstOrDefault();
        }


        public StockAuditVM GetAuditDetailVMById(int StockAuditId)
        {
            return cxt.StockAudits
                .Where(a => a.StockAuditId == StockAuditId)
                .Select(a => new StockAuditVM
                {
                    StockAuditId = a.StockAuditId,
                    StockAuditDate = a.StockAuditDate,
                    User = a.User,
                    StockAuditDetailVM = (a.StockAuditDetail.Where(d => d.IsActive).Select(vm => new StockAuditDetailVM
                    {
                        StockAuditDetailId = vm.StockAuditDetailId,
                        ItemId = vm.Item.ItemID,
                        ItemName = vm.Item.ItemName,
                        ConversionUnit = vm.Item.ConversionUnit,
                        ItemUnit = vm.Item.Unit,
                        AuditUnit = vm.Unit,
                        UnitCost = (from si in cxt.StockItems where si.Item.ItemID == vm.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice1 = (from si in cxt.StockItems where si.Item.ItemID == vm.Item.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),
                        SystemQuantity = vm.SystemQuantity,
                        CurrentAdjustedQuantity = vm.CurrentAdjustedQuantity,
                        Differnce =vm.Differnce,
                        AmountDifference = vm.AmountDifference,
                        PhysicalQuantity = vm.PhysicalQuantity,

                        // query variables to get avaolable stock
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join ad in cxt.Adjustments on ai.Adjustment.AdjustmentId equals ad.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })).ToList()
                }).FirstOrDefault();
        }

        public StockAuditDetail getAuditDetailById(int detailId)
        {
            return cxt.StockAuditDetails.Where(d => d.StockAuditDetailId == detailId).FirstOrDefault();
        }
        
        public StockItem GetStockItem(int ItemId, int batchId)
        {
            return cxt.StockItems.Where(x => x.Item.ItemID == ItemId && x.Batch.BatchId == batchId).OrderByDescending(x => x.StockItemId).FirstOrDefault();
        }
    }
}