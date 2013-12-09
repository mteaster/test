using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.Dashboard;
using test.Stuff;
using WebMatrix.WebData;

namespace band.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult Index(int bandId)
        {
            return RedirectToAction("Page", new { bandId = bandId, pageNumber = 1 });
        }

        public ActionResult Page(int bandId, int pageNumber)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return RedirectToAction("Join", "Band");
            }

            DashboardViewModel dvm = new DashboardViewModel();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            dvm.DisplayMessagesModel = MessageBoardUtil.GetPage(bandId, pageNumber);

            return View("Index", dvm);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(int bandId, PostMessageModel model)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return RedirectToAction("Join", "Band");
            }

            DashboardViewModel dvm = new DashboardViewModel();

            if (ModelState.IsValid)
            {
                MessageBoardUtil.AddMessagePost(bandId, model.Content);
                ViewBag.SuccessMessage = "Message posted!";
            }
            else
            {
                dvm.PostMessageModel = model;
                ViewBag.ErrorMessage = "We don't like empty messages.";
            }

            dvm.DisplayMessagesModel = MessageBoardUtil.GetPage(bandId, 1);

            return View(dvm);
        }

        public ActionResult DeletePost(int postId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                MessageBoardPost post = database.MessageBoardPosts.Find(postId);

                if (post == null)
                {
                    ViewBag.ErrorMessage = "Post not found.";
                    return View("Error");
                }

                if (!BandUtil.Authenticate(post.BandId, this))
                {
                    return View("Error");
                }

                if (post.PosterId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Administrator"))
                {
                    ViewBag.ErrorMessage = "You can't delete someone else's post.";
                    return View("Error");
                }

                database.MessageBoardPosts.Remove(post);
                database.SaveChanges();

                TempData["SuccessMessage"] = "Post deleted!";

                return RedirectToAction("Index", new { bandId = post.BandId, pageNumber = 1 });
            }
        }

        [HttpPost]
        public ActionResult EditPost(int postId, PostMessageModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                MessageBoardPost post = database.MessageBoardPosts.Find(postId);

                if (post == null)
                {
                    ViewBag.ErrorMessage = "Post not found.";
                    return View("Error");
                }

                if (!BandUtil.Authenticate(post.BandId, this))
                {
                    return View("Error");
                }

                if (post.PosterId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Administrator"))
                {
                    ViewBag.ErrorMessage = "You can't edit someone else's post.";
                    return View("Error");
                }

                post.Content = model.Content;
                database.SaveChanges();

                TempData["SuccessMessage"] = "Post edited!";

                return RedirectToAction("Index", new { bandId = post.BandId, pageNumber = 1 });
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }    
}
