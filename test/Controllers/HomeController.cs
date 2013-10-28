using System.Linq;
using System.Web.Mvc;
using test.Models;
using WebMatrix.WebData;
using System.Web.Security;
using test.Filters;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [PerformanceFilter]
        public ActionResult Test()
        {
            ViewBag.StatusMessage = "what am i doing here";
            return View("Success");
        }
    }
}
