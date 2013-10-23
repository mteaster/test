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

        public ActionResult Index()
        {
            ViewBag.Message = getMsg();
            return View();
        }

        [HttpPost]
        public ActionResult Post(MessageBoardPost m)
        {
            if (ModelState.IsValid)
            {
                //code to post here ??
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public String getMsg()
        {
            return "Code will go here to query DB for messages";
        }



    }
}
