using System.IO;
using System.Web.Mvc;
using System.Linq;
using test.Models;
using test.Models.Test;
using test.Stuff;

namespace band.Controllers
{
    public class OnlineController : Controller
    {
        public ActionResult Index(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();
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

                if (model.TrackAudio.ContentLength <= 0 || model.TrackAudio.ContentLength > 1048576)
                {
                    ViewBag.ErrorMessage = "The track audio file size is too big.";
                    return View("Error");
                }

                if (model.TrackImage != null && model.TrackImage.ContentLength <= 0 || model.TrackImage.ContentLength > 1048576)
                {
                    ViewBag.ErrorMessage = "The track image file size is too big.";
                    return View("Error");
                }

                
                if (database.TrackEntries.Where(e => e.BandId == bandId && e.TrackName == model.TrackName).Any())
                {
                    ViewBag.ErrorMessage = "A track with that name already exists.";
                    return View("Error");
                }

                TrackEntry trackEntry = new TrackEntry(model.TrackName, bandId);
                database.TrackEntries.Add(trackEntry);
                database.SaveChanges();

                string directory = Server.MapPath("~/App_Data/Tracks/" + bandId + "/");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                model.TrackAudio.SaveAs(directory + model.TrackName + ".mp3");

                if (model.TrackImage != null)
                {
                    model.TrackAudio.SaveAs(directory + model.TrackName + ".jpg");
                }

                TempData["SuccessMessage"] = model.TrackName + " uploaded.";
                return RedirectToAction("Index", new { bandId = bandId });
            }
        }
    }
}
