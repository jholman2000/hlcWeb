using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hlcWeb.Filters;

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
        // GET: Users
        public PartialViewResult Search(string search)
        {
            var model = _usersRepository.Search(search);
            return PartialView(model);
        }

        public ActionResult View(string id)
        {
            var model = _usersRepository.Get(id);
            return View(model);
        }
    }
}