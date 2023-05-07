using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Wishlist
    {
        private MultiStoreEntities db;

        public DAL_Wishlist()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<Wishlist> GetAll()
        {
            return db.Wishlists.ToList();
        }

        public Wishlist Getbyid(int id)
        {
            return db.Wishlists.Find(id);
        }

        public IEnumerable<Wishlist> FindWishlist(int userid)
        {
            var products = db.Wishlists.Where(p => p.user_id == userid).ToList();
            return products;
        }


        public void Insert(Wishlist admin)
        {
            db.Wishlists.Add(admin);
            SaveChanges();
        }

        public void Update(Wishlist admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            Wishlist adm = db.Wishlists.Find(id);
            db.Wishlists.Remove(adm);
            SaveChanges();

        }

        public void DeleteObj(Wishlist obj)
        {
            db.Wishlists.Remove(obj);
            SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
