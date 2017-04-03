using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    public class LogonController : Controller
    {
        // GET: Logon
        public ActionResult Logon()
        {
           // var x = Api.DoctorsController();

            System.Web.HttpContext.Current.Session["hlcUser"] = "jholman";
            //return View();
            return RedirectToAction("Index", "Home");
        }
    }
}