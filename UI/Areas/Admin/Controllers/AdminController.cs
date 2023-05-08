using Data_Access_Layer;
using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using System.Text;
using System.Drawing;
using System.IO;
using System.Web.WebSockets;
using System.Web.Optimization;

namespace UI.Areas.Admin.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class AdminController : Controller
    {

        private DAL_Product productdb;
        private DAL_Store storedb;
        private DAL_Category categorydb;
        private DAL_StoreImages storeimagedb;
        private DAL_CategoryImages categoryimagedb;
        private DAL_User userdb;
        private DAL_Carousel carouseldb;
        private DAL_Price pricesdb;


        public AdminController()
        {
            productdb = new DAL_Product();
            storedb = new DAL_Store();
            categorydb = new DAL_Category();
            storeimagedb= new DAL_StoreImages();
            categoryimagedb= new DAL_CategoryImages();
            userdb = new DAL_User();
            carouseldb = new DAL_Carousel();
            pricesdb = new DAL_Price();
        }



        public ActionResult Index()
        {
            if (Session["AdminUserName"] != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

        }


/*        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            GoogleDriveFileRepository.FileUpload(file);
            return RedirectToAction("GetGoogleDriveFiles");
        }*/



        //------------------------ADMIN PANEL CODE----------------------------------------------------------

        public ActionResult AdminPanel()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }
            return View();
        }


        //------------------------CATEGORIES CODE----------------------------------------------------------

        public ActionResult Categories()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            // if model is not valid, show error

            var categories = categorydb.GetAll();

            ViewBag.category_id = new SelectList(categories, "id", "name");

            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                // add category to database
                category.created_at = DateTime.Now;
                categorydb.Insert(category);

                // redirect to category list
                return RedirectToAction("Categories");
            }

            // if model is not valid, show error
            return View(category);
        }


        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var product = categorydb.Getbyid(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            categorydb.Delete(id);
            return RedirectToAction("Categories");
        }



        public ActionResult CategoryDetails(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }


            var categoryModelView = new CategoryViewModel();

            var category = categorydb.Getbyid(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            categoryModelView.CategoryId = category.id;
            categoryModelView.Name = category.name;
            categoryModelView.ParentId = category.parent_id;
            categoryModelView.Products = productdb.GetCategoryProducts(id);

            return View(categoryModelView);

        }


        //------------------------STORES CODE----------------------------------------------------------

        [HttpGet]
        public ActionResult Stores(string searchString)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }


            var products = storedb.GetAll();

            if (products != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.name.Contains(searchString)).ToList();
                }

                return View(products);
            }

            return View(new List<Store>());
        }


        public ActionResult AddStoreProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStoreProduct(Product newproduct)
        {

            return View();
        }

        public ActionResult StoreDetails(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }


            var storeViewModel = new StoreViewModel();

            var store = storedb.Getbyid(id);

            if (store == null)
            {
                return HttpNotFound();
            }

            storeViewModel.StoreId = store.id;
            storeViewModel.Name = store.name;
            storeViewModel.Address = store.address;
            storeViewModel.City = store.city;
            storeViewModel.State = store.state;
            storeViewModel.Country = store.country;
            storeViewModel.zipcode = store.zip_code;
            storeViewModel.Products = pricesdb.GetAllByStore(id);


            return View(storeViewModel);
        }


        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteStore")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStore(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var product = storedb.Getbyid(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            storedb.Delete(id);
            return RedirectToAction("Stores");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStore(Store category , HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                StoreImage image = new StoreImage();

                byte[] imageBytes = null;
                using (var binaryReader = new BinaryReader(Image.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(Image.ContentLength);
                }

                image.imageurl = imageBytes;

                category.StoreImages.Add(image);
                // add category to database
                category.created_at = DateTime.Now;
                storedb.Insert(category);

                // redirect to category list
                return RedirectToAction("Stores");
            }

            // if model is not valid, show error
            return View(category);
        }



        //------------------------PRODUCTS CODE----------------------------------------------------------


        public ActionResult ProductDetail(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }


            var prod = productdb.Getbyid(id);
            if (prod == null)
            {
                return HttpNotFound();
            }

            ProductDetailModel model = new ProductDetailModel();

            model.Product = prod;
            model.Prices = pricesdb.GetAllByProduct(id);
            model.StoreImages = storeimagedb.GetAll();

            List<Category> categories = categorydb.GetAll().ToList();

            ViewBag.Categories = categorydb.GetSubcategories(categories);


            return View(model);
        }


        public ActionResult Products(string searchString)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }


            var products = productdb.GetAll();

            if (products != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.name.Contains(searchString)).ToList();
                }

                return View(products);
            }

            return View(new List<Product>());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveModifiedProduct(Product model)
        {
            if (ModelState.IsValid)
            {


                productdb.Update(model);

                // redirect to category list
                return RedirectToAction("Products");
            }

            // if model is not valid, show error
            return View(model);
        }


        public ActionResult AddProduct() { 

            // Retrieve a list of categories from your database
            var categories = categorydb.GetAll();
            var stores = storedb.GetAll();
            // Create a list of SelectListItem objects for the dropdown list
            var categoryListItems = categories.Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.name
            });


            // Add the categoryListItems to the ViewData dictionary
            ViewData["category_id"] = categoryListItems;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewProduct(AddProductModel model, HttpPostedFileBase Image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] imageBytes = null;
            if (Image != null && Image.ContentLength > 0)
            {

                using (var binaryReader = new BinaryReader(Image.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(Image.ContentLength);
                }
            }

            Product newproduct = new Product();

            newproduct.name = model.product.name;
            newproduct.description = model.product.description;
            newproduct.category_id = model.product.category_id;
            newproduct.image = imageBytes;
            newproduct.created_at = DateTime.Now;
            productdb.Insert(newproduct);

            // redirect to product list
            return RedirectToAction("Products");
        }


        public ActionResult DeleteProduct(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var product = productdb.Getbyid(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            productdb.Delete(id);
            return RedirectToAction("Products");
        }



        // GET: Admin/Edit/5
        public ActionResult EditProduct(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var product = productdb.Getbyid(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var categories = categorydb.GetAll();
            var stores = storedb.GetAll();
            // Create a list of SelectListItem objects for the dropdown list
            var categoryListItems = categories.Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.name
            });

            var storeListItems = stores.Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.name
            });

            // Add the categoryListItems to the ViewData dictionary
            ViewData["category_id"] = categoryListItems;
            ViewData["store_id"] = storeListItems;


            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProduct(Product model , HttpPostedFileBase Image)
        {
            byte[] imageBytes = null;
            using (var binaryReader = new BinaryReader(Image.InputStream))
            {
                imageBytes = binaryReader.ReadBytes(Image.ContentLength);
            }


            model.image = imageBytes;
            productdb.Update(model);

            // redirect to category list
            return RedirectToAction("Products");

        }



        //------------------------USER_MENU CODE----------------------------------------------------------



        public ActionResult User_Menu()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var users = userdb.GetAll();

            return View(users);
        }


        public ActionResult NewUser()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            IEnumerable<string> role = new List<string> { "admin", "customer" };

            var roleList = role.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            });

            // Add the categoryListItems to the ViewData dictionary
            ViewData["user_role"] = roleList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewUser(Object_Layer.User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.created_at = DateTime.Now;
            userdb.Insert(model);

            // redirect to product list
            return RedirectToAction("User_Menu");
        }



        public ActionResult EditUser(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var user = userdb.Getbyid(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            IEnumerable<string> role = new List<string> { "admin", "customer" };

            var roleList = role.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            });

            // Add the categoryListItems to the ViewData dictionary
            ViewData["user_role"] = roleList;


            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUser(Object_Layer.User model)
        {

            userdb.Update(model);

            // redirect to category list
            return RedirectToAction("User_Menu");

        }


        public ActionResult DeleteUser(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var user = userdb.Getbyid(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            userdb.Delete(id);
            return RedirectToAction("User_Menu");
        }


        //--------------------------------FEATURED IMAGES------------------------------------------------


        public ActionResult Featured_Images()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var images = carouseldb.GetAll();

            return View(images);
        }


        public ActionResult AddCarousel()
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            IEnumerable<string> role = new List<string> { "carousel" , "poster1" ,  "poster2"};

            var roleList = role.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            });

            // Add the categoryListItems to the ViewData dictionary
            ViewData["role_list"] = roleList;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewCarousel(AddCarouselImage model, HttpPostedFileBase Image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] imageBytes = null;
            if (Image != null && Image.ContentLength > 0)
            {

                using (var binaryReader = new BinaryReader(Image.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(Image.ContentLength);
                }
            }

            Carousel newimage = new Carousel();

            newimage.heading = model.carousel.heading;
            newimage.sub_heading = model.carousel.sub_heading;
            newimage.carousel_role = model.carousel.carousel_role;
            newimage.imageurl = imageBytes;
            newimage.created_at = DateTime.Now;
            carouseldb.Add(newimage);

            // redirect to product list
            return RedirectToAction("Featured_Images");
        }


        public ActionResult DeleteCarousel(int id)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            var image = carouseldb.GetbyId(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            carouseldb.Delete(id);
            return RedirectToAction("Featured_Images");
        }


        //--------------------------------NAVBAR CODE----------------------------------------------------

        public ActionResult Logout()
        {
            Session.Remove("AdminUsername");
            Session.Abandon();
            return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
        }


    }




    public class StoreViewModel
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string zipcode { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class AddProductModel
    {
        public Product product { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }

    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public IEnumerable<Price> Prices { get; set; }
        public IEnumerable<StoreImage> StoreImages { get; set; }
    }


    public class AddCarouselImage
    {
        public Carousel carousel { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}