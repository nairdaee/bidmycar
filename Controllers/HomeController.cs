using BidMyCar.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Web;
using System;

namespace BidMyCar.Controllers
{


    public class HomeController : Controller
    {

        //creating an object of the database
        BidMyCarEntities db = new BidMyCarEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Login
        public ActionResult Login()
        {
            return View();
        }

        //POST LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersInfo user)
        {

            var credentials = db.UsersInfo.Where(m => m.Email == user.Email).FirstOrDefault();
            if (credentials != null)
            {

                if (string.Compare(Encryption.Hash(user.Password), credentials.Password) == 0)
                {


                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";
                    Session["UserID"] = user.UserID.ToString();
                    Session["Email"] = user.Email.ToString();
                    return RedirectToAction("Index", "UserProfile");

                }
                else
                {
                    ViewBag.LoginStatus = 0;
                }
            }
            else
            {
                ViewBag.LoginStatus = 0;

            }




           return View();
        }

        //Registration
        public ActionResult Register()
        {
            return View();
        }


        //post registration
        [HttpPost]
        public ActionResult Register(UsersInfo user)
        {
            if (ModelState.IsValid == true)
            {

                //password hashing

                user.Password = Encryption.Hash(user.Password);
                user.ConfirmPassword = Encryption.Hash(user.ConfirmPassword);

                db.UsersInfo.Add(user);

                if (db.SaveChanges() > 0)
                {
                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";
                    Session["UserID"] = user.UserID.ToString();
                    Session["Name"] = user.Name.ToString();
                    return RedirectToAction("Index", "UserProfile");

                }
                else
                {
                    ViewBag.InsertMessage = "<script> alert('Registration failed!')</script>";
                }

            }

            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}