using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;

namespace band.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        DatabaseContext database = new DatabaseContext();

        //
        // GET: /Dashboard/

        public ActionResult Index(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            DashboardViewModel dvm = new DashboardViewModel();

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                //var posts = database.MessageBoardPosts.Where(post => post.BandId == bandId).OrderBy(post => post.PostTime);

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

                dvm.DisplayMessagesModel = postModels;
            }

            ViewBag.Message = getMsg();
            ViewBag.StatusMessage = "hello";
            return View();
        }

        //
        // POST: /Dashboard/

        [HttpPost]
        public ActionResult Index(int bandId, PostMessageModel model)
        {
            if (ModelState.IsValid)
            {
                MessageBoardPost post = new MessageBoardPost();
                post.BandId = bandId;
                post.PosterId = WebSecurity.CurrentUserId;
                post.PostTime = DateTime.Now;
                post.Content = model.Content;

                using (DatabaseContext database = new DatabaseContext())
                {
                    database.MessageBoardPosts.Add(post);
                    database.SaveChanges();
                }
                
                ViewBag.StatusMessage = "message posted successfully";
            }

            // If we got this far, something failed, redisplay form
            ViewBag.StatusMessage = "something was wrong with your message";

            return RedirectToAction("Index");
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }
    }
}
