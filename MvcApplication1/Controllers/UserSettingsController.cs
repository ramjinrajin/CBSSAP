using MvcApplication1.Models.Post.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class UserSettingsController : Controller
    {
        //
        // GET: /UserSettings/

        [HttpGet]
        public ActionResult Index()
        {
            if(USerConfig.GetUserName()=="admin")
            {
                User_Controller _USerDatalayer = new User_Controller();
                List<User> ListUser = _USerDatalayer.ListUsers();
                return View(ListUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
  
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Delete()
        {
            User_Controller _USerDatalayer = new User_Controller();
            bool Result = _USerDatalayer.UserDisableEnable(Request.Form["Email"].ToString());
            if (Result)
            {
                TempData["AlertMessage"] = "Sucess";
            }
            else
            {
                TempData["AlertMessage"] = "Error";
            }
            return RedirectToAction("Index");
        }

    }
}
