using Pharmacy.Core.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class OrderAws
    {
        public string code { get; set; }
        public string msg { get; set; }
        public List<m_Order> data { get; set; }
    }
}
