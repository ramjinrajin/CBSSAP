using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcApplication1.Models.Post.Data;
 

namespace MvcApplication1.Models.Post.Data
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public string EmailID { get; set; }
        public string mobilenno { get; set; }
        public string description { get; set; }
        public char Rowstatus { get; set; }

        public string Category { get; set; }
    }

    public static class USerConfig
    {
        public static string GetUserName()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        public static int GetUserID()
        {
         
            return User_Controller.GetUserID(System.Web.HttpContext.Current.User.Identity.Name);

        }

        public static int GetUserByEmailId(string Email)
        {
            return User_Controller.GetUserIDByEmailId(Email);
        }

        public static int GetUserID(string UserName)
        {

            return User_Controller.GetUserID(UserName);

        }

    }
}
