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
        private readonly Api.DepartmentsController _departmentsRepository;

        public PresentationsController()
        {
            _presentationsRepository = new Api.PresentationsController();
            _usersRepository = new Api.UsersController();
            _departmentsRepository = new DepartmentsController();
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

            var temp = _departmentsRepository.GetSelectList().ToList();
            temp.Insert(0, new SelectListItem() { Value = "0", Text = "(Select this choice if the correct Department is not shown and enter in New Department below)" });
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
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Presentation model)
        {
            if (!ModelState.IsValid)
            {
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

            _presentationsRepository.Save(model);

            return RedirectToAction("View", new { id = model.Id });
        }
    }
}