using System.Collections.Generic;
using System.Web.Mvc;
using test.Models;
using test.Stuff;
using System.Linq;
using System;
using test.Models.Budget;
using System.Data;
using test.Models.Rolodex;

namespace band.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        [ActionName("Index")]
        public ActionResult Index(int bandId)
        {
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
            decimal totalAP = 0;
            decimal totalAR = 0;
            decimal totalDifference = 0;

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            ViewBag.Filters = new test.Models.Budget.IndexFilters { StartDT = DateTime.Now.Date, EndDT = DateTime.Now.Date.AddDays(1) };

            accountPayables = GetAccountsPayableList(bandId, ViewBag.Filters);
            accountReceivables = GetAccountsReceivableList(bandId, ViewBag.Filters);

            foreach (AccountPayables ap in accountPayables)
            {
                totalAP += ap.Amount;
            }

            foreach (AccountReceivables ar in accountReceivables)
            {
                totalAR += ar.Amount;
            }

            totalDifference = totalAR - totalAP;

            ViewBag.TotalAP = totalAP;
            ViewBag.TotalAR = totalAR;
            ViewBag.TotalDifference = totalDifference;
            if (totalDifference >= 0)
            {
                ViewBag.DifferenceColor = "green";
            }
            else
            {
                ViewBag.DifferenceColor = "red";
            }

            return View(ViewBag.Filters);
        }

        [ActionName("Index")]
        [HttpPost]
        public ActionResult Index(int bandId, test.Models.Budget.IndexFilters filters, string sort)
        {
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
            decimal totalAP = 0;
            decimal totalAR = 0;
            decimal totalDifference = 0;

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            ViewBag.Filters = filters;

            accountPayables = GetAccountsPayableList(bandId, ViewBag.Filters);
            accountReceivables = GetAccountsReceivableList(bandId, ViewBag.Filters);

            foreach (AccountPayables ap in accountPayables)
            {
                totalAP += ap.Amount;
            }

            foreach (AccountReceivables ar in accountReceivables)
            {
                totalAR += ar.Amount;
            }

            totalDifference = totalAR - totalAP;

            ViewBag.TotalAP = totalAP;
            ViewBag.TotalAR = totalAR;
            ViewBag.TotalDifference = totalDifference;
            if (totalDifference >= 0)
            {
                ViewBag.DifferenceColor = "green";
            }
            else
            {
                ViewBag.DifferenceColor = "red";
            }


            return View(filters);
        }

        public ActionResult MerchandiseList(int bandId)
        {
            List<test.Models.Budget.Merchandise> merchandise = new List<test.Models.Budget.Merchandise>();

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }
            using (DatabaseContext db = new DatabaseContext())
            {
                var mList = from p in db.Merchandise
                             where p.BandId == bandId
                             select new { p.MerchandiseId, p.Name, p.Category, p.Size, p.BandId};

                foreach (var m in mList)
                {
                    test.Models.Budget.Merchandise tempM = new test.Models.Budget.Merchandise();

                    tempM.MerchandiseId = m.MerchandiseId;
                    tempM.Name = m.Name;
                    tempM.Category = m.Category;
                    tempM.Size = m.Size;
                    tempM.BandId = m.BandId;


                    merchandise.Add(tempM);
                }
            }

            return PartialView("_MerchandiseListPartial", merchandise);

        }

        public ActionResult AccountsPayableList(int bandId, test.Models.Budget.IndexFilters filters)
        {
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (filters != null)
            {
                ViewBag.Filters = filters;
            }

            accountPayables = GetAccountsPayableList(bandId, filters);

            return PartialView("_AccountsPayableListPartial", accountPayables);

        }

        public List<test.Models.Budget.AccountPayables> GetAccountsPayableList(int bandId, test.Models.Budget.IndexFilters filters)
        {
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();

            using (DatabaseContext db = new DatabaseContext())
            {
                var apList = from p in db.AccountPayables
                             where p.BandId == bandId
                             select new { p.AccountPayableId, p.Amount, p.AssociatedBandContactId, p.AssociatedPersonContactId, p.AssociatedVenueContactId, p.BandId, p.Category, p.Date, p.Paid };

                foreach (var ap in apList)
                {
                    test.Models.Budget.AccountPayables tempAP = new test.Models.Budget.AccountPayables();

                    tempAP.AccountPayableId = ap.AccountPayableId;
                    tempAP.Amount = ap.Amount;
                    tempAP.AssociatedBandContactId = ap.AssociatedBandContactId;
                    tempAP.AssociatedBandName = FindContactName(tempAP.AssociatedBandContactId, "BAND");
                    tempAP.AssociatedPersonContactId = ap.AssociatedPersonContactId;
                    tempAP.AssociatedPersonName = FindContactName(tempAP.AssociatedPersonContactId, "PEOPLE");
                    tempAP.AssociatedVenueContactId = ap.AssociatedVenueContactId;
                    tempAP.AssociatedVenueName = FindContactName(tempAP.AssociatedVenueContactId, "VENUE");
                    tempAP.BandId = ap.BandId;
                    tempAP.Category = ap.Category;
                    tempAP.Date = ap.Date;
                    tempAP.Paid = ap.Paid;

                    if ((!string.IsNullOrEmpty(filters.Category) && tempAP.Category.Contains(filters.Category)) || string.IsNullOrEmpty(filters.Category))
                    {
                        if ((filters.Paid && tempAP.Paid) || (filters.Unpaid && !tempAP.Paid) || (!filters.Paid && !filters.Unpaid) || (filters.Paid && filters.Unpaid))
                        {
                            if ((filters.StartDT <= tempAP.Date && filters.EndDT >= tempAP.Date))
                            {
                                accountPayables.Add(tempAP);
                            }
                        }
                    }
                }
            }

            return accountPayables;
        }

        public ActionResult AccountsReceivableList(int bandId, test.Models.Budget.IndexFilters filters)
        {
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            accountReceivables = GetAccountsReceivableList(bandId, filters);

            return PartialView("_AccountsReceivableListPartial", accountReceivables);

        }

        public List<test.Models.Budget.AccountReceivables> GetAccountsReceivableList(int bandId, test.Models.Budget.IndexFilters filters)
        {
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
            

            using (DatabaseContext db = new DatabaseContext())
            {

                var arList = from r in db.AccountReceivables
                             where r.BandId == bandId
                             select new { r.AccountReceivableId, r.Amount, r.AssociatedBandContactId, r.AssociatedPersonContactId, r.AssociatedVenueContactId, r.BandId, r.Category, r.Date, r.Paid };
                foreach (var ar in arList)
                {
                    test.Models.Budget.AccountReceivables tempAR = new test.Models.Budget.AccountReceivables();

                    tempAR.AccountReceivableId = ar.AccountReceivableId;
                    tempAR.Amount = ar.Amount;
                    tempAR.AssociatedBandContactId = ar.AssociatedBandContactId;
                    tempAR.AssociatedBandName = FindContactName(tempAR.AssociatedBandContactId, "BAND");
                    tempAR.AssociatedPersonContactId = ar.AssociatedPersonContactId;
                    tempAR.AssociatedPersonName = FindContactName(tempAR.AssociatedPersonContactId, "PEOPLE");
                    tempAR.AssociatedVenueContactId = ar.AssociatedVenueContactId;
                    tempAR.AssociatedVenueName = FindContactName(tempAR.AssociatedVenueContactId, "VENUE");
                    tempAR.BandId = ar.BandId;
                    tempAR.Category = ar.Category;
                    tempAR.Date = ar.Date;
                    tempAR.Paid = ar.Paid;

                    if ((!string.IsNullOrEmpty(filters.Category) && tempAR.Category.Contains(filters.Category)) || string.IsNullOrEmpty(filters.Category))
                    {
                        if ((filters.Paid && tempAR.Paid) || (filters.Unpaid && !tempAR.Paid) || (!filters.Paid && !filters.Unpaid) || (filters.Paid && filters.Unpaid))
                        {
                            if ((filters.StartDT <= tempAR.Date && filters.EndDT >= tempAR.Date))
                            {
                                accountReceivables.Add(tempAR);
                            }
                        }
                    }
                }
            }
            return accountReceivables;
        }

        public ActionResult MerchList(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();

        }

        public ActionResult CreateMerchList(int bandId, test.Models.Budget.Merchandise m)
        {

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    m.BandId = bandId;
                    database.Merchandise.Add(m);
                    database.SaveChanges();
                }

                return RedirectToAction("MerchList", "Budget", new { BandId = bandId });
            }

            return RedirectToAction("AddMerch", new { bandId = bandId });
        }

        public ActionResult AccountPayable(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();

        }

        public ActionResult AccountReceivable(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();

        }

        public ActionResult CreateAccountPayable(int bandId, test.Models.Budget.AccountPayables ap)
        {

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

                using (DatabaseContext database = new DatabaseContext())
                {
                    ap.BandId = bandId;
                    database.AccountPayables.Add(ap);
                    database.SaveChanges();
                }

                return RedirectToAction("Index", "Budget", new { BandId = bandId });
        }

        public ActionResult CreateAccountReceivable(int bandId, test.Models.Budget.AccountReceivables ar)
        {

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

                using (DatabaseContext database = new DatabaseContext())
                {
                    ar.BandId = bandId;
                    database.AccountReceivables.Add(ar);
                    database.SaveChanges();
                }

                return RedirectToAction("Index", "Budget", new { BandId = bandId });
        }

        public ActionResult UpdateAccountPayable(int id, bool newValue)
        {

            using (DatabaseContext database = new DatabaseContext())
            {
                AccountPayables original = database.AccountPayables.Find(id);
                original.Paid = newValue;
                database.Entry(original).State = EntityState.Modified;
                database.SaveChanges();
            }

            return Json(true);
        }

        public ActionResult UpdateAccountReceivable(int id, bool newValue)
        {

            using (DatabaseContext database = new DatabaseContext())
            {
                AccountReceivables original = database.AccountReceivables.Find(id);
                original.Paid = newValue;
                database.Entry(original).State = EntityState.Modified;
                database.SaveChanges();
            }

            return Json(true);
        }

        public string FindContactName(int contactId, string type)
        {
            string returnValue = "";

            using (DatabaseContext database = new DatabaseContext())
            {
                switch (type.ToUpper())
                {
                    case "BAND":
                        BandContact b = database.BandContacts.Find(contactId);
                        if (b != null)
                        {
                            returnValue = b.Name;
                        }
                        break;
                    case "PEOPLE":
                        PeopleContact p = database.PeopleContacts.Find(contactId);
                        if (p != null)
                        {
                            returnValue = p.Name;
                        }
                        break;
                    case "VENUE":
                        VenueContact v = database.VenueContacts.Find(contactId);
                        if (v != null)
                        {
                            returnValue = v.Name;
                        }
                        break;
                }
            }

            return returnValue;
        }

        public static List<SelectListItem> GetSelectList(int bandId, string type)
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();

            if (type.ToUpper().Equals("PEOPLE"))
            {
                // the calling page is create/edit band. Show people contacts
                using (DatabaseContext database = new DatabaseContext())
                {
                    var people = from p in database.PeopleContacts
                                 where p.BandId == bandId
                                 select new { p.ContactId, p.Name };

                    foreach (var person in people)
                    {
                        returnValue.Add(new SelectListItem() { Text = person.Name, Value = person.ContactId.ToString() });
                    }

                }
            }
            else if (type.ToUpper().Equals("BAND"))
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
            else if (type.ToUpper().Equals("VENUE"))
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
