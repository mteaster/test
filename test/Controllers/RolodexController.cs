using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;

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
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
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
