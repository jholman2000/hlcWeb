using System.Web.Mvc;
using hlcWeb.Filters;

namespace hlcWeb.Controllers
{
    public class NotesController : Controller
    {
        private readonly Api.NotesController _noteRepository;

        public NotesController()
        {
            _noteRepository = new Api.NotesController();
        }
        /// <summary>
        /// Home page search for Notes.  Accessed by the Search box or clicking a Rolodex button
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public PartialViewResult Search(string search)
        {
            var model = _noteRepository.Search(search);
            return PartialView(model);
        }
    }
}