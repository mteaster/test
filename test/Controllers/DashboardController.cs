
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
            BandProfile bandProfile = database.BandProfiles.Find(bandId);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "band doesn't exist";
                return View("Status");
            }

            if (!BandManager.UserInBand(WebSecurity.CurrentUserId, bandId))
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

            // If we got this far, something failed, redisplay form
            ViewBag.StatusMessage = "something was wrong with your message";
            return View("Status");
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }
    }
}
