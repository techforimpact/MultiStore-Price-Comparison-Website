using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_CategoryImages
    {

        private MultiStoreEntities db;

        public DAL_CategoryImages()
        {
            db = new MultiStoreEntities();
        }

        public IEnumerable<CategoryImage> GetAll()
        {
            return db.CategoryImages.ToList();
        }

        public CategoryImage Get(int id)
        {
            var image = db.CategoryImages.Find(id);
            return image == null ? null : image;
        }


        public void Insert(CategoryImage admin)
        {
            db.CategoryImages.Add(admin);
            SaveChanges();
        }

        public void Update(CategoryImage admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            CategoryImage adm = db.CategoryImages.Find(id);
            db.CategoryImages.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }



    }
}
