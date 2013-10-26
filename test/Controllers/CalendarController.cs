using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using test.Stuff;

namespace band.Controllers
{
    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/

        public ActionResult Index(int bandId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            return View();
        }

    }
}
