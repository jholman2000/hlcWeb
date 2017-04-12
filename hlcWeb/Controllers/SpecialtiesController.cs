using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class SpecialtiesController : Controller
    {
        private readonly Api.SpecialtiesController _specialtyRepository;

        public SpecialtiesController()
        {
            _specialtyRepository = new Api.SpecialtiesController();
        }

        /// <summary>
        /// Home page search for Specialties.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _specialtyRepository.Search(search);
            return PartialView(model);
        }

        /// <summary>
        /// View Doctors having a specified Specialty Id
        /// </summary>
        /// <param name="id">Specialty Id</param>
        /// <param name="name">Specialty name</param>
        /// <returns></returns>
        public ActionResult ViewDoctors(int id, string name)
        {
            var doctors = _specialtyRepository.GetDoctors(id);
            ViewBag.Name = name;
            return View(doctors);
        }
    }
}