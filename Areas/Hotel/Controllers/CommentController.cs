using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class CommentController : Controller
    {
        // GET: Hotel/Comment
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index()
        {
            return View();
        }
    }
}