using System;
using System.Collections.Generic;
using System.Linq;
using test.Models;
using test.Models.Dashboard;
using WebMatrix.WebData;
using test.Models.Account;

namespace test.Stuff
{
    public class PostNotFoundException : System.Exception
    {
        public PostNotFoundException() : base("A post with this ID does not an exist.") { }
        public PostNotFoundException(string message) : base(message) { }
    }

    public class MessageBoardUtil
    {
        public static void AddMessagePost(int bandId, string content)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                AddPost(bandId, PostType.Message, content, database);
            }
        }

        public static void AddJoinPost(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                string content = database.UserProfiles.Find(WebSecurity.CurrentUserId).DisplayName + " joined the band.";
                AddPost(bandId, PostType.Join, content, database);
            }
        }

        public static void AddLeavePost(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                string content = database.UserProfiles.Find(WebSecurity.CurrentUserId).DisplayName + " left the band.";
                AddPost(bandId, PostType.Leave, content, database);
            }
        }

        public static void AddFilePost(int bandId, int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                string content = database.UserProfiles.Find(WebSecurity.CurrentUserId).DisplayName + " uploaded a file.";
                AddPost(bandId, PostType.File, content, database);
            }
        }

        private static void AddPost(int bandId, PostType type, string content, DatabaseContext database)
        {
            MessageBoardPost post = new MessageBoardPost(bandId, WebSecurity.CurrentUserId,
                            PostType.Leave, DateTime.UtcNow, content);

            database.MessageBoardPosts.Add(post);
            database.SaveChanges();
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
                              select new { p.PostId, p.PostType, p.PostTime, p.Content, u.UserId, u.DisplayName };

 
                List<MessageBoardPostModel> postModels = new List<MessageBoardPostModel>();

                foreach (var result in results)
                {
                    MessageBoardPostModel postModel = new MessageBoardPostModel(result.PostId, result.UserId, result.DisplayName,
                                                                                    (PostType)result.PostType, result.PostTime, result.Content);

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