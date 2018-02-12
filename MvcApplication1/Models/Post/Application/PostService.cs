using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Models.Property;
using InzNetworkCorrelation.Models.Post.DataLayer;
using MvcApplication1.Models.Post.Data;


namespace MvcApplication1.Models.Post.Application
{
    public class PostService
    {
        PostDataLayer ObjPostDatalayer = new PostDataLayer();
        public Dictionary<int, string> SavePost(InzPost ObjPost)
        {
            Dictionary<int, string> SavedPost; //= new Dictionary<int, string>();
            int ReturnCode = ObjPostDatalayer.InsertPostToDB(ObjPost);
            if (ReturnCode == -1)
            {
                SavedPost = new Dictionary<int, string>();
                SavedPost.Add(ReturnCode, "AlreadyExists");
                return SavedPost;
            }
            else if (ReturnCode != -1)
            {
                SavedPost = new Dictionary<int, string>();
                SavedPost.Add(ReturnCode, "Sucess");
                return SavedPost;
            }
            else
            {
                SavedPost = new Dictionary<int, string>();
                SavedPost.Add(ReturnCode, "Error");
                return SavedPost;
            }
            //else if (ObjPostDatalayer.InsertPostToDB(ObjPost) != -1)
            //{
            //    return "Sucess";
            //}
            //else if (ObjPostDatalayer.InsertPostToDB(ObjPost) == 0)
            //{
            //    return "Error";
            //}
            //else
            //{
            //    return "Error";
            //}
        }
        public List<InzPost> GetPostDetails(int USerId)
        {

            return ObjPostDatalayer.GetPostDetails(USerId).ToList();
        }

        public string UpdatePost(InzPost ObjPost)
        {
            if (ObjPostDatalayer.UpdatePostToDB(ObjPost) == 1)
            {
                return "Sucess";
            }
            else if (ObjPostDatalayer.UpdatePostToDB(ObjPost) == -1)
            {
                return "AlreadyExists";
            }
            else
            {
                return "Error";
            }
        }

        public string DeletePost(int PostID)
        {
            if (ObjPostDatalayer.DeletePost(PostID))
            {
                return "Post deleted sucessfully!!!";
            }
            else
            {
                return "Error Occured !Unable to delete post.Please try again";
            }
        }

        public List<InzPost> GetOwnerAndSharedPost(int UserId)
        {
            return ObjPostDatalayer.GetAllSharedAndOwnPost(UserId).ToList();
        }

        public List<InzPost> GetOthersPost(int UserId)
        {
            return ObjPostDatalayer.ListOthersPost(UserId).ToList();
        }

        public string AddcateGory(string Category)
        {
            if( ObjPostDatalayer.AddCateGOry(Category))
            {
                return "Sucess";
            }
            else
            {
                return "Error";
            }
        }
        internal bool SharePost(int PostId, int UserId)
        {
            return ObjPostDatalayer.SharePost(PostId, UserId);
        }

        internal string SuggestPost(string SuggestFriend, int PostId)
        {
            int FriendUserId =USerConfig.GetUserByEmailId(SuggestFriend);
            if(FriendUserId==0)
            {
                return "No user registered with the username";
            }

            int Response = ObjPostDatalayer.SuggestFriend(FriendUserId, PostId);
            if (Response == -1)//Precondition failed in SP(not interested Category)
            {
                return "The user is not interested in the category";
            }
            else if (Response == -2)//Own post
            {
                return "You cant suggest your own post !!!";
            }
            else
            {
                return "User suggestion request accepted";
            }
        }



        internal dynamic GetCategories()
        {
            return ObjPostDatalayer.getCategories();
        }

        internal string DeletecateGory(string Category)
        {
            return ObjPostDatalayer.DeleteCategory(Category);
        }

        internal int RejectPost(int PostId, int UserId)
        {
            return ObjPostDatalayer.RejectPost(PostId, UserId);
        }
    }

}