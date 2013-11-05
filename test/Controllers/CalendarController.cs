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
