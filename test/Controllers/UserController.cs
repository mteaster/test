using System.Linq;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class UserController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        //
        // GET: /User/AllUsers

        [ChildActionOnly]
        public ActionResult AllUsers()
        {
            return PartialView("_UserListPartial", database.UserProfiles.ToList());
        }
    }
}