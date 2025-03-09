using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.Core.Interfaces
{
    public interface IPurchaseOrder
    {
        List<PurchaseOrderVM> GetPurchaseOrdersWithDetailByDateRange(DateTime datefrom, DateTime dateTo);
        PurchaseOrderVM GetPODetailByOrderNo(decimal PurchaseOrderNo);
        PurchaseOrder GetPODetailByOrderNo_Origional(decimal PurchaseOrderNo);
        List<PurchaseOrderVM> GetPurchaseOrderSummaryByDateRange(DateTime datefrom, DateTime dateTo);
        PurchaseOrderVM GetPurchaseOrdersWithDetailByOrderNo(decimal PurchaseOrderNo);
        decimal GetNewPurchaseOrderNumber();
        void SetOrderDeleted(decimal PurchaseOrderNo);
    }
}