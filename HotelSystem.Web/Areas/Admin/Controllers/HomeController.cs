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
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/Home
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var role= (from m in DbContext.UserRole where m.UsersId==SessionInfo.systemUser.Id select m.Menu).ToList();
            ViewBag.User= Session["SystemUser"] as Users;
            return View(role);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            Users u = Session["SystemUser"] as Users;
            return View(u);
        }
    }
}