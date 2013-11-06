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
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            DateTime now = DateTime.UtcNow;
            MonthModel monthModel = new MonthModel(now.Month, now.Year);
            monthModel.Events = CalendarUtil.EventsForMonth(bandId, monthModel.CurrentMonth, monthModel.CurrentMonthYear);

            return View(monthModel);
        }

        public ActionResult Month(int bandId, int month, int year)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            MonthModel monthModel = new MonthModel(month, year);
            monthModel.Events = CalendarUtil.EventsForMonth(bandId, monthModel.CurrentMonth, monthModel.CurrentMonthYear);

            return View("Index", monthModel);
        }

        //
        // GET: /Calendar/AddEvent

        public ActionResult AddEvent(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvent(int bandId, CalendarEventModel model)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
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

                calendarEvent.EventTime = new DateTime(model.EventYear, model.EventMonth, model.EventDay,
                                                            actualHour, model.EventMinute, 0, DateTimeKind.Unspecified);
                calendarEvent.EventDescription = model.EventDescription;

                using (DatabaseContext database = new DatabaseContext())
                {
                    database.CalendarEvents.Add(calendarEvent);
                    database.SaveChanges();
                }

                ViewBag.SuccessMessage = "we added a calendar event and nothing broke";
                return View("Success");
            }

            return View(model);
        }

        public ActionResult EditEvent(int bandId, int eventId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;
            ViewBag.EventId = eventId;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            CalendarEventModel calendarEventModel;

            using (DatabaseContext database = new DatabaseContext())
            {
                CalendarEvent calendarEvent = database.CalendarEvents.Find(eventId);
                calendarEventModel = new CalendarEventModel(calendarEvent);
            }

            return View(calendarEventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(int bandId, int eventId, CalendarEventModel model)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
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
                    calendarEvent.EventTime = new DateTime(model.EventYear, model.EventMonth, model.EventDay,
                                                            actualHour, model.EventMinute, 0, DateTimeKind.Unspecified);

                    database.SaveChanges();
                }

                ViewBag.SuccessMessage = "we edited ur calendar event LOL";
                return View("Success");
            }

            return View(model);
        }


        public ActionResult EventsForMonth(int bandId, int month, int year)
        {
            return View(CalendarUtil.EventsForMonth(bandId, month, year));
        }
    }
}
