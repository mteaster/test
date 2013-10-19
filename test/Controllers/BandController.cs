using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using System.Web.Helpers;

namespace test.Controllers
{
    public class BandController : Controller
    {
        private BandsContext db = new BandsContext();

        [ChildActionOnly]
        public ActionResult AllBands()
        {
            return PartialView("_BandsPartial", db.BandProfiles.ToList());
        }

        //
        // POST: /Band/Register

        [ChildActionOnly]
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
                    BandProfile band = new BandProfile();
                    band.BandName = model.BandName;
                    band.HashedPassword = Crypto.HashPassword(model.Password);

                    db.BandProfiles.Add(band);
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