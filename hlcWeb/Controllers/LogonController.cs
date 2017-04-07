using System.Web.Mvc;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers
{
    public class LogonController : Controller
    {
        private Api.UsersController _userRepository;
        public LogonController()
        {
            _userRepository = new Api.UsersController();
        }

        public ActionResult Logon(string returnUrl, string infoMsg)
        {
            #if DEBUG
            //return RedirectToAction("Index", "Home");
            #endif
            ViewBag.ReturnUrl = returnUrl;
            //System.Web.HttpContext.Current.Session["hlcUser"] = "jholman";
            var viewModel = new LogonViewModel
            {
                Email = "",
                Password = "",
                InfoMessage = infoMsg
            };

            return View(viewModel);
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
                return RedirectToAction("Index", "Home");
            }

            
        }

        public ActionResult Logoff()
        {
            Session.Abandon();

            return RedirectToAction("Logon",new { infoMsg = "You are now signed out."});
        }
    }
}