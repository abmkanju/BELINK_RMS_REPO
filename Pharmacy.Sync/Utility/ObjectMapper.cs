using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Sync.MappedEntities;
using Pharmacy.Core.Entities;
using Pharmacy.Core;

namespace Pharmacy.Sync.Utility
{
    public class ObjectMapper
    {
        public List<m_Category> MapCategory(List<Category> Cats)
        {
            List<m_Category> mapped = new List<m_Category>();
            foreach (Category c in Cats)
            {
                m_Category m = new m_Category();
                m.desktop_id = (int)c.CategoryId;
                m.name = c.Name;
                m.created_at = c.CreateDate;
                m.updated_at = c.UpdateDate;
                m.practice_id = SharedVariables.PracticeId;
                mapped.Add(m);
            }
            return mapped;
        }

        public List<m_Item> MapItem(List<Item> items)
        {
            List<m_Item> mapped = new List<m_Item>();
            foreach (Item i in items)
            {
                m_Item m = new m_Item();
                m.desktop_id = i.ItemID;
                m.item_name = i.ItemName;
                m.barcode = i.Barcode;
                m.rack = i.RackNo;
                m.stocking_unit = i.Unit;
                m.re_ordering_level = i.ReOrderingLevel;
                m.retail_price = (float)i.RetailPrice;
                m.chemical_name = i.ChecmicalName;
                m.unit_cost = (float)i.UnitCostPrice;
                m.narcotic = i.IsNarcotic;
                m.item_classification_id = (int)i.CategoryId;
                m.user_id = i.User.UserId;
                m.created_at = i.CreateDate;
                m.updated_at = i.UpdateDate;
                m.deleted_at = i.DeletedAt;
                m.practice_id = SharedVariables.PracticeId;
                mapped.Add(m);
            }
            return mapped;
        }

        public List<m_Adjustment> MapAdjustment(List<Adjustment> Adjustments)
        {
            List<m_Adjustment> mapped = new List<m_Adjustment>();
            foreach (Adjustment a in Adjustments)
            {
                m_Adjustment m = new m_Adjustment();
                m.desktop_id = (int)a.AdjustmentId;
                m.user_id = a.User.UserId;
                m.created_at = a.CreateDate;
                m.updated_at = a.UpdateDate;
                mapped.Add(m);
            }
            return mapped;
        }

        public List<m_AdjustmentItem> MapAdjustmentItems(List<AdjustmentItem> AdjustmentItems)
        {
            List<m_AdjustmentItem> mapped = new List<m_AdjustmentItem>();
            foreach (var a in AdjustmentItems)
            {
                m_AdjustmentItem m = new m_AdjustmentItem();
                m.desktop_id = (int)a.AdjustmentItemId;
                m.item_id = a.Item.ItemID;
            }
            return null;
        }
    }
}