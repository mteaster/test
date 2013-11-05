using System;
using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Calendar;
using test.Models.Band;

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

            DateTime now = DateTime.UtcNow;
            ViewBag.CurrentMonth = now.Month;
            ViewBag.CurrentYear = now.Year;

            if(ViewBag.CurrentMonth == 1)
            {
                ViewBag.PreviousMonth = ViewBag.CurrentMonth = 12;
                ViewBag.PreviousMonthYear = ViewBag.CurrentYear - 1;
            }
            else
            {
                ViewBag.PreviousMonth = ViewBag.CurrentMonth - 1;
                ViewBag.PreviousMonthYear = ViewBag.CurrentYear;
            }

            if (ViewBag.CurrentMonth == 12)
            {
                ViewBag.NextMonth = 1;
                ViewBag.NextMonthYear = ViewBag.CurrentYear + 1;
            }
            else
            {
                ViewBag.NextMonth = ViewBag.CurrentMonth + 1;
                ViewBag.NextMonthYear = ViewBag.CurrentYear;
            }

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            return View(CalendarUtil.EventsForMonth(bandId, ViewBag.Month, ViewBag.Year));
        }

        public ActionResult Month(int bandId, int month, int year)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            DateTime now = DateTime.UtcNow;
            ViewBag.CurrentMonth = month;
            ViewBag.CurrentYear = year;

            if (ViewBag.CurrentMonth == 1)
            {
                ViewBag.PreviousMonth = ViewBag.CurrentMonth = 12;
                ViewBag.PreviousMonthYear = ViewBag.CurrentYear - 1;
            }
            else
            {
                ViewBag.PreviousMonth = ViewBag.CurrentMonth - 1;
                ViewBag.PreviousMonthYear = ViewBag.CurrentYear;
            }

            if (ViewBag.CurrentMonth == 12)
            {
                ViewBag.NextMonth = 1;
                ViewBag.NextMonthYear = ViewBag.CurrentYear + 1;
            }
            else
            {
                ViewBag.NextMonth = ViewBag.CurrentMonth + 1;
                ViewBag.NextMonthYear = ViewBag.CurrentYear;
            }

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            return View("Index", CalendarUtil.EventsForMonth(bandId, month, year));
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
