using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ItemsEditVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Rack Rack { get; set; }
        public string RackName { get; set; }
        public string RackId { get; set; }
        public string Barcode { get; set; }        
        public int ReorderingLevel { get; set; }
        public double RetailPrice { get; set; }
        public double UnitCost { get; set; }
        public List<SupplierInfoVM> Suppliers { get; set; }
    }
}