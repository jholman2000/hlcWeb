using System.Web.Mvc;

namespace hlcWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // I disabled this to allow YSOD oon server.  Uncomment to direct to generic error page
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
