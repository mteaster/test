﻿using System;
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
                name: "Join",
                url: "Band/Join/{bandId}",
                defaults: new { controller = "Band", action = "Join", bandId = "" }
            );

            routes.MapRoute(
                name: "Post",
                url: "Dashboard/Post/{bandId}",
                defaults: new { controller = "Dashboard", action = "Post", bandId = "" }
            );

            routes.MapRoute(
                name: "Delete",
                url: "Band/Update/{bandId}",
                defaults: new { controller = "Band", action = "Update", bandId = "" }
            );

            routes.MapRoute(
                name: "Delete",
                url: "Band/Delete/{bandId}",
                defaults: new { controller = "Band", action = "Delete", bandId = "" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{bandId}",
                defaults: new { controller = "Dashboard", action = "Index", bandId = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}