using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.Infrastructure.Repository
{
    public class BatchRepository : Repository<Batch>
    {
        PharmacyContext cxt;
        public BatchRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public Batch FindBatchByName(string BatchName)
        {
            return cxt.Batches.Where(b => b.BatchName.ToLower() == BatchName.ToLower()).FirstOrDefault();
        }

        public Batch FindItemBatch(string BatchName, int ItemId)
        {
            var batch =  (from s in cxt.StockItems
                    join b in cxt.Batches
                    on s.Batch.BatchId equals b.BatchId
                    where s.Item.ItemID == ItemId && b.BatchName.ToLower().Equals(BatchName)
                    select b).FirstOrDefault();
            return batch;
            //return cxt.Batches.Where(b => b.BatchName.ToLower() == BatchName.ToLower()).FirstOrDefault();
        }

        public Batch FindItemBatch(int BatchId, long ItemId)
        {
            return (from s in cxt.StockItems
                    join b in cxt.Batches
                    on s.Batch.BatchId equals b.BatchId
                    where s.Item.ItemID == ItemId && b.BatchId == BatchId
                    select b).FirstOrDefault();
        }

        public Batch InsertBatch(Batch objBatch)
        {
            cxt.Batches.Add(objBatch);
            cxt.SaveChanges();
            return objBatch;
        }

        public List<string> GetAllUniqueBatches()
        {
            return cxt.Batches.Select(b => b.BatchName).Distinct().ToList();
        }

        public List<SelectListVM> GetItemBatches(int ItemId)
        {
            var res = (from si in cxt.StockItems
                       join s in cxt.Stocks on si.Stock.StockId equals s.StockId
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where si.Item.ItemID == ItemId
                       select new SelectListVM
                       {
                           Value = b.BatchId,
                           Text = b.BatchName
                       }).ToList();
            return res;
        }
    }
}