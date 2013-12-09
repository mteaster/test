using System.IO;
using System.Web.Mvc;
using System.Linq;
using test.Models;
using test.Models.Test;
using test.Stuff;
using System.Collections.Generic;
using test.Models.Band;
using WebMatrix.WebData;
using System;

namespace band.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ActionResult Index(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = database.BandProfiles.Find(bandId);

                if (profile == null)
                {
                    return View("Error");
                }

                ViewBag.BandId = profile.BandId;
                ViewBag.BandName = profile.BandName;

                BandMembership membership = database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId);

                ViewBag.InBand = (membership == null) ? false : true;

                BandBio bio = database.BandBios.Find(bandId);

                if (bio != null)
                {
                    return View(new BandBioModel(bio.Bio.Replace("\n", "<br />")));
                }

                ViewBag.BioSuccessMessage = TempData["BioSuccessMessage"];
                ViewBag.BioErrorMessage = TempData["BioErrorMessage"];

                ViewBag.TracksSuccessMessage = TempData["TracksSuccessMessage"];
                ViewBag.TracksErrorMessage = TempData["TracksErrorMessage"];

                ViewBag.BioErrorMessage = TempData["OES THISE WKLWORK?"];
                ViewBag.TracksErrorMessage = "DOES THIS WORK?";

                return View();
            }
        }

        public ActionResult UploadTrack(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (!BandUtil.Authenticate(bandId, this))
                {
                    return View("Error");
                }

                return View();
            }
        }

        public ActionResult Bio(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (!BandUtil.Authenticate(bandId, this))
                {
                    return View("Error");
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult EditBio(int bandId, BandBioModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (!BandUtil.Authenticate(bandId, this))
                {
                    return View("Error");
                }

                BandBio bio = database.BandBios.Find(bandId);

                if (String.IsNullOrEmpty(model.Bio))
                {
                    if (bio != null)
                    {
                        database.BandBios.Remove(database.BandBios.Find(bandId));
                        database.SaveChanges();
                    }
                }
                else
                {
                    if (bio == null)
                    {
                        bio = new BandBio(bandId, model.Bio);
                        database.BandBios.Add(bio);
                    }
                    else
                    {
                        bio.Bio = model.Bio;
                    }
                    database.SaveChanges();
                }

                TempData["BioSuccessMessage"] = "Bio edited.";
                return RedirectToAction("Index", new { bandId = bandId });
            }
        }

        [HttpPost]
        public ActionResult UploadTrack(int bandId, UploadTrackModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (!BandUtil.Authenticate(bandId, this))
                {
                    return View("Error");
                }

                if (model.TrackAudio == null)
                {
                    ViewBag.ErrorMessage = "Something was wrong with your audio file.";
                    return View();
                }

                if (Path.GetExtension(model.TrackAudio.FileName) != ".mp3")
                {
                    ViewBag.ErrorMessage = "That's not an mp3 file.";
                    return View();
                }

                if (model.TrackAudio.ContentLength <= 0 || model.TrackAudio.ContentLength > 1048576)
                {
                    ViewBag.ErrorMessage = "The audio is too big.";
                    return View();
                }

                if (model.TrackImage != null)
                {
                    if(model.TrackImage.ContentLength <= 0 || model.TrackImage.ContentLength > 1048576)
                    {
                        ViewBag.ErrorMessage = "The image file is too big.";
                        return View();
                    }
                }

                if (database.TrackEntries.Where(e => e.BandId == bandId && e.TrackName == model.TrackName).Any())
                {
                    ViewBag.ErrorMessage = "A track with that name already exists.";
                    return View();
                }

                TrackEntry trackEntry = new TrackEntry(bandId, model.TrackName, model.AlbumName);
                database.TrackEntries.Add(trackEntry);
                database.SaveChanges();

                string directory = Server.MapPath("~/App_Data/Tracks/" + bandId + "/");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                model.TrackAudio.SaveAs(directory + trackEntry.TrackId + ".mp3");

                if (model.TrackImage != null)
                {
                    model.TrackImage.SaveAs(directory + trackEntry.TrackId + ".jpg");
                }

                TempData["TracksSuccessMessage"] = model.TrackName + " uploaded.";
                return RedirectToAction("Index", new { bandId = bandId });
            }
        }


        public ActionResult DeleteTrack(int trackId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                TrackEntry trackEntry = database.TrackEntries.Find(trackId);

                if (!BandUtil.Authenticate(trackEntry.BandId, this))
                {
                    return View("Error");
                }

                System.IO.File.Delete(Server.MapPath("~/App_Data/Tracks/" + trackEntry.BandId + "/" + trackId + ".mp3"));
                System.IO.File.Delete(Server.MapPath("~/App_Data/Tracks/" + trackEntry.BandId + "/" + trackId + ".jpg"));

                database.TrackEntries.Remove(trackEntry);
                database.SaveChanges();

                TempData["TracksSuccessMessage"] = trackEntry.TrackName + " deleted.";
                return RedirectToAction("Index", new { bandId = trackEntry.BandId });
            }
        }

        public ActionResult DownloadTrackAudio(int trackId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                TrackEntry trackEntry = database.TrackEntries.Find(trackId);

                string path = Server.MapPath("~/App_Data/Tracks/" + trackEntry.BandId + "/" + trackId + ".mp3");;
                return File(path, "audio/mp3");
            }
        }

        public ActionResult DownloadTrackImage(int trackId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                TrackEntry trackEntry = database.TrackEntries.Find(trackId);

                string path = Server.MapPath("~/App_Data/Tracks/" + trackEntry.BandId + "/" + trackId + ".jpg");

                if (System.IO.File.Exists(path))
                {
                    return File(path, "image/jpeg");
                }
                else
                {
                    return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
                }
            }
        }

        public ActionResult GetTracks(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                List<TrackEntry> entries = database.TrackEntries.Where(t => t.BandId == bandId).ToList();

                List<TrackEntryModel> models = new List<TrackEntryModel>();

                foreach (var entry in entries)
                {
                    TrackEntryModel model = new TrackEntryModel(entry);
                    models.Add(model);
                }

                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
