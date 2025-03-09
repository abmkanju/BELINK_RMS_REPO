using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pharmacy.Infrastructure.Repository
{
    public class ExpenseRepository : Repository<Expense>
    {
        PharmacyContext cxt;
        public ExpenseRepository(PharmacyContext cxt): base(cxt)
        {
            this.cxt = cxt;
        }
        public int GetVoucherNo()
        {
            int VoucherNo = 0;
            if (cxt.Expenses.FirstOrDefault() == null)
            {
                VoucherNo = 1;
            }
            else
            {
                VoucherNo = cxt.Expenses.Max(n => n.VoucherNo) + 1;
            }
            return VoucherNo;
        }
    }
}
