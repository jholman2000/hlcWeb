using System.Collections.Generic;
using System.Web.Mvc;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public virtual PartialViewResult TestSearch(string search)
        {
            var x = new hlcWeb.Controllers.Api.DoctorsController();
            var model = x.Search(search);

            return PartialView("DoctorSearch", model);
        }
    }
}