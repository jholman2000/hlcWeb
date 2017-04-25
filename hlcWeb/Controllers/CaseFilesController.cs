using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using hlcWeb.Controllers.Api;
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
        private readonly Api.DiagnosisController _diagnosisController;

        public CaseFilesController()
        {
            _caseFileRepository = new Api.CaseFilesController();
            _doctorRepository = new Api.DoctorsController();
            _hospitalRepository = new Api.HospitalsController();
            _diagnosisController = new DiagnosisController();
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
                        Text = s.LastName + ", " + s.FirstName,
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

            if (Session["DiagnosisSelectList"] == null)
            {
                var items = _diagnosisController.Search("")
                    .Select(s => new
                    {
                        Text = s.DiagnosisName,
                        Value = s.Id
                    })
                    .ToList();

                // Add an item to top of list for user to indicate they will enter a new value
                items.Insert(0, new
                {
                    Text = "(Select this choice if the correct diagnosis is not in the list and enter in Other below)",
                    Value =0
                });

                Session["DiagnosisSelectList"] = new SelectList(items, "Value", "Text");
            }
            ViewBag.DiagnosisSelectList = Session["DiagnosisSelectList"];

            if (id == 0)
            {
                model = new CaseFile();
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

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseFile model)
        {
            string returnMsg;

            if (!ModelState.IsValid)
            {
                ViewBag.DoctorSelectList = Session["DoctorSelectList"];
                ViewBag.HospitalSelectList = Session["HospitalSelectList"];
                ViewBag.DiagnosisSelectList = Session["DiagnosisSelectList"];
                return View(model);
            }

            if (model.Id == 0)
            {
                model.DateEntered = DateTime.Now;
                model.EnteredBy = Session["UserId"].ToString();
            }
            else
            {
                model.DateLastUpdated = DateTime.Now;
                model.UpdatedBy = Session["UserId"].ToString();
            }

            // See if user added a new Diagnosis on the fly
            if (model.DiagnosisId == 0 &&  !string.IsNullOrEmpty(model.DiagnosisOther))
            {
                var diagnosis = new Diagnosis()
                {
                    DiagnosisName = model.DiagnosisOther,
                    DateEntered = DateTime.Now,
                    EnteredBy = Session["UserId"].ToString()
                };
                _diagnosisController.Save(diagnosis);
                model.DiagnosisId = diagnosis.Id;
                Session["DiagnosisSelectList"] = null;  // force refresh on next use
            }

            if (_caseFileRepository.Save(model))
                returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.Id });
        }

        public string SaveText(string fieldName, string textValue)
        {
            _caseFileRepository.SaveText(fieldName, textValue);
            return "OK";
        }
    }
}