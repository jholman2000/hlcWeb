using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly Api.PresentationsController _presentationsRepository;

        public PresentationsController()
        {
            _presentationsRepository = new Api.PresentationsController();
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

    }
}