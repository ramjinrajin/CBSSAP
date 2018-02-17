using MvcApplication1.Models.File;
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
    public class FileController : Controller
    {
        //
        // GET: /File/

        public ActionResult Index()
        {
            List<FileModel> ListFiles = new List<FileModel>();



            List<InzPost> ListPost = new List<InzPost>();
            PostService ObjPostSericeLayer = new PostService();
            ListPost = ObjPostSericeLayer.GetPostDetails(USerConfig.GetUserID());


            foreach (var Post in ListPost)
            {
                FileModel fileModel = new FileModel
                {
                    FileId = Post.PostId,
                    Name = Post.Title,
                    Path = Post.path
                };
                ListFiles.Add(fileModel);
            }


            ViewBag.Users = User_Controller.getAllUsers();

            return View(ListFiles);
        }

        [HttpPost]
        public ActionResult AddPartners(FormCollection frmCollection)
        {
            int FileId = Convert.ToInt32(frmCollection["FileId"]);
            int PartnerId = Convert.ToInt32(frmCollection["PartnerId"]);

            FileDataLayer fileDatalayer = new FileDataLayer();

            FileResponse FileResponse = fileDatalayer.SaveFile(PartnerId, FileId);
            if (FileResponse.Status == FileStatus.Sucess)
            {
                TempData["AlertMessage"] = "Sucess";
            }
            else
            {
                TempData["AlertMessage"] = "Error";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewPartner(int FileId)
        {
            
            User_Controller _USerDatalayer = new User_Controller();
            ViewBag.FileId = FileId;
            List<User> ListUser = _USerDatalayer.GetPartnersbyFileId(FileId);
            return View(ListUser);
        }

        [HttpPost]
        public ActionResult ViewPartner(FormCollection PostData)
        {
            FileDataLayer fileDLayer = new FileDataLayer();
            int FileId = int.Parse(PostData["FileId"]);
            int UserId = int.Parse(PostData["UserId"]);
            bool Response = fileDLayer.UpdatePartnerStatus(FileId, UserId);
            if (Response)
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
