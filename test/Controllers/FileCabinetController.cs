﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.FileCabinet;
using test.Stuff;
using WebMatrix.WebData;

namespace band.Content
{
    [Authorize]
    public class FileCabinetController : Controller
    {
        public ActionResult Index(int bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            ViewBag.OtherSuccessMessage = TempData["OtherSuccessMessage"];
            ViewBag.OtherErrorMessage = TempData["OtherErrorMessage"];

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
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.FileGroups.Where(f => f.BandId == bandId && f.GroupName == groupName).Any())
                {
                    TempData["OtherErrorMessage"] = groupName + " already exists.";
                }
                else
                {
                    FileGroup fileGroup = new FileGroup();
                    fileGroup.BandId = bandId;
                    fileGroup.GroupName = groupName;
                    database.FileGroups.Add(fileGroup);
                    database.SaveChanges();

                    TempData["OtherSuccessMessage"] = groupName + " created.";
                }

                return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
            }
        }

        public ActionResult DeleteGroup(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup fileGroup = database.FileGroups.Find(groupId);

                if (!BandUtil.Authenticate(fileGroup.BandId, this))
                {
                    return View("Error");
                }

                var fileEntries = database.FileEntries.Where(f => f.GroupId == groupId);

                foreach (var fileEntry in fileEntries)
                {
                    System.IO.File.Delete(Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileEntry.FileId));
                    database.FileEntries.Remove(fileEntry);
                }

                database.FileGroups.Remove(fileGroup);
                database.SaveChanges();

                TempData["SuccessMessage"] = "Group deleted.";

                return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
            }
        }

        public ActionResult Files(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup fileGroup = database.FileGroups.Find(groupId);

                if (!BandUtil.Authenticate(fileGroup.BandId, this))
                {
                    return View("Error");
                }

                ViewBag.GroupId = groupId;
                ViewBag.GroupName = fileGroup.GroupName;

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View();
            }
        }

        public ActionResult GetFiles(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                int bandId = database.FileGroups.Find(groupId).BandId;

                if (!BandUtil.Authenticate(bandId, this))
                {
                    return RedirectToAction("Join", "Band");
                }

                List<FileEntry> entries = database.FileEntries.Where(f => f.GroupId == groupId).ToList();

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
                                  u.DisplayName,
                                  f.ModifiedTime
                              };

 
                List<FileEntryModel> models = new List<FileEntryModel>();
                foreach (var result in results)
                {
                    FileEntryModel model = new FileEntryModel(result.FileId, result.FileName, result.FileDescription,
                                                                (FileType)result.FileType, result.FileSize, 
                                                                result.DisplayName, result.ModifiedTime);
                    models.Add(model);
                }
                
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Groups(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                return PartialView("_GroupsPartial", database.FileGroups.Where(f => f.BandId == bandId).ToList());
            }
        }

        public ActionResult GetGroups(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                List<FileGroup> groups = database.FileGroups.Where(g => g.BandId == bandId).ToList();

                //var results = from g in database.FileGroups
                //              join e in database.FileEntries
                //              on g.GroupId equals e.GroupId
                //              into joined
                //              where joined.
                //              orderby joined.Count() descending, g.GroupName descending
                //              select new
                //              {
                //                  g.GroupId,
                //                  g.GroupName,
                //                  g.BandId,
                //                  FilesCount = joined.Count()
                //              };


                List<FileGroupModel> models = new List<FileGroupModel>();
                foreach (var result in groups)
                {
                    FileGroupModel model = new FileGroupModel(result.GroupId, result.BandId, result.GroupName, 0);
                    models.Add(model);
                }

                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadFile(int groupId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup fileGroup = database.FileGroups.Find(groupId);

                ViewBag.GroupId = groupId;
                
                if (!BandUtil.Authenticate(fileGroup.BandId, this))
                {
                    return View("Error");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadFile(int groupId, UploadFileModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                int bandId = database.FileGroups.Find(groupId).BandId;

                if (!BandUtil.Authenticate(bandId, this))
                {
                    return View("Error");
                }

                //if (model.File.ContentLength <= 0 || model.File.ContentLength > 1048576)
                if (model.File.ContentLength <= 0 || model.File.ContentLength > 52428800)
                {
                    ViewBag.ErrorMessage = "The file size is too big.";
                    return View("Error");
                }

                string fileName = Path.GetFileName(model.File.FileName);

                if (database.FileEntries.Where(f => f.GroupId == groupId && f.FileName == fileName).Any())
                {
                    ViewBag.ErrorMessage = "A file with that name already exists.";
                    return View("Error");
                }

                FileEntry fileEntry = new FileEntry(fileName, bandId, groupId, WebSecurity.CurrentUserId, FileCabinetUtil.GetFileType(fileName),
                                                        model.File.ContentLength, model.FileDescription, DateTime.UtcNow);

                database.FileEntries.Add(fileEntry);
                database.SaveChanges();
                 
                string directory = Server.MapPath("~/App_Data/" + bandId + "/");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                model.File.SaveAs(directory + fileEntry.FileId);

                MessageBoardUtil.AddFilePost(bandId, fileEntry.FileId);

                TempData["SuccessMessage"] = fileEntry.FileName + " uploaded.";
                return RedirectToAction("Files", new { bandId = bandId, groupId = groupId });
            }
        }

        public ActionResult DownloadFile(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);
                
                if (!BandUtil.Authenticate(fileEntry.BandId, this))
                {
                    return View("Error");
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
                
                if (!BandUtil.Authenticate(fileEntry.BandId, this))
                {
                    return View("Error");
                }

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);

                System.IO.File.Delete(path);
                database.FileEntries.Remove(fileEntry);
                database.SaveChanges();

                TempData["SuccessMessage"] = fileEntry.FileName + " deleted.";
                return RedirectToAction("Files", new { groupId = fileEntry.GroupId });
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult PDF(string file)
        {
            return View();
        }

        public ActionResult ViewFile(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);

                if (!BandUtil.Authenticate(fileEntry.BandId, this))
                {
                    return View("Error");
                }

                FileModel model = new FileModel(fileEntry, database.UserProfiles.Find(fileEntry.UploaderId).DisplayName);

                if(!System.IO.File.Exists(Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId)))
                {
                    ViewBag.ErrorMessage = "We couldn't find the file.";
                    return View("Error");
                }

                if (fileEntry.FileType == (int)FileType.Document || fileEntry.FileType == (int)FileType.File)
                {
                    if (Path.GetExtension(fileEntry.FileName) == ".pdf")
                    {
                        return RedirectToAction("PDF", new { file = "http://dnab.azurewebsites.net/FileCabinet/DownloadPDF?fileId=" + fileId });
                    }
                    else
                    {
                        return View("File", model);
                    }
                }

                if (fileEntry.FileType == (int)FileType.Text)
                {
                    string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);

                    using (StreamReader stream = new StreamReader(path))
                    {
                        model.Content = stream.ReadToEnd();
                    }

                    return View("Text", model);
                }

                model.Content = Path.GetExtension(fileEntry.FileName).Replace(".", "");
                string viewName = ((FileType)fileEntry.FileType).ToString();
                return View(viewName, model);
            }
        }

        public ActionResult DownloadImage(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);
                string extension = Path.GetExtension(fileEntry.FileName).Replace(".", "");
                return File(path, "image/" + extension);
            }
        }

        public ActionResult DownloadAudio(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);
                string extension = Path.GetExtension(fileEntry.FileName).Replace(".", "");
                return File(path, "audio/" + extension);
            }
        }

        public ActionResult DownloadVideo(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);
                string extension = Path.GetExtension(fileEntry.FileName).Replace(".", "");
                return File(path, "video/" + extension);
            }
        }

        public ActionResult DownloadPDF(int fileId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileEntry fileEntry = database.FileEntries.Find(fileId);

                string path = Server.MapPath("~/App_Data/" + fileEntry.BandId + "/" + fileId);
                string extension = Path.GetExtension(fileEntry.FileName).Replace(".", "");
                return File(path, "application/pdf");
            }
        }
    }
}
