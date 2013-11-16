using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Dashboard;
using test.Models.Band;

namespace band.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult DeletePost(int postId)
        {
            if (MessageBoardUtil.DeletePost(postId))
            {
                TempData["SuccessMessage"] = "Post deleted!";
            }
            else
            {
                TempData["ErrorMessage"] = "You cannot delete someone else's post!";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath); //Takes you back to the view you came from
        }

        [HttpPost]
        public ActionResult EditPost(int postId, PostMessageModel model)
        {
            if (MessageBoardUtil.EditPost(postId, model.Content))
            {
                TempData["SuccessMessage"] = "Post edited!";
            }
            else
            {
                TempData["ErrorMessage"] = "You cannot edit someone else's post!";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        public ActionResult EditPost(int postId)
        {
            ViewBag.PostId = postId;
            return View();
        }

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

        [HttpPost]
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
