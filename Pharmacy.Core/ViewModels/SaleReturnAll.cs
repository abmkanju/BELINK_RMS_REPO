using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class SaleReturnAll
    {
        public List<SalesReturnItemVM> salesReturn { get; set; }
        public Patient objPatient { get; set; }
    }
}
