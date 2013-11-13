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
                name: "Get Files",
                url: "FileCabinet/GetFiles/{groupId}",
                defaults: new { controller = "FileCabinet", action = "GetFiles" },
                constraints: new { groupId = @"\d+" }
            );

            routes.MapRoute(
                name: "Files",
                url: "FileCabinet/{bandId}/Files/{groupId}",
                defaults: new { controller = "FileCabinet", action = "Files" },
                constraints: new {  bandId = @"\d+", groupId = @"\d+" }
            );

            routes.MapRoute(
                name: "Add Event",
                url: "Calendar/AddEvent/{bandId}/{day}/{month}/{year}",
                defaults: new { controller = "Calendar", action = "AddEvent" },
                constraints: new { bandId = @"\d+", day = @"\d+", month = @"\d+", year = @"\d+"}
            );

            routes.MapRoute(
                name: "Create Contact",
                url: "Rolodex/CreateContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "CreateContact" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Create Band Contact",
                url: "Rolodex/CreateBandContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "CreateBandContact" },
                constraints: new { bandId = @"\d+" }
            );
            routes.MapRoute(
                name: "Create Person Contact",
                url: "Rolodex/CreatePersonContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "CreatePersonContact" },
                constraints: new { bandId = @"\d+" }
            );
            routes.MapRoute(
                name: "Create Venue Contact",
                url: "Rolodex/CreateVenueContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "CreateVenueContact" },
                constraints: new { bandId = @"\d+" }
            );
            routes.MapRoute(
                name: "Another Calendar",
                url: "Calendar/{bandId}/{month}/{year}",
                defaults: new { controller = "Calendar", action = "Index" },
                constraints: new { bandId = @"\d+", month = @"\d+", year = @"\d+" }
            );

            routes.MapRoute(
                name: "Month",
                url: "Calendar/{bandId}/Month/{month}/{year}",
                defaults: new { controller = "Calendar", action = "Month" },
                constraints: new { bandId = @"\d+", month = @"\d+", year = @"\d+" }
            );

            routes.MapRoute(
                name: "Day",
                url: "Calendar/{bandId}/Day/{day}/{month}/{year}",
                defaults: new { controller = "Calendar", action = "Day" },
                constraints: new { bandId = @"\d+", day = @"\d+", month = @"\d+", year = @"\d+" }
            );

            routes.MapRoute(
                name: "Events for Month",
                url: "Calendar/EventsForMonth/{bandId}/{month}/{year}",
                defaults: new { controller = "Calendar", action = "EventsForMonth" },
                constraints: new { bandId = @"\d+", month = @"\d+", year = @"\d+" }
            );

            routes.MapRoute(
                name: "Join Band",
                url: "Band/Join/{bandId}",
                defaults: new { controller = "Band", action = "Join" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Leave Band",
                url: "Band/Leave/{bandId}",
                defaults: new { controller = "Band", action = "Leave" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Post Message",
                url: "Dashboard/Post/{bandId}",
                defaults: new { controller = "Dashboard", action = "Post" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Delete Post",
                url: "Dashboard/DeletePost/{postId}",
                defaults: new { controller = "Dashboard", action = "DeletePost" },
                constraints: new { postId = @"\d+" }
            );

            routes.MapRoute(
                name: "Edit Post",
                url: "Dashboard/EditPost/{postId}",
                defaults: new { controller = "Dashboard", action = "EditPost" },
                constraints: new { postId = @"\d+" }
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
                defaults: new { controller = "FileCabinet", action = "Index" },
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