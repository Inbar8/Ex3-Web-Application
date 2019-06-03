using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
/// <summary>
/// ////
/// </summary>
namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BasicDisplay", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Main", action = "BasicDisplay", time = UrlParameter.Optional });

            //routes.MapRoute("UpdatedDisplay", "display/{ip}/{port}/{time}",
            //defaults: new { controller = "FlightViews", action = "UpdatedDisplay" });

            //don't change:
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
