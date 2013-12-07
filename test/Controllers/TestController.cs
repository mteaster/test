using System.IO;
using System.Web.Mvc;
using test.Models;
using test.Models.Band;
using test.Models.Test;
using test.Stuff;

namespace test.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchBandModel model)
        {
            SearchViewModel svm = new SearchViewModel();
            svm.searchModel = model;

            if (ModelState.IsValid)
            {
                svm.resultsModel = BandUtil.SearchByName(model.BandName);
            }

            return View(svm);
        }

        public ActionResult Band(int bandId)
        {
            ViewBag.BandId = bandId;
            return View();
        }
    }
}
