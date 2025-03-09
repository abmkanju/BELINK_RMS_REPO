using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Pharmacy.Core.ViewModels
{
    public class PurchaseReportsMasterVM
    {
        public double TotalPaid { get; set; }
        public double TotalAdvanceTax { get; set; }
        public double TotalWHT { get; set; }
        public double TotalAmount { get; set; }
        public double TotalPurchaseAmount { get; set; }
        public int pageno { get; set; }
        public IPagedList<PurchaseReportVM> PurchaseReports { get; set; }        
    }
}
