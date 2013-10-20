using System.Linq;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        public ActionResult Index()
        {
            ViewBag.Message = "welcome to our dumb website";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View(database.BandProfiles.ToList());
        }

        public ActionResult AllBands()
        {
            return PartialView("_BandsPartial", database.BandProfiles.ToList());
        }
    }
}
