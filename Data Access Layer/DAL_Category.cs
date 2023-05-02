using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Category
    {
        private MultiStoreEntities db;

        public DAL_Category()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<Category> GetSubcategories(List<Category> categories, int? parentId = null)
        {
            List<Category> subcategories = new List<Category>();

            foreach (Category category in categories.Where(c => c.parent_id == parentId))
            {
                subcategories.Add(category);

                IEnumerable<Category> children = GetSubcategories(categories, category.id);

                if (children.Any())
                {
                    subcategories.AddRange(children);
                }
            }

            return subcategories;
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category Getbyid(int id)
        {
            return db.Categories.Find(id);
        }


        public void Insert(Category admin)
        {
            db.Categories.Add(admin);
            SaveChanges();
        }

        public void Update(Category admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Category adm = db.Categories.Find(id);
            db.Categories.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
