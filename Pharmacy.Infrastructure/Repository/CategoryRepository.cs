using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>
    {
        PharmacyContext cxt;
        public CategoryRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public CategoryVM GetCategoryActiveItems(int categiryId)
        {
            return cxt.Categories.Where(c => c.CategoryId == categiryId)
                .Select(c => new CategoryVM()
                {
                    CategoryId = c.CategoryId,
                    Items = (c.Items.Where(i => i.IsActive).Select(i => new ItemsVM()
                    {
                        ItemId = i.ItemID,
                        ItemName = i.ItemName
                    })).ToList()
                }).FirstOrDefault();
        }

        public List<Category> GetAllActiveCategories()
        {
            return cxt.Categories.Where(c => c.IsActive).ToList();
        }

        public Category GetCategoryByItemId(long itemId)
        {
            return cxt.Items.Where(i => i.ItemID == itemId).Select(i => i.Category).FirstOrDefault();
        }
        public Category GetCategoryIdByName(string catName)
        {
            return cxt.Categories.Where(c => c.Name.ToLower() == catName).FirstOrDefault();
        }

        public bool CheckDuplicateName(string catName)
        {
            return cxt.Categories.Any(x => x.Name.ToLower() == catName.ToLower());
        }

        public bool CategoriesExist()
        {
            return cxt.Categories.Any();
        }
        public Category GetCategoryByName(string Category)
        {
            return cxt.Categories.Where(sp => sp.Name.ToLower().Equals(Category)).FirstOrDefault();
        }
    }
}
