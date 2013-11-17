﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;
using test.Models;
using WebMatrix.WebData;
using test.Models.Band;
using test.Models.Dashboard;
using test.Models.Calendar;
using test.Models.FileCabinet;
using System.Web;
using System.IO;

namespace test.Stuff
{
    public class TestUtil
    {
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