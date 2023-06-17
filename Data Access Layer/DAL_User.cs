using Object_Layer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_User
    {
        private MultiStoreEntities db;

        public DAL_User()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public User Getbyid(int id)
        {
            return db.Users.Find(id);
        }


        public void Insert(User admin)
        {
            db.Users.Add(admin);
            SaveChanges();
        }

        public User FindUser(string email , string password)
        {
            var user = db.Users.FirstOrDefault(p => p.email == email && p.password == password);
            return user;
        }

        public User FindUserbyEmail(string email)
        {
            var user = db.Users.FirstOrDefault(p =>p.email == email);
            return user;
        }


        public void Update(User admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            User adm = db.Users.Find(id);
            db.Users.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
