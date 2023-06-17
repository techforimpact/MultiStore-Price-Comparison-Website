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
using UI.Areas.Security.Controllers;
using Google.Apis.Drive.v3.Data;

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
        private DAL_User userdb;
        private DAL_Location locationdb;
        private Object_Layer.User currentUser;
        private DAL_Comment commentdb;
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
            userdb = new DAL_User();
            locationdb = new DAL_Location();
            currentUser = null;
            commentdb = new DAL_Comment();
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

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Default", new { area = "Common", controller = "Default" });
            }


            IndexViewModel model = new IndexViewModel();

            model.Products = productdb.GetAll();
            model.Categories = categorydb.GetAll();
            model.Stores = storedb.GetAll();
            model.Carousel = carouseldb.GetAll();
            model.CategoryImages = categoryimagedb.GetAll();
            model.StoreImages = storeimagedb.GetAll();
            model.FilteredProducts = productdb.GetAll();
            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Prices = pricesdb.GetAll();

            model.FilteredProducts = model.FilteredProducts.Where(p => p.created_at > DateTime.Now.AddMonths(-1));
            model.User = user;

            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();

            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.WishlistCount = wishcount;
            ViewBag.userid = user.id;


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
                return RedirectToAction("Index", "User");
            }

            CategoryViewModel model = new CategoryViewModel();

            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Category = categorydb.Getbyid(categoryId);
            model.CategoryImage = categoryimagedb.Get(categoryId);
            model.Products = productdb.GetAll();
            model.Prices = pricesdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;


            return View(model);
        }



        [HttpPost]
        public ActionResult CategoryPage(CategoryViewModel model, FormCollection form)
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


            int categoryId = Convert.ToInt32(form["categoryId"]);

            // Retrieve the selected price range from the form collection
            string selectedPriceRange = form["price-range"];

            // Filter the products based on the selected price range
            switch (selectedPriceRange)
            {
                case "price-all":
                    // Show all products
                    model.Products = productdb.GetAll();
                    break;
                case "price-1":
                    // Filter products in the range SAR 0 - 100
                    model.Products = productdb.GetByPriceRange(0, 100);
                    break;
                case "price-2":
                    // Filter products in the range SAR 100 - 200
                    model.Products = productdb.GetByPriceRange(100, 200);
                    break;
                case "price-3":
                    // Filter products in the range SAR 200 - 300
                    model.Products = productdb.GetByPriceRange(200, 300);
                    break;
                case "price-4":
                    // Filter products in the range SAR 300 - 400
                    model.Products = productdb.GetByPriceRange(300, 400);
                    break;
                case "price-5":
                    // Filter products in the range SAR 400 - 500
                    model.Products = productdb.GetByPriceRange(400, 500);
                    break;
                default:
                    // Show all products by default
                    model.Products = productdb.GetAll();
                    break;
            }

            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Category = categorydb.Getbyid(categoryId);
            model.CategoryImage = categoryimagedb.Get(categoryId);
            model.Prices = pricesdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            // Pass the model to the view
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

            SearchProductModelView model = new SearchProductModelView();
            model.Products = productdb.GetAll();
            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Prices = pricesdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.userid = currentUser.id;

            return View(model);
        }

        [HttpPost]
        public ActionResult Products(SearchProductModelView model, FormCollection form)
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

            // Retrieve the selected price range from the form collection
            string selectedPriceRange = form["price-range"];

            // Filter the products based on the selected price range
            switch (selectedPriceRange)
            {
                case "price-all":
                    // Show all products
                    model.Products = productdb.GetAll();
                    break;
                case "price-1":
                    // Filter products in the range SAR 0 - 100
                    model.Products = productdb.GetByPriceRange(0, 100);
                    break;
                case "price-2":
                    // Filter products in the range SAR 100 - 200
                    model.Products = productdb.GetByPriceRange(100, 200);
                    break;
                case "price-3":
                    // Filter products in the range SAR 200 - 300
                    model.Products = productdb.GetByPriceRange(200, 300);
                    break;
                case "price-4":
                    // Filter products in the range SAR 300 - 400
                    model.Products = productdb.GetByPriceRange(300, 400);
                    break;
                case "price-5":
                    // Filter products in the range SAR 400 - 500
                    model.Products = productdb.GetByPriceRange(400, 500);
                    break;
                default:
                    // Show all products by default
                    model.Products = productdb.GetAll();
                    break;
            }


            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Prices = pricesdb.GetAll();


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.userid = currentUser.id;

            // Pass the model to the view
            return View(model);
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
            
            if (prod == null)
            {
                return HttpNotFound();
            }

            ProductDetailModelView model = new ProductDetailModelView();

            model.FilteredProducts = productdb.GetAll().Where(p => p.created_at > DateTime.Now.AddMonths(-1)).Where(c => c.id != id);
            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Product = prod;
            model.Prices = pricesdb.GetAll();
            model.StoreImages = storeimagedb.GetAll();
            model.allcomments = commentdb.FindProductComments(prod.id);
            

            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.userid = currentUser.id;
            ViewBag.CommentCount = model.allcomments.Count();

            return View(model);
        }



        [HttpPost]
        public ActionResult NewComment(ProductDetailModelView model)
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

            // Create a new comment object and populate its properties from the form data
            var comment = new Object_Layer.Comment
            {
                user_id = user.id,
                product_id = model.Product.id,
                review_message = model.comment.review_message,
                review_name = model.comment.review_name,
                review_email = model.comment.review_email
            };

            // Save the comment to the database
            commentdb.Insert(comment);


            // Redirect to the product detail page or perform any other desired action
            return RedirectToAction("ProductDetail", new { id = model.Product.id });
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

            SearchProductModelView model = new SearchProductModelView();

            model.Products = productdb.SearchProducts(searchTerm);
            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Prices = pricesdb.GetAll();


            if (model.Products == null)
            {
                return HttpNotFound();
            }

            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.userid = currentUser.id;

            return View(model);

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
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
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
            if (shop == null)
            {
                return HttpNotFound();
            }


            var locations = locationdb.GetAll().Where(c=> c.store_id == shop.id);

            StoreDetailModelView model = new StoreDetailModelView();

            model.Store = shop;
            model.Products = pricesdb.GetAllByStore(id);
            model.Wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);
            model.Prices = pricesdb.GetAll();

            model.Locations = locations.Select(store => new StoreMapViewModel
            {
                Name = store.Store.name,
                Latitude = store.latitude,
                Longitude = store.longitude
            }).ToList();


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;
            ViewBag.userid = currentUser.id;

            return View(model);

        }

        public ActionResult EditUserPage()
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


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View();
        }




        [HttpPost]
        public ActionResult EditUserPage(FormCollection form)
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


            string email = form["email"];
            string currpassword = form["CurrentPassword"];
            string newpass = form["NewPassword"];
            string confpass = form["ConfirmPassword"];

            if (email == user.email)
            {
                if (currpassword == user.password)
                {
                    if (confpass == newpass)
                    {

                        user.password = newpass;

                        Object_Layer.User obj = user;

                        userdb = (DAL_User)Session["dbContext"];

                        userdb.Update(obj);

                        return RedirectToAction("Index", "User", new { controller = "User", area = "User" });

                    }
                    else
                    {
                        ViewBag.Message = "The Passwords don't match.";
                    }
                }
                else
                {
                    ViewBag.Message = "The Current password is not correct.";
                }
            }
            else
            {
                ViewBag.Message = "The Email is not correct.";
            }



            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlistdb.GetAll().Where(c => c.user_id == user.id).Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;

            return View();
        }


        public ActionResult Wishlist()
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
                return RedirectToAction("Index", "Login", new { area = "Login", controller = "Login" });
            }

            var wishlist = wishlistdb.GetAll().Where(c => c.user_id == user.id);


            List<Category> categories = categorydb.GetAll().ToList();
            var wishcount = wishlist.Count();
            ViewBag.WishlistCount = wishcount;
            ViewBag.Categories = categorydb.GetSubcategories(categories);
            ViewBag.Username = currentUser.name;


            return View(wishlist);
        }

        public ActionResult RemoveWishlist(int id)
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
                return RedirectToAction("Index", "Login", new { area = "Login", controller = "Login" });
            }

            var wishproduct = wishlistdb.GetAll().Where(c => c.product_id == id && c.user_id == user.id);

            wishlistdb.Delete(wishproduct.First().id);


            return RedirectToAction("Wishlist");
        }

        public ActionResult AddToWish(int id)
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
                return RedirectToAction("Index", "Login", new { area = "Login", controller = "Login" });
            }

            string lastPageUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : string.Empty;

            var wish = wishlistdb.GetAll().Where(c => c.user_id == user.id && c.product_id == id);

            if (wish.Count() != 0)
            {
                return Redirect(lastPageUrl);
            }

            Wishlist obj = new Wishlist();

            obj.product_id = id;
            obj.user_id = user.id;
            obj.created_at = DateTime.Now;

            wishlistdb.Insert(obj);

            return Redirect(lastPageUrl);
        }

        public ActionResult RemoveWish(int id)
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
                return RedirectToAction("Index", "Login", new { area = "Login", controller = "Login" });
            }

            string lastPageUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : string.Empty;

            var wish = wishlistdb.GetAll().Where(c => c.user_id == user.id && c.product_id == id);

            if (wish == null)
            {
                return Redirect(lastPageUrl);
            }

            wishlistdb.DeleteObj(wish.First());

            return Redirect(lastPageUrl);

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
        public IEnumerable<Price> Prices { get; set; }
    }

    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public CategoryImage CategoryImage { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Wishlist> Wishlist { get; set; }
        public IEnumerable<Price> Prices { get; set; }

    }

    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Wishlist> Wishlist { get; set; }
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
        public IEnumerable<Wishlist> Wishlist { get; set; }
        public IEnumerable<Price> Prices { get; set; }

        public IEnumerable<StoreMapViewModel> Locations { get; set; }

    }

    public class ProductDetailModelView
    {
        public Product Product { get; set; }
        public IEnumerable<Price> Prices { get; set; }

        public IEnumerable<Product> FilteredProducts { get; set; }
        public IEnumerable<Wishlist> Wishlist { get; set; }

        public IEnumerable<StoreImage> StoreImages { get; set; }

        public IEnumerable<Object_Layer.Comment> allcomments { get; set; }
        public Object_Layer.Comment comment { get; set; }


    }

    public class SearchProductModelView
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Wishlist> Wishlist { get; set; }
        public IEnumerable<Price> Prices { get; set; }

    }

    public class StoreMapViewModel
    {
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}