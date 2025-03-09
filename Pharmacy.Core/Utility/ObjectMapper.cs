using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ServiceModel;

namespace Pharmacy.Core.Utility
{
    public static class ObjectMapper
    {

        #region MapItem
        public static m_Item MapSaveItem(Item item)
        {
            try
            {
                m_Item model = new m_Item
                {
                    id = item.ItemID,
                    desktop_id = item.ItemID,
                    item_name = item.ItemName,
                    barcode = item.Barcode,
                    rack = item.Rack == null ? null : item.Rack.Name,
                    stocking_unit = item.Unit,
                    re_ordering_level = item.ReOrderingLevel,
                    strips = item.NoOfStrips,
                    retail_price = (float)Math.Round(item.RetailPrice, 3),
                    generic_name = item.ChecmicalName,
                    conversion_unit = item.ConversionUnit,
                    unit_cost = (float)Math.Round(item.UnitCostPrice, 3),
                    narcotic = item.IsNarcotic,
                    item_classification_id = (int?)item.Category.CategoryId,
                    manufacturer_id = (int?)item.ManufacturerId,
                    user_id = null,//i.User.UserId,
                    created_at = item.CreateDate,
                    updated_at = item.UpdateDate,
                    deleted_at = item.DeletedAt,
                    practice_id = null,
                    requested_by_id = SharedVariables.AdminPharmacySetting.PracticeId,
                    inventory_supplier_ids = item.Suppliers.Select(s => (int)s.SupplierID).ToList(),
                    status = 0
                };
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MapPatient
        public static m_patient MapSavePatient(Patient pat)
        {
            try
            {
                m_patient model = new m_patient
                {
                    patient_name = pat.Name,
                    phone = pat.Phone
                };
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
