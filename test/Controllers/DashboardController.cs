using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace band.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index(string bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.Message = getMsg();
            return View();
        }

        [HttpPost]
        public ActionResult Post(string bandId, PostMessageModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.StatusMessage = "let's pretend you posted a message, even though you didn't";
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }



    }
}
