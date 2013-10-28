using System;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using test.Models;
using WebMatrix.WebData;
using System.Data;

namespace test.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "you're not an admin, idiot";
                return View("Error");
            }

            return View();
        }

        public ActionResult Log()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "you're not an admin, idiot";
                return View("Error");
            }


            using (DatabaseContext database = new DatabaseContext())
            {
                return View(database.Logs);

            }
        }
    }
}
