using System;
using System.Linq;
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
        private readonly DiagnosisController _diagnosisController;

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

            ViewBag.DoctorSelectList = _doctorRepository.GetSelectList();
            ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

            // Assisting doctors is same list as Doctors but with a valid entry for none
            var temp1 = _doctorRepository.GetSelectList().ToList();
            temp1.Insert(0, new SelectListItem() { Value = "0", Text = "(No assisting physician or unknown)" });
            ViewBag.AssistingSelectList = new SelectList(temp1, "Value", "Text");

            // Assisting doctors is same list as Doctors but with a valid entry for none
            var temp2 = _doctorRepository.GetSelectListAnesthesiologists().ToList();
            temp2.Insert(0, new SelectListItem() { Value = "0", Text = "(No anesthesiologist or unknown)" });
            ViewBag.AnesthSelectList = new SelectList(temp2, "Value", "Text");

            var temp3 = _diagnosisController.GetSelectList().ToList();
            temp3.Insert(0, new SelectListItem() { Value = "0", Text = "(Select this choice if the correct diagnosis is not shown and enter in Other diagnosis below)" });
            ViewBag.DiagnosisSelectList = new SelectList(temp3, "Value", "Text");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseFile model)
        {
            //string returnMsg;

            if (!ModelState.IsValid)
            {
                ViewBag.DoctorSelectList = _doctorRepository.GetSelectList();
                ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

                // Assisting doctors is same list as Doctors but with a valid entry for none
                var temp1 = _doctorRepository.GetSelectList().ToList();
                temp1.Insert(0, new SelectListItem() { Value = "0", Text = "(No assisting physician or unknown)" });
                ViewBag.AssistingSelectList = new SelectList(temp1, "Value", "Text");

                // Assisting doctors is same list as Doctors but with a valid entry for none
                var temp2 = _doctorRepository.GetSelectListAnesthesiologists().ToList();
                temp2.Insert(0, new SelectListItem() { Value = "0", Text = "(No anesthesiologist or unknown)" });
                ViewBag.AnesthSelectList = new SelectList(temp2, "Value", "Text");

                var temp3 = _diagnosisController.GetSelectList().ToList();
                temp3.Insert(0, new SelectListItem() { Value = "0", Text = "(Select this choice if the correct diagnosis is not shown and enter in Other diagnosis below)" });
                ViewBag.DiagnosisSelectList = new SelectList(temp3, "Value", "Text");
                return View(model);
            }

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
            }

            _caseFileRepository.Save(model);

            //if (_caseFileRepository.Save(model)) 
            //    returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.Id });
        }

    }
}