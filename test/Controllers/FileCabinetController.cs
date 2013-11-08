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
using System.Collections.Generic;

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

        public ActionResult CreateGroup(int bandId)
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

        [HttpPost]
        public ActionResult CreateGroup(int bandId, string groupName)
        {
            using (DatabaseContext database = new DatabaseContext())
            {

                if (database.FileGroups.Where(f => f.BandId == bandId && f.GroupName == groupName).Any())
                {
                    ViewBag.ErrorMessage = "group already exists";
                    return View("Error");
                }

                FileGroup fileGroup = new FileGroup();
                fileGroup.BandId = bandId;
                fileGroup.GroupName = groupName;
                database.FileGroups.Add(fileGroup);
                database.SaveChanges();

                ViewBag.SuccessMessage = "group created";
                return View("Success");
            }
        }

        public ActionResult Groups(int bandId)
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

            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_GroupsPartial", database.FileGroups.Where(f => f.BandId == bandId).ToList());
            }
        }

        public ActionResult ListFiles(int bandId, int groupId)
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

            using (DatabaseContext database = new DatabaseContext())
            {
                return View(database.FileEntries.Where(f => f.BandId == bandId && f.GroupId == groupId).ToList());
            }
        }

        public ActionResult UploadFile(int bandId, int groupId)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            ViewBag.GroupId = groupId;
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(int bandId, int groupId, HttpPostedFileBase file)
        {
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);
            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            if (file.ContentLength <= 0 || file.ContentLength > 100)
            {
                return null;
            }

            FileEntry fileEntry = new FileEntry(Path.GetFileName(file.FileName), bandId, groupId, WebSecurity.CurrentUserId);

            using (DatabaseContext database = new DatabaseContext())
            {

                if (database.FileEntries.Where(f => f.BandId == fileEntry.BandId
                                        && f.GroupId == fileEntry.GroupId
                                        && f.FileName == fileEntry.FileName).Any())
                {
                    ViewBag.ErrorMessage = "file already exists";
                    return View("Error");
                }

                database.FileEntries.Add(fileEntry);
                database.SaveChanges();
            }

            string directory = Server.MapPath("~/App_Data/" + bandId + "/");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            file.SaveAs(directory + fileEntry.FileId);

            StreamReader reader = new StreamReader(file.InputStream);
            @ViewBag.Content = reader.ReadToEnd();
            
            return View(fileEntry);
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

    }
}
