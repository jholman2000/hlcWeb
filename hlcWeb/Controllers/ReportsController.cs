using System;
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
        private readonly Api.DoctorsController _doctorRepository;
        private readonly Api.HospitalsController _hospitalRepository;
        private readonly DiagnosisController _diagnosisRepository;
        private readonly Api.UsersController _userRepository;
        private readonly Api.SpecialtiesController _specialtyRepository;

        public ReportsController()
        {
            _reportRepository = new Api.ReportsController();
            _doctorRepository = new Api.DoctorsController();
            _hospitalRepository = new Api.HospitalsController();
            _diagnosisRepository = new DiagnosisController();
            _userRepository = new Api.UsersController();
            _specialtyRepository = new Api.SpecialtiesController();
        }

        public ActionResult List()
        {
            // Show main list of reports for user to select from
            return View();
        }

        #region Report: Doctors by Specialty

        public ActionResult DoctorsSpecialty()
        {
            var viewModel = new RptSetupViewModel();

            ViewBag.SpecialtySelectList = _specialtyRepository.GetSelectList();
            return View("SetupDoctorsSpecialty", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorsSpecialty(RptSetupViewModel viewModel)
        {
            string specialtyList = viewModel.Specialties.Count > 0 ? string.Join(",", viewModel.Specialties) : "";
            var rptData = _reportRepository.DoctorsSpecialty(viewModel.Attitude, specialtyList);

            ViewBag.ReportName = "Doctors by Specialty";

            var filters = "Attitude: " + viewModel.Attitude.EnumDisplayName() + "<br />";
            if (viewModel.Specialties.Count > 0)
            {
                filters += $"{Constants.SpecialtyId}: ";
                for (int i = 0; i < viewModel.Specialties.Count; i++)
                {
                    filters += _specialtyRepository.GetSelectList().LookupValue(viewModel.Specialties[i]) + ", ";
                }
                filters = filters.Substring(0, filters.Length - 2) + "<br />";
            }
            filters += rptData.Count.ToString() + " doctors found" + "<br />";

            ViewBag.Filters = filters;

            return View(rptData);
        }
        #endregion


        #region Report: Case Files

        public ActionResult CaseFiles()
        {
            var viewModel = new RptSetupViewModel();

            ViewBag.DoctorSelectList    = _doctorRepository.GetSelectList();
            ViewBag.HospitalSelectList  = _hospitalRepository.GetSelectList();
            ViewBag.DiagnosisSelectList = _diagnosisRepository.GetSelectList();
            ViewBag.UserSelectList      = _userRepository.GetSelectList();

            return View("SetupCaseFiles", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaseFiles(RptSetupViewModel viewModel)
        {
            var rptData = _reportRepository.CaseFiles(viewModel.DateFrom, viewModel.DateTo, viewModel.DoctorId,
                viewModel.HospitalId, viewModel.DiagnosisId, viewModel.EnteredBy, viewModel.IsPediatricCase);

            var filters = "Case Dates: " + viewModel.DateFrom.ToShortDateString() + " thru " + viewModel.DateTo.ToShortDateString() + "<br />";

            ViewBag.ReportName = "Case Files";

            if (viewModel.DoctorId != 0)
            {
                filters += $"{Constants.DoctorId}: " + _doctorRepository.GetSelectList().LookupValue(viewModel.DoctorId) + "<br />";
            }
            if (viewModel.HospitalId != 0)
            {
                filters += $"{Constants.HospitalId}: " + _hospitalRepository.GetSelectList().LookupValue(viewModel.HospitalId) + "<br />";
            }
            if (viewModel.DiagnosisId != 0)
            {
                filters += $"{Constants.DiagnosisId}: " + _diagnosisRepository.GetSelectList().LookupValue(viewModel.DiagnosisId) + "<br />";
            }
            if (!string.IsNullOrEmpty(viewModel.EnteredBy) )
            {
                filters += $"{Constants.EnteredBy}: " + _userRepository.GetSelectList().LookupValue(viewModel.EnteredBy) + "<br />";
            }
            if (viewModel.IsPediatricCase)
            {
                filters += $"{Constants.IsPediatricCase} only<br />" ;
            }
            filters += rptData.Count.ToString() + " case files found" + "<br />";
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