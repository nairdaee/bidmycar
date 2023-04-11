using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMyCar.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult SelectUserType()
        {
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
                } }
   // return View("Error");

                }

                // Pass the list of car details to the view
            
            
    

            

            // If no matching car was found, return an error view
        
          
        public ActionResult ProfileSettings()
        {
            return View();
        }
        public ActionResult PostItem()
        {
            return View();
        }
    }
}