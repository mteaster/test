﻿using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using test.Models.Band;
using test.Stuff;
using WebMatrix.WebData;
using System.Collections.Generic;
using test.Models;

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

        public ActionResult GetBands()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                List<SuperBandModel> models = new List<SuperBandModel>();

                var profiles = from m in database.BandMemberships
                              join p in database.BandProfiles
                              on m.BandId equals p.BandId
                              where m.MemberId == WebSecurity.CurrentUserId
                              select p;

                foreach (var profile in profiles)
                {
                    using (DatabaseContext database2 = new DatabaseContext())
                    {
                        string username = database2.UserProfiles.Find(profile.CreatorId).UserName;
                        models.Add(new SuperBandModel(profile.BandId,
                                            profile.BandName,
                                            username));
                    }
                }

                return Json(models, JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult SuperSearch()
        {
            return View();
        }

        public ActionResult SearchBands(string criteria)
        {
            return Json(BandUtil.SearchByName(criteria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profile(int bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandNameFor(bandId);

            return View();
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
                string path = Server.MapPath("~/App_Data/BandAvatars/" + bandId + ".jpg");
                file.SaveAs(path);
                TempData["SuccessMessage"] = "Avatar changed.";
            }

            return RedirectToAction("Manage");
        }

        public ActionResult DownloadAvatar(int bandId)
        {
            string path = Server.MapPath("~/App_Data/BandAvatars/" + bandId + ".jpg");

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