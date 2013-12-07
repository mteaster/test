﻿using System.Collections.Generic;
using System.Web.Mvc;
using test.Models;
using test.Stuff;
using System.Linq;

namespace band.Controllers
{
    public class BudgetController : Controller
    {
        public ActionResult Index(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();
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

        public ActionResult AccountsPayableList(int bandId)
        {
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }
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
                    tempAP.AssociatedPersonContactId = ap.AssociatedPersonContactId;
                    tempAP.AssociatedVenueContactId = ap.AssociatedVenueContactId;
                    tempAP.BandId = ap.BandId;
                    tempAP.Category = ap.Category;
                    tempAP.Date = ap.Date;
                    tempAP.Paid = ap.Paid;

                    accountPayables.Add(tempAP);
                }
            }

            return PartialView("_AccountsPayableListPartial", accountPayables);

        }

        public ActionResult AccountsReceivableList(int bandId)
        {
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

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
                    tempAR.AssociatedPersonContactId = ar.AssociatedPersonContactId;
                    tempAR.AssociatedVenueContactId = ar.AssociatedVenueContactId;
                    tempAR.BandId = ar.BandId;
                    tempAR.Category = ar.Category;
                    tempAR.Date = ar.Date;
                    tempAR.Paid = ar.Paid;

                    accountReceivables.Add(tempAR);
                }
            }

            return PartialView("_AccountsReceivableListPartial", accountReceivables);

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

            return RedirectToAction("NewMerch", new { bandId = bandId });
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

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    ap.BandId = bandId;
                    database.AccountPayables.Add(ap);
                    database.SaveChanges();
                }

                return RedirectToAction("Index", "Budget", new { BandId = bandId });
            }

            return RedirectToAction("AccountPayable", new { bandId = bandId });
        }

        public ActionResult CreateAccountReceivable(int bandId, test.Models.Budget.AccountReceivables ar)
        {

            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    ar.BandId = bandId;
                    database.AccountReceivables.Add(ar);
                    database.SaveChanges();
                }

                return RedirectToAction("Index", "Budget", new { BandId = bandId });
            }

            return RedirectToAction("AccountReceivable", new { bandId = bandId });
        }
    }
}
