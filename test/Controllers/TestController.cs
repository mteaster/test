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
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Band(int bandId)
        {
            ViewBag.BandId = bandId;
            return View();
        }
    }
}
