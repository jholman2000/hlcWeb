using System;
using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.ViewModels;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class HospitalsController : Controller
    {
        private readonly Api.HospitalsController _hospitalRepository;

        public HospitalsController()
        {
            _hospitalRepository = new Api.HospitalsController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _hospitalRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _hospitalRepository.Get(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            // Use HospitalViewModel and reference .Hospital portion because the .Get() returns this model
            var model = new HospitalViewModel();

            if (id == 0)
            {
                model.Hospital = new Hospital();
            }
            else
            {
                model = _hospitalRepository.Get(id);
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"Hospital {id} was not found in the database." });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HospitalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Hospital.Id == 0)
            {
                model.Hospital.DateEntered = DateTime.Now;
                model.Hospital.EnteredBy = Session["UserId"].ToString();
                model.Hospital.DateLastUpdated = model.Hospital.DateEntered;
                model.Hospital.LastUpdatedBy = model.Hospital.EnteredBy;
            }
            else
            {
                model.Hospital.DateLastUpdated = DateTime.Now;
                model.Hospital.LastUpdatedBy = Session["UserId"].ToString();
            }

            _hospitalRepository.Save(model);

            //if (_hospitalRepository.Save(model)) 
            //    returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.Hospital.Id });
        }
    }
}