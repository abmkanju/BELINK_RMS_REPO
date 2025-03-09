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
    public class VoucherRepository : Repository<OrderVoucher>
    {
        PharmacyContext cxt = null;
        public VoucherRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<VoucherVM> GetVoucherIntegrate(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.OrderVoucher
            .Where(i => i.IsActive
             && (DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom))
             && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(Voucher => new VoucherVM
            {
                OrderVoucherId = Voucher.OrderVoucherId,
                CreateDate = Voucher.CreateDate,
                SubTotal = Voucher.SubTotal,
                GrandTotal = Voucher.GrandTotal,
                IsVoucherPaid = Voucher.IsVoucherPaid,
                VoucherAmount = Voucher.VoucherValue,
                VoucherNumber = Voucher.VoucherNumber,
                WalletCredit = Voucher.WalletCredit
            })
            .OrderByDescending(i => i.OrderVoucherId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<VoucherVM> GetVoucherIntegrateByNumber(string voucherNumber, DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.OrderVoucher
            .Where(i => i.IsActive
             && i.VoucherNumber == voucherNumber)
            .Select(Voucher => new VoucherVM
            {
                OrderVoucherId = Voucher.OrderVoucherId,
                CreateDate = Voucher.CreateDate,
                SubTotal = Voucher.SubTotal,
                GrandTotal = Voucher.GrandTotal,
                VoucherAmount = Voucher.VoucherValue,
                VoucherNumber = Voucher.VoucherNumber,
                WalletCredit = Voucher.WalletCredit
            })
            .OrderByDescending(i => i.OrderVoucherId).ToPagedList(PageNo, PageSize);
        }
    }
}
