using System.Web.Mvc;
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
                            tmpContact.Type = Contact.ContactType.Band;

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
                            tmpContact.Type = Contact.ContactType.People;

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
                            tmpContact.Type = Contact.ContactType.Venue;

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
               


            }


            // sort list by name, return it to view
            contacts.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));


            return PartialView("_RolodexList", contacts);
        }

        public ActionResult EditContact(int bandId, int contactId, Contact.ContactType type)
        {
            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            if (type == Contact.ContactType.Band)
            {
                return RedirectToAction("EditBand", new { bandId = bandId, contactId = contactId});
                
            }
            else if (type == Contact.ContactType.People)
            {

                return RedirectToAction("EditPeople", new { bandId = bandId, contactId = contactId });
            }
            else if (type == Contact.ContactType.Venue)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    VenueContact vContact = db.VenueContacts.Find(contactId);
                }
                return EditVenue(bandId, contactId);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult EditBand(int bandId, int contactId)
        {
            BandContact bContact;

            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
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

        public ActionResult EditPeople(int bandId, int contactId)
        {
            PeopleContact pContact;

            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                pContact = db.PeopleContacts.Find(contactId);
            }

            return View(pContact);
        }

        public ActionResult EditVenue(int bandId, int contactId)
        {
            VenueContact vContact;

            ViewBag.BandId = bandId;
            ViewBag.BandName = BandUtil.BandProfileFor(bandId).BandName;

            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId) && !Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("Join", "Band");
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                vContact = db.VenueContacts.Find(contactId);
            }

            return View(vContact);
        }

    }
}
