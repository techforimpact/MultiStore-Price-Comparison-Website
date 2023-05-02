using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_StoreImages
    {
        private MultiStoreEntities db;

        public DAL_StoreImages() 
        { 
            db= new MultiStoreEntities();
        }

        public IEnumerable<StoreImage> GetAll()
        {
            return db.StoreImages.ToList();
        }

        public StoreImage Get(int id)
        {
            var image = db.StoreImages.Find(id);
            return image == null ? null : image;
        }


        public void Insert(StoreImage admin)
        {
            db.StoreImages.Add(admin);
            SaveChanges();
        }

        public void Update(StoreImage admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            StoreImage adm = db.StoreImages.Find(id);
            db.StoreImages.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }


    }
}
