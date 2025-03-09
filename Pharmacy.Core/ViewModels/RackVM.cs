using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class RackVM
    {
        public RackVM()
        {
            RackItems = new List<RackItemsVM>();
        }
        public int RackId { get; set; }
        public string Name { get; set; }
        public ICollection<RackItemsVM> RackItems { get; set; }
       

    }
    public class RackItemsVM
    {
        public int RackItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
