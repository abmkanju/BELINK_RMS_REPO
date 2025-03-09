using Pharmacy.Sync.MappedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Sync.Utility
{
    public class SyncResponse
    {
        public string status { get; set; }
        public List<failed_instances>  failed_instances{ get; set; }
        public string error { get; set; }
    }
}