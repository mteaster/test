﻿using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;
using test.Models.Band;
using System.Web.Security;
using test.Models.Rolodex;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace band.Controllers
{
    public class RolodexController : Controller
    {
        //
        // GET: /Rolodex/

        public ActionResult Index(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            // This will also display contacts for the band

            return View();
        }

        public ActionResult CreateContact(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();
        }

        
        public ActionResult CreateBandContact(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            // This will also display contacts for the band
            return View();
        }


        [HttpPost]
        public ActionResult CreateBandContact(BandContact bandContact, int bandId)
        {  
            if (ModelState.IsValid)
            {
               
               using (DatabaseContext database = new DatabaseContext())
               {
                    bandContact.BandId = bandId;
                    database.BandContacts.Add(bandContact);
                    database.SaveChanges();
               }
            }
        

            ViewBag.BandId = bandId;
            return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId});
        }

        public ActionResult CreatePersonContact(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            // This will also display contacts for the band
            return View();
        }

        [HttpPost]
        public ActionResult CreatePersonContact(PeopleContact peopleContact, int bandId)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                    {
                        peopleContact.BandId = bandId;
                        database.PeopleContacts.Add(peopleContact);
                        database.SaveChanges();
                    }
            }
            ViewBag.BandId = bandId;
            return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId });
        }

        public ActionResult CreateVenueContact(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            // This will also display contacts for the band
            return View();
        }

        [HttpPost]
        public ActionResult CreateVenueContact(VenueContact venueContact, int bandId)
        {
            if (ModelState.IsValid)
            {
                    using (DatabaseContext database = new DatabaseContext())
                    {
                        venueContact.BandId = bandId;
                        database.VenueContacts.Add(venueContact);
                        database.SaveChanges();
                    }
                
            }

           
            return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId });
        }

        [ActionName("RolodexList")]
        public ActionResult RolodexListGet(int bandId)
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
                        tmpContact.Type = ContactType.Band;

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
                        tmpContact.Type = ContactType.People;

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
                        tmpContact.Type = ContactType.Venue;

                        contacts.Add(tmpContact);
                    }
                }
            }


            // sort list by name, return it to view
            contacts.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));


            return PartialView("_RolodexList", contacts);
        }

        [HttpPost]
        [ActionName("RolodexList")]
        public ActionResult RolodexListPost(int bandId)
        {
            List<Contact> contacts = new List<Contact>();
            Contact tmpContact = new Contact();
            string searchString = "";
            string type = "";

            searchString = Request["searchString"].ToString();
            if (Request["type"] != null)
            {
                type = Request["type"].ToString();
            }


            using (DatabaseContext db = new DatabaseContext())
            {
                if (!string.IsNullOrEmpty(type) && type.Equals("band"))
                {
                    var bandResults = from b in db.BandContacts
                                      where b.BandId == bandId
                                      && (b.Name.Contains(searchString))
                                      select new { b.ContactId, b.Name, b.PhoneNumber, b.Email };


                    if (bandResults != null)
                    {
                        foreach (var band in bandResults)
                        {
                            tmpContact = new Contact();

                            tmpContact.ContactId = band.ContactId;
                            tmpContact.Name = band.Name;
                            tmpContact.PhoneNumber = band.PhoneNumber;
                            tmpContact.Email = band.Email;
                            tmpContact.Type = ContactType.Band;

                            contacts.Add(tmpContact);
                        }
                    }
                }
                
                if (!string.IsNullOrEmpty(type) && type.Equals("people"))
                {
                    var peopleResults = from p in db.PeopleContacts
                                    where p.BandId == bandId
                                  && (p.Name.Contains(searchString))
                                    select new { p.ContactId, p.Name, p.PhoneNumber, p.Email };

                    if (peopleResults != null)
                    {
                        foreach (var person in peopleResults)
                        {
                            tmpContact = new Contact();

                            tmpContact.ContactId = person.ContactId;
                            tmpContact.Name = person.Name;
                            tmpContact.PhoneNumber = person.PhoneNumber;
                            tmpContact.Email = person.Email;
                            tmpContact.Type = ContactType.People;

                            contacts.Add(tmpContact);
                        }
                    }
                }


                if (!string.IsNullOrEmpty(type) && type.Equals("venue"))
                {
                    var venueResults = from v in db.VenueContacts
                                       where v.BandId == bandId
                                      && (v.Name.Contains(searchString))
                                       select new { v.ContactId, v.Name, v.PhoneNumber, v.Email };

                    if (venueResults != null)
                    {
                        foreach (var venue in venueResults)
                        {
                            tmpContact = new Contact();

                            tmpContact.ContactId = venue.ContactId;
                            tmpContact.Name = venue.Name;
                            tmpContact.PhoneNumber = venue.PhoneNumber;
                            tmpContact.Email = venue.Email;
                            tmpContact.Type = ContactType.Venue;

                            contacts.Add(tmpContact);
                        }
                    }
                }

                if (type == "")
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
                                      && (v.Name.Contains(searchString))
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
                            tmpContact.Type = ContactType.Band;

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
                            tmpContact.Type = ContactType.People;

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
                            tmpContact.Type = ContactType.Venue;

                            contacts.Add(tmpContact);
                        }
                    }
                }
               


            }


            // sort list by name, return it to view
            contacts.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));


            return PartialView("_RolodexList", contacts);
        }

        public ActionResult EditContact(int bandId, int contactId, ContactType type)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (type == ContactType.Band)
            {
                return RedirectToAction("EditBand", new { bandId = bandId, contactId = contactId});
                
            }
            else if (type == ContactType.People)
            {

                return RedirectToAction("EditPeople", new { bandId = bandId, contactId = contactId });
            }
            else if (type == ContactType.Venue)
            {
                return RedirectToAction("EditVenue", new { bandId = bandId, contactId = contactId });
            }
            else
            {
                return View("Error");
            }
        }

        [ActionName("EditBand")]
        public ActionResult EditBandGet(int bandId, int contactId)
        {
            BandContact bContact;

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                bContact = db.BandContacts.Find(contactId);
            }
            if (bContact != null)
            {
                return View(bContact);
            }
            else
            {
                return View("Error");
            }

        }

        [HttpPost]
        [ActionName("EditBand")]
        public ActionResult EditBandPost(int bandId, BandContact bandContact)
        {
            BandContact original;

            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    original = database.BandContacts.Find(bandContact.ContactId);

                    original.Email = bandContact.Email;
                    original.MusicalStyle = bandContact.MusicalStyle;
                    original.Name = bandContact.Name;
                    original.Notes = bandContact.Notes;
                    original.PhoneNumber = bandContact.PhoneNumber;
                    original.Picture = bandContact.Picture;
                    original.PopularityLevel = bandContact.PopularityLevel;
                    original.PrimaryPeopleContactId = bandContact.PrimaryPeopleContactId;
                    original.SkillLevel = bandContact.SkillLevel;

                    database.Entry(original).State = EntityState.Modified;
                    database.SaveChanges();
                }
                return View("Index");

            }
            else
            {
                return View("EditBand");
            }
        }

        [ActionName("EditPeople")]
        public ActionResult EditPeopleGet(int bandId, int contactId)
        {
            PeopleContact pContact;

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                pContact = db.PeopleContacts.Find(contactId);
            }

            return View(pContact);
        }

        [HttpPost]
        [ActionName("EditPeople")]
        public ActionResult EditPeoplePost(int bandId, PeopleContact peopleContact)
        {
            PeopleContact original;

            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    original = database.PeopleContacts.Find(peopleContact.ContactId);

                    original.Email = peopleContact.Email;
                    original.Name = peopleContact.Name;
                    original.Notes = peopleContact.Notes;
                    original.PhoneNumber = peopleContact.PhoneNumber;
                    original.Picture = peopleContact.Picture;
                    original.AssociatedContactId = peopleContact.AssociatedContactId;
                    original.AssociatedContactTypeValue = peopleContact.AssociatedContactTypeValue;
                    original.Skill = peopleContact.Skill;

                    database.Entry(original).State = EntityState.Modified;
                    database.SaveChanges();
                }
                return View("Index");

            }
            else
            {
                return View("EditPeople");
            }
        }

        [ActionName("EditVenue")]
        public ActionResult EditVenueGet(int bandId, int contactId)
        {
            VenueContact vContact;

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                vContact = db.VenueContacts.Find(contactId);
            }

            return View(vContact);
        }

        [HttpPost]
        [ActionName("EditVenue")]
        public ActionResult EditVenuePost(int bandId, VenueContact venueContact)
        {

            VenueContact original;

            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    original = database.VenueContacts.Find(venueContact.ContactId);

                    original.Email = venueContact.Email;
                    original.Name = venueContact.Name;
                    original.Notes = venueContact.Notes;
                    original.PhoneNumber = venueContact.PhoneNumber;
                    original.Picture = venueContact.Picture;
                    original.CoverCharge = venueContact.CoverCharge;
                    original.FreeBeer = venueContact.FreeBeer;
                    original.MerchSpace = venueContact.MerchSpace;
                    original.PrimaryPeopleContactId = venueContact.PrimaryPeopleContactId;
                    original.StageSizeValue = venueContact.StageSizeValue;

                    database.Entry(original).State = EntityState.Modified;
                    database.SaveChanges();
                }
                return View("Index");

            }
            else
            {
                return View("EditVenue");
            }
        }

        [HttpPost]
        public ActionResult UploadAvatar(int contactId, ContactType contactType, HttpPostedFileBase file)
        {
            if (file.ContentLength <= 0 || file.ContentLength > 1048576)
            {
                ViewBag.ErrorMessage = "file sucks";
                return View("Error");
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                int bandId;

                switch (contactType)
                {
                    case ContactType.Band:
                        bandId = database.BandContacts.Find(contactId).BandId;
                        break;
                    case ContactType.People:
                        bandId = database.BandContacts.Find(contactId).BandId;
                        break;
                    case ContactType.Venue:
                        bandId = database.BandContacts.Find(contactId).BandId;
                        break;
                    default:
                        return View("Error");
                }

                if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
                {
                    return RedirectToAction("Join", "Band");
                }

                string path = Server.MapPath("~/App_Data/" + contactType.ToString() + "ContactAvatars/" + contactId + ".jpg");
                file.SaveAs(path);
                ViewData["SuccessMessage"] = "Avatar changed.";
                return View("Success");
            }
        }
    }
}
