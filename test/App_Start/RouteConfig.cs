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
                name: "Download Band Avatar",
                url: "Rolodex/DownloadBandAvatar/{contactId}",
                defaults: new { controller = "Rolodex", action = "DownloadBandAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Download Venue Avatar",
                url: "Rolodex/DownloadVenueAvatar/{contactId}",
                defaults: new { controller = "Rolodex", action = "DownloadVenueAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Download People Avatar",
                url: "Rolodex/DownloadPeopleAvatar/{contactId}",
                defaults: new { controller = "Rolodex", action = "DownloadPeopleAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Download Venue Avatar",
                url: "Rolodex/DownloadVenueAvatar/{contactId}",
                defaults: new { controller = "Rolodex", action = "DownloadVenueAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Download People Avatar",
                url: "Rolodex/DownloadPeopleAvatar/{contactId}",
                defaults: new { controller = "Rolodex", action = "DownloadPeopleAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Upload Contact Avatar",
                url: "Rolodex/UploadAvatar/{contactId}/{contactType}",
                defaults: new { controller = "Rolodex", action = "UploadAvatar" },
                constraints: new { contactId = @"\d+" }
            );

            routes.MapRoute(
                name: "Create Group",
                url: "FileCabinet/{bandId}/CreateGroup",
                defaults: new { controller = "FileCabinet", action = "CreateGroup" },
                constraints: new { bandId = @"\d+" }
            );

            routes.MapRoute(
                name: "Delete Group",
                url: "FileCabinet/DeleteGroup/{groupId}",
                defaults: new { controller = "FileCabinet", action = "DeleteGroup" },
                constraints: new { groupId = @"\d+" }
            );

            routes.MapRoute(
                name: "View File",
                url: "FileCabinet/ViewFile/{fileId}",
                defaults: new { controller = "FileCabinet", action = "ViewFile" },
                constraints: new { fileId = @"\d+" }
            );

            routes.MapRoute(
                name: "Delete File",
                url: "FileCabinet/DeleteFile/{fileId}",
                defaults: new { controller = "FileCabinet", action = "DeleteFile" },
                constraints: new { fileId = @"\d+" }
            );

            routes.MapRoute(
                name: "Download File",
                url: "FileCabinet/DownloadFile/{fileId}",
                defaults: new { controller = "FileCabinet", action = "DownloadFile" },
                constraints: new { fileId = @"\d+" }
            );

            routes.MapRoute(
                name: "Upload File",
                url: "FileCabinet/UploadFile/{groupId}",
                defaults: new { controller = "FileCabinet", action = "UploadFile" },
                constraints: new { groupId = @"\d+" }
            );

            routes.MapRoute(
                name: "Get Files",
                url: "FileCabinet/GetFiles/{groupId}",
                defaults: new { controller = "FileCabinet", action = "GetFiles" },
                constraints: new { groupId = @"\d+" }
            );

            routes.MapRoute(
                name: "Get Groups",
                url: "FileCabinet/GetGroups/{bandId}",
                defaults: new { controller = "FileCabinet", action = "GetGroups" },
                constraints: new { bandId = @"\d+" }
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
                name: "Edit Band Contact",
                url: "Rolodex/EditBandContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "EditBand" },
                constraints: new { bandId = @"\d+" }
            );
            routes.MapRoute(
                name: "Edit Person Contact",
                url: "Rolodex/EditPersonContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "EditPerson" },
                constraints: new { bandId = @"\d+" }
            );
            routes.MapRoute(
                name: "Edit Venue Contact",
                url: "Rolodex/EditVenueContact/{bandId}",
                defaults: new { controller = "Rolodex", action = "EditVenue" },
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
                name: "Messageboard Page",
                url: "Dashboard/{bandId}/Page/{pageNumber}",
                defaults: new { controller = "Dashboard", action = "Page" },
                constraints: new { bandId = @"\d+", pageNumber = @"\d+" }
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