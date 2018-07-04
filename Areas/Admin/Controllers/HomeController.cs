using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [Login(Area = "Admin", Role = "system")]
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
            Users u = Session["User"] as Users;
            return View(u);
        }
    }
}