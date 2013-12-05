using System.Collections.Generic;
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
            List<test.Models.Budget.AccountPayables> accountPayables = new List<test.Models.Budget.AccountPayables>();
            List<test.Models.Budget.AccountReceivables> accountReceivables = new List<test.Models.Budget.AccountReceivables>();
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

            ViewBag.accountsPayableList = accountPayables;
            ViewBag.accountsReceivableList = accountReceivables;

            return View();
        }

        public ActionResult AccountsPayableList(int bandId, List<test.Models.Budget.AccountPayables> apList)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return PartialView("_AccountsPayableListPartial", apList);

        }

        public ActionResult AccountsReceivableList(int bandId, List<test.Models.Budget.AccountReceivables> arList)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return PartialView("_AccountsReceivableListPartial", arList);

        }

        public ActionResult MerchList(int bandId)
        {
            if (!BandUtil.Authenticate(bandId, this))
            {
                return View("Error");
            }

            return View();

        }
    }
}
