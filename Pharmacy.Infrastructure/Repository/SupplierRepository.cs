using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using System.Data.Entity;
using PagedList;
using System.Data.SqlClient;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.Infrastructure.Repository
{
    public class SupplierRepository : Repository<Supplier>
    {
        PharmacyContext cxt = null;
        public SupplierRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Supplier> GetActiveSuppliers(int PageNo, int PageSize)
        {
            return cxt.Suppliers
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.SupplierID)
                .ToPagedList(PageNo, PageSize);
        }
        public IPagedList<Supplier> GetActiveSuppliers(long SupplierId)
        {
            return cxt.Suppliers
                .Where(s => s.IsActive && s.SupplierID == SupplierId)
                .OrderByDescending(s => s.SupplierID)
                .ToPagedList(1, 1);
        }
        public List<Supplier> GetActiveSuppliers()
        {
            return cxt.Suppliers
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.SupplierID).ToList();
        }

        public SupplierVM GetSupplierWithItems(long SupplierId, int PageNo, int PageSize)
        {
            //return cxt.Database.SqlQuery<Item>("select i.ItemName, i.IsNarcotic FROM Items i inner join SupplierItem si on i.ItemID = si.Item_ItemIs WHERE si.Supplier_SupplierId = @SupplierId ", new SqlParameter("@SupplierId", SupplierId)).OrderByDescending(i => i.ItemID).ToPagedList(PageNo, PageSize);            
            return cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => new SupplierVM()
                {
                    SupplierID = s.SupplierID,
                    Name = s.Name,
                    Phone = s.Phone,
                    Address = s.Address,
                    PrimaryContactPersonName = s.PrimaryContactPersonName,
                    PrimaryContactPersonPhone = s.PrimaryContactPersonPhone,
                    OpeningBalance = s.OpeningBalance,
                    NTN = s.NTN,
                    STN = s.STN,
                    CreditLImit = s.CreditLImit,
                    Items = s.Items.Where(i => i.IsActive).ToList()
                }).FirstOrDefault();
        }

        public SupplierDetailsVM GetSupplierWithDataDetails(long SupplierID)
        {
            //return cxt.Suppliers
            //    .Where(s => s.SupplierID == SupplierID)
            //    .Where(s => s.IsActive)
            //    .Include(s => s.Items)
            //    .Include(s => s.Stocks.Select(si => si.StockItems.Select(item => item.Item)))
            //    .FirstOrDefault();

            return cxt.Suppliers
                 .Where(s => s.SupplierID == SupplierID && s.IsActive)
                 .Select(d => new SupplierDetailsVM
                 {
                     Supplier = d,
                     
                     Items = d.Items.ToList(),
                     Stocks = (d.Stocks.Where(s => s.IsActive && s.GrandTotal > 0).Select(st => new StockVM
                     {
                         IsActive = st.IsActive,
                         StockId = st.StockId,
                         DocumentNo = st.DocumentNo,
                         SupplierInvoiceNo = st.SupplierInvoiceNo,
                         SupplierIvoiceDate = st.SupplierIvoiceDate,
                         ImagePath = st.ImagePath,
                         CreateDate = st.CreateDate,
                         UpdateDate = st.UpdateDate,
                         TotalAmount = st.GrandTotal,// (double?)(st.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)) ?? 0,
                         TotalPaid = (double?)(st.InvoicePayments.Sum(ip => ip.Amount)) ?? 0,
                         TotalReturnStockAmount = (double?)(from r in cxt.StockReturns where r.IsActive == true && r.Stock.StockId == st.StockId select r.GrandTotal).FirstOrDefault() ?? 0,
                         StockItems = (st.StockItems.Where(si => si.IsActive).Select(si => new StockItemVM
                         {
                             Item = si.Item,
                             Quantity = si.Quantity,
                             TotalCost = si.TotalCost,
                         })).ToList(),
                         StockPayments = (st.InvoicePayments.Select(sp => new StockPaymentVM // can be added where caluse in future to fetch only active payments
                         {
                             Payment = sp.Amount,
                             PaymentType = sp.PaymentMode,
                             CreateDate = sp.Date
                         })).ToList()
                     })).ToList()
                 }).FirstOrDefault();
        }
        public IPagedList<StockVM> GetBillPayments_Report(int PageNo, int PageSize)
        {
            return cxt.Stocks
                .Where(s => s.Supplier.SupplierID != null && s.IsActive)
                .Select(st => new StockVM()
                {
                    Supplier = st.Supplier,
                    IsActive = st.IsActive,
                    StockId = st.StockId,
                    DocumentNo = st.DocumentNo,
                    SupplierInvoiceNo = st.SupplierInvoiceNo,
                    SupplierIvoiceDate = st.SupplierIvoiceDate,
                    ImagePath = st.ImagePath,
                    CreateDate = st.CreateDate,
                    UpdateDate = st.UpdateDate,
                    TotalReturnStockAmount = (double?)(from r in cxt.StockReturns where r.IsActive == true && r.Stock.StockId == st.StockId select r.GrandTotal).FirstOrDefault() ?? 0,
                    TotalAmount = (cxt.Stocks.Where(si => si.IsActive && si.Supplier.SupplierID == st.Supplier.SupplierID).Sum(si => si.GrandTotal)), //st.GrandTotal
                    TotalPaid =  (double?)(st.InvoicePayments.Sum(ip => ip.Amount)) ?? 0,
                    StockPayments = (cxt.Stocks.Where(sp => sp.IsActive && sp.Supplier.SupplierID == st.Supplier.SupplierID).Select(sp => new StockPaymentVM // can be added where caluse in future to fetch only active payments
                    {
                        TotalPaid = (double?)(sp.InvoicePayments.Sum(ip => ip.Amount)) ?? 0
                    })).ToList(),
                })
                .OrderByDescending(r => r.CreateDate).ToPagedList(PageNo, PageSize);
        }

        public SupplierStockVM GetSupplierStockAndPayments(long SupplierId)
        {
            return cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => new SupplierStockVM
                {
                    Supplier = s,
                    StockPayments = (s.Stocks.Where(x=>x.IsActive ==true).Select(stp => new StockBillVM
                    {
                        StockId = stp.StockId,
                        DocumentNo = stp.DocumentNo,
                        SupplierInvoiceNo = stp.SupplierInvoiceNo,
                        SupplierInvoiceDate = stp.SupplierIvoiceDate,
                        TotalAmount = stp.GrandTotal,// (double?)(stp.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)) ?? 0,
                        TotalReturnStockAmount = (double?)(from r in cxt.StockReturns where r.IsActive == true && r.Stock.StockId == stp.StockId select r.GrandTotal).FirstOrDefault() ?? 0,
                        TotalPaid = (double?)(stp.InvoicePayments.Sum(ip => ip.Amount)) ?? 0

                    })).ToList()
                }).FirstOrDefault();
            //return cxt.Stocks
            //    .Where(s => s.Supplier.SupplierID == SupplierId)
            //    .Select(s => new SupplierStockVM
            //    {
            //        Supplier = s.Supplier,
            //        StockPayments = (new StockPaymentsVM(){
            //            StockId = s.StockId,
            //            DocumentNo = s.DocumentNo,
            //            SupplierInvoiceNo = s.SupplierInvoiceNo,
            //            SupplierInvoiceDate = s.SupplierIvoiceDate,
            //            TotalAmount = (s.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)),
            //            TotalPaid = (s.InvoicePayments.Sum(ip => ip.Payment))
            //        })

            //    }).ToList();
        }

        public Supplier GetSupplierByName(string SupplierName)
        {
            return cxt.Suppliers.Where(sp => sp.Name.ToLower().Equals(SupplierName)).FirstOrDefault();
        }

        public bool SuppliersExist()
        {
            return cxt.Suppliers.Any();
        }

        public SupplierItemsVM getSupplierItems(long SupplierID)
        {
            //return cxt.Suppliers
            //    .Where(s => s.SupplierID == SupplierID)
            //    .Where(s => s.IsActive)
            //    .Include(s => s.Items)
            //    .Include(s => s.Stocks.Select(si => si.StockItems.Select(item => item.Item)))
            //    .FirstOrDefault();
            return cxt.Suppliers
             .Where(s => s.SupplierID == SupplierID)
             .Select(d => new SupplierItemsVM
             {
                 SupplierId = d.SupplierID,
                 Items = d.Items.Where(i => i.IsActive).Select(ci => new ItemsComboVM
                 {
                     ItemId = ci.ItemID,
                     //ItemName = ci.ItemName
                 }).ToList()
             }).FirstOrDefault();
        }

        public List<SuppliersComboVM> getSuppliersForCombo()
        {
            return cxt.Suppliers.Where(s => s.IsActive)
                .Select(s => new SuppliersComboVM
                {
                    SupplierId = s.SupplierID,
                    Name = s.Name
                }).ToList();
        }
    }
}