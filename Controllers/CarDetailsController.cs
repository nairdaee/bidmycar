using BidMyCar.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        //public ActionResult ItemDetails()

        //{
        //    using (var db = new CarDetailsDbContext())
        //    {
        //        var features = db.CarDetails.ToList();
        //        return View(features);
        //    }
        //    //return View();

        //}
        public ActionResult SearchItems()
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
        

        public ActionResult ItemDetails()
        {
            using (var db = new CarDetailsDbContext())
            {
                //c => c.CarID == id
                var features = db.CarDetails.FirstOrDefault();
                //var featureList = features.Features.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
                // return View(featureList);
                if (features == null)
                {
                    return HttpNotFound();
                }
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