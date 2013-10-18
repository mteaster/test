using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext usersContext = new UsersContext();
        private BandsContext bandsContext = new BandsContext();

        public ActionResult Index()
        {
            ViewBag.Message = "welcome to our dumb website";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View(bandsContext.BandProfiles.ToList());
        }

        public ActionResult AllBands()
        {
            return PartialView("_BandsPartial", bandsContext.BandProfiles.ToList());
        }
    }
}
