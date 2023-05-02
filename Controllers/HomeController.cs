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
using Microsoft.EntityFrameworkCore;
using System.Web.Configuration;
using System.Data.Entity.Core.Common.CommandTrees;

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
       

        public ActionResult Dash_Layout()
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
                            car.Status = reader.GetString(18);
                            car.Rating = reader.GetString(19);
                            car.Currency = reader.GetString(20);

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

        //public ActionResult Index(CarDetail searchModel)
        //{

        //    //using (var db = new CarDetailsDbContext())
        //    //{
        //    //    var cars = db.CarDetails.FromSqlInterpolated($@"
        //    //        SELECT *
        //    //        FROM CarDetails
        //    //        WHERE (YOM IS NULL OR YOM = {searchModel.YOM}) 
        //    //          AND (Make LIKE {searchModel.Make} OR {searchModel.Make} IS NULL)
        //    //          AND (Model LIKE {searchModel.Model} OR {searchModel.Model} IS NULL)
        //    //          AND (BodyType LIKE {searchModel.BodyType} OR {searchModel.BodyType} IS NULL)")
        //    //.ToList();

        //    //    return View("SearchItems", cars);




        //        //var cars = db.CarDetails.Where(c =>
        //        //    (searchModel.YOM.HasValue == false || c.YOM == searchModel.YOM.Value) &&
        //        //    (string.IsNullOrEmpty(searchModel.Make) || c.Make.Contains(searchModel.Make)) &&
        //        //    (string.IsNullOrEmpty(searchModel.Model) || c.Model.Contains(searchModel.Model)) &&
        //        //    (string.IsNullOrEmpty(searchModel.BodyType) || c.BodyType.Contains(searchModel.BodyType))
        //        //).ToList();

        //        //return View("SearchItems", cars);
        //    //}
        //}
        //public ActionResult Index(CarDetail searchModel)
        //{
        //    using (var db = new CarDetailsDbContext())
        //    {
        //        var cars = db.CarDetails.AsQueryable();

        //        if (searchModel.YOM == 0)
        //        {
        //            cars = cars.Where(c => c.YOM == searchModel.YOM);
        //        }

        //        if (!string.IsNullOrEmpty(searchModel.Make))
        //        {
        //            cars = cars.Where(c => c.Make.Contains(searchModel.Make));
        //        }

        //        if (!string.IsNullOrEmpty(searchModel.Model))
        //        {
        //            cars = cars.Where(c => c.Model.Contains(searchModel.Model));
        //        }

        //        if (!string.IsNullOrEmpty(searchModel.BodyType))
        //        {
        //            cars = cars.Where(c => c.BodyType.Contains(searchModel.BodyType));
        //        }

        //        return View("SearchItems", cars.ToList());
        //    }
        //}
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
