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

namespace test.Controllers
{
    public class BandController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        [ChildActionOnly]
        public ActionResult AllBands()
        {
            return PartialView("_BandListPartial", BandUtil.BandModels(true));
        }

        //
        // GET: /Band/Bands

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

        [Authorize]
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

                    // After registering, we send them to their new band's dashboard
                    return RedirectToAction("Index", "Dashboard", new { bandId = bandId } );
                }
            }

            // If we got this far, something failed, redisplay form
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

        [Authorize]
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

        [Authorize]
        public ActionResult Join(int bandId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            return View();
        }

        //
        // POST: /Band/Join

        [Authorize]
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
        // Get: /Band/Manage

        [Authorize]
        public ActionResult Manage(int bandId, ManageMessageId? message)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeBandNameSuccess ? "Your band name has been changed."
                : "";

            ViewBag.BandId = bandProfile.BandId;
            ViewBag.BandName = bandProfile.BandName;
            return View();
        }

        //
        // POST: /Band/ChangeBandName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandName(int bandId, ChangeBandNameModel model)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "Invalid band ID (not in database)";
                return View("Status");
            }

            BandUtil.ChangeBandName(bandId, model.BandName);

            return RedirectToAction("Manage", new { bandId = bandId, Message = ManageMessageId.ChangeBandNameSuccess } );
        }

        //
        // POST: /Band/ChangeBandPassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBandPassword(int bandId, BandPasswordModel model)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "Invalid band ID (not in database)";
                return View("Status");
            }

            BandUtil.ChangeBandPassword(bandId, model.NewPassword);

            return RedirectToAction("Manage", new { bandId = bandId, Message = ManageMessageId.ChangePasswordSuccess });
        }

        [Authorize]
        public ActionResult Delete(int bandId)
        {
            List<BandMembership> bandMembershipList;
            // Load the current band profile by id
            BandProfile bandProfile = database.BandProfiles.Find(bandId);

            if (bandProfile == null)
            {
                // Could not find the band. This shouldn't happen
                ViewBag.StatusMessage = "Unexpected Error: Could not delete profile.";
                return View("Status");
            }
            else
            {
                // Delete the band
                database.BandProfiles.Remove(bandProfile);
                bandMembershipList = database.BandMemberships.Where(m => m.BandId == bandId).ToList();
                foreach (BandMembership bm in bandMembershipList)
                {
                    database.BandMemberships.Remove(bm);
                }
                database.SaveChanges();

                return View("~/Views/Home/About.cshtml");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeBandNameSuccess
        }
    }
}