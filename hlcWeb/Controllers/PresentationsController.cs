using System;
using System.Linq;
using System.Web.Mvc;
using hlcWeb.Controllers.Api;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly Api.PresentationsController _presentationsRepository;
        private readonly Api.UsersController _usersRepository;
        private readonly DepartmentsController _departmentsRepository;
        private readonly Api.HospitalsController _hospitalRepository;
        private readonly Api.PracticesController _practiceRepository;

        public PresentationsController()
        {
            _presentationsRepository = new Api.PresentationsController();
            _usersRepository = new Api.UsersController();
            _departmentsRepository = new DepartmentsController();
            _hospitalRepository = new Api.HospitalsController();
            _practiceRepository = new Api.PracticesController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _presentationsRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _presentationsRepository.Get(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Presentation model;
            ViewBag.UserSelectList = _usersRepository.GetSelectList();
            ViewBag.PracticeSelectList = _practiceRepository.GetSelectList(FacilityType.Legal);
            ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

            var temp = _departmentsRepository.GetSelectList().ToList();
            temp.Insert(0, new SelectListItem() { Value = "-1", Text = "(Select this choice if the correct Department is not shown and enter in New Department below)" });
            ViewBag.DepartmentsSelectList = new SelectList(temp, "Value", "Text");

            if (id == 0)
            {
                model = new Presentation
                {
                    DatePlanned = DateTime.Today,
                    PresentationFacilityType = PresentationFacilityType.Hospital
                };

            }
            else
            {
                // Retrieve existing data and populate model
                model = _presentationsRepository.Get(id);
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"Presentation {id} was not found in the database." });

                if (model.PresentationFacilityType == PresentationFacilityType.Hospital)
                {
                    model.HospitalId = model.FacilityId;
                }
                else
                {
                    model.PracticeId = model.FacilityId;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Presentation model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserSelectList = _usersRepository.GetSelectList();
                ViewBag.PracticeSelectList = _practiceRepository.GetSelectList(FacilityType.Practice).ToList();
                ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

                var temp = _departmentsRepository.GetSelectList().ToList();
                temp.Insert(0, new SelectListItem() { Value = "-1", Text = "(Select this choice if the correct Department is not shown and enter in New Department below)" });
                ViewBag.DepartmentsSelectList = new SelectList(temp, "Value", "Text");

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

            // See if user added a new Department on the fly
            if (model.DepartmentId == -1 && !string.IsNullOrEmpty(model.NewDepartment))
            {
                var department = new Department()
                {
                    DepartmentName = model.NewDepartment,
                    DateEntered = DateTime.Now,
                    EnteredBy = Session["UserId"].ToString()
                };
                _departmentsRepository.Save(department);
                model.DepartmentId = department.Id;
            }

            model.FacilityId = model.PresentationFacilityType == PresentationFacilityType.Hospital
                ? model.HospitalId
                : model.PracticeId;

            _presentationsRepository.Save(model);

            return RedirectToAction("View", new { id = model.Id });
        }
    }
}