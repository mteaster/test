﻿using System.Linq;
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
    [Authorize]
    public class BandController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult AllBands()
        {
            return PartialView("_BandListPartial", BandUtil.BandModels(true));
        }

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
        // Get: /Band/Manage

        public ActionResult Manage(int bandId, ManageMessageId? message)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            DashboardViewModel dvm = new DashboardViewModel();

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                ViewBag.StatusMessage = "You must be a member of this band to change its preferences.";
                return View("Status");
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
            return View("Status");
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
            return View("Status");
        }

        public ActionResult Delete(int bandId)
        {
            if (BandUtil.Delete(bandId))
            {
                ViewBag.StatusMessage = "Band deleted";
            }
            else
            {
                ViewBag.StatusMessage = "You can't delete this band";
            }

            return View("Status");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeBandNameSuccess
        }
    }
}