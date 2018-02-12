using MvcApplication1.Models.Post.Application;
using MvcApplication1.Models.Post.Data;
using MvcApplication1.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            PostService ObjPostService = new PostService();
            List<InzPost> ListPost = new List<InzPost>();

            ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if (ViewBag.UserName == "admin")
            {
                ListPost = ObjPostService.GetOwnerAndSharedPost(0);
            }
            else
            {
                ListPost = ObjPostService.GetOwnerAndSharedPost(USerConfig.GetUserID());
            }



           
      
            return View(ListPost.OrderBy(x => x.PostId).ToList());

        }

        [Authorize]
        public ActionResult SharePost(int PostId)
        {
            PostService ObjPostService = new PostService();
            ObjPostService.SharePost(PostId, USerConfig.GetUserID());
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult RejectPost(int PostId)
        {
            PostService ObjPostService = new PostService();
            ObjPostService.RejectPost(PostId, USerConfig.GetUserID());
            return RedirectToAction("Index");
        }
          // GET: Home
        [Authorize]
        [HttpGet]
        public ActionResult Category()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Category(FormCollection frmcollection)
        {
            PostService ObjPostService = new PostService();
           TempData["AlertMessage"]=ObjPostService.AddcateGory(frmcollection[0].ToString());
            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult AllCategory()
        {
            PostService ObjPostSericeLayer = new PostService();
            ViewBag.Categories = ObjPostSericeLayer.GetCategories();
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteCategory(string Category)
        {
            PostService ObjPostService = new PostService();
            TempData["AlertMessage"] = ObjPostService.DeletecateGory(Category);
            return RedirectToAction("AllCategory");
        }
    }
}
