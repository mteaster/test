using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.Band;
using test.Models.FileCabinet;
using test.Stuff;
using WebMatrix.WebData;
using System.Collections.Generic;
using System;

namespace band.Content
{
    [Authorize]
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
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

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
                ViewBag.BandId = bandId;
                ViewBag.BandName = database.BandProfiles.Find(bandId).BandName;

                if (database.FileGroups.Where(f => f.BandId == bandId && f.GroupName == groupName).Any())
                {
                    TempData["ErrorMessage"] = groupName + " already exists.";
                }
                else
                {
                    FileGroup fileGroup = new FileGroup();
                    fileGroup.BandId = bandId;
                    fileGroup.GroupName = groupName;
                    database.FileGroups.Add(fileGroup);
                    database.SaveChanges();

                    TempData["SuccessMessage"] = groupName + " created.";
                }

                return RedirectToAction(
            }
        }

        public ActionResult Files(int groupId, int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup fileGroup = database.FileGroups.Find(groupId);
                
                ViewBag.GroupId = groupId;
                ViewBag.GroupName = fileGroup.GroupName;
                ViewBag.BandId = fileGroup.BandId;
                ViewBag.BandName = fileGroup.BandProfile.BandName;
                if (database.BandMemberships.Find(fileGroup.BandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    ViewBag.ErrorMessage = "You are not a member of this band.";
                    return View("Error");
                }
                return View();
            }
        }

        public ActionResult GetFiles(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                int bandId = database.FileGroups.Find(groupId).BandId;
                
                if(database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    ViewBag.ErrorMessage = "You are not a member of this band.";
                    return View("Error");
                }

                List<FileEntry> entries = database.FileEntries.Where(f => f.BandId == bandId && f.GroupId == groupId).ToList();

                var results = from f in database.FileEntries
                              join u in database.UserProfiles
                              on f.UploaderId equals u.UserId
                              where f.BandId == bandId && f.GroupId == groupId
                              orderby f.FileName descending
                              select new
                              {
                                  f.FileId,
                                  f.FileName,
                                  f.FileDescription,
                                  f.FileType,
                                  f.FileSize,
                                  u.UserName,
                                  f.ModifiedTime
                              };

 
                List<FileEntryModel> models = new List<FileEntryModel>();
                foreach (var result in results)
                {
                    FileEntryModel model = new FileEntryModel(result.FileId, result.FileName, result.FileDescription,
                                                                (FileType)result.FileType, result.FileSize, result.UserName, 
                                                                result.ModifiedTime);
                    models.Add(model);
                }
                
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Groups(int bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_GroupsPartial", database.FileGroups.Where(f => f.BandId == bandId).ToList());
            }
        }

        public ActionResult GetGroups(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    ViewBag.ErrorMessage = "You are not a member of this band.";
                    return View("Error");
                }

                List<FileGroup> groups = database.FileGroups.Where(g => g.BandId == bandId).ToList();

                List<FileGroupModel> models = new List<FileGroupModel>();
                foreach (var group in groups)
                {
                    FileGroupModel model = new FileGroupModel(group.GroupId, group.BandId, group.GroupName);
                    models.Add(model);
                }

                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListFiles(int bandId, int groupId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                return View(database.FileEntries.Where(f => f.BandId == bandId && f.GroupId == groupId).ToList());
            }
        }

        public ActionResult UploadFile(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup fileGroup = database.FileGroups.Find(groupId);

                ViewBag.GroupId = groupId;
                ViewBag.BandId = fileGroup.BandId;
                ViewBag.BandName = database.BandProfiles.Find(fileGroup.BandId).BandName;

                if (database.BandMemberships.Find(fileGroup.BandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    return RedirectToAction("Join", "Band");
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadFile(int bandId, int groupId, UploadFileModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                ViewBag.BandId = bandId;
                ViewBag.BandName = database.BandProfiles.Find(bandId).BandName;

                if (database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null
                    && !Roles.IsUserInRole("Administrator"))
                {
                    return RedirectToAction("Join", "Band");
                }

                if (model.File.ContentLength <= 0 || model.File.ContentLength > 1048576)
                {
                    ViewBag.ErrorMessage = "file sucks";
                    return View("Error");
                }

                string fileName = Path.GetFileName(model.File.FileName);

                if (database.FileEntries.Where(f => f.BandId == bandId
                                        && f.GroupId == groupId
                                        && f.FileName == fileName).Any())
                {
                    ViewBag.ErrorMessage = "file already exists";
                    return View("Error");
                }

                string description = (model.FileDescription == null) ? "" : model.FileDescription;
                FileEntry fileEntry = new FileEntry(fileName, bandId, groupId, WebSecurity.CurrentUserId, FileType.File,
                                                        model.File.ContentLength, description, DateTime.UtcNow);

                database.FileEntries.Add(fileEntry);
                database.SaveChanges();

                string directory = Server.MapPath("~/App_Data/" + bandId + "/");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                model.File.SaveAs(directory + fileEntry.FileId);

                MessageBoardUtil.AddFilePost(bandId, fileEntry.FileId);

                ViewBag.SuccessMessage = fileEntry.FileName + " uploaded.";
                return View("Success");
            }
        }

        public ActionResult DownloadFile(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);
                ViewBag.BandId = fileEntry.BandId;
                ViewBag.BandName = database.BandProfiles.Find(fileEntry.BandId).BandName;

                if (database.BandMemberships.Find(fileEntry.BandId, WebSecurity.CurrentUserId) == null
                    && !Roles.IsUserInRole("Administrator"))
                {
                    return RedirectToAction("Join", "Band");
                }

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);

                return File(path, "application/" + Path.GetExtension(fileEntry.FileName), fileEntry.FileName);
            }
        }

        public ActionResult DeleteFile(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);
                ViewBag.BandId = fileEntry.BandId;
                ViewBag.BandName = database.BandProfiles.Find(fileEntry.BandId).BandName;

                if (database.BandMemberships.Find(fileEntry.BandId, WebSecurity.CurrentUserId) == null
                    && !Roles.IsUserInRole("Administrator"))
                {
                    return RedirectToAction("Join", "Band");
                }

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);

                System.IO.File.Delete(path);
                database.FileEntries.Remove(fileEntry);
                database.SaveChanges();

                ViewBag.SuccessMessage = fileEntry.FileName + " deleted.";
                return View("Success");
            }
        }
    }
}
