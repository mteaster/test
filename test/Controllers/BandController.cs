using System.Linq;
using System.Web.Mvc;
using test.Models;
using System.Web.Helpers;
using WebMatrix.WebData;
using System.Collections.Generic;
using System.Text;
using System;
using System.Data;

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
                bandDisplay.BandId = bandProfile.BandId;
                bandDisplay.BandName = bandProfile.BandName;
                bandDisplay.CreatorName = database.UserProfiles.Find(bandProfile.CreatorId).UserName;

                var members = from b in database.BandMemberships
                          join u in database.UserProfiles 
                          on b.MemberId equals u.UserId
                          where b.BandId == bandProfile.BandId
                          select u.UserName;

                bandDisplay.Members = string.Join(", ", members.ToArray());

                bandDisplays.Add(bandDisplay);
            }

            return PartialView("_BandListPartial", bandDisplays);
        }

        //
        // GET: /Band/Register

        [ChildActionOnly]
        public ActionResult Bands()
        {
            List<BandDisplayModel> bandDisplays = new List<BandDisplayModel>();

            // this probably needs to be optimized, i feel like im doing something really dumb

            var results = from m in database.BandMemberships
                        join p in database.BandProfiles
                        on m.BandId equals p.BandId
                        join u in database.UserProfiles
                        on p.CreatorId equals u.UserId
                        where m.MemberId == WebSecurity.CurrentUserId
                        select new { p.BandId, p.BandName, u.UserName };

            foreach(var row in results)
            {
                BandDisplayModel bandDisplay = new BandDisplayModel();
                bandDisplay.BandId = row.BandId;
                bandDisplay.BandName = row.BandName;
                bandDisplay.CreatorName = row.UserName;

                bandDisplays.Add(bandDisplay);
            }

            return PartialView("_BandsPartial", bandDisplays);
        }

        //
        // GET: /Band/Register

        public ActionResult Register()
        {
            return View();
        }

        //
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

        //
        // GET: /Band/Search

        public ActionResult Search()
        {
            return View(new SearchViewModel());
        }

        //
        // POST: /Band/Search

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchBandModel model)
        {
            SearchViewModel svm = new SearchViewModel();
            svm.searchModel = model;

            if (ModelState.IsValid)
            {
                List<BandDisplayModel> bandDisplays = new List<BandDisplayModel>();

                var results = from b in database.BandProfiles
                              join u in database.UserProfiles
                              on b.CreatorId equals u.UserId
                              where b.BandName.Contains(model.BandName)
                              select new { b.BandId, b.BandName, u.UserName };

                // again, this is seems dumb
                foreach (var row in results)
                {
                    BandDisplayModel bandDisplay = new BandDisplayModel();
                    bandDisplay.BandId = row.BandId;
                    bandDisplay.BandName = row.BandName;
                    bandDisplay.CreatorName = row.UserName;

                    bandDisplays.Add(bandDisplay);
                }

                svm.resultsModel = bandDisplays;

                return View(svm);
            }

            // If we got this far, something failed, redisplay form
            return View(svm);
        }

        //
        // GET: /Band/Join

        [Authorize]
        public ActionResult Join(string bandId)
        {
            int bandIdAsInt;

            if (!Int32.TryParse(bandId, out bandIdAsInt))
            {
                ViewBag.StatusMessage = "Invalid band ID (format)";
                return View("Status");
            }

            BandProfile bandProfile = database.BandProfiles.Find(bandIdAsInt);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "Invalid band ID (not in database)";
                return View("Status");
            }

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            return View();
        }

        //
        // POST: /Band/Join

        [Authorize]
        [HttpPost]
        public ActionResult Join(string bandId, JoinBandModel model)
        {
            int bandIdAsInt;

            if (!Int32.TryParse(bandId, out bandIdAsInt))
            {
                ViewBag.StatusMessage = "Invalid band ID (format)";
                return View("Status");
            }

            BandProfile bandProfile = database.BandProfiles.Find(bandIdAsInt);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "Invalid band ID (not in database)" + bandId;
            }
            else
            {
                ViewBag.BandName = bandProfile.BandName;
                ViewBag.BandId = bandId;

                //string hash = Crypto.HashPassword(model.Password);
                //if (hash == bandProfile.Password)
                //{
                    BandMembership membership = new BandMembership(bandIdAsInt, WebSecurity.CurrentUserId);
                    database.BandMemberships.Add(membership);
                    database.SaveChanges();

                    ViewBag.StatusMessage = "You joined " + bandProfile.BandName;
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Invalid band password (" + hash + ", " + bandProfile.Password + ")");
                //    return View(model);
                //}
            }

            return View("Status");
        }

        //
        // Get: /Band/Update

        [Authorize]
        public ActionResult Update(int id)
        {
            BandProfile bandProfile = database.BandProfiles.Find(id);

            if (bandProfile == null)
            {
                ViewBag.StatusMessage = "Band not in database.";
                return View("Status");
            }
            else
            {
                ViewBag.BandId = bandProfile.BandId;
                ViewBag.BandName = bandProfile.BandName;
                return View();
            }
        }

        //
        // Post: /Band/Update

        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, UpdateBandModel updateBandModel)
        {
            bool updateName = false;
            bool updatePassword = false;
            // Load the current band profile by id
            BandProfile bandProfile = database.BandProfiles.Find(id);

            if (bandProfile == null)
            {
                // Could not find the band. This shouldn't happen
                ViewBag.UpdateError = "Unexpected Error: Could not update profile.";
                return View();
            }
            else
            {
                // update band name, if new one provided
                if (!string.IsNullOrWhiteSpace(updateBandModel.NewBandName))
                {
                    bandProfile.BandName = updateBandModel.NewBandName;
                    updateName = true;
                }

                // update band password, if new one provided
                if (!string.IsNullOrWhiteSpace(updateBandModel.NewPassword))
                {
                    bandProfile.Password = Crypto.HashPassword(updateBandModel.NewPassword);
                    updatePassword = true;
                }

                if (updatePassword || updateName)
                {
                    database.Entry(bandProfile).State = EntityState.Modified;
                    database.SaveChanges();
                    return View("~/Views/Home/About.cshtml");
                }
                else
                {
                    ViewBag.UpdateError = "No changes detected.";
                    return View();
                }
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            List<BandMembership> bandMembershipList;
            // Load the current band profile by id
            BandProfile bandProfile = database.BandProfiles.Find(id);


            if (bandProfile == null)
            {
                // Could not find the band. This shouldn't happen
                ViewBag.UpdateError = "Unexpected Error: Could not delete profile.";
                return View("Update");
            }
            else
            {
                // Delete the band
                database.BandProfiles.Remove(bandProfile);
                bandMembershipList = database.BandMemberships.Where(m => m.BandId == id).ToList();
                foreach (BandMembership bm in bandMembershipList)
                {
                    database.BandMemberships.Remove(bm);
                }
                database.SaveChanges();
                return View("~/Views/Home/About.cshtml");
            }
        }

    }
}