using Data_Access_Layer;
using Object_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Security.Controllers
{


    public class GlobalVariable
    {
        public static string username { get; set; }

        public static string fullname { get; set; }
    }

    public class HomeController : Controller
    {

        private DAL_User customerdb;

        public HomeController() 
        {
            customerdb= new DAL_User();
        }


        public ActionResult Index()
        {
            return RedirectToAction("Index" , "Login" , new {controller="Login" , area="Security"});
        }




        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(FormCollection form)
        {
            string name = form["fullname"];
            string username = form["username"];
            string email = form["email"];
            string password = form["password"];
            string confpass = form["confpassword"];

            if (confpass == password)
            {

                GlobalVariable.username = username;
                GlobalVariable.fullname = email;

                // Code to store data in database
                // For example:

                Object_Layer.User newcus = new Object_Layer.User();
                newcus.name = username;
                newcus.email = email;
                newcus.password = password;

                customerdb.Insert(newcus);

                return RedirectToAction("Index", "Login", new { controller = "Home", area = "Security" });

            }
            else
            {
                ViewBag.Message = "The Passwords don't match.";
                return View();
            }
        }

    }
}