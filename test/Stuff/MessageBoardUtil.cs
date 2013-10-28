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
    public class MessageBoardUtil
    {
        public static void AddMessage(int bandId, string content)
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

        public static List<MessageBoardPostModel> MessagesFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var results = from p in database.MessageBoardPosts
                              join u in database.UserProfiles
                              on p.PosterId equals u.UserId
                              where p.BandId == bandId
                              select new { p.PostId, p.PostTime, p.Content, u.DisplayName };

                List<MessageBoardPostModel> postModels = new List<MessageBoardPostModel>();

                foreach (var result in results)
                {
                    MessageBoardPostModel postModel = new MessageBoardPostModel();
                    postModel.PostId = result.PostId;
                    postModel.PostTime = result.PostTime;
                    postModel.Content = result.Content;
                    postModel.PosterName = result.DisplayName;

                    postModels.Add(postModel);
                }

                return postModels;
            }
        }
    }
}