using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;

namespace Pharmacy.Infrastructure.Repository
{
    public class WardRepository : Repository<Ward>
    {
        PharmacyContext cxt;
        public WardRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public Ward GetWardByIdWithSubDepartment(long WardId)
        {
            return cxt.Wards.Where(w => w.WardId == WardId ).Include(d => d.SubDepratment).FirstOrDefault();
        }
        public IPagedList<Ward> GetWardsWithSubDepartment(int PageNo, int PageSize)
        {
            return cxt.Wards.Where(w =>w.IsActive).Include(d => d.SubDepratment).OrderByDescending(w=>w.WardId).ToPagedList(PageNo, PageSize);
        }
        public List<Ward> GetAllActiveWards()
        {
            return cxt.Wards.Where(w => w.IsActive).ToList();
        }

        public int HasRoomNo(long WardId, string RoomNo)
        {
            return cxt.Wards.Where(w => w.WardId == WardId && w.Beds.Any(b => b.RoomNo.ToLower().Equals(RoomNo))).Count();
        }

        public int GetActiveBedsCount(long WardId)
        {
            return cxt.Beds.Where(b => b.IsActive && b.Ward.WardId == WardId).Count();
        }

        public List<Bed> GetAvailableActiveBedsByWardId(long WardId)
        {
            return cxt.Beds.Where(b => b.IsActive && b.Ward.WardId == WardId && b.IsAvailable).ToList();
        }
    }
}
