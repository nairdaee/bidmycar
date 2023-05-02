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
                string query = "SELECT * FROM CarDetails WHERE (@Make IS NULL OR Make LIKE @Make) AND (@YOM IS NULL OR YOM = @YOM) AND (@BodyType IS NULL OR BodyType = @BodyType) AND (@Category IS NULL OR Category = @Category) AND (@Condition IS NULL OR Condition = @Condition)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(searchModel.Model) ? (object)DBNull.Value : searchModel.Model);
                    command.Parameters.AddWithValue("@Make", string.IsNullOrEmpty(searchModel.Make) ? DBNull.Value : (object)(searchModel.Make + '%'));
                    command.Parameters.AddWithValue("@BodyType", string.IsNullOrEmpty(searchModel.BodyType) ? (object)DBNull.Value : searchModel.BodyType);
                    command.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(searchModel.Category) ? (object)DBNull.Value : searchModel.Category);
                    command.Parameters.AddWithValue("@Condition", string.IsNullOrEmpty(searchModel.Condition) ? (object)DBNull.Value : searchModel.Condition);
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
                                Status = reader.GetString(18),
                                Rating = reader.GetString(19),
                                Currency = reader.GetString(20)
                    })
                            .ToList();

                        return View(carViewModels);
                    }
                }
            }
        }






        public ActionResult ItemDetails(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BidMyCarEntities1"].ConnectionString.Replace("metadata=res://*/", "")))
            {
                connection.Open();
                //CarID, cd.YOM, cd.Make, cd.Model, cd.BodyType, cd.Condition, cd.Category, " +
                //   "cd.Features, cd.CarDescription, cd.CarLocation, cd.UploadDate, cd.Price, cd.UserID, " +
                //   "cd.Color, cd.Miles, cd.Transmission, cd.EngineSize, cd.PowerOutput,
                //ImageID, vi.CarID, vi.Image1, vi.Image2, vi.Image3, vi.Image4, vi.Image5
                string query = "SELECT cd.*, " +
                "vi.* " +
                "FROM CarDetails cd " +
                "JOIN VehicleImages vi ON cd.CarID = vi.CarID " +
                "WHERE cd.CarID = @CarID";


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
                            car.Status = reader.GetString(18);
                            car.Rating = reader.GetString(19);
                            car.Currency = reader.GetString(20);


                            // Load vehicle images into a list
                            VehicleImage vehicle = new VehicleImage();
                            vehicle.ImageID = reader.GetInt32(21);
                            vehicle.CarID = reader.GetInt32(22);
                            vehicle.Image1 = reader.GetString(23);
                            vehicle.Image2 = reader.GetString(24);
                            vehicle.Image3 = reader.GetString(25);
                            vehicle.Image4 = reader.GetString(26);
                            vehicle.Image5 = reader.GetString(27);
                            if (!reader.IsDBNull(28))
                            {
                                vehicle.Iframe = reader.GetString(28);

                            }
                           
                            if (!reader.IsDBNull(29))
                            {
                                vehicle.Youtube = reader.GetString(29);
                            }

                            if (!reader.IsDBNull(30))
                            { 
                                vehicle.Vimeo = reader.GetString(30);
                            }


                            var data = Tuple.Create(car, vehicle);
                            // Pass the car details to the view
                            return View("ItemDetails", data);
                        }
                    }
                }


                // If no matching car was found, return an error view
                return View("Error");
            }

            //}
            //    public IActionResult Index()
            //    {
            //        var carDetails = new List<CarDetailModel>();
            //        var vehicleImages = new List<VehicleImageModel>();

            //        using (var connection = new SqlConnection(_connectionString))
            //        {
            //            connection.Open();

            //            // Retrieve all fields from CarDetails table
            //            using (var command = new SqlCommand("SELECT * FROM CarDetails", connection))
            //            {
            //                using (var reader = command.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        var carDetail = new CarDetailModel()
            //                        {
            //                            CarID = (int)reader["CarID"],
            //                            YOM = (int)reader["YOM"],
            //                            Make = (string)reader["Make"],
            //                            Model = (string)reader["Model"],
            //                            BodyType = (string)reader["BodyType"],
            //                            Condition = (string)reader["Condition"],
            //                            Category = (string)reader["Category"],
            //                            Features = (string)reader["Features"],
            //                            CarDescription = (string)reader["CarDescription"],
            //                            CarLocation = (string)reader["CarLocation"],
            //                            UploadDate = (DateTime)reader["UploadDate"],
            //                            Price = (int)reader["Price"],
            //                            UserID = (int)reader["UserID"],
            //                            Color = (string)reader["Color"],
            //                            Miles = (int)reader["Miles"],
            //                            Transmission = (string)reader["Transmission"],
            //                            EngineSize = (string)reader["EngineSize"],
            //                            PowerOutput = (string)reader["PowerOutput"]
            //                        };
            //                        carDetails.Add(carDetail);
            //                    }
            //                }
            //            }

            //            // Retrieve all fields from VehicleImages table
            //            using (var command = new SqlCommand("SELECT * FROM VehicleImages", connection))
            //            {
            //                using (var reader = command.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                    {
            //                        var vehicleImage = new VehicleImage()
            //                        {
            //                            ImageID = (int)reader["ImageID"],
            //                            CarID = (int)reader["CarID"],
            //                            Image1 = (string)reader["Image1"],
            //                            Image2 = (string)reader["Image2"],
            //                            Image3 = (string)reader["Image3"],
            //                            Image4 = (string)reader["Image4"],
            //                            Image5 = (string)reader["Image5"]
            //                        };
            //                        vehicleImages.Add(vehicleImage);
            //                    }
            //                }
            //            }

            //            connection.Close();
            //        }

            //        ViewBag.CarDetails = carDetails;
            //        ViewBag.VehicleImages = vehicleImages;

            //        return View();
             }
        }

}

