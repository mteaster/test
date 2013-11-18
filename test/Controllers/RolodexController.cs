using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Models.Rolodex;
using test.Stuff;
using WebMatrix.WebData;

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
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
               using (DatabaseContext database = new DatabaseContext())
               {
                    bandContact.BandId = bandId;
                    database.BandContacts.Add(bandContact);
                    database.SaveChanges();
               }

               return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId });
            }

            return View(bandContact);
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
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    peopleContact.BandId = bandId;
                    database.PeopleContacts.Add(peopleContact);
                    database.SaveChanges();

                    return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId });
                }
            }

            return View(peopleContact);
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
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    venueContact.BandId = bandId;
                    database.VenueContacts.Add(venueContact);
                    database.SaveChanges();
                }

                return RedirectToAction("CreateContact", "Rolodex", new { BandId = bandId });
            }

            return View(venueContact);
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

            ViewBag.ContactId = contactId;

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
            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    BandContact original = database.BandContacts.Find(bandContact.ContactId);
                    database.Entry(original).CurrentValues.SetValues(bandContact);
                    database.SaveChanges();
                }
                return RedirectToAction("Index", new { bandId = bandId });

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

            ViewBag.ContactId = contactId;


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
            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    PeopleContact original = database.PeopleContacts.Find(peopleContact.ContactId);
                    database.Entry(original).CurrentValues.SetValues(peopleContact);
                    database.SaveChanges();
                }
                return RedirectToAction("Index", new { bandId = bandId });

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

            ViewBag.ContactId = contactId;

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

            ViewBag.BandId = bandId;

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    VenueContact original = database.VenueContacts.Find(venueContact.ContactId);
                    database.Entry(original).CurrentValues.SetValues(venueContact);
                    database.SaveChanges();
                }
                return RedirectToAction("Index", new { bandId = bandId });

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
                        bandId = database.PeopleContacts.Find(contactId).BandId;
                        break;
                    case ContactType.Venue:
                        bandId = database.VenueContacts.Find(contactId).BandId;
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
        public ActionResult DownloadBandAvatar(int contactId)
        {
            string path = Server.MapPath("~/App_Data/BandContactAvatars/" + contactId + ".jpg");

            if (System.IO.File.Exists(path))
            {
                return File(path, "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
            }
        }
        public ActionResult DownloadPeopleAvatar(int contactId)
        {
            string path = Server.MapPath("~/App_Data/PeopleContactAvatars/" + contactId + ".jpg");

            if (System.IO.File.Exists(path))
            {
                return File(path, "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
            }
        }
        public ActionResult DownloadVenueAvatar(int contactId)
        {
            string path = Server.MapPath("~/App_Data/VenueContactAvatars/" + contactId + ".jpg");

            if (System.IO.File.Exists(path))
            {
                return File(path, "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/UserAvatars/default.jpg"), "image/jpeg");
            }
        }
        public static List<SelectListItem> GetSelectList(int bandId, string callingType)
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();

            if (callingType.ToUpper().Equals("BAND") || callingType.ToUpper().Equals("VENUE"))
            {
                // the calling page is create/edit band. Show people contacts
                using (DatabaseContext database = new DatabaseContext())
                {
                    var people = from p in database.PeopleContacts
                                 where p.BandId == bandId
                                 select new {p.ContactId, p.Name};

                    foreach (var person in people)
                    {
                        returnValue.Add(new SelectListItem() {Text = person.Name, Value = person.ContactId.ToString()});
                    }

                }
            }
            else if (callingType.ToUpper().Equals("PERSON-BAND"))
            {
                // the calling page is create/edit band. Show people contacts
                using (DatabaseContext database = new DatabaseContext())
                {
                    var bands = from b in database.BandContacts
                                 where b.BandId == bandId
                                 select new { b.ContactId, b.Name };

                    foreach (var band in bands)
                    {
                        returnValue.Add(new SelectListItem() { Text = band.Name, Value = band.ContactId.ToString() });
                    }
                }
            }
            else if (callingType.ToUpper().Equals("PERSON-VENUE"))
            {
                // the calling page is create/edit band. Show people contacts
                using (DatabaseContext database = new DatabaseContext())
                {
                    var venues = from v in database.VenueContacts
                                where v.BandId == bandId
                                select new { v.ContactId, v.Name };

                    foreach (var venue in venues)
                    {
                        returnValue.Add(new SelectListItem() { Text = venue.Name, Value = venue.ContactId.ToString() });
                    }
                }
            }

            return returnValue;
        }
    }
}
