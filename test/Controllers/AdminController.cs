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

        public ActionResult Users()
        {
            if (!Roles.IsUserInRole("Administrator"))
            {
                @ViewBag.StatusMessage = "Sorry, you can't access this page.";
                return View("Error");
            }

            return View();
        }

        [ChildActionOnly]
        public ActionResult ManageUsers()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_ManageUsersPartial", database.UserProfiles.ToList());
            }
        }

        [ChildActionOnly]
        public ActionResult UserList()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_UserListPartial", database.UserProfiles.ToList());
            }
        }

        [ChildActionOnly]
        public ActionResult BandList()
        {
            return PartialView("_BandListPartial", BandUtil.BandModels(true));
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
