﻿using System.Web.Mvc;
using hlcWeb.Filters;
using hlcWeb.Models;

namespace hlcWeb.Controllers
{
    [HLCUserLoggedOn]
    public class UsersController : Controller
    {
        private readonly Api.UsersController _usersRepository;

        public UsersController()
        {
            _usersRepository = new Api.UsersController();
        }

        public PartialViewResult Search(string search)
        {
            var model = _usersRepository.Search(search);
            return PartialView(model);
        }

        public new ActionResult View(string id)
        {
            var model = _usersRepository.Get(id);
            return View(model);
        }

        public ActionResult Edit(string id)
        {

            var model = new User();

            if (string.IsNullOrEmpty(id))
            {
                model.OriginalUserId = "";
                model.IsActive = true;
                model.UserRole = "HLC Member";
                model.MustChangePassword = true;
            }
            else
            {
                ViewBag.Action = "Edit";
                // Retrieve existing data and populate model
                model = _usersRepository.Get(id);
                model.OriginalUserId = model.UserId;
                if (model == null)
                    return RedirectToAction("Search", "Home",
                        new { msg = $"UserId {id} was not found in the database." });

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //if (model.Status != model.OriginalStatus || model.StatusDate == DateTime.MinValue)
            //    model.StatusDate = DateTime.Now;

            if (string.IsNullOrEmpty(model.OriginalUserId))
            {
                // Check to make sure the UserId entered for this new user is not already used
                var checkUser = _usersRepository.CheckUserId(model);
                if (checkUser.UserId != null)
                {
                    model.UserId = "";
                    ViewBag.ErrorMessage = $"User Id {checkUser.UserId} has already been assigned to {checkUser.FullName}.  Please choose another User Id.";
                    return View(model);
                }
            }
            else
            {
                //model.DateLastUpdated = DateTime.Now;
                //model.LastUpdatedBy = Session["UserId"].ToString();
            }

            _usersRepository.Save(model);

            //if (_doctorRepository.Save(model))
            //    returnMsg = $"Contact information for {model.FirstName + " " + model.LastName} was edited successfully.";

            return RedirectToAction("View", new { id = model.UserId });
        }

    }
}