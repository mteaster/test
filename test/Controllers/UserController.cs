using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class UserController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /User/AllUsers

        public ActionResult AllUsers()
        {
            List<UserProfile> ok = db.UserProfiles.ToList();
            return PartialView("_UsersPartial", db.UserProfiles.ToList());
        }
    }
}