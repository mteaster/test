using System.Linq;
using System.Web.Mvc;
using test.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Roles.CreateRole("Administrator");
            Roles.AddUserToRole("admin", "Administrator");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
