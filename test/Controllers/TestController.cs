using System.IO;
using System.Web.Mvc;
using test.Models;
using test.Models.Band;
using test.Models.Test;
using test.Stuff;
using WebMatrix.WebData;
using System.Web.Security;

namespace test.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Data()
        {
            // Clear out all the user avatars
            TestUtil.DeleteUserAvatars(Server);

            // Make admin account
            WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "Sir Topham Hatt" });
            Roles.CreateRole("Administrator");
            Roles.AddUserToRole("admin", "Administrator");
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("admin"), "AdminAvatar.jpg", Server);

            // Trains
            int thomasBandId = TestUtil.MakeBand("Thomas and Friends", WebSecurity.GetUserId("admin"), "password");

            TestUtil.GiveBandAvatar(thomasBandId, "blue", Server);

            WebSecurity.CreateUserAndAccount("test1", "password", new { DisplayName = "Thomas" });
            WebSecurity.CreateUserAndAccount("test2", "password", new { DisplayName = "Edward" });
            WebSecurity.CreateUserAndAccount("test3", "password", new { DisplayName = "Henry" });
            WebSecurity.CreateUserAndAccount("test4", "password", new { DisplayName = "Gordon" });
            WebSecurity.CreateUserAndAccount("test5", "password", new { DisplayName = "Percy" });
            WebSecurity.CreateUserAndAccount("test6", "password", new { DisplayName = "James" });
            WebSecurity.CreateUserAndAccount("test7", "password", new { DisplayName = "Toby" });

            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test1"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test2"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test3"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test4"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test5"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test6"));
            TestUtil.PutInBand(thomasBandId, WebSecurity.GetUserId("test7"));

            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test1"), "blue", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test2"), "blue", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test3"), "green", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test4"), "blue", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test5"), "green", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test6"), "red", Server);
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("test7"), "orange", Server);

            // Cookie Monster
            WebSecurity.CreateUserAndAccount("cookiemonster", "password", new { DisplayName = "Cookie Monster" });
            TestUtil.GiveUserAvatar(WebSecurity.GetUserId("cookiemonster"), "CookieAvatar.jpg", Server);

            int cookieBandId = TestUtil.MakeBand("C IS FOR COOKIE", WebSecurity.GetUserId("cookiemonster"), "password");
            TestUtil.GiveBandAvatar(cookieBandId, "black", Server);



            ViewBag.SuccessMessage = "Choo choo!";
            return View("Success");
        }

        public ActionResult Directories()
        {

            string[] directories = { Server.MapPath("~/App_Data/UserAvatars/"),
                                       Server.MapPath("~/App_Data/BandAvatars/"),
                                       Server.MapPath("~/App_Data/Tracks/"),
                                       Server.MapPath("~/App_Data/BandContactAvatars/"),
                                       Server.MapPath("~/App_Data/VenueContactAvatars/"),
                                       Server.MapPath("~/App_Data/PeopleContactAvatars/") };

            foreach (string directory in directories)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            ViewBag.SuccessMessage = "It probably worked";
            return View("Success");
        }
    }
}
