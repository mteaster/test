using System.Linq;
using System.Web.Mvc;
using test.Models;
using WebMatrix.WebData;
using System.Web.Security;
using test.Filters;
using test.Stuff;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // TEST ONLY

        // The old "About" page
        public ActionResult Accounts()
        {
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
            return PartialView("_BandListPartial", BandUtil.BandModelsFor(WebSecurity.CurrentUserId));
        }

        // This is where I play around with stuff
        public ActionResult Test()
        {
            ViewBag.StatusMessage = "what am i doing here";
            
            //WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "admin" });
            //Roles.CreateRole("Administrator");
            //Roles.AddUserToRole("admin", "Administrator");

            return View();
        }
    }
}
