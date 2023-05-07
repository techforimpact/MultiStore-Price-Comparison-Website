using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Security.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class LoginController : Controller
    {
        private DAL_User userdb;

        public LoginController()
        {
            userdb= new DAL_User();
        }


        // GET: Security/Login
        public ActionResult Index()
        {
            if (Session["AdminUsername"] != null)
            {
                return RedirectToAction("AdminPanel", "Admin", new { controller = "Admin", area = "Admin" });
            }
            
            if (Session["CustomerId"] != null)
            {
                return RedirectToAction("Index", "User", new { controller = "User", area = "User" });
            }

            return View();
        }


        [HttpPost]
        public ActionResult Index(FormCollection form)
        {

            string username = form["username"];
            string password = form["password"];

            // Code to authenticate user
            // For example:

            var admin = userdb.FindUser(username, password);
            if (admin != null) { 

                if (admin.role == "admin")
                {
                    Session["AdminUsername"] = admin.name.ToString();
                    // Redirect to admin area
                    return RedirectToAction("Index", "Admin", new { controller = "Admin", area = "Admin" });
                }
                else
                {
                    var user = userdb.FindUser(username, password);

                    Session["CurrentUser"] = user;
                    Session["dbContext"] = userdb;
                    // Redirect to user area
                    return RedirectToAction("Index", "User", new { controller = "User", area = "User" });
                }
            
            }
            else
            {
                // If user is not found in both tables, display error message
                ViewBag.Message = "Invalid username or password.";
                return View();

            }
        }
    }
}