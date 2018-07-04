using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Hotel/Config
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Password()
        {
            return View();
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Subaccount()
        {
            return View();
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Log()
        {
            return View();
        }
    }
}