using System;
using System.Web.Mvc;
using System.Linq;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using test.Models;
using WebMatrix.WebData;
using System.Data;
using test.Stuff;

namespace test.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "Sorry, you can't access this page.";
                return View("Error");
            }

            return View();
        }

        public ActionResult Accounts()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "Sorry, you can't access this page.";
                return View("Error");
            }

            return View();
        }

        // REMOVE ALLOW ANONYMOUS AFTER DEVELOPMENT
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult UserList()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_UserListPartial", database.UserProfiles.ToList());
            }
        }

        // REMOVE ALLOW ANONYMOUS AFTER DEVELOPMENT
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult BandList()
        {
            return PartialView("_BandListPartial", BandUtil.BandModelsFor(WebSecurity.CurrentUserId));
        }

        public ActionResult Logs()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "you're not an admin, idiot";
                return View("Error");
            }

            return View("Error");
        }
    }
}
