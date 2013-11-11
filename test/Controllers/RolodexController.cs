﻿using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web.Security;
using test.Models.Rolodex;
using System.Collections.Generic;
using System.Linq;

namespace band.Controllers
{
    public class RolodexController : Controller
    {
        //
        // GET: /Rolodex/

        public ActionResult Index(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            // This will also display contacts for the band

            return View();
        }

        public ActionResult CreateContact(int bandId)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            return View();
        }

        public ActionResult CreateBandContact(int BandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(BandId);

            ViewBag.BandId = BandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, BandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            // This will also display contacts for the band
            return PartialView("_CreateBandContact");
        }

        [HttpPost]
        public ActionResult CreateBandContact( BandContact bandContact, int bandId)
        {
            string error = "";
            string success = "";
            bool badData = false;
            
            if (ModelState.IsValid)
            {
                // Validation Code Here
                if (bandContact.Name == "")
                {
                    error = "Please Enter a Valid Name for this contact";
                    badData = true;
                }
                if (bandContact.PhoneNumber == "" && !badData)
                {
                    error = "Please enter in a valid phone number";
                    badData = true;
                }
                if (!bandContact.Email.Contains('@') && !badData)
                {
                    error = "Please Enter a Valid Email Address";
                    badData = true;
                }

                if (!badData) // If all is good, post to DB
                {
                    using (DatabaseContext database = new DatabaseContext())
                    {
                        success = "Contact Added!";
                        bandContact.BandId = bandId;
                        database.BandContacts.Add(bandContact);
                        database.SaveChanges();
                    }
                }
            }

            ViewBag.bandId = bandId;
            ViewBag.SuccessMessage = success;
            ViewBag.ErrorMessage = error;
            return RedirectToAction("CreateContact", "Rolodex", new { bandId = bandId});
        }

        public ActionResult CreatePersonContact(int BandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(BandId);

            ViewBag.BandId = BandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, BandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            // This will also display contacts for the band
            return PartialView("_CreatePersonContact");
        }

        [HttpPost]
        public ActionResult CreatePersonContact(PeopleContact peopleContact, int bandId)
        {
            string error = "";
            string success = "";
            bool badData = false;

            if (ModelState.IsValid)
            {
                // Validation Code Here
                if (peopleContact.Name == "")
                {
                    error = "Please Enter a Valid Name for this contact";
                    badData = true;
                }
                if (peopleContact.PhoneNumber == "" && !badData)
                {
                    error = "Please enter in a valid phone number";
                    badData = true;
                }
                if (!peopleContact.Email.Contains('@') && !badData)
                {
                    error = "Please Enter a Valid Email Address";
                    badData = true;
                }

                if (!badData) // If all is good, post to DB
                {
                    using (DatabaseContext database = new DatabaseContext())
                    {
                        success = "Contact Added!";
                        peopleContact.BandId = bandId;
                        database.PeopleContacts.Add(peopleContact);
                        database.SaveChanges();
                    }
                }
            }

            ViewBag.bandId = bandId;
            ViewBag.SuccessMessage = success;
            ViewBag.ErrorMessage = error;
            return RedirectToAction("CreateContact", "Rolodex", new { bandId = bandId });
        }

        public ActionResult CreateVenueContact(int BandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(BandId);

            ViewBag.BandId = BandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            // If not, redirect to join a band page
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, BandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            // This will also display contacts for the band
            return PartialView("_CreateVenueContact");
        }

        [HttpPost]
        public ActionResult CreateVenueContact(VenueContact venueContact, int bandId)
        {
            string error = "";
            string success = "";
            bool badData = false;

            if (ModelState.IsValid)
            {
                // Validation Code Here
                if (venueContact.Name == "")
                {
                    error = "Please Enter a Valid Name for this contact";
                    badData = true;
                }
                if (venueContact.PhoneNumber == "" && !badData)
                {
                    error = "Please enter in a valid phone number";
                    badData = true;
                }
                if (!venueContact.Email.Contains('@') && !badData)
                {
                    error = "Please Enter a Valid Email Address";
                    badData = true;
                }

                if (!badData) // If all is good, post to DB
                {
                    using (DatabaseContext database = new DatabaseContext())
                    {
                        success = "Contact Added!";
                        venueContact.BandId = bandId;
                        database.VenueContacts.Add(venueContact);
                        database.SaveChanges();
                    }
                }
            }

            ViewBag.bandId = bandId;
            ViewBag.SuccessMessage = success;
            ViewBag.ErrorMessage = error;
            return RedirectToAction("CreateContact", "Rolodex", new { bandId = bandId });
        }

        [ChildActionOnly]
        public ActionResult RolodexList(int bandId)
        {
            List<Contact> contacts = new List<Contact>();
            Contact tmpContact = new Contact();

            using (DatabaseContext db = new DatabaseContext())
            {
                var bandResults = from b in db.BandContacts
                              where b.BandId == bandId
                              select new { b.ContactId, b.Name, b.PhoneNumber, b.Email };


                var peopleResults = from p in db.PeopleContacts
                                  where p.BandId == bandId
                                  select new { p.ContactId, p.Name, p.PhoneNumber, p.Email };


                var venueResults = from v in db.VenueContacts
                                  where v.BandId == bandId
                                  select new { v.ContactId, v.Name, v.PhoneNumber, v.Email };


                if (bandResults != null)
                {
                    foreach (var band in bandResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = band.ContactId;
                        tmpContact.Name = band.Name;
                        tmpContact.PhoneNumber = band.PhoneNumber;
                        tmpContact.Email = band.Email;
                        tmpContact.Type = Contact.ContactType.Band;

                        contacts.Add(tmpContact);
                    }
                }


                if (peopleResults != null)
                {
                    foreach (var person in peopleResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = person.ContactId;
                        tmpContact.Name = person.Name;
                        tmpContact.PhoneNumber = person.PhoneNumber;
                        tmpContact.Email = person.Email;
                        tmpContact.Type = Contact.ContactType.People;

                        contacts.Add(tmpContact);
                    }
                }


                if (venueResults != null)
                {
                    foreach (var venue in venueResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = venue.ContactId;
                        tmpContact.Name = venue.Name;
                        tmpContact.PhoneNumber = venue.PhoneNumber;
                        tmpContact.Email = venue.Email;
                        tmpContact.Type = Contact.ContactType.Venue;

                        contacts.Add(tmpContact);
                    }
                }
            }


            // sort list by name, return it to view
            contacts.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));


            return PartialView("_RolodexList", contacts);
        }

        [ChildActionOnly]
        [HttpPost]
        public ActionResult RolodexList(int bandId)
        {
            List<Contact> contacts = new List<Contact>();
            Contact tmpContact = new Contact();
            string searchString = "";

            searchString = Request["searchString"].ToString();

            using (DatabaseContext db = new DatabaseContext())
            {
                var bandResults = from b in db.BandContacts
                                  where b.BandId == bandId
                                  && (b.Name.Contains(searchString))
                                  select new { b.ContactId, b.Name, b.PhoneNumber, b.Email };


                var peopleResults = from p in db.PeopleContacts
                                    where p.BandId == bandId
                                  && (p.Name.Contains(searchString))
                                    select new { p.ContactId, p.Name, p.PhoneNumber, p.Email };


                var venueResults = from v in db.VenueContacts
                                   where v.BandId == bandId
                                  && (p.Name.Contains(searchString))
                                   select new { v.ContactId, v.Name, v.PhoneNumber, v.Email };


                if (bandResults != null)
                {
                    foreach (var band in bandResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = band.ContactId;
                        tmpContact.Name = band.Name;
                        tmpContact.PhoneNumber = band.PhoneNumber;
                        tmpContact.Email = band.Email;
                        tmpContact.Type = Contact.ContactType.Band;

                        contacts.Add(tmpContact);
                    }
                }


                if (peopleResults != null)
                {
                    foreach (var person in peopleResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = person.ContactId;
                        tmpContact.Name = person.Name;
                        tmpContact.PhoneNumber = person.PhoneNumber;
                        tmpContact.Email = person.Email;
                        tmpContact.Type = Contact.ContactType.People;

                        contacts.Add(tmpContact);
                    }
                }


                if (venueResults != null)
                {
                    foreach (var venue in venueResults)
                    {
                        tmpContact = new Contact();

                        tmpContact.ContactId = venue.ContactId;
                        tmpContact.Name = venue.Name;
                        tmpContact.PhoneNumber = venue.PhoneNumber;
                        tmpContact.Email = venue.Email;
                        tmpContact.Type = Contact.ContactType.Venue;

                        contacts.Add(tmpContact);
                    }
                }
            }


            // sort list by name, return it to view
            contacts.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));


            return PartialView("_RolodexList", contacts);
        }

    }
}
