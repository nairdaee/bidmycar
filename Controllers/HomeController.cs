using BidMyCar.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Web;
using System;
using WebMatrix.WebData;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using System.Data.Entity.Validation;
using System.Data.Entity;

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

                //check if user has already used the email to register
                var check = db.UsersInfo.Where(m => m.Email == user.Email).FirstOrDefault();
                if (check == null)
                {
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
                else
                {
                    ViewBag.LoginStatus = 0;
                }



            }

            return View();
        }


        //forgot password action
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //post
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(forgotPassword user)
        {
            //find the email account and verifying
            using (db)
            {
                var account = db.UsersInfo.Where(m => m.Email == user.Email).FirstOrDefault();


                //if account is verified send the link to the email
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendResetLink(user.Email, resetCode);

                    account.resetCode = resetCode;
                    try
                    {
                        // Save changes to the database
                        db.SaveChanges();

                        if(db.SaveChanges() > 0)
                        {
                            ViewBag.InsertMessage = "<script> alert('Email is sent!')</script>";
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Handle validation errors
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }

                  

                }

                else { ViewBag.LoginStatus = 0; }


                return View();
            }
        }


        //email link method
        public void SendResetLink(string Email, string resetCode)
        {
            var verifyUrl = "/Home/ForgotPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("tiffahnick012@gmail.com", "BidMyCar");
            var toEmail = new MailAddress(Email);


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, "ypsupvagloikrevp")
            };
            var message = new MailMessage(fromEmail, toEmail);
            using (message)
            {
                message.Subject = "Password reset request";
                message.Body ="We got a request  for resetting your password. Please click the link below to reset your password"+ link ;



                smtp.Send(message);

            }

            
        }

    //Reset Password
        public ActionResult ResetPassword(string id)
        {
            //verify the password link
            using (db)
            {
                var code = db.UsersInfo.Where(a => a.resetCode == id).FirstOrDefault();
                if (code != null)
                {
                    //instantiate reset password class
                    resetPassword reset = new resetPassword();
                    reset.resetCode = id;

                    return View(reset);

                }
                else
                {
                    return HttpNotFound();
                }
            }
        }



        //Post click
        [HttpPost]
        public ActionResult ResetPassword(resetPassword model)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    // Retrieve user information based on reset code
                    var user = db.UsersInfo.Where(u => u.resetCode == model.resetCode).FirstOrDefault();

                    if (user != null)
                    {
                        // Update user's password and reset code
                        user.Password = Encryption.Hash(model.NewPassword);
                        user.resetCode = null;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        return View("ResetPasswordConfirmation");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }





        public ActionResult ItemDetails()
        {
            return View();
        }
        public ActionResult SearchItems()
        {
            return View();
        }
    }
}