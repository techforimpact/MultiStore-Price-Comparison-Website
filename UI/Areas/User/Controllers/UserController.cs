using Data_Access_Layer;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Object_Layer;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.User
{
    public class UserController : Controller
    {

        private DAL_Product productdb;
        private DAL_Category categorydb;
        private DAL_Store storedb;
        private DAL_Wishlist wishlistdb;
        private DAL_Carousel carouseldb;
        private DAL_CategoryImages categoryimagedb;
        private DAL_StoreImages storeimagedb;

        public UserController()
        {
            productdb = new DAL_Product();
            categorydb = new DAL_Category();
            storedb = new DAL_Store();
            wishlistdb= new DAL_Wishlist();
            carouseldb = new DAL_Carousel();
            categoryimagedb= new DAL_CategoryImages();
            storeimagedb = new DAL_StoreImages();
        }

        // GET: User/User
        public ActionResult Index()
        {

            IndexViewModel model = new IndexViewModel();

            model.Products = productdb.GetAll();
            model.Categories = categorydb.GetAll();
            model.Stores= storedb.GetAll();
            model.Carousel = carouseldb.GetAll();
            model.CategoryImages = categoryimagedb.GetAll();
            model.StoreImages = storeimagedb.GetAll();
            model.FilteredProducts = productdb.GetAll();

            model.FilteredProducts = model.FilteredProducts.Where(p => p.created_at > DateTime.Now.AddMonths(-1));


            List<Category> categories = categorydb.GetAll().ToList();
             
            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);
        }

        public PartialViewResult CategoryDropdown()
        {
            var categories = categorydb.GetAll();
            return PartialView("CategoryDropdown", categories);
        }
    }



    public class IndexViewModel
    {
        public string username { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Carousel> Carousel { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
        public IEnumerable<CategoryImage> CategoryImages { get; set; }
    }
}