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

            dvm.DisplayMessagesModel = MessageBoardUtil.MessagesFor(bandId);

            return View();
        }

        //
        // POST: /Dashboard/

        [HttpPost]
        public ActionResult Index(int bandId, PostMessageModel model)
        {
            DashboardViewModel dvm = new DashboardViewModel();
            dvm.PostMessageModel = model;

            if (ModelState.IsValid)
            {
                MessageBoardUtil.AddMessage(bandId, model.Content);
                ViewBag.StatusMessage = "message posted successfully";
            }
            else
            {
                ViewBag.StatusMessage = "something was wrong with your message";
            }

            dvm.DisplayMessagesModel = MessageBoardUtil.MessagesFor(bandId);

            return View(dvm);
        }
    }
}
