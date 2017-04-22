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

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
            _practiceRepository = new Api.PracticesController();
        }

        /// <summary>
        /// Home page search for Doctors.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _doctorRepository.Search(search);
            return PartialView(model);
        }

        /// <summary>
        /// View detailed information for a doctor
        /// </summary>
        /// <param name="id">Doctor Id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var model = _doctorRepository.Get(id);
            return View(model);
        }

        public ActionResult EditContact(int id)
        {
            //TODO: Add a DateEntered field.  Maybe switch back to using the Doctor class as the model

            var viewModel= new DoctorContactViewModel();

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
                //viewModel.StatusDate = DateTime.Now;
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

            var returnMsg = "There was an error updating this Doctor's information.";

            if (viewModel.Status != viewModel.OriginalStatus || viewModel.StatusDate == DateTime.MinValue)
                viewModel.StatusDate = DateTime.Now;

            viewModel.DateLastUpdated = DateTime.Now;
            viewModel.LastUpdatedBy = (Session["User"] as User)?.UserID;

            if (_doctorRepository.Save(viewModel))
                returnMsg = $"Contact information for {viewModel.FirstName + " " + viewModel.LastName} was edited successfully.";

            return RedirectToAction("View", new {id= viewModel.Id});
        }

    }
}