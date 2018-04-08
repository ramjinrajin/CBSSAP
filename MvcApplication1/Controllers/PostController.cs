using MvcApplication1.Models.File;
using MvcApplication1.Models.Post.Application;
using MvcApplication1.Models.Post.Data;
using MvcApplication1.Models.Property;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class PostController : Controller
    {
        PostService ObjPostSericeLayer = new PostService();
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Categories = ObjPostSericeLayer.GetCategories();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(FormCollection frmCollection, HttpPostedFileBase files)
        {

            InzPost _post = new InzPost
            {
                Title = frmCollection["PostTitle"].ToString(),
                SubDescription = frmCollection["SubDescription"].ToString(),
                Category = frmCollection["Category"].ToString(),
                Description = frmCollection["Description"],
                IsPublic = frmCollection["Privacy"].ToString()=="Public"?1:0,
                ReferenceURL = frmCollection["ReferenceURL"].ToString(),
                UserId = USerConfig.GetUserID(),
                FileName=files.FileName.ToString()
            };

            if (files != null)
            {
                if (files.ContentLength > 0)
                {


                    var fileName = Path.GetFileName(files.FileName);

                    var path = Path.Combine(Server.MapPath("~/PostImage"), fileName);
                    files.SaveAs(path);
                 
                }

            }


            Dictionary<int, string> Response = ObjPostSericeLayer.SavePost(_post);
        
       
            string getResponse="";
            int GetPostIdAsReturnCode=0;
            foreach (var pair in Response)
            {
                GetPostIdAsReturnCode = pair.Key;
                getResponse = pair.Value;
            }

             FileDataLayer objFileDataLayer = new FileDataLayer();
             objFileDataLayer.SaveFile(USerConfig.GetUserID(), GetPostIdAsReturnCode);

            TempData["AlertMessage"] = getResponse;
            ViewBag.Categories = ObjPostSericeLayer.GetCategories();
            if (GetPostIdAsReturnCode != -1 || GetPostIdAsReturnCode != -2)
            {
                InzPost SavedPost = new InzPost();
                SavedPost.PostId = GetPostIdAsReturnCode;
                return View();
            }
            else
            {

                return View();
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult ManagePost()
        {
            ViewBag.Categories = ObjPostSericeLayer.GetCategories();
            List<InzPost> ListPost = new List<InzPost>();
            ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if (ViewBag.UserName == "admin")
            {
                ListPost = ObjPostSericeLayer.GetPostDetails(0);
            }
            else
            {
                ListPost = ObjPostSericeLayer.GetPostDetails(USerConfig.GetUserID());//User should be passed
            }
            return View(ListPost.OrderBy(x => x.PostId).ToList());
        }


        //Update
        [HttpPost]
        [Authorize]
        public ActionResult ManagePost(FormCollection frmCollection)
        {
            InzPost _post = new InzPost();
            _post.PostId = Convert.ToInt32(frmCollection["PostId"]);
            _post.Title = frmCollection["Title"].ToString();
            _post.SubDescription = frmCollection["SubDescription"].ToString();
            _post.Category = frmCollection["Category"].ToString();
            _post.Description = frmCollection["Description"];
            _post.IsPublic = frmCollection["Privacy"].ToString() == "Public" ? 1 : 0;
            _post.ReferenceURL = frmCollection["ReferenceURL"].ToString();

            string getResponse = ObjPostSericeLayer.UpdatePost(_post);
            TempData["AlertMessage"] = getResponse;
            TempData["PostId"] = getResponse;

            List<InzPost> ListPost = new List<InzPost>();

            ListPost = ObjPostSericeLayer.GetPostDetails(USerConfig.GetUserID());
            return View(ListPost.OrderBy(x => x.PostId).ToList());
        }


        [HttpGet]
        [Authorize]
        public ActionResult DeletePost(int PostId)
        {
            string getResponse = ObjPostSericeLayer.DeletePost(PostId);
            TempData["AlertMessage"] = getResponse;
            return RedirectToAction("ManagePost");
        }

        [HttpGet]
        [Authorize]
        public ActionResult SuggestAction()
        {
            InzPost SavedPost = new InzPost();
            return View(SavedPost);
        }

        [HttpPost]
        [Authorize]
        [ActionName("SuggestAction")]
        public ActionResult SuggestActionPost()
        {
             int PostId = Convert.ToInt32(Request.Form["PostId"]);
            if(PostId==0)
            {
                PostId = Convert.ToInt32(TempData["PostId"]);
            }
             string SuggestFriend = Request.Form["Email"].ToString();
             string getResponse = ObjPostSericeLayer.SuggestPost(SuggestFriend,PostId);
            TempData["AlertMessage"] = getResponse;
            InzPost SavedPost = new InzPost();
            SavedPost.PostId = PostId;
            return View(SavedPost);
        }
    }
}
