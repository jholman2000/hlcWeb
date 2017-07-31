using System;
using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class PvgMembersController : Controller
    {
        private readonly Api.PvgMembersController _pvgMemberRepository;
        private readonly Api.HospitalsController _hospitalRepository;

        public PvgMembersController()
        {
            _pvgMemberRepository = new Api.PvgMembersController();
            _hospitalRepository = new Api.HospitalsController();
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

        public ActionResult EditContact(int id)
        {
            PvgMemberViewModel model;

            if (id == 0)
            {
                model = new PvgMemberViewModel();
                model.PvgMember = new PvgMember();
            }
            else
            {
                // Retrieve existing data and populate model
                model = _pvgMemberRepository.Get(id);
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"PVG Member {id} was not found in the database." });
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact(PvgMemberViewModel model)
        {
            //string returnMsg;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PvgMember.Id == 0)
            {
                model.PvgMember.DateEntered = DateTime.Now;
                model.PvgMember.EnteredBy = Session["UserId"].ToString();
                model.PvgMember.DateLastUpdated = model.PvgMember.DateEntered;
                model.PvgMember.LastUpdatedBy = model.PvgMember.EnteredBy;
            }
            else
            {
                model.PvgMember.DateLastUpdated = DateTime.Now;
                model.PvgMember.LastUpdatedBy = Session["UserId"].ToString();
            }

            _pvgMemberRepository.Save(model);

            //if (_caseFileRepository.Save(model)) 
            //    returnMsg = $"Case File for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.PvgMember.Id });
        }

        public ActionResult EditAssignments(int id)
        {

            var viewModel = _pvgMemberRepository.Get(id);

            // Extend the Hospitals to a total of 5 for data entry
            while (viewModel.Hospitals.Count < 5)
            {
                viewModel.Hospitals.Add(new PvgMemberHospital());
            }

            ViewBag.HospitalSelectList = _hospitalRepository.GetSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAssignments(PvgMemberViewModel viewModel)
        {
            _pvgMemberRepository.SaveAssignments(viewModel);
            return RedirectToAction("View", new { id = viewModel.PvgMember.Id });
        }
    }

}