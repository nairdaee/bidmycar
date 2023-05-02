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
using System.Web.Security;

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


            // Get Google access token using authorization code
            var clientId = "288450486046-07ts86j3aobfbr611dc973rq9vu586jq.apps.googleusercontent.com";
            var clientSecret = "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";
            var url = "http://localhost:60694/Authentication/LoginCallback";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());


            if (userProfile != null)
            {
                var googleUser = JsonConvert.DeserializeObject<GoogleInfo>(userProfile);

                // Check if user already exists in the database
                var existingUser = await db.UsersInfo.FirstOrDefaultAsync(u => u.Email == googleUser.Email);
                if (existingUser == null)
                {
                    // if User does not exist, create a new user entity
                    try
                    {
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

                        //redirect to user profile
                        return RedirectToAction("SelectUserType", "Profile", new { userId = Session["UserId"] });
                    }

                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the error messages into a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                        // Throw a new DbEntityValidationException with the improved exception message.
                        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    }



                }
                else
                {
                    // Store the user information in the session
                    Session["UserId"] = existingUser.UserID;
                    int userId = (int)Session["UserId"];
                    var userData = db.UsersInfo.FirstOrDefault(u => u.UserID == userId);
                    ViewBag.UserData = userData;
                }

            }





            return RedirectToAction("Dashboard", "Profile");

        }



        // GET: Authentication
        public ActionResult Register()
        {
            var clientId = "288450486046-07ts86j3aobfbr611dc973rq9vu586jq.apps.googleusercontent.com";
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


                    //store data into session
                    Session["UserID"] = user.UserID.ToString();
                    Session["Name"] = user.Name.ToString();

                    //redirect to user profile
                    return RedirectToAction("SelectUserType", "Profile", new { userId = Session["UserID"] });

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


            var clientId = "288450486046-07ts86j3aobfbr611dc973rq9vu586jq.apps.googleusercontent.com";
            var url = "http://localhost:60694/Authentication/LoginCallback";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;

            return View();
        }


        //login
        //POST LOGIN
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersInfo user)
        {
            // check if the email is valid
            var credentials = db.UsersInfo.SingleOrDefault(m => m.Email == user.Email);
            if (credentials != null)
            {
                // check if the password is valid
                if (string.Compare(Encryption.Hash(user.Password), credentials.Password) == 0)
                {
                    // create authentication ticket
                    var authTicket = new FormsAuthenticationTicket(
                        1, // version
                        user.Email, // user name
                        DateTime.Now, // issue time
                        DateTime.Now.AddMinutes(30), // expiration time
                        true, // persistent
                        credentials.UserID.ToString() // user data (store user ID)
                    );

                    // encrypt ticket and create cookie
                    var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        HttpOnly = true,
                        Secure = FormsAuthentication.RequireSSL,
                        Domain = FormsAuthentication.CookieDomain,
                        Path = FormsAuthentication.FormsCookiePath,
                        Expires = authTicket.Expiration,
                    };

                    // add cookie to response
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Dashboard", "Profile");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid credentials!";
                }
            }
            else
            {
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

                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {

                }
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

        /*public ActionResult ResetPassword(string id)
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
                */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(resetPassword reset)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var user = db.UsersInfo.FirstOrDefault((System.Linq.Expressions.Expression<Func<UsersInfo, bool>>)(a => a.resetCode == reset.resetCode));
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

        //logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Authentication");
        }

    }
}