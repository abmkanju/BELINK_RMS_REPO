using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class RackRepository : Repository<Rack>
    {
        PharmacyContext cxt;
        public RackRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public List<Rack> GetAllActiveRacks()
        {
            return cxt.Racks.Where(r => r.IsActive).ToList();
        }
        public IPagedList<RackVM> GetActiveRacks(int pageNo, int pageSize)
        {
            return cxt.Racks.Where(r => r.IsActive).Select(r => new RackVM
            {
                RackId = r.RackId,
                Name = r.Name,
                RackItems = r.RackItem.Where(x => x.IsActive).Select(ri => new RackItemsVM 
                {
                    ItemId = ri.Item.ItemID,
                    RackItemId = ri.Rack.RackId,
                    ItemName = ri.Item.ItemName,
                }).ToList()
            }).OrderByDescending(r => r.RackId)
            .ToPagedList(pageNo, pageSize);
        }

        public IPagedList<System.Linq.IGrouping<Rack, Item>> getRackView(int pageNo, int pageSize)
        {
            var r2 = (from i in cxt.Items
                      .Where(i => i.IsActive && !i.IsDefault)
                      group i by i.Rack)
                    .OrderByDescending(r => r.Key.RackId)
                    .ToPagedList(pageNo, pageSize);
            return r2;
            //Console.WriteLine("nothing");

        }
        public RackVM GetRackVMDetailsById(int RackId)
        {
            RackVM model = cxt.Racks.Where(x => x.RackId == RackId).Select(r => new RackVM
            {
                RackId = r.RackId,
                Name = r.Name,
                RackItems = r.RackItem.Where(x => x.IsActive).Select(ri => new RackItemsVM
                {
                    RackItemId = ri.RackItemId,
                    ItemId = ri.Item.ItemID,
                    ItemName = ri.Item.ItemName,

                }).ToList()
            }).FirstOrDefault();
            return model;
        }
        public Rack GetRackDetailsById(int RackId)
        {
            return cxt.Racks.Where(x => x.RackId == RackId).Include(ri => ri.RackItem ).Include(ri=>ri.RackItem.Select(x=>x.Item)).FirstOrDefault();

        }

        public bool RackExists_New(string name)
        {
            return cxt.Racks.Any(m => m.Name.ToLower() == name && m.IsActive);
        }

        public bool RackExists_Update(string name, int RackId)
        {
            return cxt.Racks.Any(m => m.Name.ToLower() == name && m.RackId != RackId && m.IsActive);
        }

        public RackItem GetRackItemById(int RackId, int ItemId)
        {
            return cxt.RackItems.Where(x => x.Rack.RackId == RackId && x.Item.ItemID == ItemId && x.IsActive).FirstOrDefault();
        }
        public bool CheckItemInRack(int ItemId, int RackId)
        {
            if (RackId > 0)
                return cxt.RackItems.Any(x => x.Item.ItemID == ItemId && x.IsActive && x.Rack.RackId != RackId);
            else
                return cxt.RackItems.Any(x => x.Item.ItemID == ItemId && x.IsActive);
        }

        public void SetRackItemInactive(int RackId, int ItemId)
        {
            List<RackItem> rackItems = cxt.RackItems.Where(x => x.Rack.RackId == RackId && x.Item.ItemID == ItemId && x.IsActive).ToList();
            rackItems.ForEach(x => { x.IsActive = false; x.DeletedAt = DateTime.Now; });
            cxt.SaveChanges();
        }

        public void InsertRackItem(RackItem rackItem)
        {
            cxt.RackItems.Add(rackItem);
        }
        public void InsertRackItemRange(List<RackItem> rackItems)
        {
            cxt.RackItems.AddRange(rackItems);
        }
        public void UpdateRackItem(RackItem obj)
        {
            cxt.Entry(obj).State = EntityState.Modified;
        }
        public RackItem GetRackItem(int RackId)
        {
            return cxt.RackItems.Where(x => x.Rack.RackId == RackId && x.IsActive).FirstOrDefault();
        }

    }
}
