﻿
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

        public ActionResult Index(string bandId)
        {
            int bandIdAsInt;

            if (!Int32.TryParse(bandId, out bandIdAsInt))
            {
                ViewBag.StatusMessage = "Invalid band ID (format)";
                return View("Status");
            }

            BandProfile bandProfile = database.BandProfiles.Find(bandIdAsInt);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "band doesn't exist";
                return View("Status");
            }

            if (!BandManager.UserInBand(WebSecurity.CurrentUserId, bandIdAsInt))
            {
                ViewBag.BandId = bandId;
                ViewBag.BandName = bandProfile.BandName;
                return RedirectToAction("Join", "Band", new { bandId = bandId } );
            }

            ViewBag.BandId = bandId;
            ViewBag.Message = getMsg();
            ViewBag.StatusMessage = "hello";
            return View();
        }

        //
        // POST: /Dashboard/Post

        [HttpPost]
        public ActionResult Post(string bandId, PostMessageModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.StatusMessage = "let's pretend you posted a message, even though you didn't";
                return View("Index");
            }

            ViewBag.StatusMessage = "let's pretend you posted a message, even though you didn't";

            // If we got this far, something failed, redisplay form
            return View("Index");
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }
    }
}
