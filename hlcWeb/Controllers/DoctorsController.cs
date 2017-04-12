using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class DoctorsController : Controller
    {
        private readonly Api.DoctorsController _doctorRepository;

        public DoctorsController()
        {
            _doctorRepository = new Api.DoctorsController();
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