using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web.Security;

namespace band.Controllers
{
    public class RolodexController : Controller
    {
        //
        // GET: /Rolodex/

        public ActionResult Index(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
<<<<<<< HEAD
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
=======
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
>>>>>>> e5a828e257ad9158803c2af4130be0e57da28926
            {
                return RedirectToAction("Join", "Band");
            }

            // This will also display contacts for the band

            return View();
        }

        public ActionResult CreateBandContact(int BandId)
        {
            return PartialView("_CreateBandContact");
        }

    }
}
