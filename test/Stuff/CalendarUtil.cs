using System.Collections.Generic;
using System.Linq;
using test.Models;
using test.Models.Calendar;

namespace test.Stuff
{
    public class CalendarUtil
    {
        public static List<CalendarEvent> EventsForMonth(int bandId, int month, int year)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return database.CalendarEvents.Where(c => c.EventTime.Month == month
                                        && c.EventTime.Year == year
                                        && c.BandId == bandId).ToList();
            }
        }

        public static List<CalendarEvent> EventsForDay(int bandId, int day, int month, int year)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                return database.CalendarEvents.Where(c => c.EventTime.Day == day
                                        && c.EventTime.Month == month
                                        && c.EventTime.Year == year
                                        && c.BandId == bandId).ToList();
            }
        }
    }
}