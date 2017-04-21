using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class CaseFilesController : Controller
    {
        private readonly Api.CaseFilesController _caseFilesRepository;
        public CaseFilesController()
        {
            _caseFilesRepository = new Api.CaseFilesController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _caseFilesRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _caseFilesRepository.Get(id);
            return View(model);

        }
    }
}