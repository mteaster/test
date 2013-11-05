using System;
using System.Web.Mvc;
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
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            MonthModel monthModel = new MonthModel();

            DateTime now = DateTime.UtcNow;
            monthModel.CurrentMonth = now.Month;
            monthModel.CurrentMonthYear = now.Year;

            if (monthModel.CurrentMonth == 1)
            {
                monthModel.PreviousMonth = 12;
                monthModel.PreviousMonthYear = monthModel.CurrentMonthYear - 1;
            }
            else
            {
                monthModel.PreviousMonth = monthModel.CurrentMonth - 1;
                monthModel.PreviousMonthYear = monthModel.CurrentMonthYear;
            }

            if (monthModel.CurrentMonth == 12)
            {
                monthModel.NextMonth = 1;
                monthModel.NextMonthYear = monthModel.CurrentMonthYear + 1;
            }
            else
            {
                monthModel.NextMonth = monthModel.CurrentMonth + 1;
                monthModel.NextMonthYear = monthModel.CurrentMonthYear;
            }

            monthModel.MonthName = "IT'S NOTHING";
            monthModel.DaysInMonth = DateTime.DaysInMonth(monthModel.CurrentMonthYear, monthModel.CurrentMonth);
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


            MonthModel monthModel = new MonthModel();

            monthModel.CurrentMonth = month;
            monthModel.CurrentMonthYear = year;

            if (monthModel.CurrentMonth == 1)
            {
                monthModel.PreviousMonth = 12;
                monthModel.PreviousMonthYear = monthModel.CurrentMonthYear - 1;
            }
            else
            {
                monthModel.PreviousMonth = monthModel.CurrentMonth - 1;
                monthModel.PreviousMonthYear = monthModel.CurrentMonthYear;
            }

            if (monthModel.CurrentMonth == 12)
            {
                monthModel.NextMonth = 1;
                monthModel.NextMonthYear = monthModel.CurrentMonthYear + 1;
            }
            else
            {
                monthModel.NextMonth = monthModel.CurrentMonth + 1;
                monthModel.NextMonthYear = monthModel.CurrentMonthYear;
            }

            monthModel.MonthName = "IT'S NOTHING";
            monthModel.DaysInMonth = DateTime.DaysInMonth(monthModel.CurrentMonthYear, monthModel.CurrentMonth);
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

        public ActionResult EventsForMonth(int bandId, int month, int year)
        {
            return View(CalendarUtil.EventsForMonth(bandId, month, year));
        }

        //
        // Post: /Calendar/AddEvent

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
                calendarEvent.EventTime = new DateTime(model.EventYear, model.EventMonth, model.EventDay);
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
    }
}
