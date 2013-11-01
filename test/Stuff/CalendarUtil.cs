using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;
using WebMatrix.WebData;
using System.Web.Helpers;
using System.Data;

namespace test.Stuff
{
    // I doubt anything in here works
    public class BandCalendar
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
    }

    public class CalendarUtil
    {
        public static void EventsForMonth(int month)
        {
            


        }

        public static void AddMessage(int bandId, string content)
        {
            MessageBoardPost post = new MessageBoardPost();
            post.BandId = bandId;
            post.PosterId = WebSecurity.CurrentUserId;
            post.PostTime = DateTime.UtcNow;
            post.Content = content;

            using (DatabaseContext database = new DatabaseContext())
            {
                database.MessageBoardPosts.Add(post);
                database.SaveChanges();
            }
        }

        public static List<MessageBoardPostModel> MessagesFor(int bandId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                var results = from p in database.MessageBoardPosts
                              join u in database.UserProfiles
                              on p.PosterId equals u.UserId
                              where p.BandId == bandId
                              select new { p.PostId, p.PostTime, p.Content, u.DisplayName };

                List<MessageBoardPostModel> postModels = new List<MessageBoardPostModel>();

                foreach (var result in results)
                {
                    MessageBoardPostModel postModel = new MessageBoardPostModel();
                    postModel.PostId = result.PostId;
                    postModel.PostTime = result.PostTime;
                    postModel.Content = result.Content;
                    postModel.PosterName = result.DisplayName;

                    postModels.Add(postModel);
                }

                return postModels;
            }
        }
    }
}