using hlcWeb.Filters;
using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
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
      
    }
}