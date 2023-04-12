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

        public ActionResult SelectUserType(string userType)
        {

            // store the user type in a session variable
            
            Session["User_type"] = userType;

            var user = (string)Session["User_type"];
            ViewBag.UserType = user;

            return View();
        }

            public ActionResult Dashboard()
        {

            return View();
        }
        public ActionResult NormalUser()
        {
            return View();
        }


        //buyer creating a profile
        [HttpPost]
        public ActionResult NormalUser(Profile user)
        {

            if (ModelState.IsValid)
            {
                var userType = Request.Form["UserType"];

                if(userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    db.Profile.Add(user);
                    db.SaveChanges();

                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";
                }

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
                var userType = Request.Form["UserType"];

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    db.Profile.Add(user);
                    db.SaveChanges();

                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";
                }

            }
            return View();
        }

        public ActionResult IndividualSeller()
        {
            return View();
        }
    }
}