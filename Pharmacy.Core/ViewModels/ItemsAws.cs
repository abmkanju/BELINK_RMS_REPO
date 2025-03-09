using Pharmacy.Core.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemsAws
    {
        public string code { get; set; }
        public string msg { get; set; }
        public List<m_items1> data { get; set; }
    }
}
