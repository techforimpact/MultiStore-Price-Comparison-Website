using Object_Layer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Data_Access_Layer
{
    public class DAL_Product
    {
        private MultiStoreEntities db;

        public DAL_Product()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<Product> SearchProducts(string query)
        {
            var products = db.Products
                .Where(p => p.name.Contains(query))
                .ToList();

            return products;
        }

        public IEnumerable<Product> GetByPriceRange(decimal low , decimal high)
        {
            return db.Products.Where(p => p.Prices.Any(price => price.product_id == p.id && price.price1 >= low && price.price1 <= high));
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


        public IEnumerable<Product> GetCategoryProducts(int id)
        {
            var categories = db.Categories.ToList().Where(c => c.parent_id == id);

            var products = db.Products.Where(p => p.category_id == id).ToList();

            foreach( var cat in categories )
            {
                if (cat.parent_id == id)
                {
                    products.AddRange(db.Products.Where(p => p.category_id == cat.id).ToList());
                }
            }

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
            // Get the product to delete
            var productToDelete = db.Products.Find(id);

            // Get the related wishlist items
            var wishlistItemsToDelete = db.Wishlists.Where(w => w.product_id == id);

            // Remove the wishlist items
            db.Wishlists.RemoveRange(wishlistItemsToDelete);

            // Delete the product
            db.Products.Remove(productToDelete);

            // Save changes
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
