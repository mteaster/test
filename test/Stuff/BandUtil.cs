using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.Band;
using test.Models.Calendar;
using test.Models.Dashboard;
using test.Models.FileCabinet;
using WebMatrix.WebData;

namespace test.Stuff
{
    public class BandNotFoundException : System.Exception
    {
        public BandNotFoundException() : base("A band with this ID does not an exist.") {}
        public BandNotFoundException(string message) : base(message) {}
    }

    public class BandUtil
    {
        public static bool IsUserInBand(int userId, int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return database.BandMemberships.Find(bandId, userId) != null;
            }
        }

        public static bool BandExists(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return database.BandProfiles.Find(bandId) != null;
            }
        }

        public static bool Authenticate(int bandId, ControllerBase controller)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = database.BandProfiles.Find(bandId);

                if (profile == null)
                {
                    controller.ViewBag.ErrorMessage = "We couldn't find a band with that id.";
                    return false;
                }

                if (database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    controller.ViewBag.ErrorMessage = "You must be a member of this band to access its content.";
                    return false;
                }

                controller.ViewBag.BandId = bandId;
                controller.ViewBag.BandName = profile.BandName;

                return true;
            }
        }

        public static int Register(RegisterBandModel model)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = new BandProfile(model.BandName, WebSecurity.CurrentUserId, Crypto.HashPassword(model.Password));

                database.BandProfiles.Add(profile);

                BandMembership membership = new BandMembership(profile.BandId, WebSecurity.CurrentUserId);
                database.BandMemberships.Add(membership);

                database.SaveChanges();

                return profile.BandId;
            }
        }

        public static bool Join(int bandId, string password)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = BandProfileFor(bandId, database);

                if(Crypto.VerifyHashedPassword(profile.Password, password))
                {
                    BandMembership membership = new BandMembership(bandId, WebSecurity.CurrentUserId);
                    database.BandMemberships.Add(membership);
                    database.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public static bool Leave(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandMembership membership = database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId);

                if (membership == null)
                {
                    return false;
                }

                database.BandMemberships.Remove(membership);
                database.SaveChanges();

                return true;
            }
        }

        // Might just want to pass the band directory here, not the HttpServerUtilityBase
        public static bool Delete(int bandId, HttpServerUtilityBase server)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = BandProfileFor(bandId, database);

                if (profile.CreatorId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Administrator"))
                {
                    return false;
                }

                database.BandProfiles.Remove(profile);

                IQueryable<BandMembership> memberships = database.BandMemberships.Where(m => m.BandId == bandId);
                foreach (BandMembership membership in memberships)
                {
                    database.BandMemberships.Remove(membership);
                }

                IQueryable<MessageBoardPost> posts = database.MessageBoardPosts.Where(p => p.BandId == bandId);
                foreach (MessageBoardPost post in posts)
                {
                    database.MessageBoardPosts.Remove(post);
                }

                IQueryable<CalendarEvent> events = database.CalendarEvents.Where(e => e.BandId == bandId);
                foreach (CalendarEvent evt in events)
                {
                    database.CalendarEvents.Remove(evt);
                }

                string directory = server.MapPath("~/App_data/" + bandId + "/");
                IQueryable<FileEntry> files = database.FileEntries.Where(f => f.BandId == bandId);
                foreach (FileEntry file in files)
                {
                    File.Delete(directory + file.FileId);
                    database.FileEntries.Remove(file);
                }
                System.IO.Directory.Delete(directory);

                IQueryable<FileGroup> groups= database.FileGroups.Where(g => g.BandId == bandId);
                foreach (FileGroup group in groups)
                {
                    database.FileGroups.Remove(group);
                }

                database.SaveChanges();

                return true;
            }
        }

        public static bool ChangeBandName(int bandId, string bandName)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    return false;
                }

                BandProfile profile = BandProfileFor(bandId, database);
                profile.BandName = bandName;
                database.Entry(profile).State = EntityState.Modified;
                database.SaveChanges();

                return true;
            }
        }

        public static bool ChangeBandPassword(int bandId, string password)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.BandMemberships.Find(bandId, WebSecurity.CurrentUserId) == null && !Roles.IsUserInRole("Administrator"))
                {
                    return false;
                }

                BandProfile profile = BandProfileFor(bandId, database);
                profile.Password = Crypto.HashPassword(password);
                database.Entry(profile).State = EntityState.Modified;
                database.SaveChanges();

                return true;
            }
        }

        // might want to integrate into Register
        public static bool IsBandNameTaken(string bandName)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return database.BandProfiles.Where(x => x.BandName == bandName).Count() > 0;
            }
        }

        public static string MembersFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var memberUsernames = from b in database.BandMemberships
                                      join u in database.UserProfiles
                                      on b.MemberId equals u.UserId
                                      where b.BandId == bandId
                                      select u.UserName;

                return string.Join(", ", memberUsernames.ToArray());
            }
        }

        public static List<MemberModel> MemberModelsFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var results = from b in database.BandMemberships
                              join u in database.UserProfiles
                              on b.MemberId equals u.UserId
                              where b.BandId == bandId
                              select new { u.UserName, u.DisplayName };

                List<MemberModel> memberModels = new List<MemberModel>();

                foreach (var result in results)
                {
                    MemberModel memberModel = new MemberModel();
                    memberModel.MemberDisplayName = result.DisplayName;
                    memberModel.MemberUserName = result.UserName;
                    memberModels.Add(memberModel);
                }

                return memberModels;
            }
        }

        public static string BandNameFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = database.BandProfiles.Find(bandId);
                if (profile == null)
                {
                    throw new BandNotFoundException();
                }
                return profile.BandName;
            }
        }

        private static BandProfile BandProfileFor(int bandId, DatabaseContext database)
        {
            BandProfile profile = database.BandProfiles.Find(bandId);
            if (profile == null)
            {
                throw new BandNotFoundException();
            }
            return profile;
        }

        public static BandProfile BandProfileFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return BandProfileFor(bandId, database);
            }
        }

        public static List<BandModel> BandModels(bool membersFlag = false)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                List<BandProfile> bandProfiles = database.BandProfiles.ToList();
                List<BandModel> bandModels = new List<BandModel>();

                foreach (BandProfile bandProfile in bandProfiles)
                {
                    bandModels.Add(BandUtil.BandModelFor(bandProfile.BandId, database, true));
                }

                return bandModels;
            }
        }

        public static List<BandModel> BandModelsFor(int userId, bool membersFlag = false)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                List<BandModel> bandModels = new List<BandModel>();

                var bandIds = from m in database.BandMemberships
                              join p in database.BandProfiles
                              on m.BandId equals p.BandId
                              where m.MemberId == userId
                              select p.BandId;

                foreach (int bandId in bandIds)
                {
                    bandModels.Add(BandModelFor(bandId, membersFlag));
                }

                return bandModels;
            }
        }

        private static BandModel BandModelFor(int bandId, DatabaseContext database, bool membersFlag = false)
        {
                BandProfile profile = BandProfileFor(bandId, database);

                return new BandModel(profile.BandId,
                                        profile.BandName,
                                        database.UserProfiles.Find(profile.CreatorId).UserName,
                                        membersFlag ? MembersFor(profile.BandId) : null);
        }

        public static BandModel BandModelFor(int bandId, bool membersFlag = false)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return BandModelFor(bandId, database, membersFlag);
            }
        }

        public static List<SuperBandModel> SearchByName(String term)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var results = from b in database.BandProfiles
                              join u in database.UserProfiles
                              on b.CreatorId equals u.UserId
                              where b.BandName.Contains(term)
                              select new { b.BandId, b.BandName, u.UserName };

                List<SuperBandModel> bands = new List<SuperBandModel>();

                foreach (var result in results)
                {
                    bands.Add(new SuperBandModel(result.BandId, result.BandName, result.UserName));
                }

                return bands;
            }
        }
    }
}