using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DAL_Location
    {

        MultiStoreEntities db;

        public DAL_Location()
        {
            db = new MultiStoreEntities();
        }

        public IEnumerable<Location> GetAll()
        {
            return db.Locations.ToList();
        }

        public void Insert(Location admin)
        {
            db.Locations.Add(admin);
            SaveChanges();
        }


        public void SaveChanges()
        {
            db.SaveChanges();
        }


    }
}
