/*****************************************************************************************
    Copyright 2013-2014 Wood Buffalo Environmental Asssociation
    
    http://wbea.org
******************************************************************************************/
using System.Web.Mvc;
using System.Web.Routing;

namespace WBEADMS
{
    /* Note: For instructions on enabling IIS6 or IIS7 classic mode, 
     * visit http://go.microsoft.com/?LinkId=9394801
     */

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                                                                        ////"{controller}/{action}/{id}",                           // URL with parameters
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
        }
    }
}