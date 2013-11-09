using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Test;
using System.Web;
using System.IO;

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
            return PartialView("_BandListPartial", BandUtil.BandModels(true));
        }

        // This is where I play around with stuff
        public ActionResult Crazy()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            CrazyModel model = new CrazyModel();

            model.AllBands = BandUtil.BandModels();
            model.UserBands = BandUtil.BandModelsFor(WebSecurity.CurrentUserId);

            return View(model);
        }

        public ActionResult Admin()
        {


            ViewBag.SuccessMessage = "Admin account created!";

            return View("Success");
        }

        public ActionResult ThomasAndFriends()
        {
            WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "Sir Topham Hatt" });
            Roles.CreateRole("Administrator");
            Roles.AddUserToRole("admin", "Administrator");

            WebSecurity.CreateUserAndAccount("test1", "password", new { DisplayName = "Thomas" });
            WebSecurity.CreateUserAndAccount("test2", "password", new { DisplayName = "Edward" });
            WebSecurity.CreateUserAndAccount("test3", "password", new { DisplayName = "Henry" });
            WebSecurity.CreateUserAndAccount("test4", "password", new { DisplayName = "Gordon" });
            WebSecurity.CreateUserAndAccount("test5", "password", new { DisplayName = "Percy" });
            WebSecurity.CreateUserAndAccount("test6", "password", new { DisplayName = "James" });
            WebSecurity.CreateUserAndAccount("test7", "password", new { DisplayName = "Toby" });

            int testBandId = TestUtil.MakeBand("Thomas and Friends", WebSecurity.GetUserId("admin"), "password");

            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test1"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test2"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test3"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test4"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test5"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test6"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test7"));

            ViewBag.SuccessMessage = "Choo choo!";
            return View("Success");
        }
    }
}
