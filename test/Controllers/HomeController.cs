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
        private UsersContext db = new UsersContext();

        public ActionResult Index()
        {
            ViewBag.Message = "welcome to our dumb website";

            return View();
        }

        public ActionResult About()
        {
            return View(db.UserProfiles.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
