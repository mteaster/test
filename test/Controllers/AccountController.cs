using Microsoft.Web.WebPages.OAuth;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using WebMatrix.WebData;
using test.Models.Account;
using System.Web;

namespace test.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { DisplayName = model.DisplayName } );
                    WebSecurity.Login(model.UserName, model.Password);

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }


        //
        // POST: /Account/ChangeDisplayName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeDisplayName(ChangeDisplayNameModel model)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    UserProfile original = database.UserProfiles.Find(WebSecurity.CurrentUserId);
                    original.DisplayName = model.DisplayName;
                    database.Entry(original).State = EntityState.Modified;
                    database.SaveChanges();
                }

                TempData["SuccessMessage"] = "Your display name has been changed.";
            }
            else
            {
                TempData["ErrorMessage"] = "Something was wrong with the display name you entered. Try again.";
            }

            return RedirectToAction("Manage");
        }
        

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;

                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (InvalidOperationException)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    TempData["SuccessMessage"] = "Your password has been changed.";
                }
                else
                {
                    TempData["ErrorMessage"] = "The current password is incorrect or the new password is invalid.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Something was wrong with the password you entered, but I don't know what.";
            }

            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file)
        {
            if (file.ContentLength <= 0 || file.ContentLength > 1048576)
            {
                TempData["ErrorMessage"] = "Something was wrong with the avatar you uploaded.";
            }
            else
            {
                string path = Server.MapPath("~/App_Data/UserAvatars/" + WebSecurity.CurrentUserId + ".jpg");
                file.SaveAs(path);
                TempData["ErrorMessage"] = "Avatar changed.";
            }


            return RedirectToAction("Manage");
        }

        [OutputCache(Duration = 60, VaryByParam = "userId")]
        public ActionResult DownloadAvatar(int userId)
        {
            string path = Server.MapPath("~/App_Data/UserAvatars/" + userId + ".jpg");

            if (System.IO.File.Exists(path))
            {
                return File(path, "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
