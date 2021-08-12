/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Web.Mvc;
using System.Web.Routing;

namespace WBEADMS.DocIt
{
    /* Note: For instructions on enabling IIS6 or IIS7 classic mode, 
     * visit http://go.microsoft.com/?LinkId=9394801
     */

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allActiveReport}", new { allActiveReport = @".*\.ar13(/.*)?" });
            routes.MapRoute(
                  "Default",                                              // Route name
                                                                          // "{controller}/{action}/{id}",                           // URL with parameters
                                                                          "{controller}.aspx/{action}/{id}", // bandaid fix for IIS 5.1
                                                                          new { controller = "Home", action = "Index", id = "" });  // Parameter defaults


            routes.MapRoute(
                "Root",
                "",
                new { controller = "Home", action = "Index", id = "" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}