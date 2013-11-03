using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;
using WebMatrix.WebData;
using System.Web.Helpers;
using System.Data;

namespace test.Stuff
{
    public class PostNotFoundException : System.Exception
    {
        public PostNotFoundException() : base("A post with this ID does not an exist.") { }
        public PostNotFoundException(string message) : base(message) { }
    }

    public class MessageBoardUtil
    {
        public static void AddPost(int bandId, string content)
        {
            MessageBoardPost post = new MessageBoardPost();
            post.BandId = bandId;
            post.PosterId = WebSecurity.CurrentUserId;
            post.PostTime = DateTime.UtcNow;
            post.Content = content;

            using (DatabaseContext database = new DatabaseContext())
            {
                database.MessageBoardPosts.Add(post);
                database.SaveChanges();
            }
        }

        public static List<MessageBoardPostModel> PostsFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var results = from p in database.MessageBoardPosts
                              join u in database.UserProfiles
                              on p.PosterId equals u.UserId
                              where p.BandId == bandId
                              orderby p.PostTime descending
                              select new { p.PostId, p.PostTime, p.Content, u.UserId, u.DisplayName };

 
                List<MessageBoardPostModel> postModels = new List<MessageBoardPostModel>();

                foreach (var result in results)
                {
                    MessageBoardPostModel postModel = new MessageBoardPostModel();
                    postModel.PostId = result.PostId;
                    postModel.PosterId = result.UserId;
                    postModel.PostTime = result.PostTime;
                    postModel.Content = result.Content.Replace("\n", "<br />");
                    postModel.PosterName = result.DisplayName;

                    postModels.Add(postModel);
                }

                return postModels;
            }
        }

        public static bool DeletePost(int postId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                MessageBoardPost post = PostFor(postId, database);

                if (post.PosterId != WebSecurity.CurrentUserId)
                {
                    return false;
                }

                database.MessageBoardPosts.Remove(post);
                database.SaveChanges();

                return true;
            }
        }

        // todo
        public static bool EditPost(int postId, string content)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                MessageBoardPost post = PostFor(postId, database);
                
                if (post.PosterId != WebSecurity.CurrentUserId)
                {
                    return false;
                }

                post.Content = content;

                database.SaveChanges();

                return true;
            }
        }

        private static MessageBoardPost PostFor(int postId, DatabaseContext database)
        {
            MessageBoardPost post = database.MessageBoardPosts.Find(postId);

            if (post == null)
            {
                throw new PostNotFoundException();
            }

            return post;
        }
    }
}