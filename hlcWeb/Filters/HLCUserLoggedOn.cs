using System.Web.Mvc;
using System.Web.Routing;

namespace hlcWeb.Filters
{
    /// <summary>
    /// Make sure an HLC User is logged on
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class HLCUserLoggedOn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session["User"] == null)
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