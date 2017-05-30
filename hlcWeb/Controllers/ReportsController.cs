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

        public ActionResult List()
        {
            // Show main list of reports for user to select from
            return View();
        }

        #region: Report: Case Files

        public ActionResult CaseFiles()
        {
            return View("Reports/SetupCaseFiles");
        }
        #endregion

        #region Report: Doctors Added/Removed
        public ActionResult DoctorsAddedRemoved(DateTime dateFrom, DateTime? dateTo)
        {
            if (dateTo == null)
                dateTo = DateTime.Now;

            var model = _reportRepository.Report(dateFrom, (DateTime)dateTo);

            return View(model);
        }
        #endregion

    }
}