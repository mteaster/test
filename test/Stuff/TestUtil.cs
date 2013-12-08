using System;
using System.IO;
using System.Web;
using System.Web.Helpers;
using test.Models;
using test.Models.Band;
using test.Models.Dashboard;
using test.Models.FileCabinet;
using System.Linq;
using WebMatrix.WebData;

namespace test.Stuff
{
    public class TestUtil
    {
        public static const string alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static int RandomBand()
        {
            string creatorUserName = RandomWord();
            string creatorDisplayName = RandomWord();
            WebSecurity.CreateUserAndAccount(creatorUserName, "password", new { DisplayName = creatorDisplayName });
            return 1;
        }

        public static void MakeAccount(string userName, string displayName)
        {
            WebSecurity.CreateUserAndAccount(userName, "password", new { DisplayName = displayName })
        }

        public static string RandomWord()
        {
            Random random = new Random();
            return new string(
                Enumerable.Repeat(alphanumeric, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        public static int MakeBand(string bandName, int creatorId, string password)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                BandProfile profile = new BandProfile(bandName, creatorId, Crypto.HashPassword(password));

                database.BandProfiles.Add(profile);

                BandMembership membership = new BandMembership(profile.BandId, creatorId);
                database.BandMemberships.Add(membership);

                database.SaveChanges();

                return profile.BandId;
            }
        }

        public static void PutInBand(int bandId, int userId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (database.BandProfiles.Find(bandId) == null)
                {
                    throw new BandNotFoundException();
                }

                BandMembership membership = new BandMembership(bandId, userId);
                database.BandMemberships.Add(membership);
                database.SaveChanges();
            }
        }

        public static void DeleteUserAvatars(HttpServerUtilityBase server)
        {
            DirectoryInfo userAvatarDirectory = new DirectoryInfo(server.MapPath("~/App_Data/UserAvatars/"));

            foreach (FileInfo file in userAvatarDirectory.GetFiles())
            {
                file.Delete();
            }
        }

        public static void GiveUserAvatar(int userId, string avatarFileName, HttpServerUtilityBase server)
        {
            string sourcePath = server.MapPath("~/App_Data/TestData/" + avatarFileName);
            string destinationPath = server.MapPath("~/App_Data/UserAvatars/" + userId + ".jpg");

            File.Copy(sourcePath, destinationPath, true);
        }

        public static void GiveBandAvatar(int bandId, string avatarFileName, HttpServerUtilityBase server)
        {
            string sourcePath = server.MapPath("~/App_Data/TestData/" + avatarFileName);
            string destinationPath = server.MapPath("~/App_Data/BandAvatars/" + bandId + ".jpg");

            File.Copy(sourcePath, destinationPath, true);
        }

        public static int MakeMessage(int posterId, int bandId, string content)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                MessageBoardPost post = new MessageBoardPost(bandId, posterId, PostType.Message, DateTime.UtcNow, content);
                database.MessageBoardPosts.Add(post);
                database.SaveChanges();
                return post.PostId;
            }
        }

        public static int MakeGroup(int bandId, string groupName)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                FileGroup group = new FileGroup(bandId, groupName);
                database.FileGroups.Add(group);
                database.SaveChanges();
                return group.GroupId;
            }
        }

        //public static int MakeFile(int posterId, int bandId, int groupId, string content)
        //{
        //    using (DatabaseContext database = new DatabaseContext())
        //    {
        //        FileEntry entry = new FileEntry(fileName, 
        //        database.SaveChanges();
        //    }
        //}

        // I doubt anything in here works
        /*public class BandCalendar
        {
            public DateTime LastAccessTime { get; set; }
            public int BandId { get; set; }
            List<CalendarEvent> bandEvents;

            public BandCalendar(int bandId, List<CalendarEvent> bandEvents)
            {
                this.BandId = bandId;
                this.bandEvents = bandEvents;
                this.LastAccessTime = DateTime.UtcNow;
            }
        
            public void AddEvent(CalendarEvent calendarEvent)
            {
                bandEvents.Add(calendarEvent);
                LastAccessTime = DateTime.UtcNow;
            }

            public void DeleteEvent(int eventId)
            {
                CalendarEvent bandEvent = bandEvents.Find(e => e.EventId == eventId);

                if (bandEvent != null)
                {
                    bandEvents.Remove(bandEvents.Find(e => e.EventId == eventId));
                }
            }

            public List<CalendarEvent> EventsForMonth(int month, int year)
            {
                LastAccessTime = DateTime.UtcNow;
                return bandEvents.Where(e => e.EventTime.Month == month && e.EventTime.Year == year).ToList();
            }
        }

        public class Calendars
        {
            const int CAPACITY = 10;
            private Object cacheLock = new Object();

            private Dictionary<int, BandCalendar> bandCalendars = new Dictionary<int, BandCalendar>();

            public void AddEvent(int bandId, CalendarEvent calendarEvent)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    database.CalendarEvents.Add(calendarEvent);
                    database.SaveChanges();
                }

                lock (cacheLock)
                {
                    if (bandCalendars.ContainsKey(bandId))
                    {
                        bandCalendars[bandId].AddEvent(calendarEvent);
                    }
                }
            }

            public void DeleteEvent(int bandId, int eventId)
            {
                lock (cacheLock)
                {
                    if (bandCalendars.ContainsKey(bandId))
                    {
                        bandCalendars[bandId].DeleteEvent(eventId);
                    }
                }
            }

            public List<CalendarEvent> EventsForMonth(int bandId, int month, int year)
            {
                lock (cacheLock)
                {
                    if (!bandCalendars.ContainsKey(bandId))
                    {
                        List<CalendarEvent> eventsForBand;

                        using (DatabaseContext database = new DatabaseContext())
                        {
                            eventsForBand = database.CalendarEvents.Where(e => e.BandId == bandId).ToList();
                        }

                        bandCalendars.Add(bandId, new BandCalendar(bandId, eventsForBand));
                    }

                    return bandCalendars[bandId].EventsForMonth(month, year);
                }
            }
        }*/

    }
}