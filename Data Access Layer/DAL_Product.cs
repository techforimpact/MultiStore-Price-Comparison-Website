using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Product
    {
        private MultiStoreEntities db;

        public DAL_Product()
        {
            db = new MultiStoreEntities();
        }

        public IEnumerable<Product> GetbyName(string name)
        {

            try
            {
                IEnumerable<Product> products = db.Products.ToList();

                if (!String.IsNullOrEmpty(name))
                {
                    products = products.Where(p => p.name.Contains(name)).ToList();
                }


                return products;

            }
            catch
            {
                return Enumerable.Empty<Product>();
            }

        }


        public IEnumerable<Product> GetAll()
        {

            try
            {
                IEnumerable<Product> products = db.Products.ToList();
                return products;
            }
            catch(Exception e) 
            {
                return Enumerable.Empty<Product>();
            }

        }

        public IEnumerable<Product> GetStoreProducts(int id)
        {
            var products = db.Products.Where(p => p.store_id == id).ToList();

            return products;
        }


        public IEnumerable<Product> GetCategoryProducts(int id)
        {
            var products = db.Products.Where(p => p.category_id == id).ToList();
            return products;
        }


        public Product Getbyid(int id)
        {
            return db.Products.Find(id);
        }


        public void Insert(Product admin)
        {
            db.Products.Add(admin);
            SaveChanges();
        }

        public void Update(Product admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Product adm = db.Products.Find(id);
            db.Products.Remove(adm);
            SaveChanges();

        }

        public void DeleteObj(Product obj)
        {
            db.Products.Remove(obj);
            SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
