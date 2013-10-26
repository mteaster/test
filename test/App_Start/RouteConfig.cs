using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace test
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Join Band",
                url: "Band/Join/{bandId}",
                defaults: new { controller = "Band", action = "Join" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Post Message",
                url: "Dashboard/Post/{bandId}",
                defaults: new { controller = "Dashboard", action = "Post" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{bandId}",
                defaults: new { controller = "Dashboard", action = "Index" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Manage Band",
                url: "Band/Manage/{bandId}",
                defaults: new { controller = "Band", action = "Manage" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Delete Band",
                url: "Band/Delete/{bandId}",
                defaults: new { controller = "Band", action = "Delete" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Calendar",
                url: "Calendar/{bandId}",
                defaults: new { controller = "Calendar", action = "Index" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Budget",
                url: "Budget/{bandId}",
                defaults: new { controller = "Budget", action = "Index" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Online",
                url: "Online/{bandId}",
                defaults: new { controller = "Online", action = "Index" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Rolodex",
                url: "Rolodex/{bandId}",
                defaults: new { controller = "Rolodex", action = "Index" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "File Cabinet",
                url: "FileCabinet/{bandId}",
                defaults: new { controller = "Band", action = "Join" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}