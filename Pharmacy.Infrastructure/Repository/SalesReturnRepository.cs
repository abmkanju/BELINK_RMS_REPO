using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class SalesReturnRepository : Repository<SalesReturn>
    {
         PharmacyContext cxt = null;
         public SalesReturnRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public void InsertSalesReturn(SalesReturn objSalesReturn)
        {
            cxt.SalesReturn.Add(objSalesReturn);
            cxt.SaveChanges();
        }
        public long GetSalesReturnRefNo()
        {
            long saleRefNo = 0;
            if (cxt.SalesReturn.FirstOrDefault() == null || cxt.SalesReturn.OrderByDescending(i => i.SaleReturnId).FirstOrDefault().SaleReturnRefNo == 0)
            {
                saleRefNo = 100001;
            }
            else
            {
                saleRefNo = cxt.SalesReturn.Max(n => n.SaleReturnRefNo) + 1;
            }
            return saleRefNo;
        }
        public IPagedList<SalesReturnTransactionVM> GetSalesReturn(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
                return
                 (from i in cxt.SalesReturn
                  where i.IsActive 
                  && (DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom))
                  && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                  select new SalesReturnTransactionVM
                  {
                      SaleReturnId = i.SaleReturnId,
                      GrandTotal = i.GrandTotal,
                      SaleReturnRefNo = i.SaleReturnRefNo,
                      SalesReturnItem = (i.SalesReturnItem.Select(item => new SalesReturnItemVM
                      {
                          SaleRetItemId = (int)item.SaleReturnItemId,
                          isActive  = item.IsActive,
                          ReturnQuantity = item.ReturnQuantity,
                          RefundAmount = item.RefundAmount,
                          CreateDate = item.CreateDate,
                          UpdateDate = item.UpdateDate
                      })).Where(j => j.isActive == true).ToList(),
                      Patient = i.Patient,
                      PatientName= i.Patient.Name,
                      MRNo = i.Patient.MRNo

                  })
                .OrderByDescending(i => i.SaleReturnId).ToPagedList(PageNo, PageSize);
        }
        public SaleReturnAll GetSalesReturnItemById(long saleReturnItemId)
        {
            return
               (from i in cxt.SalesReturnItem
                join b in cxt.SalesReturn on i.SaleReturnItemId equals saleReturnItemId
                where i.SaleReturnItemId == (int)saleReturnItemId
                select new SaleReturnAll
                {
                 objPatient = i.SalesReturn.Patient,
                 salesReturn = (b.SalesReturnItem.Select(item => new SalesReturnItemVM
                {
                    ItemName = i.Item.ItemName,
                    BatchName = i.Batch.BatchName,
                    ReturnQuantity = i.ReturnQuantity,
                    RefundAmount = i.RefundAmount,
                    RetialPrice = i.RetialPrice,
                   })).ToList(),
          }).FirstOrDefault();
        }
        public SalesReturnItemVM GetAllSalesReturn()
        {
            return
               (from i in cxt.SalesReturnItem
                select new SalesReturnItemVM
                {
                    ItemName = i.Item.ItemName,
                    BatchName = i.Batch.BatchName,
                    ReturnQuantity = i.ReturnQuantity,
                    RefundAmount = i.RefundAmount,
                    RetialPrice = i.RetialPrice
                }).FirstOrDefault();
        }
        public SummationsVM GetTotalRefund(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM res = new SummationsVM
            {
                TotalRefund = (cxt.SalesReturnItem.Where(p => p.IsActive 
                    && DbFunctions.TruncateTime(p.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(p.CreateDate) <= DbFunctions.TruncateTime(DateTo)).Sum(r => (double?)r.RefundAmount) ?? 0)
            };
            return res;
        }
        public void SetSaleReturnInActive(long saleReturnId)
        {
            cxt.Database.ExecuteSqlCommand("Update SalesReturnItems set IsActive = 0 WHERE SaleReturnItemId = @SaleReturnItemId", new SqlParameter("@SaleReturnItemId", Convert.ToInt32(saleReturnId)));
        }
    }
}
