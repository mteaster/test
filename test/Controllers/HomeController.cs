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

        public ActionResult Test()
        {
            ViewBag.StatusMessage = "what am i doing here";
            
            //WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "admin" });
            //Roles.CreateRole("Administrator");
            //Roles.AddUserToRole("admin", "Administrator");

            return View();
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            @ViewBag.Content = "IT'S NOTHING";

            if (file.ContentLength > 0)
            {
                StreamReader reader = new StreamReader(file.InputStream);
                @ViewBag.Content = reader.ReadToEnd();
            }

            return View();
        }
    }
}
