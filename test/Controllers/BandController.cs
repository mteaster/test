using System.Linq;
using System.Web.Mvc;
using test.Models;
using System.Web.Helpers;
using WebMatrix.WebData;
using System.Collections.Generic;

namespace test.Controllers
{
    public class BandController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        [ChildActionOnly]
        public ActionResult AllBands()
        {
            List<BandProfile> bandProfiles = db.BandProfiles.ToList();
            List<BandDisplayModel> bandDisplays = new List<BandDisplayModel>();

            foreach (BandProfile bandProfile in bandProfiles)
            {
                BandDisplayModel bandDisplay = new BandDisplayModel();
                bandDisplay.BandName = bandProfile.BandName;
                bandDisplay.CreatorName = db.UserProfiles.Find(bandProfile.CreatorId).UserName;
                bandDisplay.Members = "TODO";
                bandDisplays.Add(bandDisplay);
            }

            return PartialView("_BandsPartial", bandDisplays);
        }

        // POST: /Band/Register

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterBandModel model)
        {
            // todo: exceptions, class

            if (ModelState.IsValid)
            {
                if (db.BandProfiles.Where(x => x.BandName == model.BandName).Count() > 0)
                {
                    ModelState.AddModelError("", "band name taken idiot");
                }
                else
                {
                    BandProfile band = new BandProfile(model.BandName,
                        WebSecurity.CurrentUserId,
                        Crypto.HashPassword(model.Password));
                    db.BandProfiles.Add(band);

                    BandMembership membership = new BandMembership(model.BandName, WebSecurity.CurrentUserId);
                    db.BandMemberships.Add(membership);
                    db.SaveChanges();

                    // todo: send them somewhere nice
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}