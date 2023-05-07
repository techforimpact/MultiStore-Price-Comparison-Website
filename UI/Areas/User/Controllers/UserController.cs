using Data_Access_Layer;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Object_Layer;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using System.Web.Caching;

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
        private DAL_Price pricesdb;
        private Object_Layer.User currentUser;
/*        private DAL_User userdb;
        private Object_Layer.User sessionUser;*/

        public UserController()
        {
            productdb = new DAL_Product();
            categorydb = new DAL_Category();
            storedb = new DAL_Store();
            wishlistdb = new DAL_Wishlist();
            carouseldb = new DAL_Carousel();
            categoryimagedb = new DAL_CategoryImages();
            storeimagedb = new DAL_StoreImages();
            pricesdb = new DAL_Price();
            currentUser = null;

/*            string customerId = TempData["customerId"].ToString();


            sessionUser = userdb.Getbyid(Convert.ToInt32(customerId));*/

        }
        // GET: User/User
        public ActionResult Index()
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser== null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }


            IndexViewModel model = new IndexViewModel();

            model.Products = productdb.GetAll();
            model.Categories = categorydb.GetAll();
            model.Stores= storedb.GetAll();
            model.Carousel = carouseldb.GetAll();
            model.CategoryImages = categoryimagedb.GetAll();
            model.StoreImages = storeimagedb.GetAll();
            model.FilteredProducts = productdb.GetAll();
            model.Wishlist = wishlistdb.GetAll();

            model.FilteredProducts = model.FilteredProducts.Where(p => p.created_at > DateTime.Now.AddMonths(-1));
            model.User = user;

            List<Category> categories = categorydb.GetAll().ToList();
             
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;



            return View(model);
        }

        public PartialViewResult CategoryDropdown()
        {

            var categories = categorydb.GetAll();
            return PartialView("CategoryDropdown", categories);
        }


        public ActionResult CategoryPage(int categoryId)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }



            var category = categorydb.Getbyid(categoryId);
            if (category == null)
            {
                return RedirectToAction("Index" , "User");
            }

            CategoryViewModel model = new CategoryViewModel();

            model.Category = categorydb.Getbyid(categoryId);
            model.CategoryImage = categoryimagedb.Get(categoryId);
            model.Products = productdb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View(model);
        }


        public ActionResult Products()
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }


            var products = productdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View(products);
        }


        public ActionResult ProductDetail(int id)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }



            var prod = productdb.Getbyid(id);
            if(prod == null)
            {
                return HttpNotFound();
            }

            ProductDetailModelView model = new ProductDetailModelView();

            model.FilteredProducts = productdb.GetAll().Where(p => p.created_at > DateTime.Now.AddMonths(-1));

            model.Product = prod;
            model.Prices = pricesdb.GetAllByProduct(id);

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;


            return View(model);
        }

        public ActionResult SearchProduct(string searchTerm)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }


            var products = productdb.SearchProducts(searchTerm);
            if (products == null)
            {
                return HttpNotFound();
            }

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View(products);

        }


        public ActionResult Shops()
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }



            StoresViewModel model = new StoresViewModel();

            model.Stores = storedb.GetAll();
            
            model.StoreImages = storeimagedb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View(model);
        }




        public ActionResult ShopDetail(int id)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }


            var shop = storedb.Getbyid(id);
            if(shop == null)
            {
                return HttpNotFound();
            }


            StoreDetailModelView model = new StoreDetailModelView();

            model.Store = shop;
            model.Products = productdb.GetStoreProducts(id);



            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View(model);

        }



        public ActionResult Logout()
        {
            Session.Remove("CurrentUser");
            Session.Abandon();
            return RedirectToAction("Index", "Default", new { controller = "Default", area = "Common" });
        }
    }



    public class IndexViewModel
    {
        public Object_Layer.User User { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Carousel> Carousel { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
        public IEnumerable<CategoryImage> CategoryImages { get; set; }
        public IEnumerable<Wishlist> Wishlist { get; set; }
    }

    public class CategoryViewModel
    {
        public Category Category { get; set;}
        public CategoryImage CategoryImage { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class ProductDetailViewModel
    {
        public Product Product { get; set;}
        public IEnumerable<Product> FilteredProducts { get; set; }
    }

    public class StoresViewModel
    {
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
    }


    public class StoreDetailModelView
    {
        public Store Store { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class ProductDetailModelView
    {
        public Product Product { get; set; }
        public IEnumerable<Price> Prices { get; set; }

        public IEnumerable<Product> FilteredProducts { get; set; }
    }
}