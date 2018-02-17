using inzCloud.Models.Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcApplication1.Models.Post.Data
{
    public class User_Controller
    {
        public static bool AuthenticateUser(string username, string password)
        {
            // ConfigurationManager class is in System.Configuration namespace
            string CS = ConnectSQL.GetConnectionString();
            // SqlConnection is in System.Data.SqlClient namespace
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAuthenticateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // FormsAuthentication is in System.Web.Security
                string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                // SqlParameter is in System.Data namespace
                SqlParameter paramUsername = new SqlParameter("@UserName", username);
                SqlParameter paramPassword = new SqlParameter("@Password", EncryptedPassword);//we are not using authentiacated password ,use EncryptedPassword to use authenticated password 

                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassword);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                return ReturnCode == 1;
            }
        }


        public static bool UserRegister(User _user)
        {


            string CS = ConnectSQL.GetConnectionString();
            // SqlConnection is in System.Data.SqlClient namespace
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spRegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter username = new SqlParameter("@UserName", _user.Username);
                // FormsAuthentication calss is in System.Web.Security namespace
                string encryptedPassword = FormsAuthentication.
                    HashPasswordForStoringInConfigFile(_user.Password, "SHA1");
                SqlParameter password = new SqlParameter("@Password", encryptedPassword);
                SqlParameter email = new SqlParameter("@Email", _user.EmailID);
                SqlParameter Category = new SqlParameter("@Category", _user.Category);

                cmd.Parameters.Add(username);
                cmd.Parameters.Add(password);
                cmd.Parameters.Add(email);
                cmd.Parameters.Add(Category);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                return ReturnCode == 1;
            }

        }

        public static List<User> UserAllUser(int FileId)
        {
            List<User> userlist = new List<User>();
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteProcedure("SpFileAccess");
                cmd.Parameters.AddWithValue("@FileId", FileId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        User user = new User();
                        user.UserID = (int)rdr["UserID"];
                        user.Username = rdr["Username"].ToString();
                        user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                        userlist.Add(user);

                    }
                }
                else
                {
                    rdr.Dispose();
                    cmd.Dispose();
                    cmd = ConnectSQL.ExecuteCommand("select UserID,Username,Rowstatus from inz_USERS");
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.UserID = (int)rdr["UserID"];
                            user.Username = rdr["Username"].ToString();
                            user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                            userlist.Add(user);

                        }
                    }
                }



                return userlist;
            }
            catch
            {
                return userlist;
            }


        }

        public static List<User> getAllUsers()
        {
            List<User> userlist = new List<User>();
            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from inz_USERS Where Rowstatus='A'", con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.UserID = (int)rdr["UserID"];
                            user.Username = rdr["Username"].ToString();
                            user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                            user.EmailID = rdr["EmailID"].ToString();
                            userlist.Add(user);

                        }
                    }




                    return userlist;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }




        }
        public List<User> ListUsers()
        {
            List<User> _listUsers = new List<User>();
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("select EmailID,Rowstatus from inz_USERS");

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User _user = new User();
                    _user.EmailID = rdr["EmailID"].ToString();
                    _user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                    _listUsers.Add(_user);

                }


                return _listUsers;
            }
            catch
            {
                return _listUsers;
            }
        }




        public static int GetUserID(string USername)
        {
            int Userid = 0;
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("select UserId from inz_USERS Where Username=@Username");
                cmd.Parameters.AddWithValue("@Username", USername);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Userid = (int)rdr["UserId"];

                }


                return Userid;
            }
            catch
            {
                return Userid;
            }
        }

        public bool UserDisableEnable(string Email)
        {
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("SpUserEnableDisable");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public List<User> GetPartnersbyFileId(int FileId)
        {

            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    List<User> ListUser = new List<User>();
                    SqlCommand cmd = new SqlCommand("GetPartners",con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FileId", FileId);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        User userObj = new User()
                        {
                            Username = rdr["Username"].ToString(),
                            EmailID = rdr["EmailID"].ToString(),
                            Rowstatus =int.Parse(rdr["Isdeleted"].ToString())==0?'A':'D',
                            UserID=int.Parse(rdr["UserId"].ToString())
                        };
                      
                        ListUser.Add(userObj);

                    }

                    return ListUser;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }


        }

        internal static int GetUserIDByEmailId(string EmailID)
        {
            int Userid = 0;
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("select UserId from inz_USERS Where EmailID=@EmailID");
                cmd.Parameters.AddWithValue("@EmailID", EmailID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Userid = (int)rdr["UserId"];

                }


                return Userid;
            }
            catch
            {
                return Userid;
            }
        }
    }
}