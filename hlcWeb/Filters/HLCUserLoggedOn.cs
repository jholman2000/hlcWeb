using System.Web.Mvc;
using System.Web.Routing;

namespace hlcWeb.Filters
{
    /// <summary>
    /// Make sure an HLC User is logged on
    /// </summary>
    public class HLCUserLoggedOn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session["User"];
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Logon" },
                    { "action", "Logon" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}