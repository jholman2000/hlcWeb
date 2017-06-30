using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class PvgMembersController : Controller
    {
        private readonly Api.PvgMembersController _pvgMemberRepository;

        public PvgMembersController()
        {
            _pvgMemberRepository = new Api.PvgMembersController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _pvgMemberRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(int id)
        {
            var model = _pvgMemberRepository.Get(id);
            return View(model);
        }

    }
}