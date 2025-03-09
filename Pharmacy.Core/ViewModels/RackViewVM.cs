using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class RackViewVM
    {
        public int RackId { get; set; }
        public string Name { get; set; }
        public ICollection<ItemsVM> Items { get; set; }
    }
}