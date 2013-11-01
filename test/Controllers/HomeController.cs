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

        public ActionResult Accounts()
        {
            return View();
        }

        public ActionResult Test()
        {
            ViewBag.StatusMessage = "what am i doing here";
            
            //WebSecurity.CreateUserAndAccount("admin", "password", new { DisplayName = "admin" });
            //Roles.CreateRole("Administrator");
            //Roles.AddUserToRole("admin", "Administrator");

            return View();
        }
    }
}
