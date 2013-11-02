﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using test.Stuff;
using WebMatrix.WebData;

namespace band.Controllers
{
    
    [Authorize]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/RemovePost

        public ActionResult DeletePost(int postId, string returnUrl)
        {
            // TODO: Add redirect back to dashboard (too lazy to do it now)

            if (MessageBoardUtil.DeletePost(postId))
            {
                ViewBag.StatusMessage = "Post deleted!";
                return View("Success");
            }
            else
            {
                ViewBag.StatusMessage = "You cannot delete someone else's post!";
                return View("Error");
            }
        }

        //
        // /Dashboard/EditPost

        [HttpPost]
        public ActionResult EditPost(int postId, PostMessageModel model)
        {
            // TODO: Add redirect back to dashboard (too lazy to do it now)

            if (MessageBoardUtil.EditPost(postId, model.Content))
            {
                ViewBag.StatusMessage = "Post edited!";
                return View("Success");
            }
            else
            {
                ViewBag.StatusMessage = "You cannot edit someone else's post!";
                return View("Error");
            }
        }

        //
        // GET: /Dashboard/EditPost

        public ActionResult EditPost(int postId)
        {
            return View();
        }

        //
        // GET: /Dashboard/

        public ActionResult Index(int bandId)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            DashboardViewModel dvm = new DashboardViewModel();

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            dvm.DisplayMessagesModel = MessageBoardUtil.PostsFor(bandId);
            
            return View(dvm);
        }

        //
        // POST: /Dashboard/

        [HttpPost]
        public ActionResult Index(int bandId, PostMessageModel model)
        {
            // Check if band exists - if it does, get band profile
            BandProfile bandProfile = BandUtil.BandProfileFor(bandId);

            ViewBag.BandId = bandId;
            ViewBag.BandName = bandProfile.BandName;

            // Check if the user is in the band
            if (!BandUtil.IsUserInBand(WebSecurity.CurrentUserId, bandId))
            {
                return RedirectToAction("Join", "Band");
            }

            DashboardViewModel dvm = new DashboardViewModel();
            dvm.PostMessageModel = model;

            if (ModelState.IsValid)
            {
                MessageBoardUtil.AddPost(bandId, model.Content);
                ViewBag.StatusMessage = "message posted successfully";
            }
            else
            {
                ViewBag.StatusMessage = "something was wrong with your message";
            }

            dvm.DisplayMessagesModel = MessageBoardUtil.PostsFor(bandId);

            return View(dvm);
        }
    }    
}
