using BidMyCar.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.ConstrainedExecution;
using System.Configuration;
using System.Data.SqlClient;

namespace BidMyCar.Controllers
{


    public class HomeController : Controller
    {

        //creating an object of the database
        BidMyCarEntities db = new BidMyCarEntities();
        CarDetailsDbContext db2 = new CarDetailsDbContext();

        
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

        public ActionResult ResetPassword()
        {
            return View();
        }
        //public ActionResult ItemDetails()

        //{
        //    using (var db = new CarDetailsDbContext())
        //    {
        //        //var features = db.CarDetails.ToList();
        //        //return View(features);
        //        var features = db.CarDetails.FirstOrDefault(); // assuming there is only one CarDetail object in the database
        //        var featureList = features.Features.Split(','); // assuming features is a comma-separated string
        //        return View(featureList);
        //    }
        //    //return View();
        //}
        public ActionResult Index()
        {
            //List<Make> cars = CarDetailsDbContext.Make.ToList();

            //// Create a list of ViewModels to pass to the view
            //List<CarViewModel> carViewModels = new List<CarViewModel>();
            //foreach (var car in cars)
            //{
            //    CarViewModel carViewModel = new CarViewModel();
            //    carViewModel.Name = car.Name;
            //    carViewModel.Description = car.Description;
            //    carViewModel.ImageUrl = car.ImageUrl;
            //    carViewModel.Price = car.Price;

            //    // Add the ViewModel to the list
            //    carViewModels.Add(carViewModel);
            //}

            //// Pass the list of ViewModels to the view
            //return View(carViewModels); 
            //return View();
            List<CarDetail> cars = new List<CarDetail>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))

            {
                connection.Open();
                string query = "SELECT * FROM CarDetails";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarDetail car = new CarDetail();
                            car.CarID = reader.GetInt32(0);
                            car.YOM = reader.GetInt32(1);
                            car.Make = reader.GetString(2);
                            car.Model = reader.GetString(3);
                            car.BodyType = reader.GetString(4);
                            car.Condition = reader.GetString(5);
                            car.Category = reader.GetString(6);
                            car.Features = reader.GetString(7);
                            car.CarDescription = reader.GetString(8);
                            car.CarLocation = reader.GetString(9);
                            car.UploadDate = reader.GetDateTime(10);
                            car.Price = reader.GetInt32(11);
                            car.UserID = reader.GetInt32(12);
                            car.Color = reader.GetString(13);
                            car.Miles = reader.GetInt32(14);
                            car.Transmission = reader.GetString(15);
                            car.EngineSize = reader.GetString(16);
                            car.PowerOutput = reader.GetString(17);

                            // Add the car to the list
                            cars.Add(car);
                        }
                    }
                }
            }

            // Create a list of ViewModels to pass to the view
            List<CarDetail> carViewModels = new List<CarDetail>();
            foreach (var car in cars)
            {
                CarDetail carViewModel = new CarDetail();
               

                carViewModel.CarID = car.CarID;
                carViewModel.Model = car.Model;
                carViewModel.YOM = car.YOM;
                carViewModel.Price = car.Price;
                carViewModel.Features = car.Features;
                carViewModel.CarLocation = car.CarLocation;
                carViewModel.CarDescription = car.CarDescription;
                carViewModel.BodyType = car.BodyType;
                carViewModel.Category = car.Category;
                carViewModel.Make = car.Make;
                carViewModel.UploadDate = car.UploadDate;
                carViewModel.Condition = car.Condition;
                carViewModel.Miles = car.Miles;
                carViewModel.Color = car.Color;
                carViewModel.Transmission = car.Transmission;
                carViewModel.EngineSize = car.EngineSize;
                carViewModel.PowerOutput = car.PowerOutput;
                carViewModels.Add(carViewModel);

            }

            // Pass the list of ViewModels to the view
            return View(carViewModels);
        }
        public ActionResult ItemDetails()
        {
            using (var db = new CarDetailsDbContext())
            {
                var features = db.CarDetails.FirstOrDefault();
                //var featureList = features.Features.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
                // return View(featureList);
                var viewModel = new CarDetail
                {
                    CarID = features.CarID,
                    Model = features.Model,
                    YOM = features.YOM,
                    Price = features.Price,
                    Features = features.Features,
                    CarLocation = features.CarLocation,
                    CarDescription = features.CarDescription,
                    BodyType = features.BodyType,
                    Category = features.Category,
                    Make = features.Make,
                    UploadDate = features.UploadDate,
                    Condition = features.Condition,
                    Miles = features.Miles,
                    Color = features.Color,
                    Transmission = features.Transmission,
                    EngineSize = features.EngineSize,
                    PowerOutput = features.PowerOutput


                };
                return View(viewModel);


            }

        }

    }

    }
