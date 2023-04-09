using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMyCar.Controllers
{
    public class CarDetailsController : Controller
    {
        // GET: CarDetails
        //public ActionResult Index()
        //{
        //    return View();
        //}
     
        public ActionResult SearchItems(CarDetail searchModel)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))
            {
                connection.Open();
                string query = "SELECT * FROM CarDetails WHERE (@Make IS NULL OR Make LIKE @Make) AND (@YOM IS NULL OR YOM = @YOM) AND (@BodyType IS NULL OR BodyType = @BodyType)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(searchModel.Model) ? (object)DBNull.Value : searchModel.Model);
                    command.Parameters.AddWithValue("@Make", string.IsNullOrEmpty(searchModel.Make) ? DBNull.Value : (object)(searchModel.Make + '%'));
                    command.Parameters.AddWithValue("@BodyType", string.IsNullOrEmpty(searchModel.BodyType) ? (object)DBNull.Value : searchModel.BodyType);
                    command.Parameters.AddWithValue("@YOM", searchModel.YOM == 0 ? (object)DBNull.Value : searchModel.YOM);
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
        }




        //public ActionResult SearchItems(CarDetail searchModel)
        //{
        //    using (var db = new CarDetailsDbContext())
        //    {
        //        //    var cars = db.CarDetails.AsQueryable();

        //        //if (searchModel.YOM == 0)
        //        //{
        //        //    cars = cars.Where(c => c.YOM == searchModel.YOM);
        //        //}

        //        //if (!string.IsNullOrEmpty(searchModel.Make))
        //        //{
        //        //    cars = cars.Where(c => c.Make.Contains(searchModel.Make));
        //        //}

        //        //if (!string.IsNullOrEmpty(searchModel.Model))
        //        //{
        //        //    cars = cars.Where(c => c.Model.Contains(searchModel.Model));
        //        //}

        //        //if (!string.IsNullOrEmpty(searchModel.BodyType))
        //        //{
        //        //    cars = cars.Where(c => c.BodyType.Contains(searchModel.BodyType));
        //        //}

        //        //return View("SearchItems", cars.ToList());
        //        var cars = db.CarDetails.Where(c =>
        //        ( c.YOM == searchModel.YOM) ||
        //        ( c.Make.Contains(searchModel.Make)) ||
        //        ( c.Model.Contains(searchModel.Model)) ||
        //        (c.BodyType.Contains(searchModel.BodyType))
        //    ).ToList();

        //        return View("SearchItems", cars);
        //    }
        //}

        public ActionResult ItemDetails(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))
            {
                connection.Open();
                string query = "SELECT * FROM CarDetails WHERE CarID=@CarID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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

                            // Pass the car details to the view
                            return View(car);
                        }
                    }
                }
            }

            // If no matching car was found, return an error view
            return View("Error");
        }

    }
    }
