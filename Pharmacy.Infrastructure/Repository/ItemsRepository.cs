using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Pharmacy.Core;
using System.Linq.Expressions;

namespace Pharmacy.Infrastructure.Repository
{
    public class ItemsRepository : Repository<Item>, IItemRepository
    {
        UnitOfWork unitOfWork;
        PharmacyContext cxt = null;
        public ItemsRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public double GetUnitCostByItemId(int ItemId)
        {
            return cxt.Items.Where(i => i.ItemID == ItemId).Select(i => i.UnitCostPrice).FirstOrDefault();
        }
        public string GetUnitByItemId(int ItemId)
        {
            return cxt.Items.Where(i => i.ItemID == ItemId).Select(i => i.Unit).FirstOrDefault();
        }
        public Item GetItemByMappedProductId(long ItemID)
        {
            return cxt.Items.FirstOrDefault(i => i.MappedProductId == ItemID);
        }
        public Item MyProperty { get; set; }
        public List<Item> GetActiveItems()
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault).ToList();
        }

        public List<bool> IsItemInStockOrPurchaseOrder(int ItemId)
        {
            List<bool> list = new List<bool>();
            list.Add(cxt.StockItems.Any(x => x.IsActive && x.Item.ItemID == ItemId));
            list.Add(cxt.PurchaseOrderItems.Any(x => x.IsActive && x.Item.ItemID == ItemId));
            return list;
        }

        public bool AnyNarcoticItem(List<long> Ids)
        {
            var items = cxt.Items.Where(x => x.IsNarcotic == true && Ids.Contains(x.ItemID)).ToList();
            return items.Count() > 0 ? true : false;
        }
        public List<BulkItemsVM> GetBulkAllActiveItems()
        {
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                return (from i in cxt.Items
                        where i.IsActive && !i.IsDefault
                        select new BulkItemsVM
                        {
                            ItemId = i.ItemID,
                            ItemName = i.ItemName,
                            Barcode = i.Barcode,
                            GenericName = i.ChecmicalName,
                            Unit = i.Unit,
                            Manufacturer = i.Manufacturer.Name,
                            RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId select si.RetailPrice).FirstOrDefault() ?? 0,
                            CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId select si.UnitCost).FirstOrDefault() ?? 0,
                        })
                        .OrderByDescending(r => r.ItemId)
                .ToList();
            }
            else
            {
                return (from i in cxt.Items
                        where i.IsActive && !i.IsDefault
                        select new BulkItemsVM
                        {
                            ItemId = i.ItemID,
                            ItemName = i.ItemName,
                            GenericName = i.ChecmicalName,
                            Manufacturer = i.Manufacturer.Name,
                            RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                            CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId select si.UnitCost).FirstOrDefault() ?? 0,
                        })
                .ToList();
            }
        }
        public List<Item> GetActiveItems(int GetCount)
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault).Take(GetCount).ToList();
        }

        public IPagedList<Item> GetMatchingItems(string SearchString, int StoreId, int PageNo, int PageSize)
        {
            IPagedList<Item> result = null;
            if (StoreId > 0)
            {
                result = (from i in cxt.Items
                          join si in cxt.StockItems on i.ItemID
                           equals si.Item.ItemID
                          where i.IsActive && si.Store.StoreId == StoreId &&
                              !i.IsDefault && si.IsActive
                              && (i.ItemName.ToLower().StartsWith(SearchString)
                              || i.ChecmicalName.ToLower().StartsWith(SearchString)
                              || i.Barcode.ToLower().Equals(SearchString))
                          select i).Distinct().OrderBy(i => i.ItemID).ToPagedList(PageNo, PageSize);

            }
            else if (!SharedVariables.IsStockModule && SharedVariables.AdminPharmacySetting.ShowStockItems)
            {
                result = (from i in cxt.Items
                          join si in cxt.StockItems on i.ItemID
                           equals si.Item.ItemID
                          where i.IsActive &&
                              !i.IsDefault && si.IsActive
                              && (i.ItemName.ToLower().StartsWith(SearchString)
                              || i.ChecmicalName.ToLower().StartsWith(SearchString)
                              || i.Barcode.ToLower().Equals(SearchString))
                          select i).Distinct().OrderBy(i => i.ItemID).ToPagedList(PageNo, PageSize);
            }
            else
            {
                result = cxt.Items
                    .Where(i => i.IsActive && !i.IsDefault && (i.ItemName.ToLower().StartsWith(SearchString) || i.ChecmicalName.ToLower().StartsWith(SearchString) || i.Barcode.ToLower().Equals(SearchString)))
                    .OrderBy(i => i.ItemID)
                    .ToPagedList(PageNo, PageSize);
            }
            return result;
        }

        public IEnumerable<Item> GetItemsBySupplierID(long SupplierID)
        {
            return (from i in cxt.Items
                    where i.Suppliers.Any(s => s.SupplierID == SupplierID) && i.IsActive
                    select i).ToList();
        }

        public Item GetDefaultItem()
        {
            return cxt.Items
                .Where(i => i.IsDefault).FirstOrDefault();
        }

        public int GetStockItemsCount()
        {
            //var items = (from s in cxt.Stocks
            //             join si in cxt.StockItems on s.StockId equals si.StockItemId
            //             join i in cxt.Items on si.Item.ItemID equals i.ItemID
            //             group i by i.ItemID into items2
            //             select items2).ToList();
            string qry = @"select count(x.ItemID) from
                            (
                            select i.ItemID, i.ItemName from items i
                            left join StockItems si on i.ItemID = si.Item_ItemID
                            left join stocks s on si.Stock_StockId= s.StockId
                            WHERE i.IsActive = 1
                            group by i.ItemID, i.ItemName
                            )x";
            int count = cxt.Database.SqlQuery<int>(qry).FirstOrDefault();
            return count;
        }
        public int GetStockItemsCount(string FilterString)
        {
            //var items = (from s in cxt.Stocks
            //             join si in cxt.StockItems on s.StockId equals si.StockItemId
            //             join i in cxt.Items on si.Item.ItemID equals i.ItemID
            //             group i by i.ItemID into items2
            //             select items2).ToList();
            string qry = @"select count(x.ItemID) from
                            (
                            select i.ItemID, i.ItemName from items i
                            left join StockItems si on i.ItemID = si.Item_ItemID                            
                            where i.ItemName like '%" + FilterString + "%' AND i.IsActive = 1" +
                            @"group by i.ItemID, i.ItemName
                            )x";
            int count = cxt.Database.SqlQuery<int>(qry, new SqlParameter("@FilterString", FilterString)).FirstOrDefault();
            return count;
        }
        public int GetStockItemsCount(DateTime StockDate)
        {
            //var items = (from s in cxt.Stocks
            //             join si in cxt.StockItems on s.StockId equals si.StockItemId
            //             join i in cxt.Items on si.Item.ItemID equals i.ItemID
            //             group i by i.ItemID into items2
            //             select items2).ToList();
            string qry = @"select count(x.ItemID) from
                            (
                            select i.ItemID, i.ItemName from items i
                            inner join StockItems si on i.ItemID = si.Item_ItemID
                            inner join stocks s on si.Stock_StockId= s.StockId
                            where convert(varchar,s.CreateDate,23)= '" + StockDate.ToString("yyyy-MM-dd ") + "' " +
                            " AND i.IsActive = 1 " +
                            @"group by i.ItemID, i.ItemName
                            )x";
            int count = cxt.Database.SqlQuery<int>(qry).FirstOrDefault();
            return count;
        }
        public List<BatchStockVM> GetItemsWithStockDataByItemId(int ItemId)
        {
            //            string qry = @"select x.batchId, x.BatchName, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, b.Expiry, b.CreatedAt  from(
            //                            select b.BatchId, b.BatchName, 
            //                            (select isnull(sum(si.Quantity), 0) from stockitems si where si.Item_ItemID = @ItemId and si.Batch_BatchId = b.BatchId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            (select ISNULL(sum(Quantity),0) from stockitems si where si.Item_ItemID = @ItemId and si.Batch_BatchId = b.BatchId and si.IsActive = 1)
            //                            -
            //                            (select ISNULL(sum(Quantity),0) from StockConsumptions sc where sc.Item_ItemID = @ItemId and sc.Batch_BatchId = b.BatchId and sc.IsActive = 1)
            //                            -
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = @ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            )as availableStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = @ItemId and ai.IsActive =1) as AdjustedStock
            //                            from StockItems si
            //                            inner join Batches b on si.Batch_BatchId = b.BatchId
            //                            where si.Item_ItemID = @ItemId AND si.IsActive = 1
            //                            group by b.BatchId, b.BatchName
            //                            )x
            //                            inner join Batches b on x.BatchId = b.BatchId Order by b.BatchId Desc";
            //List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

            List<BatchStockVM> Result = (from si in cxt.StockItems
                                         join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                         where si.Item.ItemID == ItemId
                                         group si by si.Batch.BatchId into BatchGroup
                                         join b_ in cxt.Batches on BatchGroup.Key equals b_.BatchId
                                         select new BatchStockVM()
                              {
                                  BatchId = b_.BatchId,
                                  BatchName = b_.BatchName,
                                  TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_.BatchId select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems join b_in in cxt.Batches on es.Batch.BatchId equals b_in.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_.BatchId && DbFunctions.TruncateTime(b_in.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0
                              }).ToList();

            return Result;
        }
        //here


        public ItemsVM getStockByItemId(int itemId)
        {
            ItemsVM r = cxt.Items
             .Where(i => i.ItemID == itemId)
             .Where(i => i.IsActive)
             .Where(i => !i.IsDefault)
             .Select(vm => new ItemsVM
             {
                 ItemId = vm.ItemID,
                 ItemName = vm.ItemName,
                 //Manufacturer = vm.Manufacturer,
                 //Barcode = vm.Barcode,
                 Unit = vm.Unit,
                 ConversionUnit = vm.ConversionUnit,
                 //ReorderingLevel = vm.ReOrderingLevel,
                 //IsNarcotic = vm.IsNarcotic,
                 //SuppliersList = vm.Suppliers.ToList(),
                 UnitCost = (from si in cxt.StockItems where si.Item.ItemID == vm.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                 RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == vm.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),
                 TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                 ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                 ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
                                -
                                (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                 AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
             }).FirstOrDefault();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockData()
        {
            //            string qry = @"select x.ItemID, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.itemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   IsNarcotic = vm.IsNarcotic,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        //report data
        public List<ItemsVM> GetItemsWithStockData_Print()
        {
            //            string qry = @"select x.ItemID, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.itemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            //return cxt.Items
            //   .Where(i => i.IsActive)
            //   .Select(vm => new ItemsVM
            //   {
            //       ItemId = vm.ItemID,
            //       ItemName = vm.ItemName,
            //       Barcode = vm.Barcode,
            //       Unit = vm.Unit,
            //       ReorderingLevel = vm.ReOrderingLevel,
            //       IsNarcotic = vm.IsNarcotic,
            //       SuppliersList = vm.Suppliers.ToList(),
            //       TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
            //       ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
            //       ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
            //       AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
            //   }).OrderByDescending(i => i.ItemId).ToList();

            List<ItemsVM> r = cxt.Items
            .Where(i => i.IsActive)
            .Select(vm => new ItemsVM
            {
                ItemId = vm.ItemID,
                ItemName = vm.ItemName,
                Barcode = vm.Barcode,
                Unit = vm.Unit,
                ReorderingLevel = vm.ReOrderingLevel,
                IsNarcotic = vm.IsNarcotic,
                SuppliersList = vm.Suppliers.ToList(),
                TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
                ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
            }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByStockDate(DateTime ItemCreateDate)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreateDate,23) = @ItemCreateDate
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            where i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreateDate", ItemCreateDate.ToString("yyyy-MM-dd "))).ToList();


            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(ItemCreateDate))
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
                    SuppliersList = vm.Suppliers.ToList(),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString)
        {
            string qry = @"select x.ItemID, x.ItemName,
                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
                            select i.ItemID, i.ItemName,
                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1) as TotalStock,
                            (
                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive =1 )
                            	-
                            	(select ISNULL(sum(Quantity),0) from stockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
                                -
                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
                            ) as AvailableStock,
                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) <CONVERT(varchar,getdate(),23)) as ExpiredStock,
                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
                            from items i
                            left join stockitems si on i.itemId = si.Item_ItemID
                            where i.ItemName like '%" + FilterString + "%' AND i.IsActive = 1" +
                            @" group by ItemID, ItemName
                            )x                            
                            inner join Items i on x.ItemID = i.ItemID 
                            WHERE i.IsActive = 1
                            order by x.ItemID Desc
                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();

            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemName.ToLower().Contains(FilterString))
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
                   SuppliersList = vm.Suppliers.ToList(),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public List<Item_SuppliersVM> GetSuppliersByItemIds(List<ItemsVM> itemsList)
        {
            List<int> ids = new List<int>();
            foreach (ItemsVM i in itemsList)
            {
                ids.Add(i.ItemId);
            }

            List<Item_SuppliersVM> vm = (from i in cxt.Items
                                         from s in i.Suppliers
                                         where ids.Contains(i.ItemID)
                                         select new Item_SuppliersVM()
                                         {
                                             ItemId = i.ItemID,
                                             SupplierId = s.SupplierID,
                                             SupplierName = s.Name
                                         }).ToList();
            return vm;
        }
        public List<BatchStockVM> GetBatchStockByItemId(int ItemId)
        {
            List<BatchStockVM> Result = new List<BatchStockVM>();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {

                Result = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              Expiry = b_g.Expiry,
                              CostPrice = cxt.StockItems.Where(i => i.Item.ItemID == ItemId && i.IsActive == true && i.Batch.BatchId == b_g.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault(),
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                          })
                                             .OrderBy(r => r.BatchId)
                                             .ToList();
            }
            else
            {
                Result = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              Expiry = b_g.Expiry,
                              CostPrice = cxt.StockItems.Where(i => i.Item.ItemID == ItemId && i.IsActive == true && i.Batch.BatchId == b_g.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault(),
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                          })
                                             .OrderByDescending(r => r.BatchId)
                                             .ToList();
            }
            return Result;
        }
        public ItemDetailVM GetBatchStockByItemId_1(int ItemId)
        {
            ItemDetailVM Result = new ItemDetailVM();
            //rf!@
            Result.Item = cxt.Items.Where(i => i.ItemID == ItemId).Include(i => i.Rack).FirstOrDefault();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              CostPrice = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts.UnitCost).FirstOrDefault(),
                              RetailPrice = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts.RetailPrice).FirstOrDefault(),
                              Expiry = b_g.Expiry,
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                          })
                                             .OrderBy(r => r.BatchId)
                                             .ToList();
                Result.BatchStockList = bStock;
            }
            else
            {
                 List<BatchStockVM> bStock  = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              Expiry = b_g.Expiry,
                              CostPrice = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts.UnitCost).FirstOrDefault(),
                              RetailPrice = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts.RetailPrice).FirstOrDefault(),
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                          })
                                             .OrderByDescending(r => r.BatchId)
                                             .ToList();
                 Result.BatchStockList = bStock;
            }
            return Result;
        }
        public ItemDetailVM GetBatchStockByItemId_2(int ItemId)
        {
            ItemDetailVM Result = new ItemDetailVM();
            //rf!@
            Result.Item = cxt.Items.Where(i => i.ItemID == ItemId).Include(i => i.Rack).FirstOrDefault();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemID == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM()
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 CreatedAt = b_g.CreatedAt,
                                                 Expiry = b_g.Expiry,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                                             -
                                                                (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                   (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                                             })
                                             .OrderBy(r => r.BatchId)
                                             .ToList();
                Result.BatchStockList = bStock;
            }
            else
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemID == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM()
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 CreatedAt = b_g.CreatedAt,
                                                 Expiry = b_g.Expiry,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                                             -
                                                                (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                   (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                             })
                                            .OrderByDescending(r => r.BatchId)
                                            .ToList();
                Result.BatchStockList = bStock;
            }
            return Result;
        }
        public List<BatchStockVM> GetBatchStockReturnByItemId(int ItemId)
        {
            List<BatchStockVM> Result = new List<BatchStockVM>();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                Result = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              Expiry = b_g.Expiry,
                              Unit = (from ss in cxt.StockItems where ss.IsActive && ss.Item.ItemID == ItemId && ss.Batch.BatchId == b_g.BatchId select ss).Sum(ss => (int?)ss.Unit) ?? 0,
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                          })
                                             .OrderBy(r => r.BatchId)
                                             .ToList();
            }
            else
            {
                Result = (from si in cxt.StockItems
                          join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                          where si.Item.ItemID == ItemId
                          group si by si.Batch.BatchId into BatchGroup
                          join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                          select new BatchStockVM()
                          {
                              BatchId = b_g.BatchId,
                              BatchName = b_g.BatchName,
                              CreatedAt = b_g.CreatedAt,
                              Expiry = b_g.Expiry,
                              Unit = (from ss in cxt.StockItems where ss.IsActive && ss.Item.ItemID == ItemId && ss.Batch.BatchId == b_g.BatchId select ss).Sum(ss => (int?)ss.Unit) ?? 0,
                              TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                              ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                              ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                             (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                              AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                              //--StockReturnQuantity--//
                              StockReturn =
                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              //--BonusQunatity--//
                              BonusStock =
                              (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                          })
                                             .OrderByDescending(r => r.BatchId)
                                             .ToList();
            }
            return Result;
        }
        public double GetBatchCostPriceById(int batchId)
        {
            double BatchCostPrice = 0;
            var model = (from si in cxt.StockItems where si.Batch.BatchId == batchId && si.IsActive select si).OrderByDescending(si => si.StockItemId).FirstOrDefault();
            if (model != null && model.UnitCost > 0)
                BatchCostPrice = model.UnitCost;
            return BatchCostPrice;
        }
        public List<BatchStockVM> GetStockItemBatchkByItemId(int ItemId)
        {
            List<BatchStockVM> Result = new List<BatchStockVM>();
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                #region WithStores
                if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                              where si.Item.ItemID == ItemId && si.IsActive && st.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  
                                  StoreName = st.Name,
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                    && ts.Store.StoreId == st.StoreId
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                       && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == st.StoreId
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  && es.Store != null && es.Store.IsActive && es.Store.StoreId == st.StoreId
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == st.StoreId
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                              }).OrderBy(r => r.BatchId).ToList();
                }
                else
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                              where si.Item.ItemID == ItemId && si.IsActive && st.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                    && ts.Store.StoreId == st.StoreId
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                       && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == st.StoreId
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  && es.Store != null && es.Store.IsActive && es.Store.StoreId == st.StoreId
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == st.StoreId
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                              }).OrderByDescending(r => r.BatchId).ToList();
                }
                #endregion
            }
            else
            {
                #region WithOutStore
                if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              where si.Item.ItemID == ItemId
                              group si by si.Batch.BatchId into BatchGroup
                              join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                              select new BatchStockVM()
                              {
                                  BatchId = b_g.BatchId,
                                  BatchName = b_g.BatchName,
                                  CreatedAt = b_g.CreatedAt,
                                  Expiry = b_g.Expiry,
                                  StoreName = "",
                                  TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              })
                                                 .OrderBy(r => r.BatchId)
                                                 .ToList();
                }
                else
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              where si.Item.ItemID == ItemId
                              group si by si.Batch.BatchId into BatchGroup
                              join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                              select new BatchStockVM()
                              {
                                  BatchId = b_g.BatchId,
                                  BatchName = b_g.BatchName,
                                  CreatedAt = b_g.CreatedAt,
                                  Expiry = b_g.Expiry,
                                  StoreName = "",
                                  TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                              })
                                                 .OrderByDescending(r => r.BatchId)
                                                 .ToList();
                }
                #endregion
            }
            return Result;
        }
        public List<BatchStockVM> GetStockItemBatchkByItemId2(int ItemId)
        {
            List<BatchStockVM> Result = new List<BatchStockVM>();
            if (SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                #region WithStores
                if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                              where si.Item.ItemID == ItemId && si.IsActive && st.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  Suppliers = si.Stock.Supplier,
                                  StoreName = st.Name,
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                    && ts.Store.StoreId == st.StoreId
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                       && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == st.StoreId
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  && es.Store != null && es.Store.IsActive && es.Store.StoreId == st.StoreId
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == st.StoreId
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                              }).OrderByDescending(r => r.BatchId).ToList();
                }
                else
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                              where si.Item.ItemID == ItemId && si.IsActive && st.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  Suppliers = si.Stock.Supplier,
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                    && ts.Store.StoreId == st.StoreId
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                       && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == st.StoreId
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  && es.Store != null && es.Store.IsActive && es.Store.StoreId == st.StoreId
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == st.StoreId
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                              }).OrderByDescending(r => r.BatchId).ToList();
                }
                #endregion
            }
            else
            {
                #region WithOutStore
                if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              
                              where si.Item.ItemID == ItemId && si.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  Suppliers = si.Stock.Supplier,
                                  StoreName = "",
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                   
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                     
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,

                              }).OrderByDescending(r => r.BatchId).ToList();
                }
                else
                {
                    Result = (from si in cxt.StockItems
                              join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                              where si.Item.ItemID == ItemId && si.IsActive
                              select new BatchStockVM()
                              {
                                  BatchId = b.BatchId,
                                  BatchName = b.BatchName,
                                  CreatedAt = b.CreatedAt,
                                  Expiry = b.Expiry,
                                  Suppliers = si.Stock.Supplier,
                                  TotalStock = (from ts in cxt.StockItems
                                                where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b.BatchId
                                                    
                                                select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                  ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                   where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b.BatchId
                                                   
                                                   select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                  ExpiredStock = (from es in cxt.StockItems
                                                  join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                  where es.Item.ItemID == ItemId
                                                  && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                  
                                                  select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                   join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                   where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                   && adj_item.IsActive && adj_item.Batch.BatchId == b.BatchId
                                                   
                                                   select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                  //--StockReturnQuantity--//
                                  StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                  //--BonusQunatity--//
                                  BonusStock =
                                  (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b.BatchId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                              }).OrderByDescending(r => r.BatchId).ToList();
                }
                #endregion
            }
            return Result;
        }
        public ItemDetailVM GetItemDetailWithBatchStock(int ItemId, bool useNewestStockPrice)
        {
            ItemDetailVM Result = new ItemDetailVM();
            //rf!@
            Result.Item = cxt.Items.Where(i => i.ItemID == ItemId && i.IsActive== true).Include(i => i.Rack).FirstOrDefault();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemID == ItemId && si.Store == null && si.IsActive
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Item.IsActive && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
                                                 //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //:
                                                 //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //+
                                                 //(from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                -
                                                                (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                   (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId && i.Store == null select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                                 RetailPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                                                 CostPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
                                             })
                                           .OrderBy(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            else
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemID == ItemId && si.Store == null && si.IsActive
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Item.IsActive && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
                                                 //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //    :
                                                 //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //    +
                                                 //    (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 RetailPrice = (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                                                 CostPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == b_g.BatchId && i.Store == null select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                             })
                                           .OrderByDescending(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            return Result;
        }
        public ItemDetailVM GetItemDetailWithBatchStoreWise(int ItemId, int StoreId, bool useNewestStockPrice)
        {
            ItemDetailVM Result = new ItemDetailVM();
            //rf!@
            Result.Item = cxt.Items.Where(i => i.ItemID == ItemId).Include(i => i.Rack).FirstOrDefault();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                                             where si.Item.ItemID == ItemId && st.IsActive && st.StoreId == StoreId && st.IsActive
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId

                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems
                                                               where ts.IsActive && ts.Item.ItemID == ItemId && ts.Store != null
                                                                   && ts.Store != null && ts.Store.IsActive && ts.Store.StoreId == StoreId
                                                                   && ts.Batch.BatchId == b_g.BatchId && ts.Item.IsActive
                                                               select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,

                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                                  where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId
                                                                      && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                                                  select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems
                                                              join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                                              where i.IsHoldInvoice
                                                                  && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId
                                                                  && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                                              select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems
                                                                 join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                                 where es.Item.ItemID == ItemId
                                                                     && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                                     && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                                                 select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                -
                                                                (from es in cxt.StockConsumptionItems
                                                                 join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                                 where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId
                                                                 && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                                 && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                                                 select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                                  join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                                  where adj.IsActive
                                                                  && adj_item.Item.ItemID == ItemId
                                                                  && adj_item.IsActive
                                                                  && adj_item.Batch.BatchId == b_g.BatchId
                                                                  && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == StoreId
                                                                  select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                   (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems
                                                  join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                                                  where ii.IsActive && i.IsActive
                                                      && ii.Item.ItemID == ItemId && i.Store.StoreId == StoreId && ii.Batch.BatchId == b_g.BatchId
                                                  select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                                 RetailPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                                                 CostPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
                                             })
                                           .OrderBy(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            else
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             join st in cxt.Stores on si.Store.StoreId equals st.StoreId
                                             where si.Item.ItemID == ItemId && st.IsActive && st.StoreId == StoreId && si.Item.IsActive
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems
                                                               where ts.IsActive
                                                                   && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId
                                                                   && ts.Store != null && ts.Store.IsActive && ts.Store.StoreId == StoreId && ts.Item.IsActive
                                                               select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0,

                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems
                                                                  where cs.IsActive
                                                                      && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId
                                                                      && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                                                  select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems
                                                              join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                                              where i.IsHoldInvoice
                                                                  && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId
                                                                  && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                                              select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems
                                                                 join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                                 where es.Item.ItemID == ItemId
                                                                     && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                                     && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                                                 select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                                                  join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                                                  where adj.IsActive && adj_item.Item.ItemID == ItemId
                                                                  && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId
                                                                  && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == StoreId
                                                                  select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                                 RetailPrice = (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                                                 CostPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && es.Store != null && es.IsActive && es.Store.StoreId == StoreId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
                                                 //--StockReturnQuantity--//
                                                 StockReturn =
                                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == b_g.BatchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                 //--BonusQunatity--//
                                                 BonusStock =
                                                 (from ii in cxt.invoiceItems
                                                  join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                                                  where ii.IsActive && i.IsActive
                                                      && ii.Item.ItemID == ItemId && i.Store.StoreId == StoreId && ii.Batch.BatchId == b_g.BatchId
                                                  select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                             })
                                           .OrderByDescending(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            return Result;
        }
        public int GetItemAvailableQty(int ItemId)
        {
            int avQty = 0;
            ItemsVM AvStock = (from si in cxt.StockItems
                               where si.Item.ItemID == ItemId
                               select new ItemsVM
                               {
                                   TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                   ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,

                                   ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                  -
                                                  (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                   AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                   //--StockReturnQuantity--//
                                   StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                   //--BonusQunatity--//
                                   BonusStock =
                                   (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                               }).FirstOrDefault();
            // if deduct hol quantity
            if (AvStock != null)
            {
                avQty = SharedFunctions.CalculatePurchaseQty(AvStock);
            }
            return avQty;
        }
        public int GetItemAvailableQty1(int ItemId , int BatchID)
        {
            int avQty = 0;
            ItemsVM AvStock = (from si in cxt.StockItems
                               where si.Item.ItemID == ItemId && si.Batch.BatchId == BatchID
                               select new ItemsVM
                               {
                                   TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == BatchID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                   ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == BatchID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == BatchID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,

                                   ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == BatchID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                  -
                                                  (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == BatchID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                   AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.Batch.BatchId == BatchID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                   //--StockReturnQuantity--//
                                   StockReturn =
                                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId  && sri.Batch.BatchId == BatchID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                   //--BonusQunatity--//
                                   BonusStock =
                                   (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId && ii.Batch.BatchId == BatchID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                               }).FirstOrDefault();
            // if deduct hol quantity
            if (AvStock != null)
            {
                avQty = SharedFunctions.CalculatePurchaseQty(AvStock);
            }
            return avQty;
        }

        private void GetName(object AvStock)
        {
            throw new NotImplementedException();
        }
        public ItemsVM GetItemExpiredDetails(int itemId)
        {
            ItemsVM r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemID == itemId)
               .Select(vm => new ItemsVM
               {
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
               }).FirstOrDefault();
            return r;
        }
        public ItemsVM GetItemWithAvQty(int ItemId)
        {
            ItemsVM res = cxt.Items.Where(i => i.ItemID == ItemId)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    Manufacturer = i.Manufacturer,
                    
                    
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,

                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    //UnitCost = i.UnitCostPrice,
                    //RetailPrice = i.RetailPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Stock.IsHolding == 0 && ts.Stock.IsActive == true && ts.Stock.Ishold == false &&  ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                     BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).FirstOrDefault();
            if (res != null)
                SharedFunctions.CalculateItemAvailableQty(res);
            return res;
        }
        public ItemsVM GetItemWithAvQtywithoutexpiry(int ItemId)
        {
            ItemsVM res = cxt.Items.Where(i => i.ItemID == ItemId)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    Manufacturer = i.Manufacturer,


                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,

                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    //UnitCost = i.UnitCostPrice,
                    //RetailPrice = i.RetailPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Stock.IsHolding == 0 && ts.Stock.IsActive == true && ts.Stock.Ishold == false && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                   (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).FirstOrDefault();
            if (res != null)
                SharedFunctions.CalculateItemAvailableQtywithoutexpirty(res);
            return res;
        }
        public ItemsVM GetItemWithAvQtywithouthold(int ItemId)
        {
            ItemsVM res = cxt.Items.Where(i => i.ItemID == ItemId)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,

                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,

                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    //UnitCost = i.UnitCostPrice,
                    //RetailPrice = i.RetailPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Stock.IsHolding == 0 && ts.Stock.IsActive == true && ts.Stock.Ishold == false && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.ConsumptionType == 2 select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                   (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).FirstOrDefault();
            if (res != null)
                SharedFunctions.CalculateItemAvailableQty(res);
            return res;
        }

        public ItemsVM GetItemWithAvQtywithouthold_1(int ItemId)
        {
            ItemsVM res = cxt.Items.Where(i => i.ItemID == ItemId)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,

                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,

                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    //UnitCost = i.UnitCostPrice,
                    //RetailPrice = i.RetailPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Stock.IsHolding == 0 && ts.Stock.IsActive == true && ts.Stock.Ishold == false && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                   (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).FirstOrDefault();
            if (res != null)
                SharedFunctions.CalculateItemAvailableQty(res);
            return res;
        }
        public ItemDetailVM GetItemDetailWithStock(int ItemId)
        {
            //            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
            //                        from(
            //                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
            //                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
            //                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
            //                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = si.Item_itemId and ai.IsActive =1) as AdjustedStock
            //                		from stockitems si
            //                		inner join Batches b on si.Batch_BatchId = b.BatchId
            //                		where si.Item_ItemID = @ItemID and si.IsActive = 1
            //                		group by b.BatchId, b.BatchName, si.Item_ItemID
            //                	)x
            //                left join Batches b on b.BatchId = x.BatchId
            //                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            //            List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemID", ItemId)).ToList();

            ItemDetailVM Result = (from si in cxt.StockItems
                                   where si.Item.ItemID == ItemId && si.IsActive
                                   select new ItemDetailVM
                                   {
                                       Item = cxt.Items.Where(i => i.ItemID == ItemId).FirstOrDefault(),
                                       AvailableStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId select ts).Sum(ts => (int?)ts.Quantity + ts.BonusQuantity) ?? 0
                                        - (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                        - (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.IsActive && es.Item.ItemID == ItemId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity + es.BonusQuantity) ?? 0
                                        + (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0
                                   }).FirstOrDefault();
            return Result;
        }
        public BatchStockVM GetBatchStock(int batchId, int ItemId, int StoreId, bool UseNewestStockPrice)
        {
            BatchStockVM Result = null;
            //            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
            //                        from(
            //                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
            //                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
            //                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
            //                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = si.Item_itemId and ai.IsActive =1) as AdjustedStock
            //                		from stockitems si
            //                		inner join Batches b on si.Batch_BatchId = b.BatchId
            //                		where si.Item_ItemID = @ItemID and si.IsActive = 1
            //                		group by b.BatchId, b.BatchName, si.Item_ItemID
            //                	)x
            //                left join Batches b on b.BatchId = x.BatchId
            //                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            //List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemID", ItemId)).ToList();

            //BatchStockVM Result = (from si in cxt.StockItems
            //                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
            //                       where si.Item.ItemID == ItemId && si.Batch.BatchId == batchId
            //                       group si by si.Batch.BatchId into BatchGroup
            //                       join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
            //                       select new BatchStockVM()
            //                       {
            //                           BatchId = b_g.BatchId,
            //                           BatchName = b_g.BatchName,
            //                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusDiscount)) ?? 0,
            //                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
            //                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
            //                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
            //                           RetailPrice = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice,
            //                       }).FirstOrDefault();
            //return Result;
            if (StoreId > 0)
            {
                Result = new BatchStockVM()
                {
                    TotalStock = (from ts in cxt.StockItems
                                  where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == batchId
                                      && ts.Store != null && ts.Store.IsActive && ts.Store.StoreId == StoreId
                                  select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems
                                     where cs.IsActive && cs.Item.ItemID == ItemId
                                         && cs.Batch.BatchId == batchId
                                         && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                     select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems
                                 join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                 where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId
                                 && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                 select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems
                                    join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                    where
                                        es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                        DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                        && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                    select es).Sum(es => (int?)es.Quantity) ?? 0
                                   -
                                  (from es in cxt.StockConsumptionItems
                                   join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                   where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                   DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                   && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                   select es).Sum(es => (int?)es.Quantity) ?? 0,

                    AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                     join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                     where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive &&
                                     adj_item.Batch.BatchId == batchId
                                     && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == StoreId
                                     select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                    //RetailPrice = (double?) (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == batchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQuantity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems
                     join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                     where ii.IsActive && i.IsActive
                         && ii.Item.ItemID == ItemId && i.Store.StoreId == StoreId && ii.Batch.BatchId == batchId
                     select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    RetailPrice =
                    UseNewestStockPrice ?
                                     (double?)(from es in cxt.StockItems
                                               where es.Item.ItemID == ItemId &&
                                                   es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                               orderby es.StockItemId descending
                                               select es.RetailPrice).FirstOrDefault() ?? 0
                                     :
                                     (double?)(from es in cxt.StockItems
                                               join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                               where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                               es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                               orderby es.StockItemId descending
                                               select es.RetailPrice).FirstOrDefault() ?? 0,
                    CostPrice =
                    UseNewestStockPrice ?
                                    (double?)(from es in cxt.StockItems
                                              where es.Item.ItemID == ItemId &&
                                                  es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                              orderby es.StockItemId descending
                                              select es.UnitCost).FirstOrDefault() ?? 0
                                    :
                                    (double?)(from es in cxt.StockItems
                                              join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                              where es.Item.ItemID == ItemId &&
                                              es.Batch.BatchId == batchId &&
                                              es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                              orderby es.StockItemId descending
                                              select es.UnitCost).FirstOrDefault() ?? 0
                };
            }
            else
            {
                Result = new BatchStockVM()
                                       {
                                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == batchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                           HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                                          -
                                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == batchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                                           //RetailPrice = (double?) (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                                           //--StockReturnQuantity--//
                                           StockReturn =
                                           (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == batchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                           //--BonusQuantity--//
                                           BonusStock =
                                           (from ii in cxt.invoiceItems
                                            join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                                            where ii.IsActive && i.IsActive
                                                && ii.Item.ItemID == ItemId && i.Store == null && ii.Batch.BatchId == batchId
                                            select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                           RetailPrice =
                                           UseNewestStockPrice ?
                                                            (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                                            :
                                                            (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                                           CostPrice =
                                           UseNewestStockPrice ?
                                                           (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                           :
                                                           (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                       };

            }
            return Result;
        }

        public BatchStockVM GetBatchStockSaleRet(int batchId, int ItemId, int StoreId, bool UseNewestStockPrice)
        {
            BatchStockVM Result = null;
            if (StoreId > 0)
            {
                Result = new BatchStockVM()
                {
                    TotalStock = (from ts in cxt.StockItems
                                  where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == batchId
                                      && ts.Store != null && ts.Store.IsActive && ts.Store.StoreId == StoreId
                                  select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems
                                     where cs.IsActive && cs.Item.ItemID == ItemId
                                         && cs.Batch.BatchId == batchId
                                         && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                     select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems
                                 join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                 where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId
                                 && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                 select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems
                                    join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                    where
                                        es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                        DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                        && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                    select es).Sum(es => (int?)es.Quantity) ?? 0
                                   -
                                  (from es in cxt.StockConsumptionItems
                                   join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                   where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                   DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                   && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                   select es).Sum(es => (int?)es.Quantity) ?? 0,

                    AdjustedStock = (from adj_item in cxt.AdjustmentItems
                                     join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId
                                     where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive &&
                                     adj_item.Batch.BatchId == batchId
                                     && adj_item.Store != null && adj_item.Store.IsActive && adj_item.Store.StoreId == StoreId
                                     select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                    //RetailPrice = (double?) (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == batchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQuantity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems
                     join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                     where ii.IsActive && i.IsActive
                         && ii.Item.ItemID == ItemId && i.Store.StoreId == StoreId && ii.Batch.BatchId == batchId
                     select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    RetailPrice =
                    UseNewestStockPrice ?
                                     (double?)(from es in cxt.StockItems
                                               where es.Item.ItemID == ItemId &&
                                                   es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                               orderby es.StockItemId descending
                                               select es.RetailPrice).FirstOrDefault() ?? 0
                                     :
                                     (double?)(from es in cxt.StockItems
                                               join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                               where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId &&
                                               es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                               orderby es.StockItemId descending
                                               select es.RetailPrice).FirstOrDefault() ?? 0,
                    CostPrice =
                    UseNewestStockPrice ?
                                    (double?)(from es in cxt.StockItems
                                              where es.Item.ItemID == ItemId &&
                                                  es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                              orderby es.StockItemId descending
                                              select es.UnitCost).FirstOrDefault() ?? 0
                                    :
                                    (double?)(from es in cxt.StockItems
                                              join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                              where es.Item.ItemID == ItemId &&
                                              es.Batch.BatchId == batchId &&
                                              es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                              orderby es.StockItemId descending
                                              select es.UnitCost).FirstOrDefault() ?? 0
                };
            }
            else
            {
                Result = new BatchStockVM()
                {
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemId && ts.Batch.BatchId == batchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                   -
                                  (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == batchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                    //RetailPrice = (double?) (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId && (b_.Expiry == null || DbFunctions.TruncateTime(b_.Expiry) > DbFunctions.TruncateTime(DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemId && sri.Batch.BatchId == batchId && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQuantity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems
                     join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId
                     where ii.IsActive && i.IsActive
                         && ii.Item.ItemID == ItemId && i.Store == null && ii.Batch.BatchId == batchId
                     select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    RetailPrice =
                    UseNewestStockPrice ?
                                     (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                     :
                                     (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                    CostPrice =
                    UseNewestStockPrice ?
                                    (double?)(from es in cxt.StockItems where es.Item.ItemID == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                    :
                                    (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                };

            }
            return Result;
        }
        public List<BatchStockVM> GetBatchStockByItemId(int itemId, long BatchId)
        {
            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
                        from(
                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = si.Item_ItemID and ai.IsActive =1) as AdjustedStock
                		from stockitems si
                		left join Batches b on si.Batch_BatchId = b.BatchId
                		where si.Item_ItemID = @ItemID and si.IsActive = 1
                        AND si.Batch_BatchId = @BatchID
                		group by b.BatchId, b.BatchName, si.Item_ItemID
                	)x
                left join Batches b on b.BatchId = x.BatchId
                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            List<BatchStockVM> result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemID", itemId), new SqlParameter("@BatchID", BatchId)).ToList();
            return result;
        }
        public double GetItemRate(int iItemId, int? BatchId)
        {
            return cxt.StockItems.Where(i => i.Item.ItemID == iItemId && i.Batch.BatchId == BatchId).OrderByDescending(i => i.StockItemId).Select(si => si.RetailPrice).FirstOrDefault();
            //return obj.RetailPrice;
        }

        public double GetUnitCost(int iItemId, int? BatchId)
        {
            return cxt.StockItems.Where(i => i.Item.ItemID == iItemId && i.Batch.BatchId == BatchId).OrderByDescending(i => i.StockItemId).Select(si => si.UnitCost).FirstOrDefault();
        }

        public double GetDiscCost(int iItemId, int? BatchId)
        {
            return cxt.StockItems.Where(i => i.Item.ItemID == iItemId && i.Batch.BatchId == BatchId).OrderByDescending(i => i.StockItemId).Select(si => si.DiscountedPrice).FirstOrDefault();
        }

        public Item GetItemData_ByITemId(int ItemId)
        {
            Item item = cxt.Items
                .Where(i => i.ItemID == ItemId)
                .Include(i => i.Suppliers)
                .Include(i => i.Manufacturer)
                //rf!@
                .Include(i => i.Rack)
                .Include(i => i.Category).FirstOrDefault();
            item.RackId = (from ri in cxt.RackItems where ri.Item.ItemID == ItemId && ri.IsActive select (int?)ri.Rack.RackId).FirstOrDefault() ?? 0;
            return item;
        }

        public int ItemAlreadyExists(string ItemName, string Barcode)
        {
            return cxt.Database.SqlQuery<int>(@"if exists(select ItemId from Items where LOWER(ItemName) = LOWER(@ItemName) and IsActive = 1) select 1
				else if exists(select ItemId from Items where LOWER(Barcode) = @Barcode and barcode != '' and IsActive = 1) select 2
				else select 0", new SqlParameter("ItemName", ItemName), new SqlParameter("@Barcode", Barcode)).FirstOrDefault();
        }
        public int ItemAlreadyExists(int ItemId, string ItemName, string Barcode)
        {
            return cxt.Database.SqlQuery<int>(@"if exists(select ItemId from Items where LOWER(ItemName) = LOWER(@ItemName) and IsActive = 1 and ItemId != @ItemId) select 1
				else if exists(select ItemId from Items where LOWER(Barcode) = @Barcode and barcode != '' and IsActive = 1 and ItemId != @ItemId) select 2
				else select 0", new SqlParameter("ItemName", ItemName), new SqlParameter("@Barcode", Barcode), new SqlParameter("@ItemId", ItemId)).FirstOrDefault();
        }

        public ItemWiseSalesMasterVM GetItemWiseSales(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            IPagedList<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                                    join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                                    where i.IsActive && !i.IsQInvoice && ii.IsActive && ii.ReturnedQuantity != ii.Quantity
                                                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                    //orderby i.InvoiceId descending
                                                    group ii by ii.Item.ItemID into ItemsG
                                                    select new ItemWiseSaleVM
                                                    {
                                                        ItemId = ItemsG.Key,
                                                        ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                        SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                       -
                                                                       (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                                        //--StockReturnQuantity--//
                                                        StockReturn =
                                                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemsG.Key && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                        //--BonusQunatity--//
                                                        BonusStock =
                                                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && !i.IsQInvoice && i.IsActive && ii.Item.ItemID == ItemsG.Key select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                                    }).OrderBy(i => i.ItemName).ToPagedList(PageNo, PageSize);

            // just filling available stock variable by looping above retrived results
            foreach (ItemWiseSaleVM i in SalesList)
            {
                SharedFunctions.CalculateItemwiseSaleReport(i);
            }

            ItemWiseSalesMasterVM v = new ItemWiseSalesMasterVM();
            v.GrandSalesTotalQuantity = (cxt.Invoices
                                               .Where(i => i.IsActive
                                                   && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) &&
                                                   DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                                                    .Sum(inv => (long?)inv.InvoiceItems.Where(item => item.IsActive)
                                                        .Sum(item => item.Quantity - item.ReturnedQuantity)));
            v.ItemWiseSales = SalesList;
            return v;
        }

        public ItemWiseSalesMasterVM GetItemWiseSales(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            IPagedList<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                                    join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                                    where i.IsActive && !i.IsQInvoice  && ii.IsActive && ii.Item.ItemID == ItemId && ii.ReturnedQuantity != ii.Quantity
                                                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                    group ii by ii.Item.ItemID into ItemsG
                                                    select new ItemWiseSaleVM
                                                    {
                                                        ItemId = ItemsG.Key,
                                                        ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                        SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                       -
                                                                       (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                                    }).OrderBy(i => i.ItemName).ToPagedList(1, 1);
            // just filling available stock variable by looping above retrived results
            foreach (ItemWiseSaleVM i in SalesList)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            ItemWiseSalesMasterVM v = new ItemWiseSalesMasterVM();
            v.GrandSalesTotalQuantity = (cxt.Invoices
                                            .Where(i => i.IsActive && !i.IsQInvoice
                                                && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                            )
                                            .Sum(inv => (long?)inv.InvoiceItems.Where(item => item.IsActive && !item.IsQInvoice && item.Item.ItemID == ItemId)
                                                .Sum(item => item.Quantity - item.ReturnedQuantity)));
            v.ItemWiseSales = SalesList;
            return v;
        }

        public ItemWisePurchaseMasterVM GetItemWisePurchases(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            IPagedList<ItemWisePurchaseVM> PurchasesList = (from i in cxt.Stocks
                                                            join ii in cxt.StockItems on i equals ii.Stock
                                                            where i.IsActive && ii.IsActive && ii.Quantity > 0
                                                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                            //orderby i.InvoiceId descending
                                                            group ii by ii.Item.ItemID into ItemsG
                                                            select new ItemWisePurchaseVM
                                                            {
                                                                ItemId = ItemsG.Key,
                                                                ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                                PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                                HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                               -
                                                                               (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                                                //--StockReturnQuantity--//
                                                                StockReturn =
                                                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemsG.Key && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                                //--BonusQunatity--//
                                                                BonusStock =
                                                                (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemsG.Key select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                                            }).OrderBy(i => i.ItemName).ToPagedList(PageNo, PageSize);



            // just filling available stock variable by looping above retrived results
            foreach (ItemWisePurchaseVM i in PurchasesList)
            {
                SharedFunctions.CalculateItemwisePurchaseReport(i);
            }


            ItemWisePurchaseMasterVM v = new ItemWisePurchaseMasterVM();
            v.GrandPurchaseTotalQuantity = (cxt.Stocks
                                               .Where(i => i.IsActive
                                                   && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) &&
                                                   DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                                                    .Sum(inv => (long?)inv.StockItems.Where(item => item.IsActive)
                                                        .Sum(item => item.Quantity)));
            v.ItemWisePurchases = PurchasesList;
            return v;
        }

        public ItemWisePurchaseMasterVM GetItemWisePurchases(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            IPagedList<ItemWisePurchaseVM> PurchasesList = (from i in cxt.Stocks
                                                            join ii in cxt.StockItems on i equals ii.Stock
                                                            where i.IsActive && ii.IsActive && ii.Quantity > 0 && ii.Item.ItemID == ItemId
                                                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                            //orderby i.InvoiceId descending
                                                            group ii by ii.Item.ItemID into ItemsG
                                                            select new ItemWisePurchaseVM
                                                            {
                                                                ItemId = ItemsG.Key,
                                                                ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                                PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                                HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                               -
                                                                               (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                                                //--StockReturnQuantity--//
                                                                StockReturn =
                                                                (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == ItemsG.Key && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                                //--BonusQunatity--//
                                                                BonusStock =
                                                                (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == ItemId select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                                                            }).OrderBy(i => i.ItemName).ToPagedList(1, 1);


            // just filling available stock variable by looping above retrived results
            foreach (ItemWisePurchaseVM i in PurchasesList)
            {
                SharedFunctions.CalculateItemwisePurchaseReport(i);
            }


            ItemWisePurchaseMasterVM v = new ItemWisePurchaseMasterVM();
            v.GrandPurchaseTotalQuantity = (cxt.Stocks
                                               .Where(i => i.IsActive
                                                   && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) &&
                                                   DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                                                    .Sum(inv => (long?)inv.StockItems.Where(item => item.IsActive)
                                                        .Sum(item => item.Quantity)));
            v.ItemWisePurchases = PurchasesList;
            return v;
        }
        public List<InvoiceItemVM> GetItemWiseRevenue(DateTime DateFrom, DateTime DateTo)
        {
            List<InvoiceVM> RevenueList = cxt.Invoices
                                                    .Where(i => i.IsActive
                                                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                    )
                                                    .Select(i => new InvoiceVM
                                                    {
                                                        InvoiceId = i.InvoiceId,
                                                        InvoiceItems = (i.InvoiceItems.Select(ii => new InvoiceItemVM
                                                        {
                                                            ItemId = ii.Item.ItemID,
                                                            ItemName = ii.Item.ItemName,
                                                            Amount = ii.Amount,
                                                            Quantity = ii.Quantity,
                                                            ReturnedQuantity = ii.ReturnedQuantity
                                                        })),
                                                        TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                                                        SubTotal = i.SubTotal,
                                                        GrandTotal = i.GrandTotal
                                                    }).OrderBy(i => i.InvoiceId).ToList();//.ToPagedList(PageNo, PageSize);
            List<InvoiceItemVM> items = new List<InvoiceItemVM>();
            bool itemFound = false;
            int ActualSaleQty = 0;
            double perItemPaymentShare = 0;
            foreach (InvoiceVM i in RevenueList)
            {
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    ActualSaleQty = 0;
                    perItemPaymentShare = 0;
                    itemFound = false;
                    ActualSaleQty = ii.Quantity - ii.ReturnedQuantity;
                    perItemPaymentShare = (((ii.Amount / i.SubTotal) * i.TotalPaid) / ii.Quantity);
                    ii.Revenue = ActualSaleQty * perItemPaymentShare;
                    foreach (var item in items)
                    {
                        if (item.ItemId == ii.ItemId)
                        {
                            itemFound = true;
                            item.Revenue += ii.Revenue;
                            break;// exit if found to avoid extra iterations
                        }
                    }
                    if (!itemFound)
                    {
                        if (ii.Revenue > 0)
                        {
                            items.Add(ii);
                        }
                    }
                }
            }
            return items;
        }

        public List<InvoiceItemVM> GetItemWiseRevenue(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<InvoiceVM> RevenueList = cxt.Invoices
                                                    .Where(i => i.IsActive
                                                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                    )
                                                    .Select(i => new InvoiceVM
                                                    {
                                                        InvoiceId = i.InvoiceId,
                                                        InvoiceItems = (i.InvoiceItems.Where(ii => ii.Item.ItemID == ItemId).Select(ii => new InvoiceItemVM
                                                        {
                                                            ItemId = ii.Item.ItemID,
                                                            ItemName = ii.Item.ItemName,
                                                            Amount = ii.Amount,
                                                            Quantity = ii.Quantity,
                                                            ReturnedQuantity = ii.ReturnedQuantity
                                                        })),
                                                        TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                                                        SubTotal = i.SubTotal,
                                                        GrandTotal = i.GrandTotal
                                                    }).OrderBy(i => i.InvoiceId).ToList();//.ToPagedList(PageNo, PageSize);
            bool itemFound = false;
            int ActualSaleQty = 0;
            double perItemPaymentShare = 0;
            List<InvoiceItemVM> items = new List<InvoiceItemVM>();
            foreach (InvoiceVM i in RevenueList)
            {
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    ActualSaleQty = 0;
                    perItemPaymentShare = 0;
                    itemFound = false;
                    ActualSaleQty = ii.Quantity - ii.ReturnedQuantity;
                    perItemPaymentShare = (((ii.Amount / i.SubTotal) * i.TotalPaid) / ii.Quantity);
                    ii.Revenue = ActualSaleQty * perItemPaymentShare;

                    foreach (var item in items)
                    {
                        if (item.ItemId == ii.ItemId)
                        {
                            itemFound = true;
                            item.Revenue += ii.Revenue;
                            break;// exit if found to avoid extra iterations
                        }
                    }
                    if (!itemFound)
                    {
                        if (ii.Revenue > 0)
                        {
                            items.Add(ii);
                        }
                    }
                }
            }
            return items;
        }
        public ItemWiseRevenueMasterVM GetItemWiseRevenue_All(int pageNo, int pageSize)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     where (i.IsActive && ii.IsActive)
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToPagedList(pageNo, pageSize);

            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int batchid = 0;
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += ActualSaleQty * perItemPaymentShare;
                }

                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    {
                        obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    }
                    else
                    {
                        obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    }
                }
            }

            ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            v.HasNextPage = x.HasNextPage;
            v.HasPreviousPage = x.HasPreviousPage;
            v.IsFirstPage = x.IsFirstPage;
            v.IsLastPage = x.IsLastPage;
            v.PageCount = x.PageCount;
            v.ItemWiseRevenue = lst;
            return v;
        }

        public ItemWiseRevenueMasterVM GetItemWiseRevenue_Category(int pageNo, int pageSize, int CategoryId , DateTime Datefrom, DateTime Dateto)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice && ii.IsActive && ii.Item.Category.CategoryId == CategoryId
                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(Datefrom)
                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                     )
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToPagedList(pageNo, pageSize);
            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int batchid = 0;
            List<int> ids = new List<int>();
  
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive&& !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    double Disc = 0;
                    double DiscountAmount = 0;
                    if (invoice.TotalDiscount > 0)
                    {
                        if (r.Discount > 0)
                        {
                            if (r.DiscountType == 1)
                            {
                                Disc = r.Discount / 100;
                                DiscountAmount = r.Rate * Disc * ActualSaleQty;
                            }
                            else
                            {
                                Disc = r.Discount / r.Quantity;
                                DiscountAmount = ActualSaleQty * Disc;
                            }
                        }
                        else
                        {
                            var anyqtyDisc = false;
                            var rrt = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId).Select(xx => xx.Discount).ToList();

                            foreach (var ch in rrt)
                            {
                                if (ch > 0)
                                {
                                    anyqtyDisc = true;
                                }
                            }
                            if (!anyqtyDisc)
                            {
                                if (invoice.DiscountType == 1)
                                {
                                    var sumtotqty = invoice.InvoiceItems.Sum(ik => ik.Quantity);
                                    Disc = invoice.TotalDiscount / rrt.Count();
                                    DiscountAmount = (Disc / r.Quantity) * ActualSaleQty;
                                }
                                else
                                {
                                    Disc = invoice.TotalDiscount / 100;
                                    DiscountAmount = r.Rate * Disc * ActualSaleQty;
                                }
                            }


                        }
                    }
                    //perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    perItemPaymentShare = (r.Rate);
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += (ActualSaleQty * perItemPaymentShare) - DiscountAmount;
                }

                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }

            ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            v.HasNextPage = x.HasNextPage;
            v.HasPreviousPage = x.HasPreviousPage;
            v.IsFirstPage = x.IsFirstPage;
            v.IsLastPage = x.IsLastPage;
            v.PageCount = x.PageCount;
            v.ItemWiseRevenue = lst;
            return v;
        }
        public ItemWiseRevenueMasterVM GetItemWiseRevenue_Item(int pageNo, int pageSize, int ItemId, DateTime Datefrom, DateTime Dateto)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice && ii.IsActive && ii.Item.ItemID == ItemId
                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(Datefrom)
                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                     )
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToPagedList(pageNo, pageSize);
            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int batchid = 0;
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                    l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    
                        //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                        var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                        double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                        //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                        ActualSaleQty = r!=null ? r.Quantity - r.ReturnedQuantity : 0;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    //perItemPaymentShare = r!=null?(((r.Amount / t.SubTotal) * actualPaid) / r.Quantity) : 0;
                    double Disc = 0;
                    double DiscountAmount = 0;
                    if (invoice.TotalDiscount > 0)
                    {
                        if (r.Discount > 0)
                        {
                            if (r.DiscountType == 1)
                            {
                                Disc = r.Discount / 100;
                                DiscountAmount = r.Rate * Disc * ActualSaleQty;
                            }
                            else
                            {
                                Disc = r.Discount / r.Quantity;
                                DiscountAmount = ActualSaleQty * Disc;
                            }
                        }
                        else
                        {
                            var anyqtyDisc = false;
                            var rrt = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId).Select(xx => xx.Discount).ToList();

                            foreach (var ch in rrt)
                            {
                                if (ch > 0)
                                {
                                    anyqtyDisc = true;
                                }
                            }
                            if (!anyqtyDisc)
                            {
                                if (invoice.DiscountType == 1)
                                {
                                    var sumtotqty = invoice.InvoiceItems.Sum(ik => ik.Quantity);
                                    Disc = invoice.TotalDiscount / rrt.Count();
                                    DiscountAmount = (Disc / r.Quantity) * ActualSaleQty;
                                }
                                else
                                {
                                    Disc = invoice.TotalDiscount / 100;
                                    DiscountAmount = r.Rate * Disc * ActualSaleQty;
                                }
                            }


                        }
                    }
                    //perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    perItemPaymentShare = (r.Rate);

                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += (ActualSaleQty * perItemPaymentShare) - DiscountAmount;

                }

                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }

            ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            v.HasNextPage = x.HasNextPage;
            v.HasPreviousPage = x.HasPreviousPage;
            v.IsFirstPage = x.IsFirstPage;
            v.IsLastPage = x.IsLastPage;
            v.PageCount = x.PageCount;
            v.ItemWiseRevenue = lst;
            return v;
        }
        public ItemWiseRevenueMasterVM GetItemWiseRevenue_Date(int pageNo, int pageSize, DateTime Datefrom, DateTime Dateto)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice
                     && ii.IsActive
                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(Datefrom)
                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                     )
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToPagedList(pageNo, pageSize);

            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int batchid = 0;
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx=>xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {
                            
                            batchid = b;
                            goto l;
                        }
                    }
                    l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    //var tt = r.NetAmount;
                    //perItemPaymentShare = ( tt / r.Quantity);
                    double Disc = 0;
                    double DiscountAmount = 0;
                    if (invoice.TotalDiscount > 0)
                    { 
                        if (r.Discount > 0)
                        {
                            if (r.DiscountType == 1)
                            {
                                Disc = r.Discount / 100;
                                DiscountAmount = r.Rate * Disc * ActualSaleQty;
                            }
                            else
                            {
                                Disc = r.Discount / r.Quantity;
                                DiscountAmount = ActualSaleQty * Disc ;
                            }
                        }
                        else
                        {
                            var anyqtyDisc = false;
                            var rrt = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId).Select(xx => xx.Discount).ToList();

                            foreach (var ch in rrt)
                            {
                                if (ch > 0)
                                {
                                    anyqtyDisc = true;
                                }
                            }
                            if (!anyqtyDisc)
                            {
                                if (invoice.DiscountType == 1)
                                {
                                    var sumtotqty = invoice.InvoiceItems.Sum(ik => ik.Quantity);
                                    Disc = invoice.TotalDiscount / rrt.Count();
                                    DiscountAmount = (Disc/r.Quantity)*ActualSaleQty;
                                }
                                else
                                {
                                    Disc = invoice.TotalDiscount / 100;
                                    DiscountAmount = r.Rate * Disc * ActualSaleQty;
                                }
                            }
                            

                        }
                    }
                    
                    //perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    perItemPaymentShare = (r.Rate );
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += (ActualSaleQty * perItemPaymentShare)-DiscountAmount;
                }

                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }

            ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            v.HasNextPage = x.HasNextPage;
            v.HasPreviousPage = x.HasPreviousPage;
            v.IsFirstPage = x.IsFirstPage;
            v.IsLastPage = x.IsLastPage;
            v.PageCount = x.PageCount;
            v.ItemWiseRevenue = lst;
            return v;
        }


        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive
                                              && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                              && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                              //orderby i.InvoiceId descending
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }


        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive && ii.Item.ItemID == ItemId
                                              && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                              && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }



        public List<ItemWisePurchaseVM> GetItemWisePurchase_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWisePurchaseVM> PurchaseList = (from i in cxt.Stocks
                                                     join ii in cxt.StockItems on i equals ii.Stock
                                                     where i.IsActive && ii.IsActive
                                                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                     //orderby i.InvoiceId descending
                                                     group ii by ii.Item.ItemName into ItemsG
                                                     select new ItemWisePurchaseVM
                                                     {
                                                         ItemName = ItemsG.Key,
                                                         PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                     }).OrderBy(i => i.ItemName).ToList();
            return PurchaseList;
        }

        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt1(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && !i.IsQInvoice && ii.IsActive && ii.ReturnedQuantity != ii.Quantity
                                              && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                              && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                              //orderby i.InvoiceId descending
                                              group ii by ii.Item.ItemID into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemId = ItemsG.Key,
                                                  ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                                  TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                  ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                  HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                  ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                 -
                                                                 (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                                              }).OrderBy(i => i.ItemName).ToList();

            // just filling available stock variable by looping above retrived results
            foreach (ItemWiseSaleVM i in SalesList)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }
            return SalesList;
        }
        //this function is duplicate of above function
        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt1(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && !i.IsQInvoice && ii.IsActive && ii.Item.ItemID == ItemId && ii.ReturnedQuantity != ii.Quantity
                                              && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                              && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                              group ii by ii.Item.ItemID into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemId = ItemsG.Key,
                                                  ItemName = (from item in cxt.Items where item.ItemID == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                                  TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                                  ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                  HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                                  ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                                 -
                                                                 (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == ItemsG.Key && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                                  AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                                              }).OrderBy(i => i.ItemName).ToList();
            // just filling available stock variable by looping above retrived results
            foreach (ItemWiseSaleVM i in SalesList)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }
            return SalesList;
        }

        public List<ItemWisePurchaseVM> GetItemWisePurchase_Rpt(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<ItemWisePurchaseVM> PurchaseList = (from i in cxt.Stocks
                                                     join ii in cxt.StockItems on i equals ii.Stock
                                                     where i.IsActive && ii.IsActive && ii.Item.ItemID == ItemId
                                                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                     //orderby i.InvoiceId descending
                                                     group ii by ii.Item.ItemName into ItemsG
                                                     select new ItemWisePurchaseVM
                                                     {
                                                         ItemName = ItemsG.Key,
                                                         PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                     }).OrderBy(i => i.ItemName).ToList();
            return PurchaseList;
        }

        public List<ItemWiseSaleVM> GetItemWiseRevenue_Rpt(int ItemId)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive && ii.Item.ItemID == ItemId
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleRevenue = ItemsG.Sum(i => (double?)i.Amount) ?? 0,
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }
        public List<ItemWiseSaleVM> GetItemWiseRevenue_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive
                                              && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                              && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                              //orderby i.InvoiceId descending
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleRevenue = ItemsG.Sum(i => (double?)i.Amount) ?? 0,
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }

        public void SetItemDataInactive(int ItemId)
        {
            //            string qry = @"Update Items set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdateDate = @UpdateDate where ItemId = @ItemId 
            //                            Update StockItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdateDate where Item_ItemId = @ItemId 
            //                            Update AdjustmentItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdateDate = @UpdateDate where Item_ItemId = @ItemId";

            string qry = @"Update Items set IsActive = 0, IsSynced = 0, DeletedAt =@DeletedAt  where ItemId = @ItemId 
                             Update StockItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, DeletedAt =@DeletedAt where Item_ItemId = @ItemId 
                             Update AdjustmentItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, DeletedAt =@DeletedAt where Item_ItemId = @ItemId";

            cxt.Database.ExecuteSqlCommand(qry, new SqlParameter("@ItemId", ItemId), new SqlParameter("@DeletedAt", DateTime.Now));
        }

        public List<Stock> IsItemHaveOneStock(int ItemId)
        {
            List<Stock> Stocks = new List<Stock>();
            string qry = @"select * from Stocks where StockId IN (select Stock_StockId from StockItems where Item_ItemID = @ItemId)";
            return Stocks = cxt.Database.SqlQuery<Stock>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

        }


        public IPagedList<DeadStockVM> GetDeadStock(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(i => new DeadStockVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0)
                                        - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0

                                        //total expired stock expired stock - expired stock consumption
                                        - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
                                        - (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0

                                        + (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    LastSalesAt = (from c in cxt.StockConsumptionItems where c.IsActive && c.Item.ItemID == i.ItemID && c.Quantity > 0 && DbFunctions.TruncateTime(c.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(c.CreateDate) <= DbFunctions.TruncateTime(DateTo) orderby c.StockConsumptionItemId descending select c.CreateDate == null ? "" : c.CreateDate.ToString()).FirstOrDefault()
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            // commented now
            //int[] LastMonthSaleItemIds = cxt.invoiceItems
            //                             .Where(i => i.IsActive
            //                                 && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
            //                                 ).Select(i => i.Item.ItemID).ToArray();

            // get * from items id not in sold items

            // commented now
            //List<Item> DeadStockItems = cxt.Items.Where(r => !r.IsDefault && !LastMonthSaleItemIds.Contains(r.ItemID)).Select(x => x).ToList();

            //LastMonthSaleItemIds.Contains(cxt.Items.Select(i=>i.ItemID))
            //return DeadStockItems
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemID,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemID == i.ItemID && inv.Quantity > 0 && DbFunctions.TruncateTime(inv.CreateDate) < DbFunctions.TruncateTime(DateFrom) orderby inv.CreateDate descending select inv.CreateDate == null ?  "" : inv.CreateDate.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            //IPagedList<DeadStockVM> Result = DeadStockItems.Skip((PageNo - 1) * PageSize).Take(PageNo * PageSize)
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemID,
            //        ItemName = i.ItemName,
            //        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
            //        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
            //        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
            //        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemID == i.ItemID && inv.Quantity > 0 && DbFunctions.TruncateTime(inv.CreateDate) < DbFunctions.TruncateTime(DateFrom) orderby inv.CreateDate descending select inv.CreateDate == null ? "" : inv.CreateDate.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            //int PageCount = Convert.ToInt32(Math.Ceiling((double)(DeadStockItems.Count / PageSize)));
            //foreach (DeadStockVM v in Result)
            //{
            //    v.AvailableQuantity = (int)v.TotalStock - (int)v.ExpiredStock + (int)v.AdjustedStock + (int)v.ConsumedStock;
            //}
            //return new Tuple<IPagedList<DeadStockVM>, int>(Result, PageCount);
        }
        public List<DeadStockVM> GetDeadStock_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            //return cxt.Items
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemID,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            + (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemID == i.ItemID && inv.Quantity > 0 && DbFunctions.TruncateTime(inv.CreateDate) < DbFunctions.TruncateTime(DateFrom) orderby inv.CreateDate descending select inv.CreateDate == null ? "" : inv.CreateDate.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToList();
            int[] LastMonthSaleItemIds = cxt.invoiceItems
                                         .Where(i => i.IsActive
                                             && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                             && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                             ).Select(i => i.Item.ItemID).ToArray();

            // get * from items id not in sold items
            List<Item> DeadStockItems = cxt.Items.Where(r => !r.IsDefault && !LastMonthSaleItemIds.Contains(r.ItemID)).Select(x => x).ToList();
            //LastMonthSaleItemIds.Contains(cxt.Items.Select(i=>i.ItemID))
            //return DeadStockItems
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemID,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemID == i.ItemID && inv.Quantity > 0 && DbFunctions.TruncateTime(inv.CreateDate) < DbFunctions.TruncateTime(DateFrom) orderby inv.CreateDate descending select inv.CreateDate == null ?  "" : inv.CreateDate.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            List<DeadStockVM> Result = DeadStockItems
                .Where(i => i.IsActive)
                .Select(i => new DeadStockVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemID == i.ItemID && inv.Quantity > 0 && DbFunctions.TruncateTime(inv.CreateDate) < DbFunctions.TruncateTime(DateFrom) orderby inv.CreateDate descending select inv.CreateDate == null ? "" : inv.CreateDate.ToString()).FirstOrDefault(),
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();

            foreach (DeadStockVM v in Result)
            {
                SharedFunctions.CalculateDeadStock(v);
                //v.AvailableQuantity = (int)v.TotalStock - (int)v.ExpiredStock + (int)v.AdjustedStock + (int)v.ConsumedStock;
            }
            return Result;
        }

        public IPagedList<ExpiryVM> GetItemsExpiry(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            IPagedList<ExpiryVM> ExpiredItems = (
             from s in cxt.StockItems
             join b in cxt.Batches on s.Batch equals b
             where s.IsActive
             && DbFunctions.TruncateTime(b.Expiry) >= DbFunctions.TruncateTime(DateFrom)
             && DbFunctions.TruncateTime(b.Expiry) <= DbFunctions.TruncateTime(DateTo)
             group s by
             new
             {
                 s.Item,
                 s.Batch
             } into g
             select new ExpiryVM
             {
                 ItemId = g.Key.Item.ItemID,
                 ItemName = g.Key.Item.ItemName,
                 BatchName = g.Key.Batch.BatchName,
                 Quantity = g.Sum(r => r.Quantity),
                 ExpiryDate = (DateTime)g.Key.Batch.Expiry,
                 StockAdditionDate = g.Where(r => r.Item == g.Key.Item && r.Batch == g.Key.Batch).Select(r => r.CreatedAt).FirstOrDefault(),
                 TotalCost = g.Sum(r => r.TotalCost),
                 TotalRetailValue = g.Sum(r => r.Quantity * r.RetailPrice)
             }).OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
            return ExpiredItems;
        }
        public List<ExpiryVM> GetItemsExpiry_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ExpiryVM> ExpiredItems = (
              from s in cxt.StockItems
              join b in cxt.Batches on s.Batch equals b
              where s.IsActive
              && DbFunctions.TruncateTime(b.Expiry) >= DbFunctions.TruncateTime(DateFrom)
              && DbFunctions.TruncateTime(b.Expiry) <= DbFunctions.TruncateTime(DateTo)
              group s by
              new
              {
                  s.Item,
                  s.Batch
              } into g
              select new ExpiryVM
              {
                  ItemId = g.Key.Item.ItemID,
                  ItemName = g.Key.Item.ItemName,
                  BatchName = g.Key.Batch.BatchName,
                  Quantity = g.Sum(r => r.Quantity),
                  ExpiryDate = (DateTime)g.Key.Batch.Expiry,
                  StockAdditionDate = g.Where(r => r.Item == g.Key.Item && r.Batch == g.Key.Batch).Select(r => r.CreatedAt).FirstOrDefault(),
                  TotalCost = g.Sum(r => r.TotalCost),
                  TotalRetailValue = g.Sum(r => r.Quantity * r.RetailPrice)
              }).OrderByDescending(r => r.ItemId).ToList();
            return ExpiredItems;
        }

        public long ItemsCount()
        {
            return cxt.Items.Count();
        }

        public void updatePrices(string qry)
        {
            cxt.Database.CommandTimeout = 2000;
            cxt.Database.ExecuteSqlCommand(qry);
        }

        public int GetLowStockItemsCount()
        {
            return (from i in cxt.Items
                    where i.IsActive
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        //ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && b_.Expiry != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        //LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
              .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) < r.ReOrderingLevel)
              .Count();
        }

        public int GetOutOfStockItemsCount()
        {
            //List<LowStockVM> v = 
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && b_.Expiry != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        //LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                            .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= 0)
                            .Count();
        }

        public int GetExpiredItemsCount(DateTime? date)
        {
            int? ExpiredStock = 0;
            if (date != null)
            {
                ExpiredStock = (from es in cxt.StockItems
                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                join i in cxt.Items on es.Item.ItemID equals i.ItemID
                                where i.IsActive && es.IsActive && b_.Expiry != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                select es).Sum(es => es.Quantity);
            }
            else
            {
                ExpiredStock = (from es in cxt.StockItems
                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                join i in cxt.Items on es.Item.ItemID equals i.ItemID
                                where i.IsActive && es.IsActive && b_.Expiry != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                select es).Sum(es => (int?)es.Quantity) ?? 0;
            }
            return (int)ExpiredStock;
        }

        public IPagedList<ItemsEditVM> GetItemsForBulkEdit(int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   //rf!@
                   //Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = (int?)vm.Category.CategoryId ?? 0,
                   CategoryName = vm.Category.Name,
                   Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public IPagedList<ItemsEditVM> GenericSearchForBulkEditItems(string filter, int CategoryId, int ManufId, int SupplierId, int PageNo, int PageSize)
        {
            IQueryable<Item> result;
            if (SupplierId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filter))
                result = result.Where(i => i.ItemName.ToLower().StartsWith(filter));
            if (CategoryId > 0)
                result = result.Where(i => i.Category.CategoryId == CategoryId);
            if (ManufId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManufId);





            IPagedList<ItemsEditVM> r = result.Select(vm => new ItemsEditVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    //rf!@
                    Rack = vm.Rack,
                    UnitCost = vm.UnitCostPrice,
                    RetailPrice = vm.RetailPrice,
                    ReorderingLevel = vm.ReOrderingLevel,
                    Barcode = vm.Barcode,
                    CategoryId = (int?)vm.Category.CategoryId ?? 0,
                    CategoryName = vm.Category.Name,
                    Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public List<int> GenericSearchForEditItems(string filter, int CategoryId, int ManufId, int SupplierId)
        {
            IQueryable<Item> result;
            if (SupplierId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filter))
                result = result.Where(i => i.ItemName.ToLower().StartsWith(filter));
            if (CategoryId > 0)
                result = result.Where(i => i.Category.CategoryId == CategoryId);
            if (ManufId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManufId);
            List<int> r = result.Select(vm => vm.ItemID).ToList();
            return r;
        }

        public IPagedList<ItemsEditVM> GetItemsForBulkEdit(string filter, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.ItemName.ToLower().StartsWith(filter))
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   //rf!@
                   //Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                   CategoryName = vm.Category != null ? vm.Category.Name : "",
                   Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Sup(int SupplierId, int PageNo, int PageSize)
        {

            try
            {
                var res = cxt.Suppliers
                       .Where(s => s.SupplierID == SupplierId).FirstOrDefault();

                IPagedList<ItemsEditVM> r = cxt.Entry(res)
                    .Collection(rs => rs.Items)
                    .Query()
                    .Where(i => i.IsActive)
                    .Select(vm => new ItemsEditVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        Rack = vm.Rack,
                        UnitCost = vm.UnitCostPrice,
                        RetailPrice = vm.RetailPrice,
                        ReorderingLevel = vm.ReOrderingLevel,
                        Barcode = vm.Barcode,
                        CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                        CategoryName = vm.Category != null ? vm.Category.Name : "",
                        Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
                    })
                    .OrderByDescending(i => i.ItemId)
                    .ToPagedList(PageNo, PageSize);
                return r;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Cat(int CategoryId, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.Category.CategoryId == CategoryId)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   //rf!@
                   //Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = (int)vm.Category.CategoryId,
                   CategoryName = vm.Category.Name,
                   Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Manuf(int ManufId, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.Manufacturer.ManufacturerId == ManufId)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   //rf!@
                   //Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                   CategoryName = vm.Category != null ? vm.Category.Name : "",
                   Suppliers = (vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name })).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public List<Item> getItemsByIds(List<int> ids)
        {
            return cxt.Items.Where(i => i.IsActive && ids.Contains(i.ItemID)).Include(x => x.User).ToList();
        }

        public List<Item> getItemsWithSupplierByIds(List<int> ids)
        {
            return cxt.Items.Where(i => i.IsActive && ids.Contains(i.ItemID)).Include(x => x.Suppliers).ToList();
        }

        public Item getItemWithSupplierById(int Id)
        {
            return cxt.Items.Where(i => i.IsActive && i.ItemID == Id).Include(x => x.Suppliers).Include(x => x.User).FirstOrDefault();
        }

        public ItemsDataVM getItemData(int itemId)
        {
            return cxt.Items.Where(i => i.ItemID == itemId)
                .Select(i => new ItemsDataVM()
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    RetailPrice = i.RetailPrice
                }).FirstOrDefault();
        }
        public bool ExistsAny()
        {
            return cxt.Items.Any(i => !i.IsDefault);
        }

        // profit margin UI data
        public IPagedList<ProfitMarginVM> GetProfitMargin(int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.ItemID == ItemId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == categoryId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }

        // profit margin Report data
        public List<ProfitMarginVM> GetProfitMargin_Rpt()
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_Item_Rpt(int ItemId)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.ItemID == ItemId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_cat_Rpt(int categoryId)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == categoryId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }


        // Discounted Items (selected items)
        public IPagedList<DiscountedItemVM> GetDiscountedItems(int DiscountId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_Item(int DiscountId, int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && i.ItemID == ItemId && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_cat(int DiscountId, int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && i.Category.CategoryId == categoryId && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return
                (from d in cxt.FlatDiscount
                 join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                 join i in cxt.Items on di.Item.ItemID equals i.ItemID
                 where (i.IsActive && di.IsActive && d.Discount > 0 && !d.IsAllItems
                 && DbFunctions.TruncateTime(d.StartDate) >= DbFunctions.TruncateTime(DateFrom)
                 && DbFunctions.TruncateTime(d.EndDate) <= DbFunctions.TruncateTime(DateTo))

                 select new DiscountedItemVM
                 {
                     ItemId = i.ItemID,
                     ItemName = i.ItemName,
                     CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                     RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                     DiscountName = d.Name,
                     DiscountType = d.DiscountType,
                     Discount = d.Discount
                 }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }


        // Discounted Items report data (selected Items)
        public List<DiscountedItemVM> GetDiscountedItems_Rpt(int DiscountId)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Item_Rpt(int DiscountId, int ItemId)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && i.ItemID == ItemId && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_cat_Rpt(int DiscountId, int categoryId)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0 && i.Category.CategoryId == categoryId && d.FlatDiscountId == DiscountId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return (from d in cxt.FlatDiscount
                    join di in cxt.DiscountItem on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemID equals i.ItemID
                    where (i.IsActive && di.IsActive && d.Discount > 0
                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                    )
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }


        // Discounted Items (All items)
        public IPagedList<DiscountedItemVM> GetDiscountedItems_all(int FaltDiscId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where (i.IsActive)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_Item_all(int FaltDiscId, int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where (i.IsActive && i.ItemID == ItemId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_cat_all(int FaltDiscId, int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where (i.IsActive && i.Category.CategoryId == categoryId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }

        // Discounted Items report data (All Items)
        public List<DiscountedItemVM> GetDiscountedItems_Rpt_all(int FaltDiscId)
        {
            return (from i in cxt.Items
                    where (i.IsActive)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Item_Rpt_all(int FaltDiscId, int ItemId)
        {

            return (from i in cxt.Items
                    where (i.IsActive && i.ItemID == ItemId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault(),

                    }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_cat_Rpt_all(int FaltDiscId, int categoryId)
        {
            return (from i in cxt.Items
                    where (i.IsActive && i.Category.CategoryId == categoryId)
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == i.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscount
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToList();
        }


        // Product Prices
        public IPagedList<ProductPriceVM> GetProductPrices(int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.ItemID == ItemId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.Category.CategoryId == categoryId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                )
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }


        // Product prices report data
        public List<ProductPriceVM> GetProductPrices_Rpt()
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_Item_Rpt(int ItemId)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.ItemID == ItemId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_cat_Rpt(int categoryId)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.Category.CategoryId == categoryId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemID,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Items
               .Where(i => i.IsActive && !i.IsDefault)
               .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
               )
               .Select(m => new ProductPriceVM
               {
                   ItemId = m.ItemID,
                   ItemName = m.ItemName,
                   Category = m.Category == null ? "" : m.Category.Name,
                   CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                   RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
               }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }



        // Product Expiry
        public IPagedList<ProductExpiryVM> GetProductExpiry(int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive && i.ItemID == ItemId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive && i.Category.CategoryId == categoryId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {

            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive &&
                        DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }


        //// Product Expiry report data
        //public List<ProductExpiryVM> GetProductExpiry_Rpt()
        //{
        //    return cxt.Items
        //        .Where(i => i.IsActive && !i.IsDefault)
        //        .Select(m => new ProductPriceVM
        //        {
        //            ItemId = m.ItemID,
        //            ItemName = m.ItemName,
        //            Category = m.Category == null ? "" : m.Category.Name,
        //            CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
        //            RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemID == m.ItemID orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
        //        }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        //}
        public List<ProductExpiryVM> GetProductExpiry_Item_Rpt(int ItemId)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive && i.ItemID == ItemId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public List<ProductExpiryVM> GetProductExpiry_cat_Rpt(int categoryId)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive && i.Category.CategoryId == categoryId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public List<ProductExpiryVM> GetProductExpiry_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       where i.IsActive && si.IsActive &&
                        DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemID,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.invoiceItems where ii.Item.ItemID == i.ItemID && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreateDate).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Batch.BatchId == si.Batch.BatchId && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }

        public StockItemDetailRunTimeVM GetItemDetail(int itemId)
        {
            return cxt.Items.Where(i => i.ItemID == itemId)
                .Select(i => new StockItemDetailRunTimeVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,

                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    CostPrice =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    TotalStock = (cxt.StockItems.Where(si => si.IsActive  && si.Item.ItemID == i.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
                                   -
                                   (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                }).FirstOrDefault();
        }

        private void GetAllActiveItemsWithRetailPrice()
        {
            cxt.Items.Where(i => i.IsActive)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    RetailPrice = (double?)(from si in cxt.StockItems where si.IsActive && si.Item.ItemID == i.ItemID orderby si.Item.ItemID descending select si.RetailPrice).FirstOrDefault() ?? 0

                }).ToList();
        }

        public List<ItemsVM> LoadLowStockItems(int SupplierId)
        {
            List<ItemsVM> res = (from i in cxt.Items
                                 where i.IsActive && !i.IsDefault && i.Suppliers.Any(s => s.SupplierID == SupplierId)
                                 select new ItemsVM()
                                 {
                                     ItemId = i.ItemID,
                                     Manufacturer = i.Manufacturer,
                                     ItemName = i.ItemName,
                                     ConversionUnit = i.ConversionUnit,
                                     Unit = i.Unit,
                                     ReorderingLevel = i.ReOrderingLevel,
                                     UnitCost = i.UnitCostPrice,
                                     TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                                     ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                     HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                                     ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                                    -
                                                    (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                                     AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                                 })
                .OrderByDescending(r => r.ItemId)
                .ToList();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSup_LowStockItems(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    Manufacturer = i.Manufacturer,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    ReorderingLevel = i.ReOrderingLevel,
                    UnitCost = i.UnitCostPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }

        public List<ItemsVM> LoadSupItems(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    Manufacturer = i.Manufacturer,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    //UnitCost = i.UnitCostPrice,
                    RetailPrice =
                    (
                    (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                    i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),

                    ReorderingLevel = i.ReOrderingLevel,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();



            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSupItems(int SupplierId, List<int> ItemsIds)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    UnitCost = i.UnitCostPrice,
                    ReorderingLevel = i.ReOrderingLevel,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();



            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }

        public List<ItemsVM> GetMissedSalesItems(List<int> Ids)
        {
            List<ItemsVM> res = cxt.Items
                .Where(i => i.IsActive && !i.IsDefault && Ids.Contains(i.ItemID))
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    ReorderingLevel = i.ReOrderingLevel,
                    //UnitCost = i.UnitCostPrice,
                    RetailPrice =
                   (
                   (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() > 0
                   ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() :
                   i.RetailPrice),
                    UnitCost =
                    (
                     (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() > 0
                    ? (from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() :
                    i.UnitCostPrice),
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0
                })
                .OrderByDescending(r => r.ItemId)
                .ToList();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSup_LowStItemIDs(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemID,
                }).ToList())
                .FirstOrDefault();
            return res;
        }

        public string GetItemBarcode(int ItemID)
        {
            return cxt.Items.Where(i => i.ItemID == ItemID).Select(i => i.Barcode).FirstOrDefault().ToString();
        }

        public IPagedList<LowStockNotiVM> GetLowStockNotis(int pageNo, int pageSize)
        {
            IPagedList<LowStockNotiVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new LowStockNotiVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
               })
               .OrderByDescending(i => i.ItemId)
               .ToPagedList(pageNo, pageSize);

            // applied outer condition for performance sake..
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock - i.HoldStock;
                }
            }
            else
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock;
                }
            }
            return r;
        }
        public List<LowStockNotiVM> GetLowStockNotisList()
        {
            List<LowStockNotiVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new LowStockNotiVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
               })
               .OrderByDescending(i => i.ItemId)
               .ToList();

            // applied outer condition for performance sake..
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock - i.HoldStock;
                }
            }
            else
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock;
                }
            }
            return r;
        }
        public int GetLowStockNotiscount()
        {
            List<LowStockNotiVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new LowStockNotiVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
               })
               .OrderByDescending(i => i.ItemId)
               .ToList();

            // applied outer condition for performance sake..
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock - i.HoldStock;
                }
            }
            else
            {
                foreach (var i in r)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock;
                }
            }
            r = r.Where(i => i.AvailableStock < i.ReorderingLevel && i.TotalStock >0).ToList();
            return r.Count;
        }
        public int GetExpiryNotiscount(DateTime ExpNotiPeriod)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where i.IsActive && si.IsActive && b.Expiry != null
                       &&
                       (
                            (from es in cxt.StockItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                            -
                            (from es in cxt.StockConsumptionItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                       )
                       > 0
                       group si by new { i, b } into g
                       select new ExpiryNotiVM
                       {
                           ItemId = g.Key.i.ItemID,
                           ItemName = g.Key.i.ItemName,
                           BatchNo = g.Key.b.BatchName,
                           BatchExp = g.Key.b.Expiry.Value,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
            return res.Count;
        }
        public IPagedList<ExpiryNotiVM> GetExpiryNotis(DateTime ExpNotiPeriod, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where i.IsActive && si.IsActive && b.Expiry != null
                       &&
                       (
                            (from es in cxt.StockItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                            -
                            (from es in cxt.StockConsumptionItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                       )
                       > 0
                       group si by new { i, b } into g
                       select new ExpiryNotiVM
                       {
                           ItemId = g.Key.i.ItemID,
                           ItemName = g.Key.i.ItemName,
                           BatchNo = g.Key.b.BatchName,
                           BatchExp = g.Key.b.Expiry.Value,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
            return res;
        }
        public List<ExpiryNotiVM> GetExpiryNotisList(DateTime ExpNotiPeriod)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where i.IsActive && si.IsActive && b.Expiry != null
                       &&
                       (
                            (from es in cxt.StockItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                            -
                            (from es in cxt.StockConsumptionItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                       )
                       > 0
                       group si by new { i, b } into g
                       select new ExpiryNotiVM
                       {
                           ItemId = g.Key.i.ItemID,
                           ItemName = g.Key.i.ItemName,
                           BatchNo = g.Key.b.BatchName,
                           BatchExp = g.Key.b.Expiry.Value,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
            return res;
        }
        public bool GetExpiryPOSNotis(DateTime ExpNotiPeriod, int ItemId, int BatchId)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemID equals si.Item.ItemID
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where i.IsActive && si.IsActive && si.Item.ItemID == ItemId && b.BatchId == BatchId &&  b.Expiry != null
                       &&
                       (
                            (from es in cxt.StockItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                            -
                            (from es in cxt.StockConsumptionItems where es.Item.ItemID == i.ItemID && es.Batch.BatchId == b.BatchId && DbFunctions.TruncateTime(b.Expiry) > DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(ExpNotiPeriod) select es).Sum(es => (int?)es.Quantity) ?? 0
                       )
                       > 0
                       group si by new { i, b } into g
                       select new ExpiryNotiVM
                       {
                           ItemId = g.Key.i.ItemID,
                       }).OrderByDescending(r => r.ItemId).Count() > 0 ? true : false;
            return res;
        }



        public IPagedList<BulkItemsVM> GetActiveFilterItems
        (
            string SearchString,
            int RackId,
            int ManufacturerId,
            int SupplierId,
            int StoreId,
            int PageNo,
            int PageSize
        )
        {
            IPagedList<BulkItemsVM> list = null;
            IQueryable<Item> result;
            if (SupplierId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }
            if (RackId > 0)
                result = result.Join(cxt.RackItems, r => r.ItemID, ri => ri.Item.ItemID, (item, rack) => new { item, rack }).Where(x => x.rack.Rack.RackId == RackId && x.rack.IsActive).Select(s => s.item);
            if (ManufacturerId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManufacturerId);
            if (!string.IsNullOrEmpty(SearchString))
                result = result.Where(i =>
                       i.ItemName.ToLower().StartsWith(SearchString) ||
                       i.ChecmicalName.ToLower().StartsWith(SearchString) ||
                       i.Barcode.ToLower().Equals(SearchString));

            if (StoreId == 0)
            {
                #region WithoutStore
                list = result.Select(i => new BulkItemsVM
                {
                    ItemId = i.ItemID,
                    ItemName = i.ItemName,
                    Barcode = i.Barcode,
                    GenericName = i.ChecmicalName,
                    Unit = i.Unit,
                    Manufacturer = i.Manufacturer.Name,
                    SupplierNames = i.Suppliers.Select(x => x.Name).ToList(),
                    RackName = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == i.ItemID select ri.Rack.Name).FirstOrDefault(),
                    RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                    CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0,
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID && si.Store == null).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Store == null && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Store == null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join iv in cxt.Invoices on cs.InvoiceId equals iv.InvoiceId where iv.IsHoldInvoice && cs.IsActive && cs.Store == null && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).OrderByDescending(k => k.ItemId)
                .ToPagedList(PageNo, PageSize);
                #endregion

            }
            else if (StoreId > 0)
            {
                #region WithStore
                List<int> ItemId = (from si in cxt.StockItems join s in cxt.Stores on si.Store.StoreId equals s.StoreId where s.StoreId == StoreId && s.IsActive select si.Item.ItemID).ToList();
                if (ItemId != null && ItemId.Count() > 0)
                {
                    list = result.Where(i => ItemId.Contains(i.ItemID)).Select(i => new BulkItemsVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        GenericName = i.ChecmicalName,
                        Manufacturer = i.Manufacturer.Name,
                        SupplierNames = i.Suppliers.Select(x => x.Name).ToList(),
                        RackName = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == i.ItemID select ri.Rack.Name).FirstOrDefault(),
                        RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                        CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0,
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID && si.Store != null).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID && s.Store != null select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && s.Store != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Store != null && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Store != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join iv in cxt.Invoices on cs.InvoiceId equals iv.InvoiceId where iv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Store != null select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    }).OrderByDescending(r => r.ItemId)
                    .ToPagedList(PageNo, PageSize);
                }
                #endregion
            }
            return list;
        }

        public IPagedList<BulkItemsVM> GetActiveFilterItems
        (
            string SearchString,
            int SelectedIndex,
            int RackId,
            int ManufacturerId,
            int SupplierId,
            int StoreId,
            int PageNo,
            int PageSize,
            bool checkQty
        )
        {
            IPagedList<BulkItemsVM> list = null;
            IQueryable<Item> result;
            if (SelectedIndex == 5)
            {
                Supplier res = cxt.Suppliers.Where(s => s.Name.ToLower().Contains(SearchString)).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }
            switch (SelectedIndex)
            {
                case 0: //Name
                    result = result.Where(i => i.ItemName.ToLower().StartsWith(SearchString));
                    break;
                case 1: //Generic Name
                    result = result.Where(i => i.ChecmicalName.ToLower().StartsWith(SearchString));
                    break;
                case 2: //Barcode
                    result = result.Where(i => i.Barcode.ToLower().StartsWith(SearchString));
                    break;
                case 3: //Manufacturers
                    result = result.Where(i => i.Manufacturer.Name.ToLower().Contains(SearchString));
                    break;
                case 4: //Rack
                    result = result.Join(cxt.RackItems, r => r.ItemID, ri => ri.Item.ItemID, (item, rack) => new { item, rack }).Where(x => x.rack.Rack.RackId == RackId && x.rack.IsActive).Select(s => s.item);
                    break;
                default:
                    break;
            }
            if (StoreId == 0)
            {
                if (checkQty == true)
                {
                    #region WithoutStore
                    list = result.Select(i => new BulkItemsVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Barcode = i.Barcode,
                        GenericName = i.ChecmicalName,
                        Unit = i.Unit,
                        Manufacturer = i.Manufacturer.Name,
                        SupplierNames = i.Suppliers.Select(x => x.Name).ToList(),
                        ConversionUnit = i.ConversionUnit,
                        //RackName = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == i.ItemID select ri.Rack.Name).FirstOrDefault(),
                        RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                        CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0,
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID && si.Store == null).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Store == null && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Store == null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join iv in cxt.Invoices on cs.InvoiceId equals iv.InvoiceId where iv.IsHoldInvoice && cs.IsActive && cs.Store == null && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    })
                    .Where(j => j.TotalStock > 0)
                    .Where(k => k.ConsumedStock != k.TotalStock)
                    .Where(m => m.AdjustedStock != m.TotalStock)
                    .OrderByDescending(k => k.ItemId)
                    .ToPagedList(PageNo, PageSize);
                    #endregion
                }
                else
                {
                    #region WithoutStore
                    list = result.Select(i => new BulkItemsVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Barcode = i.Barcode,
                        GenericName = i.ChecmicalName,
                        Unit = i.Unit,
                        Manufacturer = i.Manufacturer.Name,
                        SupplierNames = i.Suppliers.Select(x => x.Name).ToList(),
                        ConversionUnit = i.ConversionUnit,
                        //RackName = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == i.ItemID select ri.Rack.Name).FirstOrDefault(),
                        RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                        CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0,
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID && si.Store == null).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && s.Store == null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Store == null && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Store == null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join iv in cxt.Invoices on cs.InvoiceId equals iv.InvoiceId where iv.IsHoldInvoice && cs.IsActive && cs.Store == null && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    })
                    .OrderByDescending(k => k.ItemId)
                    .ToPagedList(PageNo, PageSize);
                    #endregion
                }
            }
            else if (StoreId > 0)
            {
                #region WithStore
                List<int> ItemId = (from si in cxt.StockItems join s in cxt.Stores on si.Store.StoreId equals s.StoreId where s.StoreId == StoreId && s.IsActive select si.Item.ItemID).ToList();
                if (ItemId != null && ItemId.Count() > 0)
                {
                    list = result.Where(i => ItemId.Contains(i.ItemID)).Select(i => new BulkItemsVM
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        GenericName = i.ChecmicalName,
                        Manufacturer = i.Manufacturer.Name,
                        SupplierNames = i.Suppliers.Select(x => x.Name).ToList(),
                        //RackName = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == i.ItemID select ri.Rack.Name).FirstOrDefault(),
                        RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                        CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemID == i.ItemID && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0,
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.ItemID && si.Store != null).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.ItemID && s.Store != null select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.ItemID && s.Store != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Store != null && ai.Item.ItemID == i.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && es.Store != null && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join iv in cxt.Invoices on cs.InvoiceId equals iv.InvoiceId where iv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID && cs.Store != null select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    }).OrderByDescending(r => r.ItemId)
                    .ToPagedList(PageNo, PageSize);
                }
                #endregion
            }
            return list;
        }

        #region ItemWiseRevenueReportFilter
        public List<ItemWiseRevenueVM> GetItemWiseRevenue_Category_Rpt(int CategoryId)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice && ii.IsActive && ii.Item.Category.CategoryId == CategoryId)
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToList();
            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int soldqty = 0;
            int batchid = 0;
            ItemsVM vm3 = new ItemsVM();
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += ActualSaleQty * perItemPaymentShare;
                }
                using (unitOfWork = new UnitOfWork())
                {



                    using (unitOfWork = new UnitOfWork())
                    {
                        vm3 = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.Key.ItemID);
                    }

                    soldqty = vm3.ConsumedStock;
                }
                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue,
                    SoldQuantity = soldqty
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }
            return lst;
        }

        public List<ItemWiseRevenueVM> GetItemWiseRevenue_Item_Rpt(int ItemId)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice && ii.IsActive && ii.Item.ItemID == ItemId)
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToList();
            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int soldqty = 0;
            int batchid = 0;
            ItemsVM vm3 = new ItemsVM();
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += ActualSaleQty * perItemPaymentShare;
                }
                using (unitOfWork = new UnitOfWork())
                {



                    using (unitOfWork = new UnitOfWork())
                    {
                        vm3 = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.Key.ItemID);
                    }

                    soldqty = vm3.ConsumedStock;
                }
                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue,
                    SoldQuantity = soldqty
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }
            return lst;
        }

        public List<ItemWiseRevenueVM> GetItemWiseRevenue_Date_Rpt(DateTime Datefrom, DateTime Dateto)
        {
            var x = (from i in cxt.Invoices
                     join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                     //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
                     where (i.IsActive && !i.IsQInvoice
                     && ii.IsActive
                     && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(Datefrom)
                     && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                     )
                     group i by ii.Item
                 ).OrderByDescending(r => r.Key.ItemID).ToList();

            List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            double revenue = 0;
            double perItemPaymentShare = 0;
            double ActualSaleQty = 0;
            int batchid = 0;
            int soldqty = 0;
            ItemsVM vm3 = new ItemsVM();
            List<int> ids = new List<int>();
            foreach (IGrouping<Item, Invoice> i in x)
            {
                revenue = 0;
                foreach (Invoice t in i)
                {
                    var rr = cxt.invoiceItems.Where(inv => inv.IsActive && !inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID).Select(xx => xx.Batch.BatchId).ToList();
                    foreach (var b in rr)
                    {
                        if (b != batchid)
                        {

                            batchid = b;
                            goto l;
                        }
                    }
                l:
                    var r = cxt.invoiceItems.Where(inv => inv.IsActive && inv.IsQInvoice && inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemID == i.Key.ItemID && inv.Batch.BatchId == batchid).FirstOrDefault();
                    //double ActualPaid = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault().InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId && !inv.IsQInvoice).FirstOrDefault();//Include(inv=>inv.InvoicePayments).Where(inv=>inv.InvoicePayments.Where()).FirstOrDefault();//.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment);
                    double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && !p.IsQInvoice && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
                    //cxt.Invoices.Where(inv=>inv.InvoiceId == t.InvoiceId ).Include(inv=>inv.InvoiceItems).Where(ii=>ii.InvoiceItems.Where(it=>it.Item.ItemID == i.Key.ItemID))
                    ActualSaleQty = r.Quantity - r.ReturnedQuantity;
                    //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
                    perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
                    if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
                    revenue += ActualSaleQty * perItemPaymentShare;
                }
                using (unitOfWork = new UnitOfWork())
                {



                    using (unitOfWork = new UnitOfWork())
                    {
                        vm3 = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.Key.ItemID);
                    }

                    soldqty = vm3.ConsumedStock;
                }
                ItemWiseRevenueVM vm = new ItemWiseRevenueVM
                {
                    ItemId = i.Key.ItemID,
                    ItemName = i.Key.ItemName,
                    Revenue = revenue,
                    SoldQuantity = soldqty
                };
                lst.Add(vm);
                ids.Add(i.Key.ItemID);
            }

            var res = cxt.Items.Where(i => ids.Contains(i.ItemID))
                .Select(i => new AvailableQtyVM
                {
                    ItemId = i.ItemID,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (int?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.ItemID select new { Qty = cs.Quantity }).Sum(cs => (int?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (int?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (int?)a.Qty) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                      (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && !ii.IsQInvoice && iv.IsActive && ii.Item.ItemID == i.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                }).ToList();

            foreach (var obj in lst)
            {
                AvailableQtyVM objQty = res.Where(vm => vm.ItemId == obj.ItemId).FirstOrDefault();
                if (obj != null)
                {
                    SharedFunctions.CalculateItemwiseReport(obj, objQty);
                    //if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                    //else
                    //{
                    //    obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.HoldStock;
                    //}
                }
            }
            return lst;
        }
        #endregion

        #region LowStockItemReport
        public IPagedList<LowStockVM> GetLowStockItems(int PageNo, int PageSize)
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,

                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
              .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
              .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<LowStockVM> GetLowStockItems_BySupplier(long SupplierId, int PageNo, int PageSize)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.Suppliers.Any(s => s.SupplierID == SupplierId)
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<LowStockVM> GetLowStockItems_ByCategory(long CategoryId, int PageNo, int PageSize)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.Category.CategoryId == CategoryId
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<LowStockVM> GenericSearchForLowStockItem(long CategoryId, long SupplierId, int PageNo, int PageSize)
        {
            IQueryable<Item> query;
            if (SupplierId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();
                query = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                query = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault).AsQueryable();
            }
            if (CategoryId > 0)
                query = query.Where(i => i.Category.CategoryId == CategoryId);
            IPagedList<LowStockVM> result = query.Select(i => new LowStockVM
            {
                ItemId = i.ItemID,
                ItemName = i.ItemName,
                Category = i.Category.Name,
                ReOrderingLevel = i.ReOrderingLevel,
                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select (DateTime?)ts.CreatedAt).FirstOrDefault() ?? null
            })
              .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
              .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
            return result;
        }

        public List<LowStockVM> GetLowStockItems_Rpt()
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToList();
        }
        public List<LowStockVM> GetLowStockItems_BySupplier_Rpt(long SupplierId)
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault && i.Suppliers.Any(s => s.SupplierID == SupplierId)
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToList();
        }
        public List<LowStockVM> GetLowStockItems_ByCategory_Rpt(long CategoryId)
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault && i.Category.CategoryId == CategoryId
                    select new LowStockVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName,
                        Category = i.Category.Name,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToList();
        }
        public List<LowStockVM> GenericSearchForLowStockItem_Rpt(long CategoryId, long SupplierId)
        {
            IQueryable<Item> query;
            if (SupplierId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();
                query = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                query = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault).AsQueryable();
            }
            if (CategoryId > 0)
                query = query.Where(i => i.Category.CategoryId == CategoryId);
            List<LowStockVM> result = query.Select(i => new LowStockVM
            {
                ItemId = i.ItemID,
                ItemName = i.ItemName,
                Category = i.Category.Name,
                ReOrderingLevel = i.ReOrderingLevel,
                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID select ts).Sum(ts => (int?)ts.Quantity) ?? 0,
                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemID == i.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemID == i.ItemID && adj_item.IsActive select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
                LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemID == i.ItemID orderby ts.StockItemId descending select (DateTime?)ts.CreatedAt).FirstOrDefault() ?? null
            })
              .Where(r => (r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock) <= r.ReOrderingLevel)
              .OrderByDescending(r => r.ItemId).ToList();
            return result;
        }

        #endregion

        #region frmAllItems
        public IPagedList<ItemsVM> GenericSearchForAllItems(
            string FilterByName,
            int RackId,
            int ManunfId,
            int SuppId,
            int CatId,
            int StoreId,
            int StockWiseItems,
            DateTime? CreatedDate,
            int PageNo,
            int PageSize,
            bool IsHoldingStock)
        {
            IQueryable<Item> result;
            IPagedList<ItemsVM> r = null;
            if (SuppId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SuppId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }
            if (!string.IsNullOrEmpty(FilterByName))
                result = result.Where(i => i.ItemName.ToLower().StartsWith(FilterByName));
            if (CatId > 0)
                result = result.Where(i => i.Category.CategoryId == CatId);
            if (RackId > 0)
                result = result.Where(i => i.Rack.RackId == RackId);
            if (ManunfId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManunfId);
            if (CreatedDate.HasValue)
                result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(CreatedDate));
            if (StoreId > 0)
            {
                result = result.Join(cxt.StockItems, i => i.ItemID, si => si.Item.ItemID, (i, si) => new
                {
                    it = i,
                    sti = si
                }).Where(x => x.sti.Store.StoreId == StoreId).Select(x => x.it).Distinct();
            }

            if (StoreId > 0)
            {
                #region WithStore
                if (IsHoldingStock)
                {
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        Barcode = vm.Barcode,
                        Unit = vm.Unit,
                        PCTCode = vm.PCTCode,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        ConversionUnit = vm.ConversionUnit,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive 
                                   && si.Item.ItemID == vm.ItemID 
                                   && si.Store != null && si.Store.IsActive 
                                   && si.Store.StoreId == StoreId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems 
                                         where s.IsActive && s.Item.ItemID == vm.ItemID 
                                         && s.Store != null && s.Store.IsActive
                                         && s.Store.StoreId == StoreId select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems
                                        join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                        where s.IsActive && s.Item.ItemID == vm.ItemID && s.Store != null && s.Store.IsActive && s.Store.StoreId == StoreId &&
                                        DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                        select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems
                                         join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                         where ai.IsActive && ai.Item.ItemID == vm.ItemID && ai.Store != null && ai.Store.IsActive && ai.Store.StoreId == StoreId
                                         select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems
                                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                                select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems
                                     join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                     where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID
                                     && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                     select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
                  .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
                  .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) > 0)
                  .OrderByDescending(i => i.ItemId)
                  .ToPagedList(PageNo, PageSize);
                }
                else
                {
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        Barcode = vm.Barcode,
                        PCTCode = vm.PCTCode,
                        Unit = vm.Unit,
                        ConversionUnit = vm.ConversionUnit,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID && si.Store != null && si.Store.IsActive && si.Store.StoreId == StoreId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID && s.Store != null && s.Store.IsActive && s.Store.StoreId == StoreId select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems
                                        join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                        where s.IsActive && s.Item.ItemID == vm.ItemID && s.Store != null && s.Store.IsActive && s.Store.StoreId == StoreId &&
                                        DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                        select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems
                                         join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                         where ai.IsActive && ai.Item.ItemID == vm.ItemID && ai.Store != null && ai.Store.IsActive && ai.Store.StoreId == StoreId
                                         select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems
                                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                && es.Store != null && es.Store.IsActive && es.Store.StoreId == StoreId
                                                select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems
                                     join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                     where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID
                                     && cs.Store != null && cs.Store.IsActive && cs.Store.StoreId == StoreId
                                     select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
                  .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
                  .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) > 0)
                  .OrderByDescending(i => i.ItemId)
                  .ToPagedList(PageNo, PageSize);
                }
                #endregion
            }
            else
            {
                #region WithoutStore
                if (IsHoldingStock)
                {
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        PCTCode = vm.PCTCode,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        Barcode = vm.Barcode,
                        Unit = vm.Unit,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        NoOfStrips = vm.NoOfStrips,
                        ConversionUnit = vm.ConversionUnit,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID  select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
                  .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
                  .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) > 0)
                  .OrderByDescending(i => i.ItemId)
                  .ToPagedList(PageNo, PageSize);
                }
                else
                {
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        PCTCode = vm.PCTCode,
                        Barcode = vm.Barcode,
                        Unit = vm.Unit,
                        ConversionUnit = vm.ConversionUnit,
                        NoOfStrips = vm.NoOfStrips,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
                  .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
                  .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) > 0)
                  .OrderByDescending(i => i.ItemId)
                  .ToPagedList(PageNo, PageSize);
                }
                #endregion
            }
            return r;
        }
        public List<ItemsVM> GenericSearchForAllItems1(int RackId,
            int ManunfId,
            int SuppId,
            DateTime dtfrom,
            DateTime dtto,
            string Item)
        {
            IQueryable<Item> result;
            List<ItemsVM> r = null;
            if (SuppId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SuppId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
                    
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }
            
            if (RackId > 0)
                result = result.Where(i => i.Rack.RackId == RackId);
            if (ManunfId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManunfId);
            if (Item != null) 
            {
                result = result.Where(i => i.ItemName.Contains(Item));   
            }

            //if (dtfrom.HasValue && dtto.HasValue)
                //result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(dtfrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(dtto));

            if (true)
            {
                
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        Barcode = vm.Barcode,
                        PCTCode = vm.PCTCode,
                        Unit = vm.Unit,
                        chemicalname = vm.ChecmicalName,
                        ConversionUnit = vm.ConversionUnit,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        NoOfStrips = vm.NoOfStrips,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID && si.CreatedAt >= dtfrom && si.CreatedAt <= dtto).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID && s.CreateDate >=dtfrom && s.CreateDate <= dtto select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems
                                        join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                        where s.IsActive && s.Item.ItemID == vm.ItemID  &&
                                        DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) && b.CreatedAt >= dtfrom && b.CreatedAt <= dtto
                                        select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems
                                         join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                         where ai.IsActive && ai.Item.ItemID == vm.ItemID && ai.CreateDate >= dtfrom && ai.CreateDate <= dtto
                                         select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems
                                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                                where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                                && es.Store != null && es.Store.IsActive
                                                select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.CreateDate >= dtfrom && sr.CreateDate <= dtto select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems
                                     join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                     where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID
                                     select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  
                  .OrderByDescending(i => i.ItemId)
                  .ToList();
                
             
            }
            else
            {
                
                
                    r = result.Select(vm => new ItemsVM
                    {
                        ItemId = vm.ItemID,
                        ItemName = vm.ItemName,
                        RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Manufacturer = vm.Manufacturer,
                        PCTCode = vm.PCTCode,
                        Barcode = vm.Barcode,
                        Unit = vm.Unit,
                        ConversionUnit = vm.ConversionUnit,
                        RetailPrice = vm.RetailPrice,
                        UnitCost = vm.UnitCostPrice,
                        ReorderingLevel = vm.ReOrderingLevel,
                        IsNarcotic = vm.IsNarcotic,
                        NoOfStrips = vm.NoOfStrips,
                        SuppliersList = vm.Suppliers.ToList(),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                        (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    })
                  .OrderByDescending(i => i.ItemId)
                  .ToList();
                
               
            }
            return r;
        }
        public List<ItemsVM> GenericSearchForAllItems2()
        {
            IQueryable<Item> result;
            List<ItemsVM> r = null;
            
            
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            

           

            //if (dtfrom.HasValue && dtto.HasValue)
            //result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(dtfrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(dtto));

            if (true)
            {

                r = result.Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    PCTCode = vm.PCTCode,
                    Unit = vm.Unit,
                    chemicalname = vm.ChecmicalName,
                    ConversionUnit = vm.ConversionUnit,
                    RetailPrice = vm.RetailPrice,
                    UnitCost = vm.UnitCostPrice,
                    ReorderingLevel = vm.ReOrderingLevel,
                    NoOfStrips = vm.NoOfStrips,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems
                                    join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                    where s.IsActive && s.Item.ItemID == vm.ItemID &&
                                    DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) 
                                    select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems
                                     join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                     where ai.IsActive && ai.Item.ItemID == vm.ItemID
                                     select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems
                                            join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                            where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                            && es.Store != null && es.Store.IsActive
                                            select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems
                                 join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                 where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID
                                 select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })

              .OrderByDescending(i => i.ItemId)
              .ToList();


            }
            else
            {


                r = result.Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                    Manufacturer = vm.Manufacturer,
                    PCTCode = vm.PCTCode,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    NoOfStrips = vm.NoOfStrips,
                    ConversionUnit = vm.ConversionUnit,
                    RetailPrice = vm.RetailPrice,
                    UnitCost = vm.UnitCostPrice,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                    (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
              .OrderByDescending(i => i.ItemId)
              .ToList();


            }
            return r;
        }
        public List<StockStaticsVM> GenericSearchForAllItems1_Rpt(int RackId,
            int ManunfId,
            int SuppId,
            DateTime dtfrom,
            DateTime dtto,
            string Item)
        {
            IQueryable<Item> result;
            List<StockStaticsVM> r = null;
            if (SuppId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SuppId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }

            if (RackId > 0)
                result = result.Where(i => i.Rack.RackId == RackId);
            if (ManunfId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManunfId);
            if (Item != null)
            {
                result = result.Where(i => i.ItemName.Contains(Item));
            }
            //if (dtfrom.HasValue && dtto.HasValue)
                //result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(dtfrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(dtto));

            if (true)
            {
                
                r = result.Select(vm => new StockStaticsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    GenericName = vm.ChecmicalName,
                    SuppliersList = vm.Suppliers.ToList(),
                    Manufecturers = vm.Manufacturer,
                    Racks = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                    totalstock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID && si.CreatedAt >= dtfrom && si.CreatedAt <= dtto).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    TotalAdjustment = (from ai in cxt.AdjustmentItems
                                     join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                     where ai.IsActive && ai.Item.ItemID == vm.ItemID && ai.CreateDate >= dtfrom && ai.CreateDate <= dtto
                                     select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    TotalExpired = (from s in cxt.StockItems
                                    join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                    where s.IsActive && s.Item.ItemID == vm.ItemID &&
                                    DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) && b.CreatedAt >= dtfrom && b.CreatedAt <= dtto
                                    select s).Sum(s => (int?)s.Quantity) ?? 0,
                    TotalConsumed = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID && s.CreateDate >= dtfrom && s.CreateDate <= dtto select s).Sum(s => (int?)s.Quantity) ?? 0,
                    UnitCost = vm.UnitCostPrice,
                    RetailPrice = vm.RetailPrice,
                    StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.CreateDate >= dtfrom && sr.CreateDate <= dtto select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    BonusStock = (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems
                                 join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                 where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID

                                 select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
              
                    
                })

              .OrderByDescending(i => i.ItemId)
              .ToList();

                
            }
            
            return r;
        }
        public List<StockStaticsVM> GenericSearchForAllItems2_Rpt()
        {
            IQueryable<Item> result;
            List<StockStaticsVM> r = null;
            
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            

           
            //if (dtfrom.HasValue && dtto.HasValue)
            //result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(dtfrom) && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(dtto));

            if (true)
            {

                r = result.Select(vm => new StockStaticsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    GenericName = vm.ChecmicalName,
                    SuppliersList = vm.Suppliers.ToList(),
                    Manufecturers = vm.Manufacturer,
                    Racks = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == vm.ItemID select ri.Rack.Name).FirstOrDefault(),
                    totalstock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    TotalAdjustment = (from ai in cxt.AdjustmentItems
                                       join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId
                                       where ai.IsActive && ai.Item.ItemID == vm.ItemID
                                       select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    TotalExpired = (from s in cxt.StockItems
                                    join b in cxt.Batches on s.Batch.BatchId equals b.BatchId
                                    where s.IsActive && s.Item.ItemID == vm.ItemID &&
                                    DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now)
                                    select s).Sum(s => (int?)s.Quantity) ?? 0,
                    TotalConsumed = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    UnitCost = vm.UnitCostPrice,
                    RetailPrice = vm.RetailPrice,
                    StockReturn = (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    BonusStock = (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems
                                 join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId
                                 where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID

                                 select cs).Sum(cs => (int?)cs.Quantity) ?? 0,


                })

              .OrderByDescending(i => i.ItemId)
              .ToList();


            }

            return r;
        }
        private int GetReturnStock(Item vm)
        {
            //--StockReturnQuantity--//
            //return (cxt.ReturnStockItems.Where(x => x.IsActive && x.Item.ItemID == vm.ItemID).GroupBy(l => l.Batch.BatchId).Select(g => g.OrderByDescending(l => l.ReturnStockItemId).FirstOrDefault()).ToList().Sum(x => (int?)x.Quantity) ?? 0);
            return 0;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive =1 )
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from stockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) <CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.ItemName like '%" + FilterString + "%' AND i.IsActive = 1" +
            //                            @" group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();

            IPagedList<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemName.ToLower().Contains(FilterString))
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   SuppliersList = vm.Suppliers.ToList(),
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByStockDate(DateTime ItemCreateDate, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreateDate,23) = @ItemCreateDate
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            where i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreateDate", ItemCreateDate.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(ItemCreateDate))
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByManuf(int ManunfId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreateDate,23) = @ItemCreateDate
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            where i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreateDate", ItemCreateDate.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Manufacturer.ManufacturerId == ManunfId)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataBySupp(int SuppId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreateDate,23) = @ItemCreateDate
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            where i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreateDate", ItemCreateDate.ToString("yyyy-MM-dd "))).ToList();

            //IPagedList<ItemsVM>
            var r = cxt.Suppliers
            .Where(s => s.SupplierID == SuppId && s.IsActive).FirstOrDefault();

            var r2 = cxt.Entry(r).Collection(s => s.Items)
                .Query()
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.ItemID)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);

            return r2;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByCat(int CatId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.Stock_StockId where si.Item_ItemID = i.itemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.itemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreateDate,23) = @ItemCreateDate
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            where i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreateDate", ItemCreateDate.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == CatId)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithAvailableStock(int PageNo, int PageSize, bool IsHoldStockItems)
        {
            IPagedList<ItemsVM> r;
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock) > 0)
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);
            }
            else
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock) > 0)
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);
            }
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithLowStock(int PageNo, int PageSize, bool IsHoldStockItems)
        {
            IPagedList<ItemsVM> r;
            if (IsHoldStockItems)
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Where(i => i.ReOrderingLevel > 0)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock) <= x.ReorderingLevel)
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);
            }
            else
            {
                r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ReOrderingLevel > 0)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   IsNarcotic = vm.IsNarcotic,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               })
               .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock) <= x.ReorderingLevel)
               .OrderByDescending(i => i.ItemId)
               .ToPagedList(PageNo, PageSize);
            }
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithExpiredStock(int PageNo, int PageSize)
        {
            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => x.ExpiredStock > 0)
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockData(int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemID, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemID, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.itemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.itemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.itemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.Adjustment_AdjustmentId where ai.Item_ItemID = i.itemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.itemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemID, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemID = i.ItemID 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemID Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            IPagedList<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   IsNarcotic = vm.IsNarcotic,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        //------------ Reporting ------------//
        public List<ItemsVM> GenericSearchForAllItems_Rpt(string FilterByName, int RackId, int ManunfId, int SuppId, int CatId, int StockWiseItems, DateTime? CreatedDate, bool IsHoldingStock)
        {
            IQueryable<Item> result;
            List<ItemsVM> r = null;
            if (SuppId > 0)
            {
                Supplier res = cxt.Suppliers.Where(s => s.SupplierID == SuppId).FirstOrDefault();
                result = cxt.Entry(res).Collection(rs => rs.Items).Query()
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDefault).AsQueryable();
            }
            else
            {
                result = cxt.Items
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDefault)
                            .AsQueryable();
            }
            if (!string.IsNullOrEmpty(FilterByName))
                result = result.Where(i => i.ItemName.ToLower().StartsWith(FilterByName));
            if (CatId > 0)
                result = result.Where(i => i.Category.CategoryId == CatId);
            if (ManunfId > 0)
                result = result.Where(i => i.Manufacturer.ManufacturerId == ManunfId);
            if (CreatedDate.HasValue)
                result = result.Where(i => DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(CreatedDate));
            if (IsHoldingStock)
            {
                r = result.Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    ManufacturerName = vm.Manufacturer != null ? vm.Manufacturer.Name : "",
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    UnitCost = vm.UnitCostPrice,
                    RetailPrice = vm.RetailPrice,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                      (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                })
              .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
              .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
              .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock - x.StockReturn - x.BonusStock) > 0)
              .OrderByDescending(i => i.ItemId)
              .ToList();
            }
            else
            {
                r = result.Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    ManufacturerName = vm.Manufacturer != null ? vm.Manufacturer.Name : "",
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    UnitCost = vm.UnitCostPrice,
                    RetailPrice = vm.RetailPrice,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    //--StockReturnQuantity--//
                    StockReturn =
                      (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == vm.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    //--BonusQunatity--//
                    BonusStock =
                    (from ii in cxt.invoiceItems join i in cxt.Invoices on ii.Invoice.InvoiceId equals i.InvoiceId where ii.IsActive && i.IsActive && ii.Item.ItemID == vm.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                })
              .WhereIf(StockWiseItems == 3, x => x.ExpiredStock > 0)
              .WhereIf(StockWiseItems == 2, x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock - x.StockReturn - x.BonusStock) <= x.ReorderingLevel)
              .WhereIf(StockWiseItems == 1, x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock - x.StockReturn - x.BonusStock) > 0)
              .OrderByDescending(i => i.ItemId)
              .ToList();
            }
            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByItemNameFilter_Rpt(string FilterString)
        {
            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemName.ToLower().Contains(FilterString))
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   SuppliersList = vm.Suppliers.ToList(),
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByStockDate_Rpt(DateTime ItemCreateDate)
        {
            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(ItemCreateDate))
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();

            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByManuf_Rpt(int ManunfId)
        {
            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Manufacturer.ManufacturerId == ManunfId)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();

            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataBySupp_Rpt(int SuppId)
        {
            var r = cxt.Suppliers
            .Where(s => s.SupplierID == SuppId && s.IsActive).FirstOrDefault();

            var r2 = cxt.Entry(r).Collection(s => s.Items)
                .Query()
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.ItemID)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .OrderByDescending(i => i.ItemId)
                .ToList();
            return r2;
        }
        public List<ItemsVM> GetItemsWithStockDataByCat_Rpt(int CatId)
        {
            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == CatId)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();

            return r;
        }
        public List<ItemsVM> GetItemsWithAvailableStock_Rpt(bool IsHoldStockItems)
        {
            List<ItemsVM> r;
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock) > 0)
                .OrderByDescending(i => i.ItemId)
                .ToList();
            }
            else
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - x.ExpiredStock - (x.ExpiredConsumedStock - x.ConsumedStock) + x.AdjustedStock + x.HoldStock) > 0)
                .OrderByDescending(i => i.ItemId)
                .ToList();
            }
            return r;
        }
        public List<ItemsVM> GetItemsWithLowStock_Rpt(bool IsHoldStockItems)
        {
            List<ItemsVM> r;
            if (IsHoldStockItems)
            {
                r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Where(i => i.ReOrderingLevel > 0)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock) <= x.ReorderingLevel)
                .OrderByDescending(i => i.ItemId)
                .ToList();
            }
            else
            {
                r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ReOrderingLevel > 0)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   IsNarcotic = vm.IsNarcotic,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               })
               .Where(x => (x.TotalStock - (x.ExpiredStock - x.ExpiredConsumedStock) - x.ConsumedStock + x.AdjustedStock + x.HoldStock) <= x.ReorderingLevel)
               .OrderByDescending(i => i.ItemId)
               .ToList();
            }
            return r;
        }
        public List<ItemsVM> GetItemsWithExpiredStock_Rpt()
        {
            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemID,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Barcode = vm.Barcode,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    ReorderingLevel = vm.ReOrderingLevel,
                    IsNarcotic = vm.IsNarcotic,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                })
                .Where(x => x.ExpiredStock > 0)
                .OrderByDescending(i => i.ItemId)
                .ToList();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockData_Rpt()
        {
            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemID,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   IsNarcotic = vm.IsNarcotic,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == vm.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == vm.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == vm.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == vm.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == vm.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        #endregion

    }

    public static class CollectionExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}