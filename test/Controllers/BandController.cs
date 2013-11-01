using System.Linq;
using System.Web.Mvc;
using test.Models;
using System.Web.Helpers;
using WebMatrix.WebData;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data;
using test.Stuff;
using System.Web.Security;

namespace test.Controllers
{
    [Authorize]
    public class BandController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        //
        // GET: /Band/Bands

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Bands()
        {
            return PartialView("_BandsPartial", BandUtil.BandModelsFor(WebSecurity.CurrentUserId));
        }

        //
        // GET: /Band/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Band/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterBandModel model)
        {
            if (ModelState.IsValid)
            {
                if (BandUtil.IsBandNameTaken(model.BandName))
                {
                    ModelState.AddModelError("", "band name taken idiot");
                }
                else
                {
                    int bandId = BandUtil.Register(model);
                    return RedirectToAction("Index", "Dashboard", new { bandId = bandId } );
                }
            }

            return View(model);
        }

        //
        // GET: /Band/Search

        public ActionResult Search()
        {
            return View(new SearchViewModel());
        }

        //
        // POST: /Band/Search

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

        //
        // GET: /Band/Join

        public ActionResult Join(int bandId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            return View();
        }

        //
        // POST: /Band/Join

        [HttpPost]
        public ActionResult Join(int bandId, JoinBandModel model)
        {
            ViewBag.BandId = bandId;

            if (BandUtil.Join(bandId, model.Password))
            {
                return RedirectToAction("Index", "Dashboard", new { bandId = bandId } );
            }

            ViewBag.BandName = BandUtil.BandNameFor(bandId);
            ModelState.AddModelError("", "Invalid band password");
            return View(model);
        }

        //
        // GET: /Band/Leave

        public ActionResult Leave(int bandId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            if (BandUtil.Leave(bandId))
            {
                ViewBag.StatusMessage = "You left " + bandProfile.BandName + ".";
                return View("Success");
            }

            ViewBag.StatusMessage = "We can't let you leave " + bandProfile.BandName + ".";
            return View("Error");
        }

        //
        // Get: /Band/Manage

        public ActionResult Manage(int bandId, ManageMessageId? message)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                ViewBag.StatusMessage = "You must be a member of this band to change its preferences.";
                return View("Error");
            }


            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeBandNameSuccess ? "Your band name has been changed."
                : "";

            return View();
        }

        //
        // POST: /Band/ChangeBandName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandName(int bandId, ChangeBandNameModel model)
        {
            if (BandUtil.ChangeBandName(bandId, model.BandName))
            {
                return RedirectToAction("Manage", new { bandId = bandId, Message = ManageMessageId.ChangeBandNameSuccess });
            }

            ViewBag.StatusMessage = "You must be a member of this band to change its name.";
            return View("Error");
        }

        //
        // POST: /Band/ChangeBandPassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandPassword(int bandId, BandPasswordModel model)
        {
            if (BandUtil.ChangeBandPassword(bandId, model.NewPassword))
            {
                return RedirectToAction("Manage", new { bandId = bandId, Message = ManageMessageId.ChangePasswordSuccess });
            }

            ViewBag.StatusMessage = "You must be a member of this band to change its password.";
            return View("Error");
        }

        public ActionResult Delete(int bandId)
        {
            if (BandUtil.Delete(bandId))
            {
                ViewBag.StatusMessage = "Your band has been deleted.";
                return View("Success");
            }

            ViewBag.StatusMessage = "You must be the creator of this band to delete it.";
            return View("Error");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeBandNameSuccess
        }
    }
}