using System;
using System.Collections.Generic;
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
            return View();
        }
        public ActionResult ProfileSettings()
        {
            return View();
        }
        public ActionResult PostItem()
        {
            return View();
        }

        public ActionResult Dealership()
        {
            return View();
        }

        public ActionResult IndividualSeller()
        {
            return View();
        }
    }
}