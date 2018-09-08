using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Site.View
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AEmpresa",
                url: "empresa",
                defaults: new { controller = "Post", action = "GetByCategory", categoria = 1 },
                namespaces: new[] { "Site.View.Controllers" }
            );

            routes.MapRoute(
                name: "Pracas",
                url: "pracas",
                defaults: new { controller = "Post", action = "GetByCategory", categoria = 2 },
                namespaces: new[] { "Site.View.Controllers" }
            );

            routes.MapRoute(
                name: "Parceiros",
                url: "parceiros",
                defaults: new { controller = "Post", action = "GetByCategory", categoria = 3 },
                namespaces: new[] { "Site.View.Controllers" }
            );

            routes.MapRoute(
                name: "Responsablidade",
                url: "responsabilidade",
                defaults: new { controller = "Post", action = "GetByCategory", categoria = 4 },
                namespaces: new[] { "Site.View.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Site.View.Controllers" }
            );
        }
    }
}