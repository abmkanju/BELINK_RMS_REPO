using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class FBRResponseVM
    {
        public string InvoiceNumber { get; set; }
        public string Code { get; set; }
        public string Response { get; set; }
        public string Error { get; set; }
    }
}
