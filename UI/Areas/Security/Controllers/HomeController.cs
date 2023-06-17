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
        private DAL_SecurityAnswer answerdb;

        public HomeController() 
        {
            customerdb= new DAL_User();
            answerdb= new DAL_SecurityAnswer();
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
        public ActionResult RegisterUser(string username, string email, string password, string confpassword, int securityQuestion, string securityAnswer)
        {
            if (confpassword == password)
            {
                GlobalVariable.username = username;
                GlobalVariable.fullname = email;

                Object_Layer.User newcus = new Object_Layer.User();
                newcus.name = username;
                newcus.email = email;
                newcus.password = password;
                newcus.created_at = DateTime.Now;
                newcus.role = "customer";

                customerdb.Insert(newcus);

                SecurityAnswer obj = new SecurityAnswer();
                obj.answer = securityAnswer;
                obj.question_id = securityQuestion;
                obj.user_id = newcus.id;
                obj.created_at = DateTime.Now;

                answerdb.Insert(obj);

                return RedirectToAction("Index", "Login", new { controller = "Home", area = "Security" });
            }
            else
            {
                ViewBag.Message = "The passwords don't match.";
                return View();
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public ActionResult ForgotPassword(string email, int securityQuestion, string securityAnswer)
        {

            var question = customerdb.FindUserbyEmail(email);

            if (question != null && question.role != "admin")
            {

                    Session["forgotuser"] = question.id;

                    return RedirectToAction("NewPassword", "Home", new { controller = "Home", area = "Security" });
            }

            ViewBag.Message = "No such user found!.";
            return View();

        }


        public ActionResult NewPassword()
        {

            var sessionUser = Session["forgotuser"];

            if (sessionUser == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Security", controller = "Login" });
            }

            return View();
        }


        [HttpPost]
        public ActionResult NewPassword(string password, string confpassword)
        {
            var sessionUser = Session["forgotuser"];

            if (sessionUser == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Security", controller = "Login" });
            }

            if (password == confpassword) { 
                var original = customerdb.Getbyid(Convert.ToInt32(sessionUser));

                if (original != null)
                {
                    original.password = password;
                    customerdb.Update(original);

                    return RedirectToAction("Index", "Login", new { area = "Security", controller = "Login" });
                }

                ViewBag.Message = "Error while saving the new password";
                return View();
            }
            else
            {
                ViewBag.Message = "The Passwords don't match!";
                return View();
            }


        }


    }
}