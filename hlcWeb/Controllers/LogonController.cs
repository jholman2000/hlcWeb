using System.Web.Mvc;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers
{
    public class LogonController : Controller
    {
        private readonly Api.UsersController _userRepository;
        public LogonController()
        {
            _userRepository = new Api.UsersController();
        }

        public ActionResult Logon(string returnUrl, string infoMsg)
        {
            #if  DEBUG
                var user = _userRepository.Logon("jeff.holman@yahoo.com", "jholman");
                Session["User"] = user;
                Session["UserId"] = user.UserId;
                Session["UserRole"] = user.UserRole;
            return RedirectToAction("Search", "Home");
            #else
                ViewBag.ReturnUrl = returnUrl;
                var viewModel = new LogonViewModel
                {
                    Email = "",
                    Password = "",
                    InfoMessage = infoMsg
                };

                return View(viewModel);
            #endif
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LogonViewModel viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _userRepository.Logon(viewModel.Email, viewModel.Password);

            if (user == null)
            {
                viewModel.ErrorMessage = "Invalid email address or password was entered.";
                return View(viewModel);
            }
            else
            {
                Session["User"] = user;
                Session["UserId"] = user.UserId;
                Session["UserRole"] = user.UserRole;
                return RedirectToAction("Search", "Home");
            }

            
        }

        public ActionResult Logoff()
        {
            Session.Abandon();

            return RedirectToAction("Logon",new { infoMsg = "You are now signed out."});
        }
    }
}