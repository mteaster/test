using System.Linq;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/AllUsers

        [ChildActionOnly]
        public ActionResult AllUsers()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_UserListPartial", database.UserProfiles.ToList());
            }
        }
    }
}