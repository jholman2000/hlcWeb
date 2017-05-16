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
        private readonly Api.SpecialtiesController _specialtyRepository;

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
            _practiceRepository = new Api.PracticesController();
            _specialtyRepository = new Api.SpecialtiesController();
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

            var viewModel = new DoctorContactViewModel();

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
                viewModel.Attitude = Attitude.Unknown;
                viewModel.Status = Status.NewlyIdentified;
                viewModel.OriginalStatus = Status.NewlyIdentified;
            }
            else
            {
                // Retrieve existing data and populate model
                var doctor = _doctorRepository.Get(id);
                if (doctor == null)
                    return RedirectToAction("Search", "Home",
                           new {msg = $"DoctorId {id} was not found in the database."});

                viewModel = Mapper.Map<DoctorContactViewModel>(doctor);
                viewModel.OriginalStatus = doctor.Status;
            }
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact(DoctorContactViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PracticeSelectList = Session["PracticeSelectList"];
                return View(viewModel);
            }

            //var returnMsg = "There was an error updating this Doctor's information.";

            if (viewModel.Status != viewModel.OriginalStatus || viewModel.StatusDate == DateTime.MinValue)
                viewModel.StatusDate = DateTime.Now;

            if (viewModel.Id == 0)
            {
                viewModel.DateEntered = DateTime.Now;
                viewModel.EnteredBy = Session["UserId"].ToString();
                viewModel.DateLastUpdated = viewModel.DateEntered;
                viewModel.LastUpdatedBy = viewModel.EnteredBy;
            }
            else
            {
                viewModel.DateLastUpdated = DateTime.Now;
                viewModel.LastUpdatedBy = Session["UserId"].ToString();
            }

            _doctorRepository.SaveContact(viewModel);

            //if (_doctorRepository.Save(model))
            //    returnMsg = $"Contact information for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new {id= viewModel.Id});
        }

        public ActionResult EditAttitudes(int id)
        {
            // Retrieve existing data and populate model
            var doctor = _doctorRepository.Get(id);
            if (doctor == null)
                return RedirectToAction("Search", "Home",
                    new { msg = $"DoctorId {id} was not found in the database." });

            var viewModel = Mapper.Map<DoctorAttitudesViewModel>(doctor);
            //viewModel.OriginalStatus = doctor.Status;

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttitudes(DoctorAttitudesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.DateLastUpdated = DateTime.Now;
            viewModel.LastUpdatedBy = Session["UserId"].ToString();

            _doctorRepository.SaveAttitudes(viewModel);

            return RedirectToAction("View", new { id = viewModel.Id });
        }

        public ActionResult EditSpecialties(int id)
        {
            var doctor = _doctorRepository.Get(id);
            var viewModel = new DoctorSpecialtiesViewModel()
            {
                DoctorId = doctor.Id,
                FullName = doctor.FullName,
                Specialties = doctor.Specialties
            };

            // Extend the SPecialties to a total of 6 for data entry
            while (viewModel.Specialties.Count < 6)
            {
                viewModel.Specialties.Add(new DoctorSpecialty());
            }

            if (Session["SpecialtyItems"] == null)
            {
                var items = _specialtyRepository.Search("")
                    .Select(s => new
                    {
                        Text = s.SpecialtyName,
                        Value = s.ID
                    })
                    .ToList();
                Session["SpecialtyItems"] = items;
            }
            ViewBag.SpecialtyItems = Session["SpecialtyItems"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSpecialties(DoctorSpecialtiesViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.SpecialtyItems = Session["SpecialtyItems"];
            //    return View(viewModel);
            //}

            // Pass to repository to update
            _specialtyRepository.SaveDoctorSpecialties(viewModel);

            return RedirectToAction("View", new { id = viewModel.DoctorId });
        }
    }
}