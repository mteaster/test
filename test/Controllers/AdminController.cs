using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
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
                @ViewBag.StatusMessage = "Sorry, you can't access this page.";
                return View("Error");
            }

            @ViewBag.StatusMessage = "I didn't actually make the logs work yet.";
            return View("Error");
        }


    }
}
