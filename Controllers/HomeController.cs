using BidMyCar.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Web;
using System;
using WebMatrix.WebData;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Net.Mime;
using System.Text;
using System.Diagnostics;

namespace BidMyCar.Controllers
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
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
       
        private static object GetDebuggerDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
    