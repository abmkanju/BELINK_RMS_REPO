using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using Pharmacy.Core.ViewModels;
using System.Data.SqlClient;


namespace Pharmacy.Infrastructure.Repository
{
    public class StoreRepository
    {
        PharmacyContext cxt;
        public StoreRepository(PharmacyContext cxt)
        {
            this.cxt = cxt;
        }

        public void InsertStores(List<Store> stores)
        {
            cxt.Stores.AddRange(stores);
            cxt.SaveChanges();
        }

        public Store GetStoreEditDetail(int StoreId)
        {
            return cxt.Stores.Where(x => x.StoreId == StoreId).FirstOrDefault();
        }

        public void SetStoreInactive(int StoreId)
        {
            cxt.Database.ExecuteSqlCommand(
                "Update Stores Set IsActive = 0, IsSynced = 0, DeletedAt = @DeletedAt WHERE StoreId = @StoreId",
                new SqlParameter("@StoreId", StoreId), new SqlParameter("@DeletedAt", DateTime.Now));
        }

        public void Update(Store obj)
        {
            cxt.Entry(obj).State = EntityState.Modified;
            cxt.SaveChanges();
        }
        public IPagedList<StoreVM> GenericSearchForStores(DateTime? dtpFromDate, DateTime? dtpToDate, string FilterByName, int PageNo, int PageSize)
        {
            IQueryable<Store> query = cxt.Stores.Where(x => x.IsActive == true).AsQueryable();
            if (dtpFromDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) >= DbFunctions.TruncateTime(dtpFromDate));
            if (dtpToDate.HasValue)
                query = query.Where(x => DbFunctions.TruncateTime(x.CreateDate) <= DbFunctions.TruncateTime(dtpToDate));
            if (!string.IsNullOrEmpty(FilterByName.Trim()))
                query = query.Where(x => x.Name.Contains(FilterByName));
            IPagedList<StoreVM> r = query.Select(x => new StoreVM
            {
                StoreId = x.StoreId,
                Name = x.Name,
                UserName = x.User.UserName,
                CreateDate = x.CreateDate,
            }).OrderByDescending(x => x.StoreId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public bool CheckDuplicateItem(string StoreName, int StoreId)
        {
            return cxt.Stores.Where(x => x.IsActive.Equals(true)).Any(x => x.Name == StoreName && x.StoreId != StoreId);
        }

        public List<StoreVM> GetAllStores()
        {
            return cxt.Stores.Where(x=>x.IsActive.Equals(true)).Select(x => new StoreVM
            {
                StoreId = x.StoreId,
                Name = x.Name,
            }).ToList();
        }

        public Store GetById(int Id)
        {
            var store = cxt.Stores.Where(x=>x.StoreId == Id && x.IsActive).FirstOrDefault();
            return store; 
        }
    }
}
