using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_SecurityAnswer
    {

        private MultiStoreEntities db;

        public DAL_SecurityAnswer()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<SecurityAnswer> GetAll()
        {
            return db.SecurityAnswers.ToList();
        }

        public SecurityAnswer Getbyid(int id)
        {
            return db.SecurityAnswers.Where(c => c.user_id == id).FirstOrDefault();
        }


        public void Insert(SecurityAnswer admin)
        {
            db.SecurityAnswers.Add(admin);
            SaveChanges();
        }

        public SecurityAnswer FindUser(string email, int quesid)
        {
            var user = db.SecurityAnswers.FirstOrDefault(p => p.User.email == email && p.question_id == quesid);
            return user;
        }

        public void Update(SecurityAnswer admin)
        {
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            SecurityAnswer adm = db.SecurityAnswers.Find(id);
            db.SecurityAnswers.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

    }
}
