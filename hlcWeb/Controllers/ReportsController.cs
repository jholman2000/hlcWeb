using System;
using System.Collections.Generic;
using System.Web.Mvc;
using hlcWeb.Controllers.Api;
using hlcWeb.Filters;
using hlcWeb.Infrastructure;
using hlcWeb.Models;
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

        #region Report: Annual Report
        public ActionResult AnnualReport()
        {
            ViewBag.ReportName = "Annual U.S. HLC Questionnaire Information";

            var rptData = _reportRepository.AnnualReport();

            return View(rptData);
        }
        #endregion

        #region Report: Case Files

        public ActionResult CaseFiles()
        {
            var viewModel = new RptSetupViewModel();

            ViewBag.DoctorSelectList = _doctorRepository.GetSelectList();
            ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();
            ViewBag.DiagnosisSelectList = _diagnosisRepository.GetSelectList();
            ViewBag.UserSelectList = _userRepository.GetSelectList();

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
            if (!string.IsNullOrEmpty(viewModel.EnteredBy))
            {
                filters += $"{Constants.EnteredBy}: " + _userRepository.GetSelectList().LookupValue(viewModel.EnteredBy) + "<br />";
            }
            if (viewModel.IsPediatricCase)
            {
                filters += $"{Constants.IsPediatricCase} only<br />";
            }
            filters += rptData.Count.ToString() + " case files found" + "<br />";
            ViewBag.Filters = filters;

            return View(rptData);
        }
        #endregion

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
            if (viewModel.Specialties == null)
            {
                ViewBag.ErrorMessage = "Please select at least one Specialty";
                ViewBag.SpecialtySelectList = _specialtyRepository.GetSelectList();
                return View("SetupDoctorsSpecialty", viewModel);
            }

            // Pass selected Specialties as comma separate list
            string specialtyList = viewModel.Specialties.Count > 0 ? string.Join(",", viewModel.Specialties) : "";
            var rptData = _reportRepository.DoctorsSpecialty((int)viewModel.Attitude, specialtyList);

            ViewBag.ReportName = "Doctors by Specialty";

            var filters = "Attitude: " + viewModel.Attitude.EnumDisplayName() + "<br />";
            if (viewModel.Specialties.Count > 0)
            {
                filters += $"{Constants.SpecialtyId}: ";
                foreach (var s in viewModel.Specialties)
                {
                    filters += _specialtyRepository.GetSelectList().LookupValue(s) + ", ";
                }
                filters = filters.Substring(0, filters.Length - 2) + "<br />";
            }
            filters += rptData.Count + " doctors found" + "<br />";

            ViewBag.Filters = filters;

            return View(rptData);
        }
        #endregion

        #region Report: Hospitals by Type
        public ActionResult HospitalsByType()
        {
            var viewModel = new RptSetupViewModel();

            //TODO: Look into using a GetSelectList() function in the future
            var array = Enum.GetValues(typeof(HospitalType));
            var listItems = new List<SelectListItem> {new SelectListItem {Text = "(Any Hospital Type)", Value = "-1"}};
            foreach (var i in array)
            {
                listItems.Add(new SelectListItem
                {
                    Text = ((HospitalType) i).EnumDisplayName(),
                    Value = ((int) i).ToString()
                });
            }
            ViewBag.HospitalTypesSelectList = new SelectList(listItems, "Value", "Text");

            return View("SetupHospitalsByType", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalsByType(RptSetupViewModel viewModel)
        {
            var hospitalType = viewModel.HospitalType;
            var rptData = _reportRepository.HospitalsByType(hospitalType);

            ViewBag.ReportName = "Hospitals by Type";

            var filters = "";
            if (hospitalType != -1)
            {
                filters += "Hospital Type: " + ((HospitalType) hospitalType).EnumDisplayName() +"<br />";
            }
            filters += rptData.Count + " hospitals found" + "<br />";

            ViewBag.Filters = filters;

            return View(rptData);
        }

        #endregion

        #region Report: PVG Members
        public ActionResult PVGMembers()
        {
            var viewModel = new RptSetupViewModel();
            var groupBy = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "Alphabetic by PVG Member last name" },
                    new SelectListItem { Value = "1", Text = "Grouped by Hospital then by day of week" },
                }, "Value", "Text");
            ViewBag.GroupBySelectList = groupBy;

            var array = Enum.GetValues(typeof(Models.DayOfWeek));
            var listItems = new List<SelectListItem> { new SelectListItem { Text = "(Any day)", Value = "-1" } };
            foreach (var i in array)
            {
                listItems.Add(new SelectListItem
                {
                    Text = ((Models.DayOfWeek)i).EnumDisplayName(),
                    Value = ((int)i).ToString()
                });
            }
            ViewBag.DayOfWeekSelectList = new SelectList(listItems, "Value", "Text");

            ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

            return View("SetupPVGMembers", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PVGMembers(RptSetupViewModel viewModel)
        {
            var rptData = _reportRepository.PVGMembers(viewModel.HospitalId, viewModel.DayOfWeek, viewModel.GroupBy);

            ViewBag.ReportName = "PVG Members";

            var filters = "";
            if (viewModel.HospitalId != 0)
            {
                filters += $"{Constants.HospitalId}: " + _hospitalRepository.GetSelectList().LookupValue(viewModel.HospitalId) + "<br />";
            }
            if (viewModel.DayOfWeek != -1)
            {
                filters += ((Models.DayOfWeek)viewModel.DayOfWeek).EnumDisplayName() + "<br />";
            }
            if (viewModel.GroupBy == 0)
            {
                filters += rptData.Count + " PVG members found" + "<br />";
            }

            ViewBag.Filters = filters;

            var viewName = viewModel.GroupBy == 0 ? "PVGMembersAlpha" : "PVGMembersByHospital";

            return View(viewName, rptData);
        }

        #endregion

        #region Report: Doctors Added/Removed
        public ActionResult DoctorsAddedRemoved()
        {
            var viewModel = new RptSetupViewModel();

            return View("SetupDoctorsAddedRemoved", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorsAddedRemoved(RptSetupViewModel viewModel)
        {
            var rptData = _reportRepository.DoctorsAddedRemoved(viewModel.DateFrom, viewModel.DateTo);

            ViewBag.ReportName = "Doctors Added or Removed";
            ViewBag.Filters = "Dates: " + viewModel.DateFrom.ToShortDateString() + " thru " + viewModel.DateTo.ToShortDateString() + "<br />";

            return View(rptData);
        }
        #endregion

    }
}