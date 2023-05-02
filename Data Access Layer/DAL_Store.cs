using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Store
    {
        private MultiStoreEntities db;

        public DAL_Store()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<Store> GetAll()
        {
            return db.Stores.ToList();
        }

        public Store Getbyid(int id)
        {
            return db.Stores.Find(id);
        }


        public void Insert(Store admin)
        {
            db.Stores.Add(admin);
            SaveChanges();
        }

        public void Update(Store admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Store adm = db.Stores.Find(id);
            db.Stores.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
