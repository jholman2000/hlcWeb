using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using hlcWeb.Filters;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class DoctorsController : Controller
    {
        private readonly Api.DoctorsController _doctorRepository;
        private readonly Api.PracticesController _practiceRepository;

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
            _practiceRepository = new Api.PracticesController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _doctorRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _doctorRepository.Get(id);
            return View(model);
        }

        public ActionResult EditContact(int id)
        {
            //TODO: Add a DateEntered field.  Maybe switch back to using the Doctor class as the model

            //var model= new Doctor();
            var model = new DoctorContactViewModel();
            if (Session["PracticeSelectList"] == null)
            {
                var items = _practiceRepository.Search("")
                    .Select(s => new
                    {
                        Text = s.PracticeName + " - " + s.Address1,
                        Value = s.Id
                    })
                    .ToList();
                Session["PracticeSelectList"] = new SelectList(items, "Value", "Text");
            }
            ViewBag.PracticeSelectList = Session["PracticeSelectList"];

            if (id == 0)
            {
                model.Attitude = Attitude.Unknown;
                model.Status = Status.NewlyIdentified;
                model.OriginalStatus = Status.NewlyIdentified;
            }
            else
            {
                // Retrieve existing data and populate model
                var doctor = _doctorRepository.Get(id);
                if (doctor == null)
                    return RedirectToAction("Search", "Home",
                           new {msg = $"DoctorId {id} was not found in the database."});

                model = Mapper.Map<DoctorContactViewModel>(doctor);
                model.OriginalStatus = doctor.Status;
            }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact(DoctorContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PracticeSelectList = Session["PracticeSelectList"];
                return View(model);
            }

            //var returnMsg = "There was an error updating this Doctor's information.";

            if (model.Status != model.OriginalStatus || model.StatusDate == DateTime.MinValue)
                model.StatusDate = DateTime.Now;

            if (model.Id == 0)
            {
                model.DateEntered = DateTime.Now;
                model.EnteredBy = Session["UserId"].ToString();
                model.DateLastUpdated = model.DateEntered;
                model.LastUpdatedBy = model.EnteredBy;
            }
            else
            {
                model.DateLastUpdated = DateTime.Now;
                model.LastUpdatedBy = Session["UserId"].ToString();
            }

            _doctorRepository.SaveContact(model);

            //if (_doctorRepository.Save(model))
            //    returnMsg = $"Contact information for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new {id= model.Id});
        }

    }
}