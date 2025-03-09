using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.Interfaces
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetItemsBySupplierID(long SupplierID);
        //List<ItemsVM> GetItemsWithStockData(int StartRecord, int NexRequired);
        //List<ItemsVM> GetItemsWithStockDataByStockDate(DateTime StockDate, int OffSet, int NextFetch);
        //List<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString, int OffSet, int NextFetch);
        //List<Item_SuppliersVM> GetSuppliersByItemIds(List<ItemsVM> itemsList);
    }
}