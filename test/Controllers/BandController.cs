using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class BandController : Controller
    {
        private BandsContext db = new BandsContext();

        //
        // GET: /Band/AllBands/

        public ActionResult AllBands()
        {
            return PartialView("_BandsPartial");
        }
    }
}