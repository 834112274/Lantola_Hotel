using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Hotel/Home
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            HotelUsers u = Session["User"] as HotelUsers;
            return View(u);
        }
    }
}