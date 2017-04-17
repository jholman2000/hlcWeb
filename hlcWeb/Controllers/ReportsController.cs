using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hlcWeb.Controllers
{
    public class ReportsController : Controller
    {
        private readonly Api.ReportsController _reportRepository;
        public ReportsController()
        {
            _reportRepository = new Api.ReportsController();
        }

        // GET: Reports
        public ActionResult DoctorsAddedRemoved(DateTime dateFrom, DateTime? dateTo)
        {
            if (dateTo == null)
                dateTo = DateTime.Now;

            var model = _reportRepository.Report(dateFrom, (DateTime)dateTo);

            return View(model);
        }
    }
}