using Data_Access_Layer;
using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;

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


        public AdminController()
        {
            productdb = new DAL_Product();
            storedb = new DAL_Store();
            categorydb = new DAL_Category();
            storeimagedb= new DAL_StoreImages();
            categoryimagedb= new DAL_CategoryImages();
            userdb = new DAL_User();
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
            storeViewModel.Products = productdb.GetStoreProducts(id);


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
               
                image.imageurl = Image.FileName;

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
                /*                var fileId = GoogleDriveFileRepository.FileUpload(model.Image);*/

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

            var storeListItems = stores.Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.name
            });

            // Add the categoryListItems to the ViewData dictionary
            ViewData["category_id"] = categoryListItems;
            ViewData["store_id"] = storeListItems;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {

/*                var fileId = GoogleDriveFileRepository.FileUpload(model.Image);*/

                model.product.image = model.Image.FileName;
                //add category to database
                model.product.created_at = DateTime.Now;
                productdb.Insert(model.product);

                // redirect to category list
                return RedirectToAction("Products");
            }

            // if model is not valid, show error
            return View(model.product);
        }


        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
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
            model.image = Image.FileName;
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

        public ActionResult NewUser(Object_Layer.User user)
        {
            if (Session["AdminUserName"] == null)
            {
                return RedirectToAction("Index", "Login", new { controller = "Login", area = "Security" });
            }

            if(ModelState.IsValid)
            {
                user.created_at = DateTime.Now;

                userdb.Insert(user);

                return RedirectToAction("User_Menu");
            }

            return View();
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
}