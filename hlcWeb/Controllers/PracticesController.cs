using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class PracticesController : Controller
    {
        private readonly Api.PracticesController _practiceRepository;

        public PracticesController()
        {
            _practiceRepository = new Api.PracticesController();
        }

        /// <summary>
        /// Home page search for Practices.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _practiceRepository.Search(search);
            return PartialView(model);
        }
    }
}