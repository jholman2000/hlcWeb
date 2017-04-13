using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class HospitalsController : Controller
    {
        private readonly Api.HospitalsController _hospitalRepository;

        public HospitalsController()
        {
            _hospitalRepository = new Api.HospitalsController();
        }

        /// <summary>
        /// Home page search for Hospitals.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _hospitalRepository.Search(search);
            return PartialView(model);
        }

        /// <summary>
        /// View Doctors at a specified hospital
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var model = _hospitalRepository.GetDoctors(id);
            return View(model);
        }

    }
}