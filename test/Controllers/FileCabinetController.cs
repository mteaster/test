using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web;
using System.IO;
using test.Models.FileCabinet;
using System.Web.Security;

namespace band.Content
{
    public class FileCabinetController : Controller
    {
        //
        // GET: /FileCabinet/

        public ActionResult Index(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }
            return View();
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(int bandId, HttpPostedFileBase file)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            ViewBag.Content = "IT'S NOTHING";

            if(file.ContentLength <= 0)
            {
                ViewBag.ErrorMessage = "File size is less than or equal to 0"; 
                return View("Error");
            }
            
            if(file.ContentLength > 100)
            {
                ViewBag.ErrorMessage = "File size is greater than 100 (whatever that means)";
                return View("Error");
            }

            FileEntry fileEntry = new FileEntry();
            fileEntry.BandId = bandId;
            fileEntry.UploaderId = WebSecurity.CurrentUserId;
            fileEntry.FileName = Path.GetFileName(file.FileName);
            fileEntry.FilePath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileEntry.FileName);

            using (DatabaseContext database = new DatabaseContext())
            {
                database.FileEntries.Add(fileEntry);
            }

            file.SaveAs(fileEntry.FilePath);

            StreamReader reader = new StreamReader(file.InputStream);
            @ViewBag.Content = reader.ReadToEnd();

            return View();
        }

    }
}
