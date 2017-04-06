using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    public class LogonController : Controller
    {
        // GET: Logon
        public ActionResult Logon()
        {
            System.Web.HttpContext.Current.Session["hlcUser"] = "jholman";
            //return View();
            return RedirectToAction("Index", "Home");
        }
    }
}