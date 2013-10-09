using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace test.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewData["Message"] = "youre logged in";
            }
            else
            {
                ViewData["Message"] = "youre NOT logged in";
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
