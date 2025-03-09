using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;
using System.Data.SqlClient;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.Infrastructure.Repository
{
    public class AdjustmentRepository : Repository<Adjustment>
    {
        PharmacyContext cxt;
        public AdjustmentRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        #region frmStockAdjustments
        public IPagedList<Adjustment> GetAdjustments(int PageNo, int PageSize)
        {
            return cxt.Adjustments
                .Where(a => a.IsActive == true)
                .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                .Include(a => a.AdjustmentItems)
                .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                .Include(a => a.AdjustmentItems.Select(i => i.Item))
                .Include(a => a.User)
                .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<Adjustment> GetAdjustments(int ItemId, int PageNo, int PageSize)
        {

            return cxt.Adjustments
                    .Where(a => a.IsActive && a.AdjustmentItems.Any(ai => ai.IsActive && ai.Item.ItemID == ItemId))
                    .Include(a => a.AdjustmentItems)
                    .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                    .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                    .Include(a => a.AdjustmentItems.Select(i => i.Item))
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<Adjustment> GetAdjustments(DateTime? FromDate, DateTime? Dateto, int PageNo, int PageSize)
        {
            IQueryable<Adjustment> query = cxt.Adjustments.Where(s => s.IsActive).AsQueryable();
            if (FromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= DbFunctions.TruncateTime(FromDate));
            if (Dateto.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) <= DbFunctions.TruncateTime(Dateto));

            return query
                    .Where(a => a.IsActive == true)
                    .Include(a => a.AdjustmentItems)
                    .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                    .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                    .Include(a => a.AdjustmentItems.Select(i => i.Item))
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);

        }

        public List<Adjustment> GetAdjustmentSum()
        {
            return cxt.Adjustments.Where(a => a.IsActive == true)
                .Include(a => a.AdjustmentItems)
                .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                .Include(a => a.AdjustmentItems.Select(i => i.Item)).ToList();
        }

        public List<Adjustment> GetAdjustmentSum(int ItemId)
        {
            return cxt.Adjustments
                     .Where(a => a.IsActive && a.AdjustmentItems.Any(ai => ai.IsActive && ai.Item.ItemID == ItemId))
                     .Include(a => a.AdjustmentItems)
                     .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                     .Include(a => a.AdjustmentItems.Select(i => i.Item))
                     .Include(a => a.User).ToList();
        }

        public List<Adjustment> GetAdjustmentSum(DateTime? FromDate, DateTime? ToDate)
        {
            IQueryable<Adjustment> query = cxt.Adjustments.Where(s => s.IsActive).AsQueryable();
            if (FromDate.HasValue)
                query = query.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(FromDate));
            if (ToDate.HasValue)
                query = query.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(ToDate));
            return query
                     .Where(a => a.IsActive == true)
                     .Include(a => a.AdjustmentItems)
                     .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                     .Include(a => a.AdjustmentItems.Select(i => i.Item)).ToList();
        }

        public AdjustmentsVM GetAdjustments_Rpt(long AdjustmentID)
        {
            return cxt.Adjustments
                    .Where(a => a.AdjustmentId == AdjustmentID)
                    .Where(a => a.IsActive == true)
                    .Select(ad => new AdjustmentsVM
                    {
                        AdjustmentId = ad.AdjustmentId,
                        AdjustmentItems = (ad.AdjustmentItems.Where(i => i.IsActive).Select(i => new AdjustmentItemVM
                        {
                            ItemName = i.Item.ItemName,
                            Unit = i.Unit,
                            ConversionUnit = i.Item.ConversionUnit,
                            BatchName = i.Batch.BatchName,
                            CostPrice = i.CostPrice,
                            Quantity = i.Quantity,
                            AdjustmentValue = i.AdjustmentValue,
                            Reason = i.Reason,
                            CreateDate = i.CreateDate
                        })).ToList()
                    }).FirstOrDefault();
        }
        #endregion

        public Adjustment GetAdjustmentById(long AdjustmentId)
        {
            return cxt.Adjustments
                .Where(a => a.AdjustmentId == AdjustmentId)
                .Include(a => a.AdjustmentItems.Select(i => i.Item))
                .Include(a => a.AdjustmentItems.Select(i => i.Batch)).FirstOrDefault();
        }

        public void SetInactive(long AdjustmentId)
        {
            cxt.Database.ExecuteSqlCommand("Update Adjustments set IsActive = 0, issynced = 0 WHERE AdjustmentId = @AdjustmentID Update AdjustmentItems set IsActive = 0 WHERE Adjustment_AdjustmentId = @AdjustmentID ", new SqlParameter("@AdjustmentID", AdjustmentId));
        }

        public List<AdjustmentItem> GetStockAdjustmentsByItemId(int ItemId)
        {
            return cxt.AdjustmentItems
                .Where(a => a.IsActive)
                .Include(ai => ai.Batch)
                .Where(ai => ai.Item.ItemID == ItemId).ToList();
        }
        public bool AnyActiveAdjExists()
        {
            return cxt.Adjustments.Any(a => a.IsActive);
        }
    }
}
