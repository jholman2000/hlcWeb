using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using hlcWeb.Filters;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class CaseFilesController : Controller
    {
        private readonly Api.CaseFilesController _caseFileRepository;
        private readonly Api.DoctorsController _doctorRepository;
        private readonly Api.HospitalsController _hospitalRepository;
        public CaseFilesController()
        {
            _caseFileRepository = new Api.CaseFilesController();
            _doctorRepository = new Api.DoctorsController();
            _hospitalRepository = new Api.HospitalsController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _caseFileRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _caseFileRepository.Get(id);
            return View(model);

        }

        public ActionResult Edit(int id)
        {
            CaseFile model;

            //TODO: Can this be moved to inside the API controller?
            if (Session["DoctorSelectList"] == null)
            {
                var items = _doctorRepository.Search("", false)
                    .Select(s => new
                    {
                        Text = s.LastName + ", " + s.LastName,
                        Value = s.Id
                    })
                    .ToList();
                Session["DoctorSelectList"] = new SelectList(items, "Value", "Text");
            }
            ViewBag.DoctorSelectList = Session["DoctorSelectList"];

            if (Session["HospitalSelectList"] == null)
            {
                var items = _hospitalRepository.Search("")
                    .Select(s => new
                    {
                        Text = s.HospitalName + " - " + s.City + " " + s.State,
                        Value = s.Id
                    })
                    .ToList();
                Session["HospitalSelectList"] = new SelectList(items, "Value", "Text");
            }
            ViewBag.HospitalSelectList = Session["HospitalSelectList"];

            if (id == 0)
            {
                model = new CaseFile
                {
                    CaseDate = DateTime.Now,
                    DateEntered = DateTime.Now,
                    EnteredBy = ((User) Session["User"]).UserID
                };
            }
            else
            {
                // Retrieve existing data and populate model
                model = _caseFileRepository.Get(id);
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"Case File {id} was not found in the database." });
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseFile model)
        {
            string returnMsg;

            if (!ModelState.IsValid)
            {
                ViewBag.DoctorSelectList = Session["DoctorSelectList"];
                return View(model);
            }

            model.DateLastUpdated = DateTime.Now;
            model.UpdatedBy = ((User) Session["User"]).UserID;

            if (_caseFileRepository.Save(model))
                returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.Id });
        }
    }
}