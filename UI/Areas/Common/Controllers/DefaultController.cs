using Data_Access_Layer;
using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Common.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Common/Default
        private DAL_Product productdb;
        private DAL_Category categorydb;
        private DAL_Store storedb;
        private DAL_Wishlist wishlistdb;
        private DAL_Carousel carouseldb;
        private DAL_CategoryImages categoryimagedb;
        private DAL_StoreImages storeimagedb;
        private DAL_Price pricesdb;
        /*        private DAL_User userdb;
                private Object_Layer.User sessionUser;*/

        public DefaultController()
        {
            productdb = new DAL_Product();
            categorydb = new DAL_Category();
            storedb = new DAL_Store();
            wishlistdb = new DAL_Wishlist();
            carouseldb = new DAL_Carousel();
            categoryimagedb = new DAL_CategoryImages();
            storeimagedb = new DAL_StoreImages();
            pricesdb = new DAL_Price();

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
                if(user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }

            IndexViewModel model = new IndexViewModel();

            model.Products = productdb.GetAll();
            model.Categories = categorydb.GetAll();
            model.Stores = storedb.GetAll();
            model.Carousel = carouseldb.GetAll();
            model.CategoryImages = categoryimagedb.GetAll();
            model.StoreImages = storeimagedb.GetAll();
            model.FilteredProducts = productdb.GetAll();
            model.Wishlist = wishlistdb.GetAll();
            model.Prices= pricesdb.GetAll();
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


        public ActionResult CategoryPage(int categoryId)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }


            var category = categorydb.Getbyid(categoryId);
            if (category == null)
            {
                return RedirectToAction("Index", "User");
            }

            CategoryViewModel model = new CategoryViewModel();

            model.Category = categorydb.Getbyid(categoryId);
            model.CategoryImage = categoryimagedb.Get(categoryId);
            model.Products = productdb.GetAll();
            model.Prices = pricesdb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);
        }


        public ActionResult Products()
        {

            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }

            ProductsViewModel model = new ProductsViewModel();

            model.Products = productdb.GetAll();
            model.Prices = pricesdb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);
        }


        public ActionResult ProductDetail(int id)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }


            var prod = productdb.Getbyid(id);
            if (prod == null)
            {
                return HttpNotFound();
            }

            ProductDetailModelView model = new ProductDetailModelView();

            model.FilteredProducts = productdb.GetAll().Where(p => p.created_at > DateTime.Now.AddMonths(-1)).Where(c => c.id != id);
            model.Product = prod;
            model.Prices = pricesdb.GetAll();
            model.StoreImages = storeimagedb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);


            return View(model);
        }

        public ActionResult SearchProduct(string searchTerm)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }

            var products = productdb.SearchProducts(searchTerm);
            if (products == null)
            {
                return HttpNotFound();
            }

            ProductsViewModel model = new ProductsViewModel();
            model.Products = products;
            model.Prices = pricesdb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);

        }


        public ActionResult Shops()
        {

            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }

            StoresViewModel model = new StoresViewModel();

            model.Stores = storedb.GetAll();

            model.StoreImages = storeimagedb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);
        }


        public ActionResult ShopDetail(int id)
        {
            var user = (Object_Layer.User)Session["CurrentUser"];
            // var user = (User)TempData["CurrentUser"];

            // Use the user object to display user data or perform other operations
            if (user != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin", controller = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "User", controller = "User" });
                }

            }


            var shop = storedb.Getbyid(id);
            if (shop == null)
            {
                return HttpNotFound();
            }


            StoreDetailModelView model = new StoreDetailModelView();

            model.Store = shop;
            model.Products = pricesdb.GetAllByStore(id);
            model.Prices = pricesdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);

            return View(model);

        }

        public ActionResult Wishlist()
        {
            return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
        }

    }



    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Carousel> Carousel { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
        public IEnumerable<CategoryImage> CategoryImages { get; set; }
        public IEnumerable<Wishlist> Wishlist { get; set; }
        public IEnumerable<Price> Prices { get; set; }
    }

    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public CategoryImage CategoryImage { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Price> Prices { get; set; }
    }

    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Price> Prices { get; set; }
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
        public IEnumerable<Price> Prices { get; set; }
    }

    public class ProductDetailModelView
    {
        public Product Product { get; set; }
        public IEnumerable<Price> Prices { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
    }

    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set;}
        public IEnumerable<Price> Prices { get; set;}
    }
}
