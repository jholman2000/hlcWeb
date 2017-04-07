using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    public class DoctorsController : Controller
    {
        private Api.DoctorsController _doctorRepository;

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
        }

        /// <summary>
        /// Home page search for Doctor.  Accesses by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _doctorRepository.Search(search);
            return PartialView("DoctorSearch", model);
        }

        /// <summary>
        /// View a Doctor information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var doctor = _doctorRepository.GetDoctor(id);
            return View(doctor);
        }
    }
}