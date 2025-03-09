using Newtonsoft.Json;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ManageOfflineTask
{
    public class OfflineJsonTask
    {
        //SOT
        /// <summary>
        /// Any changes in OfflineTask must also be changed in the Configuration.cs class
        /// </summary>
        /// <param name="unitOfwork"></param>
        /// <param name="obj"></param>
        /// <param name="moduleName"></param>
        /// <param name="moduleAction"></param>
        /// <param name="ModuleId"></param>
        public static void SaveOfflineTask(UnitOfWork unitOfwork, dynamic obj, ModuleName moduleName, ModuleAction moduleAction, long ModuleId)
        {
            try
            {
                OfflineTask task = new OfflineTask();
                task.ModuleName = moduleName.ToString();
                task.ModuleAction = moduleAction.ToString();
                task.ModuleJson = JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                task.ModuleId = ModuleId;
                task.CreatedOn = DateTime.Now;
                task.IsActive = true;
                unitOfwork.OfflineTaskRepository.Insert(task);
                unitOfwork.Save();
            }
            catch (Exception ex)
            {

            }
        }

    }

    public enum ModuleName
    {
        User,
        Patient,
        Category,
        Supplier,
        Item,
        Batch,
        PurchaseOrder,
        Stock,
        FileStock,
        StockAdjustment,
        Expense,
        Invoice,
        SalesReturn,
        Store,
        StockReturn,
        Rack,
        Manufacturer,
        BulkItems,
        BulkFileItems,
        BulkBatches,
        BulkFileBatches,
        BulkStores,
        BulkSuppliers,
        BulkCategories,
        BulkManufactures,
        BulkStockConsumptionItems,
    }

    public enum ModuleAction
    {
        Create,
        Update,
        Delete,
    }



}
