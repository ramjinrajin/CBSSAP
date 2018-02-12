using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication1.Models.Post.Data;

namespace MvcApplication1.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Login()
        {
            bool isChecked = false;
            if (Request.Form["remember"] != null)
            {
                isChecked = true;
            }

            if (User_Controller.AuthenticateUser(Request.Form["USer name"].ToString(), Request.Form["password"].ToString()))//Authentication in database
            //if (FormsAuthentication.Authenticate(Request.Form["USer name"].ToString(), Request.Form["password"].ToString()))//Authentication in Web.config
            {
                FormsAuthentication.SetAuthCookie(Request.Form["USer name"].ToString(), isChecked);
                FormsAuthentication.RedirectFromLoginPage(Request.Form["USer name"].ToString(), isChecked);
                ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;
                return RedirectToAction("index", "Home");
            }

            else
            {
                TempData["AlertMessage"] = "Invalid";
                return RedirectToAction("index", "Login");
            }
        }

        [HttpPost]
        public ActionResult RegisterUSer(FormCollection FrmUserDetails)
        {

            User _user = new User();
            _user.Username = FrmUserDetails["username"];
            _user.Password = FrmUserDetails["confirm-password2"];
            _user.EmailID = FrmUserDetails["email"];
            _user.Category = FrmUserDetails["Category"];
            if (User_Controller.UserRegister(_user))
            {
                TempData["AlertMessage"] = "Sucess";
                return RedirectToAction("index", "Login");
            }
            else
            {
                TempData["AlertMessage"] = "Exists";
                return RedirectToAction("index", "Login");
            }


        }

        [Authorize]
        public ActionResult logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");


        }



    }
}
