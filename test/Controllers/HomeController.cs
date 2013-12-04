using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using test.Filters;
using test.Models;
using test.Models.Test;
using test.Stuff;
using WebMatrix.WebData;

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

        // This is where I play around with stuff
        public ActionResult Band()
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

        public ActionResult AvatarDirectories()
        {
            string userAvatars = Server.MapPath("~/App_Data/UserAvatars/");

            if (!Directory.Exists(userAvatars))
            {
                Directory.CreateDirectory(userAvatars);
            }

            string bandAvatars = Server.MapPath("~/App_Data/BandAvatars/");

            if (!Directory.Exists(bandAvatars))
            {
                Directory.CreateDirectory(bandAvatars);
            }

            ViewBag.SuccessMessage = "it probably worked";
            return View("Success");
        }

        //[PerformanceFilter]
        //public ActionResult Iterate()
        //{
        //    for(int i = 0; i < 100000; i++);

        //    ViewBag.SuccessMessage = "Iterated!";
        //    return View("Success");
        //}

        public ActionResult Test()
        {
            // Clear out all the user avatars
            TestUtil.DeleteUserAvatars(Server);

            // Make admin account
            WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "Sir Topham Hatt" });
            Roles.CreateRole("Administrator");
            Roles.AddUserToRole("admin", "Administrator");
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("admin"), "AdminAvatar.jpg", Server);

            // Make Thomas and Friends
            int testBandId = TestUtil.MakeBand("Thomas and Friends", WebSecurity.GetUserId("admin"), "password");

            WebSecurity.CreateUserAndAccount("test1", "password", new { DisplayName = "Thomas" });
            WebSecurity.CreateUserAndAccount("test2", "password", new { DisplayName = "Edward" });
            WebSecurity.CreateUserAndAccount("test3", "password", new { DisplayName = "Henry" });
            WebSecurity.CreateUserAndAccount("test4", "password", new { DisplayName = "Gordon" });
            WebSecurity.CreateUserAndAccount("test5", "password", new { DisplayName = "Percy" });
            WebSecurity.CreateUserAndAccount("test6", "password", new { DisplayName = "James" });
            WebSecurity.CreateUserAndAccount("test7", "password", new { DisplayName = "Toby" });

            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test1"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test2"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test3"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test4"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test5"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test6"));
            TestUtil.PutInBand(testBandId, WebSecurity.GetUserId("test7"));

            // Make Cookie Monster
            WebSecurity.CreateUserAndAccount("cookiemonster", "password", new { DisplayName = "Cookie Monster" });
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("cookiemonster"), "CookieAvatar.jpg", Server);

            ViewBag.SuccessMessage = "Choo choo!";
            return View("Success");
        }


    }
}
