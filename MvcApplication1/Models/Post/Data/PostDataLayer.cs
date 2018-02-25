using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using InzNetworkCorrelation.Backend;
using MvcApplication1.Models.Property;

namespace InzNetworkCorrelation.Models.Post.DataLayer
{
    public class PostDataLayer
    {


        public int InsertPostToDB(InzPost ObjPost)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spInsertPostDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(ObjPost.UserId));
                    cmd.Parameters.AddWithValue("@Title", ObjPost.Title.ToString());
                    cmd.Parameters.AddWithValue("@SubDescription", ObjPost.SubDescription.ToString());
                    cmd.Parameters.AddWithValue("@Category", ObjPost.Category.ToString());
                    cmd.Parameters.AddWithValue("@IsPublic", Convert.ToInt32(ObjPost.IsPublic));
                    cmd.Parameters.AddWithValue("@Description", ObjPost.Description.ToString());
                    cmd.Parameters.AddWithValue("@ReferenceURL", ObjPost.ReferenceURL.ToString());
                    cmd.Parameters.AddWithValue("@FileName", ObjPost.FileName.ToString());

                    con.Open();
                    return (int)cmd.ExecuteScalar();


                }

            }
            catch
            {
                return -2;

            }
        }


        public IEnumerable<InzPost> GetPostDetails(int UserId)
        {
            List<InzPost> PostList = new List<InzPost>();
            SqlCommand cmd;
            using (SqlConnection con = new SqlConnection(DBConnect.ConnectionString()))
            {
                con.Open();
                if (UserId == 0)//is admin
                {
                    cmd = new SqlCommand("select * from inz_Post  order by 1 desc", con);
                }
                else
                {
                    cmd = new SqlCommand("select * from inz_Post where UserID=@UserID order by PostId desc", con);
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                }

                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InzPost _post = new InzPost();
                    _post.PostId = (int)rdr["PostId"];
                    _post.Title = rdr["Title"].ToString();
                    _post.SubDescription = rdr["SubDescription"].ToString();
                    _post.Description = rdr["Description"].ToString();
                    _post.Category = rdr["Category"].ToString();
                    _post.ReferenceURL = rdr["ReferenceURL"].ToString();
                    _post.path = rdr["FileName"].ToString();
                    PostList.Add(_post);
                }

            }

            return PostList;

        }


        public IEnumerable<InzPost> GetAllSharedAndOwnPost(int UserId)
        {
            List<InzPost> PostList = new List<InzPost>();
            SqlCommand cmd;
            using (SqlConnection con = new SqlConnection(DBConnect.ConnectionString()))
            {
                con.Open();


                if (UserId == 0)//is admin
                {
                    cmd = new SqlCommand("select * from inz_Post  order by 1 desc", con);
                }
                else
                {
                    cmd = new SqlCommand("select * from inz_Post where   PostID in (select PostID from PostShared where USERID=@UserID) order by 1 desc", con);
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                }




                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        InzPost _post = new InzPost();
                        _post.PostId = (int)rdr["PostId"];
                        _post.Title = rdr["Title"].ToString();
                        _post.SubDescription = rdr["SubDescription"].ToString();
                        _post.Description = rdr["Description"].ToString();
                        _post.Category = rdr["Category"].ToString();
                        _post.IsApproved = Convert.ToInt32(rdr["IsApproved"]);
                        _post.ReferenceURL = rdr["ReferenceURL"].ToString();
                        _post.ListPost = ListOthersPost(UserId);
                        _post.FileName = SetFile(rdr["FileName"].ToString());
                        PostList.Add(_post);
                    }
                }
                else
                {
                    InzPost _post = new InzPost();
                    _post.ListPost = ListOthersPost(UserId);
                    _post.NoPost = true;
                    PostList.Add(_post);
                }


            }

            return PostList;

        }
        private string SetFile(string filename)
        {
            if (filename == "" || !filename.Contains(".jpg") || !filename.Contains(".jpeg") || !filename.Contains(".png"))
            {
                return "ErrorIcon.png";
            }
            else
            {
                return filename;
            }
        }
        public List<InzPost> ListOthersPost(int UserId)
        {
            List<InzPost> PostList = new List<InzPost>();
            SqlCommand cmd;
            using (SqlConnection con = new SqlConnection(DBConnect.ConnectionString()))
            {
                con.Open();
                cmd = new SqlCommand("select * from Inz_post  WHere PostID   IN (select PostID FROM FriendSuggest Where USERID=@UserID and IsAccepted=0)", con);
                // cmd = new SqlCommand("select * from Inz_post  WHere UserID!=@UserID and PostID NOT IN (select PostID FROM PostShared Where USERID=@UserID AND IsDeclined!=1)", con);
                // cmd = new SqlCommand("Select P.PostId,Title,SubDescription,Description,P.Category,P.IsApproved,P.ReferenceURL from inz_Post P  JOIN PostShared S ON S.PostId=P.PostID WHERE S.IsDeclined=0 AND P.UserID=@UserID", con);
                cmd.Parameters.AddWithValue("@UserID", UserId);


                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InzPost _post = new InzPost();
                    _post.PostId = (int)rdr["PostId"];
                    _post.Title = rdr["Title"].ToString();
                    _post.SubDescription = rdr["SubDescription"].ToString();
                    _post.Category = rdr["Category"].ToString();
                    _post.IsApproved = Convert.ToInt32(rdr["IsApproved"]);
                    _post.ReferenceURL = rdr["ReferenceURL"].ToString();
                    _post.FileName = rdr["FileName"].ToString();
                    PostList.Add(_post);
                }

            }

            return PostList;
        }


        public int UpdatePostToDB(InzPost ObjPost)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spUpdatePostDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostId", ObjPost.PostId);
                    cmd.Parameters.AddWithValue("@Title", ObjPost.Title);
                    cmd.Parameters.AddWithValue("@SubDescription", ObjPost.SubDescription);
                    cmd.Parameters.AddWithValue("@Category", ObjPost.Category);
                    cmd.Parameters.AddWithValue("@IsPublic", ObjPost.IsPublic);
                    cmd.Parameters.AddWithValue("@Description", ObjPost.Description);
                    cmd.Parameters.AddWithValue("@ReferenceURL", ObjPost.ReferenceURL);
                    cmd.Parameters.AddWithValue("@ReferenceURL", ObjPost.ReferenceURL);
                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ObjPost.FileName) ? "" : ObjPost.FileName.ToString());

                    con.Open();
                    return (int)cmd.ExecuteScalar();


                }

            }
            catch
            {
                return 0;

            }
        }

        public bool DeletePost(int PostId)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM INZ_POST WHERE POSTID=@PostId", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PostId", PostId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;


                }

            }
            catch
            {
                return false;

            }
        }


        internal bool SharePost(int PostId, int User)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[PostShared] ([PostId],[UserId],[IsShared],[IsDeclined]) SELECT @PostId,@UserId,1,0", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PostId", PostId);
                    cmd.Parameters.AddWithValue("@UserId", User);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    UpdateSuggestion(PostId, User);
                    return true;


                }

            }
            catch
            {
                return false;

            }
        }

        private void UpdateSuggestion(int PostId, int User)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand(" UPDATE  FriendSuggest SET IsAccepted=1 Where POstId=@PostId and UserId=@UserId", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PostId", PostId);
                    cmd.Parameters.AddWithValue("@UserId", User);
                    con.Open();
                    cmd.ExecuteNonQuery();

                }

            }
            catch
            {
                throw;

            }
        }

        internal int SuggestFriend(int UserName, int PostId)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spSuggestFriend", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostId", PostId);
                    cmd.Parameters.AddWithValue("@UserId", UserName);
                    con.Open();
                    return (int)cmd.ExecuteScalar();

                }

            }
            catch
            {
                return 0;

            }
        }

        internal bool AddCateGOry(string Category)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO INZ_CATEGORY (CATEGORY) VALUES (@Category)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Category", Category);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;

                }

            }
            catch
            {
                return false;

            }
        }

        internal dynamic getCategories()
        {
            List<string> Categories = new List<string>();
            SqlCommand cmd;
            using (SqlConnection con = new SqlConnection(DBConnect.ConnectionString()))
            {
                con.Open();



                cmd = new SqlCommand("select CATEGORY from INZ_CATEGORY", con);





                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        Categories.Add(rdr[0].ToString());

                    }
                }


            }

            return Categories;
        }

        internal string DeleteCategory(string Category)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM INZ_CATEGORY WHERE CATEGORY=@Category", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Category", Category);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return "Sucess";

                }

            }
            catch
            {
                return "Error";

            }
        }

        internal int RejectPost(int PostId, int UserId)
        {
            string CS = DBConnect.ConnectionString();

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand(" UPDATE  FriendSuggest SET IsAccepted=-1 Where POstId=@PostId and UserId=@UserId", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PostId", PostId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return 1;
                }

            }
            catch
            {
                return -1;

            }
        }
    }
}