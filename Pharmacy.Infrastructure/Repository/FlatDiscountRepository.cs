using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using System.Data.SqlClient;
namespace Pharmacy.Infrastructure.Repository
{
    public class FlatDiscountRepository : Repository<FlatDiscount>
    {
        private PharmacyContext cxt;
        public FlatDiscountRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public FlatDiscount GetDiscountDet(int FlatDiscountId)
        {
            return cxt.FlatDiscount
                 .Where(d => d.FlatDiscountId == FlatDiscountId)
                 .Include(d => d.DiscountItems)
                 .Include(d => d.DiscountItems.Select(di => di.Item))
                 .FirstOrDefault();
        }
        public FlatDiscountVM GetDiscountDet_VM(int FlatDiscountId)
        {
            return cxt.FlatDiscount
                 .Where(d => d.FlatDiscountId == FlatDiscountId)
                 .Select(d => new FlatDiscountVM
                 {
                     FlatDiscountId = d.FlatDiscountId,
                     Name = d.Name,

                     Code = d.Code,
                     Discount = d.Discount,
                     DiscountType = d.DiscountType,

                     IsAllDays = d.IsAllDays,
                     IsAllTimes = d.IsAllTimes,
                     SelectedDays = d.SelectedDays,
                     IsAllItems = d.IsAllItems,

                     StartDate = d.StartDate,
                     EndDate = d.EndDate,
                     StartTime = d.StartTime,
                     EndTime = d.EndTime,

                     CreatedAt = d.CreatedAt,
                     UpdatedAt = d.UpdatedAt,
                     User = d.User,
                     DiscountItems = (d.DiscountItems.Where(di => di.IsActive).Select(di => new DiscountItemVM
                     {
                         DiscountItemId = di.DiscountItemId,
                         Item = di.Item
                     })).ToList()
                 }).FirstOrDefault();
        }

        public IPagedList<FlatDiscount> GetAllACtiveDiscounts(int pageNo, int pageSize)
        {
            return cxt.FlatDiscount
                    .Where(d => d.IsActive)
                    .OrderByDescending(d => d.FlatDiscountId)
                    .ToPagedList(pageNo, pageSize);
        }

        public void SetInActive(int DiscountId)
        {
            cxt.Database
                .ExecuteSqlCommand("Update FlatDiscounts Set IsActive = 0, issynced = 0, DeletedAt = @DeletedAt WHERE flatdiscountId = @DiscountId", new SqlParameter("@DiscountId", DiscountId), new SqlParameter("@DeletedAt", DateTime.Now));
        }

        public bool DiscNameExists_new(string name)
        {
            return cxt.FlatDiscount.Any(m => m.Name.ToLower() == name && m.IsActive);
        }
        public bool DiscCodeExists_new(string name)
        {
            return cxt.FlatDiscount.Any(m => m.Code.ToLower() == name && m.IsActive);
        }

        public bool DiscNameExists_Update(string name, int DiscountId)
        {
            return cxt.FlatDiscount.Any(m => m.Name.ToLower() == name && m.FlatDiscountId != DiscountId && m.IsActive);
        }
        public bool DiscCodeExists_Update(string code, int DiscountId)
        {
            return cxt.FlatDiscount.Any(m => m.Code.ToLower() == code && m.FlatDiscountId != DiscountId && m.IsActive);
        }
        public List<FlatDiscount> getTodayDiscounts()
        {
            return cxt.FlatDiscount
                .Where(d => d.IsActive
                    && DbFunctions.TruncateTime(DateTime.Now) >= DbFunctions.TruncateTime(d.StartTime)
                    && DbFunctions.TruncateTime(DateTime.Now) <= DbFunctions.TruncateTime(d.EndDate)
                    )
                    .Include(d => d.DiscountItems)
                    .Include(d => d.DiscountItems.Select(di => di.Item))
                    .ToList();
            //cxt.FlatDiscount
            //    .Where(d=> d.IsActive
            //        && DbFunctions.TruncateTime(DateTime.Now) >= DbFunctions.TruncateTime(d.StartTime)
            //        && DbFunctions.TruncateTime(DateTime.Now) <= DbFunctions.TruncateTime(d.EndDate)
            //        && 
            //        (
            //            d.IsAllTimes 
            //            || (DbFunctions.CreateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)>= DbFunctions.CreateTime(d.StartTime.Hour, d.StartTime.Minute, d.StartTime.Second)
            //                &&
            //                DbFunctions.CreateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)>= DbFunctions.CreateTime(d.EndTime.Hour, d.EndTime.Minute, d.EndTime.Second)
            //                )
            //        )
            //        &&
            //        (
            //            d.IsAllDays
            //            ||
            //            (

            //            )
            //        )



            //            )
        }

        public IPagedList<FlatDiscountVM> GetAllItemDiscounts(int pageNo, int pageSize)
        {
            return cxt.FlatDiscount
                .Where(d => d.IsActive
                    )
                    .Select(d => new FlatDiscountVM
                    {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscountType = d.DiscountType,
                        IsAllItems = d.IsAllItems
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<FlatDiscountVM> GetAllItemDiscounts(DateTime DateFrom, DateTime DateTo, int pageNo, int pageSize)
        {
            return cxt.FlatDiscount
                .Where(d => d.IsActive
                    && DbFunctions.TruncateTime(d.StartDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(d.EndDate) <= DbFunctions.TruncateTime(DateTo)
                    )
                    .Select(d=> new FlatDiscountVM {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscountType = d.DiscountType                        
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToPagedList(pageNo, pageSize);
        }

        public List<FlatDiscountVM> GetAllItemDiscounts_rpt()
        {
            return cxt.FlatDiscount
                .Where(d => d.IsActive
                    && d.IsAllItems
                    )
                    .Select(d => new FlatDiscountVM
                    {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscTypeString = d.DiscountType ==0 ? "%" : "Value"
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToList();
        }
        public List<FlatDiscountVM> GetAllItemDiscounts_rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.FlatDiscount
                .Where(d => d.IsActive
                    && d.IsAllItems
                    && DbFunctions.TruncateTime(d.StartDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(d.EndDate) <= DbFunctions.TruncateTime(DateTo)
                    )
                    .Select(d => new FlatDiscountVM
                    {         
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscTypeString = d.DiscountType == 0 ? "%" : "Value"
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToList();
        }
    }
}