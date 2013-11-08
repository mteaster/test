using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web;
using System.IO;
using System.Linq;
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
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }
            return View();
        }

        public ActionResult UploadFile(int bandId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            return View();
        }

        public ActionResult DownloadFile(int bandId, int fileId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            string fileName = "";

            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);
                fileName = fileEntry.FileName;
            }



            string path = Server.MapPath("~/App_Data/" + bandId + "/" + fileName);

            return null;

        }

        private byte[] ReadFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        [HttpPost]
        public ActionResult UploadFile(int bandId, string path, HttpPostedFileBase file)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

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
            
            string directory = Server.MapPath("~/App_Data/" + bandId + "/");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileEntry fileEntry = new FileEntry();
            fileEntry.BandId = bandId;
            fileEntry.UploaderId = WebSecurity.CurrentUserId;
            fileEntry.FileName = Path.GetFileName(file.FileName);
            fileEntry.FilePath = path;

            int fileId;

            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.FileEntries.Where(f => f.BandId == fileEntry.BandId
                                        && f.FilePath == fileEntry.FilePath
                                        && f.FileName == fileEntry.FileName).Any())
                {
                    ViewBag.ErrorMessage = "file already exists";
                    return View("Error");
                }

                database.FileEntries.Add(fileEntry);
                database.SaveChanges();
                fileId = fileEntry.FileId;
            }
 
            file.SaveAs(directory + fileId);

            StreamReader reader = new StreamReader(file.InputStream);
            @ViewBag.Content = reader.ReadToEnd();
            
            return View(fileEntry);
        }
    }
}
