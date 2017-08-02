using System;
using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class NotesController : Controller
    {
        private readonly Api.NotesController _noteRepository;

        public NotesController()
        {
            _noteRepository = new Api.NotesController();
        }

        public ActionResult Edit(int id = 0, int doctorId = 0, string url = "")
        {
            if (id == 0 && doctorId == 0)
                throw new ArgumentNullException(nameof(doctorId));

            ViewBag.Url = url;

            var model = id == 0 ? new DoctorNote() : _noteRepository.Get(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorNote viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var returnMsg = "There was an error updating this comment.";

            viewModel.DateEntered = DateTime.Now;
            viewModel.UserId = (Session["User"] as User)?.UserId;

            if (_noteRepository.Save(viewModel))
                returnMsg = $"Comment was edited successfully.";

            //return RedirectToAction("Search", "Home", new {msg = returnMsg });
            return RedirectToAction("View", new {id = viewModel.Id});
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

        /// <summary>
        /// View a Note having a specified Note Id
        /// </summary>
        /// <param name="id">Note Id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var model = _noteRepository.Get(id);
            ViewBag.Url = HttpContext?.Request?.Url?.AbsoluteUri;
            return View(model);
        }
    }
}