using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{

    public class StockConsumptionRepository : Repository<StockConsumption>
    {
        PharmacyContext cxt;
        public StockConsumptionRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }






        public void BulkInsert(DataTable dtStockConsumption)
        {
            try
            {
                SqlBulkCopy objbulk = new SqlBulkCopy(cxt.Database.Connection.ConnectionString);
                objbulk.DestinationTableName = "StockConsumptions";
                objbulk.ColumnMappings.Add("Quantity", "Quantity");
                objbulk.ColumnMappings.Add("ConsumptionType", "ConsumptionType");
                objbulk.ColumnMappings.Add("Comment", "Comment");
                objbulk.ColumnMappings.Add("Batch_BatchId", "Batch_BatchId");
                objbulk.ColumnMappings.Add("Item_ItemID", "Item_ItemID");
                objbulk.ColumnMappings.Add("Invoice_InvoiceId", "Invoice_InvoiceId");
                objbulk.ColumnMappings.Add("Date", "Date");
                objbulk.WriteToServer(dtStockConsumption);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StockConsumptionItem GetConsumptionById(long StockConsumptionId)
        {
            return cxt.StockConsumptionItems
                .Where(s => s.StockConsumptionItemId == StockConsumptionId)
                .Include(s => s.Item)
                .Include(s => s.Batch).FirstOrDefault();
        }
        public StockConsumptionVM GetStockConsumptionById(long StockConsumptionId)
        {

            return cxt.StockConsumptions
          .Where(s => s.StockConsumptionId == StockConsumptionId)
          .Select(s => new StockConsumptionVM
          {
              StockConsumptionId = s.StockConsumptionId,
              CreateDate = s.CreateDate,
              UpdateDate = s.UpdateDate,
              User = s.User,
              StockConsumptionsList =
              (s.StockConsumptions.Select(sc => new StockConsumptionItemVM
              {
                  StockConsumptionItemId = sc.StockConsumptionItemId,
                  Item = sc.Item,
                  Batch = sc.Batch,
                  UnitString = sc.Unit == 0 ? sc.Item.Unit : "Units",
                  ItemName = sc.Item.ItemName,
                  BatchName = sc.Batch.BatchName,
                  UnitPrice = (cxt.StockItems.Where(i => i.Item.ItemID == sc.Item.ItemID && i.Batch.BatchId == sc.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()),
                  Quantity = sc.Quantity,
                  Comment = sc.Comment,
                  ConsumptionType = sc.ConsumptionType,
                  CreateDate = sc.CreateDate,
                  UpdateDate = sc.UpdateDate,
                  IsActive = sc.IsActive
              })).ToList()
          }).FirstOrDefault();
        }
        public StockConsumption GetStockConumptionById_(long StockConsumptionId)
        {
            return cxt.StockConsumptions
                .Include(c => c.StockConsumptions)
                .Include(c => c.StockConsumptions.Select(sc => sc.Item))
                .Include(c => c.StockConsumptions.Select(sc => sc.Batch))
                .Where(c => c.StockConsumptionId == StockConsumptionId)
                .FirstOrDefault();
        }

        //public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, string NameFilter, string BatchName, int PageNo, int PageSize)
        //{
        //    if (!string.IsNullOrEmpty(NameFilter) && !string.IsNullOrEmpty(BatchName))
        //    {
        //        return cxt.StockConsumptions
        //      .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
        //      .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
        //      .Where(s => s.Item.ItemName.Contains(NameFilter))
        //      .Where(s => s.Batch.BatchName.ToLower() == BatchName)
        //      .Where(s => s.IsActive)
        //      .Select(s => new StockConsumptionVM
        //      {
        //          StockConsumptionId = s.StockConsumptionId,
        //          //InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //          ItemName = s.Item.ItemName,
        //          BatchName = s.Batch.BatchName,
        //          Quantity = s.Quantity,
        //          ConsumptionType = s.ConsumptionType,
        //          ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //          Comment = s.Comment,
        //          TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == s.Item.ItemID && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //          CreateDate = s.CreateDate
        //      }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else if (!string.IsNullOrEmpty(NameFilter))
        //    {
        //        return cxt.StockConsumptions
        //            .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
        //            .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
        //            .Where(s => s.Item.ItemName.Contains(NameFilter))
        //            .Where(s => s.IsActive)
        //            .Select(s => new StockConsumptionVM
        //            {
        //                StockConsumptionId = s.StockConsumptionId,
        //                InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //                ItemName = s.Item.ItemName,
        //                BatchName = s.Batch.BatchName,
        //                Quantity = s.Quantity,
        //                ConsumptionType = s.ConsumptionType,
        //                ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //                Comment = s.Comment,
        //                TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == s.Item.ItemID && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //                CreateDate = s.CreateDate
        //            }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else if (!string.IsNullOrEmpty(BatchName))
        //    {
        //        return cxt.StockConsumptions
        //        .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
        //        .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
        //        .Where(s => s.Batch.BatchName.ToLower() == BatchName)
        //        .Where(s => s.IsActive)
        //        .Select(s => new StockConsumptionVM
        //        {
        //            StockConsumptionId = s.StockConsumptionId,
        //            InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //            ItemName = s.Item.ItemName,
        //            BatchName = s.Batch.BatchName,
        //            Quantity = s.Quantity,
        //            ConsumptionType = s.ConsumptionType,
        //            ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //            Comment = s.Comment,
        //            TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == s.Item.ItemID && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //            CreateDate = s.CreateDate
        //        }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else
        //    {
        //        return cxt.StockConsumptions
        //             .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
        //             .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
        //             .Where(s => s.IsActive)
        //             .Select(s => new StockConsumptionVM
        //             {
        //                 StockConsumptionId = s.StockConsumptionId,
        //                 InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //                 ItemName = s.Item.ItemName,
        //                 BatchName = s.Batch.BatchName,
        //                 Quantity = s.Quantity,
        //                 ConsumptionType = s.ConsumptionType,
        //                 ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //                 Comment = s.Comment,
        //                 TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == s.Item.ItemID && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //                 CreateDate = s.CreateDate
        //             }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //}

        //public void SetInActive(long StockConsumptionId)
        //{
        //    cxt.Database.ExecuteSqlCommand("Update StockConsumptions Set IsActive = 0 WHERE StockConsumptionId = @StockConsumptionId", new SqlParameter("@StockConsumptionId", StockConsumptionId));
        //}

        public bool AnyActiveConsExists()
        {
            return cxt.StockConsumptions.Any(c => c.IsActive);
        }

        #region frmViewItem
        public List<StockConsumptionItem> GetConsStockByBatchId(int BatchId, int ItemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                return (from c in cxt.StockConsumptions
                        join ci in cxt.StockConsumptionItems on c.StockConsumptionId equals ci.StockConsumption.StockConsumptionId
                        where c.IsActive && ci.IsActive && ci.Batch.BatchId == BatchId && ci.Item.ItemID == ItemId
                        && ci.Store != null && ci.Store.IsActive
                        select ci
                        )
                        .Include(r => r.Batch)
                        .Include(x => x.Store)
                        .ToList();
            }
            else
            {
                return (from c in cxt.StockConsumptions
                        join ci in cxt.StockConsumptionItems on c.StockConsumptionId equals ci.StockConsumption.StockConsumptionId
                        where c.IsActive && ci.IsActive && ci.Batch.BatchId == BatchId && ci.Item.ItemID == ItemId
                        select ci
                        )
                        .Include(r => r.Batch)
                        .ToList();
            }
        }
        public List<StockConsumptionItem> GetConsumptionsByItemId(int ItemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                return (from c in cxt.StockConsumptions
                        join ci in cxt.StockConsumptionItems on c.StockConsumptionId equals ci.StockConsumption.StockConsumptionId
                        where c.IsActive && ci.IsActive && ci.Item.ItemID == ItemId
                        && ci.Store != null && ci.Store.IsActive
                        select ci
                        )
                        .Include(r => r.Batch)
                        .Include(x => x.Store)
                        .ToList();
            }
            else
            {

                return (from c in cxt.StockConsumptions
                        join ci in cxt.StockConsumptionItems on c.StockConsumptionId equals ci.StockConsumption.StockConsumptionId
                        where c.IsActive && ci.IsActive && ci.Item.ItemID == ItemId
                        select ci
                       )
                       .Include(r => r.Batch)
                       .ToList();
            }
        }

        #endregion

        #region frmStockConsumptions
        public int CheckAllInvoiceItemReturn(long InvoiceId)
        {
            int result = 0;
            List<InvoiceItem> list = cxt.invoiceItems.Where(x => x.Invoice.InvoiceId == InvoiceId).ToList();
            foreach (var item in list)
            {
                if (item.Quantity > item.ReturnedQuantity)
                    return result = 0;
            }
            return result = 1;
            //return cxt.invoiceItems.Where(x => x.Invoice.InvoiceId == InvoiceId && x.Quantity == x.ReturnedQuantity).Count();
        }
        public List<ItemsVM> GetStockConsumptionsItems()
        {
            List<ItemsVM> items = new List<ItemsVM>();
            List<int> ItemIds = cxt.StockConsumptionItems.Select(i => i.Item.ItemID).Distinct().ToList();
            if (ItemIds != null && ItemIds.Count() > 0)
                items = cxt.Items.Where(x => ItemIds.Contains(x.ItemID)).Select(x => new ItemsVM { ItemId = x.ItemID, ItemName = x.ItemName, }).ToList();
            return items;
        }
        public IPagedList<StockConsumptionVM> GenericSearchForStockConsumptions(DateTime? fromDate, DateTime? toDate, int StoreId, int UserId, long ItemId, long InvoiceNo, int PageNo, int PageSize)
        {
            IPagedList<StockConsumptionVM> r = null;
            IQueryable<StockConsumption> result;
            result = cxt.StockConsumptions
                        .Where(i => i.IsActive)
                        .AsQueryable();
            if (InvoiceNo > 0)
                result = result.Join(cxt.Invoices, s => s.InvoiceId, i => i.InvoiceId, (s, i) => new { S = s, I = i }).Where(S => S.I.InvoiceRefNo == InvoiceNo).Select(x => x.S);
            if (UserId > 0)
                result = result.Where(s => s.User.UserId == UserId);
            if (ItemId > 0)
                result = result.Where(s => s.StockConsumptions.Any(si => si.IsActive && si.Item.ItemID == ItemId));
            if (fromDate.HasValue)
                result = result.Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(fromDate));
            if (toDate.HasValue)
                result = result.Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(toDate));
            if (StoreId > 0)
                result = result.Join(cxt.StockConsumptionItems, s => s.StockConsumptionId, i => i.StockConsumption.StockConsumptionId, (s, i) => new { S = s, I = i }).Where(S => S.I.Store != null && S.I.Store.IsActive && S.I.Store.StoreId == StoreId).Select(x => x.S).Distinct();
            r = result.Select(s => new StockConsumptionVM
                {
                    StockConsumptionId = s.StockConsumptionId, 
                    InvoiceId = s.InvoiceId,
                    InvoiceRefNo = (cxt.Invoices.Where(i => i.InvoiceId == s.InvoiceId).Select(item => (long?)item.InvoiceRefNo).FirstOrDefault() ?? 0),
                    CreateDate = s.CreateDate,
                    UpdateDate = s.UpdateDate,
                    Comment = s.Comment,
                    UserName = s.User.UserName,
                    StockConsumptionsList = (s.StockConsumptions.Select(l => new StockConsumptionItemVM
                    {
                        ItemId = l.Item.ItemID,
                        StockConsumptionItemId = l.StockConsumptionItemId,
                        UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                        ConversionUnit = l.Item.ConversionUnit,
                        ItemName = l.Item.ItemName,
                        Store = l.Store,
                        Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                        BatchId = l.Batch.BatchId,
                        BatchName = l.Batch.BatchName,
                        ConsumptionType = l.ConsumptionType,
                        ConsumptionTypeString = ((GlobalSharing.ConsumptionType)l.ConsumptionType).ToString(),
                        TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * l.Quantity,
                        UnitPrice = cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault(),
                        CreateDate = l.CreateDate,
                        IsActive = l.IsActive,
                        User = l.User
                    })).ToList()
                }).OrderByDescending(k => k.StockConsumptionId).ToPagedList(PageNo, PageSize);

            return r;

        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
            .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
            .Where(s => s.IsActive)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = (s.StockConsumptions.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemID,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((GlobalSharing.ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * l.Quantity,
                    CreateDate = l.CreateDate,
                    IsActive = l.IsActive,
                    User = l.User
                })).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(int ItemId, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.IsActive && s.StockConsumptions.Any(si => si.IsActive && si.Item.ItemID == ItemId))
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = (s.StockConsumptions.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemID,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((GlobalSharing.ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * l.Quantity,
                    CreateDate = l.CreateDate,
                    IsActive = l.IsActive,
                    User = l.User
                })).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, int UserId, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(StartDate))
            .Where(s => DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(EndDate))
            .Where(s => s.IsActive)
            .Where(s => s.User.UserId == UserId)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = (s.StockConsumptions.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemID,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((GlobalSharing.ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * l.Quantity,
                    CreateDate = l.CreateDate,
                    IsActive = l.IsActive,
                    User = l.User
                })).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.IsActive)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = (s.StockConsumptions.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemID,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    ItemName = l.Item.ItemName,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((GlobalSharing.ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = (cxt.StockItems.Where(i => i.Item.ItemID == l.Item.ItemID && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * l.Quantity,
                    CreateDate = l.CreateDate,
                    IsActive = l.IsActive,
                    User = l.User
                })).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        #endregion
    }
}