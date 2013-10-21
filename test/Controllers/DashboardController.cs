using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace band.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            ViewBag.Message = getMsg();
            return View();
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }



    }
}
