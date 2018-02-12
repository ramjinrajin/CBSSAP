using MvcApplication1.Models.File;
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
            List<FileModel> ListFiles = new List<FileModel>()
            {
                new FileModel
                {
                    FileId=1,Name="Ramjin",Path="sdaasd"
                },
                new FileModel
                {
                    FileId=1,Name="samjin",Path="sdaasd"
                },
                  new FileModel
                {
                    FileId=1,Name="Ramesh",Path="sdaasd"
                }
            };
            return View(ListFiles);
        }

    }
}
