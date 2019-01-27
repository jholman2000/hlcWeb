using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using hlcWeb.Controllers.Api;
using hlcWeb.Filters;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class AdminController : Controller
    {
        private readonly Api.SpecialtiesController _specialtyRepository;
        private readonly DepartmentsController _departmentsRepository;
        private readonly DiagnosisController _diagnosisController;

        public AdminController()
        {
            _specialtyRepository = new Api.SpecialtiesController();
            _departmentsRepository = new DepartmentsController();
            _diagnosisController = new DiagnosisController();
        }
        public ActionResult Menu(string msg = "")
        {
            return View();
        }

        public ActionResult Edit(string item)
        {
            var itemList = new List<LookupCode>();

            switch (item.ToUpper())
            {
                case "SPEC":
                    ViewBag.Title      = "Specialties";
                    ViewBag.ItemName   = "Specialty";
                    ViewBag.Icon       = "fa-stethoscope";
                    ViewBag.Controller = "specialties";
                    itemList = _specialtyRepository.Search("*").Select(i => new LookupCode
                    {
                        Id = i.Id,
                        Description = i.SpecialtyName,
                        AdditionalText = i.NumberOfDoctors.ToString()
                    }).ToList();
                    break;

                case "DIAG":
                    ViewBag.Title      = "Diagnoses";
                    ViewBag.ItemName   = "Diagnosis";
                    ViewBag.Icon       = "fa-list";
                    ViewBag.Controller = "diagnosis";
                    itemList = _diagnosisController.Search("").Select(i => new LookupCode
                    {
                        Id = i.Id,
                        Description = i.DiagnosisName,
                        AdditionalText = i.NumberInUse.ToString()
                    }).ToList();
                    break;

                case "DEPT":
                    ViewBag.Title      = "Departments";
                    ViewBag.ItemName   = "Department";
                    ViewBag.Icon       = "fa-list";
                    ViewBag.Controller = "departments";
                    itemList = _departmentsRepository.Search("").Select(i => new LookupCode
                    {
                        Id = i.Id,
                        Description = i.DepartmentName,
                        AdditionalText = i.NumberInUse.ToString()
                    }).ToList();
                    break;

            }

            return View(itemList);
        }

    }
}