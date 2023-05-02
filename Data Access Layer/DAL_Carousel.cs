using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Data_Access_Layer
{
    public class DAL_Carousel
    {
        private MultiStoreEntities db;

        public DAL_Carousel()
        {
            db = new MultiStoreEntities();
        }

        public IEnumerable<Carousel> GetAll()
        {
            return db.Carousels.ToList();
        }

        public Carousel GetbyId(int id)
        {
            return db.Carousels.Find(id);
        }

        public void Add(Carousel c)
        {
            db.Carousels.Add(c);
            Save();
        }

        public IEnumerable<Carousel> GetbyRole(string s)
        {
            var user = db.Carousels.Where(p => p.carousel_role == s).ToList();
            return user;
        }


        public void Remove(Carousel c)
        {
            db.Carousels.Remove(c);
            Save();
        }

        public void Delete(int id)
        {
            Carousel c = db.Carousels.Find(id);
            db.Carousels.Remove(c);
            Save();
        }

        public void Edit(Carousel c)
        {
            db.Entry(c).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
