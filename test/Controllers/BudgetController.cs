using System.Web.Mvc;
using test.Stuff;

namespace band.Controllers
{
    public class BudgetController : Controller
    {
        public ActionResult Index(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();
        }
    }
}
