using System.Linq;
using System.Web.Mvc;
using test.Models;
using System.Web.Helpers;
using WebMatrix.WebData;
using System.Collections.Generic;
using System.Text;
using System;

namespace test.Controllers
{
    public class BandController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        [ChildActionOnly]
        public ActionResult AllBands()
        {
            List<BandProfile> bandProfiles = database.BandProfiles.ToList();
            List<BandDisplayModel> bandDisplays = new List<BandDisplayModel>();

            foreach (BandProfile bandProfile in bandProfiles)
            {
                BandDisplayModel bandDisplay = new BandDisplayModel();
                bandDisplay.BandName = bandProfile.BandName;
                bandDisplay.CreatorName = database.UserProfiles.Find(bandProfile.CreatorId).UserName;

                var members = from b in database.BandMemberships
                          join u in database.UserProfiles 
                          on b.MemberId equals u.UserId
                          where b.BandId == bandProfile.BandId
                          select u.UserName;

                bandDisplay.Members = string.Join(", ", members.ToArray());

                //SELECT UserName
                //FROM BandMembership
                //INNER JOIN UserProfile
                //ON BandMembership.MemberId = UserProfile.UserId
                //WHERE BandId = bandProfile.BandId;

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
                if (database.BandProfiles.Where(x => x.BandName == model.BandName).Count() > 0)
                {
                    ModelState.AddModelError("", "band name taken idiot");
                }
                else
                {
                    BandProfile band = new BandProfile(model.BandName,
                        WebSecurity.CurrentUserId,
                        Crypto.HashPassword(model.Password));
                    database.BandProfiles.Add(band);

                    BandMembership membership = new BandMembership(band.BandId, WebSecurity.CurrentUserId);
                    database.BandMemberships.Add(membership);
                    database.SaveChanges();

                    // todo: send them somewhere nice
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult Join(string bandId)
        {
            ViewBag.BandId = id;
            return View();

            int idAsInt;

            try
            {
               idAsInt = Convert.ToInt32(id);

                ViewBag.BandId = id + " | " + idAsInt;

                return View();
            }
            catch (FormatException fe)
            {
                ViewBag.BandId = "format exception for id " + id;
                return View();
            }
            catch (OverflowException oe)
            {
                ViewBag.BandId = "overflow exception for id " + id;
                return View();
            }

            if (database.BandProfiles.Find(idAsInt) == null)
            {
                ViewBag.BandId = "couldn't find band with id " + idAsInt;
            }
            else
            {
                ViewBag.BandId = "found the band with id " + idAsInt;
            }

            //BandMembership membership = new BandMembership(band.BandId, WebSecurity.CurrentUserId);
            //database.BandMemberships.Add(membership);
            //database.SaveChanges();

            //ViewBag.BandId = id;

            return View();
        }
    }
}