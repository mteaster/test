using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web;

namespace test.Controllers
{
    [Authorize]
    public class BandController : Controller
    {
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Bands()
        {
            return PartialView("_BandsPartial", BandUtil.BandModelsFor(WebSecurity.CurrentUserId));
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Members(int bandId)
        {
            return PartialView("_MembersPartial", BandUtil.MemberModelsFor(bandId));
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterBandModel model)
        {
            if (ModelState.IsValid)
            {
                if (BandUtil.IsBandNameTaken(model.BandName))
                {
                    ModelState.AddModelError("", "This band name is already in use.");
                }
                else
                {
                    int bandId = BandUtil.Register(model);
                    return RedirectToAction("Index", "Dashboard", new { bandId = bandId } );
                }
            }

            return View(model);
        }

        public ActionResult Search()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchBandModel model)
        {
            SearchViewModel svm = new SearchViewModel();
            svm.searchModel = model;

            if (ModelState.IsValid)
            {
                svm.resultsModel = BandUtil.SearchByName(model.BandName);
            }

            return View(svm);
        }

        public ActionResult Join(int bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandNameFor(bandId);

            if (BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                ViewBag.ErrorMessage = "You're already in '" + ViewBag.BandName + "'.";
                return View("Error");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Join(int bandId, JoinBandModel model)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandNameFor(bandId);

            if(BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                ViewBag.ErrorMessage = "You're already in '" + ViewBag.BandName + "'.";
                return View("Error");
            }

            if (BandUtil.Join(bandId, model.Password))
            {
                MessageBoardUtil.AddJoinPost(bandId);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "The password you entered is invalid.";
                return View(model);
            }
        }

        public ActionResult Leave(int bandId)
        {
            string bandName = BandUtil.BandNameFor(bandId);

            if (BandUtil.Leave(bandId))
            {
                MessageBoardUtil.AddLeavePost(bandId);
                ViewBag.SuccessMessage = "You left '" + bandName + "'.";
                return View("Success");
            }

            ViewBag.ErrorMessage = "We can't let you leave '" + bandName + "'.";
            return View("Error");
        }

        public ActionResult Manage(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandName(int bandId, ChangeBandNameModel model)
        {
            if (BandUtil.ChangeBandName(bandId, model.BandName))
            {
                TempData["SuccessMessage"] = "Your band's name has been changed.";
            }
            else
            {
                TempData["ErrorMessage"] = "You must be a member of this band to change its name.";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandPassword(int bandId, BandPasswordModel model)
        {
            if (BandUtil.ChangeBandPassword(bandId, model.NewPassword))
            {
                TempData["SuccessMessage"] = "Your band's password has been changed.";
            }
            else
            {
                TempData["ErrorMessage"] = "You must be a member of this band to change its password.";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        public ActionResult Delete(int bandId)
        {
            if (BandUtil.Delete(bandId, Server))
            {
                ViewBag.SuccessMessage = "Your band has been deleted.";
                return View("Success");
            }

            TempData["ErrorMessage"] = "You must be the creator of this band to delete it.";
            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        [HttpPost]
        public ActionResult UploadAvatar(int bandId, HttpPostedFileBase file)
        {
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                ViewBag.ErrorMessage = "You must be a member of this band to change its preferences.";
                return View("Error");
            }

            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandNameFor(bandId);

            if (file.ContentLength <= 0 || file.ContentLength > 1048576)
            {
                TempData["ErrorMessage"] = "Something was wrong with the avatar you uploaded.";
            }
            else
            {
                string path = Server.MapPath("~/App_Data/BandAvatars/" + WebSecurity.CurrentUserId + ".jpg");
                file.SaveAs(path);
                TempData["ErrorMessage"] = "Avatar changed.";
            }

            return RedirectToAction("Manage");
        }

        public ActionResult DownloadAvatar(int userId)
        {
            string path = Server.MapPath("~/App_Data/UserAvatars/" + userId + ".jpg");

            if (System.IO.File.Exists(path))
            {
                return File(path, "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
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