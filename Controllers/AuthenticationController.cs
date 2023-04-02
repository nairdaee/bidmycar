using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using GoogleAuthentication.Services;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BidMyCar.Controllers
{
    public class AuthenticationController : Controller
    {


        //creating an object of the database
        BidMyCarEntities db = new BidMyCarEntities();


        // GET: Authentication/LoginCallback
        public async Task<ActionResult> LoginCallback(string code, UsersInfo newUser)
        {
            if (string.IsNullOrEmpty(code))
            {
                // No authorization code, redirect to login page
                return RedirectToAction("Login");
            }

            try
            {
                // Get Google access token using authorization code
                var clientId = "288450486046-7ni9arjrk2derhn1s5vbg7nakt3105is.apps.googleusercontent.com";
                var clientSecret = "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";
                var url = "http://localhost:60694/Authentication/LoginCallback";
                var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                

                if(userProfile != null)
                {
                    var googleUser = JsonConvert.DeserializeObject<GoogleInfo>(userProfile);

                    // Check if user already exists in the database
                    var existingUser = await db.UsersInfo.FirstOrDefaultAsync(u => u.Email == googleUser.Email);
                    if (existingUser == null)
                    {
                        // if User does not exist, create a new user entity
                       
                        newUser.Email = googleUser.Email;
                        newUser.Name = googleUser.GivenName;
                        newUser.GoogleId = googleUser.Id;

                        // Save the new user to the database
                        db.UsersInfo.Add(newUser);
                        db.SaveChanges();

                        // Store the user information in the session
                        Session["UserId"] = newUser.UserID;
                        Session["Email"] = newUser.Email;
                        Session["Name"] = newUser.Name;
                    }
                    else
                    {
                        // Store the user information in the session
                        Session["UserId"] = existingUser.UserID;
                        Session["Email"] = existingUser.Email;
                        Session["Name"] = existingUser.Name;

                    }

                }
           
        }

            catch (Exception ex) { 
            }



            return RedirectToAction("Index", "UsersProfile");

        }



        // GET: Authentication
        public ActionResult Register()
        {
            var clientId = "288450486046-7ni9arjrk2derhn1s5vbg7nakt3105is.apps.googleusercontent.com";
            var url = "http://localhost:60694/Authentication/LoginCallback";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;


            return View();
        }


        //post
        [HttpPost]
        public ActionResult Register(UsersInfo user)
        {
            if (ModelState.IsValid)
            {
                // Check if user has already used the email to register
                var check = db.UsersInfo.Where(m => m.Email == user.Email).FirstOrDefault();
                if (check == null)
                {
                    // Password hashing
                    user.Password = Encryption.Hash(user.Password);
                    user.ConfirmPassword = Encryption.Hash(user.ConfirmPassword);
                    db.UsersInfo.Add(user);
                    db.SaveChanges();

                    // Success message
                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";

                    //store data into session
                    Session["UserID"] = user.UserID.ToString();
                    Session["Name"] = user.Name.ToString();

                    //redirect to user profile
                    return RedirectToAction("Index", "UsersProfile");

                }
                else
                {
                    // Error message
                    ViewBag.ErrorMessage = "Email address is already registered! Please choose a different email address.";
                }
            }
            // If model state is not valid or email already exists, return to the registration page with error message
            return View();
        }

        //login
        public ActionResult Login()
        {
           

            var clientId = "288450486046-7ni9arjrk2derhn1s5vbg7nakt3105is.apps.googleusercontent.com";
            var url = "http://localhost:60694/Authentication/LoginCallback";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;

            return View();
        }


        //login
        //POST LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersInfo user)
        {

            //check if the email is valid
            var credentials = db.UsersInfo.Where(m => m.Email == user.Email).FirstOrDefault();
            if (credentials != null)
            {

                if (string.Compare(Encryption.Hash(user.Password), credentials.Password) == 0)
                {


                    Session["UserID"] = user.UserID.ToString();
                    Session["Email"] = user.Email.ToString();
                    return RedirectToAction("Index", "UsersProfile");

                }
                else
                {
                    // Error message
                    ViewBag.ErrorMessage = "Invalid credentials!";
                }
            }
            else
            {
                // Error message
                ViewBag.ErrorMessage = "Email address not found!";

            }

            return View();
        }





        //forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }


        //post
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 


            using (db)
            {
                var account = db.UsersInfo.Where(m => m.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendResetLink(account.Email, resetCode);
                    account.resetCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Password reset link sent to your email.";
                }
                else
                {
                    // Error message
                    ViewBag.ErrorMessage = "Sorry, we couldn't find an account with that email address. Please try again or register for a new account.";
                }
            }

            return View();
        }

        [NonAction]
        //email link method
        public void SendResetLink(string Email, string resetCode)
        {
            var verifyUrl = "/Authentication/ResetPassword/" + resetCode;
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

                // Encode the link URL to remove < and > characters
                var encodedLink = HttpUtility.HtmlEncode(link);

                // Create a plain text version of the message
                var plainTextView = AlternateView.CreateAlternateViewFromString("We received a request to reset your password. Please copy and paste the following link into your browser to reset your password: " + encodedLink, Encoding.UTF8, MediaTypeNames.Text.Plain);

                // Create an HTML version of the message
                var htmlView = AlternateView.CreateAlternateViewFromString("We received a request to reset your password. Please click the link below to reset your password: <a href=\"" + encodedLink + "\">Reset Password</a>", Encoding.UTF8, MediaTypeNames.Text.Html);

                // Add both the plain text and HTML versions of the message to the MailMessage object
                message.AlternateViews.Add(plainTextView);
                message.AlternateViews.Add(htmlView);

                smtp.Send(message);
            }


        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (db)
            {
                var user = db.UsersInfo.Where(a => a.resetCode == id).FirstOrDefault();
                if (user != null)
                {
                    resetPassword model = new resetPassword();
                    model.resetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(resetPassword reset)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var user = db.UsersInfo.FirstOrDefault(a => a.resetCode == reset.resetCode);
                    if (user != null)
                    {
                        user.Password = Encryption.Hash(reset.NewPassword);
                        user.resetCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        ViewBag.SuccessMessage = "Password has been changed successfully!";
                        return RedirectToAction("Login", "Authentication");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid reset code.";
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid input.";
            }

            return View();
        }

    }
}