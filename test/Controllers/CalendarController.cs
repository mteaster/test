using System;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.Band;
using test.Models.Calendar;
using test.Stuff;
using WebMatrix.WebData;

namespace band.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index(int bandId)
        {
            DateTime now = DateTime.UtcNow;
            return RedirectToAction("Month", new { bandId = bandId, month = now.Month, year = now.Year });
        }

        public ActionResult Month(int bandId, int month, int year)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            MonthModel monthModel = new MonthModel(month, year);
            monthModel.Events = CalendarUtil.EventsForMonth(bandId, monthModel.CurrentMonth, monthModel.CurrentMonthYear);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View("Index", monthModel);
        }

        public ActionResult AddEvent(int bandId, int day, int month, int year)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            ViewBag.Day = day;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvent(int bandId, int day, int month, int year, CalendarEventModel model)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                CalendarEvent calendarEvent = new CalendarEvent();

                calendarEvent.BandId = bandId;
                calendarEvent.EventTitle = model.EventTitle;    

                int actualHour = model.EventHour;
                
                if(model.EventPeriod.ToUpper() == "PM")
                {
                    actualHour += 12;
                }
                else if (actualHour == 12)
                {
                    actualHour = 0;
                }

                calendarEvent.EventTime = new DateTime(year, month, day, actualHour, model.EventMinute, 0, DateTimeKind.Unspecified);
                calendarEvent.EventDescription = model.EventDescription;

                using (DatabaseContext database = new DatabaseContext())
                {
                    database.CalendarEvents.Add(calendarEvent);
                    database.SaveChanges();
                }

                TempData["SuccessMessage"] = "we added an event";
                return RedirectToAction("Index", new { bandId = bandId });
            }

            return View(model);
        }

        public ActionResult EditEvent(int bandId, int eventId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            ViewBag.EventId = eventId;

            EditEventModel model;

            using (DatabaseContext database = new DatabaseContext())
            {
                model = new EditEventModel(database.CalendarEvents.Find(eventId));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(int bandId, int eventId, EditEventModel model)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    CalendarEvent calendarEvent = database.CalendarEvents.Find(eventId);

                    calendarEvent.EventTitle = model.EventTitle;
                    calendarEvent.EventDescription = model.EventDescription;

                    int actualHour = model.EventHour;
                    if (model.EventPeriod.ToUpper() == "PM")
                    {
                        actualHour += 12;
                    }

                    DateTime date = DateTime.Parse(model.EventDate);
                    TimeSpan span = new TimeSpan(actualHour, model.EventMinute, 0);
                    calendarEvent.EventTime = date.Date + span;

                    database.SaveChanges();
                }

                TempData["SuccessMessage"] = "we edited ur calendar event LOL";
                return RedirectToAction("Index", new { bandId = bandId });
            }

            return View(model);
        }

        public ActionResult DeleteEvent(int eventId)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CalendarEvent calendarEvent = database.CalendarEvents.Find(eventId);

                if (!BandUtil.Authenticate(calendarEvent.BandId, this))
                {
                    return View("Error");
                }

                database.CalendarEvents.Remove(calendarEvent);
                database.SaveChanges();

                TempData["SuccessMessage"] = "we edited ur calendar event LOL";
                return RedirectToAction("Index", new { bandId = calendarEvent.BandId });
            }
        }

        public ActionResult Day(int bandId, int day, int month, int year)
        {
            ViewBag.Day = day;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View(CalendarUtil.EventsForDay(bandId, day, month, year));
        }
    }
}
