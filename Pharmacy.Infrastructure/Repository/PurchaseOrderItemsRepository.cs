using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    
    public class PurchaseOrderItemsRepository : Repository<PurchaseOrderItem>
    {
        PharmacyContext cxt;
        public PurchaseOrderItemsRepository(PharmacyContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }

        public List<PurchaseOrderItem> GetItemsByPurchaseOrderNo(decimal PurchaseOrderNo)
        {
            return cxt.PurchaseOrderItems.Where(i => i.PurchaseOrderNo == PurchaseOrderNo).ToList();
        }     
        
        public void RemoveRange(List<PurchaseOrderItem> RemoveList)
        {
            cxt.PurchaseOrderItems.RemoveRange(RemoveList);
        }
    }
}