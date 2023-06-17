using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Comment
    {
        private MultiStoreEntities db;

        public DAL_Comment()
        {
            db = new MultiStoreEntities();
        }


        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public Comment Getbyid(int id)
        {
            return db.Comments.Find(id);
        }

        
        public void Insert(Comment admin)
        {
            db.Comments.Add(admin);
            SaveChanges();
        }

        public IEnumerable<Comment> FindProductComments(int productid)
        {
            var user = db.Comments.Where(p => p.product_id == productid).ToList();
            return user;
        }

        public Comment Find(int id)
        {
            var comment = db.Comments.FirstOrDefault(c => c.review_id == id);
            return comment;
        }


        public void Delete(int id)
        {
            Comment adm = db.Comments.Find(id);
            db.Comments.Remove(adm);
            SaveChanges();

        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }



    }
}
