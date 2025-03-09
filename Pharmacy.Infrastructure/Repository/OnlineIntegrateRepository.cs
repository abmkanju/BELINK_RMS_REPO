using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Pharmacy.Core.ViewModels;
using PagedList;
using System.Data.SqlClient;
using Pharmacy.Core;

namespace Pharmacy.Infrastructure.Repository
{
    public class OnlineIntegrateRepository : Repository<OnlineOrder>
    {
        PharmacyContext cxt = null;
        public OnlineIntegrateRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<OnlineIntegrateVM> GetOnlineIntegrateByPhone(string phone, int PageNo, int PageSize)
        {
            return (from i in cxt.OnlineOrder
                    join p in cxt.OnlineCustomer on i equals p.OnlineOrder
                    where i.IsActive && p.Phone == phone
                    select new OnlineIntegrateVM
                    {
                        OrderId = i.OrderId,
                        CreateDate = i.CreateDate,
                        Origin = i.Origin,
                        OrderValue = i.OrderValue,
                        TotalDiscount = i.TotalDiscount,
                        GrandTotal = i.GrandTotal,
                        Pack = i.Pack,
                        origin = i.Origin,
                        WalletAmount = i.WalletAmount,
                        Delivered = i.Delivered,
                        Dispatch = i.Dispatch,
                        Cancel = i.Cancel,
                        InvoiceId = (int?)i.Invoice.InvoiceId,
                        CancelReason = i.CancelReason,
                        OnlineCustomer = (i.OnlineCustomer.Select(j => new OnlineCustomerVM
                        {
                            CustName = j.Name,
                            CustPhone = j.Phone,
                            CustAddress = j.Address
                        })).ToList(),
                        OnlineItem = (i.OnlineOrderItem.Select(j => new OnlineItemVM
                        {
                            //itemId = j.Desktop_ItemId,
                            ItemName = j.ItemName,
                            Quantity = j.Quantity,
                            ConversionUnit = j.ConversionUnit
                        }))
                    }).OrderByDescending(i => i.OrderId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<OnlineIntegrateVM> GetOnlineIntegrateByCutomer(string Customer, int PageNo, int PageSize)
        {
            return (from i in cxt.OnlineOrder
                    join p in cxt.OnlineCustomer on i equals p.OnlineOrder
                    where i.IsActive && p.Name == Customer
                    select new OnlineIntegrateVM
                    {
                        OrderId = i.OrderId,
                        CreateDate = i.CreateDate,
                        Origin = i.Origin,
                        OrderValue = i.OrderValue,
                        TotalDiscount = i.TotalDiscount,
                        GrandTotal = i.GrandTotal,
                        Pack = i.Pack,
                        origin = i.Origin,
                        WalletAmount = i.WalletAmount,
                        Delivered = i.Delivered,
                        Dispatch = i.Dispatch,
                        Cancel = i.Cancel,
                        InvoiceId = (int?)i.Invoice.InvoiceId,
                        CancelReason = i.CancelReason,
                        OnlineCustomer = (i.OnlineCustomer.Select(j => new OnlineCustomerVM
                        {
                            CustName = j.Name,
                            CustPhone = j.Phone,
                            CustAddress = j.Address
                        })).ToList(),
                        OnlineItem = (i.OnlineOrderItem.Select(j => new OnlineItemVM
                        {
                            //itemId = j.Desktop_ItemId,
                            ItemName = j.ItemName,
                            Quantity = j.Quantity,
                            ConversionUnit = j.ConversionUnit
                        }))
                    }).OrderByDescending(i => i.OrderId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<OnlineIntegrateVM> GetOnlineIntegrateByOrder(string Order, int PageNo, int PageSize)
        {
            return cxt.OnlineOrder
            .Where(i => i.IsActive && i.OrderValue == Order)
            .Select(OnlineOrder => new OnlineIntegrateVM
            {
                OrderId = OnlineOrder.OrderId,
                CreateDate = OnlineOrder.CreateDate,
                Origin = OnlineOrder.Origin,
                OrderValue = OnlineOrder.OrderValue,
                TotalDiscount = OnlineOrder.TotalDiscount,
                GrandTotal = OnlineOrder.GrandTotal,
                Pack = OnlineOrder.Pack,
                origin = OnlineOrder.Origin,
                WalletAmount = OnlineOrder.WalletAmount,
                Delivered = OnlineOrder.Delivered,
                Dispatch = OnlineOrder.Dispatch,
                Cancel = OnlineOrder.Cancel,
                CancelReason = OnlineOrder.CancelReason,
                InvoiceId = (int?)OnlineOrder.Invoice.InvoiceId,
                OnlineCustomer = (OnlineOrder.OnlineCustomer.Select(i => new OnlineCustomerVM
                {
                    CustName = i.Name,
                    CustPhone = i.Phone,
                    CustAddress = i.Address

                })).ToList(),
                OnlineItem = (OnlineOrder.OnlineOrderItem.Select(j => new OnlineItemVM
                {
                    //itemId = j.Desktop_ItemId,
                    ItemName = j.ItemName,
                    Quantity = j.Quantity,
                    ConversionUnit = j.ConversionUnit
                }))
            }).OrderByDescending(i => i.OrderId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<OnlineIntegrateVM> GetOnlineIntegrate(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.OnlineOrder
            .Where(i => i.IsActive
             && (DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom))
             && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(OnlineOrder => new OnlineIntegrateVM
            {
                OrderId = OnlineOrder.OrderId,
                CreateDate = OnlineOrder.CreateDate,
                Origin = OnlineOrder.Origin,
                OrderValue = OnlineOrder.OrderValue,
                TotalDiscount = OnlineOrder.TotalDiscount,
                GrandTotal = OnlineOrder.GrandTotal,
                Pack = OnlineOrder.Pack,
                Delivered = OnlineOrder.Delivered,
                Dispatch = OnlineOrder.Dispatch,
                Cancel = OnlineOrder.Cancel,
                CancelReason = OnlineOrder.CancelReason,
                origin = OnlineOrder.Origin,
                WalletAmount = OnlineOrder.WalletAmount,
                InvoiceId = (int?)OnlineOrder.Invoice.InvoiceId,
                OnlineCustomer = (OnlineOrder.OnlineCustomer.Select(i => new OnlineCustomerVM
                {
                    CustName = i.Name,
                    CustPhone = i.Phone,
                    CustAddress = i.Address

                })).ToList(),
                OnlineItem = (OnlineOrder.OnlineOrderItem.Select(j => new OnlineItemVM
                {
                    //itemId = j.Desktop_ItemId,
                    ItemName = j.ItemName,
                    Quantity = j.Quantity,
                    ConversionUnit = j.ConversionUnit
                }))
            }).OrderByDescending(i => i.OrderId).ToPagedList(PageNo, PageSize);
        }

        public OnlineIntegrateVM GetOnlineIntegrateById(int OrderId)
        {
            return cxt.OnlineOrder
            .Where(i => i.IsActive && i.OrderId == OrderId)
            .Select(OnlineOrders => new OnlineIntegrateVM
            {
                OrderId = OnlineOrders.OrderId,
                CreateDate = OnlineOrders.CreateDate,
                Origin = OnlineOrders.Origin,
                OrderValue = OnlineOrders.OrderValue,
                TotalDiscount = OnlineOrders.TotalDiscount,
                TotalAmount = OnlineOrders.TotalAmount,
                SubTotal = OnlineOrders.SubTotal,
                DiscountType = OnlineOrders.DiscountType,
                GrandTotal = OnlineOrders.GrandTotal,
                Pack = OnlineOrders.Pack,
                Delivered = OnlineOrders.Delivered,
                Dispatch = OnlineOrders.Dispatch,
                Cancel = OnlineOrders.Cancel,
                origin = OnlineOrders.Origin,
                WalletAmount = OnlineOrders.WalletAmount,
                CancelReason = OnlineOrders.CancelReason,
                DeliveryCharges = OnlineOrders.DeliveryCharges,
                VoucherNumber = OnlineOrders.VoucherNumber,
                VoucherAmount = OnlineOrders.VoucherAmount,
                PaymentMethod = OnlineOrders.PaymentMethod,

                //InvoiceId = OnlineOrder.Invoice.InvoiceId,
                OnlineCustomer = (OnlineOrders.OnlineCustomer.Select(i => new OnlineCustomerVM
                {
                    CustName = i.Name,
                    CustPhone = i.Phone,
                    CustAddress = i.Address

                })),
                OnlineItem = (OnlineOrders.OnlineOrderItem.Select(j => new OnlineItemVM
                {
                    //itemId = j.Desktop_ItemId,
                    ItemName = j.ItemName,
                    ItemId = j.UniqueItem_Id,
                    Rate = j.Rate,
                    Unit = j.Unit,
                    PerUnitCostPrice = j.PerUnitCostPrice,
                    SizeOfStrip = j.SizeOfStrip,
                    Discount = j.Discount,
                    DiscountType = j.DiscountType,
                    saletax = j.saletax,
                    saletaxType = j.saletaxType,
                    Amount = j.Amount,
                    NetAmount = j.NetAmount,
                    Quantity = j.Quantity,
                    ConversionUnit = j.ConversionUnit
                })),
            }).OrderByDescending(i => i.OrderId).FirstOrDefault();
        }

        public OnlineOrder GetOrderByIdWithDetails(int OrderId)
        {
            OnlineOrder Oc = cxt.OnlineOrder
                .Where(i => i.OrderId == OrderId)
                .Include(p => p.OnlineOrderItem)
                .Include(s => s.OnlineCustomer)
                .FirstOrDefault();
            return Oc;
        }
    }
}