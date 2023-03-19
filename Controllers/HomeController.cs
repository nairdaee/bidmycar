using BidMyCar.Models;
using System.Web.Mvc;
using BidMyCar.Models;

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
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        //post registration
        [HttpPost]
        public ActionResult Register(UsersInfo u)
        {
            if (ModelState.IsValid == true)
            {

                //password hashing

                u.Password = Encryption.Hash(u.Password);
                u.ConfirmPassword = Encryption.Hash(u.ConfirmPassword);

                db.UsersInfo.Add(u);

                if (db.SaveChanges() > 0)
                {
                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";

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