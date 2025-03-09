using PagedList;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class MissedsalesRepository : Repository<MissedSale>
    {
        PharmacyContext cxt = null;
        public MissedsalesRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<MissedSale> GetAllActiveMissedSale(int pageNo, int pagesize)
        {
            return cxt.MissedSales.Where(s => s.IsActive).Include(s => s.Item).Include(s => s.User).OrderByDescending(s => s.MissedSaleId).ToPagedList(pageNo, pagesize);
        }
        public List<MissedSale> GetAllActiveMissedSale()
        {
            return cxt.MissedSales.Where(s => s.IsActive)
                .Include(s => s.Item)
                .OrderByDescending(s => s.MissedSaleId).ToList();
        }

        public List<int> GetMissedSalesItemsIds()
        {
            return cxt.MissedSales.Where(s => s.IsActive)
                .Select(s => s.Item.ItemID)
                .ToList();
        }

        public List<MissedSale> GetAllActiveMissedSalesByItem(int itemId)
        {
            return cxt.MissedSales.Where(s => s.IsActive && s.Item.ItemID == itemId).Include(s => s.Item).Include(s => s.User).OrderByDescending(s => s.MissedSaleId).ToList();
        }

        public IPagedList<MissedSale> GetMissedSalesBWDate(DateTime DateFrom, DateTime DateTo, int pageNo, int pagesize)
        {
            return cxt.MissedSales
                .Where(s => s.IsActive)
                .Where(s => DbFunctions.TruncateTime(s.CreatedAt) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(s.CreatedAt) <= DbFunctions.TruncateTime(DateTo))
                .Include(s => s.Item)
                .Include(s => s.User)
                .OrderByDescending(s => s.MissedSaleId).ToPagedList(pageNo, pagesize);
        }
        public IPagedList<MissedSale> GetMissedSalesByItem(int ItemId)
        {
            return cxt.MissedSales
                .Where(s => s.IsActive && s.Item.ItemID == ItemId)
                .Include(s => s.Item)
                    .Include(s => s.User)
                .OrderByDescending(s => s.MissedSaleId).ToPagedList(1, 1);
        }

        public bool ItemMissedSaleExists(int ItemId)
        {
            return cxt.MissedSales.Any(m => m.IsActive && m.Item.ItemID == ItemId);
        }

        public List<MissedSale> GetMissedSalesByItemIds(List<int> itemIds)
        {
            return cxt.MissedSales.Where(s => s.IsActive && itemIds.Contains(s.Item.ItemID)).ToList();
        }
    }
}