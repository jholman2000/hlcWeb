using System;
using System.Linq;
using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.Models;

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

        public PartialViewResult Search(string search)
        {
            var model = _doctorRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _doctorRepository.Get(id);
            return View(model);
        }

        public ActionResult EditInfo(int id)
        {
            //TODO: Add a DateEntered field.  Maybe switch back to using the Doctor class as the model

            var model= new Doctor();

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
                model.Attitude = Attitude.Unknown;
                model.Status = Status.NewlyIdentified;
                model.OriginalStatus = Status.NewlyIdentified;
                //model.StatusDate = DateTime.Now;
            }
            else
            {
                // Retrieve existing data and populate model
                var doctor = _doctorRepository.Get(id);
                if (doctor == null)
                    return RedirectToAction("Search", "Home",
                           new {msg = $"DoctorId {id} was not found in the database."});

                //model = Mapper.Map<Doctor>(doctor);
                model.OriginalStatus = doctor.Status;
            }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(Doctor model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PracticeSelectList = Session["PracticeSelectList"];
                return View(model);
            }

            //var returnMsg = "There was an error updating this Doctor's information.";

            if (model.Status != model.OriginalStatus || model.StatusDate == DateTime.MinValue)
                model.StatusDate = DateTime.Now;

            model.DateLastUpdated = DateTime.Now;
            model.LastUpdatedBy = (Session["User"] as User)?.UserID;

            _doctorRepository.Save(model);

            //if (_doctorRepository.Save(model))
            //    returnMsg = $"Contact information for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new {id= model.Id});
        }

    }
}