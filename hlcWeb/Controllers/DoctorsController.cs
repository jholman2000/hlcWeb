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

        public ActionResult Edit(int id)
        {
            var model= new DoctorViewModel(); 

            if (Session["PracticeList"] == null)
            {
                var practiceList = _practiceRepository.Search("")
                    .Select(s => new
                    {
                        Text = s.PracticeName + " - " + s.Address1,
                        Value = s.Id
                    })
                    .ToList();
                Session["PracticeList"] = new SelectList(practiceList, "Value", "Text");
            }
            ViewBag.PracticeList = Session["PracticeList"];

            if (id != 0)
            {
                

                var doctor = _doctorRepository.Get(id);
                if (doctor == null) throw new ArgumentNullException(nameof(doctor));
                model = Mapper.Map<DoctorViewModel>(doctor);
            }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
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
    }
}