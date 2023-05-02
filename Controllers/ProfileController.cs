using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BidMyCar.Controllers
{
    public class ProfileController : Controller

    {
        // GET: Profile
        //public ActionResult Index()
        //{
        //    return View();
        //}
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

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    db.Profile.Add(user);
                    db.SaveChanges();

                    ViewBag.InsertMessage = "<script> alert('User Registered Successfully!')</script>";
                    return RedirectToAction("Dashboard");
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
            // List<CarDetail> cars = new List<CarDetail>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))
            {
                connection.Open();
                string query = "SELECT * FROM CarDetails WHERE UserID=1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarID", 1);
                    using (var reader = command.ExecuteReader())
                    {
                        var carViewModels = reader.Cast<IDataRecord>()
                            .Select(record => new CarDetail
                            {
                                CarID = record.GetInt32(0),
                                YOM = record.GetInt32(1),
                                Make = record.GetString(2),
                                Model = record.GetString(3),
                                BodyType = record.GetString(4),
                                Condition = record.GetString(5),
                                Category = record.GetString(6),
                                Features = record.GetString(7),
                                CarDescription = record.GetString(8),
                                CarLocation = record.GetString(9),
                                UploadDate = record.GetDateTime(10),
                                Price = record.GetInt32(11),
                                UserID = record.GetInt32(12),
                                Color = record.GetString(13),
                                Miles = record.GetInt32(14),
                                Transmission = record.GetString(15),
                                EngineSize = record.GetString(16),
                                PowerOutput = record.GetString(17),
                            })
                            .ToList();

                        return View(carViewModels);



                    }
                }
            }
            // return View("Error");

        }

        // Pass the list of car details to the view






        // If no matching car was found, return an error view


        public ActionResult ProfileSettings()
        {
            return View();
        }

        public ActionResult PostItem(CarDetail car)
        {

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))
            {



                //string query = "INSERT INTO CarDetails (YOM, Make, Model, BodyType, Condition, Category, Features, CarDescription, CarLocation, UploadDate, Price, UserID, Color, Miles, Transmission, EngineSize, PowerOutput) VALUES (@YOM, @Make, @Model, @BodyType, @Condition, @Category, @Features, @CarDescription, @CarLocation, @UploadDate, @Price, @UserID, @Color, @Miles, @Transmission, @EngineSize, @PowerOutput, @Status,@Rating, @Currency)";
                string query = "INSERT INTO CarDetails (YOM, Make, Model, BodyType, Condition, Category, Features, CarDescription, CarLocation, UploadDate, Price, /*UserID*/ Color, Miles, Transmission, EngineSize, PowerOutput, Status, Rating, Currency) VALUES (@YOM, @Make, @Model, @BodyType, @Condition, @Category, @Features, @CarDescription, @CarLocation, @UploadDate, @Price, /*@UserID*/ @Color, @Miles, @Transmission, @EngineSize, @PowerOutput, @Status, @Rating, @Currency)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string[] selectedFeatures = Request.Form.GetValues("Features") ?? new string[] { };
                    string Features = string.Join(",", selectedFeatures);
                    command.Parameters.AddWithValue("@YOM", car.YOM);
                    //command.Parameters.AddWithValue("@Make", car.Make);
                    //command.Parameters.AddWithValue("@Model", car.Model);
                    //command.Parameters.AddWithValue("@BodyType", car.BodyType);
                    //command.Parameters.AddWithValue("@Condition", car.Condition);
                    //command.Parameters.AddWithValue("@Category", car.Category);
                    //command.Parameters.AddWithValue("@Features", Features);
                    //command.Parameters.AddWithValue("@CarDescription", car.CarDescription);
                    //command.Parameters.AddWithValue("@CarLocation", car.CarLocation);
                    //command.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                    //command.Parameters.AddWithValue("@Price", car.Price);
                    ////command.Parameters.AddWithValue("@UserID", car.UserID);
                    //command.Parameters.AddWithValue("@Color", car.Color);
                    //command.Parameters.AddWithValue("@Miles", car.Miles);
                    //command.Parameters.AddWithValue("@Transmission", car.Transmission);
                    //command.Parameters.AddWithValue("@EngineSize", car.EngineSize);
                    //command.Parameters.AddWithValue("@PowerOutput", car.PowerOutput);
                    //command.Parameters.AddWithValue("@Status", car.Status);
                    //command.Parameters.AddWithValue("@Rating", car.Rating);
                    //command.Parameters.AddWithValue("@Currency", car.Currency);
                    command.Parameters.AddWithValue("@Make", (object)car.Make ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Model", (object)car.Model ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BodyType", (object)car.BodyType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Condition", (object)car.Condition ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Category", (object)car.Category ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Features", (object)Features ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CarDescription", (object)car.CarDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CarLocation", (object)car.CarLocation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Price", (object)car.Price ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@UserID", (object)car.UserID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Color", (object)car.Color ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Miles", (object)car.Miles ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Transmission", (object)car.Transmission ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EngineSize", (object)car.EngineSize ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PowerOutput", (object)car.PowerOutput ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (object)car.Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Rating", (object)car.Rating ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Currency", (object)car.Currency ?? (object)DBNull.Value);




                    connection.Open();
                    command.ExecuteNonQuery();
                }


                
            }
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
                var userId = Session["UserID"] != null ? (int)Session["UserID"] : 0;

                //get the usertype
                var userType = Request.Form["UserType"];

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    user.UserID = userId;
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
                var userId = Session["UserID"] != null ? (int)Session["UserID"] : 0;

                //get the usertype
                var userType = Request.Form["UserType"];

                if (userType != null)
                {
                    // Save the new user to the database
                    user.User_type = userType;
                    user.UserID = userId;
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