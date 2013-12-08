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

        //[PerformanceFilter]
        //public ActionResult Iterate()
        //{
        //    for(int i = 0; i < 100000; i++);

        //    ViewBag.SuccessMessage = "Iterated!";
        //    return View("Success");
        //}


    }
}
