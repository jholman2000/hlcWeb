using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class PracticesController : Controller
    {
        private readonly Api.PracticesController _practiceRepository;

        public PracticesController()
        {
            _practiceRepository = new Api.PracticesController();
        }

        /// <summary>
        /// Home page search for Practices.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _practiceRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _practiceRepository.Get(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            // Use PracticeViewModel and reference .Practice portion because the .Get() returns this model
            var model = new PracticeViewModel();

            if (id == 0)
            {
                model.Practice = new Practice();
            }
            else
            {
                model = _practiceRepository.Get(id);
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"Hospital {id} was not found in the database." });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PracticeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Practice.Id == 0)
            {
                //model.Hospital.DateEntered = DateTime.Now;
                //model.Hospital.EnteredBy = Session["UserId"].ToString();
                //model.Hospital.DateLastUpdated = model.Hospital.DateEntered;
                //model.Hospital.LastUpdatedBy = model.Hospital.EnteredBy;
            }
            else
            {
                //model.Hospital.DateLastUpdated = DateTime.Now;
                //model.Hospital.LastUpdatedBy = Session["UserId"].ToString();
            }

            _practiceRepository.Save(model);

            //if (_hospitalRepository.Save(model)) 
            //    returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.Practice.Id });
        }

    }
}