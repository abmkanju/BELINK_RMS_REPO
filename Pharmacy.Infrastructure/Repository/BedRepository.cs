using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;

namespace Pharmacy.Infrastructure.Repository
{
    public class BedRepository:Repository<Bed>
    {
        PharmacyContext cxt;
        public BedRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Bed> GetAllActiveBeds(int pageno, int pageSize)
        {
            return cxt.Beds.Where(b => b.IsActive).Include(b => b.Ward).OrderByDescending(b=>b.BedId).ToPagedList(pageno, pageSize);
        }

        public List<Bed> GetAllActiveBeds()
        {
            return cxt.Beds.Where(b => b.IsActive).ToList();
        }

        public Bed GetBedWithWardData(long BedId)
        {
            return cxt.Beds.Where(b => b.BedId == BedId).Include(b => b.Ward).FirstOrDefault();
        }
    }
}