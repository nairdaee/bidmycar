using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMyCar.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //creating an object of the database
        BidMyCarEntities2 db = new BidMyCarEntities2();
        BidMyCarEntities db2 = new BidMyCarEntities();

        public ActionResult SelectUserType(string userType)
        {
            Session["User_type"] = userType;

            var user = (string)Session["User_type"];
            ViewBag.UserType = user;

            return View();

        }


        public ActionResult Dashboard(Profile user)
        {
            // Get the user's ID from the session
            var userId = Session["userId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Convert the user ID to an integer
            if (!int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Get the user's type from the database based on the ID
            var userType = db.Profile
                .Where(u => u.Prof_ID == id)
                .Select(u => u.User_type)
                .FirstOrDefault();

            // Pass the user type to the view using ViewBag
            ViewBag.UserType = userType;

            // Get the user's details from the database based on the ID
            var person = db.Profile
                .Where(u => u.Prof_ID == id)
                .FirstOrDefault();

            return View(person);
        }

        public ActionResult NormalUser()
        {

            return View();
        }


        //buyer creating a profile
        [HttpPost]
        public ActionResult NormalUser(Profile profile)
        {

            if (ModelState.IsValid)
            {

                // get the user id from the session
                var userId = Session["userId"] != null ? (int)Session["userId"] : 0;

                // create a new profile with the user id
                var newProfile = new Profile
                    {
                        Prof_ID = userId,
                        Name = profile.Name,
                        Username = profile.Username,
                        Email = profile.Email,
                        Phone_no = profile.Phone_no,
                        Bio = profile.Bio,
                        Location = profile.Location,
                        Instagram = profile.Instagram,
                        Twitter = profile.Twitter,
                        WebsiteUrl = profile.WebsiteUrl,
                        User_type = "buyer",
                        userId = userId
                        

                    };

                    // add the new profile to the database
                    db.Profile.Add(newProfile);
                    db.SaveChanges();


                // Redirect the user to the dashboard
                return RedirectToAction("Dashboard", "Profile");
            }
            return View();

        }
        public ActionResult BookmarkedItems()
        {
            return View();
        }
        public ActionResult MyItems()
        {
            return View();
        }
        public ActionResult ProfileSettings()
        {
            return View();
        }
        public ActionResult PostItem()
        {
            return View();
        }

        public ActionResult Dealership()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dealership(Profile user)
        {
            if (ModelState.IsValid)
            {

                // get the user id from the session
                var userId = Session["userId"] != null ? (int)Session["userId"] : 0;

                //get the usertype
                var userType = Request.Form["UserType"];

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    user.Prof_ID = userId;
                    user.userId = userId;
                    db.Profile.Add(user);
                    db.SaveChanges();

                    //redirect to the dashboard

                    return RedirectToAction("Dashboard", "Profile");
                   
                }

            }
            return View();
        }

        public ActionResult IndividualSeller()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndividualSeller(Profile user)
        {
            if (ModelState.IsValid)
            {
                // get the user id from the session
                var userId = Session["userId"] != null ? (int)Session["userId"] : 0;

                //get the usertype
                var userType = Request.Form["UserType"];

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    user.Prof_ID = userId;
                    user.userId = userId;
                    db.Profile.Add(user);
                    db.SaveChanges();

                    //redirect to the dashboard

                    return RedirectToAction("Dashboard", "Profile");

                }

            }

            return View();
        }
    }
}