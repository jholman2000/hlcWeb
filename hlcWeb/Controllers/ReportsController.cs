using System;
using System.Linq;
using System.Web.Mvc;
using hlcWeb.Controllers.Api;
using hlcWeb.Filters;
using hlcWeb.Infrastructure;
using hlcWeb.ViewModels.Reports;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class ReportsController : Controller
    {
        private readonly Api.ReportsController _reportRepository;
        private readonly Api.CaseFilesController _caseFileRepository;
        private readonly Api.DoctorsController _doctorRepository;
        private readonly Api.HospitalsController _hospitalRepository;
        private readonly Api.DiagnosisController _diagnosisController;

        public ReportsController()
        {
            _reportRepository = new Api.ReportsController();
            _caseFileRepository = new Api.CaseFilesController();
            _doctorRepository = new Api.DoctorsController();
            _hospitalRepository = new Api.HospitalsController();
            _diagnosisController = new DiagnosisController();
        }

        public ActionResult List()
        {
            // Show main list of reports for user to select from
            return View();
        }

        #region Report: Case Files

        public ActionResult CaseFiles()
        {
            var viewModel = new RptSetupViewModel();

            // TODO: Refactor to pull this from the Api and cache
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
                //items.Insert(0, new
                //{
                //    Text = "(Select this choice if the correct diagnosis is not shown and enter in Other below)",
                //    Value = 0
                //});

                Session["DiagnosisSelectList"] = new SelectList(items, "Value", "Text");
            }
            ViewBag.DiagnosisSelectList = Session["DiagnosisSelectList"];

            return View("SetupCaseFiles", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaseFiles(RptSetupViewModel viewModel)
        {
            var rptData = _reportRepository.CaseFiles(viewModel.DateFrom, viewModel.DateTo, viewModel.DoctorId,
                viewModel.HospitalId, viewModel.DiagnosisId, viewModel.EnteredBy, viewModel.IsPediatricCase);

            var filters = "Case Dates: " + viewModel.DateFrom.ToShortDateString() + " thru " + viewModel.DateTo.ToShortDateString() + "<br />";

            if (viewModel.DoctorId != 0)
            {
                filters += "Doctor: " + ((SelectList) Session["DoctorSelectList"]).LookupValue(viewModel.DoctorId) + "<br />";
            }
            if (viewModel.HospitalId != 0)
            {
                filters += "Hospital: " + ((SelectList)Session["HospitalSelectList"]).LookupValue(viewModel.HospitalId) + "<br />";
            }
            if (viewModel.DiagnosisId != 0)
            {
                filters += "Diagnosis: " + ((SelectList)Session["DiagnosisSelectList"]).LookupValue(viewModel.DiagnosisId) + "<br />";
            }

            ViewBag.Filters = filters;

            return View(rptData);
        }
        #endregion

        #region Report: Doctors Added/Removed
        public ActionResult DoctorsAddedRemoved(DateTime dateFrom, DateTime? dateTo)
        {
            if (dateTo == null)
                dateTo = DateTime.Now;

            var model = _reportRepository.Report(dateFrom, (DateTime)dateTo);

            return View(model);
        }
        #endregion

    }
}