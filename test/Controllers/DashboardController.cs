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
        // GET: /Dashboard/DeletePost

        public ActionResult DeletePost(int postId)
        {
            // TODO: Add redirect back to dashboard (too lazy to do it now)

            if (MessageBoardUtil.DeletePost(postId))
            {
                ViewBag.SuccessMessage = "Post deleted!";
            }
            else
            {
                ViewBag.ErrorMessage = "You cannot delete someone else's post!";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        //
        // /Dashboard/EditPost

        [HttpPost]
        public ActionResult EditPost(int postId, PostMessageModel model)
        {
            if (MessageBoardUtil.EditPost(postId, model.Content))
            {
                ViewBag.SuccessMessage = "Post edited!";
            }
            else
            {
                ViewBag.ErrorMessage = "You cannot edit someone else's post!";
            }

            return RedirectToLocal(Request.UrlReferrer.AbsolutePath);
        }

        //
        // GET: /Dashboard/EditPost

        public ActionResult EditPost(int postId)
        {
            ViewBag.PostId = postId;
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
                ViewBag.SuccessMessage = "Message posted!";
            }
            else
            {
                ViewBag.ErrorMessage = "We don't like empty messages.";
            }

            dvm.DisplayMessagesModel = MessageBoardUtil.PostsFor(bandId);

            return View(dvm);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }    
}
