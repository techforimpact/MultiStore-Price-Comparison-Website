using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Price
    {

        private MultiStoreEntities db;

        public DAL_Price()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<Price> GetAll()
        {
            return db.Prices.ToList();
        }

        public IEnumerable<Price> GetAllByProduct(int id)
        {
            var prices = GetAll().Where(c => c.product_id == id);

            return prices;
        }

        public IEnumerable<Product> GetAllByStore(int id)
        {
            var prices = GetAll().Where(c => c.store_id == id).Select( c => c.Product);

            return prices;
        }

        public Price Getbyid(int id)
        {
            return db.Prices.Find(id);
        }


        public void Insert(Price admin)
        {
            db.Prices.Add(admin);
            SaveChanges();
        }


        public void Update(Price admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Price adm = db.Prices.Find(id);
            db.Prices.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
