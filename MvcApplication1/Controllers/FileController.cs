using MvcApplication1.Models.File;
using MvcApplication1.Models.OTP_Generate;
using MvcApplication1.Models.Post.Application;
using MvcApplication1.Models.Post.Data;
using MvcApplication1.Models.Property;
using MvcApplication1.Models.SMTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class FileController : Controller
    {
        FileDataLayer objFileDataLayer = new FileDataLayer();

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

        [HttpGet]
        public ActionResult Download(int FileId)
        {
            
            try
            {
                int CurrentUserId=USerConfig.GetUserID();
                if (objFileDataLayer.FileUserAccess(FileId,CurrentUserId))
                {

                    int RequestId = objFileDataLayer.GenerateRequest(FileId, CurrentUserId);
                    List<int> PartnerIds = objFileDataLayer.GetPartnerIds(FileId);
                    objFileDataLayer.GenerateOTP(PartnerIds, RequestId);
                    string AccessStatus = objFileDataLayer.AuthorizeOTP(RequestId);
                    User_Controller userData = new User_Controller(); 
                    

                    foreach (var partnerid in PartnerIds)
                    {
                        int GetOtp = userData.GetOtp(RequestId, partnerid);
                        SMTPProtocol.NotifyPartners("Notification", string.Format("You partners is waiting for the file ,Please find the OTP : {0}", GetOtp), userData.GetEmailbyPartnerId(partnerid.ToString()));
                    }

                    if (AccessStatus == "NoUserPending" && AccessStatus!="")
                    {
                        string fileName = objFileDataLayer.GetFileName(FileId);
                        if (fileName != "NIL")
                        {

                            var filepath = Path.Combine(Server.MapPath("~/PostImage"), fileName);
                            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                            string contentType = MimeMapping.GetMimeMapping(filepath);

                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                FileName = fileName,
                                Inline = true,
                            };

                            Response.AppendHeader("Content-Disposition", cd.ToString());
                            objFileDataLayer.DeleteRequest(RequestId);
                            return File(filedata, contentType);
                        }
                        else
                        {
                            ViewBag.Title = "File not found";
                            ViewBag.Message = "The file you are trying to access is deleted";
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                    
           
                }
                else
                {
                    ViewBag.Title = "Illegal Entry";
                    ViewBag.Message = "You are not authorized to access this file";
                    return View();
                }




            }
            catch
            {
                return View();
            }
        }

    }
}
