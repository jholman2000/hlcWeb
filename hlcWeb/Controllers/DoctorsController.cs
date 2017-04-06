using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hlcWeb.Controllers.Api;

namespace hlcWeb.Controllers
{
    public class DoctorsController : Controller
    {
        private Api.DoctorsController _doctorRepository;

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
        }

        // GET: Doctors
        public PartialViewResult Search(string search)
        {
            var model = _doctorRepository.Search(search);
            return PartialView("DoctorSearch", model);
        }

        public ActionResult View(int id)
        {
            return null;
        }
    }
}