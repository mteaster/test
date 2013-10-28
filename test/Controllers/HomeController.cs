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
            WebSecurity.CreateUserAndAccount("admin", "Goose1234", new { DisplayName = "admin" });
            Roles.AddUserToRole("admin", "Administrator");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
