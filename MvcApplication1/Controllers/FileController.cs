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
            //return View(ListPost.OrderBy(x => x.PostId).ToList());

            foreach (var Post in ListPost)
            {
                FileModel fileModel = new FileModel
                {
                    FileId=Post.PostId,Name=Post.FileName,Path=Post.ReferenceURL
                };
                ListFiles.Add(fileModel);
            }


            return View(ListFiles);
        }

    }
}
