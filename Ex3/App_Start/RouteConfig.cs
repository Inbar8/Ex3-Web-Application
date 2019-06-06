using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // / display / 127.0.0.1 / 5400
            // -------or---------
            // / display / flight1 / 4
            // either way it sends the info to the BasicDisplay action in MainController and divides to cases there.
            routes.MapRoute("BasicDisplay", "display/{ip}/{port}",
            defaults: new { controller = "Main", action = "BasicDisplay"});


            // / display / 127.0.0.1 / 5400 / 4
            routes.MapRoute("AnimatedDisplay", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Main", action = "AnimatedDisplay" });


            // / save / 127.0.0.1 / 5400 / 4 / 10 / flight1
            routes.MapRoute("DisplayAndSave", "save/{ip}/{port}/{time}/{seconds}/{file}",
            defaults: new { controller = "Main", action = "DisplayAndSave" });


            //don't change:
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
